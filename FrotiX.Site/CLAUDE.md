# 🤖 CLAUDE.md – Configuração Claude Code

> **ATENÇÃO CLAUDE:** Este arquivo é carregado automaticamente no início de cada sessão.
> **Versão:** 2.0
> **Última Atualização:** 18/01/2026

---

## 🚨 PROTOCOLO DE INICIALIZAÇÃO (LEIA PRIMEIRO)

### ✅ AÇÕES OBRIGATÓRIAS AO INICIAR SESSÃO

Antes de qualquer resposta ao usuário, você DEVE:

1. ✅ **Ler completamente** o arquivo `RegrasDesenvolvimentoFrotiX.md`
2. ✅ **Se a tarefa envolver banco de dados:** Ler `FrotiX.sql`
3. ✅ **Confirmar mentalmente** que ambos arquivos foram lidos

**IMPORTANTE:** Não prossiga sem ler `RegrasDesenvolvimentoFrotiX.md`. Ele contém TODAS as regras do projeto.

---

## 📋 HIERARQUIA DE ARQUIVOS DE REGRAS

| Arquivo | Função | Quando Ler |
|---------|--------|------------|
| **`RegrasDesenvolvimentoFrotiX.md`** | ⭐ FONTE OFICIAL DE REGRAS | **SEMPRE** (obrigatório) |
| **`CLAUDE.md`** | Índice e instruções de inicialização | Automático (você está aqui) |
| **`FrotiX.sql`** | Estrutura do banco de dados | Quando trabalhar com dados |
| **`.claude/CLAUDE.md`** | Diretrizes de documentação | Quando documentar |

---

## 🧠 SISTEMA DE MEMÓRIA PERMANENTE

### 📝 REGRA: Como Memorizar Regras Permanentemente

**GATILHOS** - Quando o usuário disser:
- "memorize"
- "guarde na memória"
- "lembre-se disso"
- "adicione às regras"
- "isso é uma regra nova"
- "nunca esqueça"
- "sempre faça X"
- "de agora em diante"

**AÇÃO OBRIGATÓRIA:**

```
1. Abrir: RegrasDesenvolvimentoFrotiX.md
2. Identificar seção apropriada ou criar nova
3. Adicionar regra seguindo formato padrão (ver abaixo)
4. Salvar arquivo
5. Commitar com mensagem: "docs: Adiciona regra permanente - [Nome da Regra]"
6. Confirmar ao usuário que regra foi memorizada
```

### 📐 FORMATO PADRÃO PARA NOVAS REGRAS

```markdown
### [NÚMERO_SEÇÃO].[NÚMERO_SUBSEÇÃO] [Nome da Regra]

**Contexto:** [Por que esta regra existe]

**Regra:** [O que deve ser feito/evitado - IMPERATIVO]

**Exemplo:**
\`\`\`[linguagem]
// Código de exemplo mostrando aplicação da regra
\`\`\`

**Data de Adição:** DD/MM/AAAA
```

### ✅ EXEMPLO PRÁTICO

**Usuário diz:**
> "Memorize: sempre usar `await` em todas as chamadas assíncronas"

**Você faz:**

1. Abrir `RegrasDesenvolvimentoFrotiX.md`
2. Localizar seção `4. PADRÕES DE CÓDIGO`
3. Adicionar:

```markdown
### 4.3 Async/Await Obrigatório

**Contexto:** Para evitar callbacks aninhados e garantir tratamento correto de erros assíncronos.

**Regra:** SEMPRE usar `await` em chamadas assíncronas. NUNCA usar `.then()` sem `await`.

**Exemplo:**
\`\`\`javascript
// ✅ CORRETO
async function buscarDados() {
    try {
        const dados = await fetch('/api/dados');
        return await dados.json();
    } catch (erro) {
        Alerta.TratamentoErroComLinha("arquivo.js", "buscarDados", erro);
    }
}

// ❌ ERRADO
function buscarDados() {
    fetch('/api/dados').then(d => d.json());
}
\`\`\`

**Data de Adição:** 18/01/2026
```

4. Salvar
5. Commitar: `git add RegrasDesenvolvimentoFrotiX.md && git commit -m "docs: Adiciona regra permanente - Async/Await Obrigatório" && git push`
6. Responder: "✅ Regra memorizada permanentemente em `RegrasDesenvolvimentoFrotiX.md` (seção 4.3)"

---

## ⚠️ REGRAS CRÍTICAS (RESUMO RÁPIDO)

### 🗄️ Banco de Dados
- **SEMPRE** consultar `FrotiX.sql` ANTES de codificar operações com banco
- Nunca assumir nome de coluna "de cabeça"
- Verificar tipos de dados, nullable, FKs

### 🔒 Try-Catch
- **OBRIGATÓRIO** em TODAS as funções (C# e JS)
- Usar `Alerta.TratamentoErroComLinha(arquivo, metodo, erro)`

### 🎨 UI/UX
- **Alertas:** SEMPRE usar `Alerta.*` (SweetAlert), NUNCA `alert()`
- **Ícones:** SEMPRE `fa-duotone`, NUNCA `fa-solid/regular/light`
- **Loading:** SEMPRE `FtxSpin.show()`, NUNCA spinner Bootstrap
- **Tooltips:** SEMPRE Syncfusion `data-ejtip`, NUNCA Bootstrap

### 📝 Documentação
- **SEMPRE** atualizar documentação em `Documentacao/` antes de commitar
- Formato: Prosa técnica + snippets + explicação linha-a-linha

### 🔄 Git
- Branch preferencial: `main`
- **Push SEMPRE para `main`** (nunca outras branches sem autorização)
- **Commit e push automáticos IMEDIATOS** após criar/alterar código importante
- **Commit e push automáticos** após fornecer código durante conversa
- Atualizar documentação ANTES do push
- Tipos de commit: `feat:`, `fix:`, `refactor:`, `docs:`, `style:`, `chore:`

---

## 📚 REFERÊNCIA RÁPIDA

| Arquivo | Descrição |
|---------|-----------|
| `RegrasDesenvolvimentoFrotiX.md` | ⭐ REGRAS CONSOLIDADAS (fonte oficial) |
| `FrotiX.sql` | Estrutura do banco de dados |
| `wwwroot/js/alerta.js` | Sistema de alertas SweetAlert |
| `wwwroot/js/frotix.js` | JS global (FtxSpin) |
| `wwwroot/css/frotix.css` | CSS global |

---

## 🎯 CHECKLIST DE INÍCIO DE SESSÃO

Antes de responder ao usuário, confirme mentalmente:

- [ ] Li `RegrasDesenvolvimentoFrotiX.md` completamente?
- [ ] Se envolver banco: li `FrotiX.sql`?
- [ ] Entendi a hierarquia de arquivos?
- [ ] Sei como memorizar regras permanentemente?
- [ ] Conheço as regras críticas (try-catch, alertas, ícones)?

---

## 🔄 VERSIONAMENTO

| Versão | Data | Mudanças |
|--------|------|----------|
| 2.0 | 18/01/2026 | Reformulação completa: protocolo de inicialização, sistema de memória permanente |
| 1.0 | 14/01/2026 | Versão inicial |

---

## 💡 NOTA PARA DESENVOLVEDORES

Este arquivo serve como **ponto de entrada** para agentes Claude Code. Ele redireciona para as regras completas em `RegrasDesenvolvimentoFrotiX.md`, garantindo que:

1. As regras sejam lidas no início de cada sessão
2. Novas regras sejam adicionadas no local correto
3. Haja consistência entre todas as IAs do projeto

---

**✅ Arquivo carregado com sucesso. Aguardando suas instruções.**
