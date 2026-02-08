# Pages/Shared/_Head.cshtml

**Mudanca:** MEDIA | **+12** linhas | **-13** linhas

---

```diff
--- JANEIRO: Pages/Shared/_Head.cshtml
+++ ATUAL: Pages/Shared/_Head.cshtml
@@ -30,13 +30,13 @@
 <link rel="stylesheet" type="text/css" href="
 
 <link rel="stylesheet" href="~/css/site.css" />
-<link href="https://cdn.syncfusion.com/ej2/32.1.19/bootstrap5.css" rel="stylesheet" />
-<link href="https://cdn.syncfusion.com/ej2/32.1.19/ej2-base/styles/bootstrap5.css" rel="stylesheet" />
-<link href="https://cdn.syncfusion.com/ej2/32.1.19/ej2-buttons/styles/bootstrap5.css" rel="stylesheet" />
-<link href="https://cdn.syncfusion.com/ej2/32.1.19/ej2-inputs/styles/bootstrap5.css" rel="stylesheet" />
-<link href="https://cdn.syncfusion.com/ej2/32.1.19/ej2-popups/styles/bootstrap5.css" rel="stylesheet" />
-<link href="https://cdn.syncfusion.com/ej2/32.1.19/ej2-dropdowns/styles/bootstrap5.css" rel="stylesheet" />
-<link href="https://cdn.syncfusion.com/ej2/32.1.19/ej2-dropdowns/styles/bootstrap5.css" rel="stylesheet" />
+<link href="https://unpkg.com/@@syncfusion/ej2@@32.1.23/bootstrap5.css" rel="stylesheet" />
+<link href="https://unpkg.com/@@syncfusion/ej2-base@@32.1.23/styles/bootstrap5.css" rel="stylesheet" />
+<link href="https://unpkg.com/@@syncfusion/ej2-buttons@@32.1.23/styles/bootstrap5.css" rel="stylesheet" />
+<link href="https://unpkg.com/@@syncfusion/ej2-inputs@@32.1.23/styles/bootstrap5.css" rel="stylesheet" />
+<link href="https://unpkg.com/@@syncfusion/ej2-popups@@32.1.23/styles/bootstrap5.css" rel="stylesheet" />
+<link href="https://unpkg.com/@@syncfusion/ej2-dropdowns@@32.1.23/styles/bootstrap5.css" rel="stylesheet" />
+<link href="https://unpkg.com/@@syncfusion/ej2-calendars@@32.1.23/styles/bootstrap5.css" rel="stylesheet" />
 
 <link rel="stylesheet" type="text/css" href="https://cdn.datatables.net/1.12.0/css/jquery.dataTables.min.css">
 <link rel="stylesheet" href="https://cdn.datatables.net/buttons/2.2.3/css/buttons.dataTables.min.css" />
@@ -62,8 +62,6 @@
 
 <link href="https://fonts.googleapis.com/css2?family=Poppins:wght@400;600&display=swap" rel="stylesheet">
 
-<link href="https://cdn.syncfusion.com/ej2/32.1.19/ej2-base/styles/bootstrap5.css" rel="stylesheet" />
-
 <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/flatpickr/dist/flatpickr.min.css">
 <script src="https://cdn.jsdelivr.net/npm/flatpickr"></script>
 
@@ -71,16 +69,16 @@
 <link rel="preconnect" href="https://fonts.gstatic.com" crossorigin>
 <link href="https://fonts.googleapis.com/css2?family=Poppins:wght@800&display=swap" rel="stylesheet">
 
+<link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/css/bootstrap.min.css" crossorigin="anonymous" />
+
+<link rel="stylesheet" href="~/lib/kendo/2025.2.520/styles/bootstrap-main.css" asp-append-version="true" />
+
+<link rel="preconnect" href="https://fonts.googleapis.com">
+<link rel="preconnect" href="https://fonts.gstatic.com" crossorigin>
+<link href="https://fonts.googleapis.com/css2?family=Outfit:wght@900&display=swap" rel="stylesheet">
+
 <link rel="stylesheet" href="~/css/frotix.css" asp-append-version="true" />
 
 <link rel="stylesheet" href="~/css/alertassino.css" asp-append-version="true" />
 
 <partial name="_Favicon" />
-
-<link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/css/bootstrap.min.css" crossorigin="anonymous" />
-
-<link rel="stylesheet" href="https://kendo.cdn.telerik.com/themes/12.0.1/bootstrap/bootstrap-main.css" />
-
-<link rel="preconnect" href="https://fonts.googleapis.com">
-<link rel="preconnect" href="https://fonts.gstatic.com" crossorigin>
-<link href="https://fonts.googleapis.com/css2?family=Outfit:wght@900&display=swap" rel="stylesheet">
```

