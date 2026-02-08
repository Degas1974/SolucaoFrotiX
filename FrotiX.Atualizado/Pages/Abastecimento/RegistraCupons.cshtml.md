# Pages/Abastecimento/RegistraCupons.cshtml

**Mudanca:** GRANDE | **+17** linhas | **-98** linhas

---

```diff
--- JANEIRO: Pages/Abastecimento/RegistraCupons.cshtml
+++ ATUAL: Pages/Abastecimento/RegistraCupons.cshtml
@@ -17,7 +17,7 @@
 
 @section HeadBlock {
     <style>
-        /* ======== OUTLINE BRANCO NO BOTÃO DO HEADER (Padrão FrotiX) ======== */
+/* ======== OUTLINE BRANCO NO BOTÃO DO HEADER (Padrão FrotiX) ======== */
         .ftx-card-header .btn-fundo-laranja {
             outline: 2px solid rgba(255, 255, 255, 0.5) !important;
             outline-offset: 1px;
@@ -28,12 +28,8 @@
             outline: 2px solid rgba(255, 255, 255, 0.8) !important;
             outline-offset: 2px;
         }
+
     </style>
-    <link href="https://cdn.kendostatic.com/2022.1.412/styles/kendo.default-main.min.css" rel="stylesheet"
-        type="text/css" />
-    <script src="https://cdn.kendostatic.com/2022.1.412/js/jszip.min.js"></script>
-    <script src="https://cdn.kendostatic.com/2022.1.412/js/kendo.all.min.js"></script>
-    <script src="https://cdn.kendostatic.com/2022.1.412/js/kendo.aspnetmvc.min.js"></script>
     <script src="https://cdnjs.cloudflare.com/ajax/libs/pdf.js/2.2.2/pdf.js"></script>
     <script>
         window.pdfjsLib.GlobalWorkerOptions.workerSrc = 'https://cdnjs.cloudflare.com/ajax/libs/pdf.js/2.2.2/pdf.worker.js';
@@ -84,24 +80,9 @@
 </style>
 
 <script>
-    /**
-     * ═══════════════════════════════════════════════════════════════════════════
-     * VARIÁVEIS GLOBAIS - REGISTRO DE CUPONS
-     * ═══════════════════════════════════════════════════════════════════════════
-     * @@description Variáveis de configuração para chamadas à API de cupons
-     */
-
-    /** @@type {string} URL base da API de abastecimento */
     var URLapi = "api/abastecimento/ListaRegistroCupons";
-
-    /** @@type {string} Parâmetro ID para filtros da API */
     var IDapi = "";
 
-    /**
-     * Lista todos os registros de cupons de abastecimento
-     * @@description Carrega a tabela com todos os cupons cadastrados no sistema
-     * @@returns {void}
-     */
     function ListaTodosRegistros() {
         console.log("Lista Todos");
 
@@ -111,16 +92,8 @@
         ListaTblCupons(URLapi, IDapi);
     }
 
-    /**
-     * Configuração do Rich Text Editor (RTE) Syncfusion
-     * @@description Ajusta funcionamento do RTE dentro de modais
-     */
     var defaultRTE;
 
-    /**
-     * Callback de criação do RTE
-     * @@this {Object} Instância do RTE Syncfusion
-     */
     function onCreate() {
         defaultRTE = this;
     }
@@ -161,7 +134,7 @@
                                     <thead>
                                         <tr>
                                             <th>Data do Registro dos Cupons</th>
-                                            <th>Ações</th>
+                                            <th>Ação</th>
                                             <th></th>
                                         </tr>
                                     </thead>
@@ -192,8 +165,7 @@
                             <div class="col-auto">
                                 <div class="form-group">
                                     <label class="label font-weight-bold">Data de Vencimento</label>
-                                    <input id="txtDataRegistro" class="form-control form-control-xs" type="date"
-                                        disabled="disabled" />
+                                    <input id="txtDataRegistro" class="form-control form-control-xs" type="date" disabled="disabled" />
                                 </div>
                             </div>
                             <br />
@@ -215,18 +187,15 @@
                             </div>
                         </div>
                     </div>
-                    <br />
+                    <br/>
                     <div class="row">
                         <div class="col-12">
                             <div class="form-group">
                                 <div class="control-wrapper">
                                     <div>
-                                        <label class="label font-weight-bold">Observações a respeito do Registro de
-                                            Cupons</label>
-                                        <ejs-richtexteditor id="rte" toolbarClick="toolbarClick" locale="pt-BR"
-                                            height="150px" created="onCreate">
-                                            <e-richtexteditor-insertimagesettings saveUrl="api/Viagem/SaveImage"
-                                                path="./DadosEditaveis/ImagensViagens/"></e-richtexteditor-insertimagesettings>
+                                        <label class="label font-weight-bold">Observações a respeito do Registro de Cupons</label>
+                                        <ejs-richtexteditor id="rte" toolbarClick="toolbarClick" locale="pt-BR" height="150px" created="onCreate">
+                                            <e-richtexteditor-insertimagesettings saveUrl="api/Viagem/SaveImage" path="./DadosEditaveis/ImagensViagens/"></e-richtexteditor-insertimagesettings>
                                         </ejs-richtexteditor>
                                     </div>
                                 </div>
@@ -236,17 +205,15 @@
                     <br />
                 </form>
                 <div class="modal-footer">
-                    <button id="btnFechar" type="button" class="btn btn-vinho" data-bs-dismiss="modal"><i
-                            class="fa-duotone fa-circle-xmark icon-space icon-pulse"></i> Fechar</button>
+                    <button id="btnFechar" type="button" class="btn btn-vinho" data-bs-dismiss="modal"><i class="fa-duotone fa-circle-xmark icon-space icon-pulse"></i> Fechar</button>
                 </div>
             </div>
         </div>
     </div>
 </div>
 
-@section ScriptsBlock {
-    <link href="https://cdn.kendostatic.com/2022.1.412/styles/kendo.default-main.min.css" rel="stylesheet"
-        type="text/css" />
+@section ScriptsBlock{
+    <link href="https://cdn.kendostatic.com/2022.1.412/styles/kendo.default-main.min.css" rel="stylesheet" type="text/css" />
 
     <script src="https://cdn.kendostatic.com/2022.1.412/js/jszip.min.js"></script>
     <script src="https://cdn.kendostatic.com/2022.1.412/js/kendo.all.min.js"></script>
@@ -257,13 +224,7 @@
     </script>
 
     <script>
-        /**
-         * ═══════════════════════════════════════════════════════════════════════════
-         * INICIALIZAÇÃO DA PÁGINA - REGISTRO DE CUPONS
-         * ═══════════════════════════════════════════════════════════════════════════
-         */
         $(document).ready(function () {
-
             $("txtData").on('keyup', function (e) {
                 if (e.key === 'Enter' || e.keyCode === 13) {
                     document.getElementById('txtData').onchange();
@@ -273,24 +234,14 @@
             ListaTodosRegistros();
         });
 
-        /**
-         * Handler do botão Fechar do Modal
-         * @@description Remove a classe modal-backdrop para fechar o modal corretamente
-         */
         $("#btnFechar").click(function (e) {
             $("div").removeClass("modal-backdrop");
         });
     </script>
 
     <script>
-        /**
-         * ═══════════════════════════════════════════════════════════════════════════
-         * FILTRO POR DATA - REGISTRO DE CUPONS
-         * ═══════════════════════════════════════════════════════════════════════════
-         * @@description Filtra os registros de cupons pela data selecionada
-         */
+
         $("#txtData").change(function () {
-
             var date = $('#txtData').val().split("-");
             console.log(date, $('#txtData').val())
             day = date[2];
@@ -308,13 +259,6 @@
     </script>
 
     <script>
-        /**
-         * Carrega e renderiza a tabela de cupons de abastecimento
-         * @@description Configura DataTable com dados da API, exportação e paginação
-         * @@param {string} URLapi - Endpoint da API a ser chamado
-         * @@param {string} IDapi - Parâmetro de filtro (data ou ID)
-         * @@returns {void}
-         */
         function ListaTblCupons(URLapi, IDapi) {
 
             $('#divCupons').LoadingScript('method_12', {
@@ -377,14 +321,14 @@
                         "data": "registroCupomId",
                         "render": function (data, type, full, meta) {
                             return `<div class="text-center">
