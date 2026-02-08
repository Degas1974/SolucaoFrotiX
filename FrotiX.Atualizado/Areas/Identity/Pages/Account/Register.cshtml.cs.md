# Areas/Identity/Pages/Account/Register.cshtml.cs

**Mudanca:** GRANDE | **+32** linhas | **-29** linhas

---

```diff
--- JANEIRO: Areas/Identity/Pages/Account/Register.cshtml.cs
+++ ATUAL: Areas/Identity/Pages/Account/Register.cshtml.cs
@@ -1,3 +1,4 @@
+using System;
 using System.ComponentModel.DataAnnotations;
 using System.Text.Encodings.Web;
 using System.Threading.Tasks;
@@ -12,10 +13,10 @@
 using FrotiX.Validations;
 
 namespace FrotiX.Areas.Identity.Pages.Account
-{
+    {
     [AllowAnonymous]
     public class RegisterModel : PageModel
-    {
+        {
         private readonly IEmailSender _emailSender;
         private readonly ILogger<RegisterModel> _logger;
         private readonly SignInManager<IdentityUser> _signInManager;
@@ -23,73 +24,76 @@
 
         public RegisterModel(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, ILogger<RegisterModel> logger,
             IEmailSender emailSender)
-        {
+            {
             _userManager = userManager;
             _signInManager = signInManager;
             _logger = logger;
             _emailSender = emailSender;
-        }
+            }
 
         [BindProperty] public InputModel Input { get; set; }
 
         public string ReturnUrl { get; set; }
 
         public void OnGet(string returnUrl = null)
-        {
+            {
             try
-            {
+                {
+
                 ReturnUrl = returnUrl;
+                }
+            catch (Exception ex)
+                {
+                _logger.LogError(ex, "Erro ao inicializar página de registro");
+                TempData["Erro"] = "Erro ao carregar página de registro. Tente novamente.";
+                }
             }
-            catch (System.Exception error)
-            {
-
-                FrotiX.Helpers.Alerta.TratamentoErroComLinha("Register.cshtml.cs", "OnGet", error);
-            }
-        }
 
         public async Task<IActionResult> OnPostAsync(string returnUrl = null)
-        {
+            {
             try
-            {
+                {
                 returnUrl = returnUrl ?? Url.Content("~/");
+
                 if (ModelState.IsValid)
-                {
+                    {
 
                     var user = new AspNetUsers
-                    {
+                        {
                         UserName = Input.Ponto,
                         Email = Input.Email,
                         NomeCompleto = Input.NomeCompleto,
                         Ponto = Input.Ponto
-                    };
+                        };
 
                     var result = await _userManager.CreateAsync(user, Input.Senha);
+
                     if (result.Succeeded)
-                    {
+                        {
                         _logger.LogInformation("User created a new account with password.");
 
                         await _signInManager.SignInAsync(user, false);
                         return LocalRedirect("/Identity/Account/LoginFrotiX");
+                        }
+
+                    foreach (var error in result.Errors)
+                        {
+                        ModelState.AddModelError(string.Empty, error.Description);
+                        }
                     }
 
-                    foreach (var error in result.Errors)
-                    {
-                        ModelState.AddModelError(string.Empty, error.Description);
-                    }
+                return Page();
                 }
-
+            catch (Exception ex)
+                {
+                _logger.LogError(ex, "Erro ao registrar novo usuário");
+                TempData["Erro"] = "Erro ao processar registro. Verifique os dados e tente novamente.";
                 return Page();
+                }
             }
-            catch (System.Exception error)
-            {
-
-                FrotiX.Helpers.Alerta.TratamentoErroComLinha("Register.cshtml.cs", "OnPostAsync", error);
-                return Page();
-            }
-        }
 
         public class InputModel
-        {
+            {
             [Required]
             [Display(Name = "Ponto")]
             public string Ponto { get; set; }
@@ -115,6 +119,6 @@
             [Compare("Senha", ErrorMessage = "A senha e a confirmação não combinam.")]
             public string ConfirmacaoSenha { get; set; }
 
+            }
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
            {
            catch (System.Exception error)
            {
                FrotiX.Helpers.Alerta.TratamentoErroComLinha("Register.cshtml.cs", "OnGet", error);
            }
        }
        {
            {
                {
                    {
                    };
                    {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
            catch (System.Exception error)
            {
                FrotiX.Helpers.Alerta.TratamentoErroComLinha("Register.cshtml.cs", "OnPostAsync", error);
                return Page();
            }
        }
        {
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
                {
                }
            catch (Exception ex)
                {
                _logger.LogError(ex, "Erro ao inicializar página de registro");
                TempData["Erro"] = "Erro ao carregar página de registro. Tente novamente.";
                }
            {
                {
                    {
                        {
                        };
                        {
                        }
                    foreach (var error in result.Errors)
                        {
                        ModelState.AddModelError(string.Empty, error.Description);
                        }
                return Page();
            catch (Exception ex)
                {
                _logger.LogError(ex, "Erro ao registrar novo usuário");
                TempData["Erro"] = "Erro ao processar registro. Verifique os dados e tente novamente.";
                }
            {
            }
```
