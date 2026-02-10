// ╔══════════════════════════════════════════════════════════════════╗
// ║  FrotiX Conversa Manager - Extensão VS Code                    ║
// ║  Gerencia registro de conversas com IAs no projeto FrotiX      ║
// ║  Versão: 1.0.0 | Data: 10/02/2026                              ║
// ╚══════════════════════════════════════════════════════════════════╝

const vscode = require('vscode');
const fs = require('fs');
const path = require('path');

// ═══════════════════════════════════════════════════════════════════
// CONSTANTES E CONFIGURAÇÃO
// ═══════════════════════════════════════════════════════════════════

const IA_LABELS = {
    'Claude': 'Claude Code',
    'Copilot': 'GitHub Copilot',
    'Continue': 'Continue',
    'Gemini': 'Gemini Code Assist',
    'Genie': 'Genie AI',
    'Chat': 'GitHub Copilot'
};

const IA_TAB_PATTERNS = [
    { pattern: /claude/i, ia: 'Claude Code' },
    { pattern: /copilot/i, ia: 'GitHub Copilot' },
    { pattern: /continue/i, ia: 'Continue' },
    { pattern: /gemini/i, ia: 'Gemini Code Assist' },
    { pattern: /genie/i, ia: 'Genie AI' },
    { pattern: /chat/i, ia: 'GitHub Copilot' }
];

// Template de finalização que será copiado para o clipboard
const FINALIZACAO_PROMPT = `Finalize esta conversa agora. Gere o resumo final COMPLETO do arquivo de registro seguindo EXATAMENTE o template da Seção 5.3.3 do RegrasDesenvolvimentoFrotiX.md.

O resumo DEVE incluir TODAS estas seções com tabelas visuais e ícones:

1. **Resumo Executivo** (2-3 parágrafos)
2. **⏱️ Informações da Sessão** (tabela com Início, Término, Duração, IA, Continuação)
3. **📁 Arquivos Alterados** (tabela com ícones ➕✏️🗑️, caminho e motivo)
4. **🐛 Problemas e Soluções** (tabela com #, Problema, Causa, Solução, Lição)
5. **🔧 Decisões Técnicas** (tabelas por decisão)
6. **📋 Tarefas Pendentes** (checkboxes)
7. **🔄 Continuidade** (tabela com Próximos Passos, Contexto, Arquivos-Chave, Riscos)
8. **✅ Status Final** (tabela de objetivos + estatísticas)

ATUALIZE o arquivo de conversa em Conversas/ com este resumo completo. Marque a conversa como FINALIZADA.`;

// ═══════════════════════════════════════════════════════════════════
// ESTADO GLOBAL
// ═══════════════════════════════════════════════════════════════════

let statusBarItem;
let conversaAtual = null; // { nome, ia, arquivo, inicio, projeto }
let detectedTabs = new Set();

// ═══════════════════════════════════════════════════════════════════
// FUNÇÕES UTILITÁRIAS
// ═══════════════════════════════════════════════════════════════════

/**
 * Formata data no padrão do projeto: [YYYY.MM.DD]-[HH.mm]
 */
function formatarDataHora(date) {
    const y = date.getFullYear();
    const m = String(date.getMonth() + 1).padStart(2, '0');
    const d = String(date.getDate()).padStart(2, '0');
    const h = String(date.getHours()).padStart(2, '0');
    const min = String(date.getMinutes()).padStart(2, '0');
    return `[${y}.${m}.${d}]-[${h}.${min}]`;
}

/**
 * Formata data como timestamp legível: YYYY-MM-DD HH:mm:ss
 */
function formatarTimestamp(date) {
    const y = date.getFullYear();
    const m = String(date.getMonth() + 1).padStart(2, '0');
    const d = String(date.getDate()).padStart(2, '0');
    const h = String(date.getHours()).padStart(2, '0');
    const min = String(date.getMinutes()).padStart(2, '0');
    const s = String(date.getSeconds()).padStart(2, '0');
    return `${y}-${m}-${d} ${h}:${min}:${s}`;
}

/**
 * Obtém a pasta Conversas/ do workspace atual
 */
