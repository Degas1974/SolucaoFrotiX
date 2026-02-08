# Areas/Identity/Pages/Account/ResetPasswordConfirmation.cshtml.cs

**Mudanca:** MEDIA | **+18** linhas | **-10** linhas

---

```diff
--- JANEIRO: Areas/Identity/Pages/Account/ResetPasswordConfirmation.cshtml.cs
+++ ATUAL: Areas/Identity/Pages/Account/ResetPasswordConfirmation.cshtml.cs
@@ -1,24 +1,31 @@
+using System;
 using Microsoft.AspNetCore.Authorization;
 using Microsoft.AspNetCore.Mvc.RazorPages;
+using Microsoft.Extensions.Logging;
 
 namespace FrotiX.Areas.Identity.Pages.Account
-{
+    {
     [AllowAnonymous]
+    public class ResetPasswordConfirmationModel : PageModel
+        {
+        private readonly ILogger<ResetPasswordConfirmationModel> _logger;
 
-    public class ResetPasswordConfirmationModel : PageModel
-    {
+        public ResetPasswordConfirmationModel(ILogger<ResetPasswordConfirmationModel> logger)
+            {
+            _logger = logger;
+            }
 
         public void OnGet()
-        {
+            {
             try
-            {
+                {
 
-            }
-            catch (System.Exception error)
-            {
-
-                FrotiX.Helpers.Alerta.TratamentoErroComLinha("ResetPasswordConfirmation.cshtml.cs", "OnGet", error);
+                _logger.LogInformation("Página de confirmação de reset de senha acessada");
+                }
+            catch (Exception ex)
+                {
+                _logger.LogError(ex, "Erro ao carregar confirmação de reset");
+                }
             }
         }
     }
-}
```

### REMOVER do Janeiro

```csharp
{
    public class ResetPasswordConfirmationModel : PageModel
    {
        {
            {
            }
            catch (System.Exception error)
            {
                FrotiX.Helpers.Alerta.TratamentoErroComLinha("ResetPasswordConfirmation.cshtml.cs", "OnGet", error);
}
```


### ADICIONAR ao Janeiro

```csharp
using System;
using Microsoft.Extensions.Logging;
    {
    public class ResetPasswordConfirmationModel : PageModel
        {
        private readonly ILogger<ResetPasswordConfirmationModel> _logger;
        public ResetPasswordConfirmationModel(ILogger<ResetPasswordConfirmationModel> logger)
            {
            _logger = logger;
            }
            {
                {
                _logger.LogInformation("Página de confirmação de reset de senha acessada");
                }
            catch (Exception ex)
                {
                _logger.LogError(ex, "Erro ao carregar confirmação de reset");
                }
```
