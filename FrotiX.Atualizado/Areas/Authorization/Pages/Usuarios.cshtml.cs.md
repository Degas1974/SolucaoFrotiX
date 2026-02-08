# Areas/Authorization/Pages/Usuarios.cshtml.cs

**Mudanca:** MEDIA | **+0** linhas | **-20** linhas

---

```diff
--- JANEIRO: Areas/Authorization/Pages/Usuarios.cshtml.cs
+++ ATUAL: Areas/Authorization/Pages/Usuarios.cshtml.cs
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
     public class UsuariosModel : PageModel
     {
 
-        private readonly ILogService _log;
-
-        public UsuariosModel(ILogService log)
-        {
-            _log = log;
-        }
-
-        public void OnGet()
-        {
-            try
-            {
-                _log.Info("Acesso à página de Gerenciamento de Usuários");
-
-            }
-            catch (Exception error)
-            {
-
-                _log.Error("Erro ao carregar Gerenciamento de Usuários", error);
-                Alerta.TratamentoErroComLinha("Usuarios.cshtml.cs", "OnGet", error);
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
        public UsuariosModel(ILogService log)
        {
            _log = log;
        }
        public void OnGet()
        {
            try
            {
                _log.Info("Acesso à página de Gerenciamento de Usuários");
            }
            catch (Exception error)
            {
                _log.Error("Erro ao carregar Gerenciamento de Usuários", error);
                Alerta.TratamentoErroComLinha("Usuarios.cshtml.cs", "OnGet", error);
            }
        }
```

