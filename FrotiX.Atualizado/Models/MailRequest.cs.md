# Models/MailRequest.cs

**Mudanca:** PEQUENA | **+0** linhas | **-0** linhas

---

```diff
--- JANEIRO: Models/MailRequest.cs
+++ ATUAL: Models/MailRequest.cs
@@ -5,10 +5,14 @@
 
 namespace FrotiX.Models
     {
+
     public class MailRequest
         {
+
         public string ToEmail { get; set; }
+
         public string Subject { get; set; }
+
         public string Body { get; set; }
         }
     }
```
