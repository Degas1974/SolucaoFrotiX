# Super-Prompt: Migração Kendo/Telerik - FrotiX.Site.OLD

## Contexto e Problema Original

O projeto FrotiX.Site.OLD carregava controles Kendo/Telerik de **DUAS versões conflitantes**:

| Versão | Onde | Tipo |
|--------|------|------|
| **2022.1.412** | CDN (`kendo.cdn.telerik.com` e `cdn.kendostatic.com`) em ~6 páginas individuais | CSS classic theme (`kendo.common` + `kendo.default`) + JS completo |
| **2025.2.520** | Local (`~/lib/kendo/2025.2.520/`) via `_ScriptsBasePlugins.cshtml` global | CSS bootstrap-main theme + JS completo |

**Resultado**: Conflito de CSS (visual cinza/curto nos controles), conflito de JS (jQuery plugins registrados 2x), e erros de licenciamento.

---

## Dificuldades Encontradas e Soluções

### 1. `ReferenceError: kendo is not defined`

**Causa**: TagHelpers Kendo (`<kendo-dropdownlist>`) geram scripts inline no `@RenderBody()`, que executa ANTES do `_ScriptsBasePlugins.cshtml` carregar `kendo.all.min.js`.

**Ordem de renderização do Layout**:
```
@RenderBody()                    ← TagHelper scripts executam aqui (kendo NÃO existe ainda)
<partial name="_ScriptsBasePlugins" />  ← kendo.all.min.js carrega aqui
@RenderSection("ScriptsBlock")   ← scripts de página executam aqui (kendo JÁ existe)
```

**Tentativa 1 - `deferred="true"`**: Adicionamos `deferred="true"` nos TagHelpers + `@Html.Kendo().DeferredScripts()` no ScriptsBlock. Funcionou para atrasar execução, mas exigiu `@using Kendo.Mvc.UI` que causou ambiguidade com `FrotiX.Models.ItemType`.

**Solução final**: Usar `<input>` HTML simples + inicialização jQuery `.kendoDropDownList()` no `@section ScriptsBlock {}`, que roda APÓS kendo.all.min.js.

### 2. Conflito de CSS: Visual cinza/curto nos DropDownLists

**Causa**: Kendo 2022 CSS (`kendo.common.min.css` + `kendo.default.min.css`) = tema "classic" com seletores genéricos que sobrescrevem o tema "bootstrap-main" do Kendo 2025.

**Solução**: Remover TODAS as referências CSS do Kendo 2022 de cada página. O tema global `bootstrap-main.css` (2025.2.520) já fornece estilização correta.

### 3. Erro de licenciamento no Editor Kendo

**Causa**: Ao remover Kendo 2022, o Editor passou a usar Kendo 2025.2.520 que exige ativação de licença JavaScript (client-side).

**Investigação**: No projeto `Kendo.Mvc.Examples` (v2025.4.1321), o Editor funciona perfeitamente. Descobrimos que o "segredo" é um arquivo de licença: `wwwroot/kendo-lic.js`.

**Referência**: `d:\FrotiX\Solucao FrotiX 2026\Kendo.Mvc.Examples\Views\Shared\ExampleLayout.cshtml` (linha 23):
```html
<script data-src="@Url.Content("~/kendo-lic.js")"></script>
```

**Conteúdo de kendo-lic.js**:
```javascript
KendoLicensing.setScriptKey('..chave codificada...');
```

**Solução**: Copiamos `kendo-lic.js` de `Kendo.Mvc.Examples/wwwroot/` para `FrotiX.Site.OLD/wwwroot/` e adicionamos referência em `_ScriptsBasePlugins.cshtml` logo após `kendo.all.min.js`.

### 4. Ambiguidade `ItemType` entre namespaces

**Causa**: Adicionar `@using Kendo.Mvc.UI` globalmente em `_ViewImports.cshtml` conflita com `FrotiX.Models.ItemType` (mesmo nome de classe em ambos namespaces).

**Solução**: NUNCA usar `@using Kendo.Mvc.UI` globalmente. Se necessário em uma página específica, adicionar só nela. Mas a abordagem jQuery init elimina essa necessidade completamente.

### 5. Duplicação de cultura pt-BR

