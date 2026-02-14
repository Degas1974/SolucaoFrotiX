# 📚 Guia de Enriquecimento de Documentação CSHTML

**Versão:** 2.0
**Data:** 03/02/2026
**Objetivo:** Padronizar enriquecimento de documentação de páginas Razor (.cshtml)

---

## 🎯 O que é Enriquecimento?

Enriquecimento de documentação é o processo de **adicionar metadata, cards de identificação, análise de scripts inline e rastreabilidade** aos arquivos CSHTML, criando uma **documentação técnica visual e rastreável** que facilita compreensão, manutenção e refatoração.

### Exemplo Visual

```
ANTES (Apenas código)
├── Pages/Abastecimento/Index.cshtml
│   └── 1340 linhas de código, sem documentação clara

DEPOIS (Com Enriquecimento)
├── Pages/Abastecimento/Index.cshtml
│   └── 1340 linhas com comentários visuais
└── Documentacao/Pages/Abastecimento - Index.md
    └── Card com análise completa, scripts, dependências, fluxo
```

---

## 📋 Estrutura de Um Card de Documentação

### Template Completo

```markdown
## 🔹 CARD: [Caminho do Arquivo]

### Identificação Rápida
- **Localização:** Pages/[Modulo]/[Arquivo].cshtml
- **Linhas Totais:** [N]
- **Tamanho:** [N] KB
- **Última Modificação:** DD/MM/YYYY HH:MM
- **Versão Compilada:** [v1.0]

### Visão Geral
[1-3 parágrafos explicando o propósito da página, funcionalidades principais]

### Estrutura do Arquivo
- `@page` → Rota de acesso
- `@model` → Classe de modelo
- `@using` statements → Imports necessários
- `@functions { OnGet() }` → Inicialização no servidor (ViewData)
- `@section HeadBlock` → CSS customizado
- `@section ScriptsBlock` → JavaScript externo ou inline
- HTML/Razor → Estrutura visual

### ViewData Carregada (OnGet)

| ViewData | Origem | Tipo | Uso |
|----------|--------|------|-----|
| lstVeiculos | ListaVeiculos helper | List<SelectListItem> | DropDown de veículos |
| lstCombustivel | ListaCombustivel helper | List<SelectListItem> | DropDown combustível |
| ... | ... | ... | ... |

### Scripts Inline Mapeados

#### [N]️⃣ [Nome da Função/Handler]
**Localização no Arquivo:** Linhas XXX-YYY (section ScriptBlock)
**Propósito:** [Descrição breve]
**Responsabilidade:**
- [Item 1]
- [Item 2]

**Assinatura:**
```javascript
function nomeFunc(param1, param2) {
    // Código-exemplo
}
```

**Chamadas AJAX:**
```
POST /api/[Controller]/[Action]
Parâmetros: { }
Resposta: { success: bool, data: object }
```

**Dependências Locais:** [alerta.js, sincfusion, ...]
**Status:** ✅ ACEITÁVEL / ⚠️ REQUER EXTRAÇÃO (>50 linhas) / 🔴 CRÍTICO (>800 linhas)
**Recomendação:**
- [ ] Se >50 linhas: Extrair para `~/js/[modulo]/[funcao].js`
- [ ] Adicionar documentação de função (JSDoc)
- [ ] Mapear todos os eventos (click, change, etc)

### CSS Customizado

**Localização:** section HeadBlock (Linhas XXX-YYY)
**Total:** [N] linhas
**Classes Principais:**
```css
.class-1 { ... } /* Propósito */
.class-2 { ... } /* Propósito */
```

**Recomendação:**
- [ ] Se >150 linhas: Mover para `~/css/[modulo]-[arquivo].css`

### Dependências Externas

```
ASP.NET Core
├── Razor Pages 8.0
├── Entity Framework Core
└── IUnitOfWork pattern

Frontend
├── Syncfusion EJ2 (DropDown, DataTable, Grid)
├── Bootstrap 5.3
├── jQuery 3.7
├── DataTables.js
├── Chart.js
├── alerta.js (SweetAlert2 wrapper)
├── AppToast.js
├── global-toast.js
└── Font Awesome 6 Duotone

Backend
├── [ListaHelper1]
├── [ListaHelper2]
└── [ControllerAPI]
```

### Fluxo de Dados (Rastreabilidade)

```
┌─────────────────────────────────────────┐
│ Usuário Clica no Botão "Filtrar"        │
└──────────────────┬──────────────────────┘
                   │
                   v
