# Pages/Multa/ExibePDFAutuacao.cshtml

**Mudanca:** MEDIA | **+6** linhas | **-19** linhas

---

```diff
--- JANEIRO: Pages/Multa/ExibePDFAutuacao.cshtml
+++ ATUAL: Pages/Multa/ExibePDFAutuacao.cshtml
@@ -50,8 +50,10 @@
 
                     <div id="divPainel" class="panel-content">
                         <div id="pdfContainer" style="height: 800px;">
-                            <ejs-pdfviewer id="pdfViewer" style="height:100%" serviceUrl="/api/PdfViewer"
-                                documentPath="">
+                            <ejs-pdfviewer id="pdfViewer"
+                                           style="height:100%"
+                                           serviceUrl="/api/PdfViewer"
+                                           documentPath="">
                             </ejs-pdfviewer>
                         </div>
                     </div>
@@ -62,26 +64,11 @@
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
-             * EXIBE PDF AUTUAÇÃO - VISUALIZADOR DE DOCUMENTO
-             * ═══════════════════════════════════════════════════════════════════════════
-             * @@description Carrega e exibe PDF de auto de infração(autuação).
-             * @@requires Syncfusion PdfViewer, AppToast
-            * @@file Multa / ExibePDFAutuacao.cshtml
-            */
 
-            /**
-             * Previne submit do form ao pressionar Enter
-             * @@param { KeyboardEvent } e - Evento de teclado
-            * @@description Intercepta tecla Enter exceto em divs contenteditable
-                */
         function stopEnterSubmitting(e) {
             try {
                 e = e || window.event;
```

### REMOVER do Janeiro

```html
                            <ejs-pdfviewer id="pdfViewer" style="height:100%" serviceUrl="/api/PdfViewer"
                                documentPath="">
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/css/bootstrap.min.css"
        crossorigin="anonymous" />
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/js/bootstrap.bundle.min.js"
        crossorigin="anonymous"></script>
            /**
             * ═══════════════════════════════════════════════════════════════════════════
             * EXIBE PDF AUTUAÇÃO - VISUALIZADOR DE DOCUMENTO
             * ═══════════════════════════════════════════════════════════════════════════
             * @@description Carrega e exibe PDF de auto de infração(autuação).
             * @@requires Syncfusion PdfViewer, AppToast
            * @@file Multa / ExibePDFAutuacao.cshtml
            */
            /**
             * Previne submit do form ao pressionar Enter
             * @@param { KeyboardEvent } e - Evento de teclado
            * @@description Intercepta tecla Enter exceto em divs contenteditable
                */
```


### ADICIONAR ao Janeiro

```html
                            <ejs-pdfviewer id="pdfViewer"
                                           style="height:100%"
                                           serviceUrl="/api/PdfViewer"
                                           documentPath="">
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/css/bootstrap.min.css" crossorigin="anonymous" />
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/js/bootstrap.bundle.min.js" crossorigin="anonymous"></script>
```
