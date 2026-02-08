# Areas/Identity/Pages/Account/ConfirmEmail.cshtml.cs

**Mudanca:** GRANDE | **+24** linhas | **-17** linhas

---

```diff
--- JANEIRO: Areas/Identity/Pages/Account/ConfirmEmail.cshtml.cs
+++ ATUAL: Areas/Identity/Pages/Account/ConfirmEmail.cshtml.cs
@@ -8,43 +8,50 @@
 using Microsoft.AspNetCore.Mvc;
 using Microsoft.AspNetCore.Mvc.RazorPages;
 using Microsoft.AspNetCore.WebUtilities;
-using FrotiX.Helpers;
 
 namespace FrotiX.Identity.Pages.Account
-{
+    {
     [AllowAnonymous]
     public class ConfirmEmailModel : PageModel
-    {
-
+        {
         private readonly UserManager<IdentityUser> _userManager;
 
         public ConfirmEmailModel(UserManager<IdentityUser> userManager)
-        {
+            {
             _userManager = userManager;
-        }
+            }
 
         [TempData]
         public string StatusMessage { get; set; }
 
         public async Task<IActionResult> OnGetAsync(string userId, string code)
-        {
+            {
+            try
+            {
 
-            if (userId == null || code == null)
+                if (userId == null || code == null)
+                    {
+                    return RedirectToPage("/Index");
+                    }
+
+                var user = await _userManager.FindByIdAsync(userId);
+                if (user == null)
+                    {
+                    return NotFound($"Unable to load user with ID '{userId}'.");
+                    }
+
+                code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
+
+                var result = await _userManager.ConfirmEmailAsync(user, code);
+
+                StatusMessage = result.Succeeded ? "Thank you for confirming your email." : "Error confirming your email.";
+                return Page();
+            }
+            catch (Exception ex)
             {
-                return RedirectToPage("/Index");
+                StatusMessage = $"Error confirming your email: {ex.Message}";
+                return Page();
             }
-
-            var user = await _userManager.FindByIdAsync(userId);
-            if (user == null)
-            {
-                return NotFound($"Unable to load user with ID '{userId}'.");
             }
-
-            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
-            var result = await _userManager.ConfirmEmailAsync(user, code);
-
-            StatusMessage = result.Succeeded ? "Thank you for confirming your email." : "Error confirming your email.";
-            return Page();
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
            if (userId == null || code == null)
                return RedirectToPage("/Index");
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{userId}'.");
            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, code);
            StatusMessage = result.Succeeded ? "Thank you for confirming your email." : "Error confirming your email.";
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
                if (userId == null || code == null)
                    {
                    return RedirectToPage("/Index");
                    }
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                    {
                    return NotFound($"Unable to load user with ID '{userId}'.");
                    }
                code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
                var result = await _userManager.ConfirmEmailAsync(user, code);
                StatusMessage = result.Succeeded ? "Thank you for confirming your email." : "Error confirming your email.";
                return Page();
            }
            catch (Exception ex)
                StatusMessage = $"Error confirming your email: {ex.Message}";
                return Page();
```