┌─────────────────────────────────────────┐
│ JavaScript: $("#btnFiltrar").click()    │
│ Validação local de dados                │
└──────────────────┬──────────────────────┘
                   │
                   v
┌─────────────────────────────────────────┐
│ AJAX: POST /api/[Controller]/[Action]   │
│ Payload: { filtro1, filtro2, ... }      │
└──────────────────┬──────────────────────┘
                   │
                   v
┌─────────────────────────────────────────┐
│ [Controller]Controller.cs                │
│ Processamento backend                   │
│ Acesso dados via IUnitOfWork            │
└──────────────────┬──────────────────────┘
                   │
                   v
┌─────────────────────────────────────────┐
│ JSON Response                            │
│ { success: true, data: [...] }          │
└──────────────────┬──────────────────────┘
                   │
                   v
┌─────────────────────────────────────────┐
│ JavaScript: dataTable.ajax.reload()     │
│ Modal atualiza com dados                │
│ Toast/Alert notifica usuário            │
└─────────────────────────────────────────┘
```

### Mapeamento de Eventos

| Evento | Seletor | Handler | Ação |
|--------|---------|---------|------|
| click | #btnFiltrar | fnomeFunc() | Chama API, recarrega DataTable |
| change | #ddlVeiculo | fnomeFunc() | Sincroniza com outro dropdown |
| shown.bs.modal | #modal1 | fnomeFunc() | Carrega dados ao abrir |

### APIs Mapeadas

**Origem:** [Controlador]

| Método | Rota | Parâmetros | Resposta | Status HTTP |
|--------|------|------------|----------|-------------|
| GET | /api/[Controller]/[Action] | [param1], [param2] | { data: [...] } | 200 OK / 404 |
| POST | /api/[Controller]/Salvar | { model } | { success: bool, id: guid } | 200 / 400 |
| PUT | /api/[Controller]/Atualizar | { id, dados } | { success: bool } | 200 / 404 |
| DELETE | /api/[Controller]/Deletar | { id } | { success: bool } | 200 / 404 |

### Observações Técnicas

- **Performance:** [N] registros carregados por página (paginação ativa)
- **Validação:** Frontend (HTML5 + JS) + Backend (DataAnnotations)
- **Segurança:** [AuthorizeAttribute], [ValidateAntiForgeryToken]
- **Tratamento de Erro:** Try-catch com Alerta.TratamentoErroComLinha
- **Acessibilidade:** Aria-labels presentes, WCAG 2.1 AA (esperado)

### Recomendações de Refatoração

- [ ] **Script Extraction:** Se JavaScript >800 linhas, mover para `~/js/[modulo]/[arquivo].js`
- [ ] **CSS Extraction:** Se CSS >200 linhas, mover para `~/css/[modulo]-[arquivo].css`
- [ ] **Modal Consolidation:** Se múltiplos modais, considerar arquivo compartilhado
- [ ] **API Documentation:** Adicionar documentação Swagger
- [ ] **Unit Tests:** Adicionar testes de validação frontend/backend
- [ ] **Performance:** Analisar tamanho de bundle, considerar lazy-loading

### Checklist de Qualidade

- [ ] Todas as funções JavaScript possuem try-catch
- [ ] Alerta.js está sendo usado para erros (nunca alert())
- [ ] Font Awesome Duotone (nunca fa-solid)
- [ ] ViewData inicializada com helpers corretos
- [ ] Modal usa Bootstrap 5 classes corretas
- [ ] DataTable configurado com locale pt-br
- [ ] Syncfusion components possuem placeholder e filtering
- [ ] Arquivo JS externo está documentado em comentário
- [ ] CSS customizado não duplica estilos globais (frotix.css)

### Histórico de Atualizações

| Data | Versão | Alteração | Por |
|------|--------|-----------|-----|
| 03/02/2026 | 1.0 | Criação do card | [Seu nome] |
| | | | |

### Links Relacionados

- **Controlador:** `/Controllers/[Modulo]/[NomeController].cs`
- **Model:** `/Models/[NomeModel].cs`
- **Helper:** `/Helpers/Lista[Tabela].cs`
- **API Endpoint:** Documentado acima em "APIs Mapeadas"
- **Testes:** `/Tests/[NomeController]Tests.cs`

---
```

### Notas sobre o Template

1. **Seções Obrigatórias:**
   - Identificação Rápida
   - Visão Geral
   - Estrutura do Arquivo
   - Scripts Inline Mapeados
   - Rastreabilidade/Fluxo de Dados

