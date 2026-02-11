# Controles Kendo/Telerik UI - Guia Oficial FrotiX

> **Projeto:** FrotiX 2026 (FrotiX.Site.OLD)
> **Versao:** Kendo UI 2025.4.1321 (Q4 2025) - LOCAL, nao CDN
> **Stack:** ASP.NET Core, Razor Pages, jQuery `.kendoXxx()` init em @section ScriptsBlock
> **Status:** LEITURA OBRIGATORIA para agentes de IA
> **Ultima Atualizacao:** 09/02/2026

---

## 0. COMO USAR ESTE DOCUMENTO

### Para Agentes de IA (Claude/Gemini/Copilot/Codex)

Este documento e a **FONTE UNICA DE VERDADE** para uso de controles Kendo/Telerik no FrotiX.
Antes de criar ou modificar qualquer controle Kendo em paginas CSHTML ou arquivos JS, **CONSULTE ESTE DOCUMENTO**.

### Regras Fundamentais

1. **SEMPRE** usar controles Telerik Kendo UI (nunca HTML5 nativo, nunca Syncfusion para novos controles)
2. **SEMPRE** inicializar controles via jQuery `$("#id").kendoXxx({...})` dentro de `@section ScriptsBlock`
3. **NUNCA** usar TagHelpers (`<kendo-*>`) nem `@Html.Kendo().DeferredScripts()` (causa "kendo is not defined")
4. **SEMPRE** acessar widget via `.data("kendo[NomeWidget]")` em JavaScript
5. **NUNCA** inicializar o mesmo widget duas vezes no mesmo elemento
6. **NUNCA** adicionar CDN Kendo 2022 em paginas (conflita com tema bootstrap-main 2025)
7. **SEMPRE** usar `<input>` simples no HTML e inicializar o controle no ScriptsBlock

### Por que NAO usar TagHelpers no FrotiX?

O layout do FrotiX renderiza nesta ordem:
1. `@RenderBody()` - onde ficam os TagHelpers (geram scripts inline)
2. `_ScriptsBasePlugins.cshtml` - onde carrega `kendo.all.min.js`
3. `@RenderSection("ScriptsBlock")` - onde ficam scripts da pagina

TagHelpers geram scripts inline no passo 1, **ANTES** do kendo.all.min.js carregar no passo 2.
Resultado: `"kendo is not defined"`. Por isso usamos jQuery init no passo 3 (ScriptsBlock).

### Referencia de Exemplos

O projeto `Kendo.Mvc.Examples/` na raiz do workspace contem **149 controles** com **2.482 exemplos**
funcionais. Cada pasta tem variantes HtmlHelper (`.cshtml`) e TagHelper (`*_TagHelper.cshtml`).
**ATENCAO:** Os exemplos usam TagHelpers, mas no FrotiX usamos jQuery init. Adapte os exemplos.

---

## 1. DROPDOWNLIST

### O que e

Lista suspensa de selecao unica. O usuario escolhe UM item de uma lista pre-definida.
Diferente do ComboBox, **NAO permite digitacao livre** (apenas filtro).

### Quando Usar no FrotiX

- Selecao de Finalidade (Viagens/Upsert)
- Selecao de Combustivel (Viagens/Upsert)
- Qualquer campo com opcoes fixas vindas do backend

### Sintaxe jQuery Init (USADA NO FROTIX)

**HTML:**
```html
<input id="ddlFinalidade" name="ViagemObj.Viagem.Finalidade" style="width: 100%;" />
```

**JavaScript (em @section ScriptsBlock):**
```javascript
var dataFinalidade = @Html.Raw(Json.Serialize(ViewData["dataFinalidade"]));
$("#ddlFinalidade").kendoDropDownList({
    dataTextField: "descricao",        // camelCase! (JSON serialization)
    dataValueField: "finalidadeId",    // camelCase!
    optionLabel: "Escolha a Finalidade...",
    dataSource: dataFinalidade,
    height: 200,
    value: "@(Model.ViagemObj?.Viagem?.Finalidade ?? "")",
    change: function (e) { lstFinalidade_Change(e); }
});
```

### Sintaxe TagHelper (NAO USAR - apenas referencia)

```html
<!-- NAO USAR no FrotiX! Causa "kendo is not defined" -->
<kendo-dropdownlist name="ddlFinalidade"
    datatextfield="Descricao"
    datavaluefield="FinalidadeId"
    option-label="Escolha a Finalidade..."
    bind-to="@((IEnumerable<object>)ViewData["dataFinalidade"])"
    height="200"
    value="@(Model.ViagemObj?.Viagem?.Finalidade ?? "")"
    on-change="lstFinalidade_Change"
    deferred="true"
    style="width: 100%;">
</kendo-dropdownlist>
```

### Sintaxe HtmlHelper (apenas referencia)

```csharp
@(Html.Kendo().DropDownList()
    .Name("ddlFinalidade")
    .DataTextField("Descricao")
    .DataValueField("FinalidadeId")
    .OptionLabel("Escolha a Finalidade...")
    .BindTo((IEnumerable<SelectListItem>)ViewData["dataFinalidade"])
    .Value(Model.ViagemObj?.Viagem?.Finalidade ?? "")
    .Events(e => e.Change("lstFinalidade_Change"))
    .HtmlAttributes(new { style = "width: 100%" })
)
```

### Atributos TagHelper Importantes

| Atributo | Tipo | Descricao | Exemplo |
|----------|------|-----------|---------|
| `name` | string | ID do elemento HTML (obrigatorio) | `"ddlFinalidade"` |
| `datatextfield` | string | Campo exibido ao usuario | `"Descricao"` |
| `datavaluefield` | string | Campo enviado no form/value | `"FinalidadeId"` |
| `option-label` | string | Texto placeholder (primeira opcao vazia) | `"Escolha..."` |
| `bind-to` | IEnumerable | Dados locais (do PageModel/ViewData) | `ViewData["data"]` |
| `value` | string | Valor pre-selecionado | `Model.Valor` |
| `height` | int | Altura do popup em px | `200` |
| `filter` | FilterType | Tipo de filtro no dropdown | `FilterType.Contains` |
| `on-change` | string | Nome da funcao JS chamada ao mudar valor | `"onChange"` |
| `on-select` | string | Nome da funcao JS chamada ao selecionar | `"onSelect"` |
| `on-open` | string | Evento ao abrir popup | `"onOpen"` |
| `on-close` | string | Evento ao fechar popup | `"onClose"` |
| `on-data-bound` | string | Evento apos dados carregados | `"onDataBound"` |
| `deferred` | bool | Adia renderizacao dos scripts | `true` |
| `enable` | bool | Habilita/desabilita o controle | `true` |
| `auto-bind` | bool | Carrega dados automaticamente | `true` |
| `cascade-from` | string | ID do dropdown pai (cascata) | `"ddlCategoria"` |

### Acesso via JavaScript

```javascript
// Obter instancia do widget
const ddl = $("#ddlFinalidade").data("kendoDropDownList");

// Obter valor selecionado
const valor = ddl.value();           // retorna o Value (ex: "3")
const texto = ddl.text();            // retorna o Text (ex: "Administrativa")

// Definir valor programaticamente
ddl.value("5");                      // seleciona item com Value = "5"

// Obter item de dados selecionado (objeto completo)
const dataItem = ddl.dataItem();     // { FinalidadeId: 5, Descricao: "Administrativa" }

// Habilitar/Desabilitar
ddl.enable(false);                   // desabilita
ddl.enable(true);                    // habilita

// Limpar selecao
ddl.value("");                       // volta ao option-label

// Recarregar dados
ddl.dataSource.read();               // re-executa leitura dos dados

// Abrir/Fechar popup
ddl.open();
ddl.close();

// Filtrar programaticamente
ddl.search("texto");                 // filtra a lista

// Selecionar por indice
ddl.select(0);                       // seleciona primeiro item
ddl.select(function(item) {          // seleciona por funcao
    return item.Descricao === "Administrativa";
});
```

