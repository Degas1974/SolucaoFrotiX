# Pages/Multa/UploadPDF.cshtml

**Mudanca:** GRANDE | **+13** linhas | **-34** linhas

---

```diff
--- JANEIRO: Pages/Multa/UploadPDF.cshtml
+++ ATUAL: Pages/Multa/UploadPDF.cshtml
@@ -30,18 +30,24 @@
 
 <div class="row">
     <div class="col-md-8">
-        <ejs-uploader id="uploaderPDF" multiple="false" autoUpload="true" allowedExtensions=".pdf"
-            success="onUploadSuccess" failure="onUploadFailure" removing="onFileRemoving">
+        <ejs-uploader id="uploaderPDF"
+                      multiple="false"
+                      autoUpload="true"
+                      allowedExtensions=".pdf"
+                      success="onUploadSuccess"
+                      failure="onUploadFailure"
+                      removing="onFileRemoving">
             <e-uploader-asyncsettings saveUrl="/Multa/UploadPDF?handler=Save"
-                removeUrl="/Multa/UploadPDF?handler=Remove">
+                                      removeUrl="/Multa/UploadPDF?handler=Remove">
             </e-uploader-asyncsettings>
         </ejs-uploader>
 
         <input type="text" id="txtArquivo" class="form-control" placeholder="nome do arquivo..." readonly />
 
         <div id="pdfViewerContainer" style="margin-top: 20px;">
-            <ejs-pdfviewer id="pdfviewer" style="height:600px;width:100%;display:none;"
-                serviceUrl="/api/MultaPdfViewer">
+            <ejs-pdfviewer id="pdfviewer"
+                           style="height:600px;width:100%;display:none;"
+                           serviceUrl="/api/MultaPdfViewer">
             </ejs-pdfviewer>
         </div>
     </div>
@@ -49,25 +55,10 @@
 
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
-             * UPLOAD DE PDF - HANDLERS DO UPLOADER SYNCFUSION
-             * ═══════════════════════════════════════════════════════════════════════════
-             * @@description Gerencia upload de arquivos PDF de multas / autuações
-            * com preview no PdfViewer Syncfusion.
-             */
-
-            /**
-             * Handler de sucesso no upload.
-             * @@param { Object } args - Argumentos do evento success.
-             * @@description Carrega PDF no viewer e exibe toast de confirmação.
-             */
         function onUploadSuccess(args) {
             try {
                 if (args.operation === 'upload') {
@@ -92,11 +83,6 @@
             }
         }
 
-            /**
-             * Handler de falha no upload.
-             * @@param { Object } args - Argumentos do evento failure.
-             * @@description Exibe toast de erro.
-             */
         function onUploadFailure(args) {
             try {
                 AppToast.show("Vermelho", "Falha no upload do arquivo!", 3000);
@@ -106,11 +92,6 @@
             }
         }
 
-            /**
-             * Handler de remoção de arquivo.
-             * @@param { Object } args - Argumentos do evento removing.
-             * @@description Limpa campo, oculta viewer e descarrega PDF.
-             */
         function onFileRemoving(args) {
             try {
                 $("#txtArquivo").val('');
@@ -128,9 +109,6 @@
             }
         }
 
-        /**
-         * Configura token anti-CSRF no uploader após DOM pronto.
-         */
         document.addEventListener('DOMContentLoaded', function () {
             try {
                 const uploader = document.getElementById('uploaderPDF');
```

### REMOVER do Janeiro

```html
        <ejs-uploader id="uploaderPDF" multiple="false" autoUpload="true" allowedExtensions=".pdf"
            success="onUploadSuccess" failure="onUploadFailure" removing="onFileRemoving">
                removeUrl="/Multa/UploadPDF?handler=Remove">
            <ejs-pdfviewer id="pdfviewer" style="height:600px;width:100%;display:none;"
                serviceUrl="/api/MultaPdfViewer">
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/css/bootstrap.min.css"
        crossorigin="anonymous" />
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/js/bootstrap.bundle.min.js"
        crossorigin="anonymous"></script>
            /**
             * ═══════════════════════════════════════════════════════════════════════════
             * UPLOAD DE PDF - HANDLERS DO UPLOADER SYNCFUSION
             * ═══════════════════════════════════════════════════════════════════════════
             * @@description Gerencia upload de arquivos PDF de multas / autuações
            * com preview no PdfViewer Syncfusion.
             */
            /**
             * Handler de sucesso no upload.
             * @@param { Object } args - Argumentos do evento success.
             * @@description Carrega PDF no viewer e exibe toast de confirmação.
             */
            /**
             * Handler de falha no upload.
             * @@param { Object } args - Argumentos do evento failure.
             * @@description Exibe toast de erro.
             */
            /**
             * Handler de remoção de arquivo.
             * @@param { Object } args - Argumentos do evento removing.
             * @@description Limpa campo, oculta viewer e descarrega PDF.
             */
        /**
         * Configura token anti-CSRF no uploader após DOM pronto.
         */
```


### ADICIONAR ao Janeiro

```html
        <ejs-uploader id="uploaderPDF"
                      multiple="false"
                      autoUpload="true"
                      allowedExtensions=".pdf"
                      success="onUploadSuccess"
                      failure="onUploadFailure"
                      removing="onFileRemoving">
                                      removeUrl="/Multa/UploadPDF?handler=Remove">
            <ejs-pdfviewer id="pdfviewer"
                           style="height:600px;width:100%;display:none;"
                           serviceUrl="/api/MultaPdfViewer">
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/css/bootstrap.min.css" crossorigin="anonymous" />
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/js/bootstrap.bundle.min.js" crossorigin="anonymous"></script>
```
