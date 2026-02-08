# Areas/Identity/Pages/Account/LoginFrotiX.cshtml.cs

**Mudanca:** GRANDE | **+53** linhas | **-54** linhas

---

```diff
--- JANEIRO: Areas/Identity/Pages/Account/LoginFrotiX.cshtml.cs
+++ ATUAL: Areas/Identity/Pages/Account/LoginFrotiX.cshtml.cs
@@ -13,25 +13,22 @@
 using Microsoft.AspNetCore.Mvc.ModelBinding;
 using FrotiX.Repository.IRepository;
 using System.Security.Claims;
-using FrotiX.Services;
 
 namespace FrotiX.Areas.Identity.Pages.Account
-{
+    {
     [AllowAnonymous]
     public class LoginFrotiX : PageModel
-    {
+        {
         private readonly SignInManager<IdentityUser> _signInManager;
         private readonly ILogger<LoginModel> _logger;
         private readonly IUnitOfWork _unitOfWork;
-        private readonly ILogService _log;
 
-        public LoginFrotiX(SignInManager<IdentityUser> signInManager, ILogger<LoginModel> logger, IUnitOfWork unitOfWork, ILogService log)
-        {
+        public LoginFrotiX(SignInManager<IdentityUser> signInManager, ILogger<LoginModel> logger, IUnitOfWork unitOfWork)
+            {
             _signInManager = signInManager;
             _logger = logger;
             _unitOfWork = unitOfWork;
-            _log = log;
-        }
+            }
 
         [BindProperty]
         public LoginFrotiXModel Input { get; set; }
@@ -44,26 +41,27 @@
         public string ErrorMessage { get; set; }
 
         public class LoginFrotiXModel
-        {
+            {
             [Required(ErrorMessage = "Insira o seu ponto! (p_xxxx)")]
             public string Ponto { get; set; }
 
-            [Required(ErrorMessage = "A senha � obrigat�ria!")]
+            [Required(ErrorMessage = "A senha é obrigatória!")]
             [DataType(DataType.Password)]
             public string Password { get; set; }
 
             [Display(Name = "Remember me?")]
             public bool RememberMe { get; set; }
-        }
+            }
 
         public async Task OnGetAsync(string returnUrl = null)
-        {
+            {
             try
             {
+
                 if (!string.IsNullOrEmpty(ErrorMessage))
-                {
+                    {
                     ModelState.AddModelError(string.Empty, ErrorMessage);
-                }
+                    }
 
                 returnUrl = returnUrl ?? Url.Content("~/");
 
@@ -73,91 +71,98 @@
 
                 ReturnUrl = returnUrl;
             }
-            catch (Exception error)
+            catch (Exception ex)
+            {
+                _logger.LogError(ex, "Erro ao processar GET de LoginFrotiX");
+
+            }
+            }
+
+        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
+            {
+            try
             {
 
-                _log.Error("Erro ao carregar tela de LoginFrotiX", error);
-                FrotiX.Helpers.Alerta.TratamentoErroComLinha("LoginFrotiX.cshtml.cs", "OnGetAsync", error);
-            }
-        }
-
-        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
-        {
-            try
-            {
                 returnUrl = returnUrl ?? Url.Content("~/");
 
                 if (ModelState.IsValid)
-                {
+                    {
 
                     var result = await _signInManager.PasswordSignInAsync(Input.Ponto, Input.Password, Input.RememberMe, lockoutOnFailure: true);
 
                     if (result.Succeeded)
-                    {
-                        _log.Info($"Usu�rio logged in via Ponto: {Input.Ponto}");
-
+                        {
+                        _logger.LogInformation("User logged in.");
                         return new JsonResult(
                             new
-                            {
+                                {
                                 isSuccess = true,
                                 returnUrl = "/intel/analyticsdashboard"
-                            });
-                    }
+                                });
+
+                        }
+
                     if (result.RequiresTwoFactor)
-                    {
+                        {
                         return new JsonResult(
                             new
-                            {
+                                {
                                 isSuccess = true,
                                 returnUrl = "./LoginWith2fa"
-                            });
-                    }
+                                });
+
+                        }
+
                     if (result.IsLockedOut)
-                    {
-                        _log.Warning($"Conta bloqueada para o ponto: {Input.Ponto}");
+                        {
+                        _logger.LogWarning("User account locked out.");
                         return new JsonResult(
                             new
-                            {
+                                {
                                 isSuccess = true,
                                 returnUrl = "./Lockout"
-                            });
-                    }
+                                });
+
+                        }
                     else
-                    {
-                        _log.Warning($"Tentativa de login falhou para o ponto: {Input.Ponto}");
-                        ModelState.AddModelError(string.Empty, "Login Inv�lido.");
+                        {
+
+                        ModelState.AddModelError(string.Empty, "Login Inválido.");
                         return new JsonResult(
                             new
-                            {
+                                {
                                 isSuccess = false
-                            });
+                                });
+                        }
                     }
-                }
 
                 var errorMessage = "";
-
                 foreach (var modelState in ViewData.ModelState.Values)
-                {
+                    {
                     foreach (ModelError error in modelState.Errors)
-                    {
+                        {
                         errorMessage = error.ErrorMessage;
+                        }
                     }
-                }
 
                 return new JsonResult(
                             new
-                            {
+                                {
                                 isSuccess = false,
                                 message = errorMessage
-                            });
+                                });
+
             }
-            catch (Exception error)
+            catch (Exception ex)
             {
-
-                _log.Error($", errorErro ao processar login via Ponto: {Input?.Ponto}");
-                FrotiX.Helpers.Alerta.TratamentoErroComLinha("LoginFrotiX.cshtml.cs", "OnPostAsync", error);
-                return new JsonResult(new { isSuccess = false, message = "Erro interno ao realizar login." });
+                _logger.LogError(ex, "Erro ao processar login FrotiX para Ponto: {Ponto}", Input?.Ponto);
+                return new JsonResult(
+                    new
+                        {
+                        isSuccess = false,
+                        message = $"Erro ao processar login: {ex.Message}"
+                        });
+            }
             }
         }
     }
-}
```