**Causa**: Cada página com CDN 2022 incluía suas próprias referências a `kendo.culture.pt-BR.min.js` e `kendo.messages.pt-BR.min.js`, além do global em `_ScriptsBasePlugins.cshtml`.

**Solução**: Remover de cada página individual. A configuração global já cobre todas as páginas.

---

## Arquitetura Correta (Pós-Migração)

### Carregamento Global (NÃO mexer - já está correto)

**`_Head.cshtml`** (linha 180):
```html
<link rel="stylesheet" href="~/lib/kendo/2025.2.520/styles/bootstrap-main.css" asp-append-version="true" />
```

**`_ScriptsBasePlugins.cshtml`** (linhas 764-769):
```html
<script src="~/lib/kendo/2025.2.520/js/kendo.all.min.js" asp-append-version="true"></script>
<script src="~/kendo-lic.js" asp-append-version="true"></script>
<script src="~/lib/kendo/2025.2.520/js/kendo.aspnetmvc.min.js" asp-append-version="true"></script>
<script src="~/lib/kendo/2025.2.520/js/cultures/kendo.culture.pt-BR.min.js" asp-append-version="true"></script>
<script src="~/lib/kendo/2025.2.520/js/messages/kendo.messages.pt-BR.min.js" asp-append-version="true"></script>
```

### Padrão para Páginas Individuais

**HTML** (dentro do form/body):
```html
<input id="meuControle" name="Model.Propriedade" style="width: 100%;" />
```

**JavaScript** (dentro de `@section ScriptsBlock { }`):
```javascript
// Roda APÓS kendo.all.min.js (carregado em _ScriptsBasePlugins)
$("#meuControle").kendoDropDownList({
    dataTextField: "descricao",      // camelCase (JSON serializado)
    dataValueField: "id",
    optionLabel: "Selecione...",
    dataSource: @Html.Raw(Json.Serialize(ViewData["dados"])),
    height: 200,
    value: "@(Model.Valor ?? "")"
});
```

### Regras Críticas

1. **NUNCA** adicionar CDN Kendo 2022 em páginas individuais
2. **NUNCA** usar `@using Kendo.Mvc.UI` globalmente
3. **SEMPRE** inicializar controles via jQuery no `@section ScriptsBlock {}`
4. **SEMPRE** usar camelCase nos nomes de campo (JSON serialization)
5. **NUNCA** duplicar referências de culture/messages (já global)
6. **kendo-lic.js** deve ser carregado IMEDIATAMENTE após kendo.all.min.js

---

## Inventário de Páginas (Pré-Migração)

| Página | Controles Kendo | CDN 2022? | TagHelper? | jQuery Init? | Status |
|--------|----------------|-----------|------------|--------------|--------|
| Viagens/Upsert.cshtml | DropDownList x3, Editor | ~~Sim~~ Removido | ~~Sim~~ Removido | ✅ Migrado | ✅ PRONTO |
| Viagens/Index.cshtml | Culture only | Sim (2022.1.412) | Não | Não | ⏳ Pendente |
| Agenda/Index.cshtml | ComboBox x2, DatePicker x2, Editor | Sim (2022.1.412) | Sim | Não | ⏳ Pendente |
| Manutencao/Upsert.cshtml | Culture only | Sim (2022.1.412) | Não | Não | ⏳ Pendente |
| Manutencao/ControleLavagem.cshtml | Culture only | Sim (2022.1.412 + 2022.3.913) | Não | Não | ⏳ Pendente |
| Multa/UpsertAutuacao.cshtml | Editor | Sim (2022.1.412) | Não | Não | ⏳ Pendente |
| Abastecimento/RegistraCupons.cshtml | Nenhum direto | Sim (2022.1.412) | Não | Não | ⏳ Pendente |
| Abastecimento/UpsertCupons.cshtml | Upload | Não CDN | Não | ✅ jQuery | ✅ Verificar |
| Uploads/UploadPDF.cshtml | Upload | Não CDN | Não | ✅ jQuery | ✅ Verificar |
| Viagens/TestGrid.cshtml | TabStrip | Não CDN | Não | ✅ jQuery | ✅ Verificar |
| Temp/Index.cshtml | Grid, ComboBox, DataSource | Não CDN | Não | ✅ jQuery | ✅ Verificar |
