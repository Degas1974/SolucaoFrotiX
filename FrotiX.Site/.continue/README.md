# Continue.dev – Configuração para FrotiX

> **Sistema:** Continue (VS Code Extension)
> **Versão:** 1.0
> **Última Atualização:** 18/01/2026

---

## 📋 ARQUIVOS DE CONFIGURAÇÃO

```
.continue/
├── config.json          # Configuração principal
└── README.md           # Este arquivo
```

---

## 🚨 PROTOCOLO DE INICIALIZAÇÃO

### LEITURA OBRIGATÓRIA

O Continue está configurado para:

1. ✅ Ler `RegrasDesenvolvimentoFrotiX.md` automaticamente
2. ✅ Ler `FrotiX.sql` quando trabalhar com banco
3. ✅ Seguir todas as regras do projeto

---

## 🎯 COMANDOS PERSONALIZADOS

### Slash Commands

| Comando | Descrição | Uso |
|---------|-----------|-----|
| `/regras` | Ver resumo das regras | `/regras` |
| `/memorizar` | Adicionar regra permanente | `/memorizar [regra]` |
| `/banco` | Consultar estrutura do banco | `/banco` |
| `/edit` | Editar código destacado | `/edit` |
| `/comment` | Comentar código | `/comment` |
| `/commit` | Gerar mensagem de commit | `/commit` |
| `/test` | Gerar testes unitários | `/test` |
| `/check` | Verificar conformidade | `/check` |

---

## 🧠 MEMÓRIA PERMANENTE

### Como Funciona

Quando você usa `/memorizar` ou diz "memorize":

1. Continue abre `RegrasDesenvolvimentoFrotiX.md`
2. Adiciona regra no formato padrão
3. Faz commit: `docs: Adiciona regra - [Nome]`
4. Confirma a memorização

**Formato da regra:**

```markdown
### [N].[N] [Nome da Regra]

**Contexto:** [Por que existe]
**Regra:** [O que fazer/evitar]
**Exemplo:**
\`\`\`[lang]
// código
\`\`\`
**Data:** DD/MM/AAAA
```

---

## ⚠️ REGRAS CRÍTICAS

### Try-Catch
```csharp
// OBRIGATÓRIO
try { /* código */ }
catch (Exception e)
{
    Alerta.TratamentoErroComLinha("arquivo", "método", e);
}
```

### UI/UX
```yaml
Alertas: Alerta.* (não alert/confirm)
Ícones: fa-duotone (não fa-solid)
Loading: FtxSpin (não Bootstrap spinner)
Tooltips: data-ejtip (não Bootstrap)
```

### Banco de Dados
```yaml
Consultar: FrotiX.sql sempre
Verificar: Colunas, tipos, nullable
```

---

## 🔧 CONFIGURAÇÃO MANUAL

### Continue Settings (Ctrl+Shift+P > Continue: Open Settings)

Já configurado em `config.json`:

- ✅ System message com regras
- ✅ Context providers (code, diff, terminal, etc.)
- ✅ Slash commands personalizados
- ✅ Custom commands (/test, /check)

---

## 💡 EXEMPLOS DE USO

### Adicionar Regra Permanente

```
Você: /memorizar sempre usar UTC para datas no backend

Continue:
✅ Abrindo RegrasDesenvolvimentoFrotiX.md...
✅ Adicionando regra na seção 4.1...
✅ Commit: "docs: Adiciona regra - Uso de UTC para Datas"
✅ Regra memorizada em RegrasDesenvolvimentoFrotiX.md (seção 4.1.2)
```

### Verificar Código

```
Você: /check

Continue:
Verificando código contra RegrasDesenvolvimentoFrotiX.md...

❌ Problemas encontrados:
1. Linha 15: Usando alert() em vez de Alerta.Erro()
2. Linha 23: Ícone fa-solid em vez de fa-duotone
3. Linha 45: Falta try-catch na função processarDados()

Sugestões de correção: [...]
```

### Consultar Banco

```
Você: /banco qual a estrutura da tabela Viagem?

Continue:
Consultando FrotiX.sql...

Tabela: Viagem
Colunas:
- ViagemId (uniqueidentifier, PK)
- DataInicial (datetime2, nullable)
- DataFinal (datetime2, nullable)
- VeiculoId (uniqueidentifier, FK)
- MotoristaId (uniqueidentifier, FK)
[...]
```

---

## 📚 REFERÊNCIAS

| Arquivo | Descrição |
|---------|-----------|
| `RegrasDesenvolvimentoFrotiX.md` | ⭐ REGRAS OFICIAIS |
| `FrotiX.sql` | Estrutura do banco |
| `wwwroot/js/alerta.js` | Sistema de alertas |
| `wwwroot/js/frotix.js` | Utilitários JS |

---

## 🔄 ATUALIZAÇÃO

Para atualizar configuração:

```bash
# Editar manualmente
code .continue/config.json

# Ou via Continue UI
Ctrl+Shift+P > Continue: Open Settings
```

---

**✅ Continue.dev configurado para FrotiX. Sistema pronto.**