### Cascata (DropDownList Dependente)

```html
<!-- Dropdown PAI -->
<kendo-dropdownlist name="ddlCategoria"
    datatextfield="CategoryName"
    datavaluefield="CategoryId"
    option-label="Selecione categoria..."
    style="width: 100%;">
    <datasource>
        <transport>
            <read url="/api/Categorias/GetAll" />
        </transport>
    </datasource>
</kendo-dropdownlist>

<!-- Dropdown FILHO (cascata) -->
<kendo-dropdownlist name="ddlProduto"
    datatextfield="ProductName"
    datavaluefield="ProductId"
    option-label="Selecione produto..."
    cascade-from="ddlCategoria"
    auto-bind="false"
    enable="false"
    filter="FilterType.Contains"
    style="width: 100%;">
    <datasource server-filtering="true">
        <transport>
            <read url="/api/Produtos/GetByCategoria" />
        </transport>
    </datasource>
</kendo-dropdownlist>
```

### Eventos Disponiveis

| Evento | Quando Dispara | Parametro `e` |
|--------|----------------|---------------|
| `change` | Valor muda | `e.sender` = widget |
| `select` | Item selecionado (antes de mudar) | `e.item` = elemento DOM |
| `open` | Popup abre | `e.sender` = widget |
| `close` | Popup fecha | `e.sender` = widget |
| `dataBound` | Dados carregados | `e.sender` = widget |
| `filtering` | Filtro aplicado | `e.filter` = texto filtrado |
| `cascade` | Dropdown filho atualizado | `e.sender` = widget |

### Exemplo Real FrotiX (Viagens/Upsert.cshtml)

**HTML (no corpo da pagina):** usar `<input>` simples, sem TagHelper

```html
<!-- Finalidade da Viagem -->
<input id="ddlFinalidade" name="ViagemObj.Viagem.Finalidade" style="width: 100%;" />

<!-- Combustivel Inicial -->
<input id="ddlCombustivelInicial" name="ViagemObj.Viagem.CombustivelInicial" style="width: 100%;" />

<!-- Combustivel Final -->
<input id="ddlCombustivelFinal" name="ViagemObj.Viagem.CombustivelFinal" style="width: 100%;" />
```

**JavaScript (em @section ScriptsBlock):** inicializar com jQuery

```javascript
@section ScriptsBlock {
<script>
(function () {
    try {
        var dataFinalidade = @Html.Raw(Json.Serialize(ViewData["dataFinalidade"]));
        var dataCombustivel = @Html.Raw(Json.Serialize(ViewData["dataCombustivel"]));

        $("#ddlFinalidade").kendoDropDownList({
            dataTextField: "descricao",        // camelCase! (JSON serialization)
            dataValueField: "finalidadeId",    // camelCase!
            optionLabel: "Escolha a Finalidade...",
            dataSource: dataFinalidade,
            height: 200,
            value: "@(Model.ViagemObj?.Viagem?.Finalidade ?? "")",
            change: function (e) {
                try { lstFinalidade_Change(e); }
                catch (err) { console.error("lstFinalidade_Change:", err); }
            }
        });

        $("#ddlCombustivelInicial").kendoDropDownList({
            dataTextField: "descricao",
            dataValueField: "nivel",
            optionLabel: "Selecione...",
            dataSource: dataCombustivel,
            height: 200,
            value: "@(Model.ViagemObj?.Viagem?.CombustivelInicial ?? "")",
            template: combustivelTemplate,
            valueTemplate: combustivelValueTemplate
        });

        $("#ddlCombustivelFinal").kendoDropDownList({
            dataTextField: "descricao",
            dataValueField: "nivel",
            optionLabel: "Selecione...",
            dataSource: dataCombustivel,
            height: 200,
            value: "@(Model.ViagemObj?.Viagem?.CombustivelFinal ?? "")",
            template: combustivelTemplate,
            valueTemplate: combustivelValueTemplate
        });
    } catch (error) {
        Alerta.TratamentoErroComLinha("Upsert.cshtml", "kendoDropDownList.init", error);
    }
})();
</script>
}
```

**IMPORTANTE - camelCase:** `@Html.Raw(Json.Serialize(...))` gera JSON com camelCase.
Use `data.descricao` e NAO `data.Descricao`.

```javascript
// Acesso programatico em outros JS
var ddlFinalidade = $("#ddlFinalidade").data("kendoDropDownList");
var ddlCombInicial = $("#ddlCombustivelInicial").data("kendoDropDownList");
var ddlCombFinal = $("#ddlCombustivelFinal").data("kendoDropDownList");

// Obter valor
var finalidadeValor = ddlFinalidade.value();

// Resetar para placeholder
ddlFinalidade.value("");
```

### Referencia Kendo.Mvc.Examples

- `Views/DropDownList/Basic_Usage.cshtml` - Uso basico HtmlHelper
- `Views/DropDownList/Basic_Usage_TagHelper.cshtml` - Uso basico TagHelper
- `Views/DropDownList/CascadingDropDownList.cshtml` - Cascata entre dropdowns
- `Views/DropDownList/ServerFiltering.cshtml` - Filtro no servidor
- `Views/DropDownList/Events.cshtml` - Todos os eventos
- `Views/DropDownList/Template.cshtml` - Templates customizados
- `Views/DropDownList/Virtualization.cshtml` - Listas com 50K+ itens

---

## 2. COMBOBOX

### O que e

Caixa de selecao com **digitacao livre**. O usuario pode escolher da lista OU digitar valor customizado.
Diferente do DropDownList, **permite entrada de texto que nao esta na lista**.

### Quando Usar no FrotiX

- Selecao de Requisitante (Agenda/Index) - usuario pode digitar para filtrar
- Qualquer campo onde filtro por digitacao e essencial

### Sintaxe jQuery Init (USADA NO FROTIX)

**HTML:**
```html
<input id="lstRequisitante" name="lstRequisitante" class="flex-grow-1" style="width: 100%;"
    data-ejtip="Se o <strong>Requisitante</strong> nao estiver presente..." />
```

**JavaScript (em @section ScriptsBlock):**
```javascript
var dataRequisitante = @Html.Raw(Json.Serialize(ViewData["dataRequisitante"]));
$("#lstRequisitante").kendoComboBox({
    dataTextField: "requisitante",     // camelCase!
    dataValueField: "requisitanteId",  // camelCase!
    placeholder: "Selecione um Requisitante...",
    filter: "contains",                // string, nao enum
    dataSource: dataRequisitante,
    height: 200
});
```

### Sintaxe TagHelper (NAO USAR - apenas referencia)

```html
<!-- NAO USAR no FrotiX! Causa "kendo is not defined" -->
<kendo-combobox name="lstRequisitante"
    placeholder="Selecione um Requisitante..."
    filter="Kendo.Mvc.UI.FilterType.Contains"
    datatextfield="Requisitante"
    datavaluefield="RequisitanteId"
    bind-to="@((IEnumerable<object>)ViewData["dataRequisitante"])"
    height="200"
    class="flex-grow-1"
    style="width: 100%; height: 38px;">
</kendo-combobox>
```

### Sintaxe HtmlHelper (apenas referencia)

```csharp
@(Html.Kendo().ComboBox()
    .Name("lstRequisitante")
    .Filter(FilterType.Contains)
    .Placeholder("Selecione um Requisitante...")
    .DataTextField("Requisitante")
    .DataValueField("RequisitanteId")
    .BindTo((IEnumerable<object>)ViewData["dataRequisitante"])
    .Suggest(true)
    .HtmlAttributes(new { style = "width:100%; height: 38px;" })
)
```

### Atributos TagHelper Importantes

