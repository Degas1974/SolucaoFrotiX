📋 SUPER PROMPT - CONTINUAÇÃO DOCUMENTAÇÃO FROTIX
🎯 CONTEXTO E STATUS ATUAL
✅ Trabalho Concluído
Documentação completa do módulo agendamento (16 arquivos JavaScript):

Lote 191 (Commit: 289a7e8)

ajax-helper.js, evento.service.js, requisitante.service.js, modal-config.js, syncfusion.utils.js
Lote 192 (Commit: b43f9f2)

evento.js, calendario-config.js, kendo-editor-helper.js
Lote 193 (Commit: 1f4811b)

modal-viagem-novo.js (2874 linhas, 28 funções)
recorrencia-logic.js (1395 linhas, 24 funções)
recorrencia.js (527 linhas, 9 métodos)
relatorio.js (1478 linhas, 20 funções)
reportviewer-close-guard.js (248 linhas, 4 funções)
Lote 194 (Commit: 8a0420f)

sweetalert_interop.patch.js (92 linhas com header)
recorrencia-init.js (306 linhas, 6 funções)
main.js (2388 linhas, entry point)
📊 Estatísticas
8 arquivos documentados nesta sessão
9.308 linhas de código documentadas
81 funções/métodos indexados com fluxos detalhados
2.874 linhas de headers abrangentes adicionados
📐 REGRAS DE DOCUMENTAÇÃO (PADRÃO FROTIX)
1. 🎴 CARD DE ARQUIVO (Header Comprehensive)

/* ****************************************************************************************
 * ⚡ ARQUIVO: nome-arquivo.js
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Descrição concisa (2-4 linhas) do propósito do arquivo. Principais
 *                   funcionalidades, número de funções, fluxos principais, tecnologias
 *                   usadas (Syncfusion, jQuery, Bootstrap, etc.). Mencionar patterns
 *                   (IIFE, Singleton, Observer).
 * 📥 ENTRADAS     : Tipos de parâmetros que as funções recebem (strings, Objects,
 *                   Events, DOM elements, etc.). Ser específico sobre estruturas.
 * 📤 SAÍDAS       : Tipos de retorno (Promises, void, Objects, Arrays, boolean). Mencionar
 *                   side effects importantes (DOM updates, state changes, API calls).
 * 🔗 CHAMADA POR  : Arquivos/módulos que chamam este código (main.js, components/*.js,
 *                   eventos Bootstrap Modal, DOMContentLoaded, user clicks).
 * 🔄 CHAMA        : APIs/funções que este código invoca (ApiClient.get/post, Syncfusion
 *                   API, jQuery, Alerta, StateManager, outras funções do projeto).
 * 📦 DEPENDÊNCIAS : Bibliotecas externas (jQuery, Syncfusion EJ2, Bootstrap, Kendo UI,
 *                   moment.js), módulos internos, DOM elements específicos (#elementId).
 * 📝 OBSERVAÇÕES  : Detalhes técnicos importantes (IIFE pattern, global variables,
 *                   try-catch coverage, console.log debug, export pattern, versão,
 *                   documentação externa quando aplicável).
 *
 * 📋 ÍNDICE DE FUNÇÕES (N funções + X exports/variables):
 * [Lista detalhada de todas as funções com boxes ASCII]
 *
 * 🔄 FLUXO TÍPICO 1 - [Nome do fluxo]:
 * [Passo a passo numerado do fluxo principal de uso]
 *
 * 📌 [SEÇÕES ADICIONAIS]:
 * [Arrays de dados, configurações, observações técnicas específicas]
 *
 * 🔌 VERSÃO: X.0
 * 📌 ÚLTIMA ATUALIZAÇÃO: DD/MM/AAAA
 **************************************************************************************** */
Regras do Card de Arquivo:

SEMPRE EM PORTUGUÊS (inclusive emojis contextualizados)
Mínimo 150 linhas para arquivos grandes (>500 linhas)
Máximo 400 linhas (se maior, referenciar doc externa como main.js)
Incluir TODOS os métodos/funções com boxes ASCII
Fluxos típicos detalhados (3-5 fluxos comuns)
Seções 📌 para dados importantes (arrays, configs, estruturas)
2. 🎴 CARD DE FUNÇÃO (Dentro do Índice)

 * ┌─ SEÇÃO NOME (N funções) ────────────────────────────────────────────┐
 * │ N. nomeFuncao(param1, param2)                                         │
 * │    → Descrição breve (1 linha) do que a função faz                   │
 * │    → param param1: tipo (descrição)                                  │
 * │    → param param2: tipo opcional (descrição, default valor)          │
 * │    → returns tipo: descrição do retorno                              │
 * │    → Fluxo: (X linhas de código)                                     │
 * │      1. Passo 1 da execução                                          │
 * │      2. Se condição:                                                  │
 * │         a. Sub-passo indentado                                       │
 * │         b. Sub-passo 2                                               │
 * │      3. Passo final                                                   │
 * │      4. try-catch: Alerta.TratamentoErroComLinha                     │
 * │    → Uso típico: onde/quando esta função é chamada                   │
 * │    → Nota: observações importantes (se aplicável)                    │
 * └──────────────────────────────────────────────────────────────────────┘
