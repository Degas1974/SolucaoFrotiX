# Pages/Abastecimento/UpsertCupons.cshtml

**Mudanca:** GRANDE | **+45** linhas | **-83** linhas

---

```diff
--- JANEIRO: Pages/Abastecimento/UpsertCupons.cshtml
+++ ATUAL: Pages/Abastecimento/UpsertCupons.cshtml
@@ -1,5 +1,4 @@
 @page
-
 @addTagHelper *, Microsoft.AspNetCore.Mvc.TagHelpers
 @using Syncfusion.EJ2
 @model FrotiX.Pages.Abastecimento.UpsertCuponsModel
@@ -42,11 +41,6 @@
         color: white;
     }
 </style>
-
-@section HeadBlock {
-    <link href="https://cdn.kendostatic.com/2022.1.412/styles/kendo.default-main.min.css" rel="stylesheet"
-        type="text/css" />
-}
 
 <form method="post" onkeypress="stopEnterSubmitting(event)" enctype="multipart/form-data">
     @Html.AntiForgeryToken()
@@ -207,10 +201,6 @@
 @section ScriptsBlock {
     <script src="https://cdnjs.cloudflare.com/ajax/libs/toastr.js/latest/js/toastr.min.js"></script>
 
-    <script src="https://cdn.kendostatic.com/2022.1.412/js/jszip.min.js"></script>
-    <script src="https://cdn.kendostatic.com/2022.1.412/js/kendo.all.min.js"></script>
-    <script src="https://cdn.kendostatic.com/2022.1.412/js/kendo.aspnetmvc.min.js"></script>
-
     <script src="https://cdnjs.cloudflare.com/ajax/libs/pdf.js/2.2.2/pdf.js"></script>
     <script>
         window.pdfjsLib.GlobalWorkerOptions.workerSrc = 'https://cdnjs.cloudflare.com/ajax/libs/pdf.js/2.2.2/pdf.worker.js';
@@ -219,20 +209,10 @@
     <script src="~/js/cadastros/tiraacento_001.js"></script>
 
     <script>
-            /**
-             * ═══════════════════════════════════════════════════════════════════════════
-             * PREVENÇÃO DE SUBMIT VIA ENTER
-             * ═══════════════════════════════════════════════════════════════════════════
-             * @@description Impede que o formulário seja enviado ao pressionar Enter
-            * Permite Enter apenas em elementos DIV(ex: Rich Text Editor)
-                * @@param { KeyboardEvent } e - Evento de teclado
-                    * @@returns { void}
-                    */
         function stopEnterSubmitting(e) {
             try {
                 try {
                     if (e.keyCode === 13) {
-                            /** @@type { HTMLElement } Elemento que disparou o evento */
                         var src = e.srcElement || e.target;
 
                         console.log(src && src.tagName ? src.tagName.toLowerCase() : 'unknown');
@@ -255,13 +235,7 @@
     </script>
 
     <script>
-            /**
-             * ═══════════════════════════════════════════════════════════════════════════
-             * SUBMIT DO FORMULÁRIO - VALIDAÇÃO CUSTOMIZADA
-             * ═══════════════════════════════════════════════════════════════════════════
-             * @@description Controla o submit do formulário através de botão visível
-            * Valida campos obrigatórios antes de acionar o botão escondido de submit
-                */
+
         $("#btnSubmit").on("click", function (event) {
             try {
                 event.preventDefault();
@@ -279,26 +253,11 @@
     </script>
 
     <script>
-            /**
-             * ═══════════════════════════════════════════════════════════════════════════
-             * RICH TEXT EDITOR - TOOLBAR E INICIALIZAÇÃO
-             * ═══════════════════════════════════════════════════════════════════════════
-             * @@description Configurações e handlers do Syncfusion Rich Text Editor
-            */
-
-            /**
-             * Handler de clique na toolbar do RTE
-             * @@description Configura token CSRF para upload de imagens
-            * @@param { Object } e - Evento de clique na toolbar
-                * @@returns { void}
-                */
         function toolbarClick(e) {
             try {
-
                 if (e.item.id === "rte_toolbar_Image") {
                     var element = document.getElementById('rte_upload');
                     if (element && element.ej2_instances && element.ej2_instances[0]) {
-
                         element.ej2_instances[0].uploading = function upload(args) {
                             var tokenField = document.getElementsByName('__RequestVerificationToken')[0];
                             if (tokenField) {
@@ -312,18 +271,12 @@
             }
         }
 
-            /**
-             * Inicialização da página
-             * @@description Carrega PDF existente se estiver editando registro
-            */
         $(document).ready(function () {
             try {
-
                 if ('@Model.RegistroCupomAbastecimentoObj.RegistroCupomAbastecimento.RegistroCupomId' !== '00000000-0000-0000-0000-000000000000') {
 
                     if ('@Model.RegistroCupomAbastecimentoObj.RegistroCupomAbastecimento.RegistroPDF' != null &&
                         '@Model.RegistroCupomAbastecimentoObj.RegistroCupomAbastecimento.RegistroPDF' !== '') {
-
                         $("#pdfViewer").kendoPDFViewer({
                             pdfjsProcessing: {
                                 file: "/DadosEditaveis/Cupons/" + '@Model.RegistroCupomAbastecimentoObj.RegistroCupomAbastecimento.RegistroPDF'
@@ -333,7 +286,6 @@
                         }).data("kendoPDFViewer");
                     }
                 } else {
-
                     document.getElementById("txtDataRegistro").setAttribute('value', '');
                 }
             } catch (error) {
@@ -343,53 +295,55 @@
     </script>
 
     <script>
-            /**
-             * ═══════════════════════════════════════════════════════════════════════════
-             * UPLOAD DE PDF - KENDO UPLOAD
-             * ═══════════════════════════════════════════════════════════════════════════
-             * @@description Configuração do componente Kendo Upload para PDFs de cupons
-            */
-
-            /**
-             * Desabilita a funcionalidade de arrastar e soltar
-             * @@description Override do método _supportsDrop do Kendo Upload
-            * @@returns { boolean } Sempre retorna false
-                */
-        kendo.ui.Upload.fn._supportsDrop = function () {
-            try {
-                return false;
-            } catch (error) {
-                Alerta.TratamentoErroComLinha("UpsertCupons.cshtml", "_supportsDrop", error);
-            }
-        };
-
-            /**
-             * Inicialização do Kendo Upload para PDFs
-             * @@description Configura endpoints, localização e validação
-            */
-        $("#pdf").kendoUpload({
-            async: {
-                saveUrl: "/Abastecimento/UpsertCupons?handler=SavePDF",
-                removeUrl: "/Abastecimento/UpsertCupons?handler=RemovePDF"
-            },
-            localization: {
-                select: "Selecione o Registro de Cupons de Abastecimento...",
-                headerStatusUploaded: "Arquivo Carregado",
-                uploadSuccess: "Arquivo Carregado com Sucesso"
-            },
-            validation: {
-                allowedExtensions: [".pdf"]
-            },
-            success: onSuccess
+        /****************************************************************************************
+         * ⚡ INICIALIZAÇÃO: Kendo Upload para PDF de Cupons
+         * --------------------------------------------------------------------------------------
+         * 🎯 OBJETIVO : Configurar upload de PDF desabilitando drag-drop e inicializando
+         * o widget kendoUpload com validação de extensão .pdf
+         * 📥 ENTRADAS : Elemento #pdf
+         * 📤 SAÍDAS : Widget Kendo Upload configurado
+         * 🔗 CHAMADA POR : $(document).ready()
+         * 📝 OBSERVAÇÕES : Código movido para $(document).ready() para garantir que
+         * kendo.ui.Upload está disponível antes de modificar o protótipo
+         ****************************************************************************************/
+        $(document).ready(function() {
+            try {
+
+                if (typeof kendo !== 'undefined' && kendo.ui && kendo.ui.Upload) {
+                    kendo.ui.Upload.fn._supportsDrop = function () {
+                        return false;
+                    };
+                }
+
+                $("#pdf").kendoUpload({
+                    async: {
+                        saveUrl: "/Abastecimento/UpsertCupons?handler=SavePDF",
+                        removeUrl: "/Abastecimento/UpsertCupons?handler=RemovePDF"
+                    },
+                    localization: {
+                        select: "Selecione o Registro de Cupons de Abastecimento...",
+                        headerStatusUploaded: "Arquivo Carregado",
+                        uploadSuccess: "Arquivo Carregado com Sucesso"
+                    },
+                    validation: {
+                        allowedExtensions: [".pdf"]
+                    },
+                    success: onSuccess
+                });
+            } catch (error) {
+                Alerta.TratamentoErroComLinha("UpsertCupons.cshtml", "kendoUpload.init", error);
+            }
         });
 
-            /**
-             * Callback de sucesso do upload
-             * @@description Processa arquivo enviado, atualiza campo hidden e exibe PDF
-            * @@param { Object } e - Evento de sucesso do Kendo Upload
-                * @@returns { void}
-                */
-            function onSuccess(e) {
+        /****************************************************************************************
+         * ⚡ FUNÇÃO: onSuccess
+         * --------------------------------------------------------------------------------------
+         * 🎯 OBJETIVO : Callback executado após upload bem-sucedido do PDF
+         * 📥 ENTRADAS : e [object] - Evento do Kendo Upload com dados dos arquivos
+         * 📤 SAÍDAS : Renderiza PDF no viewer
+         * 🔗 CHAMADA POR : kendoUpload success event
+         ****************************************************************************************/
+        function onSuccess(e) {
             try {
 
                 $("#pdfViewer").remove();
```
