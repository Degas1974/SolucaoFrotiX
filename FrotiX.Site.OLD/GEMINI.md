# 🤖 GEMINI.md – Configuração Google AI Studio / Gemini Code Assist

> **ATENÇÃO GEMINI:** Este arquivo deve ser incluído no contexto de cada sessão.
> **Versão:** 1.0
> **Última Atualização:** 18/01/2026

---

## 🚨 INSTRUÇÕES DE INICIALIZAÇÃO

### ✅ PROTOCOLO OBRIGATÓRIO

**Antes de gerar qualquer código ou resposta:**

1. ✅ **Leia completamente** o arquivo `RegrasDesenvolvimentoFrotiX.md`
2. ✅ **Se trabalhar com banco de dados:** Leia `FrotiX.sql`
3. ✅ **Confirme** que leu ambos os arquivos antes de prosseguir

**IMPORTANTE:** O arquivo `RegrasDesenvolvimentoFrotiX.md` contém TODAS as regras técnicas do projeto. Não codifique sem lê-lo.

---

## 📋 ARQUIVOS DE REGRAS DO PROJETO

| Arquivo | Propósito | Quando Ler |
|---------|-----------|------------|
| **`RegrasDesenvolvimentoFrotiX.md`** | ⭐ FONTE OFICIAL DE REGRAS | **SEMPRE** (obrigatório) |
| **`GEMINI.md`** | Instruções de inicialização Gemini | Automático (você está aqui) |
| **`FrotiX.sql`** | Estrutura do banco de dados SQL Server | Ao trabalhar com dados |
| **`.claude/CLAUDE.md`** | Diretrizes de documentação | Ao documentar código |

---

## 🧠 SISTEMA DE MEMÓRIA PERMANENTE

### Como Adicionar Regras Permanentes ao Projeto

**GATILHOS** - Quando o usuário solicitar:

- "memorize"
- "guarde na memória"
- "lembre-se disso"
- "adicione às regras"
- "nova regra"
- "nunca esqueça"
- "sempre faça X"
- "de agora em diante"

**PROCEDIMENTO OBRIGATÓRIO:**

```yaml
Passo 1: Abrir arquivo RegrasDesenvolvimentoFrotiX.md
Passo 2: Identificar seção apropriada (ou criar nova se necessário)
Passo 3: Adicionar regra no formato padrão (ver abaixo)
Passo 4: Salvar arquivo
Passo 5: Criar commit Git com mensagem: "docs: Adiciona regra permanente - [Nome]"
Passo 6: Push para repositório
Passo 7: Confirmar ao usuário que regra foi memorizada
```

### 📐 FORMATO DE REGRA PERMANENTE

```markdown
### [NÚMERO_SEÇÃO].[NÚMERO_SUBSEÇÃO] [Nome da Regra]

**Contexto:** [Explicação do motivo da regra existir]

**Regra:** [Instrução clara e direta do que deve/não deve ser feito]

**Exemplo:**
\`\`\`[linguagem]
// Código demonstrando a aplicação correta da regra
\`\`\`

**Data de Adição:** DD/MM/AAAA
```

### ✅ EXEMPLO REAL

**Usuário solicita:**
> "Memorize: sempre validar datas em C# usando DateTime.TryParse"

**Você executa:**

1. Abrir `RegrasDesenvolvimentoFrotiX.md`
2. Localizar seção `4. PADRÕES DE CÓDIGO` > `4.1 Controllers / APIs`
3. Adicionar nova subseção:

```markdown
### 4.1.1 Validação de Datas com TryParse

**Contexto:** Para evitar exceções de conversão e garantir tratamento seguro de entrada de usuário.

**Regra:** SEMPRE usar `DateTime.TryParse()` ao converter strings em datas. NUNCA usar `DateTime.Parse()` diretamente.

**Exemplo:**
\`\`\`csharp
// ✅ CORRETO
public IActionResult ProcessarData(string dataStr)
{
    if (!DateTime.TryParse(dataStr, out DateTime data))
    {
        return BadRequest("Data inválida");
    }

    // processar data...
    return Ok(data);
}

// ❌ ERRADO
public IActionResult ProcessarData(string dataStr)
{
    DateTime data = DateTime.Parse(dataStr); // Pode lançar exceção
    return Ok(data);
}
\`\`\`

**Data de Adição:** 18/01/2026
```

