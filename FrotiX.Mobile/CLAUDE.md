# CLAUDE.md - Configuracao Claude Code (FrotiX.Mobile Workspace)

> **ATENCAO CLAUDE:** Este arquivo e carregado automaticamente no inicio de cada sessao.
> **Versao:** 1.0
> **Ultima Atualizacao:** 10/02/2026

---

## PROTOCOLO DE INICIALIZACAO (LEIA PRIMEIRO)

### ACOES OBRIGATORIAS AO INICIAR SESSAO

Antes de qualquer resposta ao usuario, voce DEVE:

1. **Ler completamente** o arquivo `RegrasDesenvolvimentoFrotiXMobile.md` (NESTE MESMO DIRETORIO)
2. **Confirmar mentalmente** que o arquivo foi lido
3. **Exibir** a mensagem de confirmacao visual (secao abaixo)

**IMPORTANTE:** Nao prossiga sem ler `RegrasDesenvolvimentoFrotiXMobile.md`. Ele contem TODAS as regras dos projetos mobile.

---

## CONFIRMACAO VISUAL OBRIGATORIA

**AO INICIAR CADA NOVA SESSAO/CHAT**, voce DEVE exibir a seguinte mensagem:

```
FROTIX MOBILE - CLAUDE CODE CONFIGURADO

Arquivos Carregados:
  RegrasDesenvolvimentoFrotiXMobile.md

Regras Criticas Ativas:
  * Try-catch obrigatorio em todas funcoes (C# e JS Interop)
  * AlertaJs via JS Interop (NUNCA alert() nativo)
  * fa-duotone (NUNCA fa-solid)
  * Syncfusion Blazor para componentes UI
  * Services = equivalente a Controllers do Web
  * Azure Relay para comunicacao com backend

Projetos do Workspace:
  * FrotiX.Mobile.Shared (Razor Class Library)
  * FrotiX.Mobile.Economildo (.NET MAUI Blazor Hybrid)
  * FrotiX.Mobile.Vistorias (.NET MAUI Blazor Hybrid)

Pronto para comecar!
```

---

## DIRETORIO PADRAO DE TRABALHO

**Este workspace contem a solucao FrotiX.Mobile** com 3 projetos:

- `FrotiX.Mobile.Shared/` - Razor Class Library (Models, Services, Helpers, Validations)
- `FrotiX.Mobile.Economildo/` - App MAUI Blazor Hybrid (com.camara.frotix.economildo)
- `FrotiX.Mobile.Vistorias/` - App MAUI Blazor Hybrid (com.camara.frotix.vistorias)

**NOTA:** Este workspace e INDEPENDENTE do workspace web (FrotiX.Site.Fevereiro). Nao referencie arquivos web daqui.

---

## HIERARQUIA DE ARQUIVOS DE REGRAS

| Arquivo | Funcao | Quando Ler |
|---------|--------|------------|
| **`RegrasDesenvolvimentoFrotiXMobile.md`** | FONTE OFICIAL DE REGRAS MOBILE | **SEMPRE** (obrigatorio) |
| **`CLAUDE.md`** | Indice e instrucoes de inicializacao (voce esta aqui) | Automatico |

---

## SISTEMA DE MEMORIA PERMANENTE

### REGRA: Como Memorizar Regras Permanentemente

**GATILHOS** - Quando o usuario disser:
- "memorize", "guarde na memoria", "lembre-se disso"
- "adicione as regras", "isso e uma regra nova"
- "nunca esqueca", "sempre faca X", "de agora em diante"

**ACAO OBRIGATORIA:**

1. Abrir e ler: `RegrasDesenvolvimentoFrotiXMobile.md`
2. **VERIFICAR DUPLICATAS:** Procurar se a informacao ja existe no arquivo
   - **Se ja existe e esta COMPLETA:** Informar o usuario que a regra ja esta registrada
   - **Se ja existe mas esta INCOMPLETA/DESATUALIZADA:** Atualizar a entrada existente sem duplicar
   - **Se NAO existe:** Criar nova entrada seguindo os passos abaixo
