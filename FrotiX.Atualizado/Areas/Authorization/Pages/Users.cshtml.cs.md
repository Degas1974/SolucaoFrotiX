# Areas/Authorization/Pages/Users.cshtml.cs

**Mudanca:** MEDIA | **+0** linhas | **-12** linhas

---

```diff
--- JANEIRO: Areas/Authorization/Pages/Users.cshtml.cs
+++ ATUAL: Areas/Authorization/Pages/Users.cshtml.cs
@@ -1,25 +1,12 @@
 using Microsoft.AspNetCore.Authorization;
 using Microsoft.AspNetCore.Mvc.RazorPages;
-using FrotiX.Helpers;
-using System;
 
 namespace FrotiX.Areas.Authorization.Pages
 {
+
     [Authorize]
     public class UserModel : PageModel
     {
 
-        public void OnGet()
-        {
-            try
-            {
-
-            }
-            catch (Exception error)
-            {
-
-                Alerta.TratamentoErroComLinha("Users.cshtml.cs", "OnGet", error);
-            }
-        }
     }
 }
```

### REMOVER do Janeiro

```csharp
using FrotiX.Helpers;
using System;
        public void OnGet()
        {
            try
            {
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("Users.cshtml.cs", "OnGet", error);
            }
        }
```

