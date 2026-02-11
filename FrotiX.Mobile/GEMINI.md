# GEMINI.md - Configuracao Google AI Studio / Gemini Code Assist (FrotiX.Mobile)

> **ATENCAO GEMINI:** Este arquivo deve ser incluido no contexto de cada sessao.
> **Versao:** 1.0
> **Ultima Atualizacao:** 10/02/2026

---

## INSTRUCOES DE INICIALIZACAO

### PROTOCOLO OBRIGATORIO

**Antes de gerar qualquer codigo ou resposta:**

1. **Leia completamente** o arquivo `RegrasDesenvolvimentoFrotiXMobile.md`
2. **Confirme** que leu o arquivo antes de prosseguir

**IMPORTANTE:** O arquivo `RegrasDesenvolvimentoFrotiXMobile.md` contem TODAS as regras tecnicas dos projetos mobile. Nao codifique sem le-lo.

---

## ARQUIVOS DE REGRAS DO PROJETO

| Arquivo | Proposito | Quando Ler |
|---------|-----------|------------|
| **`RegrasDesenvolvimentoFrotiXMobile.md`** | FONTE OFICIAL DE REGRAS MOBILE | **SEMPRE** (obrigatorio) |
| **`GEMINI.md`** | Instrucoes de inicializacao Gemini (voce esta aqui) | Automatico |

---

## CONTEXTO DO PROJETO

Este workspace contem a solucao **FrotiX.Mobile** - aplicativos .NET MAUI Blazor Hybrid para Android:

- **FrotiX.Mobile.Shared** - Razor Class Library compartilhada (Models, Services, Helpers)
- **FrotiX.Mobile.Economildo** - App de controle de abastecimento/viagens
- **FrotiX.Mobile.Vistorias** - App de vistorias veiculares

**Stack tecnologica:**
- .NET 10 / MAUI Blazor Hybrid / Android (net10.0-android)
- Syncfusion Blazor + MudBlazor (Economildo) + Radzen (Vistorias)
- Azure Relay para comunicacao com backend
- SecureStorage para cache offline

---

## SISTEMA DE MEMORIA PERMANENTE

### Como Adicionar Regras Permanentes ao Projeto

**GATILHOS** - Quando o usuario solicitar:
- "memorize", "guarde na memoria", "lembre-se disso"
- "adicione as regras", "nova regra"
- "nunca esqueca", "sempre faca X", "de agora em diante"

**PROCEDIMENTO OBRIGATORIO:**

```yaml
Passo 1: Abrir arquivo RegrasDesenvolvimentoFrotiXMobile.md
Passo 2: Verificar se a regra ja existe (evitar duplicatas)
Passo 3: Identificar secao apropriada (ou criar nova se necessario)
Passo 4: Adicionar regra no formato padrao (ver abaixo)
Passo 5: Salvar arquivo
Passo 6: Confirmar ao usuario que regra foi memorizada
```

### FORMATO DE REGRA PERMANENTE

```markdown
### [NUMERO_SECAO].[NUMERO_SUBSECAO] [Nome da Regra]

**Contexto:** [Explicacao do motivo da regra existir]

**Regra:** [Instrucao clara e direta do que deve/nao deve ser feito]

**Exemplo:**
\`\`\`[linguagem]
// Codigo demonstrando a aplicacao correta da regra
\`\`\`

**Data de Adicao:** DD/MM/AAAA
```

---

## REGRAS CRITICAS (RESUMO EXECUTIVO)

### Arquitetura

```yaml
Plataforma: .NET MAUI Blazor Hybrid (net10.0-android)
Comunicacao: Azure Relay via RelayApiService
Storage: SecureStorage (JSON serializado)
Services: Equivalente a Controllers do web - toda logica de negocio passa por Services
```

### Tratamento de Erros

```yaml
Obrigacao: Try-catch em TODAS funcoes (C# e JavaScript)
Ferramenta: AlertaJs via JS Interop (SweetAlert2)
Logging: MauiProgram.LogInfo/LogError (arquivo com rotacao 5MB)
Proibido: Funcoes sem tratamento de excecao
```

### Interface e UX

```yaml
Alertas:
  Usar: AlertaJs via JS Interop (SweetAlert2)
  Proibido: alert(), confirm(), prompt() nativos

Icones:
  Usar: fa-duotone (sempre)
  Proibido: fa-solid, fa-regular, fa-light, fa-thin

Componentes UI:
  Principal: Syncfusion Blazor (SfComboBox, SfDatePicker, SfTimePicker, etc.)
  Economildo: MudBlazor adicional (MudDatePicker, MudAutocomplete, MudSelect)
  Vistorias: Radzen Blazor adicional
```

### Memoria e Performance

```yaml
Navegacao: GC.Collect() antes de navegar entre paginas
Recursos: Dispose de objetos nao gerenciados
Listas: Cuidado com listas grandes em memoria
```

### Controle de Versao

```yaml
Branch: main (preferencial)
Push: SEMPRE para main
Tipos: feat, fix, refactor, docs, style, chore
```

---

## ARQUIVOS DE REFERENCIA

| Arquivo/Pasta | Conteudo |
|---------------|----------|
| `RegrasDesenvolvimentoFrotiXMobile.md` | Regras consolidadas (LEITURA OBRIGATORIA) |
| `FrotiX.Mobile.Shared/Services/` | Camada de Services (equivalente Controllers) |
| `FrotiX.Mobile.Shared/Models/` | Modelos compartilhados |
| `FrotiX.Mobile.Shared/Helpers/` | Helpers utilitarios |
| `FrotiX.Mobile.Shared/wwwroot/js/` | JavaScript compartilhado |

---

## CHECKLIST PRE-CODIGO

Antes de gerar qualquer codigo, confirme:

- [ ] Li `RegrasDesenvolvimentoFrotiXMobile.md` completamente?
- [ ] Entendi que Services = Controllers do web?
- [ ] Conheco as regras de try-catch, alertas, icones?
- [ ] Sei que comunicacao e via Azure Relay?
- [ ] Sei diferenciar padroes Economildo vs Vistorias?

---

## NOTA TECNICA

Este arquivo foi criado para **Google AI Studio** e **Gemini Code Assist**:

- **Google AI Studio (Web):** Inclua este arquivo manualmente no contexto
- **Gemini Code Assist (VS Code):** Configurado em `.vscode/settings.json`:

```json
{
  "gemini.contextFiles": [
    "GEMINI.md",
    "RegrasDesenvolvimentoFrotiXMobile.md"
  ]
}
```

---

## HISTORICO DE VERSOES

| Versao | Data | Alteracoes |
|--------|------|------------|
| 1.0 | 10/02/2026 | Criacao inicial - Configuracao para workspace FrotiX.Mobile |

---

**Configuracao carregada. Sistema pronto para desenvolvimento FrotiX Mobile.**