Regras dos Cards de Função:

Boxes ASCII com bordas ┌─┐│└┘ (caracteres Unicode)
Indentação de fluxo: números (1, 2, 3), letras (a, b, c), símbolos (-, *, →)
Mencionar try-catch se presente
Incluir "Uso típico" para contextualizar
Parâmetros e retornos tipados (string, Object, Promise<void>, etc.)
3. 💬 COMENTÁRIOS INLINE ROBUSTOS
Padrão Atual (já presente nos arquivos):


// ====================================================================
// SEÇÃO NOME - Descrição breve
// ====================================================================

/**
 * Descrição da função (2-4 linhas)
 * @param {tipo} nomeParam - Descrição
 * @returns {tipo} Descrição do retorno
 */
function minhaFuncao(nomeParam) {
    try {
        // Passo importante do código
        const resultado = operacao();
        
        // Validação crítica
        if (!resultado) {
            console.error("❌ Erro específico");
            return null;
        }
        
        return resultado;
    } catch (error) {
        Alerta.TratamentoErroComLinha("arquivo.js", "minhaFuncao", error);
        throw error;
    }
}
Regras dos Comentários Inline:

JSDoc com @param, @returns, @throws quando aplicável
Seções com // ==== separadores
Comentários explicativos antes de blocos complexos
Emojis em console.log: ✅ (success), ❌ (error), ⚠️ (warning), 🔧 (config), 📊 (data)
4. 🛡️ TRY-CATCH OBRIGATÓRIO
Regra: Todas as funções públicas (window.*) e event handlers DEVEM ter try-catch.


window.minhaFuncaoPublica = function() {
    try {
        // Código da função
        console.log("🚀 Executando minhaFuncaoPublica");
        
        // Lógica...
        
    } catch (error) {
        Alerta.TratamentoErroComLinha("arquivo.js", "minhaFuncaoPublica", error);
        // throw error; (opcional - apenas se deve propagar)
    }
};
Exceções (sem try-catch):

Funções internas privadas simples (<10 linhas)
Getters/setters triviais
Funções que apenas retornam valores constantes
5. 🔔 TOASTS SYNCFUSION (SweetAlert2)
SEMPRE usar Swal.fire() para feedback visual:


// ✅ SUCCESS
Swal.fire({
    icon: 'success',
    title: 'Sucesso!',
    text: 'Operação concluída com sucesso',
    timer: 2000,
    showConfirmButton: false
});

// ❌ ERROR
Swal.fire({
    icon: 'error',
    title: 'Erro!',
    text: error.message || 'Erro ao processar',
    confirmButtonText: 'OK'
});

// ⚠️ WARNING
Swal.fire({
    icon: 'warning',
    title: 'Atenção',
    text: 'Esta ação não pode ser desfeita',
    showCancelButton: true,
    confirmButtonText: 'Continuar',
    cancelButtonText: 'Cancelar'
});

// ℹ️ INFO
Swal.fire({
    icon: 'info',
    title: 'Informação',
    text: 'Processamento em andamento...',
    timer: 3000
});
NUNCA usar:

alert() nativo
console.log() como feedback para usuário (apenas debug)
Toasts customizados sem padronização
🔄 WORKFLOW DE DOCUMENTAÇÃO
Processo Passo a Passo
Identificar Arquivos (Glob pattern ou lista manual)

find wwwroot/js/[modulo] -name "*.js" | sort
Ler Arquivo Completo (ou por partes se >2000 linhas)

Read({ file_path: "caminho/arquivo.js" })
Grep Funções (identificar todas as funções)

Grep({ 
    pattern: "^(window\\.|function\\s+\\w+|const\\s+\\w+\\s*=\\s*function)",
    path: "caminho/arquivo.js",
    output_mode: "content"
})
Contar Linhas (para estatísticas)

wc -l "caminho/arquivo.js"
Criar Header Comprehensive (seguindo template acima)

