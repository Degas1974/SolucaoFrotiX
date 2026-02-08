# Pages/Uploads/UploadPDF.cshtml

**Mudanca:** GRANDE | **+26** linhas | **-36** linhas

---

```diff
--- JANEIRO: Pages/Uploads/UploadPDF.cshtml
+++ ATUAL: Pages/Uploads/UploadPDF.cshtml
@@ -15,14 +15,7 @@
 }
 
 @section HeadBlock {
-    <link rel="stylesheet" href="https://kendo.cdn.telerik.com/2022.1.412/styles/kendo.default-v2.min.css" />
-    <script src="https://code.jquery.com/jquery-1.12.4.min.js"></script>
-    <script src="https://kendo.cdn.telerik.com/2022.1.412/js/kendo.all.min.js"></script>
-    <script src="https://kendo.cdn.telerik.com/2022.1.412/js/kendo.aspnetmvc.min.js"></script>
-    <script src="https://cdnjs.cloudflare.com/ajax/libs/pdf.js/2.2.2/pdf.js"></script>
-    <script>
-        window.pdfjsLib.GlobalWorkerOptions.workerSrc = 'https://cdnjs.cloudflare.com/ajax/libs/pdf.js/2.2.2/pdf.worker.js';
-    </script>
+
 }
 
 <h1>Upload de Notificação (PDF)</h1>
@@ -42,22 +35,26 @@
 
 @section ScriptsBlock {
 
-    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/css/bootstrap.min.css"
-        crossorigin="anonymous" />
-    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/js/bootstrap.bundle.min.js"
-        crossorigin="anonymous"></script>
+    <script>
+        /****************************************************************************************
+         * ⚡ INICIALIZAÇÃO: Kendo Upload para PDF de Notificação
+         * --------------------------------------------------------------------------------------
+         * 🎯 OBJETIVO : Configurar upload de PDF com visualização integrada
+         * 📥 ENTRADAS : Elemento #pdf
+         * 📤 SAÍDAS : Widget Kendo Upload + PDFViewer
+         * 🔗 CHAMADA POR : $(document).ready()
+         * 📝 OBSERVAÇÕES : Código movido para $(document).ready() para garantir que
+         * kendo.ui.Upload está disponível antes de modificar o protótipo
+         ****************************************************************************************/
+        $(document).ready(function () {
+            try {
 
-    <script>
-        kendo.ui.Upload.fn._supportsDrop = function () {
-            try {
-                return false;
-            } catch (error) {
-                Alerta.TratamentoErroComLinha("UploadPDF.cshtml", "_supportsDrop", error);
-            }
-        };
+                if (typeof kendo !== 'undefined' && kendo.ui && kendo.ui.Upload) {
+                    kendo.ui.Upload.fn._supportsDrop = function () {
+                        return false;
+                    };
+                }
 
-        $(function () {
-            try {
                 $("#pdf").kendoUpload({
                     async: {
                         saveUrl: "/Multa/UploadPDF?handler=Save",
@@ -98,20 +95,14 @@
             }
         });
 
-            /**
-             * ═══════════════════════════════════════════════════════════════════════════
-             * UPLOAD PDF - CALLBACK DE SUCESSO
-             * ═══════════════════════════════════════════════════════════════════════════
-             * @@description Callback executado após upload bem - sucedido do PDF.
-             * @@requires Kendo UI Upload, Kendo PDFViewer
-            * @@file Uploads / UploadPDF.cshtml
-            */
-
-            /**
-             * Callback de upload bem-sucedido
-             * @@param { Object } e - Evento do Kendo Upload com dados do arquivo
-            * @@description Exibe o PDF no viewer Kendo após upload
-                */
+        /****************************************************************************************
+         * ⚡ FUNÇÃO: onSuccess
+         * --------------------------------------------------------------------------------------
+         * 🎯 OBJETIVO : Callback executado após upload bem-sucedido do PDF
+         * 📥 ENTRADAS : e [object] - Evento do Kendo Upload com dados dos arquivos
+         * 📤 SAÍDAS : Renderiza PDF no viewer
+         * 🔗 CHAMADA POR : kendoUpload success event
+         ****************************************************************************************/
         function onSuccess(e) {
             try {
                 if (e.operation !== "upload" || !e.files || !e.files.length) return;
@@ -155,6 +146,5 @@
                 Alerta.TratamentoErroComLinha("UploadPDF.cshtml", "onSuccess", error);
             }
         }
-            }
     </script>
 }
```

### REMOVER do Janeiro

```html
    <link rel="stylesheet" href="https://kendo.cdn.telerik.com/2022.1.412/styles/kendo.default-v2.min.css" />
    <script src="https://code.jquery.com/jquery-1.12.4.min.js"></script>
    <script src="https://kendo.cdn.telerik.com/2022.1.412/js/kendo.all.min.js"></script>
    <script src="https://kendo.cdn.telerik.com/2022.1.412/js/kendo.aspnetmvc.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/pdf.js/2.2.2/pdf.js"></script>
    <script>
        window.pdfjsLib.GlobalWorkerOptions.workerSrc = 'https://cdnjs.cloudflare.com/ajax/libs/pdf.js/2.2.2/pdf.worker.js';
    </script>
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/css/bootstrap.min.css"
        crossorigin="anonymous" />
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/js/bootstrap.bundle.min.js"
        crossorigin="anonymous"></script>
    <script>
        kendo.ui.Upload.fn._supportsDrop = function () {
            try {
                return false;
            } catch (error) {
                Alerta.TratamentoErroComLinha("UploadPDF.cshtml", "_supportsDrop", error);
            }
        };
        $(function () {
            try {
            /**
             * ═══════════════════════════════════════════════════════════════════════════
             * UPLOAD PDF - CALLBACK DE SUCESSO
             * ═══════════════════════════════════════════════════════════════════════════
             * @@description Callback executado após upload bem - sucedido do PDF.
             * @@requires Kendo UI Upload, Kendo PDFViewer
            * @@file Uploads / UploadPDF.cshtml
            */
            /**
             * Callback de upload bem-sucedido
             * @@param { Object } e - Evento do Kendo Upload com dados do arquivo
            * @@description Exibe o PDF no viewer Kendo após upload
                */
            }
```


### ADICIONAR ao Janeiro

```html
    <script>
        /****************************************************************************************
         * ⚡ INICIALIZAÇÃO: Kendo Upload para PDF de Notificação
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO : Configurar upload de PDF com visualização integrada
         * 📥 ENTRADAS : Elemento #pdf
         * 📤 SAÍDAS : Widget Kendo Upload + PDFViewer
         * 🔗 CHAMADA POR : $(document).ready()
         * 📝 OBSERVAÇÕES : Código movido para $(document).ready() para garantir que
         * kendo.ui.Upload está disponível antes de modificar o protótipo
         ****************************************************************************************/
        $(document).ready(function () {
            try {
                if (typeof kendo !== 'undefined' && kendo.ui && kendo.ui.Upload) {
                    kendo.ui.Upload.fn._supportsDrop = function () {
                        return false;
                    };
                }
        /****************************************************************************************
         * ⚡ FUNÇÃO: onSuccess
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO : Callback executado após upload bem-sucedido do PDF
         * 📥 ENTRADAS : e [object] - Evento do Kendo Upload com dados dos arquivos
         * 📤 SAÍDAS : Renderiza PDF no viewer
         * 🔗 CHAMADA POR : kendoUpload success event
         ****************************************************************************************/
```