4. Salvar
5. Executar: `git add RegrasDesenvolvimentoFrotiX.md && git commit -m "docs: Adiciona regra permanente - Validação de Datas com TryParse" && git push`
6. Responder: "✅ Regra memorizada em `RegrasDesenvolvimentoFrotiX.md` (seção 4.1.1)"

---

## ⚠️ REGRAS CRÍTICAS (RESUMO EXECUTIVO)

### 🗄️ Banco de Dados

```yaml
Obrigação: SEMPRE consultar FrotiX.sql ANTES de codificar
Proibido: Assumir nomes de colunas sem verificar
Verificar: Tipos de dados, nullable, chaves estrangeiras
```

### 🔒 Tratamento de Erros

```yaml
Obrigação: Try-catch em TODAS funções (C# e JavaScript)
Ferramenta: Alerta.TratamentoErroComLinha(arquivo, metodo, erro)
Proibido: Funções sem tratamento de exceção
```

### 🎨 Interface e UX

```yaml
Alertas:
  Usar: Alerta.Sucesso(), Alerta.Erro(), Alerta.Confirmar()
  Proibido: alert(), confirm(), prompt()

Ícones:
  Usar: fa-duotone (sempre)
  Proibido: fa-solid, fa-regular, fa-light, fa-thin

Loading:
  Usar: FtxSpin.show(), FtxSpin.hide()
  Proibido: Spinners Bootstrap, fa-spinner

Tooltips:
  Usar: Syncfusion data-ejtip
  Proibido: Bootstrap data-bs-toggle="tooltip"
```

### 📝 Documentação

```yaml
Quando: SEMPRE antes de commitar
Onde: Pasta Documentacao/
Formato: Prosa técnica + snippets + explicação detalhada
Obrigação: Atualizar arquivo .md correspondente
```

### 🔄 Controle de Versão

```yaml
Branch: main (preferencial)
Push: SEMPRE para main (nunca outras branches sem autorização)
Commit: Automático IMEDIATO após criar/alterar código importante
Push: Automático IMEDIATO após commit (sempre para main)
Commit durante conversa: Automático quando fornecer código importante
Documentação: Atualizar ANTES do push
Tipos: feat, fix, refactor, docs, style, chore
```

---

## 📚 ARQUIVOS DE REFERÊNCIA

| Arquivo | Conteúdo |
|---------|----------|
| `RegrasDesenvolvimentoFrotiX.md` | ⭐ Regras consolidadas (LEITURA OBRIGATÓRIA) |
| `FrotiX.sql` | Estrutura completa do banco de dados |
| `wwwroot/js/alerta.js` | Implementação do sistema de alertas |
| `wwwroot/js/frotix.js` | Utilitários globais JavaScript (FtxSpin) |
| `wwwroot/css/frotix.css` | Estilos globais do sistema |

---

## 🎯 CHECKLIST PRÉ-CÓDIGO

Antes de gerar qualquer código, confirme:

- [ ] Li `RegrasDesenvolvimentoFrotiX.md` completamente?
- [ ] Se banco de dados: li `FrotiX.sql`?
- [ ] Entendi sistema de memória permanente?
- [ ] Conheço regras de try-catch, alertas, ícones?
- [ ] Sei onde documentar (pasta Documentacao/)?

---

## 🔄 HISTÓRICO DE VERSÕES

| Versão | Data | Alterações |
|--------|------|------------|
| 1.0 | 18/01/2026 | Criação inicial - Configuração para Gemini |

---

## 💡 NOTA TÉCNICA

Este arquivo foi criado especificamente para **Google AI Studio** e **Gemini Code Assist**. Devido às diferenças de integração com IDEs:

- **Google AI Studio (Web):** Inclua este arquivo manualmente no contexto
- **Gemini Code Assist (VS Code):** Configure em `.vscode/settings.json`:

```json
{
  "gemini.contextFiles": [
    "GEMINI.md",
    "RegrasDesenvolvimentoFrotiX.md"
  ]
}
```

---

**✅ Configuração carregada. Sistema pronto para desenvolvimento FrotiX.**
