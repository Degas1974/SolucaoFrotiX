# 🚀 HANDOFF - MIGRAÇÃO SYNCFUSION → KENDO UI - PROJETO FROTIX

> **Documento de Continuação para IA**
> **Data:** 10/02/2026
> **Projeto:** FrotiX.Site.Fevereiro
> **Progresso Atual:** 2% (1/50 páginas 100% Kendo)
> **Branch:** main
> **Build Status:** ✅ 0 erros, 0 warnings

---

## 📋 ÍNDICE

1. [Contexto e Objetivo](#1-contexto-e-objetivo)
2. [Arquitetura e Stack Técnico](#2-arquitetura-e-stack-técnico)
3. [Regras Críticas (LEIA PRIMEIRO)](#3-regras-críticas-leia-primeiro)
4. [Trabalho Já Realizado](#4-trabalho-já-realizado)
5. [Template de Migração (Passo a Passo)](#5-template-de-migração-passo-a-passo)
6. [Exemplos Completos de Código](#6-exemplos-completos-de-código)
7. [Erros Comuns e Soluções](#7-erros-comuns-e-soluções)
8. [Próximos Passos (Priorização)](#8-próximos-passos-priorização)
9. [Checklist de Entrega](#9-checklist-de-entrega)
10. [Referências Rápidas](#10-referências-rápidas)

---

## 1. CONTEXTO E OBJETIVO

### 🎯 Missão Principal

Migrar **50 páginas Razor** do projeto FrotiX de **Syncfusion EJ2 TagHelpers** para **Kendo UI jQuery** (versão 2025.4.1321 local).

### 📊 Estado Atual

| Métrica | Valor | Detalhes |
|---------|-------|----------|
| **Total de Páginas** | 192 .cshtml | Projeto completo |
| **Páginas com Kendo/Syncfusion** | 63 (32.8%) | Identificadas na auditoria |
| **Páginas com Syncfusion** | 50 (26%) | **ALVO DE MIGRAÇÃO** |
| **Páginas 100% Kendo** | 1 (2%) | Viagens/Upsert ✅ |
| **Páginas Parciais** | 1 (18%) | Agenda/Index (2/13 controles) |
| **Páginas Pendentes** | 48 (96%) | Backlog |
| **Esforço Estimado** | 47-62h | Restante do projeto |

### 🚨 Por Que Esta Migração É Crítica

1. **Inputs HTML5 nativos** (`type="date"`, `type="time"`) **não aplicam** validação, formatação ou tema FrotiX
2. **Mistura Syncfusion + Kendo** aumenta bundle JavaScript em **~2MB**
3. **Inconsistência visual** entre controles afeta UX
4. **Chamadas de erro incorretas** causam falhas silenciosas
5. **Acesso via `.ej2_instances`** pode causar `TypeError` em produção

### 🎁 Benefícios Esperados

- ✅ Consistência visual total (tema Kendo uniforme)
- ✅ Redução de 30% no bundle JavaScript
- ✅ Eliminação de conflitos CSS Syncfusion vs Kendo
- ✅ Tratamento de erro uniforme e confiável
- ✅ Manutenção facilitada (um framework, não dois)

---

## 2. ARQUITETURA E STACK TÉCNICO

### 🏗️ Tecnologias

```
ASP.NET Core 8.0 (Razor Pages + MVC Controllers)
├── EF Core 8.0 (IUnitOfWork/Repository pattern)
├── SQL Server 2022 (database "Frotix", ~70 tabelas, ~40 views)
├── Kendo UI 2025.4.1321 (jQuery, local /wwwroot/lib/kendo/)
├── jQuery 3.7.1
├── Bootstrap 5.3.8
├── Font Awesome Duotone 6.x
├── SweetAlert2 (via Alerta.js)
└── SignalR, Hangfire, NPOI
```

### 📂 Estrutura de Diretórios

```
c:\FrotiX\Solucao FrotiX 2026\
├── FrotiX.Site.Fevereiro\           ← PROJETO ATIVO (trabalhe aqui)
│   ├── Pages\
│   │   ├── Viagens\
│   │   │   ├── Upsert.cshtml        ← ✅ TEMPLATE DE REFERÊNCIA (100% Kendo)
│   │   │   └── Index.cshtml         ← ⏭️ PRÓXIMO ALVO (3h)
│   │   ├── Agenda\
│   │   │   └── Index.cshtml         ← ⚠️ PARCIAL (18%, 11 controles pendentes)
│   │   ├── Abastecimento\
│   │   ├── Manutencao\
│   │   └── ... (45 páginas pendentes)
│   ├── wwwroot\
│   │   ├── lib\kendo\               ← Kendo UI 2025.4.1321 local
│   │   ├── css\
│   │   └── js\
│   │       └── alerta.js            ← Alerta.TratamentoErroComLinha()
│   └── FrotiX.csproj
├── RegrasDesenvolvimentoFrotiX.md   ← 📖 LEIA SEMPRE (regras oficiais)
├── ControlesKendo.md                ← 📖 LEIA SEMPRE (doc oficial Kendo)
└── CLAUDE.md                        ← Configuração do projeto (versão 5.0)
```

### 🔐 Database

- **Connection String (Dev):** `Data Source=localhost;Initial Catalog=Frotix;Trusted_Connection=True;`
- **Arquivo de Estrutura:** `FrotiX.Site.OLD\FrotiX.sql` (13.502 linhas, ler em chunks de 500 linhas)
- **Tabelas Críticas:** Viagem (~100 cols), Veiculo, Motorista, Abastecimento, Multa, Contrato

---

## 3. REGRAS CRÍTICAS (LEIA PRIMEIRO)

### ⚠️ PROTOCOLO DE INICIALIZAÇÃO

**ANTES DE QUALQUER AÇÃO, VOCÊ DEVE:**

1. ✅ Ler completamente: `c:\FrotiX\Solucao FrotiX 2026\RegrasDesenvolvimentoFrotiX.md`
2. ✅ Ler completamente: `c:\FrotiX\Solucao FrotiX 2026\ControlesKendo.md`
3. ✅ Se trabalhar com banco: Ler `FrotiX.Site.OLD\FrotiX.sql` (em chunks de 500 linhas)
4. ✅ Confirmar mentalmente que todos foram lidos

### 🚫 REGRAS CRÍTICAS KENDO UI (NUNCA VIOLE)

| ❌ NUNCA | ✅ SEMPRE |
|---------|-----------|
| `<kendo-datepicker>` TagHelper | `$("#id").kendoDatePicker({})` jQuery init |
| `<ejs-combobox>` Syncfusion | `$("#id").kendoComboBox({})` |
| `type="date"` HTML5 input | Kendo DatePicker com `format: "dd/MM/yyyy"` |
| `type="time"` HTML5 input | Kendo TimePicker com `format: "HH:mm"` |
| `.ej2_instances[0]` acesso | `$("#id").data("kendoWidget")` acesso |
| `alert()` JavaScript nativo | `Alerta.*` (SweetAlert2) |
| `TratamentoErroComLinha()` sem prefixo | `Alerta.TratamentoErroComLinha(arquivo, metodo, erro)` |
| `fa-solid` ícones | `fa-duotone` (padrão FrotiX) |
| Spinner Bootstrap | `FtxSpin.show()` / `FtxSpin.hide()` |
| Tooltip Bootstrap | Syncfusion `data-ejtip` |

### 📜 PADRÕES OBRIGATÓRIOS

#### Try-Catch (TODAS as funções)

```javascript
// ✅ CORRETO - SEMPRE fazer assim
function minhaFuncao() {
    try {
        // lógica
    } catch (error) {
        Alerta.TratamentoErroComLinha("NomeArquivo.cshtml", "minhaFuncao", error);
    }
}
```

```csharp
// ✅ CORRETO - C# também
public async Task<IActionResult> OnPostAsync()
{
    try
    {
        // lógica
    }
    catch (Exception ex)
    {
        _logService.LogError(ex, "NomeController", "OnPostAsync");
        return BadRequest();
    }
}
```

#### Cultura e Formatos

```javascript
// ✅ DatePicker - SEMPRE pt-BR, dd/MM/yyyy
$("#txtData").kendoDatePicker({
    format: "dd/MM/yyyy",
    culture: "pt-BR"
});

// ✅ TimePicker - SEMPRE HH:mm, interval 30
$("#txtHora").kendoTimePicker({
    format: "HH:mm",
    culture: "pt-BR",
    interval: 30
});
```

### 🔧 Git Workflow

```bash
# Branch padrão: main (SEMPRE push para main)
git add [arquivo]
git commit -m "refactor(kendo): [descrição curta]

[detalhes opcionais]

Co-Authored-By: Claude Sonnet 4.5 <noreply@anthropic.com>"
git push origin main
```

**Tipos de commit:** `feat:`, `fix:`, `refactor:`, `docs:`, `style:`, `chore:`

---

## 4. TRABALHO JÁ REALIZADO

### ✅ Commits Realizados (3 total)

#### 1. **c855636** - Viagens/Upsert 100% Kendo ✅
```bash
refactor(kendo): migra Viagens/Upsert de Syncfusion para Kendo UI 100%

FASE 1: Inputs HTML5 → Kendo (6 inputs)
- txtDataInicial, txtHoraInicial, txtDataFinal, txtHoraFinal
- txtDataInicialEvento, txtDataFinalEvento

FASE 2: Syncfusion → Kendo (7 controles)
- cmbMotorista (ComboBox com template foto)
- cmbVeiculo, cmbOrigem, cmbDestino (ComboBox)
- ddtEventos, ddlFinalidade, ddtSetor (DropDownList)

FASE 3: Correção erros (5 handlers)
- lstFinalidade_Change: TratamentoErroComLinha → Alerta.TratamentoErroComLinha
- MotoristaValueChange, VeiculoValueChange, RequisitanteValueChange

FASE 4: Simplificação try-catch (3 funções)

FASE 5: Remoção imports Syncfusion (4 linhas)

Página agora 100% jQuery init, 0% TagHelpers
Todos handlers acessam via .data("kendoWidget")
```

#### 2. **d8cbb3d** - Agenda/Index Parcial (18%) ⚠️
```bash
refactor(kendo): corrige inputs HTML5 type=time em Agenda/Index.cshtml

Migração PARCIAL (2/13 controles):
- txtHoraInicial, txtHoraFinal: HTML5 → Kendo TimePicker

PENDENTE: 11 controles Syncfusion (linhas 987-1486)
- lstFinalidade, cmbOrigem, cmbDestino, lstEventos
- lstMotorista, lstVeiculo, ddtCombustivelInicial, ddtCombustivelFinal
- lstSetorRequisitanteAgendamento, lstRecorrente, lstDiasMes
```

#### 3. **5428fc9** - Build Fixes + Node_modules ✅
```bash
fix(build): corrige erros de compilação e remove node_modules refs

1. FrotiX.csproj: Removidas 438 refs node_modules (sed -i '/node_modules/d')
2. DateTime.Hours/Minutes → Hour/Minute (4 fixes)
3. Nullable checks .HasValue → != null (4 fixes)
4. @@description escape em comentários Razor (2 fixes)
5. ComboBoxFieldSettings Syncfusion removido (1 fix)

Build: 438 erros → 0 erros ✅
```

### 📊 Auditoria Completa Realizada

**Top 10 Páginas Prioritárias (26-32 horas):**

1. ✅ **Viagens/Upsert** - 100% completo (6 inputs, 7 controles) - 3h gastas
2. ⏭️ **Viagens/Index** - Listagem principal - 3h estimadas
3. ⏭️ **Abastecimento/Index** - Operação diária crítica - 2-3h
4. ⏭️ **Multa/ListaAutuacao** - CRUD multas - 3h
5. ⏭️ **Manutencao/ListaManutencao** - CRUD manutenções - 3h
6. ⏭️ **Motorista/Index** - Gestão motoristas - 2-3h
7. ⏭️ **Veiculo/Index** - Gestão veículos - 2-3h
8. ⏭️ **Contrato/Index** - Contratos terceirizados - 2h
9. ⏭️ **Fornecedor/Index** - Cadastro fornecedores - 2h
10. ⏭️ **Escalas/ListaEscala** - Escalas de trabalho - 3-4h

**Outras 40 páginas:** 21-30h estimadas

---

## 5. TEMPLATE DE MIGRAÇÃO (PASSO A PASSO)

### 🔄 FASE 1: Inputs HTML5 → Kendo DatePicker/TimePicker

#### Passo 1.1: Identificar Inputs HTML5

```bash
# Buscar inputs com type="date" ou type="time"
grep -n 'type="date"' Pages/[Nome]/[Arquivo].cshtml
grep -n 'type="time"' Pages/[Nome]/[Arquivo].cshtml
```

#### Passo 1.2: Remover Atributo `type`

```html
<!-- ❌ ANTES -->
<input id="txtDataInicial" class="form-control"
       asp-for="Model.DataInicial" type="date" />

<!-- ✅ DEPOIS -->
<input id="txtDataInicial" class="form-control"
       asp-for="Model.DataInicial" />
```

#### Passo 1.3: Adicionar Inicialização jQuery no @section ScriptsBlock

**⚠️ IMPORTANTE:** Buscar no final do arquivo por `@section ScriptsBlock` ou criar se não existir.

```javascript
@section ScriptsBlock {
<script>
    /**
     * ═══════════════════════════════════════════════════════════════════════════
     * INICIALIZAÇÃO KENDO UI - DATE/TIME PICKERS
     * ═══════════════════════════════════════════════════════════════════════════
     * @@description Inicialização jQuery dos controles Kendo conforme ControlesKendo.md
     * IMPORTANTE: NUNCA usar TagHelpers <kendo-*> - sempre jQuery init
     */
    $(document).ready(function () {
        try {
            // DatePicker - Data Inicial
            $("#txtDataInicial").kendoDatePicker({
                format: "dd/MM/yyyy",
                culture: "pt-BR",
                value: @Html.Raw(Model.DataInicial != null
                    ? $"new Date('{Model.DataInicial.Value:yyyy-MM-dd}')"
                    : "null"),
                change: function(e) {
                    try {
                        // Lógica de change se necessário (ex: calcular duração)
                    } catch (error) {
                        Alerta.TratamentoErroComLinha("[NomeArquivo].cshtml", "txtDataInicial.change", error);
                    }
                }
            });

            // TimePicker - Hora Inicial
            $("#txtHoraInicial").kendoTimePicker({
                format: "HH:mm",
                culture: "pt-BR",
                interval: 30,
                value: @Html.Raw(Model.HoraInicial != null
                    ? $"new Date(2000, 0, 1, {Model.HoraInicial.Value.Hour}, {Model.HoraInicial.Value.Minute})"
                    : "null"),
                change: function(e) {
                    try {
                        // Lógica de change
                    } catch (error) {
                        Alerta.TratamentoErroComLinha("[NomeArquivo].cshtml", "txtHoraInicial.change", error);
                    }
                }
            });

        } catch (error) {
            Alerta.TratamentoErroComLinha("[NomeArquivo].cshtml", "kendo.init.datepickers", error);
        }
    });
</script>
}
```

**⚠️ ARMADILHAS COMUNS:**

1. **DateTime.Hours/Minutes NÃO EXISTE** → Use `.Hour` e `.Minute` (singular)
2. **`.HasValue` retorna bool?** → Use `!= null` em vez de `.HasValue` para evitar erros de compilação
3. **@description é Razor code** → Use `@@description` (double @) para escape

---

### 🔄 FASE 2: Syncfusion EJ2 → Kendo ComboBox/DropDownList

#### Passo 2.1: Identificar Controles Syncfusion

```bash
# Buscar TagHelpers Syncfusion
grep -n '<ejs-combobox' Pages/[Nome]/[Arquivo].cshtml
grep -n '<ejs-dropdownlist' Pages/[Nome]/[Arquivo].cshtml
grep -n '<ejs-dropdowntree' Pages/[Nome]/[Arquivo].cshtml
```

#### Passo 2.2: Substituir TagHelper por Input Simples

```html
<!-- ❌ ANTES (Syncfusion) -->
<ejs-combobox id="cmbMotorista"
    placeholder="Selecione um Motorista"
    ejs-for="@Model.MotoristaId"
    allowFiltering="true"
    filterType="Contains"
    popupHeight="200px"
    width="100%"
    showClearButton="true"
    dataSource="@ViewData["dataMotorista"]"
    created="onCmbMotoristaCreated"
    change="MotoristaValueChange">
    <e-combobox-fields text="Nome" value="MotoristaId"></e-combobox-fields>
</ejs-combobox>

<!-- ✅ DEPOIS (Input simples - Kendo vai transformar) -->
<input id="cmbMotorista"
       name="MotoristaId"
       style="width: 100%;" />
```

**⚠️ NOTA:** O `name` deve corresponder ao campo do Model para model binding funcionar.

#### Passo 2.3: Adicionar Inicialização jQuery

**ComboBox (permite digitação livre):**

```javascript
// ComboBox - Motorista (com template de foto)
var dataMotorista = @Html.Raw(Json.Serialize(ViewData["dataMotorista"]));
$("#cmbMotorista").kendoComboBox({
    dataTextField: "nome",           // Campo para exibição
    dataValueField: "motoristaId",   // Campo para value
    dataSource: dataMotorista,       // Array de objetos
    placeholder: "Selecione um Motorista",
    filter: "contains",              // Filtro ao digitar
    suggest: true,                   // Autocompletar ao digitar
    height: 200,                     // Altura do dropdown
    value: "@(Model.MotoristaId?.ToString() ?? "")",  // Valor inicial
    template: function(data) {
        // Template customizado (exemplo: com foto)
        var fotoUrl = data.foto || '/images/motorista-default.png';
        return '<div style="display: flex; align-items: center; gap: 10px;">' +
               '  <img src="' + kendo.htmlEncode(fotoUrl) + '" ' +
               '       style="width: 32px; height: 32px; border-radius: 50%; object-fit: cover;" />' +
               '  <span>' + kendo.htmlEncode(data.nome) + '</span>' +
               '</div>';
    },
    change: function(e) {
        try {
            // Lógica de change (chamar handler existente se houver)
            if (typeof MotoristaValueChange === 'function') {
                MotoristaValueChange();
            }
        } catch (error) {
            Alerta.TratamentoErroComLinha("[NomeArquivo].cshtml", "cmbMotorista.change", error);
        }
    }
});
```

**DropDownList (lista fixa, sem digitação livre):**

```javascript
// DropDownList - Finalidade
var dataFinalidade = @Html.Raw(Json.Serialize(ViewData["dataFinalidade"]));
$("#ddlFinalidade").kendoDropDownList({
    dataTextField: "descricao",
    dataValueField: "finalidadeId",
    dataSource: dataFinalidade,
    placeholder: "Selecione uma Finalidade...",
    filter: "contains",
    height: 200,
    value: "@(Model.FinalidadeId?.ToString() ?? "")",
    change: function(e) {
        try {
            if (typeof lstFinalidade_Change === 'function') {
                lstFinalidade_Change();
            }
        } catch (error) {
            Alerta.TratamentoErroComLinha("[NomeArquivo].cshtml", "ddlFinalidade.change", error);
        }
    }
});
```

**⚠️ DIFERENÇAS ComboBox vs DropDownList:**

| ComboBox | DropDownList |
|----------|--------------|
| Permite **digitação livre** | Apenas **seleção** da lista |
| `filter: "contains"` + `suggest: true` | `filter: "contains"` (opcional) |
| Use para: Origem/Destino, nomes | Use para: Status, categorias fixas |

#### Passo 2.4: Atualizar Handlers JavaScript

**Buscar por `.ej2_instances` no código JavaScript:**

```bash
grep -n 'ej2_instances' Pages/[Nome]/[Arquivo].cshtml
```

**Substituir acesso Syncfusion por Kendo:**

```javascript
// ❌ ANTES (Syncfusion)
function MotoristaValueChange() {
    try {
        try {
            const motoristaId = document.getElementById('cmbMotorista').ej2_instances[0]?.value;
            if (motoristaId) {
                // lógica
            }
        } catch (error) {
            TratamentoErroComLinha("Viagem_050", "MotoristaValueChange", error);
        }
    } catch (error) {
        Alerta.TratamentoErroComLinha("Upsert.cshtml", "MotoristaValueChange", error);
    }
}

// ✅ DEPOIS (Kendo) - try-catch simplificado também
function MotoristaValueChange() {
    try {
        const cmbMotorista = $("#cmbMotorista").data("kendoComboBox");
        const motoristaId = cmbMotorista ? cmbMotorista.value() : null;

        if (!motoristaId) return;

        // lógica
        const ddtSetor = $("#ddtSetor").data("kendoDropDownList");
        if (ddtSetor) {
            ddtSetor.enable(false);  // Kendo API
        }
    } catch (error) {
        Alerta.TratamentoErroComLinha("Upsert.cshtml", "MotoristaValueChange", error);
    }
}
```

**⚠️ KENDO API METHODS:**

```javascript
const widget = $("#id").data("kendoComboBox"); // ou kendoDropDownList

// Métodos principais:
widget.value()              // Get valor
widget.value("123")         // Set valor
widget.text()               // Get texto exibido
widget.enable(true/false)   // Habilitar/desabilitar
widget.readonly(true/false) // Readonly on/off
widget.dataItem()           // Get objeto completo selecionado
widget.select(index)        // Selecionar por index
```

---

### 🔄 FASE 3: Correção Chamadas de Erro

#### Passo 3.1: Buscar Chamadas Incorretas

```bash
# Buscar TratamentoErroComLinha SEM "Alerta." prefix
grep -n 'TratamentoErroComLinha' Pages/[Nome]/[Arquivo].cshtml | grep -v 'Alerta\.TratamentoErroComLinha'
```

#### Passo 3.2: Adicionar Prefixo `Alerta.`

```javascript
// ❌ ANTES
TratamentoErroComLinha("Viagem_050", "lstFinalidade_Change", error);

// ✅ DEPOIS
Alerta.TratamentoErroComLinha("Upsert.cshtml", "lstFinalidade_Change", error);
```

**⚠️ PADRÃO CORRETO:**
```javascript
Alerta.TratamentoErroComLinha(
    "[NomeArquivo].cshtml",  // Nome do arquivo
    "[nomeFuncao]",          // Nome da função
    error                     // Objeto error do catch
);
```

---

### 🔄 FASE 4: Simplificação Try-Catch Aninhados

#### Passo 4.1: Identificar Try-Catch Aninhados

```bash
# Buscar padrões de try-catch duplo
grep -A 5 'try {' Pages/[Nome]/[Arquivo].cshtml | grep -A 3 'try {'
```

#### Passo 4.2: Remover Inner Try-Catch

```javascript
// ❌ ANTES (try-catch aninhado redundante)
function lstFinalidade_Change() {
    try {
        try {
            const lstEvento = document.getElementById("ddtEventos")?.ej2_instances?.[0];
            if (lstEvento) {
                lstEvento.enabled = true;
            }
        } catch (error) {
            TratamentoErroComLinha("Viagem_050", "lstFinalidade_Change", error);
        }
    } catch (error) {
        Alerta.TratamentoErroComLinha("Upsert.cshtml", "lstFinalidade_Change", error);
    }
}

// ✅ DEPOIS (try-catch único)
function lstFinalidade_Change() {
    try {
        const ddl = $("#ddlFinalidade").data("kendoDropDownList");
        const finalidade = ddl ? ddl.value() : null;

        const ddtEventos = $("#ddtEventos").data("kendoDropDownList");
        if (finalidade === 'Evento' && ddtEventos) {
            ddtEventos.enable(true);
        }
    } catch (error) {
        Alerta.TratamentoErroComLinha("Upsert.cshtml", "lstFinalidade_Change", error);
    }
}
```

---

### 🔄 FASE 5: Remoção Imports Syncfusion

#### Passo 5.1: Verificar Se Ainda Há Controles Syncfusion

```bash
# Se retornar 0 linhas, pode remover imports
grep -c '<ejs-' Pages/[Nome]/[Arquivo].cshtml
```

#### Passo 5.2: Remover Imports (Se 0 Syncfusion)

```csharp
// ❌ REMOVER (se não houver mais <ejs-*>)
@using Syncfusion.EJ2.DropDowns;
@using Syncfusion.EJ2;
@using Syncfusion.Data;
@using Syncfusion.EJ2.DocumentEditor;

// ✅ MANTER apenas
@addTagHelper *, Microsoft.AspNetCore.Mvc.TagHelpers
@model [Namespace].[ModelClass]
```

---

### 🧪 FASE 6: Build e Validação

#### Passo 6.1: Build Test

```bash
cd "c:/FrotiX/Solucao FrotiX 2026/FrotiX.Site.Fevereiro"
dotnet build --no-incremental
```

**Se houver erros, consulte seção [7. Erros Comuns e Soluções](#7-erros-comuns-e-soluções)**

#### Passo 6.2: Validação Visual (Se Build OK)

```bash
dotnet run --environment Development
# Acessar: http://localhost:5000
```

**Checklist Visual:**
- [ ] DatePickers exibem calendário pt-BR ao clicar
- [ ] TimePickers exibem lista com intervalos de 30min
- [ ] ComboBox permite digitar e filtrar
- [ ] DropDownList exibe apenas seleção
- [ ] Templates customizados (foto, ícones) renderizam
- [ ] Validações funcionam (campos obrigatórios)
- [ ] Handlers `change` executam corretamente
- [ ] Nenhum erro no Console do navegador (F12)

#### Passo 6.3: Commit e Push

```bash
git add "FrotiX.Site.Fevereiro/Pages/[Nome]/[Arquivo].cshtml"
git commit -m "refactor(kendo): migra [Nome]/[Arquivo] de Syncfusion para Kendo UI 100%

FASE 1: Inputs HTML5 → Kendo ([N] inputs)
- [listar inputs convertidos]

FASE 2: Syncfusion → Kendo ([N] controles)
- [listar controles convertidos]

FASE 3: Correção erros ([N] handlers)
- [listar handlers corrigidos]

FASE 4: Simplificação try-catch ([N] funções)

FASE 5: Remoção imports Syncfusion

Página agora 100% jQuery init, 0% TagHelpers
Todos handlers acessam via .data('kendoWidget')

Co-Authored-By: Claude Sonnet 4.5 <noreply@anthropic.com>"

git push origin main
```

---

## 6. EXEMPLOS COMPLETOS DE CÓDIGO

### 📄 Exemplo 1: DatePicker com Valor Nullable

```javascript
// Model.DataViagem é DateTime? (nullable)
$("#txtDataViagem").kendoDatePicker({
    format: "dd/MM/yyyy",
    culture: "pt-BR",
    value: @Html.Raw(Model.DataViagem != null
        ? $"new Date('{Model.DataViagem.Value:yyyy-MM-dd}')"
        : "null"),
    change: function(e) {
        try {
            console.log("Data selecionada:", e.sender.value());
        } catch (error) {
            Alerta.TratamentoErroComLinha("Exemplo.cshtml", "txtDataViagem.change", error);
        }
    }
});
```

### 📄 Exemplo 2: TimePicker com Valor TimeSpan

```javascript
// Model.HoraInicio é TimeSpan? (nullable)
$("#txtHoraInicio").kendoTimePicker({
    format: "HH:mm",
    culture: "pt-BR",
    interval: 30,
    value: @Html.Raw(Model.HoraInicio != null
        ? $"new Date(2000, 0, 1, {Model.HoraInicio.Value.Hours}, {Model.HoraInicio.Value.Minutes})"
        : "null"),
    change: function(e) {
        try {
            const hora = e.sender.value();
            if (hora) {
                console.log("Hora:", hora.getHours() + ":" + hora.getMinutes());
            }
        } catch (error) {
            Alerta.TratamentoErroComLinha("Exemplo.cshtml", "txtHoraInicio.change", error);
        }
    }
});
```

**⚠️ ARMADILHA:** `TimeSpan` tem `Hours`, mas `DateTime` tem `Hour` (singular). Se Model for `DateTime`, use `.Hour` e `.Minute`.

### 📄 Exemplo 3: ComboBox com DataSource Vazio (Evitar Erro)

```javascript
// ViewData pode ser null - sempre validar
var dataOrigem = @Html.Raw(Json.Serialize(ViewData["ListaOrigem"] ?? new List<object>()));
$("#cmbOrigem").kendoComboBox({
    dataTextField: "descricao",
    dataValueField: "id",
    dataSource: dataOrigem,
    placeholder: "Selecione ou digite a Origem",
    filter: "contains",
    suggest: true,
    height: 220,
    value: "@(Model.OrigemId?.ToString() ?? "")",
    noDataTemplate: "<div style='padding: 10px;'>Nenhum registro encontrado</div>"
});
```

### 📄 Exemplo 4: DropDownList com Hierarquia Simples (Ex: Setores)

```javascript
// Se era DropDownTree (Syncfusion), simplificar para lista flat
var dataSetor = @Html.Raw(Json.Serialize(ViewData["dataSetor"]));
$("#ddtSetor").kendoDropDownList({
    dataTextField: "nome",
    dataValueField: "setorId",
    dataSource: dataSetor,
    placeholder: "Selecione um Setor...",
    optionLabel: "-- Selecione --",  // Opção "vazia" no topo
    filter: "contains",
    height: 250,
    value: "@(Model.SetorId?.ToString() ?? "")"
});
```

**⚠️ NOTA:** Se hierarquia real for necessária (pai/filho), considerar Kendo DropDownTree (mais complexo) - consulte ControlesKendo.md.

### 📄 Exemplo 5: Handler Change Chamando Função Existente

```javascript
// Se página tem função legacy MotoristaValueChange(), chamar dela
$("#cmbMotorista").kendoComboBox({
    // ... config
    change: function(e) {
        try {
            if (typeof MotoristaValueChange === 'function') {
                MotoristaValueChange();
            }
        } catch (error) {
            Alerta.TratamentoErroComLinha("Exemplo.cshtml", "cmbMotorista.change", error);
        }
    }
});

// Função legacy atualizada para Kendo API
function MotoristaValueChange() {
    try {
        const widget = $("#cmbMotorista").data("kendoComboBox");
        if (!widget) return;

        const motoristaId = widget.value();
        const motoristaNome = widget.text();

        console.log("Motorista selecionado:", motoristaId, motoristaNome);

        // Lógica adicional...
    } catch (error) {
        Alerta.TratamentoErroComLinha("Exemplo.cshtml", "MotoristaValueChange", error);
    }
}
```

### 📄 Exemplo 6: Template Complexo (Foto + Badge Status)

```javascript
$("#cmbVeiculo").kendoComboBox({
    dataTextField: "placa",
    dataValueField: "veiculoId",
    dataSource: @Html.Raw(Json.Serialize(ViewData["dataVeiculo"])),
    placeholder: "Selecione um Veículo",
    filter: "contains",
    height: 250,
    template: function(data) {
        var statusClass = data.status === 'Disponível' ? 'success' : 'danger';
        var foto = data.foto || '/images/veiculo-default.png';

        return '<div style="display: flex; align-items: center; gap: 10px;">' +
               '  <img src="' + kendo.htmlEncode(foto) + '" ' +
               '       style="width: 40px; height: 40px; border-radius: 4px; object-fit: cover;" />' +
               '  <div>' +
               '    <div style="font-weight: 600;">' + kendo.htmlEncode(data.placa) + '</div>' +
               '    <div style="font-size: 0.85em; color: #666;">' +
               '      <span class="badge bg-' + statusClass + '">' + kendo.htmlEncode(data.status) + '</span>' +
               '      ' + kendo.htmlEncode(data.modelo) +
               '    </div>' +
               '  </div>' +
               '</div>';
    },
    valueTemplate: function(data) {
        // Template para item selecionado (mais simples)
        return '<span>' + kendo.htmlEncode(data.placa) + ' - ' + kendo.htmlEncode(data.modelo) + '</span>';
    }
});
```

**⚠️ SEGURANÇA:** SEMPRE usar `kendo.htmlEncode()` em templates para evitar XSS.

---

## 7. ERROS COMUNS E SOLUÇÕES

### ❌ Erro 1: "kendo is not defined"

**Sintoma:**
```
Uncaught ReferenceError: kendo is not defined
    at HTMLDocument.<anonymous> (Upsert:2985)
```

**Causa:** TagHelper `<kendo-*>` usado em vez de jQuery init, OU scripts Kendo não carregados.

**Solução:**

1. **Verificar se Kendo está carregado** - No `_Layout.cshtml`:
```html
<script src="~/lib/kendo/js/kendo.all.min.js"></script>
<script src="~/lib/kendo/js/kendo.aspnetmvc.min.js"></script>
<script src="~/lib/kendo/js/cultures/kendo.culture.pt-BR.min.js"></script>
```

2. **NUNCA usar TagHelpers** - Substituir por jQuery init conforme templates acima.

---

### ❌ Erro 2: "DateTime não contém definição para Hours"

**Sintoma:**
```
error CS1061: 'DateTime' não contém uma definição para "Hours"
```

**Causa:** Propriedade errada - `DateTime` tem `Hour` (singular), não `Hours` (plural).

**Solução:**
```csharp
// ❌ ERRADO
new Date(2000, 0, 1, {Model.HoraInicio.Value.Hours}, {Model.HoraInicio.Value.Minutes})

// ✅ CORRETO
new Date(2000, 0, 1, {Model.HoraInicio.Value.Hour}, {Model.HoraInicio.Value.Minute})
```

**⚠️ NOTA:** Se `HoraInicio` for `TimeSpan`, aí usa `Hours` (plural). Verifique o tipo do Model.

---

### ❌ Erro 3: "Não é possível converter bool? em bool"

**Sintoma:**
```
error CS0266: Não é possível converter implicitamente tipo "bool?" em "bool"
```

**Causa:** Null-conditional operator `?.HasValue` retorna `bool?`, compilador espera `bool` puro.

**Solução:**
```csharp
// ❌ ERRADO
value: @Html.Raw(Model.DataInicial.HasValue ? ... : "null")

// ✅ CORRETO
value: @Html.Raw(Model.DataInicial != null ? ... : "null")
```

---

### ❌ Erro 4: "O nome 'description' não existe no contexto atual"

**Sintoma:**
```
error CS0103: O nome "description" não existe no contexto atual
```

**Causa:** Em Razor, `@description` é interpretado como código C#. Precisa escapar.

**Solução:**
```javascript
// ❌ ERRADO
/**
 * @description Inicialização Kendo
 */

// ✅ CORRETO
/**
 * @@description Inicialização Kendo
 */
```

---

### ❌ Erro 5: "ComboBoxFieldSettings não pode ser encontrado"

**Sintoma:**
```
error CS0246: O tipo 'ComboBoxFieldSettings' não pode ser encontrado
```

**Causa:** Código Syncfusion legado ainda presente no C# (PageModel).

**Solução:**

1. **No arquivo .cshtml.cs** (PageModel), buscar e remover:
```csharp
// ❌ REMOVER (Syncfusion legacy)
ViewData["fieldsMotorista"] = new ComboBoxFieldSettings
{
    Text = "Nome",
    Value = "MotoristaId"
};
```

2. **Kendo não precisa disso** - dataTextField/dataValueField são configurados no JavaScript.

---

### ❌ Erro 6: "Cannot read property 'ej2_instances' of null"

**Sintoma (Console do navegador):**
```
Uncaught TypeError: Cannot read property 'ej2_instances' of null
```

**Causa:** Handler JavaScript ainda usando Syncfusion API após migração.

**Solução:**

```javascript
// ❌ ERRADO (Syncfusion)
const valor = document.getElementById('cmbMotorista').ej2_instances[0]?.value;

// ✅ CORRETO (Kendo)
const widget = $("#cmbMotorista").data("kendoComboBox");
const valor = widget ? widget.value() : null;
```

**Buscar em todo o arquivo:**
```bash
grep -n 'ej2_instances' Pages/[Nome]/[Arquivo].cshtml
```

---

### ❌ Erro 7: "TratamentoErroComLinha is not defined"

**Sintoma (Console do navegador):**
```
Uncaught ReferenceError: TratamentoErroComLinha is not defined
```

**Causa:** Falta prefixo `Alerta.` na chamada.

**Solução:**
```javascript
// ❌ ERRADO
TratamentoErroComLinha("Arquivo", "funcao", error);

// ✅ CORRETO
Alerta.TratamentoErroComLinha("Arquivo.cshtml", "funcao", error);
```

**⚠️ VERIFICAR:** Se `alerta.js` está carregado no _Layout.cshtml:
```html
<script src="~/js/alerta.js"></script>
```

---

### ❌ Erro 8: Build - "No file exists for asset node_modules"

**Sintoma:**
```
error : System.InvalidOperationException: No file exists for the asset at either location
'C:\...\wwwroot\js\bs5-patcher\node_modules\@popperjs\core\package.json'
```

**Causa:** `FrotiX.csproj` tem referências `<Content Update>` para node_modules que não existem.

**Solução:**
```bash
cd "c:/FrotiX/Solucao FrotiX 2026/FrotiX.Site.Fevereiro"
sed -i '/node_modules/d' FrotiX.csproj
```

**⚠️ NOTA:** Isso remove TODAS as linhas com "node_modules" do .csproj. Se tiver node_modules legítimos, fazer manualmente.

---

### ❌ Erro 9: Kendo Widget Não Renderiza (Input Permanece Simples)

**Sintoma:** Input não vira DatePicker/ComboBox (continua input HTML simples).

**Diagnóstico (Console do navegador):**
```javascript
// Verificar se widget foi inicializado
console.log($("#txtDataInicial").data("kendoDatePicker")); // deve retornar objeto
```

**Causas Possíveis:**

1. **Script não executou** - Verificar se `@section ScriptsBlock` está no lugar certo (após `</html>`)
2. **jQuery não carregado** - Verificar ordem de scripts em _Layout.cshtml:
```html
<script src="~/lib/jquery/dist/jquery.min.js"></script>
<script src="~/lib/kendo/js/kendo.all.min.js"></script>
```
3. **ID incorreto** - Verificar se `id` do input corresponde ao `$("#id")` do jQuery
4. **Erro JavaScript silencioso** - Abrir Console (F12) e verificar erros

**Solução:** Revisar ordem de scripts e IDs.

---

## 8. PRÓXIMOS PASSOS (PRIORIZAÇÃO)

### 🎯 PASSO IMEDIATO (Alta Prioridade)

**COMPLETAR: Agenda/Index.cshtml - 11 controles Syncfusion restantes**

**Arquivos:** `c:\FrotiX\Solucao FrotiX 2026\FrotiX.Site.Fevereiro\Pages\Agenda\Index.cshtml`

**Status Atual:** 2/13 controles migrados (18%)

**Pendente:**

| # | ID | Tipo | Linha | Estimativa |
|---|---|------|-------|------------|
| 1 | `lstFinalidade` | DropDownList | 987 | 20min |
| 2 | `cmbOrigem` | ComboBox | 1017 | 15min |
| 3 | `cmbDestino` | ComboBox | 1042 | 15min |
| 4 | `lstEventos` | ComboBox | 1077 | 20min |
| 5 | `lstMotorista` | ComboBox | 1176 | 25min (template foto) |
| 6 | `lstVeiculo` | ComboBox | 1198 | 20min |
| 7 | `ddtCombustivelInicial` | DropDownList | 1272 | 15min |
| 8 | `ddtCombustivelFinal` | DropDownList | 1290 | 15min |
| 9 | `lstSetorRequisitanteAgendamento` | DropDownList | 1393 | 25min (hierarquia) |
| 10 | `lstRecorrente` | DropDownList | 1434 | 10min |
| 11 | `lstDiasMes` | DropDownList | 1486 | 10min |

**Tempo Total Estimado:** 3-4 horas

**Como Proceder:**

1. Abrir arquivo: `Pages\Agenda\Index.cshtml`
2. Para cada controle da tabela acima:
   - Substituir `<ejs-*>` por `<input>`
   - Adicionar jQuery init no @section ScriptsBlock (após linha 1945)
   - Atualizar handlers JavaScript se existirem
3. Build test após cada 3-4 controles
4. Commit quando todos 11 estiverem prontos

**Template de Commit:**
```bash
git commit -m "refactor(kendo): migra 11 controles Syncfusion para Kendo em Agenda/Index

Completa migração iniciada em commit d8cbb3d:
- lstFinalidade, cmbOrigem, cmbDestino: ComboBox/DropDownList
- lstEventos, lstMotorista, lstVeiculo: ComboBox com filtros
- ddtCombustivelInicial, ddtCombustivelFinal: DropDownList
- lstSetorRequisitanteAgendamento: DropDownList hierárquico
- lstRecorrente, lstDiasMes: DropDownList

Todos controles inicializados via jQuery no @section ScriptsBlock
Handlers JavaScript atualizados para usar .data('kendoWidget')
Imports Syncfusion removidos

Página agora 100% Kendo UI conforme RegrasDesenvolvimentoFrotiX.md

Co-Authored-By: Claude Sonnet 4.5 <noreply@anthropic.com>"
```

---

### 📅 ROADMAP (Ordem de Execução)

#### SPRINT 1: Top 5 Críticas (10-12h) - 10% do Projeto

| Prioridade | Página | Estimativa | Motivo |
|------------|--------|------------|--------|
| 1 | ✅ Viagens/Upsert | 3h (FEITO) | Template de referência |
| 2 | ⏭️ Agenda/Index | 4h (11 pendentes) | Agendamento diário |
| 3 | ⏭️ Viagens/Index | 3h | Listagem principal |
| 4 | ⏭️ Abastecimento/Index | 2-3h | Operação diária crítica |
| 5 | ⏭️ Multa/ListaAutuacao | 3h | Gestão multas frequente |

**Meta SPRINT 1:** 5/50 páginas = **10% completo**

---

#### SPRINT 2: Cadastros Principais (8-10h) - 20% do Projeto

| Prioridade | Página | Estimativa | Motivo |
|------------|--------|------------|--------|
| 6 | ⏭️ Manutencao/ListaManutencao | 3h | CRUD manutenções |
| 7 | ⏭️ Motorista/Index | 2-3h | Gestão motoristas |
| 8 | ⏭️ Veiculo/Index | 2-3h | Gestão veículos |
| 9 | ⏭️ Contrato/Index | 2h | Contratos terceirizados |
| 10 | ⏭️ Fornecedor/Index | 2h | Cadastro fornecedores |

**Meta SPRINT 2:** 10/50 páginas = **20% completo**

---

#### SPRINT 3: Multas e Escalas (10-12h) - 30% do Projeto

| Prioridade | Página | Estimativa |
|------------|--------|------------|
| 11-15 | Multa/* (5 páginas) | 8-10h |
| 16-17 | Escalas/* (2 páginas) | 4-5h |

**Meta SPRINT 3:** 17/50 páginas = **34% completo**

---

#### SPRINT 4: Restante (21-26h) - 100% do Projeto

| Grupo | Páginas | Estimativa |
|-------|---------|------------|
| Abastecimento | 3 páginas | 3-4h |
| Setores/Requisitantes | 5 páginas | 4-5h |
| Patrimônio | 3 páginas | 3-4h |
| Outras | 22 páginas | 11-13h |

**Meta SPRINT 4:** 50/50 páginas = **100% completo** 🎉

---

### 📋 TAREFAS PARALELAS (Baixa Prioridade)

**Estas podem ser feitas a qualquer momento, mas não bloqueiam migração:**

1. **Extrair CSS de ControleLavagem.cshtml** (2h)
   - Criar: `wwwroot/css/manutencao/controle-lavagem.css`
   - Copiar linhas 46-299
   - Substituir `<style>` por `<link>` no .cshtml

2. **Deletar Temp/Index.cshtml** (5min)
   ```bash
   git rm "FrotiX.Site.Fevereiro/Pages/Temp/Index.cshtml"
   git commit -m "chore: remove Temp/Index.cshtml (estrutura Razor inválida)"
   ```

3. **Auditoria FrotiX.Site.OLD** (1h)
   - Verificar se há páginas similares já migradas para referência

4. **Documentar Padrões** (1h)
   - Adicionar exemplos de migração em `RegrasDesenvolvimentoFrotiX.md`

---

## 9. CHECKLIST DE ENTREGA

**Use esta checklist para CADA página migrada:**

### ✅ PRÉ-MIGRAÇÃO

- [ ] Ler `RegrasDesenvolvimentoFrotiX.md` (se não leu hoje)
- [ ] Ler `ControlesKendo.md` (se não leu hoje)
- [ ] Abrir arquivo .cshtml completo
- [ ] Buscar todos `<ejs-*>` e `type="date"/"time"`
- [ ] Buscar handlers JavaScript com `.ej2_instances`

### ✅ DURANTE MIGRAÇÃO

#### FASE 1: Inputs HTML5
- [ ] Remover `type="date"` e `type="time"`
- [ ] Adicionar inicialização Kendo DatePicker/TimePicker
- [ ] Configurar `format`, `culture`, `value`, `change`
- [ ] Adicionar try-catch nos handlers `change`

#### FASE 2: Controles Syncfusion
- [ ] Substituir cada `<ejs-*>` por `<input>`
- [ ] Adicionar jQuery init para cada controle
- [ ] Configurar `dataTextField`, `dataValueField`, `dataSource`
- [ ] Adicionar templates se necessário (foto, status)
- [ ] Adicionar handlers `change` se necessário

#### FASE 3: Handlers JavaScript
- [ ] Buscar e substituir `.ej2_instances` por `.data("kendoWidget")`
- [ ] Adicionar prefixo `Alerta.` em `TratamentoErroComLinha`
- [ ] Verificar chamadas de métodos Kendo API (`.value()`, `.enable()`)

#### FASE 4: Try-Catch
- [ ] Simplificar try-catch aninhados (remover inner)
- [ ] Garantir que TODAS as funções têm try-catch

#### FASE 5: Limpeza
- [ ] Buscar e remover imports `@using Syncfusion.*` (se não houver mais `<ejs-*>`)
- [ ] Remover código comentado legado
- [ ] Verificar comentários estão corretos (`@@description`)

### ✅ VALIDAÇÃO

#### Build Test
- [ ] `cd "c:/FrotiX/Solucao FrotiX 2026/FrotiX.Site.Fevereiro"`
- [ ] `dotnet build --no-incremental`
- [ ] **0 Erros, 0 Warnings**

#### Visual Test (Se Build OK)
- [ ] `dotnet run --environment Development`
- [ ] Acessar página no navegador
- [ ] Todos DatePickers/TimePickers renderizam
- [ ] ComboBox/DropDownList exibem dados
- [ ] Filtros funcionam (digitar em ComboBox)
- [ ] Templates customizados renderizam (foto, badge)
- [ ] Handlers `change` executam
- [ ] Validações funcionam
- [ ] **Console (F12): 0 erros JavaScript**

### ✅ COMMIT E PUSH

- [ ] `git add "FrotiX.Site.Fevereiro/Pages/[Nome]/[Arquivo].cshtml"`
- [ ] Mensagem commit descritiva (ver template na Seção 5)
- [ ] Incluir `Co-Authored-By: Claude Sonnet 4.5 <noreply@anthropic.com>`
- [ ] `git commit`
- [ ] `git push origin main`
- [ ] Verificar push OK no GitHub

### ✅ DOCUMENTAÇÃO

- [ ] Atualizar este HANDOFF com status da página:
  - Mudar de ⏭️ para ✅ na tabela de roadmap
  - Atualizar percentual de progresso
- [ ] Se encontrou padrão novo, adicionar em `RegrasDesenvolvimentoFrotiX.md`

---

## 10. REFERÊNCIAS RÁPIDAS

### 📖 Documentação Oficial

| Arquivo | Caminho | Conteúdo |
|---------|---------|----------|
| **RegrasDesenvolvimentoFrotiX.md** | Raiz do workspace | Regras oficiais do projeto |
| **ControlesKendo.md** | Raiz do workspace | Doc oficial Kendo UI |
| **FrotiX.sql** | FrotiX.Site.OLD\ | Estrutura banco (13.502 linhas) |
| **CLAUDE.md** | Raiz do workspace | Config projeto (versão 5.0) |

### 🎯 Arquivos Template (100% Kendo)

| Arquivo | Status | Usar Para |
|---------|--------|-----------|
| **Viagens/Upsert.cshtml** | ✅ 100% | Referência completa: 6 inputs, 7 controles, templates |
| **Agenda/Index.cshtml** | ⚠️ 18% | Referência TimePickers (linhas 1918-1945) |

### 🛠️ Comandos Úteis

```bash
# Build
cd "c:/FrotiX/Solucao FrotiX 2026/FrotiX.Site.Fevereiro"
dotnet build --no-incremental

# Run (Dev)
dotnet run --environment Development
# Acesso: http://localhost:5000

# Buscar controles Syncfusion
grep -n '<ejs-' Pages/[Nome]/[Arquivo].cshtml

# Buscar inputs HTML5
grep -n 'type="date"' Pages/[Nome]/[Arquivo].cshtml
grep -n 'type="time"' Pages/[Nome]/[Arquivo].cshtml

# Buscar .ej2_instances (Syncfusion API)
grep -n 'ej2_instances' Pages/[Nome]/[Arquivo].cshtml

# Buscar TratamentoErroComLinha sem prefixo Alerta
grep -n 'TratamentoErroComLinha' Pages/[Nome]/[Arquivo].cshtml | grep -v 'Alerta\.'

# Contar linhas de arquivo
wc -l Pages/[Nome]/[Arquivo].cshtml

# Git
git status
git add .
git commit -m "refactor(kendo): [mensagem]"
git push origin main
```

### 🔗 Kendo API Quick Reference

```javascript
// Criar widget
$("#id").kendoDatePicker({ /* config */ });
$("#id").kendoTimePicker({ /* config */ });
$("#id").kendoComboBox({ /* config */ });
$("#id").kendoDropDownList({ /* config */ });

// Obter instância
const widget = $("#id").data("kendoDatePicker");
const widget = $("#id").data("kendoTimePicker");
const widget = $("#id").data("kendoComboBox");
const widget = $("#id").data("kendoDropDownList");

// Métodos principais
widget.value()              // Get valor
widget.value("novo")        // Set valor
widget.text()               // Get texto exibido
widget.enable(true/false)   // Habilitar/desabilitar
widget.readonly(true/false) // Readonly
widget.dataItem()           // Get objeto completo
widget.select(index)        // Selecionar por index
widget.dataSource.read()    // Recarregar dados
```

### 📞 Helpers Globais FrotiX

```javascript
// Alerta (SweetAlert2)
Alerta.TratamentoErroComLinha(arquivo, metodo, erro);
Alerta.Sucesso("Mensagem");
Alerta.Erro("Mensagem");
Alerta.Aviso("Mensagem");
Alerta.Confirmacao("Mensagem").then(result => { /* ... */ });

// Loading
FtxSpin.show();
FtxSpin.hide();

// Escape HTML (Kendo)
kendo.htmlEncode(string);
```

### 📊 Status do Projeto (Atualizar Aqui)

```
PROGRESSO ATUAL: 2% (1/50 páginas 100% Kendo)

✅ COMPLETO (1):
- Viagens/Upsert (commit c855636)

⚠️ PARCIAL (1):
- Agenda/Index (18%, commit d8cbb3d)

⏭️ PRÓXIMO:
- Agenda/Index (completar 11 controles, 3-4h)

📅 MARCOS:
- 10% (5 páginas): ~12h de trabalho restantes
- 50% (25 páginas): ~35h de trabalho restantes
- 100% (50 páginas): ~50h de trabalho restantes
```

---

## 🎓 PROTOCOLO DE TRABALHO (Para IA Sucessora)

### QUANDO INICIAR TRABALHO

1. ✅ Ler este HANDOFF completamente
2. ✅ Ler `RegrasDesenvolvimentoFrotiX.md`
3. ✅ Ler `ControlesKendo.md`
4. ✅ Executar `git pull origin main` (garantir código atualizado)
5. ✅ Executar `dotnet build` (garantir build limpo)
6. ✅ Identificar próxima página pendente (consultar Seção 8 - ROADMAP)

### DURANTE MIGRAÇÃO

1. ✅ Seguir template da Seção 5 (FASE 1 → FASE 6)
2. ✅ Build test a cada 3-4 controles migrados
3. ✅ Consultar Seção 6 (Exemplos) e Seção 7 (Erros) quando houver dúvida
4. ✅ NUNCA pular try-catch ou validação de erro

### AO FINALIZAR PÁGINA

1. ✅ Build test: `dotnet build --no-incremental`
2. ✅ Visual test: `dotnet run` + testar no navegador
3. ✅ Commit descritivo com `Co-Authored-By`
4. ✅ Push para `main`
5. ✅ Atualizar status neste HANDOFF (mudar ⏭️ para ✅)
6. ✅ Se último commit do dia: executar `git log --oneline -5` e guardar hash

### SE ENCONTRAR BLOQUEIO

1. ❓ Consultar Seção 7 (Erros Comuns)
2. ❓ Buscar padrão similar em Viagens/Upsert.cshtml (arquivo template)
3. ❓ Verificar ControlesKendo.md para sintaxe Kendo específica
4. ❓ Se erro persistir, documentar:
   - Erro completo (mensagem + stack trace)
   - Linha do código
   - O que foi tentado
   - Abrir issue ou consultar desenvolvedor

---

## 🏁 META FINAL

**OBJETIVO:** 50/50 páginas 100% Kendo UI (0% Syncfusion)

**COMO VALIDAR 100% DO PROJETO:**

```bash
# 1. Buscar TODOS os <ejs- no projeto
grep -r '<ejs-' FrotiX.Site.Fevereiro/Pages/*.cshtml
# DEVE RETORNAR: 0 resultados

# 2. Buscar TODOS os type="date" ou type="time"
grep -r 'type="date"' FrotiX.Site.Fevereiro/Pages/*.cshtml
grep -r 'type="time"' FrotiX.Site.Fevereiro/Pages/*.cshtml
# DEVE RETORNAR: 0 resultados

# 3. Buscar TODOS os .ej2_instances
grep -r 'ej2_instances' FrotiX.Site.Fevereiro/Pages/*.cshtml
# DEVE RETORNAR: 0 resultados

# 4. Buscar imports Syncfusion
grep -r '@using Syncfusion' FrotiX.Site.Fevereiro/Pages/*.cshtml
# DEVE RETORNAR: 0 resultados

# 5. Build sem erros
dotnet build --no-incremental
# DEVE RETORNAR: 0 Erros, 0 Warnings
```

**Quando todos os 5 comandos acima retornarem 0, o projeto está 100% Kendo. 🎉**

---

## 📝 NOTAS FINAIS

- **Estimativa Total:** 47-62h de trabalho restantes
- **Velocidade Média:** ~3h por página (varia conforme complexidade)
- **Build Atual:** ✅ 0 erros, 0 warnings
- **Branch:** `main` (sempre push aqui)
- **Última Atualização:** 10/02/2026

**BOA SORTE! 🚀**

Se encontrar qualquer inconsistência neste documento ou descobrir padrões melhores, ATUALIZE este HANDOFF para as IAs futuras.

---

**Documento gerado por:** Claude Sonnet 4.5
**Para:** IA Sucessora
**Projeto:** FrotiX.Site.Fevereiro - Migração Syncfusion → Kendo UI
**Commit Base:** 5428fc9