### REMOVER do Janeiro

```html
<link href="https://cdn.syncfusion.com/ej2/32.1.19/bootstrap5.css" rel="stylesheet" />
<link href="https://cdn.syncfusion.com/ej2/32.1.19/ej2-base/styles/bootstrap5.css" rel="stylesheet" />
<link href="https://cdn.syncfusion.com/ej2/32.1.19/ej2-buttons/styles/bootstrap5.css" rel="stylesheet" />
<link href="https://cdn.syncfusion.com/ej2/32.1.19/ej2-inputs/styles/bootstrap5.css" rel="stylesheet" />
<link href="https://cdn.syncfusion.com/ej2/32.1.19/ej2-popups/styles/bootstrap5.css" rel="stylesheet" />
<link href="https://cdn.syncfusion.com/ej2/32.1.19/ej2-dropdowns/styles/bootstrap5.css" rel="stylesheet" />
<link href="https://cdn.syncfusion.com/ej2/32.1.19/ej2-dropdowns/styles/bootstrap5.css" rel="stylesheet" />
<link href="https://cdn.syncfusion.com/ej2/32.1.19/ej2-base/styles/bootstrap5.css" rel="stylesheet" />
<link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/css/bootstrap.min.css" crossorigin="anonymous" />
<link rel="stylesheet" href="https://kendo.cdn.telerik.com/themes/12.0.1/bootstrap/bootstrap-main.css" />
<link rel="preconnect" href="https://fonts.googleapis.com">
<link rel="preconnect" href="https://fonts.gstatic.com" crossorigin>
<link href="https://fonts.googleapis.com/css2?family=Outfit:wght@900&display=swap" rel="stylesheet">
```


### ADICIONAR ao Janeiro

```html
<link href="https://unpkg.com/@@syncfusion/ej2@@32.1.23/bootstrap5.css" rel="stylesheet" />
<link href="https://unpkg.com/@@syncfusion/ej2-base@@32.1.23/styles/bootstrap5.css" rel="stylesheet" />
<link href="https://unpkg.com/@@syncfusion/ej2-buttons@@32.1.23/styles/bootstrap5.css" rel="stylesheet" />
<link href="https://unpkg.com/@@syncfusion/ej2-inputs@@32.1.23/styles/bootstrap5.css" rel="stylesheet" />
<link href="https://unpkg.com/@@syncfusion/ej2-popups@@32.1.23/styles/bootstrap5.css" rel="stylesheet" />
<link href="https://unpkg.com/@@syncfusion/ej2-dropdowns@@32.1.23/styles/bootstrap5.css" rel="stylesheet" />
<link href="https://unpkg.com/@@syncfusion/ej2-calendars@@32.1.23/styles/bootstrap5.css" rel="stylesheet" />
<link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/css/bootstrap.min.css" crossorigin="anonymous" />
<link rel="stylesheet" href="~/lib/kendo/2025.2.520/styles/bootstrap-main.css" asp-append-version="true" />
<link rel="preconnect" href="https://fonts.googleapis.com">
<link rel="preconnect" href="https://fonts.gstatic.com" crossorigin>
<link href="https://fonts.googleapis.com/css2?family=Outfit:wght@900&display=swap" rel="stylesheet">
```
