# Pages/AtaRegistroPrecos/Upsert.cshtml

**Mudanca:** GRANDE | **+57** linhas | **-183** linhas

---

```diff
--- JANEIRO: Pages/AtaRegistroPrecos/Upsert.cshtml
+++ ATUAL: Pages/AtaRegistroPrecos/Upsert.cshtml
@@ -1,5 +1,4 @@
 @page
-
 @addTagHelper *, Microsoft.AspNetCore.Mvc.TagHelpers
 
 @model FrotiX.Pages.AtaRegistroPrecos.UpsertModel
@@ -91,8 +90,7 @@
                 <div class="ftx-card-header d-flex justify-content-between align-items-center">
                     <h2 class="titulo-paginas mb-0">
                         <i class="fa-duotone fa-folders"></i>
-                        @(Model.AtaObj.AtaRegistroPrecos.AtaId != Guid.Empty ? "Atualizar " : "Criar ") Ata Registro
-                        Preços
+                        @(Model.AtaObj.AtaRegistroPrecos.AtaId != Guid.Empty ? "Atualizar " : "Criar ") Ata Registro Preços
                     </h2>
                     <a asp-page="Index" class="btn btn-header-orange">
                         <i class="fa-duotone fa-rotate-left icon-space icon-rotate-left"></i> Voltar à Lista
@@ -108,46 +106,32 @@
                             <div class="row">
                                 <div class="col-2">
                                     <div class="form-group">
-                                        <label class="label font-weight-bold label-required"
-                                            asp-for="AtaObj.AtaRegistroPrecos.AnoAta">Ano da Ata</label>
-                                        <span class="text-danger"
-                                            asp-validation-for="AtaObj.AtaRegistroPrecos.AnoAta"></span>
-                                        <select id="txtAno" class="form-control form-control-xs"
-                                            asp-for="AtaObj.AtaRegistroPrecos.AnoAta"
-                                            asp-items="@((SelectList)ViewData["AnosList"])">
+                                        <label class="label font-weight-bold label-required" asp-for="AtaObj.AtaRegistroPrecos.AnoAta">Ano da Ata</label>
+                                        <span class="text-danger" asp-validation-for="AtaObj.AtaRegistroPrecos.AnoAta"></span>
+                                        <select id="txtAno" class="form-control form-control-xs" asp-for="AtaObj.AtaRegistroPrecos.AnoAta" asp-items="@((SelectList)ViewData["AnosList"])">
                                             <option value="">- Ano -</option>
                                         </select>
                                     </div>
                                 </div>
                                 <div class="col-2">
                                     <div class="form-group">
-                                        <label class="label font-weight-bold label-required"
-                                            asp-for="AtaObj.AtaRegistroPrecos.NumeroAta">Número da Ata</label>
-                                        <span class="text-danger"
-                                            asp-validation-for="AtaObj.AtaRegistroPrecos.NumeroAta"></span>
-                                        <input id="txtNumeroAta" class="form-control form-control-xs"
-                                            asp-for="AtaObj.AtaRegistroPrecos.NumeroAta" />
+                                        <label class="label font-weight-bold label-required" asp-for="AtaObj.AtaRegistroPrecos.NumeroAta">Número da Ata</label>
+                                        <span class="text-danger" asp-validation-for="AtaObj.AtaRegistroPrecos.NumeroAta"></span>
+                                        <input id="txtNumeroAta" class="form-control form-control-xs" asp-for="AtaObj.AtaRegistroPrecos.NumeroAta" />
                                     </div>
                                 </div>
                                 <div class="col-3">
                                     <div class="form-group">
-                                        <label class="label font-weight-bold label-required"
-                                            asp-for="AtaObj.AtaRegistroPrecos.NumeroProcesso">Número do Processo</label>
-                                        <span class="text-danger"
-                                            asp-validation-for="AtaObj.AtaRegistroPrecos.NumeroProcesso"></span>
-                                        <input id="txtNumeroProcesso" class="form-control form-control-xs"
-                                            asp-for="AtaObj.AtaRegistroPrecos.NumeroProcesso" />
+                                        <label class="label font-weight-bold label-required" asp-for="AtaObj.AtaRegistroPrecos.NumeroProcesso">Número do Processo</label>
+                                        <span class="text-danger" asp-validation-for="AtaObj.AtaRegistroPrecos.NumeroProcesso"></span>
+                                        <input id="txtNumeroProcesso" class="form-control form-control-xs" asp-for="AtaObj.AtaRegistroPrecos.NumeroProcesso" />
                                     </div>
                                 </div>
                                 <div class="col-2">
                                     <div class="form-group">
-                                        <label class="label font-weight-bold label-required"
-                                            asp-for="AtaObj.AtaRegistroPrecos.AnoProcesso">Ano do Processo</label>
-                                        <span class="text-danger"
-                                            asp-validation-for="AtaObj.AtaRegistroPrecos.AnoProcesso"></span>
-                                        <select id="txtAnoProcesso" class="form-control form-control-xs"
-                                            asp-for="AtaObj.AtaRegistroPrecos.AnoProcesso"
-                                            asp-items="@((SelectList)ViewData["AnosList"])">
+                                        <label class="label font-weight-bold label-required" asp-for="AtaObj.AtaRegistroPrecos.AnoProcesso">Ano do Processo</label>
+                                        <span class="text-danger" asp-validation-for="AtaObj.AtaRegistroPrecos.AnoProcesso"></span>
+                                        <select id="txtAnoProcesso" class="form-control form-control-xs" asp-for="AtaObj.AtaRegistroPrecos.AnoProcesso" asp-items="@((SelectList)ViewData["AnosList"])">
                                             <option value="">- Ano -</option>
                                         </select>
                                     </div>
@@ -157,25 +141,19 @@
                             <div class="row">
                                 <div class="col-6">
                                     <div class="form-group">
-                                        <label class="label font-weight-bold label-required"
-                                            asp-for="AtaObj.AtaRegistroPrecos.FornecedorId"></label>
-                                        <span class="text-danger"
-                                            asp-validation-for="AtaObj.AtaRegistroPrecos.FornecedorId"></span>
+                                        <label class="label font-weight-bold label-required" asp-for="AtaObj.AtaRegistroPrecos.FornecedorId"></label>
+                                        <span class="text-danger" asp-validation-for="AtaObj.AtaRegistroPrecos.FornecedorId"></span>
                                         @Html.DropDownListFor(m => m.AtaObj.AtaRegistroPrecos.FornecedorId,
                                         Model.AtaObj.FornecedorList,
-                                                                                "- Selecione um Fornecedor -",
-                                                                                new { @class = "form-control form-control-xs" })
+                                        "- Selecione um Fornecedor -",
+                                        new { @class = "form-control form-control-xs" })
                                     </div>
                                 </div>
                                 <div class="col-6">
                                     <div class="form-group">
-                                        <label class="label font-weight-bold label-required"
-                                            asp-for="AtaObj.AtaRegistroPrecos.Objeto"></label>
-                                        <span class="text-danger"
-                                            asp-validation-for="AtaObj.AtaRegistroPrecos.Objeto"></span>
-                                        <input id="txtObjeto" class="form-control form-control-xs"
-                                            asp-for="AtaObj.AtaRegistroPrecos.Objeto"
-                                            oninput="this.value = this.value.replace(/\b\w/g, l => l.toUpperCase())" />
+                                        <label class="label font-weight-bold label-required" asp-for="AtaObj.AtaRegistroPrecos.Objeto"></label>
+                                        <span class="text-danger" asp-validation-for="AtaObj.AtaRegistroPrecos.Objeto"></span>
+                                        <input id="txtObjeto" class="form-control form-control-xs" asp-for="AtaObj.AtaRegistroPrecos.Objeto" oninput="this.value = this.value.replace(/\b\w/g, l => l.toUpperCase())" />
                                     </div>
                                 </div>
                             </div>
@@ -183,34 +161,23 @@
                             <div class="row">
                                 <div class="col-3">
                                     <div class="form-group">
-                                        <label class="label font-weight-bold label-required"
-                                            asp-for="AtaObj.AtaRegistroPrecos.DataInicio"></label>
-                                        <span class="text-danger"
-                                            asp-validation-for="AtaObj.AtaRegistroPrecos.DataInicio"></span>
-                                        <input id="txtDataInicio" class="form-control form-control-xs" type="date"
-                                            asp-for="AtaObj.AtaRegistroPrecos.DataInicio" />
+                                        <label class="label font-weight-bold label-required" asp-for="AtaObj.AtaRegistroPrecos.DataInicio"></label>
+                                        <span class="text-danger" asp-validation-for="AtaObj.AtaRegistroPrecos.DataInicio"></span>
+                                        <input id="txtDataInicio" class="form-control form-control-xs" type="date" asp-for="AtaObj.AtaRegistroPrecos.DataInicio" />
                                     </div>
                                 </div>
                                 <div class="col-3">
                                     <div class="form-group">
-                                        <label class="label font-weight-bold label-required"
-                                            asp-for="AtaObj.AtaRegistroPrecos.DataFim"></label>
-                                        <span class="text-danger"
-                                            asp-validation-for="AtaObj.AtaRegistroPrecos.DataFim"></span>
-                                        <input id="txtDataFim" class="form-control form-control-xs" type="date"
-                                            asp-for="AtaObj.AtaRegistroPrecos.DataFim" />
+                                        <label class="label font-weight-bold label-required" asp-for="AtaObj.AtaRegistroPrecos.DataFim"></label>
+                                        <span class="text-danger" asp-validation-for="AtaObj.AtaRegistroPrecos.DataFim"></span>
+                                        <input id="txtDataFim" class="form-control form-control-xs" type="date" asp-for="AtaObj.AtaRegistroPrecos.DataFim" />
                                     </div>
                                 </div>
                                 <div class="col-3">
                                     <div class="form-group">
-                                        <label class="label font-weight-bold label-required"
-                                            asp-for="AtaObj.AtaRegistroPrecos.Valor"></label>
-                                        <span class="text-danger"
-                                            asp-validation-for="AtaObj.AtaRegistroPrecos.Valor"></span>
-                                        <input id="valor" class="form-control form-control-xs"
-                                            data-inputmask="'alias': 'currency'" style="text-align: right;"
-                                            value="@Model.AtaObj.AtaRegistroPrecos.Valor?.ToString("N2")"
-                                            onKeyPress="return(moeda(this,'.',',',event))" />
+                                        <label class="label font-weight-bold label-required" asp-for="AtaObj.AtaRegistroPrecos.Valor"></label>
+                                        <span class="text-danger" asp-validation-for="AtaObj.AtaRegistroPrecos.Valor"></span>
+                                        <input id="valor" class="form-control form-control-xs" data-inputmask="'alias': 'currency'" style="text-align: right;" value="@Model.AtaObj.AtaRegistroPrecos.Valor?.ToString("N2")" onKeyPress="return(moeda(this,'.',',',event))" />
                                     </div>
                                 </div>
                             </div>
@@ -222,43 +189,31 @@
                                     cols.Add(new { field = "numitem", direction = "Ascending" });
                                 }
                                 @{
-                                    <ejs-grid id="grdVeiculos"
-                                        toolbar="@(new List<string>() { "Add", "Update", "Delete", "Cancel" })"
-                                        GridLines="Both" QueryCellInfo="calculate" allowSorting="true">
-                                        <e-grid-editSettings allowAdding="true" allowDeleting="true" allowEditing="false"
-                                            newRowPosition="Bottom"></e-grid-editSettings>
+                                    <ejs-grid id="grdVeiculos" toolbar="@(new List<string>() { "Add", "Update", "Delete", "Cancel" })" GridLines="Both" QueryCellInfo="calculate" allowSorting="true">
+                                        <e-grid-editSettings allowAdding="true" allowDeleting="true" allowEditing="false" newRowPosition="Bottom"></e-grid-editSettings>
                                         <e-grid-sortsettings columns="cols"></e-grid-sortsettings>
                                         <e-grid-columns>
-                                            <e-grid-column field="numitem" headerText="Item" textAlign="Center" width="20"
-                                                allowEditing="true"></e-grid-column>
-                                            <e-grid-column field="descricao" headerText="Descrição do Veículo"
-                                                textAlign="Left" width="150" allowEditing="true"></e-grid-column>
-                                            <e-grid-column field="quantidade" headerText="Quantidade" textAlign="Center"
-                                                width="30" allowEditing="true"></e-grid-column>
-                                            <e-grid-column field="valorunitario" headerText="Valor Unitário"
-                                                textAlign="Right" width="30" allowEditing="true"
-                                                format="N2"></e-grid-column>
-                                            <e-grid-column field="valortotal" headerText="Valor Total" width="30"
-                                                textAlign="Right" allowEditing="false" format="N2"></e-grid-column>
+                                            <e-grid-column field="numitem" headerText="Item" textAlign="Center" width="20" allowEditing="true"></e-grid-column>
+                                            <e-grid-column field="descricao" headerText="Descrição do Veículo" textAlign="Left" width="150" allowEditing="true"></e-grid-column>
+                                            <e-grid-column field="quantidade" headerText="Quantidade" textAlign="Center" width="30" allowEditing="true"></e-grid-column>
+                                            <e-grid-column field="valorunitario" headerText="Valor Unitário" textAlign="Right" width="30" allowEditing="true" format="N2"></e-grid-column>
+                                            <e-grid-column field="valortotal" headerText="Valor Total" width="30" textAlign="Right" allowEditing="false" format="N2"></e-grid-column>
                                         </e-grid-columns>
                                     </ejs-grid>
                                 }
                                 <div class="col-3">
                                     <div class="form-group">
                                         <label class="label font-weight-bold">Total Geral</label>
-                                        <input id="txtTotal" class="form-control form-control-xs"
-                                            data-inputmask="'alias': 'currency'" style="text-align: right;" disabled />
+                                        <input id="txtTotal" class="form-control form-control-xs" data-inputmask="'alias': 'currency'" style="text-align: right;" disabled />
                                     </div>
                                 </div>
                             </div>
 
                             <div id="divVeiculosEdit" class="row" style="display:none">
                                 <div class="col-4">
-                                    <label class="label font-weight-bold">Selecione uma Repactuação para ver os
-                                        Veículos</label>
+                                    <label class="label font-weight-bold">Selecione uma Repactuação para ver os Veículos</label>
                                     <span class="text-danger font-weight-light"></span>
-                                    <select id="lstRepactuacao" name="lstRepactuacao"
-                                        class="form-control form-control-xs">
+                                    <select id="lstRepactuacao" name="lstRepactuacao" class="form-control form-control-xs">
                                         <option value="">-- Selecione uma Repactuacao --</option>
                                     </select>
                                 </div>
@@ -266,24 +221,18 @@
                                 <div>
                                     <ejs-grid id="grdVeiculos2" allowPaging="false" GridLines="Both">
                                         <e-grid-columns>
-                                            <e-grid-column field="numitem" headerText="Item" textAlign="Center"
-                                                width="20"></e-grid-column>
-                                            <e-grid-column field="descricao" headerText="Descrição do Veículo"
-                                                textAlign="Left" width="150"></e-grid-column>
-                                            <e-grid-column field="quantidade" headerText="Quantidade" textAlign="Center"
-                                                width="30" format="N0"></e-grid-column>
-                                            <e-grid-column field="valorunitario" headerText="Valor Unitário"
-                                                textAlign="Right" width="30" format="N2"></e-grid-column>
-                                            <e-grid-column field="valortotal" headerText="Valor Total" width="30"
-                                                textAlign="Right" format="N2"></e-grid-column>
+                                            <e-grid-column field="numitem" headerText="Item" textAlign="Center" width="20"></e-grid-column>
+                                            <e-grid-column field="descricao" headerText="Descrição do Veículo" textAlign="Left" width="150"></e-grid-column>
+                                            <e-grid-column field="quantidade" headerText="Quantidade" textAlign="Center" width="30" format="N0"></e-grid-column>
+                                            <e-grid-column field="valorunitario" headerText="Valor Unitário" textAlign="Right" width="30" format="N2"></e-grid-column>
+                                            <e-grid-column field="valortotal" headerText="Valor Total" width="30" textAlign="Right" format="N2"></e-grid-column>
                                         </e-grid-columns>
                                     </ejs-grid>
                                 </div>
                                 <div class="col-3">
                                     <div class="form-group">
                                         <label class="label font-weight-bold">Total Geral</label>
-                                        <input id="txtTotalEdit" class="form-control form-control-xs"
-                                            data-inputmask="'alias': 'currency'" style="text-align: right;" disabled />
+                                        <input id="txtTotalEdit" class="form-control form-control-xs" data-inputmask="'alias': 'currency'" style="text-align: right;" disabled />
                                     </div>
                                 </div>
                             </div>
@@ -295,8 +244,7 @@
                                     <div class="form-group">
                                         <label class="label font-weight-bold" for="chkAtivo">Ativo/Inativo</label>
                                         <br />
-                                        <ejs-switch id="chkAtivo"
-                                            ejs-for="AtaObj.AtaRegistroPrecos.Status"></ejs-switch>
+                                        <ejs-switch id="chkAtivo" ejs-for="AtaObj.AtaRegistroPrecos.Status"></ejs-switch>
                                     </div>
                                     <br />
                                 </div>
@@ -308,26 +256,20 @@
                                         <div class="col-6">
                                             @if (Model.AtaObj.AtaRegistroPrecos.AtaId != Guid.Empty)
                                             {
-                                                <button id="btnEdita" type="button" onclick="InsereRegistro()"
-                                                    class="btn btn-azul btn-submit-spin form-control">
-                                                    <i class="fa-duotone fa-floppy-disk icon-space icon-pulse"></i>
-                                                    Atualizar Ata
+                                                <button id="btnEdita" type="button" onclick="InsereRegistro()" class="btn btn-azul btn-submit-spin form-control">
+                                                    <i class="fa-duotone fa-floppy-disk icon-space icon-pulse"></i> Atualizar Ata
                                                 </button>
                                             }
                                             else
                                             {
-                                                <button id="btnAdiciona" type="button" onclick="InsereRegistro()"
-                                                    class="btn btn-azul btn-submit-spin form-control">
-                                                    <i class="fa-duotone fa-floppy-disk icon-space icon-pulse"></i> Criar
-                                                    Ata
+                                                <button id="btnAdiciona" type="button" onclick="InsereRegistro()" class="btn btn-azul btn-submit-spin form-control">
+                                                    <i class="fa-duotone fa-floppy-disk icon-space icon-pulse"></i> Criar Ata
                                                 </button>
                                             }
                                         </div>
                                         <div class="col-6">
-                                            <a asp-page="./Index" class="btn btn-ftx-fechar form-control"
-                                                data-ftx-loading>
-                                                <i class="fa-duotone fa-circle-xmark icon-space icon-pulse"></i>
-                                                Cancelar Operação
+                                            <a asp-page="./Index" class="btn btn-ftx-fechar form-control" data-ftx-loading>
+                                                <i class="fa-duotone fa-circle-xmark icon-space icon-pulse"></i> Cancelar Operação
                                             </a>
                                         </div>
                                     </div>
@@ -342,31 +284,13 @@
 </form>
 
 @section ScriptsBlock {
-    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/css/bootstrap.min.css"
-        crossorigin="anonymous" />
-    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/js/bootstrap.bundle.min.js"
-        crossorigin="anonymous"></script>
+    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/css/bootstrap.min.css" crossorigin="anonymous" />
+    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/js/bootstrap.bundle.min.js" crossorigin="anonymous"></script>
 
     <script>
-            /**
-             * ═══════════════════════════════════════════════════════════════════════════
-             * INSERÇÃO/EDIÇÃO DE ATA DE REGISTRO DE PREÇOS
-             * ═══════════════════════════════════════════════════════════════════════════
-             * @@description Gerencia a criação e edição de Atas de Registro de Preços.
-             * Valida campos obrigatórios, formata valores monetários e
-            * insere itens da ata via AJAX.
-             */
-
-            /**
-             * Insere ou atualiza uma Ata de Registro de Preços
-             * @@description Valida campos obrigatórios, converte valores monetários para float,
-             * e envia os dados via AJAX.No modo criação, também insere os itens
-            * da grid de veículos.
-             */
         function InsereRegistro() {
             try {
 
-                    /** @@type { string[] } Lista de campos com erro */
                 var erros = [];
 
                 if (!$('#txtAno').val()) erros.push("Ano da Ata");
@@ -391,7 +315,7 @@
 
                 if (erros.length > 0) {
                     var msg = "Por favor, preencha os seguintes campos:<br><br>" +
-                        erros.map(e => "• " + e).join("<br>");
+                              erros.map(e => "• " + e).join("<br>");
                     Alerta.Warning("Campos Obrigatórios não Informados", msg);
                     return;
                 }
@@ -404,7 +328,6 @@
                     valorata = valorata.replace('.', '');
                     valorata = valorata.replace(',', '.');
 
-                        /** @@type { string } JSON com dados da Ata para criação */
                     var objAta = JSON.stringify({
                         "AnoAta": $('#txtAno').val(),
                         "NumeroAta": $('#txtNumeroAta').val(),
@@ -476,7 +399,7 @@
                         },
                         error: function (data) {
                             try {
-                                AppToast.show('Vermelho', data.message,);
+                                AppToast.show('Vermelho', data.message, );
                                 console.log(data);
                             } catch (error) {
                                 Alerta.TratamentoErroComLinha("Upsert.cshtml", "InsereAta.error", error);
@@ -492,7 +415,6 @@
                     valorata = valorata.replace('.', '');
                     valorata = valorata.replace(',', '.');
 
-                        /** @@type { string } JSON com dados da Ata para atualização */
                     var objAta = JSON.stringify({
                         "AtaId": '@Model.AtaObj.AtaRegistroPrecos.AtaId',
                         "AnoAta": $('#txtAno').val(),
@@ -548,24 +470,6 @@
     </script>
 
     <script>
-            /**
-             * ═══════════════════════════════════════════════════════════════════════════
-             * FORMATAÇÃO DE VALORES MONETÁRIOS
-             * ═══════════════════════════════════════════════════════════════════════════
-             * @@description Formata valores monetários em tempo real durante a digitação.
-             * Utiliza formato brasileiro(1.234, 56).
-             */
-
-            /**
-             * Formata valor monetário durante digitação
-             * @@description Aplica máscara monetária brasileira com separadores de milhar
-            * e vírgula como separador decimal.
-             * @@param { HTMLInputElement } a - Elemento input sendo editado
-            * @@param { string } e - Separador de milhar(geralmente '.')
-                * @@param { string } r - Separador decimal(geralmente ',')
-                    * @@param { KeyboardEvent } t - Evento de teclado
-                        * @@returns { boolean } false para prevenir digitação inválida
-                            */
         function moeda(a, e, r, t) {
             try {
                 let n = "", h = j = 0, u = tamanho2 = 0, l = ajd2 = "", o = window.Event ? t.which : t.keyCode;
@@ -592,18 +496,9 @@
     </script>
 
     <script>
-            /**
-             * ═══════════════════════════════════════════════════════════════════════════
-             * VALIDAÇÃO DE DATAS E EVENTOS DA GRID
-             * ═══════════════════════════════════════════════════════════════════════════
-             * @@description Validação de período(Data Início / Fim) e callbacks da grid
-            * Syncfusion para cálculo automático de valores.
-             */
         document.addEventListener('DOMContentLoaded', function () {
             try {
-                /**
-                 * Validação de datas - Data Fim não pode ser anterior a Data Início
-                 */
+
                 $('#txtDataInicio, #txtDataFim').on('change', function () {
                     try {
                         var inicio = $('#txtDataInicio').val();
@@ -627,15 +522,7 @@
             }
         });
 
-            /**
-             * Callback para ação completa na grid Syncfusion
-             * @@description Atualiza o dataSource da grid após salvar um registro,
-             * removendo o item temporário e adicionando o novo.
-             * @@param { Object } args - Argumentos do evento Syncfusion
-            * @@param { string } args.requestType - Tipo de ação(save, delete, etc.)
-                * @@param { Object } args.data - Dados do registro afetado
-                    */
-            function actionComplete(args) {
+        function actionComplete(args) {
             try {
                 if (args.requestType == "save") {
                     var gridObj = document.getElementById('grdVeiculos').ej2_instances[0];
@@ -648,15 +535,6 @@
             }
         }
 
-            /**
-             * Calcula e exibe o valor total na célula da grid
-             * @@description Multiplica quantidade por valor unitário e atualiza
-            * o campo de valor total geral somando todos os itens.
-             * @@param { Object } args - Argumentos do evento queryCellInfo
-            * @@param { Object } args.data - Dados da linha atual
-                * @@param { Object } args.column - Informações da coluna
-                    * @@param { HTMLTableCellElement } args.cell - Célula sendo renderizada
-                        */
         function calculate(args) {
             try {
                 var valorunitario = args.data.valorunitario;
@@ -688,11 +566,6 @@
             }
         }
 
-            /**
-             * Handler de mudança do dropdown de repactuação
-             * @@description Carrega os itens da ata filtrados pela repactuação selecionada
-            * e atualiza a grid e o campo de valor total.
-             */
         document.getElementById("lstRepactuacao").addEventListener("change", function () {
             try {
                 RepactuacaoAtaId = document.getElementById("lstRepactuacao").value;
@@ -704,7 +577,6 @@
                     success: function (res) {
                         try {
                             console.log(res);
-                                /** @@type { Array } Itens da ata filtrados por repactuação */
                             objItens = res;
                             objItens = objItens.filter(function (obj) {
                                 return obj.repactuacaoId === RepactuacaoAtaId;
```
