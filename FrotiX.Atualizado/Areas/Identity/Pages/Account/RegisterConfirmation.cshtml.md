# Areas/Identity/Pages/Account/RegisterConfirmation.cshtml

**Mudanca:** PEQUENA | **+0** linhas | **-0** linhas

---

```diff
--- JANEIRO: Areas/Identity/Pages/Account/RegisterConfirmation.cshtml
+++ ATUAL: Areas/Identity/Pages/Account/RegisterConfirmation.cshtml
@@ -5,9 +5,11 @@
 }
 
 <h1>@ViewData["Title"]</h1>
+
 @{
     if (@Model.DisplayConfirmAccountLink)
     {
+
 <p>
     This app does not currently have a real email sender registered, see <a href="https://aka.ms/aspaccountconf">these docs</a> for how to configure a real email sender.
     Normally this would be emailed: <a id="confirm-link" href="@Model.EmailConfirmationUrl">Click here to confirm your account</a>
@@ -15,6 +17,7 @@
     }
     else
     {
+
 <p>
         Please check your email to confirm your account.
 </p>
```