function getConversasPasta() {
    const config = vscode.workspace.getConfiguration('frotix');
    const nomePasta = config.get('conversasPasta', 'Conversas');

    const workspaceFolders = vscode.workspace.workspaceFolders;
    if (!workspaceFolders || workspaceFolders.length === 0) {
        return null;
    }

    // Procurar Conversas/ nos projetos conhecidos do FrotiX
    const projetosPrioritarios = [
        'FrotiX.Site.OLD',
        'FrotiX.Site.Fevereiro'
    ];

    const rootPath = workspaceFolders[0].uri.fsPath;

    // Primeiro: verificar nos projetos prioritários
    for (const projeto of projetosPrioritarios) {
        const pastaConversa = path.join(rootPath, projeto, nomePasta);
        if (fs.existsSync(pastaConversa)) {
            return { pasta: pastaConversa, projeto };
        }
    }

    // Segundo: verificar na raiz do workspace
    const pastaRaiz = path.join(rootPath, nomePasta);
    if (fs.existsSync(pastaRaiz)) {
        return { pasta: pastaRaiz, projeto: 'Raiz' };
    }

    // Terceiro: criar no primeiro projeto prioritário que existir
    for (const projeto of projetosPrioritarios) {
        const pastaProjeto = path.join(rootPath, projeto);
        if (fs.existsSync(pastaProjeto)) {
            const pastaConversa = path.join(pastaProjeto, nomePasta);
            fs.mkdirSync(pastaConversa, { recursive: true });
            return { pasta: pastaConversa, projeto };
        }
    }

    // Fallback: criar na raiz
    fs.mkdirSync(pastaRaiz, { recursive: true });
    return { pasta: pastaRaiz, projeto: 'Raiz' };
}

/**
 * Lista as últimas N conversas registradas (por data de modificação)
 */
function listarConversasRecentes(maxItems) {
    const info = getConversasPasta();
    if (!info) return [];

    try {
        const arquivos = fs.readdirSync(info.pasta)
            .filter(f => f.endsWith('.md') && f.startsWith('['))
            .map(f => {
                const stat = fs.statSync(path.join(info.pasta, f));
                return { nome: f, mtime: stat.mtime, pasta: info.pasta };
            })
            .sort((a, b) => b.mtime - a.mtime)
            .slice(0, maxItems);

        return arquivos;
    } catch {
        return [];
    }
}

/**
 * Detecta qual IA está associada a um tab
 */
function detectarIA(tabLabel) {
    if (!tabLabel) return null;
    for (const { pattern, ia } of IA_TAB_PATTERNS) {
        if (pattern.test(tabLabel)) return ia;
    }
    return null;
}

/**
 * Gera o conteúdo inicial do arquivo de conversa
 */
function gerarConteudoInicial(nome, ia, inicio, continuacaoDe) {
    return `# ${nome}

## Resumo Executivo

[Atualizar incrementalmente durante a sessão]

---

## ⏱️ Informações da Sessão

| ⏱️ Tempo | 📋 Detalhes |
|----------|-------------|
| **Início** | ${formatarTimestamp(inicio)} |
| **Término** | (em andamento) |
| **Duração** | (em andamento) |
| **IA** | ${ia} |
| **Continuação de** | ${continuacaoDe} |

---

## 📁 Arquivos Alterados

| Ação | Arquivo | Motivo |
|------|---------|--------|
| | | (atualizar incrementalmente) |

**Legenda:** ➕ Criado | ✏️ Modificado | 🗑️ Removido

---

## 🐛 Problemas Encontrados e Soluções

| # | Problema | Causa Raiz | Solução | Lição Aprendida |
|---|---------|------------|---------|-----------------|
| | | | | (atualizar incrementalmente) |

---

## 🔧 Decisões Técnicas

(atualizar incrementalmente)

---

## 📋 Tarefas Pendentes

- [ ] (atualizar incrementalmente)

---

## 🔄 Continuidade

| Item | Detalhe |
|------|---------|
| **Próximos Passos** | (preencher ao finalizar) |
| **Contexto Necessário** | (preencher ao finalizar) |
| **Arquivos-Chave** | (preencher ao finalizar) |
| **Riscos/Alertas** | (preencher ao finalizar) |

---

## ✅ Status Final

⏳ **CONVERSA EM ANDAMENTO**

---

*Conversa iniciada em: ${formatarTimestamp(inicio)}*
`;
}