Mínimo 150 linhas para arquivos grandes
Incluir TODOS os cards de função
Adicionar fluxos típicos (2-4 fluxos)
Seções 📌 com dados importantes
Substituir Header Antigo (Edit tool)


Edit({
    file_path: "caminho/arquivo.js",
    old_string: "// Header antigo simples...",
    new_string: "/* Header comprehensive novo... */"
})
Atualizar TodoList (marcar progresso)

TodoWrite({
    todos: [
        { content: "Arquivo 1", status: "completed", activeForm: "..." },
        { content: "Arquivo 2", status: "in_progress", activeForm: "..." }
    ]
})
Organização em Lotes
SEMPRE trabalhar em lotes de 5 arquivos:

Lote N: 5 arquivos (ou 3-4 se arquivos grandes >1500 linhas)
Commit após cada lote completo
Push após cada commit
Todo list atualizado a cada arquivo
📝 PADRÃO DE COMMITS
Mensagem de Commit (seguir EXATAMENTE este formato):

git commit -m "$(cat <<'EOF'
docs: Lote [N] - [Módulo/categoria] revisão cards completos ([X] arquivos)

Adiciona cabeçalhos abrangentes em português para [X] arquivos JavaScript
do módulo [nome módulo]:
- arquivo1.js ([linhas] linhas, [N] funções): [Descrição concisa 1 linha]
- arquivo2.js ([linhas] linhas, [N] funções): [Descrição concisa 1 linha]
- arquivo3.js ([linhas] linhas, [N] funções): [Descrição concisa 1 linha]
- arquivo4.js ([linhas] linhas, [N] funções): [Descrição concisa 1 linha]
- arquivo5.js ([linhas] linhas, [N] funções): [Descrição concisa 1 linha]

Cada cabeçalho documenta: objetivo, entradas, saídas, chamadores, dependências,
índice de funções (com fluxos detalhados), fluxos típicos, observações técnicas.

[Observação específica do lote, se aplicável]

Co-Authored-By: Claude Sonnet 4.5 <noreply@anthropic.com>
EOF
)"
Frequência de Commit/Push
OBRIGATÓRIO:

✅ Commit após cada 5 arquivos (1 lote)
✅ Push imediatamente após cada commit
✅ Mensagem descritiva (formato acima)
✅ Co-Authored-By: Claude Sonnet 4.5
Exemplo de workflow:


# Documentar arquivos 1-5
git add arquivo1.js arquivo2.js arquivo3.js arquivo4.js arquivo5.js
git commit -m "docs: Lote 195 - ..."
git push

# Documentar arquivos 6-10
git add arquivo6.js arquivo7.js arquivo8.js arquivo9.js arquivo10.js
git commit -m "docs: Lote 196 - ..."
git push
📊 FEEDBACK VISUAL (Barra de Progresso)
Formato de Progresso
Ao iniciar cada lote:


🚀 Iniciando Lote [N] - [Categoria]
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
Arquivos: [1/5] ⬜⬜⬜⬜⬜ 0%
Durante o progresso:


Lote [N] - [Categoria]
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
Arquivos: [3/5] ✅✅✅⬜⬜ 60%

✅ arquivo1.js (150 linhas header)
✅ arquivo2.js (200 linhas header)
✅ arquivo3.js (180 linhas header)
⏳ arquivo4.js (em progresso...)
⬜ arquivo5.js (pendente)
Ao finalizar lote:


✅ Lote [N] Concluído!
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
Arquivos: [5/5] ✅✅✅✅✅ 100%

Estatísticas:
- 5 arquivos documentados
- 2.450 linhas de código
- 43 funções indexadas
- 920 linhas de headers

📦 Commit: abc1234
✈️ Push: origin/main
🎯 PRÓXIMOS PASSOS (Após Lote 194)
Módulos Restantes para Documentar
1. Módulo Models (FrotiX.Site/Models/) - 15-20 arquivos C#

ViewModels/*.cs
DTOs/*.cs
Entities/*.cs
2. Módulo Controllers (FrotiX.Site/Controllers/) - 8-12 arquivos C#

ViagemController.cs
AgendamentoController.cs
RelatorioController.cs
etc.
3. Módulo Services (FrotiX.Site/Services/) - 10-15 arquivos C#

LogService.cs
ClaudeAnalysisService.cs
ViagemService.cs
etc.
4. Módulo wwwroot/js (outros módulos) - 20-30 arquivos JS

Módulos não-agendamento
Helpers globais
Plugins customizados
Ordem Sugerida
Finalizar JavaScript (wwwroot/js restante) - 20-30 arquivos
Services C# (mais críticos) - 10-15 arquivos
Controllers C# - 8-12 arquivos
Models C# - 15-20 arquivos
Razor Pages (se necessário) - quantidade variável
🔧 COMANDOS ÚTEIS
Buscar Arquivos JS sem Header Comprehensive

# Buscar arquivos JS sem header comprehensive (regex)
grep -L "⚡ ARQUIVO:" wwwroot/js/**/*.js