2. **Seções Opcionais (dependendo do arquivo):**
   - ViewData Carregada (se usar @functions OnGet)
   - CSS Customizado (se tiver @section HeadBlock)
   - Dependências Externas
   - Eventos Mapeados
   - APIs Mapeadas

3. **Ícones Padrão:**
   - ⚡ = Crítico/Importante
   - 🎯 = Objetivo
   - 📥 = Entrada/Parâmetros
   - 📤 = Saída/Resposta
   - 🔗 = Relacionamento/Link
   - 🔄 = Fluxo/Cycle
   - 📦 = Dependência
   - 📝 = Observação
   - ✅ = OK/Bom
   - ⚠️ = Aviso/Necessita Atenção
   - 🔴 = Crítico/Erro

---

## 🚀 Processo Passo-a-Passo

### Fase 1: Análise Inicial (15 min)

1. **Abrir o arquivo CSHTML**
   ```
   Pages/[Modulo]/[Arquivo].cshtml
   ```

2. **Contar linhas totais**
   ```
   wc -l Pages/[Modulo]/[Arquivo].cshtml
   ```

3. **Identificar estrutura principal**
   - Localizar `@page`
   - Localizar `@model`
   - Localizar `@using` statements
   - Localizar `@functions { OnGet() }`
   - Localizar `@section HeadBlock`
   - Localizar `@section ScriptsBlock`

4. **Extrair ViewData carregada**
   ```csharp
   ViewData["lstVeiculos"] = ...
   ViewData["lstMotorista"] = ...
   ```

### Fase 2: Análise de Scripts (20-30 min)

1. **Localizar todos os scripts**
   - Dentro de `@section ScriptsBlock`
   - Tags `<script>` isolados
   - `@Html.Raw(TempData["errojs"])`

2. **Para cada script, documentar:**
   - Linhas de início/fim
   - Nome da função
   - Propósito
   - Parâmetros de entrada
   - Valores de saída
   - Chamadas AJAX (URL, método, payload)
   - Dependências (alerta.js, jquery, syncfusion, etc)

3. **Avaliar tamanho:**
   - Se >50 linhas → Sugerir extração
   - Se >800 linhas → CRÍTICO, extrair imediatamente
   - Se duplica lógica em outros arquivos → Consolidar

### Fase 3: Mapeamento de Rastreabilidade (15 min)

1. **De onde é chamado?**
   - Rota Menu/Sidebar
   - Link direto
   - Redirecionamento

2. **O que ele chama?**
   - APIs (GET/POST/PUT/DELETE)
   - Controllers
   - Services

3. **Desenhar fluxo**
   - Usuário → Click → JS Handler → AJAX → Controller → DB → Response → DOM

### Fase 4: Criar Card de Documentação (30 min)

1. **Copiar template acima**
2. **Preencher todas as seções**
3. **Adicionar exemplos de código**
4. **Adicionar fluxograma visual**
5. **Salvar em:** `Documentacao/Pages/[Modulo] - [Arquivo].md`

### Fase 5: Validação e Commit (10 min)

1. **Verificar:**
   - Nenhum `@` dentro de comentários (exceto @page, @model)
   - Todos os links relativos corretos
   - Formatação Markdown correta

2. **Commit:**
   ```bash
   git add Documentacao/Pages/[Modulo]-[Arquivo].md
   git commit -m "docs: Enriquecimento CSHTML [Modulo]/[Arquivo]"
   git push
   ```

---

## ✅ Checklist de Enriquecimento

### Antes de Entregar

- [ ] Card criado com todas as seções obrigatórias
- [ ] Scripts inline mapeados com linhas de início/fim
- [ ] Scripts >50 linhas marcados com "REQUER EXTRAÇÃO"
- [ ] Scripts >800 linhas marcados com "CRÍTICO"
- [ ] Nenhum `@` dentro de comentários de script
- [ ] Fluxo de dados documentado com diagrama visual
- [ ] Todas as APIs mapeadas (GET/POST/PUT/DELETE)
- [ ] ViewData documentada em tabela
- [ ] Dependências externas listadas
- [ ] Recomendações de refatoração incluídas
- [ ] Checklist de qualidade preenchido
- [ ] Links para arquivos relacionados corretos
- [ ] Histórico de atualizações iniciado
- [ ] Arquivo salvo em local correto
- [ ] Commit realizado com mensagem clara

---

## 🔑 Regras Críticas

### ❌ NUNCA