// ═══════════════════════════════════════════════════════════════════
// STATUS BAR
// ═══════════════════════════════════════════════════════════════════

function atualizarStatusBar() {
    if (!statusBarItem) return;

    if (conversaAtual) {
        const nomeResumido = conversaAtual.nome.length > 30
            ? conversaAtual.nome.substring(0, 27) + '...'
            : conversaAtual.nome;
        statusBarItem.text = `$(comment-discussion) ${nomeResumido}`;
        statusBarItem.tooltip = `FrotiX Conversa: ${conversaAtual.nome}\nIA: ${conversaAtual.ia}\nInício: ${formatarTimestamp(conversaAtual.inicio)}\n\nClique para checkpoint (Ctrl+Shift+S)`;
        statusBarItem.command = 'frotix.checkpoint';
        statusBarItem.backgroundColor = new vscode.ThemeColor('statusBarItem.warningBackground');
    } else {
        statusBarItem.text = '$(comment-discussion) FrotiX';
        statusBarItem.tooltip = 'FrotiX Conversa Manager\nClique para iniciar nova conversa (Ctrl+Shift+C)';
        statusBarItem.command = 'frotix.novaConversa';
        statusBarItem.backgroundColor = undefined;
    }

    statusBarItem.show();
}

// ═══════════════════════════════════════════════════════════════════
// COMANDOS PRINCIPAIS
// ═══════════════════════════════════════════════════════════════════

/**
 * Ctrl+Shift+C - Nova Conversa (ou Quick Pick se detectado automaticamente)
 */