3. Identificar a secao tematica mais apropriada do arquivo
4. Adicionar a nova regra/orientacao seguindo o formato padrao abaixo
5. **NUNCA baguncar, reordenar ou reformatar** o conteudo ja existente no arquivo
6. Salvar arquivo
7. Confirmar ao usuario o que foi feito

### FORMATO PADRAO PARA NOVAS REGRAS

```markdown
### [NUMERO_SECAO].[NUMERO_SUBSECAO] [Nome da Regra]

**Contexto:** [Por que esta regra existe]

**Regra:** [O que deve ser feito/evitado - IMPERATIVO]

**Exemplo:**
\`\`\`[linguagem]
// Codigo de exemplo mostrando aplicacao da regra
\`\`\`

**Data de Adicao:** DD/MM/AAAA
```

---

## REGRAS CRITICAS (RESUMO RAPIDO)

### Arquitetura Mobile
- **Plataforma:** .NET MAUI Blazor Hybrid (net10.0-android)
- **Comunicacao:** Azure Relay via RelayApiService
- **Storage offline:** SecureStorage (JSON serializado)
- **Services = Controllers:** A camada de Services em Shared e o equivalente dos Controllers no web

### Try-Catch
- **OBRIGATORIO** em TODAS as funcoes (C# e JS)
- Usar `AlertaJs` via JS Interop para erros de usuario
- Usar logging em arquivo (MauiProgram.LogInfo/LogError)

### UI/UX
- **Alertas:** SEMPRE `AlertaJs` via JS Interop (NUNCA `alert()` nativo)
- **Icones:** SEMPRE `fa-duotone`, NUNCA `fa-solid/regular/light`
- **Componentes:** Syncfusion Blazor (SfComboBox, SfDatePicker, SfTimePicker, etc.)
- **Economildo adicional:** MudBlazor (MudDatePicker, MudAutocomplete, MudSelect)
- **Vistorias adicional:** Radzen Blazor

### Memoria Mobile
- **GC.Collect()** antes de navegacao entre paginas
- **Dispose** de recursos nao gerenciados
- **Cuidado** com listas grandes em memoria

### Git
- Branch preferencial: `main`
- **Push SEMPRE para `main`**
- Tipos de commit: `feat:`, `fix:`, `refactor:`, `docs:`, `style:`, `chore:`

---

## REFERENCIA RAPIDA

| Arquivo/Pasta | Descricao |
|---------------|-----------|
| `RegrasDesenvolvimentoFrotiXMobile.md` | Regras consolidadas (LEITURA OBRIGATORIA) |
| `FrotiX.Mobile.Shared/Services/` | Camada de Services (equivalente Controllers) |
| `FrotiX.Mobile.Shared/Models/` | Modelos compartilhados |
| `FrotiX.Mobile.Shared/Helpers/` | Helpers utilitarios |
| `FrotiX.Mobile.Shared/wwwroot/js/` | JavaScript compartilhado (SweetAlert interop) |

---

## CHECKLIST DE INICIO DE SESSAO

Antes de responder ao usuario, confirme mentalmente:

- [ ] Li `RegrasDesenvolvimentoFrotiXMobile.md` completamente?
- [ ] Entendi a hierarquia de arquivos?
- [ ] Sei que Services = Controllers do web?
- [ ] Conheco as regras de try-catch, alertas, icones?
- [ ] Sei que a comunicacao e via Azure Relay?

---

## VERSIONAMENTO

| Versao | Data | Mudancas |
|--------|------|----------|
| 1.0 | 10/02/2026 | Versao inicial - Configuracao para workspace FrotiX.Mobile |

---

**Arquivo carregado com sucesso. Aguardando instrucoes.**
