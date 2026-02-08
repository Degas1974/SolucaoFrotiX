# Cursor AI – Regras do Projeto FrotiX

> **Sistema:** Cursor AI (VS Code Fork)
> **Versão:** 1.0
> **Última Atualização:** 18/01/2026

---

## 🚨 PROTOCOLO DE INICIALIZAÇÃO

### LEITURA OBRIGATÓRIA

Antes de gerar código ou responder:

1. ✅ **Ler:** `RegrasDesenvolvimentoFrotiX.md` (COMPLETO)
2. ✅ **Se banco de dados:** Ler `FrotiX.sql`
3. ✅ **Confirmar:** Leitura realizada antes de prosseguir

> **IMPORTANTE:** `RegrasDesenvolvimentoFrotiX.md` é a fonte oficial de todas as regras técnicas.

---

## 📋 ARQUIVOS DE CONFIGURAÇÃO

| Arquivo | Propósito | Quando Usar |
|---------|-----------|-------------|
| `RegrasDesenvolvimentoFrotiX.md` | ⭐ REGRAS OFICIAIS | SEMPRE |
| `.cursor/rules.md` | Regras Cursor AI | Automático |
| `FrotiX.sql` | Estrutura BD | Operações com dados |

---

## 🧠 SISTEMA DE MEMÓRIA PERMANENTE

### Como Adicionar Regras Permanentes

**GATILHOS:**
- "memorize"
- "guarde na memória"
- "adicione às regras"
- "sempre faça X"
- "nunca esqueça"

**PROCEDIMENTO:**

```yaml
1. Abrir: RegrasDesenvolvimentoFrotiX.md
2. Localizar: Seção apropriada (criar se necessário)
3. Adicionar: Regra no formato padrão
4. Salvar: Arquivo
5. Commitar: "docs: Adiciona regra permanente - [Nome]"
6. Push: Enviar para repositório
7. Confirmar: Informar usuário sobre memorização
```

**FORMATO PADRÃO:**

```markdown
### [N].[N] [Nome da Regra]

**Contexto:** [Motivo da existência da regra]

**Regra:** [Instrução clara - imperativo]

**Exemplo:**
\`\`\`[linguagem]
// Código demonstrando aplicação
\`\`\`

**Data de Adição:** DD/MM/AAAA
```

---

## ⚠️ REGRAS CRÍTICAS (RESUMO)

### 🔒 Try-Catch
```csharp
// OBRIGATÓRIO em TODAS as funções
try { /* código */ }
catch (Exception e)
{
    Alerta.TratamentoErroComLinha("arquivo", "método", e);
    return Json(new { success = false, message = e.Message });
}
```

### 🎨 UI/UX
```yaml
Alertas:
  ✅ Usar: Alerta.Sucesso/Erro/Confirmar
  ❌ Proibido: alert(), confirm()

Ícones:
  ✅ Usar: fa-duotone
  ❌ Proibido: fa-solid, fa-regular

Loading:
  ✅ Usar: FtxSpin.show/hide
  ❌ Proibido: spinner-border

Tooltips:
  ✅ Usar: data-ejtip (Syncfusion)
  ❌ Proibido: data-bs-toggle="tooltip"
```

### 🗄️ Banco de Dados
```yaml
Antes de codificar:
  - Consultar: FrotiX.sql
  - Verificar: Colunas, tipos, nullable
  - Confirmar: FKs e constraints
```

### 📝 Documentação
```yaml
Quando: Antes de cada commit
Onde: Pasta Documentacao/
Formato: Prosa + snippets + explicação
```

### 🔄 Git
```yaml
Branch: main (preferencial)
Push: SEMPRE para main (nunca outras branches sem autorização)
Commit: Automático IMEDIATO após código importante
Push: Automático IMEDIATO após commit (git push origin main)
Durante conversa: Commit automático ao fornecer código importante
Tipos: feat, fix, refactor, docs, style, chore
Exceção: Só não commitar se usuário pedir "aguarde"
```

---

## 🎯 COMPOSER & CHAT

### Instruções para Composer

Ao gerar código com Cursor Composer:

1. ✅ Sempre incluir try-catch
2. ✅ Usar sistema de alertas FrotiX (Alerta.*)
3. ✅ Usar ícones fa-duotone
4. ✅ Usar FtxSpin para loading
5. ✅ Consultar FrotiX.sql antes de DB ops
6. ✅ Atualizar documentação em Documentacao/

### Instruções para Chat

Ao responder via Cursor Chat:

1. ✅ Ler RegrasDesenvolvimentoFrotiX.md primeiro
2. ✅ Fornecer código completo (nunca parcial)
3. ✅ Incluir tratamento de erros
4. ✅ Seguir padrões do projeto
5. ✅ Sugerir atualização de documentação

---

## 📚 REFERÊNCIAS

| Arquivo | Descrição |
|---------|-----------|
| `RegrasDesenvolvimentoFrotiX.md` | ⭐ REGRAS CONSOLIDADAS |
| `FrotiX.sql` | Estrutura do banco |
| `wwwroot/js/alerta.js` | Sistema de alertas |
| `wwwroot/js/frotix.js` | Utilitários globais |
| `wwwroot/css/frotix.css` | Estilos globais |

---

## 🔧 CONFIGURAÇÃO CURSOR

### Settings.json

```json
{
  "cursor.aiRules": {
    "rulesFile": ".cursor/rules.md",
    "contextFiles": [
      "RegrasDesenvolvimentoFrotiX.md",
      "FrotiX.sql"
    ]
  }
}
```

---

## 💡 DICAS DE USO

### Comandos Rápidos

```
@rules - Ver regras
@RegrasDesenvolvimentoFrotiX.md - Consultar regras completas
@FrotiX.sql - Ver estrutura do banco
```

### Auto-Completion

Cursor deve sugerir automaticamente:
- Try-catch em funções sem tratamento
- Alerta.* em vez de alert()
- fa-duotone em vez de outros estilos
- FtxSpin em vez de spinners Bootstrap

---

## 🔄 VERSIONAMENTO

| Versão | Data | Alterações |
|--------|------|------------|
| 1.0 | 18/01/2026 | Criação inicial - Configuração Cursor |

---

**✅ Cursor AI configurado para FrotiX. Sistema pronto.**