async function novaConversa(iaDetectada) {
    const config = vscode.workspace.getConfiguration('frotix');
    const maxRecentes = config.get('maxConversasRecentes', 20);

    // Quick Pick: Nova ou Continuar?
    const recentes = listarConversasRecentes(maxRecentes);

    const opcoes = [
        {
            label: '$(add) Nova Conversa',
            description: 'Iniciar registro de uma nova conversa',
            action: 'nova'
        },
        {
            label: '$(history) Continuar Conversa Anterior',
            description: `Últimas ${recentes.length} conversas registradas`,
            action: 'continuar'
        },
        {
            label: '$(close) Ignorar',
            description: 'Não registrar esta conversa',
            action: 'ignorar'
        }
    ];

    const escolha = await vscode.window.showQuickPick(opcoes, {
        placeHolder: iaDetectada
            ? `Chat de ${iaDetectada} detectado - Deseja registrar esta conversa?`
            : 'FrotiX Conversa Manager - O que deseja fazer?',
        title: 'FrotiX Conversa Manager'
    });

    if (!escolha || escolha.action === 'ignorar') return;

    if (escolha.action === 'continuar') {
        await continuarConversa(recentes, iaDetectada);
        return;
    }

    // === NOVA CONVERSA ===
    // Pedir nome da conversa
    const nome = await vscode.window.showInputBox({
        prompt: 'Nome/objetivo da conversa',
        placeHolder: 'Ex: Implementar sistema de login',
        validateInput: (value) => {
            if (!value || value.trim().length === 0) return 'Nome é obrigatório';
            if (/[\\/:*?"<>|]/.test(value)) return 'Caracteres inválidos: \\ / : * ? " < > |';
            return null;
        }
    });

    if (!nome) return;

    // Selecionar IA (se não detectada automaticamente)
    let ia = iaDetectada;
    if (!ia) {
        const iasDisponiveis = [
            { label: 'Claude Code', description: 'Anthropic Claude' },
            { label: 'GitHub Copilot', description: 'Microsoft/OpenAI' },
            { label: 'Continue', description: 'Continue Dev' },
            { label: 'Gemini Code Assist', description: 'Google' },
            { label: 'Genie AI', description: 'OpenAI/Custom' }
        ];

        const iaEscolhida = await vscode.window.showQuickPick(iasDisponiveis, {
            placeHolder: 'Qual IA está sendo utilizada?',
            title: 'Selecionar IA'
        });

        if (!iaEscolhida) return;
        ia = iaEscolhida.label;
    }

    // Criar arquivo
    const agora = new Date();
    const dataFormatada = formatarDataHora(agora);
    const nomeArquivo = `${dataFormatada} - [${nome}] - [${ia}].md`;

    const info = getConversasPasta();
    if (!info) {
        vscode.window.showErrorMessage('FrotiX: Nenhum workspace aberto. Abra um projeto primeiro.');
        return;
    }

    const caminhoArquivo = path.join(info.pasta, nomeArquivo);
    const conteudo = gerarConteudoInicial(nome, ia, agora, 'Conversa nova');

    fs.writeFileSync(caminhoArquivo, conteudo, 'utf8');

    // Definir conversa atual
    conversaAtual = {
        nome,
        ia,
        arquivo: caminhoArquivo,
        inicio: agora,
        projeto: info.projeto
    };

    atualizarStatusBar();

    vscode.window.showInformationMessage(
        `FrotiX: Conversa "${nome}" iniciada! Arquivo criado em ${info.projeto}/Conversas/`
    );

    // Abrir o arquivo criado
    const doc = await vscode.workspace.openTextDocument(caminhoArquivo);
    await vscode.window.showTextDocument(doc, { preview: false, viewColumn: vscode.ViewColumn.Beside });
}

/**
 * Quick Pick para continuar conversa anterior
 */
async function continuarConversa(recentes, iaDetectada) {
    if (recentes.length === 0) {
        vscode.window.showInformationMessage('FrotiX: Nenhuma conversa anterior encontrada.');
        await novaConversa(iaDetectada);
        return;
    }

    const items = recentes.map(r => {
        // Extrair nome legível do arquivo
        // Formato: [YYYY.MM.DD]-[HH.mm] - [Nome] - [IA].md
        const match = r.nome.match(/\[[\d.]+\]-\[[\d.]+\] - \[(.+?)\] - \[(.+?)\]\.md/);
        const nomeLegivel = match ? match[1] : r.nome;
        const iaOriginal = match ? match[2] : '?';

        return {
            label: `$(file) ${nomeLegivel}`,
            description: iaOriginal,
            detail: `Última modificação: ${r.mtime.toLocaleString('pt-BR')}`,
            arquivo: path.join(r.pasta, r.nome),
            nomeOriginal: nomeLegivel,
            iaOriginal
        };
    });

    const escolha = await vscode.window.showQuickPick(items, {
        placeHolder: 'Selecione a conversa para continuar',
        title: 'Continuar Conversa Anterior',
        matchOnDescription: true,
        matchOnDetail: true
    });

    if (!escolha) return;

    // Definir como conversa atual
    conversaAtual = {
        nome: escolha.nomeOriginal,
        ia: iaDetectada || escolha.iaOriginal,
        arquivo: escolha.arquivo,
        inicio: new Date(),
        projeto: '(continuação)'
    };

    atualizarStatusBar();

    vscode.window.showInformationMessage(
        `FrotiX: Continuando conversa "${escolha.nomeOriginal}"`
    );

    // Abrir o arquivo
    const doc = await vscode.workspace.openTextDocument(escolha.arquivo);
    await vscode.window.showTextDocument(doc, { preview: false, viewColumn: vscode.ViewColumn.Beside });
}

/**
 * Ctrl+Shift+S - Checkpoint (salvar progresso)
 */
async function checkpoint() {
    if (!conversaAtual) {
        const resposta = await vscode.window.showWarningMessage(
            'FrotiX: Nenhuma conversa ativa. Deseja iniciar uma nova?',
            'Sim', 'Não'
        );
        if (resposta === 'Sim') await novaConversa();
        return;
    }

    // Copiar prompt de checkpoint para o clipboard
    const checkpointPrompt = `Atualize o arquivo de registro da conversa em Conversas/ (${path.basename(conversaAtual.arquivo)}).

Atualize INCREMENTALMENTE as seguintes seções:
- 📁 Arquivos Alterados (adicione novos arquivos criados/modificados/removidos)
- 🐛 Problemas Encontrados (adicione novos problemas se houver)
- 🔧 Decisões Técnicas (adicione novas decisões se houver)
- 📋 Tarefas Pendentes (atualize status das tarefas)

NÃO finalize a conversa. Mantenha status "⏳ EM ANDAMENTO".`;

    await vscode.env.clipboard.writeText(checkpointPrompt);

    vscode.window.showInformationMessage(
        `FrotiX: Prompt de checkpoint copiado para o clipboard! Cole (Ctrl+V) no chat da ${conversaAtual.ia}.`
    );
}

/**
 * Ctrl+Shift+F - Finalizar Conversa
 */
async function finalizarConversa() {
    if (!conversaAtual) {
        vscode.window.showWarningMessage('FrotiX: Nenhuma conversa ativa para finalizar.');
        return;
    }

    const confirma = await vscode.window.showWarningMessage(
        `FrotiX: Finalizar conversa "${conversaAtual.nome}"?`,
        'Sim, Finalizar', 'Cancelar'
    );

    if (confirma !== 'Sim, Finalizar') return;

    // Copiar prompt de finalização para o clipboard
    await vscode.env.clipboard.writeText(FINALIZACAO_PROMPT);

    vscode.window.showInformationMessage(
        `FrotiX: Prompt de finalização copiado para o clipboard! Cole (Ctrl+V) no chat da ${conversaAtual.ia} para gerar o resumo final completo.`
    );

    // Limpar conversa atual (após um delay para dar tempo de copiar)
    const conversaFinalizada = { ...conversaAtual };
    conversaAtual = null;
    atualizarStatusBar();

    // Mostrar notificação com link para o arquivo
    const abrirArquivo = await vscode.window.showInformationMessage(
        `FrotiX: Conversa "${conversaFinalizada.nome}" finalizada! Não esqueça de colar o prompt no chat.`,
        'Abrir Arquivo'
    );

    if (abrirArquivo === 'Abrir Arquivo') {
        const doc = await vscode.workspace.openTextDocument(conversaFinalizada.arquivo);
        await vscode.window.showTextDocument(doc, { preview: false });
    }
}

// ═══════════════════════════════════════════════════════════════════
// DETECÇÃO AUTOMÁTICA DE TABS DE IA
// ═══════════════════════════════════════════════════════════════════

function configurarDeteccaoAutomatica(context) {
    const config = vscode.workspace.getConfiguration('frotix');
    if (!config.get('deteccaoAutomatica', true)) return;

    // Monitorar mudanças de tabs
    const disposable = vscode.window.tabGroups.onDidChangeTabs(async (event) => {
        // Verificar tabs abertas
        for (const tab of event.opened) {
            const label = tab.label;
            if (!label) continue;

            const ia = detectarIA(label);
            if (!ia) continue;

            // Evitar detectar a mesma tab múltiplas vezes
            const tabKey = `${label}-${tab.group.viewColumn}`;
            if (detectedTabs.has(tabKey)) continue;
            detectedTabs.add(tabKey);

            // Se não há conversa ativa, perguntar se quer registrar
            if (!conversaAtual) {
                // Pequeno delay para não atrapalhar abertura da tab
                setTimeout(() => {
                    novaConversa(ia);
                }, 500);
            }
        }

        // Limpar tabs fechadas do cache
        for (const tab of event.closed) {
            const label = tab.label;
            if (label) {
                const tabKey = `${label}-${tab.group.viewColumn}`;
                detectedTabs.delete(tabKey);
            }
        }
    });

    context.subscriptions.push(disposable);
}

// ═══════════════════════════════════════════════════════════════════
// ATIVAÇÃO E DESATIVAÇÃO DA EXTENSÃO
// ═══════════════════════════════════════════════════════════════════

/**
 * @param {vscode.ExtensionContext} context
 */
function activate(context) {
    console.log('FrotiX Conversa Manager ativado!');

    // Criar Status Bar Item
    statusBarItem = vscode.window.createStatusBarItem(
        vscode.StatusBarAlignment.Left,
        100
    );
    context.subscriptions.push(statusBarItem);
    atualizarStatusBar();

    // Registrar comandos
    context.subscriptions.push(
        vscode.commands.registerCommand('frotix.novaConversa', () => novaConversa()),
        vscode.commands.registerCommand('frotix.checkpoint', () => checkpoint()),
        vscode.commands.registerCommand('frotix.finalizarConversa', () => finalizarConversa())
    );

    // Configurar detecção automática
    configurarDeteccaoAutomatica(context);
}

function deactivate() {
    console.log('FrotiX Conversa Manager desativado.');
}

module.exports = {
    activate,
    deactivate
};