| Atributo | Tipo | Descricao | Exemplo |
|----------|------|-----------|---------|
| `name` | string | ID do elemento HTML | `"lstRequisitante"` |
| `placeholder` | string | Texto quando vazio | `"Selecione..."` |
| `filter` | FilterType | Tipo de filtro (Contains, StartsWith) | `FilterType.Contains` |
| `datatextfield` | string | Campo de texto exibido | `"Requisitante"` |
| `datavaluefield` | string | Campo de valor | `"RequisitanteId"` |
| `bind-to` | IEnumerable | Dados locais | `ViewData["data"]` |
| `suggest` | bool | Auto-completar enquanto digita | `true` |
| `height` | int | Altura do popup em px | `200` |
| `min-length` | int | Minimo de chars para filtrar | `1` |
| `auto-bind` | bool | Carregar automaticamente | `true` |
| `cascade-from` | string | ComboBox pai | `"cmbCategoria"` |
| `value` | string | Valor pre-selecionado | `"123"` |

### Acesso via JavaScript

```javascript
// Obter instancia
const combo = $("#lstRequisitante").data("kendoComboBox");

// Obter valor e texto
const valor = combo.value();           // ex: "42"
const texto = combo.text();            // ex: "Joao Silva"
const dataItem = combo.dataItem();     // objeto completo

// Definir valor programaticamente
combo.value("42");                     // por valor
combo.text("Joao Silva");             // por texto (digitacao livre)

// Filtrar
combo.search("Silva");                 // filtra a lista

// Limpar
combo.value("");
combo.text("");

// Habilitar/Desabilitar
combo.enable(false);
combo.enable(true);

// Recarregar dados
combo.dataSource.read();

// Obter datasource
const ds = combo.dataSource;
const todosItems = ds.data();          // array de objetos
```

### Diferenca entre ComboBox e DropDownList

| Caracteristica | DropDownList | ComboBox |
|----------------|-------------|----------|
| Digitacao livre | NAO | SIM |
| Filtro | Opcional | Sim (padrao) |
| Suggest | NAO | SIM |
| Valor nao listado | NAO permite | SIM permite |
| Uso tipico | Listas fixas (Status, Tipo) | Listas com busca (Requisitante, Motorista) |

### Exemplo Real FrotiX (Agenda/Index.cshtml)

**HTML (no corpo da pagina):** usar `<input>` simples

```html
<!-- Requisitante no modal de agendamento -->
<input id="lstRequisitante" name="lstRequisitante" class="flex-grow-1" style="width: 100%;"
    data-ejtip="Se o <strong>Requisitante</strong> nao estiver na Lista, verifique se nao esta <strong>Inativo</strong>" />

<!-- Requisitante no modal de evento -->
<input id="lstRequisitanteEvento" name="lstRequisitanteEvento" style="width: 100%;" />
```

**JavaScript (em @section ScriptsBlock):**

```javascript
(function () {
    try {
        var dataRequisitante = @Html.Raw(Json.Serialize(ViewData["dataRequisitante"]));

        $("#lstRequisitante").kendoComboBox({
            dataTextField: "requisitante",     // camelCase!
            dataValueField: "requisitanteId",  // camelCase!
            placeholder: "Selecione um Requisitante...",
            filter: "contains",
            dataSource: dataRequisitante,
            height: 200
        });

        $("#lstRequisitanteEvento").kendoComboBox({
            dataTextField: "requisitante",
            dataValueField: "requisitanteId",
            placeholder: "Selecione o requisitante...",
            filter: "contains",
            dataSource: dataRequisitante,
            height: 200
        });
    } catch (error) {
        Alerta.TratamentoErroComLinha("Agenda/Index.cshtml", "kendoControls.init", error);
    }
})();
```

```javascript
// exibe-viagem.js - Acesso programatico
const kendoComboBox = $("#lstRequisitante").data("kendoComboBox");
if (kendoComboBox) {
    kendoComboBox.value(objViagem.requisitanteId);
    // ou
    kendoComboBox.text(objViagem.requisitante);
}

// frotix.js - Helper generico
const combo = input.data("kendoComboBox");
if (combo) {
    combo.value(null);
    combo.text("");
}
```

### Referencia Kendo.Mvc.Examples

- `Views/ComboBox/Basic_Usage.cshtml` / `*_TagHelper.cshtml`
- `Views/ComboBox/CascadingCombobox.cshtml` - Cascata
- `Views/ComboBox/ServerFiltering.cshtml` - Filtro no servidor
- `Views/ComboBox/virtualization.cshtml` - Listas gigantes
- `Views/ComboBox/template.cshtml` - Templates customizados

---

## 3. MULTISELECT

### O que e

Permite selecionar **MULTIPLOS itens** de uma lista. Exibe como tags/chips removiveis.

### Quando Usar no FrotiX

- Selecao de Lavadores (Manutencao/ControleLavagem)
- Qualquer campo onde multipla selecao e necessaria

### Sintaxe JavaScript (usada no FrotiX)

```javascript
// Inicializacao via jQuery
$("#lstLavadores").kendoMultiSelect({
    dataTextField: "nome",
    dataValueField: "lavadorId",
    dataSource: data,                  // array de objetos
    autoClose: false,                  // manter popup aberto apos selecionar
    placeholder: "Selecione os lavadores participantes..."
});
```

### Sintaxe TagHelper

```html
<kendo-multiselect name="lstLavadores"
    datatextfield="nome"
    datavaluefield="lavadorId"
    placeholder="Selecione os lavadores..."
    auto-close="false"
    bind-to="@((IEnumerable<object>)ViewData["dataLavadores"])"
    style="width: 100%;">
</kendo-multiselect>
```

### Sintaxe HtmlHelper

```csharp
@(Html.Kendo().MultiSelect()
    .Name("lstLavadores")
    .DataTextField("nome")
    .DataValueField("lavadorId")
    .Placeholder("Selecione os lavadores...")
    .AutoClose(false)
    .BindTo((IEnumerable<object>)ViewData["dataLavadores"])
    .Value(new string[] { "1", "3" })   // pre-selecionar
    .HtmlAttributes(new { style = "width: 100%" })
)
```

### Acesso via JavaScript

```javascript
// Obter instancia
var multi = $("#lstLavadores").data("kendoMultiSelect");

// Obter valores selecionados (array)
var valores = multi.value();           // ex: [1, 3, 7]

// Obter itens de dados selecionados
var dataItems = multi.dataItems();     // array de objetos

// Definir valores
multi.value([1, 3, 7]);               // selecionar multiplos

// Limpar
multi.value([]);

// Habilitar/Desabilitar
multi.enable(false);
multi.enable(true);
```

### Referencia Kendo.Mvc.Examples

- `Views/MultiSelect/Basic_Usage.cshtml` / `*_TagHelper.cshtml`
- `Views/MultiSelect/Tag_Mode.cshtml` - Modo de tags
- `Views/MultiSelect/ServerFiltering.cshtml` - Filtro servidor
- `Views/MultiSelect/Virtualization.cshtml` - Listas gigantes

---

## 4. DATEPICKER

### O que e

Seletor de data com calendario popup. Substitui `<input type="date">` HTML5.

### Quando Usar no FrotiX

- Campos de data em formularios (Data Inicial, Data Final)
- Filtros por periodo em dashboards

### Sintaxe jQuery Init (USADA NO FROTIX)

**HTML:**
```html
<input id="txtDataInicialEvento" name="txtDataInicialEvento" style="width: 100%;" />
```

**JavaScript (em @section ScriptsBlock):**
```javascript
$("#txtDataInicialEvento").kendoDatePicker({
    format: "dd/MM/yyyy",
    placeholder: "Data Inicial"
});
```

### ⚠️ CONFIGURAÇÃO CRÍTICA PARA pt-BR (OBRIGATÓRIA)

**IMPORTANTE:** Apenas `culture: "pt-BR"` NÃO é suficiente para ter formatação pt-BR completa!

#### 🔧 Problema Comum

O Kendo DatePicker com `dateInput: true` usa um componente interno chamado **DateInput** que tem seus próprios placeholders e formatações padrão em inglês (`day/month/year`). A configuração `culture: "pt-BR"` global afeta apenas o calendário popup, mas **NÃO afeta** o componente DateInput que gerencia a digitação direta e os placeholders.

