# Areas/Authorization/Pages/Roles.cshtml.cs

**Mudanca:** MEDIA | **+0** linhas | **-20** linhas

---

```diff
--- JANEIRO: Areas/Authorization/Pages/Roles.cshtml.cs
+++ ATUAL: Areas/Authorization/Pages/Roles.cshtml.cs
@@ -1,35 +1,12 @@
 using Microsoft.AspNetCore.Authorization;
 using Microsoft.AspNetCore.Mvc.RazorPages;
-using FrotiX.Helpers;
-using FrotiX.Services;
-using System;
 
 namespace FrotiX.Areas.Authorization.Pages
 {
+
     [Authorize]
     public class RoleModel : PageModel
     {
 
-        private readonly ILogService _log;
-
-        public RoleModel(ILogService log)
-        {
-            _log = log;
-        }
-
-        public void OnGet()
-        {
-            try
-            {
-                _log.Info("Acesso à página de Gerenciamento de Perfis (Roles)");
-
-            }
-            catch (Exception error)
-            {
-
-                _log.Error("Erro ao carregar Gerenciamento de Perfis (Roles)", error);
-                Alerta.TratamentoErroComLinha("Roles.cshtml.cs", "OnGet", error);
-            }
-        }
     }
 }
```

### REMOVER do Janeiro

```csharp
using FrotiX.Helpers;
using FrotiX.Services;
using System;
        private readonly ILogService _log;
        public RoleModel(ILogService log)
        {
            _log = log;
        }
        public void OnGet()
        {
            try
            {
                _log.Info("Acesso à página de Gerenciamento de Perfis (Roles)");
            }
            catch (Exception error)
            {
                _log.Error("Erro ao carregar Gerenciamento de Perfis (Roles)", error);
                Alerta.TratamentoErroComLinha("Roles.cshtml.cs", "OnGet", error);
            }
        }
```

