# Areas/Identity/Pages/Account/ForgotPassword.cshtml.cs

**Mudanca:** GRANDE | **+46** linhas | **-30** linhas

---

```diff
--- JANEIRO: Areas/Identity/Pages/Account/ForgotPassword.cshtml.cs
+++ ATUAL: Areas/Identity/Pages/Account/ForgotPassword.cshtml.cs
@@ -1,3 +1,4 @@
+using System;
 using System.ComponentModel.DataAnnotations;
 using System.Text.Encodings.Web;
 using System.Threading.Tasks;
@@ -7,73 +8,90 @@
 using Microsoft.AspNetCore.Mvc;
 using Microsoft.AspNetCore.Mvc.RazorPages;
 using Microsoft.Extensions.Logging;
-using FrotiX.Helpers;
 
 namespace FrotiX.Areas.Identity.Pages.Account
-{
+    {
     [AllowAnonymous]
     public class ForgotPasswordModel : PageModel
-    {
-
+        {
         private readonly SignInManager<IdentityUser> _signInManager;
         private readonly ILogger<LogoutModel> _logger;
         private readonly UserManager<IdentityUser> _userManager;
         private readonly IEmailSender _emailSender;
 
         public ForgotPasswordModel(SignInManager<IdentityUser> signInManager, ILogger<LogoutModel> logger, UserManager<IdentityUser> userManager, IEmailSender emailSender)
-        {
+            {
             _signInManager = signInManager;
             _logger = logger;
             _userManager = userManager;
             _emailSender = emailSender;
-        }
+            }
 
         [BindProperty]
         public InputModel Input { get; set; }
 
         public class InputModel
-        {
+            {
             [Required]
             [EmailAddress]
             public string Email { get; set; }
-        }
+            }
 
         public async Task OnGet()
-        {
+            {
+            try
+            {
 
-            await _signInManager.SignOutAsync();
+                await _signInManager.SignOutAsync();
+                _logger.LogInformation("User logged out.");
+            }
+            catch (Exception ex)
+            {
+                _logger.LogError(ex, "Erro ao processar GET de ForgotPassword");
 
-            _logger.LogInformation("User logged out.");
-        }
+            }
+            }
 
         public async Task<IActionResult> OnPostAsync()
-        {
-            if (ModelState.IsValid)
+            {
+            try
             {
 
-                var user = await _userManager.FindByEmailAsync(Input.Email);
-                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
-                {
+                if (ModelState.IsValid)
+                    {
+
+                    var user = await _userManager.FindByEmailAsync(Input.Email);
+
+                    if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
+                        {
+
+                        return RedirectToPage("./ForgotPasswordConfirmation");
+                        }
+
+                    var code = await _userManager.GeneratePasswordResetTokenAsync(user);
+
+                    var callbackUrl = Url.Page(
+                        "/Account/ResetPassword",
+                        pageHandler: null,
+                        values: new { code },
+                        protocol: Request.Scheme);
+
+                    await _emailSender.SendEmailAsync(
+                        Input.Email,
+                        "Reset Password",
+                        $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
 
                     return RedirectToPage("./ForgotPasswordConfirmation");
-                }
+                    }
 
-                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
-                var callbackUrl = Url.Page(
-                    "/Account/ResetPassword",
-                    pageHandler: null,
-                    values: new { code },
-                    protocol: Request.Scheme);
-
-                await _emailSender.SendEmailAsync(
-                    Input.Email,
-                    "Reset Password",
-                    $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
-
-                return RedirectToPage("./ForgotPasswordConfirmation");
+                return Page();
             }
-
-            return Page();
+            catch (Exception ex)
+            {
+                _logger.LogError(ex, "Erro ao processar recuperação de senha para email: {Email}", Input?.Email);
+                TempData["Erro"] = $"Erro ao processar solicitação: {ex.Message}";
+                return Page();
+            }
+            }
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
        }
        {
        }
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
        }
        {
            if (ModelState.IsValid)
                var user = await _userManager.FindByEmailAsync(Input.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                }
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                var callbackUrl = Url.Page(
                    "/Account/ResetPassword",
                    pageHandler: null,
                    values: new { code },
                    protocol: Request.Scheme);
                await _emailSender.SendEmailAsync(
                    Input.Email,
                    "Reset Password",
                    $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
                return RedirectToPage("./ForgotPasswordConfirmation");
            return Page();
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
            }
            {
            try
            {
                await _signInManager.SignOutAsync();
                _logger.LogInformation("User logged out.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao processar GET de ForgotPassword");
            }
            }
            {
            try
                if (ModelState.IsValid)
                    {
                    var user = await _userManager.FindByEmailAsync(Input.Email);
                    if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                        {
                        return RedirectToPage("./ForgotPasswordConfirmation");
                        }
                    var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var callbackUrl = Url.Page(
                        "/Account/ResetPassword",
                        pageHandler: null,
                        values: new { code },
                        protocol: Request.Scheme);
                    await _emailSender.SendEmailAsync(
                        Input.Email,
                        "Reset Password",
                        $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
                    }
                return Page();
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao processar recuperação de senha para email: {Email}", Input?.Email);
                TempData["Erro"] = $"Erro ao processar solicitação: {ex.Message}";
                return Page();
            }
            }
```
