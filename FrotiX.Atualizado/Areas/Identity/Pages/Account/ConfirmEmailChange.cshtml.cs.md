# Areas/Identity/Pages/Account/ConfirmEmailChange.cshtml.cs

**Mudanca:** GRANDE | **+34** linhas | **-27** linhas

---

```diff
--- JANEIRO: Areas/Identity/Pages/Account/ConfirmEmailChange.cshtml.cs
+++ ATUAL: Areas/Identity/Pages/Account/ConfirmEmailChange.cshtml.cs
@@ -8,58 +8,65 @@
 using Microsoft.AspNetCore.Mvc;
 using Microsoft.AspNetCore.Mvc.RazorPages;
 using Microsoft.AspNetCore.WebUtilities;
-using FrotiX.Helpers;
 
 namespace FrotiX.Areas.Identity.Pages.Account
-{
+    {
     [AllowAnonymous]
     public class ConfirmEmailChangeModel : PageModel
-    {
-
+        {
         private readonly UserManager<IdentityUser> _userManager;
         private readonly SignInManager<IdentityUser> _signInManager;
 
         public ConfirmEmailChangeModel(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
-        {
+            {
             _userManager = userManager;
             _signInManager = signInManager;
-        }
+            }
 
         [TempData]
         public string StatusMessage { get; set; }
 
         public async Task<IActionResult> OnGetAsync(string userId, string email, string code)
-        {
+            {
+            try
+            {
 
-            if (userId == null || email == null || code == null)
-            {
-                return RedirectToPage("/Index");
-            }
+                if (userId == null || email == null || code == null)
+                    {
+                    return RedirectToPage("/Index");
+                    }
 
-            var user = await _userManager.FindByIdAsync(userId);
-            if (user == null)
-            {
-                return NotFound($"Unable to load user with ID '{userId}'.");
-            }
+                var user = await _userManager.FindByIdAsync(userId);
+                if (user == null)
+                    {
+                    return NotFound($"Unable to load user with ID '{userId}'.");
+                    }
 
-            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
-            var result = await _userManager.ChangeEmailAsync(user, email, code);
-            if (!result.Succeeded)
-            {
-                StatusMessage = "Error changing email.";
+                code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
+
+                var result = await _userManager.ChangeEmailAsync(user, email, code);
+                if (!result.Succeeded)
+                    {
+                    StatusMessage = "Error changing email.";
+                    return Page();
+                    }
+
+                var setUserNameResult = await _userManager.SetUserNameAsync(user, email);
+                if (!setUserNameResult.Succeeded)
+                    {
+                    StatusMessage = "Error changing user name.";
+                    return Page();
+                    }
+
+                await _signInManager.RefreshSignInAsync(user);
+                StatusMessage = "Thank you for confirming your email change.";
                 return Page();
             }
-
-            var setUserNameResult = await _userManager.SetUserNameAsync(user, email);
-            if (!setUserNameResult.Succeeded)
+            catch (Exception ex)
             {
-                StatusMessage = "Error changing user name.";
+                StatusMessage = $"Error confirming email change: {ex.Message}";
                 return Page();
             }
-
-            await _signInManager.RefreshSignInAsync(user);
-            StatusMessage = "Thank you for confirming your email change.";
-            return Page();
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
            if (userId == null || email == null || code == null)
            {
                return RedirectToPage("/Index");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{userId}'.");
            }
            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ChangeEmailAsync(user, email, code);
            if (!result.Succeeded)
            {
                StatusMessage = "Error changing email.";
            var setUserNameResult = await _userManager.SetUserNameAsync(user, email);
            if (!setUserNameResult.Succeeded)
                StatusMessage = "Error changing user name.";
            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Thank you for confirming your email change.";
            return Page();
}
```


### ADICIONAR ao Janeiro

```csharp
    {
        {
            {
            }
            {
            try
            {
                if (userId == null || email == null || code == null)
                    {
                    return RedirectToPage("/Index");
                    }
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                    {
                    return NotFound($"Unable to load user with ID '{userId}'.");
                    }
                code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
                var result = await _userManager.ChangeEmailAsync(user, email, code);
                if (!result.Succeeded)
                    {
                    StatusMessage = "Error changing email.";
                    return Page();
                    }
                var setUserNameResult = await _userManager.SetUserNameAsync(user, email);
                if (!setUserNameResult.Succeeded)
                    {
                    StatusMessage = "Error changing user name.";
                    return Page();
                    }
                await _signInManager.RefreshSignInAsync(user);
                StatusMessage = "Thank you for confirming your email change.";
            catch (Exception ex)
                StatusMessage = $"Error confirming email change: {ex.Message}";
            }
```
