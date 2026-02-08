# Models/LoginView.cs

**Mudanca:** PEQUENA | **+0** linhas | **-0** linhas

---

```diff
--- JANEIRO: Models/LoginView.cs
+++ ATUAL: Models/LoginView.cs
@@ -6,11 +6,14 @@
 using System.ComponentModel.DataAnnotations;
 namespace FrotiX.Models
     {
+
     public class LoginView
         {
+
         [Required]
         [UIHint("username")]
         public string UserName { get; set; }
+
         [Required]
         [UIHint("password")]
         public string Password { get; set; }
```
