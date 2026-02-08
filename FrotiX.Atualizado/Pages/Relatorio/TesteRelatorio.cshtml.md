# Pages/Relatorio/TesteRelatorio.cshtml

**Mudanca:** MEDIA | **+0** linhas | **-14** linhas

---

```diff
--- JANEIRO: Pages/Relatorio/TesteRelatorio.cshtml
+++ ATUAL: Pages/Relatorio/TesteRelatorio.cshtml
@@ -39,11 +39,6 @@
 
 @section HeadBlock {
 
-    <link href="https://cdn.kendostatic.com/2022.1.412/styles/kendo.default-main.min.css" rel="stylesheet" type="text/css" />
-    <script src="https://cdn.kendostatic.com/2022.1.412/js/jquery.min.js"></script>
-    <script src="https://cdn.kendostatic.com/2022.1.412/js/jszip.min.js"></script>
-    <script src="https://cdn.kendostatic.com/2022.1.412/js/kendo.all.min.js"></script>
-    <script src="https://cdn.kendostatic.com/2022.1.412/js/kendo.aspnetmvc.min.js"></script>
     <script src="https://cdnjs.cloudflare.com/ajax/libs/pdf.js/2.2.2/pdf.js"></script>
     <script>
         window.pdfjsLib.GlobalWorkerOptions.workerSrc = 'https://cdnjs.cloudflare.com/ajax/libs/pdf.js/2.2.2/pdf.worker.js';
@@ -74,16 +69,6 @@
 @section ScriptsBlock
 {
 
-    <link href="https://cdn.kendostatic.com/2022.1.412/styles/kendo.default-main.min.css" rel="stylesheet" type="text/css" />
-    <script src="https://cdn.kendostatic.com/2022.1.412/js/jquery.min.js"></script>
-    <script src="https://cdn.kendostatic.com/2022.1.412/js/jszip.min.js"></script>
-    <script src="https://cdn.kendostatic.com/2022.1.412/js/kendo.all.min.js"></script>
-    <script src="https://cdn.kendostatic.com/2022.1.412/js/kendo.aspnetmvc.min.js"></script>
-    <script src="https://cdnjs.cloudflare.com/ajax/libs/pdf.js/2.2.2/pdf.js"></script>
-    <script>
-        window.pdfjsLib.GlobalWorkerOptions.workerSrc = 'https://cdnjs.cloudflare.com/ajax/libs/pdf.js/2.2.2/pdf.worker.js';
-    </script>
-
     <script src="/api/reports/resources/js/telerikReportViewer"></script>
 
     <script type="text/javascript">
```

### REMOVER do Janeiro

```html
    <link href="https://cdn.kendostatic.com/2022.1.412/styles/kendo.default-main.min.css" rel="stylesheet" type="text/css" />
    <script src="https://cdn.kendostatic.com/2022.1.412/js/jquery.min.js"></script>
    <script src="https://cdn.kendostatic.com/2022.1.412/js/jszip.min.js"></script>
    <script src="https://cdn.kendostatic.com/2022.1.412/js/kendo.all.min.js"></script>
    <script src="https://cdn.kendostatic.com/2022.1.412/js/kendo.aspnetmvc.min.js"></script>
    <link href="https://cdn.kendostatic.com/2022.1.412/styles/kendo.default-main.min.css" rel="stylesheet" type="text/css" />
    <script src="https://cdn.kendostatic.com/2022.1.412/js/jquery.min.js"></script>
    <script src="https://cdn.kendostatic.com/2022.1.412/js/jszip.min.js"></script>
    <script src="https://cdn.kendostatic.com/2022.1.412/js/kendo.all.min.js"></script>
    <script src="https://cdn.kendostatic.com/2022.1.412/js/kendo.aspnetmvc.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/pdf.js/2.2.2/pdf.js"></script>
    <script>
        window.pdfjsLib.GlobalWorkerOptions.workerSrc = 'https://cdnjs.cloudflare.com/ajax/libs/pdf.js/2.2.2/pdf.worker.js';
    </script>
```

