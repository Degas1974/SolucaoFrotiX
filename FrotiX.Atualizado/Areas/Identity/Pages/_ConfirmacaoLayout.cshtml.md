# Areas/Identity/Pages/_ConfirmacaoLayout.cshtml

**Mudanca:** PEQUENA | **+1** linhas | **-1** linhas

---

```diff
--- JANEIRO: Areas/Identity/Pages/_ConfirmacaoLayout.cshtml
+++ ATUAL: Areas/Identity/Pages/_ConfirmacaoLayout.cshtml
@@ -20,7 +20,8 @@
     <link rel="stylesheet" href="~/Neon/css/neon-theme.css">
     <link rel="stylesheet" href="~/Neon/css/neon-forms.css">
     <link rel="stylesheet" href="~/Neon/css/custom.css">
-    <link href="~/lib/fontawesome-pro/css/all.css" rel="stylesheet" />
+
+    <script src="https://kit.fontawesome.com/afeb78ad1f.js" crossorigin="anonymous"></script>
 
     <script src="~/Neon/js/jquery-1.11.3.min.js"></script>
 
```

### REMOVER do Janeiro

```html
    <link href="~/lib/fontawesome-pro/css/all.css" rel="stylesheet" />
```


### ADICIONAR ao Janeiro

```html
    <script src="https://kit.fontawesome.com/afeb78ad1f.js" crossorigin="anonymous"></script>
```
