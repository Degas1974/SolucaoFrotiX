# DocumentacaoFrotiX.md - Regras Mestras de Documentação

> **⚠️ LEITURA OBRIGATÓRIA ANTES DE CADA COMMIT DE DOCUMENTAÇÃO**
>
> Este arquivo consolida TODAS as regras de documentação do projeto FrotiX.
> Claude deve lê-lo antes de criar ou atualizar qualquer arquivo MD ou HTML.

> **Última Atualização**: 13/01/2026
> **Versão**: 1.0

---

# ÍNDICE

1. [Regra Crítica - Atualização Obrigatória](#regra-crítica---atualização-obrigatória)
2. [Documentação de Razor Pages (CSHTML + CSHTML.CS)](#documentação-de-razor-pages-cshtml--cshtmlcs)
3. [Documentação de Integração Completa](#documentação-de-integração-completa)
4. [Estrutura de Arquivos MD](#estrutura-de-arquivos-md)
5. [Estrutura de Arquivos HTML (Portfólio A4)](#estrutura-de-arquivos-html-portfólio-a4)
6. [Paleta de Cores e Identidade Visual](#paleta-de-cores-e-identidade-visual)
7. [Padrões de Código FrotiX](#padrões-de-código-frotix)
8. [Workflow de Documentação](#workflow-de-documentação)
9. [Troubleshooting e Validação](#troubleshooting-e-validação)

---

# REGRA CRÍTICA - Atualização Obrigatória

## ⚠️ QUALQUER MUDANÇA EM QUALQUER ARQUIVO DOCUMENTADO DEVE SER ATUALIZADA E COMMITADA IMEDIATAMENTE

### Arquivos que REQUEREM atualização imediata da documentação:

- ✅ **CSHTML** (Razor Pages) - Qualquer alteração em `.cshtml`
- ✅ **CSHTML.CS** (PageModel) - Qualquer alteração em `.cshtml.cs`
- ✅ **JAVASCRIPT** - Qualquer alteração em `.js` (especialmente em `wwwroot/js/`)
- ✅ **CONTROLLERS** - Qualquer alteração em `.cs` em `Controllers/`
- ✅ **HELPERS** - Qualquer alteração em `.cs` em `Helpers/`
- ✅ **REPOSITORY** - Qualquer alteração em `.cs` em `Repository/`
- ✅ **DATA** - Qualquer alteração em `.cs` em `Data/`
- ✅ **SERVICES** - Qualquer alteração em `.cs` em `Services/`
- ✅ **MIDDLEWARES** - Qualquer alteração em `.cs` em `Middlewares/`
- ✅ **MODELS** - Qualquer alteração em `.cs` em `Models/`
- ✅ **CSS** - Qualquer alteração em `.css`

### Processo OBRIGATÓRIO após qualquer alteração:

1. **IDENTIFICAR** qual arquivo foi alterado
2. **LOCALIZAR** a documentação correspondente em `Documentacao/`
3. **ATUALIZAR** a documentação refletindo EXATAMENTE as mudanças feitas
4. **ATUALIZAR** a seção "PARTE 2: LOG DE MODIFICAÇÕES/CORREÇÕES" com:
   - Data da alteração
   - Descrição do que foi alterado
   - Arquivos afetados
   - Impacto da mudança
5. **COMMITAR** imediatamente com mensagem: `docs: Atualiza documentação de [Nome do Arquivo] - [Breve descrição]`
6. **VERIFICAR** se o arquivo `0-INDICE-GERAL.md` precisa ser atualizado

---

# Documentação de Razor Pages (CSHTML + CSHTML.CS)

## 📋 REGRA FUNDAMENTAL

**SEMPRE** documentar o par **CSHTML + CSHTML.CS** juntos em um único arquivo:

### Padrão de Nomenclatura:

```
Arquivos de código:
├── UpsertCupons.cshtml          (View Razor - Frontend)
└── UpsertCupons.cshtml.cs       (PageModel - Backend)

Documentação (mesmo diretório):
├── UpsertCupons.md              (Documentação técnica - Markdown)
└── UpsertCupons.html            (Documentação portfólio - HTML A4)
```

### Exemplo Prático Completo:

```
Pages/Abastecimento/
├── UpsertCupons.cshtml          ← View
├── UpsertCupons.cshtml.cs       ← PageModel

Documentacao/Pages/Abastecimento/
├── UpsertCupons.md              ← Documentação MD (único arquivo para ambos)
└── UpsertCupons.html            ← Documentação HTML A4 (portfólio)
```

---

## 📐 Estrutura do Arquivo MD para Razor Pages

O arquivo único `[Nome].md` deve documentar ambos os arquivos em seções separadas:

### PARTE 1 - Documentação da Funcionalidade

#### Seção 1: Visão Geral Integrada
- Descrição completa da funcionalidade
- Como frontend e backend trabalham juntos
- Fluxo de dados completo (usuário → view → pagemodel → repository → banco)

#### Seção 2: Frontend (CSHTML)
- **HTML/Razor Markup**:
  - Estrutura de cards, modais, formulários
  - Uso de componentes Syncfusion/Telerik
  - DataAnnotations e validações

- **JavaScript Inline**:
  - Inicialização de componentes
  - Event handlers
  - Chamadas AJAX
  - Validações client-side

- **CSS Inline**:
  - Estilos específicos da página
  - Animações customizadas
  - Responsividade

#### Seção 3: Backend (CSHTML.CS - PageModel)
- **Propriedades**:
  - ViewData
  - BindProperty
  - Campos injetados (DI)

- **Handlers**:
  - OnGet() / OnGetAsync()
  - OnPost() / OnPostAsync()
  - Handlers customizados

- **Métodos Auxiliares**:
  - Validações
  - Transformações de dados
  - Lógica de negócio

#### Seção 4: Integração Frontend ↔ Backend
- **Model Binding**:
  - Como dados fluem da view para o PageModel
  - Uso de [BindProperty]
  - Validações compartilhadas

- **ViewData/TempData**:
  - Dados passados do backend para frontend
  - Estado temporário entre requisições

- **AJAX Calls**:
  - Endpoints chamados via JavaScript
  - Formato de request/response
  - Tratamento de erros

#### Seção 5: Integrações Externas
**ESTA É A SEÇÃO MAIS IMPORTANTE - NÃO PODE FALTAR**

Documentar TODAS as integrações com:

##### Controllers (APIs)
- Quais endpoints são chamados?
- Formato de request/response
- Tratamento de erros
- Exemplo de código

##### Helpers
- Quais helpers são usados?
- Onde são chamados (frontend/backend)?
- Exemplo de uso
- Dependências

##### JavaScript Externos
- Quais arquivos `.js` são referenciados?
- Onde estão localizados (`wwwroot/js/...`)
- Funções específicas utilizadas
- Inicialização e configuração

##### CSS Externos
- Quais arquivos `.css` são usados?
- Classes específicas aplicadas
- Estilos globais vs. locais
- Sobrescritas de tema

##### Repository/Services
- Quais repositórios são injetados?
- Métodos chamados
- Entidades manipuladas
- Transações e Unit of Work

##### Models/ViewModels
- Quais models são usados?
- Estrutura de dados
- Validações (DataAnnotations)
- Mapeamento com banco

##### Banco de Dados
- Tabelas/Views consultadas
- Stored Procedures chamadas
- Triggers afetados
- Estrutura SQL relevante

##### Componentes de Terceiros
- Syncfusion (qual componente? configuração?)
- Telerik (qual componente?)
- jQuery plugins
- FontAwesome icons

##### Sistemas Globais FrotiX
- `Alerta.js` - Uso de alertas customizados
- `sweetalert_interop.js` - Confirmações
- `global-toast.js` - Notificações
- `frotix.js` - Funções globais
- `syncfusion_tooltips.js` - Tooltips

#### Seção 6: Fluxo Completo de Dados

Documentar o fluxo passo a passo:

```
1. Usuário acessa /Abastecimento/UpsertCupons?id=123
   ↓
2. OnGet(Guid id) é chamado (PageModel)
   ↓
3. Repository busca dados no banco
   ↓
4. ViewModel é populado
   ↓
5. View renderiza com dados
   ↓
6. JavaScript inicializa componentes Syncfusion
   ↓
7. Usuário preenche formulário
   ↓
8. JavaScript faz validações client-side
   ↓
9. Usuário clica "Salvar"
   ↓
10. JavaScript chama AJAX para upload de PDF (se houver)
   ↓
11. OnPostSubmit() é chamado (PageModel)
   ↓
12. Validações server-side
   ↓
13. Repository salva no banco
   ↓
14. Trigger executa (se houver)
   ↓
15. Toast de sucesso via AppToast.show()
   ↓
16. Redirecionamento para página de listagem
```

#### Seção 7: Validações
- Frontend (JavaScript/jQuery)
- Backend (ModelState, DataAnnotations)
- Consistência entre ambas
- Mensagens de erro

#### Seção 8: Troubleshooting
- Problemas comuns
- Erros típicos
- Soluções testadas
- Debug tips

### PARTE 2 - Log de Modificações/Correções

Formato cronológico decrescente (mais recente primeiro):

```markdown
## [DD/MM/AAAA HH:mm] - Título da Modificação

**Descrição**: O que foi alterado e por quê

**Arquivos Afetados**:
- Frontend (CSHTML):
  - Linha X: Alteração no HTML
  - Linha Y: Novo JavaScript
- Backend (CSHTML.CS):
  - Linha Z: Novo método
  - Linha W: Atualização de validação

**Impacto**: O que isso afeta no sistema

**Status**: ✅ Concluído / 🔄 Em Progresso / ⚠️ Requer Testes

**Responsável**: Nome

**Versão**: X.X
```

---

# Documentação de Integração Completa

## 🔗 REGRA: Documentar TODAS as Dependências

A documentação **NÃO É SOMENTE** entre CSHTML e CSHTML.CS.

É também entre **TODOS os arquivos auxiliares** que eles chamam e usam:

### Checklist de Integrações Obrigatórias

Para cada arquivo documentado, responder:

#### ✅ Controllers
- [ ] Quais controllers são chamados via AJAX?
- [ ] Quais endpoints específicos?
- [ ] Formato de request/response?
- [ ] Tratamento de erros?

#### ✅ Helpers
- [ ] Quais helpers são usados?
- [ ] Onde são invocados (frontend/backend)?
- [ ] Métodos específicos chamados?

#### ✅ JavaScript
- [ ] Quais arquivos JS externos são referenciados?
- [ ] Localização (`wwwroot/js/...`)?
- [ ] Funções específicas utilizadas?
- [ ] Dependências entre arquivos JS?

#### ✅ CSS
- [ ] Quais arquivos CSS são usados?
- [ ] Classes específicas aplicadas?
- [ ] Sobrescritas de estilos globais?

#### ✅ Repository/Services
- [ ] Quais repositórios são injetados?
- [ ] Quais métodos são chamados?
- [ ] Entidades manipuladas?
- [ ] Transações e Unit of Work?

#### ✅ Models
- [ ] Quais models/viewmodels são usados?
- [ ] Estrutura de dados?
- [ ] Validações (DataAnnotations)?
- [ ] Mapeamento com banco?

#### ✅ Banco de Dados
- [ ] Tabelas/Views consultadas?
- [ ] Stored Procedures chamadas?
- [ ] Triggers afetados?
- [ ] Constraints e validações?

#### ✅ Componentes de Terceiros
- [ ] Syncfusion (qual componente)?
- [ ] Telerik?
- [ ] jQuery plugins?
- [ ] FontAwesome (quais ícones)?

#### ✅ Sistemas Globais FrotiX
- [ ] `Alerta.js` - Sistema de alertas?
- [ ] `sweetalert_interop.js` - Confirmações?
- [ ] `global-toast.js` - Toasts?
- [ ] `frotix.js` - Funções globais?
- [ ] `syncfusion_tooltips.js` - Tooltips?

### Diagrama de Interdependências

Sempre incluir um diagrama visual mostrando todas as conexões:

```
┌─────────────────────────────────────────────────────────┐
│                   UpsertCupons.cshtml                   │
│  (Frontend - View Razor)                                │
├─────────────────────────────────────────────────────────┤
│  Referências:                                           │
│  • wwwroot/js/abastecimento/cupons.js                   │
│  • wwwroot/css/abastecimento.css                        │
│  • Syncfusion DropDownList                              │
│  • Syncfusion Upload                                    │
│  • FontAwesome Duotone icons                            │
└─────────────────┬───────────────────────────────────────┘
                  │
                  ▼
┌─────────────────────────────────────────────────────────┐
│              UpsertCupons.cshtml.cs                     │
│  (Backend - PageModel)                                  │
├─────────────────────────────────────────────────────────┤
│  Dependências Injetadas:                                │
│  • IUnitOfWork _unitOfWork                              │
│  • INotyfService _notyf                                 │
│  • IWebHostEnvironment _hostingEnvironment             │
│                                                         │
│  Métodos:                                               │
│  • OnGet(Guid id)                                       │
│  • OnPostSubmit()                                       │
│  • OnPostEdit(Guid id)                                  │
│  • OnPostSavePDF(IEnumerable<IFormFile>)               │
└─────────────────┬───────────────────────────────────────┘
                  │
    ┌─────────────┼─────────────┐
    ▼             ▼             ▼
┌────────┐  ┌──────────┐  ┌─────────────┐
│Repository│ │Controllers│ │Sistema Arq. │
│          │  │          │  │             │
│GetFirst  │  │/api/     │  │FileStream   │
│Add       │  │Cupom     │  │Directory    │
│Update    │  │          │  │             │
│Save      │  │          │  │             │
└────┬─────┘  └──────────┘  └─────────────┘
     │
     ▼
┌─────────────────────────────────────┐
│      Banco de Dados                 │
├─────────────────────────────────────┤
│  Tabelas:                           │
│  • RegistroCupomAbastecimento       │
│                                     │
│  Triggers:                          │
│  • tr_RegistroCupom_AfterInsert     │
└─────────────────────────────────────┘
```

---

# Estrutura de Arquivos MD

## 📄 Template Padrão para Arquivos MD

### Cabeçalho Obrigatório

```markdown
# Documentação: [Nome do Arquivo/Funcionalidade]

> **Última Atualização**: DD/MM/AAAA
> **Versão Atual**: X.X

---

# PARTE 1: DOCUMENTAÇÃO DA FUNCIONALIDADE

## Índice
1. [Visão Geral](#visão-geral)
2. [Arquitetura](#arquitetura)
3. [Estrutura de Arquivos](#estrutura-de-arquivos)
4. [Frontend (se aplicável)](#frontend)
5. [Backend (se aplicável)](#backend)
6. [Integrações](#integrações)
7. [Lógica de Negócio](#lógica-de-negócio)
8. [Validações](#validações)
9. [Fluxo de Dados](#fluxo-de-dados)
10. [Exemplos de Uso](#exemplos-de-uso)
11. [Troubleshooting](#troubleshooting)

---

## Visão Geral

**Descrição clara e objetiva** do que o arquivo/funcionalidade faz.

### Características Principais
- ✅ **Funcionalidade 1**: Descrição
- ✅ **Funcionalidade 2**: Descrição
- ✅ **Funcionalidade 3**: Descrição

### Objetivo
Explicar em linguagem simples qual problema resolve.

---

## Arquitetura

### Tecnologias Utilizadas
| Tecnologia | Versão | Uso |
|------------|--------|-----|
| ASP.NET Core | 3.1+ | Backend |
| jQuery | 3.x | Manipulação DOM |
| Syncfusion EJ2 | - | Componentes UI |

### Padrões de Design
- Repository Pattern
- Dependency Injection
- MVVM / MVC

---

## Estrutura de Arquivos

### Arquivo Principal
```
Caminho/Completo/Do/Arquivo.cs
```

### Arquivos Relacionados
- `ArquivoRelacionado1.cs` - Descrição da relação
- `ArquivoRelacionado2.js` - Descrição da relação

---

## Integrações

### ⚠️ SEÇÃO OBRIGATÓRIA E CRÍTICA

**Controllers**:
- `/api/Viagem/AdicionarRequisitante` (POST)
  - Request: `{ Nome, Ponto, Ramal, Email, SetorSolicitanteId }`
  - Response: `{ success: bool, message: string, requisitanteid: Guid }`
  - Usado em: `salvarNovoRequisitante()` linha 1150

**Helpers**:
- `Alerta.TratamentoErroComLinha(arquivo, metodo, erro)`
  - Usado em: Todos os blocos catch
  - Localização: `Helpers/Alerta.cs`

**JavaScript Externos**:
- `wwwroot/js/frotix.js`
  - Função: `inicializarTooltips()`
  - Usado em: `document.ready` linha 50

**CSS Externos**:
- `wwwroot/css/frotix.css`
  - Classes: `btn-azul`, `btn-header-orange`, `tooltip-ftx-azul`
  - Usado em: Botões de ação e tooltips

**Repository**:
- `IUnitOfWork.RegistroCupomAbastecimento`
  - Métodos: `GetFirstOrDefault()`, `Add()`, `Update()`
  - Entidade: `RegistroCupomAbastecimento`

**Banco de Dados**:
- Tabela: `RegistroCupomAbastecimento`
  - Trigger: `tr_RegistroCupom_AfterInsert`
  - Stored Procedure: Nenhuma

**Componentes de Terceiros**:
- Syncfusion DropDownList
  - ID: `lstRequisitante`
  - Configuração: Linha 200 do CSHTML
  - DataSource: ViewData["Requisitantes"]

**Sistemas Globais FrotiX**:
- `Alerta.Confirmar()` - Confirmação de exclusão
- `AppToast.show()` - Notificações de sucesso/erro
- `ftx-spin-overlay` - Loading overlay

---

## Lógica de Negócio

[... resto da documentação ...]

---

# PARTE 2: LOG DE MODIFICAÇÕES/CORREÇÕES

> **FORMATO**: Entradas em ordem **decrescente** (mais recente primeiro)

---

## [13/01/2026 14:30] - Título da Modificação

**Descrição**: O que foi alterado e por quê

**Arquivos Afetados**:
- `Arquivo1.cs` (linhas X-Y)
- `Arquivo2.js` (linha Z)

**Impacto**: O que isso afeta no sistema

**Integrações Afetadas**:
- Controller `/api/Viagem` - Atualizado endpoint
- Helper `Alerta` - Nova função adicionada
- JavaScript `frotix.js` - Atualizada inicialização

**Status**: ✅ **Concluído**

**Responsável**: Claude Sonnet 4.5

**Versão**: X.X

---
```

### Tamanho Mínimo

- **Arquivos Simples**: **500+ linhas**
- **Arquivos Complexos**: **1000+ linhas**
- **Razor Pages (par CSHTML+CS)**: **1500+ linhas**

---

# Estrutura de Arquivos HTML (Portfólio A4)

## 🎨 Objetivo

Produzir documentação HTML visualmente rica para apresentação executiva, impressão e PDF.

## 📐 Formato A4

- **Tamanho**: 210mm x 297mm
- **Margens**: 16mm
- **Orientação**: Retrato (Portrait)
- **Paginação**: Se exceder 1 página, usar sufixos `A401`, `A402`, etc.

## 🎨 Identidade Visual Obrigatória

### Paleta de Cores FrotiX

```css
:root {
  /* Cores Principais */
  --vinho: #722F37;           /* Cancelar/Fechar, btn-ftx-fechar */
  --vinho-light: #8B3A44;

  --azul: #325d88;            /* Primário, btn-azul, tooltips */
  --azul-light: #3d6f9e;

  --terracota: #A97B6E;       /* Headers suaves, cards */
  --terracota-light: #C08B7E;

  --verde: #557570;           /* Estados OK */
  --verde-light: #6A8A85;

  --laranja-header: #b66a3d;  /* Header principal, btn-header-orange */

  /* Utilitários */
  --cinza: #f5f7fb;           /* Fundo geral */
  --card: #ffffff;            /* Fundo de cards */
  --texto: #1f1f1f;           /* Texto principal */
  --code-bg: #33465c;         /* Code snippets (azul petróleo 20% mais escuro) */

  --shadow: 0 20px 45px -18px rgba(0,0,0,.35);
  --radius: 14px;
}
```

### Header Padrão

```html
<header class="hero">
  <svg class="icon" aria-hidden="true" viewBox="0 0 640 640">
    <!-- FontAwesome Duotone SVG inline -->
  </svg>
  <div>
    <h1>Título da Página</h1>
    <p class="subtitle">Subtítulo descritivo</p>
  </div>
</header>
```

**Estilo do Header**:
- Fundo: `#b66a3d` (laranja FrotiX)
- Fonte: Branca, bold
- Bordas: `box-shadow: 0 0 0 1px #000, 0 0 0 4px #fff, var(--shadow);`
  - 1px preta fina
  - 4px branca grossa
  - Sombra externa

### Cards

```html
<section class="card">
  <div class="section-title">
    <img class="icon" src="../../Fontawesome/duotone/[icone].svg" />
    Título da Seção
  </div>
  <!-- Conteúdo -->
</section>
```

**Estilo dos Cards**:
- Fundo: `#ffffff`
- Bordas: `border-radius: 14px`
- Sombra: `var(--shadow)`
- Border: `1px solid rgba(0,0,0,0.05)`

### Code Snippets

```html
<pre><code>
// Código aqui
</code></pre>
```

**Estilo dos Snippets**:
- Fundo: `#33465c` (azul petróleo ~20% mais escuro que padrão)
- Texto: `#e9edf5` (branco/cinza claro)
- Padding: `10px 12px`
- Border-radius: `10px`
- Font-size: `12px`
- Line-height: `1.45`
- White-space: `pre-wrap`

### Ícones FontAwesome

**Sempre inline (SVG)** para exportar PDF corretamente.

**Localização**: `../../Fontawesome/duotone/[nome-icone].svg`

**Cores Padrão Duotone**:
- Primária: `#ff6b35` (laranja forte)
- Secundária: `#6c757d` (cinza médio)

**Uso**: Headers, section-titles, cards, listas, badges

### Botões e Badges

```html
<div class="pill">Tag 1</div>
<div class="badge">STATUS</div>
<span class="status status-ok">ATIVO</span>
<span class="status status-warn">PENDENTE</span>
<span class="status status-bad">ERRO</span>
```

### Tipografia

- **Fonte**: `"Segoe UI", "Inter", system-ui, -apple-system, sans-serif`
- **Não usar fontes externas** (Google Fonts, etc.)
- **Títulos**: Bold, letter-spacing 0.2px
- **Corpo**: Regular, line-height 1.6

## 📝 Conteúdo Mínimo por Página HTML

1. **Contexto e Propósito**: O que é e por que existe
2. **Interdependências**: Quem usa, onde, como se conecta
3. **Principais Métodos/Endpoints/Entidades**: Fluxo resumido
4. **Padrões de Inicialização**: Controles, AJAX, eventos
5. **Erros e Cuidados**: Try-catch, validações, edge cases

### Tom e Estilo

- **Prosa leve**: Narrativo, mas preciso
- **Termos técnicos**: Usar naturalmente
- **Snippets**: Explicar linha por linha quando relevante
- **Objetivo**: Compreensível para leigos em TI e técnicos

## 🔄 Nomenclatura de Arquivos HTML

### Padrão Base

```
(Diretorio) NomeA4XX.html
```

### Exemplos

```
(Controllers) HomeControllerA401.html
(Controllers) HomeControllerA402.html  ← Se precisar de mais páginas

(Pages) Usuarios - UpsertA401.html
(Pages) Abastecimento - DashboardA401.html

(JavaScript) frotix.jsA401.html
```

### Regras

- **Prefixo**: Nome do diretório entre parênteses
- **Nome**: Nome real do arquivo (sem inventar)
- **Sufixo**: `A4` + número sequencial `01`, `02`, etc.
- **Sempre mesma pasta do MD**: HTML e MD no mesmo diretório

---

# Paleta de Cores e Identidade Visual

## 🎨 Cores Principais

| Cor | Hex | Uso | Variante Light |
|-----|-----|-----|----------------|
| **Vinho** | `#722F37` | Cancelar/Fechar, btn-ftx-fechar | `#8B3A44` |
| **Azul** | `#325d88` | Primário, btn-azul, tooltips | `#3d6f9e` |
| **Terracota** | `#A97B6E` | Headers suaves, cards secundários | `#C08B7E` |
| **Verde** | `#557570` | Estados OK, sucesso | `#6A8A85` |
| **Laranja Header** | `#b66a3d` | Header principal, btn-header-orange | `#C67750` |
| **Azul Petróleo Code** | `#33465c` | Code snippets (20% mais escuro) | - |

## 🎨 Cores Utilitárias

| Cor | Hex | Uso |
|-----|-----|-----|
| **Cinza Claro** | `#f5f7fb` | Fundo geral da página |
| **Branco** | `#ffffff` | Cards, modais |
| **Texto** | `#1f1f1f` | Texto principal |
| **Preto** | `#000000` | Bordas finas |

## 🖌️ Gradientes e Sombras

```css
/* Fundo da página */
background: radial-gradient(circle at 12% 20%, rgba(50,93,136,.08), transparent 26%),
            radial-gradient(circle at 90% 10%, rgba(114,47,55,.10), transparent 30%),
            var(--cinza);

/* Sombra padrão de cards */
box-shadow: 0 20px 45px -18px rgba(0,0,0,.35);

/* Borda dupla (header) */
box-shadow: 0 0 0 1px #000,      /* Preta fina */
            0 0 0 4px #fff,      /* Branca grossa */
            var(--shadow);       /* Sombra externa */
```

---

# Padrões de Código FrotiX

## 🚨 Alertas e Notificações

### Sistema de Alertas SweetAlert (Obrigatório)

**SEMPRE** usar o sistema customizado de alertas. **NUNCA** usar `alert()`, `confirm()`, `prompt()` nativos.

**Funções Disponíveis**:

```javascript
// Confirmação
Alerta.Confirmar(titulo, texto, confirm, cancel)
  .then(result => {
    if (result) {
      // Usuário confirmou
    } else {
      // Usuário cancelou
    }
  });

// Erro
Alerta.Erro(titulo, texto, confirm);

// Sucesso
Alerta.Sucesso(titulo, texto, confirm);

// Aviso
Alerta.Warning(titulo, texto, confirm);

// Informação
Alerta.Info(titulo, texto, confirm);

// Tratamento de erro com linha
Alerta.TratamentoErroComLinha(arquivo, metodo, erro);
```

### Toasts

```javascript
// AppToast (padrão FrotiX)
AppToast.show('Verde', 'Mensagem de sucesso');
AppToast.show('Vermelho', 'Mensagem de erro');
AppToast.show('Amarelo', 'Mensagem de aviso');
AppToast.show('Azul', 'Mensagem de informação');

// Toastr (fallback)
toastr.success('Mensagem');
toastr.error('Mensagem');
toastr.warning('Mensagem');
toastr.info('Mensagem');
```

## 🛡️ Try-Catch Obrigatório

**TODAS** as funções JavaScript e C# devem ter blocos try-catch.

### JavaScript

```javascript
function minhaFuncao() {
    try {
        // código
    } catch (erro) {
        Alerta.TratamentoErroComLinha("meuArquivo.js", "minhaFuncao", erro);
    }
}
```

### C#

```csharp
public IActionResult MinhaAction() {
    try {
        // código
    } catch (Exception error) {
        Alerta.TratamentoErroComLinha("MeuController.cs", "MinhaAction", error);
        return Json(new { success = false, message = error.Message });
    }
}
```

## 🎨 Ícones FontAwesome

**SEMPRE** usar estilo **Duotone**:

```html
<!-- Correto -->
<i class="fa-duotone fa-home"></i>
<i class="fa-duotone fa-car"></i>

<!-- Incorreto -->
<i class="fa-regular fa-home"></i>
<i class="fa-solid fa-car"></i>
```

**Cores Padrão**:

```css
.fa-duotone {
  --fa-primary-color: #ff6b35;    /* Laranja forte */
  --fa-secondary-color: #6c757d;  /* Cinza médio */
}
```

## 🔘 Botões Padrão FrotiX

### Botão Header (Novo/Voltar)

```html
<button class="btn btn-header-orange">
  <i class="fa-duotone fa-plus icon-space"></i>
  Novo Registro
</button>

<button class="btn btn-header-orange">
  <i class="fa-duotone fa-rotate-left icon-space icon-rotate-left"></i>
  Voltar à Lista
</button>
```

### Botão Primário (Salvar/Criar/Atualizar)

```html
<button class="btn btn-azul btn-submit-spin">
  <i class="fa-duotone fa-floppy-disk icon-space icon-pulse"></i>
  Criar Registro
</button>

<button class="btn btn-azul btn-submit-spin">
  <i class="fa-duotone fa-floppy-disk icon-space icon-pulse"></i>
  Atualizar Registro
</button>
```

### Botão Cancelar

```html
<button class="btn btn-ftx-fechar">
  <i class="fa-duotone fa-circle-xmark icon-space icon-pulse"></i>
  Cancelar Operação
</button>
```

### Botão Voltar (Rodapé)

```html
<button class="btn btn-voltar">
  <i class="fa-duotone fa-rotate-left icon-space icon-rotate-left"></i>
  Voltar
</button>
```

## 📱 Modais Bootstrap

### Modal Empilhado (Stacked)

Para abrir modal filho ACIMA de modal pai:

```javascript
const modalInstance = new bootstrap.Modal(modalElement, {
    backdrop: false,  // NÃO cobrir modal pai
    keyboard: true
});
modalInstance.show();
```

### Modal de Loading (Espera)

```javascript
// Mostrar
const loadingHtml = `
  <div id="meu-loading-overlay" class="ftx-spin-overlay" style="z-index: 999999; cursor: wait;">
    <div class="ftx-spin-box" style="text-align: center; min-width: 300px;">
      <img src="/images/logo_gota_frotix_transparente.png" alt="FrotiX" class="ftx-loading-logo" />
      <div class="ftx-loading-bar"></div>
      <div class="ftx-loading-text">Carregando...</div>
      <div class="ftx-loading-subtext">Aguarde, por favor</div>
    </div>
  </div>
`;
$('body').append(loadingHtml);

// Esconder
$('#meu-loading-overlay').fadeOut(300, function() { $(this).remove(); });
```

**IMPORTANTE**: Logo **DEVE SEMPRE PULSAR** (classe `ftx-loading-logo` já contém animação).

## 🎯 Tooltips Padrão FrotiX

**SEMPRE** usar a classe `tooltip-ftx-azul`:

```html
<button data-bs-toggle="tooltip"
        data-bs-custom-class="tooltip-ftx-azul"
        data-bs-placement="top"
        title="Texto da tooltip">
    Botão
</button>
```

**JavaScript**:

```javascript
new bootstrap.Tooltip(elemento, {
    customClass: 'tooltip-ftx-azul'
});
```

---

# Workflow de Documentação

## 📋 Processo Completo

### 1. Identificação da Mudança

Quando **QUALQUER** arquivo de código for alterado:

```
Arquivos Monitorados:
- *.cshtml
- *.cshtml.cs
- *.cs (Controllers, Helpers, Services, Repository, Data, Models)
- *.js
- *.css
```

### 2. Localização da Documentação

**Padrão de Mapeamento**:

```
Arquivo de Código                    → Documentação
─────────────────────────────────────────────────────────
Pages/Abastecimento/UpsertCupons.cshtml
Pages/Abastecimento/UpsertCupons.cshtml.cs
                                     → Documentacao/Pages/Abastecimento/UpsertCupons.md
                                       Documentacao/Pages/Abastecimento/UpsertCupons.html

Controllers/ViagemController.cs      → Documentacao/Controllers/ViagemController.md
                                       Documentacao/Controllers/ViagemController.html

wwwroot/js/frotix.js                 → Documentacao/JavaScript/frotix.md
                                       Documentacao/JavaScript/frotix.html

wwwroot/css/frotix.css               → Documentacao/CSS/frotix.md
                                       Documentacao/CSS/frotix.html

Helpers/Alerta.cs                    → Documentacao/Helpers/Alerta.md
                                       Documentacao/Helpers/Alerta.html
```

### 3. Verificação de Existência

```bash
# Se documentação NÃO existe
→ Criar nova documentação completa (MD + HTML)

# Se documentação existe
→ Atualizar:
  - Seção afetada da PARTE 1
  - Adicionar entrada na PARTE 2 (Log de Modificações)
  - Incrementar versão
  - Atualizar data
```

### 4. Criação/Atualização

#### Para Razor Pages (par CSHTML + CSHTML.CS):

1. **Ler ambos os arquivos** (`.cshtml` e `.cshtml.cs`)
2. **Mapear todas as integrações**:
   - Controllers chamados
   - Helpers usados
   - JavaScript referenciados
   - CSS aplicados
   - Repository/Services injetados
   - Models/ViewModels
   - Banco de dados (tabelas, triggers, SPs)
   - Componentes de terceiros
   - Sistemas globais FrotiX
3. **Criar diagrama de interdependências**
4. **Documentar fluxo completo de dados**
5. **Escrever arquivo MD único** (mínimo 1500 linhas)
6. **Gerar arquivo HTML** correspondente (formato A4)

#### Para Outros Arquivos:

1. **Ler arquivo de código**
2. **Mapear todas as integrações**
3. **Criar diagrama de interdependências**
4. **Escrever arquivo MD** (mínimo 500-1000 linhas)
5. **Gerar arquivo HTML** correspondente (formato A4)

### 5. Validação

Antes de commitar:

```bash
# Executar script de validação
powershell.exe -ExecutionPolicy Bypass -File "Scripts/ValidarDocumentacao.ps1" -PreCommit

# Verificar:
- [ ] Documentação existe?
- [ ] Timestamp atualizado (< 5 min diferença)?
- [ ] PARTE 2 (Log) atualizada?
- [ ] Versão incrementada?
- [ ] Seção de Integrações completa?
```

### 6. Commit

```bash
git add [arquivo-codigo] [arquivo.md] [arquivo.html]
git commit -m "docs: Atualiza documentação de [Nome] - [Breve descrição]"
git push
```

## 🔄 Atualização de Índice Geral

Após criar nova documentação:

1. Abrir `Documentacao/0-INDICE-GERAL.md`
2. Adicionar entrada na seção apropriada
3. Manter ordem alfabética
4. Incluir link relativo
5. Commitar índice junto

---

# Troubleshooting e Validação

## ⚠️ Problemas Comuns

### 1. Commit Bloqueado - "SEM DOCUMENTAÇÃO"

**Sintoma**: Pre-commit hook bloqueia com mensagem "arquivo sem documentação".

**Causas**:
- Documentação não existe
- Documentação em local errado
- Nome de arquivo incorreto

**Solução**:
1. Verificar padrão de nomenclatura:
   - Razor Pages: `UpsertCupons.md` (não `UpsertCupons.cshtml.cs.md`)
   - Controllers: `ViagemController.md`
   - JavaScript: Nome do arquivo (ex: `frotix.md`)
2. Verificar diretório correto
3. Criar documentação se não existir

---

### 2. Commit Bloqueado - "DOCUMENTAÇÃO DESATUALIZADA"

**Sintoma**: Pre-commit hook bloqueia com timestamp desatualizado.

**Causas**:
- Arquivo de código foi modificado DEPOIS da documentação
- Diferença de timestamp > 5 minutos

**Solução**:
1. Atualizar documentação com as mudanças
2. Adicionar entrada na PARTE 2 (Log)
3. Salvar arquivo (atualiza timestamp automaticamente)
4. Commitar novamente

---

### 3. Documentação Incompleta

**Sintoma**: Seção de Integrações vazia ou genérica.

**Problema**: Documentação não detalha todas as dependências.

**Solução**:
1. Ler o arquivo de código novamente
2. Usar checklist de integrações (Controllers, Helpers, JS, CSS, etc.)
3. Adicionar diagrama de interdependências
4. Documentar fluxo completo passo a passo

---

### 4. HTML Não Gera PDF Corretamente

**Sintoma**: Ao imprimir/exportar PDF, layout quebra ou ícones não aparecem.

**Causas**:
- Ícones externos (não inline)
- CSS com `@media print` inadequado
- Excede tamanho A4

**Solução**:
1. Garantir ícones FontAwesome inline (SVG)
2. Verificar `@page { size: A4; margin: 16mm; }`
3. Se exceder 1 página, dividir em `A401`, `A402`, etc.
4. Testar impressão antes de commitar

---

## ✅ Checklist Final Antes de Commitar

### Documentação MD

- [ ] Arquivo existe no diretório correto?
- [ ] Nome segue padrão (sem `.cshtml.cs` no nome)?
- [ ] **Seção de Integrações completa?** (Controllers, Helpers, JS, CSS, Repository, BD, etc.)
- [ ] Diagrama de interdependências presente?
- [ ] Fluxo de dados passo a passo documentado?
- [ ] PARTE 2 (Log) atualizada com nova entrada?
- [ ] Versão incrementada?
- [ ] Data atualizada?
- [ ] Mínimo de linhas atendido? (500/1000/1500 conforme tipo)

### Documentação HTML

- [ ] Arquivo HTML existe no mesmo diretório do MD?
- [ ] Nome segue padrão `(Diretorio) NomeA4XX.html`?
- [ ] Header laranja `#b66a3d` com borda dupla?
- [ ] Code snippets em `#33465c` (azul petróleo)?
- [ ] Ícones FontAwesome inline (SVG)?
- [ ] Cores seguem paleta FrotiX?
- [ ] Cabe em A4 (ou dividido em múltiplas páginas)?
- [ ] Testado para impressão/PDF?

### Código

- [ ] Try-catch em todas as funções?
- [ ] `Alerta.TratamentoErroComLinha()` nos catches?
- [ ] Alertas usam sistema SweetAlert (não `alert()` nativo)?
- [ ] Ícones FontAwesome Duotone (não regular/solid)?
- [ ] Comentário visual no topo apontando para documentação?

### Git

- [ ] Script de validação passou sem erros?
- [ ] Mensagem de commit descritiva?
- [ ] Arquivos staged: código + MD + HTML?
- [ ] Índice geral atualizado (se novo arquivo)?

---

## 🎯 Resumo Executivo

### Regras de Ouro

1. **PAR CSHTML+CS = 1 MD + 1 HTML**: Sempre documentar juntos
2. **INTEGRAÇÕES OBRIGATÓRIAS**: Controllers, Helpers, JS, CSS, Repository, BD, Componentes, Sistemas Globais
3. **DIAGRAMA DE INTERDEPENDÊNCIAS**: Sempre incluir
4. **FLUXO COMPLETO**: Documentar passo a passo (usuário → view → pagemodel → repository → banco)
5. **TIMESTAMP**: Documentação SEMPRE após código (< 5 min)
6. **PARTE 2 (LOG)**: Atualizar em TODA mudança
7. **HTML A4**: Mesmo diretório do MD, formato imprimível
8. **LER ESTE ARQUIVO**: Antes de CADA commit de documentação

---

**Última atualização**: 13/01/2026
**Versão**: 1.0
**Mantido por**: Claude Sonnet 4.5


---

# PARTE 2: LOG DE MODIFICAÃ‡Ã•ES/CORREÃ‡Ã•ES

> **FORMATO**: Entradas em ordem **decrescente** (mais recente primeiro)

---

## [19/01/2026] - AtualizaÃ§Ã£o: ImplementaÃ§Ã£o de MÃ©todos com Tracking Seletivo

**DescriÃ§Ã£o**: MigraÃ§Ã£o de chamadas .AsTracking() para novos mÃ©todos GetWithTracking() e GetFirstOrDefaultWithTracking() como parte da otimizaÃ§Ã£o de performance do Entity Framework Core.

**Arquivos Afetados**:
- Este arquivo (uso dos novos mÃ©todos do repositÃ³rio)
- Repository/IRepository/IRepository.cs (definiÃ§Ã£o dos novos mÃ©todos)
- Repository/Repository.cs (implementaÃ§Ã£o)
- RegrasDesenvolvimentoFrotiX.md (seÃ§Ã£o 4.2 - nova regra permanente)

**MudanÃ§as**:
- âŒ **ANTES**: _unitOfWork.Entity.AsTracking().Get(id) ou _unitOfWork.Entity.AsTracking().GetFirstOrDefault(filter)
- âœ… **AGORA**: _unitOfWork.Entity.GetWithTracking(id) ou _unitOfWork.Entity.GetFirstOrDefaultWithTracking(filter)

**Motivo**: 
- OtimizaÃ§Ã£o de memÃ³ria e performance
- Tracking seletivo (apenas quando necessÃ¡rio para Update/Delete)
- PadrÃ£o mais limpo e explÃ­cito
- Conformidade com nova regra permanente (RegrasDesenvolvimentoFrotiX.md seÃ§Ã£o 4.2)

**Impacto**: 
- Melhoria de performance em operaÃ§Ãµes de leitura (usa AsNoTracking por padrÃ£o)
- Tracking correto em operaÃ§Ãµes de escrita (Update/Delete)
- Zero impacto funcional (comportamento mantido)

**Status**: âœ… **ConcluÃ­do**

**ResponsÃ¡vel**: Sistema (AtualizaÃ§Ã£o AutomÃ¡tica)

**VersÃ£o**: Incremento de patch