#### ❌ Configuração INCORRETA (placeholder em inglês)

```javascript
$("#txtDataInicial").kendoDatePicker({
    format: "dd/MM/yyyy",
    culture: "pt-BR",
    dateInput: true,  // ❌ Usa configurações padrão em inglês
    placeholder: "Data Inicial"
});
// RESULTADO: Campo vazio mostra "day/month/year" mesmo com culture pt-BR
```

#### ✅ Configuração CORRETA (completa pt-BR)

```javascript
$("#txtDataInicial").kendoDatePicker({
    format: "dd/MM/yyyy",
    culture: "pt-BR",
    dateInput: {
        format: "dd/MM/yyyy",
        messages: {
            year: "yyyy",
            month: "MM",
            day: "dd"
        }
    },
    placeholder: "dd/MM/yyyy",
    min: dataMinima,
    max: dataMaxima,
    value: dataInicial
});
// RESULTADO: Campo vazio mostra "dd/MM/yyyy" (formato brasileiro)
```

#### 📋 3 Níveis de Configuração Necessários

Para ter controle completo da formatação pt-BR, configure **3 níveis**:

| Nível | Propriedade | Afeta | Obrigatório? |
|-------|-------------|-------|--------------|
| 1️⃣ | `culture: "pt-BR"` | Calendário popup e formatação de valores | ✅ SIM |
| 2️⃣ | `format: "dd/MM/yyyy"` | Formato de exibição do valor | ✅ SIM |
| 3️⃣ | `dateInput: { format, messages }` | Digitação direta e **placeholders** | ✅ SIM |

#### 🎯 Exemplo Completo FrotiX (Data Inicial/Final)

```javascript
// Limites de data
var dataMinima = new Date();
dataMinima.setDate(dataMinima.getDate() - 15);
var dataMaxima = new Date();

// DatePicker com configuração completa pt-BR
$("#txtDataInicial").kendoDatePicker({
    format: "dd/MM/yyyy",
    culture: "pt-BR",
    min: dataMinima,
    max: dataMaxima,
    dateInput: {
        format: "dd/MM/yyyy",
        messages: {
            year: "yyyy",
            month: "MM",
            day: "dd"
        }
    },
    placeholder: "dd/MM/yyyy",
    value: null,
    open: function() {
        // Armazena valor antes de abrir calendário
        this._previousValue = this.value();
    },
    close: function() {
        // Se fechou sem selecionar, restaura valor anterior
        var currentValue = this.value();
        if (!currentValue || isNaN(currentValue)) {
            this.value(this._previousValue || null);
        }
    }
});
```

#### 📊 Resultado Visual

| Configuração | Campo Vazio | Campo Preenchido |
|-------------|-------------|------------------|
| ❌ **Sem dateInput explícito** | `day/month/year` | `2026-02-10` (americano) |
| ✅ **Com dateInput completo** | `dd/MM/yyyy` | `10/02/2026` (brasileiro) |

### Sintaxe TagHelper (NAO USAR - apenas referencia)

```html
<!-- NAO USAR no FrotiX! Causa "kendo is not defined" -->
<kendo-datepicker name="txtDataInicialEvento"
    format="dd/MM/yyyy"
    placeholder="Data Inicial"
    style="width: 100%;">
</kendo-datepicker>
```

### Sintaxe HtmlHelper (apenas referencia)

```csharp
@(Html.Kendo().DatePicker()
    .Name("txtDataInicial")
    .Format("dd/MM/yyyy")
    .Min(DateTime.Today)               // bloquear datas passadas
    .Value(Model.DataInicial)
    .HtmlAttributes(new { @class = "form-control", style = "width: 100%;" })
)
```

### Atributos TagHelper Importantes

| Atributo | Tipo | Descricao | Exemplo |
|----------|------|-----------|---------|
| `name` | string | ID do elemento | `"txtDataInicial"` |
| `format` | string | Formato da data | `"dd/MM/yyyy"` |
| `value` | DateTime | Valor inicial | `DateTime.Today` |
| `min` | DateTime | Data minima permitida | `DateTime.Today` |
| `max` | DateTime | Data maxima permitida | `DateTime.Today.AddYears(1)` |
| `placeholder` | string | Texto placeholder | `"Selecione..."` |
| `start` | CalendarView | Visao inicial do calendario | `CalendarView.Year` |
| `depth` | CalendarView | Profundidade de selecao | `CalendarView.Year` |
| `date-input` | bool | Permite digitacao direta | `true` |
| `on-change` | string | Evento ao mudar data | `"onDateChange"` |
| `culture` | string | Cultura para formatacao | `"pt-BR"` |

### Acesso via JavaScript (HELPER FrotiX)

O FrotiX possui o arquivo `kendo-datetime.js` com helpers prontos:

```javascript
// HELPERS GLOBAIS (kendo-datetime.js)

// Obter instancia do widget
const picker = window.getKendoDatePicker("txtDataInicial");

// Obter valor como Date
const data = window.getKendoDateValue("txtDataInicial");  // retorna Date ou null

// Definir valor
window.setKendoDateValue("txtDataInicial", "2026-02-09");
window.setKendoDateValue("txtDataInicial", new Date(2026, 1, 9));
window.setKendoDateValue("txtDataInicial", null);  // limpar

// Habilitar/Desabilitar
window.enableKendoDatePicker("txtDataInicial", true);   // habilitar
window.enableKendoDatePicker("txtDataInicial", false);  // desabilitar

// Mostrar/Esconder
window.showKendoDatePicker("txtDataInicial", true);     // mostrar
window.showKendoDatePicker("txtDataInicial", false);    // esconder
```

```javascript
// ACESSO DIRETO (sem helper)
const picker = $("#txtDataInicial").data("kendoDatePicker");

// Obter valor
const valor = picker.value();          // retorna Date ou null

// Definir valor
picker.value(new Date(2026, 1, 9));    // meses comecam em 0!
picker.value(null);                    // limpar

// Definir limites
picker.min(new Date(2026, 0, 1));
picker.max(new Date(2026, 11, 31));

// Habilitar/Desabilitar
picker.enable(false);
picker.enable(true);

// Abrir/Fechar calendario
picker.open();
picker.close();
```

### Formato de Data Padrao FrotiX

**SEMPRE** usar `dd/MM/yyyy` (formato brasileiro).

```html
<kendo-datepicker name="txtData"
    format="dd/MM/yyyy"
    culture="pt-BR"
    placeholder="DD/MM/AAAA">
</kendo-datepicker>
```

### Referencia Kendo.Mvc.Examples

- `Views/DatePicker/Basic_Usage.cshtml` / `*_TagHelper.cshtml`
- `Views/DatePicker/Rangeselection.cshtml` - Selecao de intervalo
- `Views/DatePicker/Disable_Dates.cshtml` - Desabilitar datas especificas
- `Views/DatePicker/Api.cshtml` - API programatica
- `Views/DatePicker/Events.cshtml` - Todos os eventos
- `Views/DatePicker/Globalization.cshtml` - Formatos por cultura

---

## 5. TIMEPICKER

### O que e

Seletor de hora com popup de horarios. Substitui `<input type="time">` HTML5.

### Sintaxe HtmlHelper (usada no FrotiX)

```csharp
@(Html.Kendo().TimePicker()
    .Name("txtHoraInicial")
    .Format("HH:mm")
    .Interval(TimeSpan.FromMinutes(30))   // intervalos de 30min
    .HtmlAttributes(new { @class = "form-control", style = "width: 100%; height: 38px;" })
    .Value(Model.HoraInicial)
)
```

### Sintaxe TagHelper

```html
<kendo-timepicker name="txtHoraInicial"
    format="HH:mm"
    interval="30"
    placeholder="HH:mm"
    style="width: 100%; height: 38px;">
</kendo-timepicker>
```

### ⚠️ CONFIGURAÇÃO CRÍTICA PARA pt-BR (OBRIGATÓRIA)