# Contar arquivos JS totais
find wwwroot/js -name "*.js" | wc -l

# Contar arquivos JS com header comprehensive
grep -l "⚡ ARQUIVO:" wwwroot/js/**/*.js | wc -l
Git Status e Estatísticas

# Ver arquivos modificados
git status

# Ver últimos 5 commits
git log --oneline -5

# Ver estatísticas de commit
git diff --stat HEAD~1

# Ver quem documentou arquivos
git log --all --grep="docs: Lote" --oneline
Analisar Arquivo JS

# Contar linhas
wc -l arquivo.js

# Contar funções (aproximado)
grep -c "function" arquivo.js

# Ver estrutura de funções
grep -n "function\|class\|window\." arquivo.js
⚡ REGRAS ESPECIAIS
1. Arquivos Grandes (>1500 linhas)
Ler por partes (offset + limit no Read tool)
Header pode ter até 400 linhas
Referenciar doc externa se contexto muito complexo (como main.js)
Focar em fluxos principais (não documentar CADA linha)
2. Arquivos Minificados
Não adicionar header detalhado (apenas comentário header curto)
Exemplo: sweetalert_interop.patch.js (1 linha minificada)
Header explica código expandido em comentários
3. Arquivos com Documentação Externa
Main.js padrão: Referenciar Documentacao/Pages/*.md
Header high-level apenas (150-250 linhas)
Box ASCII especial:

 * ╔══════════════════════════════════════════════════════════════════╗
 * ║  📄 Documentacao/Pages/Arquivo.md                                 ║
 * ║  Última atualização: DD/MM/AAAA                                   ║
 * ╚══════════════════════════════════════════════════════════════════╝
4. Arquivos IIFE
Mencionar IIFE pattern no OBSERVAÇÕES
Documentar funções internas mesmo que privadas
Indicar exports (window.* variables)
Exemplo: reportviewer-close-guard.js, relatorio.js
5. Classes ES6
Constructor documentado separadamente
Métodos listados em ordem lógica (não alfabética)
Singleton pattern: mencionar instância global
Exemplo: GerenciadorRecorrencia, StateManager
📋 CHECKLIST FINAL (Antes de Commit)
Antes de commitar cada lote, verificar:

 Headers completos (mínimo 150 linhas para arquivos grandes)
 Todos os métodos documentados (cards ASCII)
 Fluxos típicos incluídos (2-4 fluxos)
 Seções 📌 com dados importantes
 Emojis contextualizados (⚡🎯📥📤🔗🔄📦📝📋🔄📌🔌)
 Português correto (sem erros ortográficos)
 Todo list atualizado
 Git add correto (apenas arquivos do lote)
 Mensagem de commit formatada (seguir template)
 Push executado (verificar sucesso)
🎓 EXEMPLOS DE REFERÊNCIA
Arquivos Bem Documentados (Usar como Modelo)
modal-viagem-novo.js (2874 linhas, 28 funções)

Header de 400+ linhas
Documentação completa de ciclo de vida modal
5 fluxos típicos detalhados
Seções com estruturas de objetos
recorrencia.js (527 linhas, 9 métodos classe)

Documentação de classe ES6
Singleton pattern explicado
Fluxos com exemplos práticos
Arrays de dados documentados
reportviewer-close-guard.js (248 linhas, 4 funções IIFE)

IIFE bem documentado
Flags globais explicadas
Callbacks wrapeados documentados
Fluxo de bloqueio/desbloqueio detalhado
main.js (2388 linhas, entry point)

Referência a documentação externa
Header high-level
12 módulos coordenados listados
Ordem de carregamento crítica documentada
💾 ONDE PAROU
Status: ✅ MÓDULO AGENDAMENTO COMPLETO
Último commit: 8a0420f - "docs: Lote 194 - Finalização módulo agendamento (3 arquivos)"

Arquivos documentados na sessão:

Lote 193: 5 arquivos (6522 linhas totais)
Lote 194: 3 arquivos (2786 linhas totais)
Total sessão: 8 arquivos, 9.308 linhas, 81 funções

Próximo lote sugerido: Lote 195