-                                                    <a href="/Abastecimento/UpsertCupons?id=${data}" class="btn btn_topo fundo-azul btn-xs text-white" aria-label="Editar o Registro!" data-microtip-position="top" role="tooltip" style="cursor:pointer;">
-                                                        <i class="fa-duotone fa-edit"></i>
-                                                    </a>
-
-                                                    <a class="btn btn_topo btn-apagar btn-vinho btn-xs text-white" aria-label="Apagar a lista de Cupons!" data-microtip-position="top" role="tooltip" style="cursor:pointer;" data-id='${data}'>
-                                                        <i class="fa-duotone fa-window-close"></i>
-                                                    </a>
-                                                </div>`;
+                                        <a href="/Abastecimento/UpsertCupons?id=${data}" class="btn btn_topo fundo-azul btn-xs text-white" aria-label="Editar o Registro!" data-microtip-position="top" role="tooltip" style="cursor:pointer;">
+                                            <i class="fa-duotone fa-edit"></i>
+                                        </a>
+
+                                        <a class="btn btn_topo btn-apagar btn-vinho btn-xs text-white" aria-label="Apagar a lista de Cupons!" data-microtip-position="top" role="tooltip" style="cursor:pointer;" data-id='${data}'>
+                                            <i class="fa-duotone fa-window-close"></i>
+                                        </a>
+                                    </div>`;
                         }
                     },
                     {
@@ -604,15 +548,8 @@
     </script>
 
     <script>
-        /**
-         * ═══════════════════════════════════════════════════════════════════════════
-         * EXCLUSÃO DE REGISTRO - CUPOM DE ABASTECIMENTO
-         * ═══════════════════════════════════════════════════════════════════════════
-         * @@description Handler para botão de exclusão de cupom
-         * Exibe confirmação SweetAlert e envia requisição DELETE à API
-         */
+
         $(document).on('click', '.btn-apagar', function () {
-            /** @@type {number} ID do registro a ser excluído */
             var id = $(this).data('id');
 
             swal({
@@ -626,10 +563,8 @@
                 }
             }).then((willDelete) => {
                 if (willDelete) {
-
                     var dataToPost = JSON.stringify({ 'IDapi': id });
                     var url = '/api/Abastecimento/DeleteRegistro';
-
                     $.ajax({
                         url: url,
                         type: "GET",
@@ -637,7 +572,6 @@
                         contentType: "application/json; charset=utf-8",
                         dataType: "json",
                         success: function (data) {
-
                             if (data.success) {
                                 AppToast.show('Verde', data.message);
                                 ListaTodosRegistros();
@@ -657,19 +591,6 @@
     </script>
 
     <script>
-        /**
-         * ═══════════════════════════════════════════════════════════════════════════
-         * MODAL DE VISUALIZAÇÃO - DETALHES DO CUPOM
-         * ═══════════════════════════════════════════════════════════════════════════
-         * @@description Handlers para abertura e fechamento do modal de visualização
-         * Carrega PDF do cupom e observações no Rich Text Editor (Syncfusion)
-         */
-
-        /**
-         * Handler de abertura do modal
-         * @@param {Event} event - Evento Bootstrap Modal shown
-         * @@description Carrega dados do cupom via AJAX e renderiza PDF com Kendo
-         */
         $('#modalRegistro').on('shown.bs.modal', function (event) {
 
             $("#PDFContainer").append("<div id='pdfViewer'> </div>");
@@ -686,9 +607,7 @@
                 dataType: "json",
                 data: { IDapi: RegistroId },
                 success: function (data) {
-                    /** @@type {string} Nome do arquivo PDF do cupom */
                     var RegistroPDF = data.registroPDF;
-                    /** @@type {string} Observações em HTML do cupom */
                     var Observacoes = data.observacoes;
 
                     var rte = document.getElementById('rte').ej2_instances[0];
@@ -707,13 +626,7 @@
                     console.log(data);
                 }
             });
-        /**
-         * Handler de fechamento do modal
-         * @@param {Event} event - Evento Bootstrap Modal hide
-         * @@description Limpa RTE, data e remove container PDF
-         */
         }).on("hide.bs.modal", function (event) {
-
             var rte = document.getElementById('rte').ej2_instances[0];
             rte.value = '';
 
```
