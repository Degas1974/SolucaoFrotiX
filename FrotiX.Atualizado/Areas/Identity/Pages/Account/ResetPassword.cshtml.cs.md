# Areas/Identity/Pages/Account/ResetPassword.cshtml.cs

**Mudanca:** GRANDE | **+46** linhas | **-40** linhas

---

```diff
--- JANEIRO: Areas/Identity/Pages/Account/ResetPassword.cshtml.cs
+++ ATUAL: Areas/Identity/Pages/Account/ResetPassword.cshtml.cs
@@ -1,27 +1,31 @@
+using System;
 using System.ComponentModel.DataAnnotations;
 using System.Threading.Tasks;
 using Microsoft.AspNetCore.Authorization;
 using Microsoft.AspNetCore.Identity;
 using Microsoft.AspNetCore.Mvc;
 using Microsoft.AspNetCore.Mvc.RazorPages;
+using Microsoft.Extensions.Logging;
 
 namespace FrotiX.Areas.Identity.Pages.Account
-{
+    {
     [AllowAnonymous]
     public class ResetPasswordModel : PageModel
-    {
+        {
         private readonly UserManager<IdentityUser> _userManager;
+        private readonly ILogger<ResetPasswordModel> _logger;
 
-        public ResetPasswordModel(UserManager<IdentityUser> userManager)
-        {
+        public ResetPasswordModel(UserManager<IdentityUser> userManager, ILogger<ResetPasswordModel> logger)
+            {
             _userManager = userManager;
-        }
+            _logger = logger;
+            }
 
         [BindProperty]
         public InputModel Input { get; set; }
 
         public class InputModel
-        {
+            {
             [Required]
             [EmailAddress]
             public string Email { get; set; }
@@ -37,67 +41,73 @@
             public string ConfirmPassword { get; set; }
 
             public string Code { get; set; }
-        }
+            }
 
         public IActionResult OnGet(string code = null)
-        {
+            {
             try
-            {
+                {
+
                 if (code == null)
+                    {
+                    _logger.LogWarning("Tentativa de acessar reset de senha sem código");
+                    return BadRequest("A code must be supplied for password reset.");
+                    }
+
+                Input = new InputModel
+                    {
+                    Code = code
+                    };
+                return Page();
+                }
+            catch (Exception ex)
                 {
-                    return BadRequest("A code must be supplied for password reset.");
-                }
-                else
-                {
-                    Input = new InputModel
-                    {
-                        Code = code
-                    };
-                    return Page();
+                _logger.LogError(ex, "Erro ao carregar página de reset de senha");
+                TempData["Erro"] = "Erro ao carregar formulário. Tente novamente.";
+                return BadRequest("Erro ao processar solicitação.");
                 }
             }
-            catch (System.Exception error)
-            {
-
-                FrotiX.Helpers.Alerta.TratamentoErroComLinha("ResetPassword.cshtml.cs", "OnGet", error);
-                return BadRequest("Erro ao processar solicitação.");
-            }
-        }
 
         public async Task<IActionResult> OnPostAsync()
-        {
+            {
             try
-            {
+                {
+
                 if (!ModelState.IsValid)
-                {
+                    {
                     return Page();
-                }
+                    }
 
                 var user = await _userManager.FindByEmailAsync(Input.Email);
+
                 if (user == null)
-                {
+                    {
 
+                    _logger.LogWarning($"Tentativa de reset de senha para email inexistente: {Input.Email}");
                     return RedirectToPage("./ResetPasswordConfirmation");
-                }
+                    }
 
                 var result = await _userManager.ResetPasswordAsync(user, Input.Code, Input.Password);
+
                 if (result.Succeeded)
-                {
+                    {
+                    _logger.LogInformation($"Senha redefinida com sucesso para usuário: {Input.Email}");
                     return RedirectToPage("./ResetPasswordConfirmation");
-                }
+                    }
 
                 foreach (var error in result.Errors)
+                    {
+                    ModelState.AddModelError(string.Empty, error.Description);
+                    }
+
+                return Page();
+                }
+            catch (Exception ex)
                 {
-                    ModelState.AddModelError(string.Empty, error.Description);
+                _logger.LogError(ex, $"Erro ao resetar senha para email: {Input.Email}");
+                TempData["Erro"] = "Erro ao redefinir senha. Tente novamente ou solicite novo código.";
+                return Page();
                 }
-                return Page();
-            }
-            catch (System.Exception error)
-            {
-
-                FrotiX.Helpers.Alerta.TratamentoErroComLinha("ResetPassword.cshtml.cs", "OnPostAsync", error);
-                return Page();
             }
         }
     }
-}
```