1. **Usar @ dentro de comentários de script**
   ```javascript
   // ❌ ERRADO
   // Este bloco usa @Model.Propriedade para carregar dados

   // ✅ CORRETO
   // Este bloco usa Model.Propriedade para carregar dados
   ```

2. **Deixar script inline >50 linhas sem documentar extração**
   ```javascript
   // ⚠️ AVISO: Esta função possui 120 linhas
   // RECOMENDAÇÃO: Mover para ~/js/[modulo]/[funcao].js

   function muitoGrande() {
       // 120 linhas...
   }
   ```

3. **Misturar documentação de C# com JavaScript sem separação clara**
   ```
   ✅ CORRETO: Seção separada "Backend (C#)" e "Frontend (JavaScript)"
   ❌ ERRADO: Tudo misturado
   ```

### ✅ SEMPRE

1. **Documentar cada função JavaScript com:**
   - Linhas de início/fim
   - Parâmetros
   - Valor de retorno
   - Chamadas AJAX (se houver)
   - Eventos que a disparam

2. **Mapear fluxo visual com diagrama ASCII ou Mermaid:**
   ```
   Usuário → Click → Handler → AJAX → Controller → DB
   ```

3. **Listar todas as dependências externas:**
   ```
   - Syncfusion EJ2
   - jQuery
   - alerta.js
   - AppToast.js
   ```

4. **Usar tabelas para dados estruturados:**
   ```
   | ViewData | Tipo | Uso |
   |----------|------|-----|
   | ... | ... | ... |
   ```

---

## 📊 Exemplos Práticos

### Exemplo 1: Arquivo Pequeno (Simples)

**Arquivo:** Pages/Combustivel/Index.cshtml (600 linhas, 200 linhas JS)

```markdown
## 🔹 CARD: Pages/Combustivel/Index.cshtml

### Identificação Rápida
- **Localização:** Pages/Combustivel/Index.cshtml
- **Linhas Totais:** 600
- **Tamanho:** 25 KB
- **Última Modificação:** 02/02/2026

### Visão Geral
Página de listagem e gestão de tipos de combustível com DataTable, modal de criação/edição e integração com DropDowns Syncfusion.

### Scripts Inline Mapeados

#### 1️⃣ Inicialização DataTable
**Localização:** section ScriptBlock (Linhas 450-550)
**Propósito:** Carregar lista de combustíveis em DataTable com paginação
**Status:** ✅ ACEITÁVEL (100 linhas)

#### 2️⃣ Modal Handler
**Localização:** section ScriptBlock (Linhas 550-600)
**Propósito:** Salvar novo combustível via modal
**Status:** ✅ ACEITÁVEL (50 linhas)

### Recomendações
- [ ] CSS pode ficar inline (apenas 50 linhas)
- [ ] JavaScript pode ficar inline (apenas 150 linhas)
- [ ] Extrair helpers para ListaCombustivel se não existir
```

### Exemplo 2: Arquivo Médio (Complexo)

**Arquivo:** Pages/Abastecimento/Index.cshtml (1340 linhas, 800 linhas JS)

```markdown
## 🔹 CARD: Pages/Abastecimento/Index.cshtml

### Identificação Rápida
- **Localização:** Pages/Abastecimento/Index.cshtml
- **Linhas Totais:** 1340
- **Tamanho:** 48.9 KB
- **Última Modificação:** 02/02/2026 19:04

### Scripts Inline Mapeados

#### 1️⃣ dtCommonOptions()
**Localização:** section ScriptBlock (Linhas 750-800)
**Propósito:** Definir opções padrão DataTable
**Status:** ⚠️ REQUER EXTRAÇÃO
**Recomendação:** Mover para `~/js/cadastros/datatable-comum.js` (compartilhado entre múltiplas páginas)

#### 2️⃣ Filtros e Sincronização (400 linhas)
**Localização:** section ScriptBlock (Linhas 800-1200)
**Propósito:** Sincronizar Syncfusion DropDowns com DataTable
**Status:** ⚠️ REQUER EXTRAÇÃO (>50 linhas)
**Recomendação:** Mover para `~/js/abastecimento/index-filters.js`

### APIs Mapeadas
| Método | Rota | Status |
|--------|------|--------|
| GET | /api/Abastecimento/ListaAbastecimentos | 200 OK |
| POST | /api/Abastecimento/AtualizarKm | 200 OK |
| DELETE | /api/Abastecimento/DeletarAbastecimento | 204 No Content |
```

### Exemplo 3: Arquivo Grande (CRÍTICO)

