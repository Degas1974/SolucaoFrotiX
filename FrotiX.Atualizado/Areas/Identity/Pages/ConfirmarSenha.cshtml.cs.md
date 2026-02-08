# Areas/Identity/Pages/ConfirmarSenha.cshtml.cs

**Mudanca:** GRANDE | **+23** linhas | **-27** linhas

---

```diff
--- JANEIRO: Areas/Identity/Pages/ConfirmarSenha.cshtml.cs
+++ ATUAL: Areas/Identity/Pages/ConfirmarSenha.cshtml.cs
@@ -9,28 +9,22 @@
 using Microsoft.AspNetCore.Mvc.RazorPages;
 using Microsoft.Extensions.Logging;
 using FrotiX.Models;
-using FrotiX.Helpers;
 using System;
 using Microsoft.AspNetCore.Mvc.ModelBinding;
 
-using FrotiX.Services;
-
 namespace FrotiX.Areas.Identity.Pages
-{
+    {
     [AllowAnonymous]
     public class ConfirmarSenha : PageModel
-    {
-
+        {
         private readonly SignInManager<IdentityUser> _signInManager;
         private readonly ILogger<ConfirmarSenhaModel> _logger;
-        private readonly ILogService _log;
 
-        public ConfirmarSenha(SignInManager<IdentityUser> signInManager, ILogger<ConfirmarSenhaModel> logger, ILogService log)
-        {
+        public ConfirmarSenha(SignInManager<IdentityUser> signInManager, ILogger<ConfirmarSenhaModel> logger)
+            {
             _signInManager = signInManager;
             _logger = logger;
-            _log = log;
-        }
+            }
 
         [BindProperty]
         public ConfirmarSenhaModel Input { get; set; }
@@ -43,7 +37,8 @@
         public string ErrorMessage { get; set; }
 
         public class ConfirmarSenhaModel
-        {
+            {
+
             [Required(ErrorMessage = "A senha é obrigatória!")]
             [DataType(DataType.Password)]
             public string Password { get; set; }
@@ -51,17 +46,17 @@
             [Compare(nameof(Password), ErrorMessage = "A confirmação da senha não combina com a senha!")]
             [DataType(DataType.Password)]
             public string ConfirmacaoPassword { get; set; }
-        }
+            }
 
         public async Task OnGetAsync(string returnUrl = null)
-        {
+            {
             try
             {
 
                 if (!string.IsNullOrEmpty(ErrorMessage))
-                {
+                    {
                     ModelState.AddModelError(string.Empty, ErrorMessage);
-                }
+                    }
 
                 returnUrl = returnUrl ?? Url.Content("~/");
 
@@ -69,33 +64,32 @@
 
                 ReturnUrl = returnUrl;
             }
-            catch (Exception error)
+            catch (Exception ex)
             {
-
-                _log.Error("Erro ao carregar tela de ConfirmarSenha", error);
-                Alerta.TratamentoErroComLinha("ConfirmarSenha.cshtml.cs", "OnGetAsync", error);
+                _logger.LogError(ex, "Erro ao preparar página de confirmação de senha");
+                ErrorMessage = $"Erro ao carregar página: {ex.Message}";
             }
-        }
+            }
 
         public async Task<IActionResult> OnPostAsync(string returnUrl = null)
-        {
+            {
             try
             {
 
                 if (ModelState.IsValid)
-                {
-                    _log.Info("Senha confirmada com sucesso.");
-                }
+                    {
+                    Console.WriteLine("Validando o Modelo");
 
-                return RedirectToPage("Account/LoginFrotiX");
+                    }
+
+                return RedirectToPage("Account/LoginFrotiX.html");
             }
-            catch (Exception error)
+            catch (Exception ex)
             {
-
-                _log.Error("Erro ao processar confirmação de senha", error);
-                Alerta.TratamentoErroComLinha("ConfirmarSenha.cshtml.cs", "OnPostAsync", error);
+                _logger.LogError(ex, "Erro ao processar confirmação de senha");
+                ErrorMessage = $"Erro ao confirmar senha: {ex.Message}";
                 return Page();
+            }
             }
         }
     }
-}
```

### REMOVER do Janeiro

```csharp
using FrotiX.Helpers;
using FrotiX.Services;
{
    {
        private readonly ILogService _log;
        public ConfirmarSenha(SignInManager<IdentityUser> signInManager, ILogger<ConfirmarSenhaModel> logger, ILogService log)
        {
            _log = log;
        }
        {
        }
        {
                {
                }
            catch (Exception error)
                _log.Error("Erro ao carregar tela de ConfirmarSenha", error);
                Alerta.TratamentoErroComLinha("ConfirmarSenha.cshtml.cs", "OnGetAsync", error);
        }
        {
                {
                    _log.Info("Senha confirmada com sucesso.");
                }
                return RedirectToPage("Account/LoginFrotiX");
            catch (Exception error)
                _log.Error("Erro ao processar confirmação de senha", error);
                Alerta.TratamentoErroComLinha("ConfirmarSenha.cshtml.cs", "OnPostAsync", error);
}
```


### ADICIONAR ao Janeiro

```csharp
    {
        {
        public ConfirmarSenha(SignInManager<IdentityUser> signInManager, ILogger<ConfirmarSenhaModel> logger)
            {
            }
            {
            }
            {
                    {
                    }
            catch (Exception ex)
                _logger.LogError(ex, "Erro ao preparar página de confirmação de senha");
                ErrorMessage = $"Erro ao carregar página: {ex.Message}";
            }
            {
                    {
                    Console.WriteLine("Validando o Modelo");
                    }
                return RedirectToPage("Account/LoginFrotiX.html");
            catch (Exception ex)
                _logger.LogError(ex, "Erro ao processar confirmação de senha");
                ErrorMessage = $"Erro ao confirmar senha: {ex.Message}";
            }
```
