# Pages/Usuarios/InsereRecursosUsuarios.cshtml

**Mudanca:** PEQUENA | **+1** linhas | **-1** linhas

---

```diff
--- JANEIRO: Pages/Usuarios/InsereRecursosUsuarios.cshtml
+++ ATUAL: Pages/Usuarios/InsereRecursosUsuarios.cshtml
@@ -1,6 +1,7 @@
 @page
+
 @model FrotiX.Models.Abastecimento
-@addTagHelper*, Microsoft.AspNetCore.Mvc.TagHelpers
+@addTagHelper *, Microsoft.AspNetCore.Mvc.TagHelpers
 
 @{
     ViewData["Title"] = "Importa Abastecimentos";
```

### REMOVER do Janeiro

```html
@addTagHelper*, Microsoft.AspNetCore.Mvc.TagHelpers
```


### ADICIONAR ao Janeiro

```html
@addTagHelper *, Microsoft.AspNetCore.Mvc.TagHelpers
```
