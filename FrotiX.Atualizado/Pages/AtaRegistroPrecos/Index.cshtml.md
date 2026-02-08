# Pages/AtaRegistroPrecos/Index.cshtml

**Mudanca:** MEDIA | **+4** linhas | **-6** linhas

---

```diff
--- JANEIRO: Pages/AtaRegistroPrecos/Index.cshtml
+++ ATUAL: Pages/AtaRegistroPrecos/Index.cshtml
@@ -1,5 +1,4 @@
 @page
-
 @model FrotiX.Models.AtaRegistroPrecos
 
 @{
@@ -138,13 +137,12 @@
             border-radius: 8px !important;
             padding: 8px 12px !important;
             font-size: 13px !important;
-            box-shadow: 0 2px 8px rgba(0, 0, 0, 0.15) !important;
+            box-shadow: 0 2px 8px rgba(0,0,0,0.15) !important;
             text-align: left !important;
             white-space: normal !important;
             line-height: 1.4 !important;
             max-width: 300px !important;
         }
-
         .tooltip-ftx-syncfusion .tooltip-arrow {
             display: none !important;
         }
@@ -180,7 +178,7 @@
                                     <th>Vigência</th>
                                     <th>Valor</th>
                                     <th>Status</th>
-                                    <th>Ações</th>
+                                    <th>Ação</th>
                                 </tr>
                             </thead>
                             <tbody>
@@ -195,10 +193,8 @@
 
 @section ScriptsBlock {
 
-    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/css/bootstrap.min.css"
-        crossorigin="anonymous" />
-    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/js/bootstrap.bundle.min.js"
-        crossorigin="anonymous"></script>
+    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/css/bootstrap.min.css" crossorigin="anonymous" />
+    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/js/bootstrap.bundle.min.js" crossorigin="anonymous"></script>
 
     <script src="~/js/cadastros/ata.js" asp-append-version="true"></script>
 }
```

### REMOVER do Janeiro

```html
            box-shadow: 0 2px 8px rgba(0, 0, 0, 0.15) !important;
                                    <th>Ações</th>
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/css/bootstrap.min.css"
        crossorigin="anonymous" />
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/js/bootstrap.bundle.min.js"
        crossorigin="anonymous"></script>
```


### ADICIONAR ao Janeiro

```html
            box-shadow: 0 2px 8px rgba(0,0,0,0.15) !important;
                                    <th>Ação</th>
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/css/bootstrap.min.css" crossorigin="anonymous" />
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/js/bootstrap.bundle.min.js" crossorigin="anonymous"></script>
```