**IMPORTANTE:** Assim como o DatePicker, o TimePicker também requer configuração explícita do `dateInput` para placeholders pt-BR!

#### ❌ Configuração INCORRETA (placeholder em inglês)

```javascript
$("#txtHoraInicial").kendoTimePicker({
    format: "HH:mm",
    culture: "pt-BR",
    dateInput: true,  // ❌ Usa configurações padrão em inglês
    interval: 15
});
// RESULTADO: Campo vazio mostra "hours:minutes" mesmo com culture pt-BR
```

#### ✅ Configuração CORRETA (completa pt-BR)

```javascript
$("#txtHoraInicial").kendoTimePicker({
    format: "HH:mm",
    culture: "pt-BR",
    interval: 15,
    dateInput: {
        format: "HH:mm",
        messages: {
            hour: "HH",
            minute: "mm"
        }
    },
    placeholder: "HH:mm",
    value: horaInicial
});
// RESULTADO: Campo vazio mostra "HH:mm" (formato brasileiro)
```

#### 🎯 Exemplo Completo FrotiX (Hora Inicial/Final)

```javascript
$("#txtHoraInicial").kendoTimePicker({
    format: "HH:mm",
    culture: "pt-BR",
    interval: 15,  // intervalos de 15 minutos
    dateInput: {
        format: "HH:mm",
        messages: {
            hour: "HH",
            minute: "mm"
        }
    },
    placeholder: "HH:mm",
    value: null,
    open: function() {
        // Armazena valor antes de abrir popup
        this._previousValue = this.value();
    },
    close: function() {
        // Se fechou sem selecionar, restaura valor anterior
        var currentValue = this.value();
        if (!currentValue || isNaN(currentValue)) {
            this.value(this._previousValue || null);
        }
    }
});
```

#### 📊 Resultado Visual

| Configuração | Campo Vazio | Campo Preenchido |
|-------------|-------------|------------------|
| ❌ **Sem dateInput explícito** | `hours:minutes` | `14:30` |
| ✅ **Com dateInput completo** | `HH:mm` | `14:30` |

### Acesso via JavaScript (HELPER FrotiX)

```javascript
// HELPERS GLOBAIS (kendo-datetime.js)

// Obter instancia
const picker = window.getKendoTimePicker("txtHoraInicial");

// Obter valor formatado (string "HH:mm")
const hora = window.getKendoTimeValue("txtHoraInicial");   // ex: "14:30"

// Definir valor
window.setKendoTimeValue("txtHoraInicial", "14:30");
window.setKendoTimeValue("txtHoraInicial", new Date(0,0,0,14,30));

// Habilitar/Desabilitar
window.enableKendoTimePicker("txtHoraInicial", true);
window.enableKendoTimePicker("txtHoraInicial", false);

// Mostrar/Esconder
window.showKendoTimePicker("txtHoraInicial", true);
window.showKendoTimePicker("txtHoraInicial", false);
```

```javascript
// ACESSO DIRETO
const picker = $("#txtHoraInicial").data("kendoTimePicker");
picker.value();                // Date com hora/minuto
picker.value(new Date(0,0,0,14,30));  // setar 14:30
picker.enable(false);
```

### Referencia Kendo.Mvc.Examples

- `Views/TimePicker/Basic_Usage.cshtml` / `*_TagHelper.cshtml`
- `Views/TimePicker/Api.cshtml` - API programatica
- `Views/TimePicker/Events.cshtml` - Eventos
- `Views/TimePicker/Rangeselection.cshtml` - Faixa de horario

---

## 6. DATETIMEPICKER

### O que e

Combinacao de DatePicker + TimePicker em um unico controle.

### Sintaxe HtmlHelper

```csharp
@(Html.Kendo().DateTimePicker()
    .Name("txtDataHora")
    .Format("dd/MM/yyyy HH:mm")
    .Min(DateTime.Now)
    .Value(Model.DataHora)
    .HtmlAttributes(new { @class = "form-control", style = "width: 100%;" })
)
```

### Sintaxe TagHelper

```html
<kendo-datetimepicker name="txtDataHora"
    format="dd/MM/yyyy HH:mm"
    min="DateTime.Now"
    value="DateTime.Now"
    style="width: 100%;">
</kendo-datetimepicker>
```

### Acesso via JavaScript

```javascript
const picker = $("#txtDataHora").data("kendoDateTimePicker");
picker.value();                        // Date com data+hora
picker.value(new Date(2026, 1, 9, 14, 30));
picker.min(new Date());
picker.enable(false);
```

---

## 7. GRID

### O que e

Tabela de dados rica com paginacao, ordenacao, filtros, edicao, agrupamento e exportacao.

### Quando Usar no FrotiX

- Listagens de dados (via DataTables atualmente, mas Kendo Grid e alternativa)
- Paginas de teste (Temp/Index)

### Sintaxe HtmlHelper (Kendo.Mvc.Examples)

```csharp
@(Html.Kendo().Grid<ProductViewModel>()
    .Name("Grid")
    .Columns(columns => {
        columns.Bound(p => p.ProductName).Title("Nome").Width(200);
        columns.Bound(p => p.UnitPrice).Title("Preco").Width(150);
        columns.Bound(p => p.UnitsInStock).Title("Estoque").Width(150);
        columns.Command(command => command.Destroy()).Width(160);
    })
    .ToolBar(toolbar => {
        toolbar.Create();
        toolbar.Save();
    })
    .Editable(editable => editable.Mode(GridEditMode.InCell))
    .Pageable()
    .Sortable()
    .Groupable()
    .Filterable()
    .Scrollable()
    .DataSource(dataSource => dataSource
        .Ajax()
        .Batch(true)
        .PageSize(20)
        .Model(model => {
            model.Id(p => p.ProductID);
            model.Field(p => p.ProductID).Editable(false);
        })
        .Create("Products_Create", "Grid")
        .Read("Products_Read", "Grid")
        .Update("Products_Update", "Grid")
        .Destroy("Products_Destroy", "Grid")
    )
)
```

### Sintaxe JavaScript (usada em Temp/Index)

```javascript
$("#grid").kendoGrid({
    dataSource: {
        data: dados,
        pageSize: 20,
        schema: {
            model: {
                fields: {
                    nome: { type: "string" },
                    preco: { type: "number" },
                    data: { type: "date" }
                }
            }
        }
    },
    columns: [
        { field: "nome", title: "Nome", width: 200 },
        { field: "preco", title: "Preco", format: "{0:c}", width: 150 },
        { field: "data", title: "Data", format: "{0:dd/MM/yyyy}", width: 120 },
        { command: ["edit", "destroy"], title: "Acoes", width: 200 }
    ],
    pageable: true,
    sortable: true,
    filterable: true,
    editable: "inline",
    toolbar: ["create", "save", "cancel"]
});
```

### Acesso via JavaScript

```javascript
const grid = $("#grid").data("kendoGrid");

// Obter dados
const dados = grid.dataSource.data();

// Obter linha selecionada
const selected = grid.select();
const dataItem = grid.dataItem(selected);

// Refresh
grid.dataSource.read();

// Adicionar registro
grid.dataSource.add({ nome: "Novo", preco: 0 });

// Remover registro
grid.dataSource.remove(dataItem);

// Exportar
grid.saveAsExcel();
grid.saveAsPDF();
```

### Referencia Kendo.Mvc.Examples

- `Views/Grid/Basic_Usage.cshtml` - Uso basico com CRUD
- `Views/Grid/Editing.cshtml` - Edicao inline
- `Views/Grid/Editing_Popup.cshtml` - Edicao em popup
- `Views/Grid/Filter_Row.cshtml` - Filtro por linha
- `Views/Grid/Excel_Export.cshtml` - Exportar Excel
- `Views/Grid/Pdf_Export.cshtml` - Exportar PDF
- `Views/Grid/Virtualization_Remote_Data.cshtml` - Dados remotos com virtual scroll

---

## 8. EDITOR (WYSIWYG)

### O que e

