# Areas/Identity/Pages/Account/Logout.cshtml.cs

**Mudanca:** GRANDE | **+19** linhas | **-17** linhas

---

```diff
--- JANEIRO: Areas/Identity/Pages/Account/Logout.cshtml.cs
+++ ATUAL: Areas/Identity/Pages/Account/Logout.cshtml.cs
@@ -1,3 +1,4 @@
+using System;
 using System.Threading.Tasks;
 using Microsoft.AspNetCore.Authorization;
 using Microsoft.AspNetCore.Identity;
@@ -7,60 +8,57 @@
 using FrotiX.Models;
 
 namespace FrotiX.Areas.Identity.Pages.Account
-{
+    {
     [AllowAnonymous]
     public class LogoutModel : PageModel
-    {
+        {
         private readonly SignInManager<IdentityUser> _signInManager;
         private readonly ILogger<LogoutModel> _logger;
 
         public LogoutModel(SignInManager<IdentityUser> signInManager, ILogger<LogoutModel> logger)
-        {
+            {
             _signInManager = signInManager;
             _logger = logger;
-        }
+            }
 
         public async Task OnGet()
-        {
+            {
             try
             {
 
                 await _signInManager.SignOutAsync();
-
                 _logger.LogInformation("User logged out.");
             }
-            catch (System.Exception error)
+            catch (Exception ex)
             {
+                _logger.LogError(ex, "Erro ao processar GET de Logout");
 
-                FrotiX.Helpers.Alerta.TratamentoErroComLinha("Logout.cshtml.cs", "OnGet", error);
             }
-        }
+            }
 
         public async Task<IActionResult> OnPost(string returnUrl = null)
-        {
+            {
             try
             {
 
                 await _signInManager.SignOutAsync();
-
                 _logger.LogInformation("User logged out.");
 
                 if (returnUrl != null)
-                {
+                    {
                     return LocalRedirect(returnUrl);
-                }
+                    }
                 else
-                {
-
+                    {
                     return Page();
-                }
+                    }
             }
-            catch (System.Exception error)
+            catch (Exception ex)
             {
-
-                FrotiX.Helpers.Alerta.TratamentoErroComLinha("Logout.cshtml.cs", "OnPost", error);
-                return RedirectToPage();
+                _logger.LogError(ex, "Erro ao processar POST de Logout");
+                TempData["Erro"] = $"Erro ao processar logout: {ex.Message}";
+                return Page();
+            }
             }
         }
     }
-}
```

### REMOVER do Janeiro

```csharp
{
    {
        {
        }
        {
            catch (System.Exception error)
                FrotiX.Helpers.Alerta.TratamentoErroComLinha("Logout.cshtml.cs", "OnGet", error);
        }
        {
                {
                }
                {
                }
            catch (System.Exception error)
                FrotiX.Helpers.Alerta.TratamentoErroComLinha("Logout.cshtml.cs", "OnPost", error);
                return RedirectToPage();
}
```


### ADICIONAR ao Janeiro

```csharp
using System;
    {
        {
            {
            }
            {
            catch (Exception ex)
                _logger.LogError(ex, "Erro ao processar GET de Logout");
            }
            {
                    {
                    }
                    {
                    }
            catch (Exception ex)
                _logger.LogError(ex, "Erro ao processar POST de Logout");
                TempData["Erro"] = $"Erro ao processar logout: {ex.Message}";
                return Page();
            }
```
