# Areas/Identity/Pages/Account/Lockout.cshtml.cs

**Mudanca:** GRANDE | **+53** linhas | **-37** linhas

---

```diff
--- JANEIRO: Areas/Identity/Pages/Account/Lockout.cshtml.cs
+++ ATUAL: Areas/Identity/Pages/Account/Lockout.cshtml.cs
@@ -1,3 +1,4 @@
+using System;
 using System.ComponentModel.DataAnnotations;
 using System.Threading.Tasks;
 using Microsoft.AspNetCore.Authorization;
@@ -5,14 +6,12 @@
 using Microsoft.AspNetCore.Mvc;
 using Microsoft.AspNetCore.Mvc.RazorPages;
 using Microsoft.Extensions.Logging;
-using FrotiX.Helpers;
 
 namespace FrotiX.Areas.Identity.Pages.Account
-{
+    {
     [AllowAnonymous]
     public class LockoutModel : PageModel
-    {
-
+        {
         private readonly ILogger<LogoutModel> _logger;
         private readonly SignInManager<IdentityUser> _signInManager;
 
@@ -20,60 +19,79 @@
         public InputModel Input { get; set; }
 
         public class InputModel
-        {
+            {
             [Required]
             [DataType(DataType.Password)]
             public string Password { get; set; }
-        }
+            }
 
         public LockoutModel(SignInManager<IdentityUser> signInManager, ILogger<LogoutModel> logger)
-        {
+            {
             _signInManager = signInManager;
             _logger = logger;
-        }
+            }
 
         public async Task OnGetAsync()
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
+                _logger.LogError(ex, "Erro ao processar GET de Lockout");
 
-            _logger.LogInformation("User logged out.");
-        }
+            }
+            }
 
         public async Task<IActionResult> OnPostAsync(string returnUrl = null)
-        {
-            returnUrl = returnUrl ?? Url.Content("~/");
-
-            if (ModelState.IsValid)
+            {
+            try
             {
 
-                var userName = ViewData["Email"]?.ToString();
+                returnUrl = returnUrl ?? Url.Content("~/");
 
-                var result = await _signInManager.PasswordSignInAsync(userName, Input.Password, true, lockoutOnFailure: true);
+                if (ModelState.IsValid)
+                    {
 
-                if (result.Succeeded)
-                {
-                    _logger.LogInformation("User logged in.");
-                    return LocalRedirect(returnUrl);
-                }
-                if (result.RequiresTwoFactor)
-                {
-                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = true });
-                }
-                if (result.IsLockedOut)
-                {
+                    var userName = ViewData["Email"]?.ToString();
 
-                    _logger.LogWarning("User account locked out.");
-                    return RedirectToPage("./Lockout");
-                }
-                else
-                {
-                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
-                    return Page();
-                }
+                    var result = await _signInManager.PasswordSignInAsync(userName, Input.Password, true, lockoutOnFailure: true);
+
+                    if (result.Succeeded)
+                        {
+                        _logger.LogInformation("User logged in.");
+                        return LocalRedirect(returnUrl);
+                        }
+
+                    if (result.RequiresTwoFactor)
+                        {
+                        return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = true });
+                        }
+
+                    if (result.IsLockedOut)
+                        {
+                        _logger.LogWarning("User account locked out.");
+                        return RedirectToPage("./Lockout");
+                        }
+                    else
+                        {
+
+                        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
+                        return Page();
+                        }
+                    }
+
+                return Page();
             }
-
-            return Page();
+            catch (Exception ex)
+            {
+                _logger.LogError(ex, "Erro ao processar desbloqueio para usuário: {UserName}", ViewData["Email"]);
+                TempData["Erro"] = $"Erro ao processar desbloqueio: {ex.Message}";
+                return Page();
+            }
+            }
         }
     }
-}
```
