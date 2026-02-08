# Areas/Identity/Pages/Account/RegisterConfirmation.cshtml.cs

**Mudanca:** GRANDE | **+25** linhas | **-18** linhas

---

```diff
--- JANEIRO: Areas/Identity/Pages/Account/RegisterConfirmation.cshtml.cs
+++ ATUAL: Areas/Identity/Pages/Account/RegisterConfirmation.cshtml.cs
@@ -1,4 +1,5 @@
 using Microsoft.AspNetCore.Authorization;
+using System;
 using System.Text;
 using System.Threading.Tasks;
 using Microsoft.AspNetCore.Identity;
@@ -6,20 +7,23 @@
 using Microsoft.AspNetCore.Mvc;
 using Microsoft.AspNetCore.Mvc.RazorPages;
 using Microsoft.AspNetCore.WebUtilities;
+using Microsoft.Extensions.Logging;
 
 namespace FrotiX.Areas.Identity.Pages.Account
-{
+    {
     [AllowAnonymous]
     public class RegisterConfirmationModel : PageModel
-    {
+        {
         private readonly UserManager<IdentityUser> _userManager;
         private readonly IEmailSender _sender;
+        private readonly ILogger<RegisterConfirmationModel> _logger;
 
-        public RegisterConfirmationModel(UserManager<IdentityUser> userManager, IEmailSender sender)
-        {
+        public RegisterConfirmationModel(UserManager<IdentityUser> userManager, IEmailSender sender, ILogger<RegisterConfirmationModel> logger)
+            {
             _userManager = userManager;
             _sender = sender;
-        }
+            _logger = logger;
+            }
 
         public string Email { get; set; }
 
@@ -28,44 +32,49 @@
         public string EmailConfirmationUrl { get; set; }
 
         public async Task<IActionResult> OnGetAsync(string email)
-        {
+            {
             try
-            {
+                {
+
                 if (email == null)
-                {
+                    {
+                    _logger.LogWarning("Tentativa de acessar confirmação de registro sem email");
                     return RedirectToPage("/Index");
-                }
+                    }
 
                 var user = await _userManager.FindByEmailAsync(email);
                 if (user == null)
-                {
+                    {
+                    _logger.LogWarning($"Usuário com email '{email}' não encontrado para confirmação");
                     return NotFound($"Unable to load user with email '{email}'.");
-                }
+                    }
 
                 Email = email;
 
                 DisplayConfirmAccountLink = true;
 
                 if (DisplayConfirmAccountLink)
-                {
+                    {
                     var userId = await _userManager.GetUserIdAsync(user);
+
                     var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                     code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
+
                     EmailConfirmationUrl = Url.Page(
                         "/Account/ConfirmEmail",
                         pageHandler: null,
                         values: new { area = "Identity", userId = userId, code = code },
                         protocol: Request.Scheme);
-                }
+                    }
 
                 return Page();
-            }
-            catch (System.Exception error)
-            {
-
-                FrotiX.Helpers.Alerta.TratamentoErroComLinha("RegisterConfirmation.cshtml.cs", "OnGetAsync", error);
+                }
+            catch (Exception ex)
+                {
+                _logger.LogError(ex, $"Erro ao processar confirmação de registro para email: {email}");
+                TempData["Erro"] = "Erro ao processar confirmação. Contate o suporte.";
                 return RedirectToPage("/Index");
+                }
             }
         }
     }
-}
```

### REMOVER do Janeiro

```csharp
{
    {
        public RegisterConfirmationModel(UserManager<IdentityUser> userManager, IEmailSender sender)
        {
        }
        {
            {
                {
                }
                {
                }
                {
                }
            }
            catch (System.Exception error)
            {
                FrotiX.Helpers.Alerta.TratamentoErroComLinha("RegisterConfirmation.cshtml.cs", "OnGetAsync", error);
}
```


### ADICIONAR ao Janeiro

```csharp
using System;
using Microsoft.Extensions.Logging;
    {
        {
        private readonly ILogger<RegisterConfirmationModel> _logger;
        public RegisterConfirmationModel(UserManager<IdentityUser> userManager, IEmailSender sender, ILogger<RegisterConfirmationModel> logger)
            {
            _logger = logger;
            }
            {
                {
                    {
                    _logger.LogWarning("Tentativa de acessar confirmação de registro sem email");
                    }
                    {
                    _logger.LogWarning($"Usuário com email '{email}' não encontrado para confirmação");
                    }
                    {
                    }
                }
            catch (Exception ex)
                {
                _logger.LogError(ex, $"Erro ao processar confirmação de registro para email: {email}");
                TempData["Erro"] = "Erro ao processar confirmação. Contate o suporte.";
                }
```
