# Areas/Identity/Pages/Account/ForgotPasswordConfirmation.cshtml.cs

**Mudanca:** MEDIA | **+2** linhas | **-10** linhas

---

```diff
--- JANEIRO: Areas/Identity/Pages/Account/ForgotPasswordConfirmation.cshtml.cs
+++ ATUAL: Areas/Identity/Pages/Account/ForgotPasswordConfirmation.cshtml.cs
@@ -1,24 +1,15 @@
 using Microsoft.AspNetCore.Authorization;
 using Microsoft.AspNetCore.Mvc.RazorPages;
-using FrotiX.Helpers;
 
 namespace FrotiX.Areas.Identity.Pages.Account
-{
+    {
     [AllowAnonymous]
     public class ForgotPasswordConfirmation : PageModel
-    {
+        {
 
         public void OnGet()
-        {
-            try
             {
 
             }
-            catch (System.Exception error)
-            {
-
-                Alerta.TratamentoErroComLinha("ForgotPasswordConfirmation.cshtml.cs", "OnGet", error);
-            }
         }
     }
-}
```

### REMOVER do Janeiro

```csharp
using FrotiX.Helpers;
{
    {
        {
            try
            catch (System.Exception error)
            {
                Alerta.TratamentoErroComLinha("ForgotPasswordConfirmation.cshtml.cs", "OnGet", error);
            }
}
```


### ADICIONAR ao Janeiro

```csharp
    {
        {
```