**Arquivo:** Pages/Administracao/GestaoRecursosNavegacao.cshtml (5600 linhas, 2800 linhas JS)

```markdown
## 🔹 CARD: Pages/Administracao/GestaoRecursosNavegacao.cshtml

### Identificação Rápida
- **Localização:** Pages/Administracao/GestaoRecursosNavegacao.cshtml
- **Linhas Totais:** 5600
- **Tamanho:** 220 KB
- **Última Modificação:** 02/02/2026 18:32

### 🔴 AVISO: ARQUIVO CRÍTICO - REFATORAÇÃO URGENTE

**Problema:** 2800 linhas de JavaScript sem organização, impossível manter

### Estratégia de Refatoração

1. **Dividir em 5 arquivos:**
   - `gestao-menu.js` (módulo de menu principal)
   - `gestao-sidebar.js` (sidebar interativa)
   - `gestao-modais.js` (modais de gestão)
   - `gestao-validacao.js` (validações)
   - `gestao-eventos.js` (event handlers)

2. **Timeline:** 4-6 horas de refatoração
3. **Prioridade:** ALTA (impacta performance e manutenibilidade)
```

---

## 🎓 Boas Práticas

### 1. Documentar o Óbvio
```
✅ BOAS
- "Função carrega lista de veículos via AJAX para DropDown"
- "Modal abre ao clicar no botão #btnEditar"

❌ RUINS
- "Função faz coisas"
- "Há um modal aqui"
```

### 2. Usar Diagrama Visual
```
✅ BOAS
┌─── Usuário Clica ───┐
│                     │
└──→ Handler JS ──→ AJAX ──→ Controller
       │
       └──→ Validação
       └──→ Toast Notificação

❌ RUINS
"O usuário clica, depois há validação, depois há um ajax..."
```

### 3. Mapear Todas as Dependências
```
✅ BOAS
Dependências:
- Syncfusion EJ2 (DropDown, DataTable)
- jQuery 3.7
- alerta.js (SweetAlert2 wrapper)
- AppToast.js (toast notifications)

❌ RUINS
"O arquivo usa jQuery e alguns componentes"
```

### 4. Indicar Status de Cada Script
```
✅ BOAS
- ✅ ACEITÁVEL (50 linhas)
- ⚠️ REQUER EXTRAÇÃO (800 linhas)
- 🔴 CRÍTICO (2500 linhas - REFATORAR)

❌ RUINS
- "Script é ok"
- "Script precisa melhorias"
```

---

## 📚 Templates Prontos

Todos os templates abaixo podem ser copiados e preenchidos:

### Template Mínimo (5 min)
```markdown
## 🔹 CARD: [Arquivo]

### Identificação Rápida
- **Localização:** [path]
- **Linhas:** [N]

### Visão Geral
[2-3 linhas]

### Scripts Inline
1. [Nome] - [Propósito] - [Status]
2. [Nome] - [Propósito] - [Status]

### APIs
[Tabela com métodos]

### Recomendações
- [ ] [Ação 1]
- [ ] [Ação 2]
```

### Template Completo (30 min)
[Vide template acima com todas as seções]

---

## 🔗 Recursos Adicionais

- **Documentação Razor Pages:** https://docs.microsoft.com/en-us/aspnet/core/razor-pages
- **Syncfusion EJ2:** https://www.syncfusion.com/ej2-api-reference/
- **Bootstrap 5:** https://getbootstrap.com/
- **DataTables:** https://datatables.net/manual/
- **JavaScript JSDoc:** https://jsdoc.app/

---

## 💡 FAQ

**P: Quanto tempo leva documentar um arquivo?**
R: 30-60 minutos dependendo do tamanho (Pequeno: 30 min, Médio: 45 min, Grande: 60+ min)

**P: Posso deixar script inline se tiver menos de 50 linhas?**
R: Sim, está aceitável. >50 linhas = considerar extração.

**P: O que faço com arquivo de 5000+ linhas?**
R: Marque como CRÍTICO e recomende refatoração urgente.

**P: Preciso documentar CSS inline também?**
R: Não é obrigatório, mas é bom indicar se tem >150 linhas (sugerir extração).

**P: Como nomeio arquivo JS extraído?**
R: Padrão: `~/js/[modulo]/[funcionalidade].js`
Exemplo: `~/js/abastecimento/modal-editar.js`

---

**Guia Versão:** 2.0
**Última Atualização:** 03/02/2026
**Mantido por:** Sistema de Documentação FrotiX

