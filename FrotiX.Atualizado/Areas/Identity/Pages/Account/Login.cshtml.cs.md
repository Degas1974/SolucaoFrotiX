# Areas/Identity/Pages/Account/Login.cshtml.cs

**Mudanca:** GRANDE | **+56** linhas | **-40** linhas

---

```diff
--- JANEIRO: Areas/Identity/Pages/Account/Login.cshtml.cs
+++ ATUAL: Areas/Identity/Pages/Account/Login.cshtml.cs
@@ -1,3 +1,4 @@
+using System;
 using System.Collections.Generic;
 using System.ComponentModel.DataAnnotations;
 using System.Linq;
@@ -9,22 +10,20 @@
 using Microsoft.AspNetCore.Mvc.RazorPages;
 using Microsoft.Extensions.Logging;
 using FrotiX.Models;
-using FrotiX.Helpers;
 
 namespace FrotiX.Areas.Identity.Pages.Account
-{
+    {
     [AllowAnonymous]
     public class LoginModel : PageModel
-    {
-
+        {
         private readonly SignInManager<IdentityUser> _signInManager;
         private readonly ILogger<LoginModel> _logger;
 
         public LoginModel(SignInManager<IdentityUser> signInManager, ILogger<LoginModel> logger)
-        {
+            {
             _signInManager = signInManager;
             _logger = logger;
-        }
+            }
 
         [BindProperty]
         public InputModel Input { get; set; }
@@ -37,7 +36,7 @@
         public string ErrorMessage { get; set; }
 
         public class InputModel
-        {
+            {
             [Required]
             public string Ponto { get; set; }
 
@@ -47,55 +46,77 @@
 
             [Display(Name = "Remember me?")]
             public bool RememberMe { get; set; }
-        }
+            }
 
         public async Task OnGetAsync(string returnUrl = null)
-        {
-            if (!string.IsNullOrEmpty(ErrorMessage))
             {
-                ModelState.AddModelError(string.Empty, ErrorMessage);
+            try
+            {
+
+                if (!string.IsNullOrEmpty(ErrorMessage))
+                    {
+                    ModelState.AddModelError(string.Empty, ErrorMessage);
+                    }
+
+                returnUrl = returnUrl ?? Url.Content("~/");
+
+                await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
+
+                ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
+
+                ReturnUrl = returnUrl;
+            }
+            catch (Exception ex)
+            {
+                _logger.LogError(ex, "Erro ao processar GET de Login");
+
+            }
             }
 
-            returnUrl = returnUrl ?? Url.Content("~/");
-
-            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
-
-            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
-
-            ReturnUrl = returnUrl;
-        }
-
         public async Task<IActionResult> OnPostAsync(string returnUrl = null)
-        {
-            returnUrl = returnUrl ?? Url.Content("~/");
-
-            if (ModelState.IsValid)
+            {
+            try
             {
 
-                var result = await _signInManager.PasswordSignInAsync(Input.Ponto, Input.Password, Input.RememberMe, lockoutOnFailure: true);
-                if (result.Succeeded)
-                {
-                    _logger.LogInformation("User logged in.");
+                returnUrl = returnUrl ?? Url.Content("~/");
 
-                    return LocalRedirect("/intel/analyticsdashboard");
-                }
-                if (result.RequiresTwoFactor)
-                {
-                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
-                }
-                if (result.IsLockedOut)
-                {
-                    _logger.LogWarning("User account locked out.");
-                    return RedirectToPage("./Lockout");
-                }
-                else
-                {
-                    ModelState.AddModelError(string.Empty, "Login Inválido.");
-                    return Page();
-                }
+                if (ModelState.IsValid)
+                    {
+
+                    var result = await _signInManager.PasswordSignInAsync(Input.Ponto, Input.Password, Input.RememberMe, lockoutOnFailure: true);
+
+                    if (result.Succeeded)
+                        {
+                        _logger.LogInformation("User logged in.");
+                        return LocalRedirect("/intel/analyticsdashboard");
+                        }
+
+                    if (result.RequiresTwoFactor)
+                        {
+                        return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
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
+                        ModelState.AddModelError(string.Empty, "Login Inválido.");
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
+                _logger.LogError(ex, "Erro ao processar login para Ponto: {Ponto}", Input?.Ponto);
+                TempData["Erro"] = $"Erro ao processar login: {ex.Message}";
+                return Page();
+            }
+            }
         }
     }
-}
```
