# Models/ErrorViewModel.cs

**Mudanca:** PEQUENA | **+0** linhas | **-0** linhas

---

```diff
--- JANEIRO: Models/ErrorViewModel.cs
+++ ATUAL: Models/ErrorViewModel.cs
@@ -4,8 +4,10 @@
 
 namespace FrotiX.Models
     {
+
     public class ErrorViewModel
         {
+
         public string RequestId { get; set; }
 
         public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
@@ -13,6 +15,7 @@
 
     public class EmailSender : IEmailSender
         {
+
         public Task SendEmailAsync(string email, string subject, string htmlMessage)
             {
             throw new NotImplementedException("No email provider is implemented by default, please Google on how to add one, like SendGrid.");
```