Editor de texto rico (WYSIWYG) para HTML. Substitui Syncfusion RichTextEditor no FrotiX.

### Quando Usar no FrotiX

- Campo de relatorio/descricao em Viagens/Upsert (textarea #rte)
- Campo de observacoes em Multa/UpsertAutuacao
- Qualquer campo que precise de texto formatado

### Arquivo Helper FrotiX: `kendo-editor-upsert.js`

O FrotiX possui um helper dedicado em `wwwroot/js/viagens/kendo-editor-upsert.js` que:

1. Inicializa o Kendo Editor em `<textarea id="rte">`
2. Fornece compatibilidade com API Syncfusion antiga
3. Expoe funcoes globais para get/set/clear

```javascript
// FUNCOES GLOBAIS DISPONIVEIS

// Inicializar (chamado automaticamente no document.ready)
initKendoEditorUpsert();

// Obter conteudo HTML
const html = getEditorUpsertValue();         // retorna string HTML

// Definir conteudo HTML
setEditorUpsertValue("<p>Texto...</p>");

// Limpar
clearEditorUpsert();

// Habilitar
enableEditorUpsert();

// Desabilitar
disableEditorUpsert();

// Destruir
destroyKendoEditorUpsert();
```

### Inicializacao via JavaScript (padrao FrotiX)

```javascript
$(textarea).kendoEditor({
    tools: [
        "bold", "italic", "underline", "strikethrough",
        "separator",
        "justifyLeft", "justifyCenter", "justifyRight", "justifyFull",
        "separator",
        "insertUnorderedList", "insertOrderedList",
        "separator",
        "indent", "outdent",
        "separator",
        "createLink", "unlink",
        "separator",
        "insertImage",
        "separator",
        "fontName", "fontSize",
        "separator",
        "foreColor", "backColor",
        "separator",
        "cleanFormatting",
        "separator",
        "viewHtml"
    ],
    resizable: { content: true, toolbar: false },
    messages: {
        bold: "Negrito",
        italic: "Italico",
        underline: "Sublinhado",
        viewHtml: "Ver HTML"
        // ... (ver kendo-editor-upsert.js para lista completa)
    },
    imageBrowser: {
        transport: {
            read: "/api/Viagem/ListarImagens",
            uploadUrl: "/api/Viagem/SaveImage"
        }
    }
}).data('kendoEditor');
```

### Sintaxe HtmlHelper (Kendo.Mvc.Examples)

```csharp
@(Html.Kendo().Editor()
    .Name("editor")
    .HtmlAttributes(new { style = "width: 100%; height:470px" })
    .StyleSheets(css => css.Add(Url.Content("~/css/editor-styles.css")))
    .ImageBrowser(ib => ib
        .Read("Read", "ImageBrowser")
        .Create("Create", "ImageBrowser")
        .Upload("Upload", "ImageBrowser")
        .Destroy("Destroy", "ImageBrowser")
    )
    .Value(@<text><p>Conteudo inicial...</p></text>)
)
```

### Acesso via JavaScript

```javascript
const editor = $("#rte").data("kendoEditor");

// Obter HTML
const html = editor.value();

// Definir HTML
editor.value("<p><strong>Texto formatado</strong></p>");

// Obter texto puro (sem tags HTML)
const texto = $(editor.body).text();

// Inserir HTML na posicao do cursor
editor.exec("inserthtml", { value: "<p>Novo paragrafo</p>" });

// Foco
editor.focus();

// Refresh
editor.refresh();

// Destruir
editor.destroy();
```

### Referencia Kendo.Mvc.Examples

- `Views/Editor/Basic_Usage.cshtml` - Editor com toolbar padrao
- `Views/Editor/All_Tools.cshtml` - Todas as ferramentas
- `Views/Editor/ImageBrowser.cshtml` - Upload de imagens
- `Views/Editor/Import_Export.cshtml` - Importar/Exportar
- `Views/Editor/Custom_Tools.cshtml` - Ferramentas customizadas
- `Views/Editor/Inline_Editing.cshtml` - Edicao inline (sem toolbar fixa)

---

## 9. PDFVIEWER

### O que e

Visualizador de PDF integrado na pagina, sem necessidade de plugins externos.

### Quando Usar no FrotiX

- Visualizacao de cupons de abastecimento (Abastecimento/UpsertCupons)
- Visualizacao de PDFs carregados (Uploads/UploadPDF)
- Visualizacao de cupons registrados (Abastecimento/RegistraCupons)

### Sintaxe JavaScript (usada no FrotiX)

```javascript
// Inicializar com URL de PDF
$("#pdfViewer").kendoPDFViewer({
    height: 400,
    pdfjsProcessing: {
        file: urlDoPdf
    }
});

// Obter instancia
var viewer = $("#pdfViewer").data("kendoPDFViewer");
```

### Carregar PDF Dinamicamente

```javascript
// Destruir instancia anterior se existir
const instanciaAnterior = $viewer.data("kendoPDFViewer");
if (instanciaAnterior) {
    instanciaAnterior.destroy();
}

// Criar nova instancia com novo PDF
$viewer.kendoPDFViewer({
    height: 400,
    pdfjsProcessing: {
        file: novaUrl
    }
});
```

### Sintaxe HtmlHelper

```csharp
@(Html.Kendo().PDFViewer()
    .Name("pdfViewer")
    .Height(600)
    .PdfjsProcessing(pdf => pdf.File(Url.Content("~/docs/documento.pdf")))
)
```

### Referencia Kendo.Mvc.Examples

- `Views/PDFViewer/index.cshtml` - Visualizacao basica
- `Views/PDFViewer/Api.cshtml` - API programatica
- `Views/PDFViewer/Form_Filling.cshtml` - Preenchimento de formularios PDF

---

## 10. UPLOAD

### O que e

Controle de upload de arquivos com drag-and-drop, progresso e validacao.

### Quando Usar no FrotiX

- Upload de cupons PDF (Abastecimento/UpsertCupons)
- Upload de documentos (Uploads/UploadPDF)
- Upload de imagens em manutencao

### Sintaxe JavaScript (usada no FrotiX)

```javascript
$("#pdf").kendoUpload({
    async: {
        saveUrl: "/api/Upload/Save",
        removeUrl: "/api/Upload/Remove",
        autoUpload: true
    },
    multiple: false,
    validation: {
        allowedExtensions: [".pdf"],
        maxFileSize: 10485760             // 10MB
    },
    localization: {
        select: "Selecionar arquivo...",
        dropFilesHere: "Arraste arquivos aqui",
        statusUploading: "Enviando...",
        statusUploaded: "Enviado",
        statusFailed: "Falha no envio"
    },
    success: function(e) {
        // Callback apos upload bem-sucedido
        const response = e.response;
    },
    error: function(e) {
        Alerta.Erro("Erro", "Falha ao enviar arquivo");
    }
});
```

### Sintaxe HtmlHelper

```csharp
@(Html.Kendo().Upload()
    .Name("files")
    .Async(a => a
        .Save("Async_Save", "Upload")
        .Remove("Async_Remove", "Upload")
        .AutoUpload(true)
    )
)
```

### Acesso via JavaScript

```javascript
const upload = $("#pdf").data("kendoUpload");

// Limpar arquivos
upload.clearAllFiles();

// Desabilitar
upload.enable(false);
```

### Referencia Kendo.Mvc.Examples

- `Views/Upload/BasicUsage.cshtml` - Upload sincrono
- `Views/Upload/AsyncUpload.cshtml` - Upload assincrono (AJAX)
- `Views/Upload/ChunkUpload.cshtml` - Upload em chunks (arquivos grandes)
- `Views/Upload/validation.cshtml` - Validacao de tipo/tamanho

---

## 11. TABSTRIP

### O que e

Controle de abas para organizar conteudo em tabs.

### Sintaxe JavaScript (usada no FrotiX)

```javascript
$("#tabstrip").kendoTabStrip({
    animation: {
        open: { effects: "fadeIn" }
    }
});
```

### Sintaxe HtmlHelper

```csharp
@(Html.Kendo().TabStrip()
    .Name("tabstrip")
    .Items(items => {
        items.Add().Text("Dados Gerais").Content("conteudo tab 1");
        items.Add().Text("Documentos").Content("conteudo tab 2");
        items.Add().Text("Historico").Content("conteudo tab 3");
    })
    .SelectedIndex(0)
)
```

### Sintaxe TagHelper

```html
<kendo-tabstrip name="tabstrip">
    <items>
        <tabstrip-item text="Dados Gerais" selected="true">
            <content>
                <p>Conteudo da aba 1</p>
            </content>
        </tabstrip-item>
        <tabstrip-item text="Documentos">
            <content>
                <p>Conteudo da aba 2</p>
            </content>
        </tabstrip-item>
    </items>
</kendo-tabstrip>
```

### Acesso via JavaScript

```javascript
const tabstrip = $("#tabstrip").data("kendoTabStrip");

// Selecionar aba por indice
tabstrip.select(0);                    // primeira aba
tabstrip.select(2);                    // terceira aba

// Selecionar aba por elemento
tabstrip.select(tabstrip.tabGroup.children().eq(1));

// Habilitar/Desabilitar aba
tabstrip.enable(tabstrip.tabGroup.children().eq(1), false);

// Adicionar aba dinamicamente
tabstrip.append({
    text: "Nova Aba",
    content: "<p>Conteudo novo</p>"
});
```

### Referencia Kendo.Mvc.Examples

- `Views/TabStrip/Index.cshtml` - Uso basico
- `Views/TabStrip/Api.cshtml` - API programatica
- `Views/TabStrip/Ajax.cshtml` - Conteudo carregado via AJAX
- `Views/TabStrip/Scrollable_Tabs.cshtml` - Abas com scroll
- `Views/TabStrip/Sortable_Tabs.cshtml` - Abas reordenaveis

---

## 12. NUMERICTTEXTBOX

### O que e

Campo numerico com botoes incrementar/decrementar, formatacao e validacao.

### Quando Usar no FrotiX

- Campos de valor monetario (preco, custo)
- Campos de quantidade (km, litros)

### Sintaxe TagHelper

```html
<kendo-numerictextbox name="txtValor"
    format="n2"
    min="0"
    max="999999.99"
    step="0.01"
    decimals="2"
    placeholder="0,00"
    style="width: 100%;">
</kendo-numerictextbox>
```

### Sintaxe HtmlHelper

```csharp
@(Html.Kendo().NumericTextBox()
    .Name("txtValor")
    .Format("n2")
    .Min(0)
    .Max(999999.99)
    .Step(0.01)
    .Decimals(2)
    .HtmlAttributes(new { style = "width: 100%" })
)
```

### Formatos Comuns

| Formato | Resultado | Uso |
|---------|-----------|-----|
| `"n2"` | 1.234,56 | Numeros decimais |
| `"c2"` | R$ 1.234,56 | Valores monetarios |
| `"p0"` | 85% | Percentuais |
| `"#,##0.00"` | 1.234,56 | Custom |

### Acesso via JavaScript

```javascript
const numeric = $("#txtValor").data("kendoNumericTextBox");
numeric.value();                       // obter valor
numeric.value(123.45);                 // definir valor
numeric.enable(false);                 // desabilitar
numeric.min(0);                        // definir minimo
numeric.max(1000);                     // definir maximo
```

### Referencia Kendo.Mvc.Examples

- `Views/NumericTextBox/Index.cshtml`
- `Views/NumericTextBox/Api.cshtml`
- `Views/NumericTextBox/Globalization.cshtml`

---

## 13. DEFERREDSCRIPTS (OBSOLETO - NAO USAR)

### Por que NAO usar no FrotiX

DeferredScripts e um mecanismo dos TagHelpers Kendo. Como o FrotiX **NAO usa TagHelpers**
(ver secao 0 - "Por que NAO usar TagHelpers"), DeferredScripts tambem nao deve ser usado.

### Abordagem Correta

Em vez de TagHelper + `deferred="true"` + `@Html.Kendo().DeferredScripts()`, usar:

1. `<input>` simples no HTML (corpo da pagina)
2. jQuery `$("#id").kendoXxx({...})` em `@section ScriptsBlock`

```html
<!-- ERRADO (NAO USAR) -->
<kendo-dropdownlist name="ddl" deferred="true" ...></kendo-dropdownlist>
@section ScriptsBlock {
    @Html.Kendo().DeferredScripts()
}

<!-- CORRETO (USAR ISSO) -->
<input id="ddl" name="ddl" style="width: 100%;" />
@section ScriptsBlock {
<script>
    $("#ddl").kendoDropDownList({ dataTextField: "descricao", dataValueField: "id", ... });
</script>
}
```

---

## 14. CONTROLES DISPONIVEIS (NAO USADOS AINDA NO FROTIX)

O projeto `Kendo.Mvc.Examples/` contem 149 controles. Abaixo os mais uteis para futuro uso:

| Controle | Pasta Examples | Uso Potencial no FrotiX |
|----------|---------------|------------------------|
| **Scheduler** | `Views/Scheduler/` | Calendario de escalas, agendamentos |
| **Gantt** | `Views/Gantt/` | Planejamento de manutencoes |
| **Chart** | `Views/Bar_Charts/`, `Views/Line_Charts/` | Dashboards |
| **TreeView** | `Views/TreeView/` | Hierarquia de setores/unidades |
| **Window** | `Views/Window/` | Modais ricos (alternativa a Bootstrap modal) |
| **Notification** | `Views/Notification/` | Notificacoes (alternativa a AppToast) |
| **Splitter** | `Views/Splitter/` | Layout de paineis |
| **FileManager** | `Views/FileManager/` | Gerenciamento de documentos |
| **Wizard** | `Views/Wizard/` | Formularios em etapas |
| **Form** | `Views/Form/` | Formularios validados |
| **Signature** | `Views/Signature/` | Assinatura digital |
| **Dialog** | `Views/Dialog/` | Dialogo de confirmacao |
| **MaskedTextBox** | `Views/MaskedTextBox/` | CPF, CNPJ, Telefone |
| **AutoComplete** | `Views/AutoComplete/` | Auto-completar em campos de texto |
| **DropDownTree** | `Views/DropDownTree/` | Selecao em arvore |
| **ColorPicker** | `Views/ColorPicker/` | Selecao de cores |
| **ListView** | `Views/ListView/` | Listagens customizadas (cards) |
| **OrgChart** | `Views/OrgChart/` | Organograma |

---

## 15. ARQUIVOS HELPER FROTIX EXISTENTES

### kendo-datetime.js

**Localização:** `wwwroot/js/agendamento/utils/kendo-datetime.js`

Helpers para DatePicker/TimePicker com fallback para input nativo.

| Funcao | Descricao |
|--------|-----------|
| `getKendoDatePicker(id)` | Retorna instancia do widget DatePicker |
| `getKendoTimePicker(id)` | Retorna instancia do widget TimePicker |
| `getKendoDateValue(id)` | Retorna Date do DatePicker (ou null) |
| `setKendoDateValue(id, value, triggerChange)` | Define data no DatePicker |
| `enableKendoDatePicker(id, enabled)` | Habilita/desabilita DatePicker |
| `showKendoDatePicker(id, show)` | Mostra/esconde DatePicker |
| `getKendoTimeValue(id)` | Retorna hora formatada "HH:mm" |
| `setKendoTimeValue(id, value)` | Define hora no TimePicker |
| `enableKendoTimePicker(id, enabled)` | Habilita/desabilita TimePicker |
| `showKendoTimePicker(id, show)` | Mostra/esconde TimePicker |

### kendo-editor-upsert.js

**Localizacao:** `wwwroot/js/viagens/kendo-editor-upsert.js`

Wrapper para Kendo Editor com compatibilidade Syncfusion.

| Funcao | Descricao |
|--------|-----------|
| `initKendoEditorUpsert()` | Inicializa editor em #rte (auto no document.ready) |
| `getEditorUpsertValue()` | Retorna HTML do editor |
| `setEditorUpsertValue(html)` | Define HTML no editor |
| `clearEditorUpsert()` | Limpa o editor |
| `enableEditorUpsert()` | Habilita edicao |
| `disableEditorUpsert()` | Desabilita edicao (readonly) |
| `destroyKendoEditorUpsert()` | Destroi instancia |

### kendo-editor-helper.js

**Localizacao:** `wwwroot/js/agendamento/utils/kendo-editor-helper.js`

Helper para Kendo Editor na pagina de Agenda (upload de imagens).

---

## 16. PADRAO DE ACESSO A WIDGETS (REGRA OBRIGATORIA)

### Padrao CORRETO

```javascript
// 1. Obter referencia ao widget
const widget = $("#meuElemento").data("kendo[NomeWidget]");

// 2. Verificar se existe antes de usar
if (widget) {
    widget.value("novo valor");
}
```

### Nomes dos widgets para .data()

| Controle | .data("kendo...") |
|----------|-------------------|
| DropDownList | `"kendoDropDownList"` |
| ComboBox | `"kendoComboBox"` |
| MultiSelect | `"kendoMultiSelect"` |
| DatePicker | `"kendoDatePicker"` |
| TimePicker | `"kendoTimePicker"` |
| DateTimePicker | `"kendoDateTimePicker"` |
| NumericTextBox | `"kendoNumericTextBox"` |
| Grid | `"kendoGrid"` |
| Editor | `"kendoEditor"` |
| PDFViewer | `"kendoPDFViewer"` |
| Upload | `"kendoUpload"` |
| TabStrip | `"kendoTabStrip"` |
| TreeView | `"kendoTreeView"` |
| Scheduler | `"kendoScheduler"` |
| Window | `"kendoWindow"` |
| Dialog | `"kendoDialog"` |
| MaskedTextBox | `"kendoMaskedTextBox"` |
| AutoComplete | `"kendoAutoComplete"` |

---

## 17. VERSAO E ARQUIVOS LOCAIS (CONFIGURACAO ATUAL)

### IMPORTANTE: Kendo UI e 100% LOCAL (nao CDN)

Todos os arquivos Kendo estao em `wwwroot/lib/kendo/2025.4.1321/`.
**NUNCA** adicionar referencias CDN `kendo.cdn.telerik.com` ou `kendostatic.com` nas paginas.

### Scripts Kendo UI (carregados em _ScriptsBasePlugins.cshtml)

```
~/lib/kendo/2025.4.1321/js/kendo.all.min.js
~/kendo-lic.js                                    (licenca client-side - OBRIGATORIO)
~/lib/kendo/2025.4.1321/js/kendo.aspnetmvc.min.js
```

### CSS Kendo UI (carregado em _Head.cshtml)

```
~/lib/kendo/2025.4.1321/styles/bootstrap-main.css
```

### Licenciamento (kendo-lic.js)

O arquivo `kendo-lic.js` contem `KendoLicensing.setScriptKey('...')` e e **OBRIGATORIO**
para Kendo 2025+. Sem ele, controles exibem erro de licenca no console.
Fonte original: `Kendo.Mvc.Examples/wwwroot/kendo-lic.js`

### Ordem de Carga CRITICA

1. jQuery 3.7.1 (GRUPO 1) - **DEVE** vir primeiro
2. Bootstrap 5.3.8 (GRUPO 3)
3. Kendo UI JS + kendo-lic.js (GRUPO 3B) - **DEVE** vir APOS jQuery
4. `@RenderSection("ScriptsBlock")` - onde os controles sao inicializados via jQuery

**NUNCA** carregar jQuery apos Kendo (destroi registros `$.fn` dos plugins).
**NUNCA** adicionar CDN Kendo 2022 (CSS ocean-blue/classic conflita com bootstrap-main 2025).

---

## 18. ERROS COMUNS E SOLUCOES

### Erro: "kendo is not defined" (MAIS COMUM)

**Causa:** TagHelpers (`<kendo-*>`) geram scripts inline em `@RenderBody()`, que executa
ANTES de `kendo.all.min.js` ser carregado em `_ScriptsBasePlugins.cshtml`.
**Solucao:** NAO usar TagHelpers. Usar `<input>` simples + jQuery init em `@section ScriptsBlock`.

### Erro: Controles cinza, baixos ou sem estilo

**Causa:** CSS Kendo 2022 (tema ocean-blue/classic) carregado junto com CSS Kendo 2025
(tema bootstrap-main). As versoes conflitam e sobrescrevem estilos.
**Solucao:** Remover TODAS as referencias CDN Kendo 2022 (`kendostatic.com`, `kendo.cdn.telerik.com/2022*`).
O CSS global `bootstrap-main.css` de `_Head.cshtml` e suficiente.

### Erro: Licenca / "License not found" no console

**Causa:** Kendo 2025+ requer licenca client-side via `KendoLicensing.setScriptKey()`.
**Solucao:** Garantir que `kendo-lic.js` esta carregado APOS `kendo.all.min.js` em `_ScriptsBasePlugins.cshtml`.
Fonte do arquivo: `Kendo.Mvc.Examples/wwwroot/kendo-lic.js`

### Erro: jQuery plugins destruidos ($.fn.kendoXxx undefined)

**Causa:** Um segundo jQuery carregado APOS os plugins Kendo terem sido registrados.
Acontecia com `vendors.bundle.js` que continha jQuery 3.5.1 embutido.
**Solucao:** NUNCA carregar jQuery depois de Kendo. Verificar se nenhum bundle inclui jQuery.

### Erro: `ItemType` ambiguidade de namespace

**Causa:** `@using Kendo.Mvc.UI` no topo da pagina conflita com `FrotiX.Models.ItemType`.
**Solucao:** NAO usar `@using Kendo.Mvc.UI` (desnecessario com abordagem jQuery init).

### Erro: Propriedades JSON com casing errado

**Causa:** `@Html.Raw(Json.Serialize(...))` gera JSON em camelCase (padrao ASP.NET Core).
O C# tem `Descricao` mas o JSON gera `descricao`.
**Solucao:** Usar camelCase no JavaScript: `data.descricao`, NAO `data.Descricao`.

### Erro: Widget nao inicializa

**Causa:** Kendo JS nao carregado ou jQuery nao disponivel.
**Solucao:** Verificar se kendo.all.min.js esta carregado (F12 > Network).

### Erro: `.data("kendoXxx")` retorna undefined

**Causa:** Widget ainda nao foi inicializado no momento da chamada.
**Solucao:** Garantir que o codigo de acesso roda APOS a inicializacao no ScriptsBlock.

### Erro: Widget inicializado duas vezes

**Causa:** Script de inicializacao executado novamente (ex: AJAX reload).
**Solucao:** Verificar e destruir antes de re-inicializar:
```javascript
const existente = $("#el").data("kendoDropDownList");
if (existente) existente.destroy();
// agora pode inicializar novamente
```

### Erro: `@` em JSDoc dentro de .cshtml

**Causa:** Razor interpreta `@` como sintaxe C#.
**Solucao:** Escapar com `@@` ou mover JS para arquivo `.js` separado.

### Erro: Cascata nao funciona

**Causa:** `auto-bind="false"` esquecido no dropdown filho.
**Solucao:** Sempre usar `auto-bind="false"` e `enable="false"` no filho.

### Erro: Formato de data errado

**Causa:** Format nao especificado ou cultura errada.
**Solucao:** Sempre usar `format="dd/MM/yyyy"` e `culture="pt-BR"`.

---

## VERSIONAMENTO

| Versao | Data | Descricao |
|--------|------|-----------|
| 2.0 | 09/02/2026 | Migracao TagHelper→jQuery init, CDN→Local, erros descobertos, licenciamento |
| 1.0 | 09/02/2026 | Criacao inicial com 13 controles documentados |
