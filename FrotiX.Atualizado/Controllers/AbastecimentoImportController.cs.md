# Controllers/AbastecimentoImportController.cs

**Mudanca:** GRANDE | **+15** linhas | **-38** linhas

---

```diff
--- JANEIRO: Controllers/AbastecimentoImportController.cs
+++ ATUAL: Controllers/AbastecimentoImportController.cs
@@ -2,7 +2,6 @@
 using FrotiX.Hubs;
 using FrotiX.Models;
 using FrotiX.Repository.IRepository;
-using FrotiX.Services;
 using Microsoft.AspNetCore.Hosting;
 using Microsoft.AspNetCore.Http;
 using Microsoft.AspNetCore.Mvc;
@@ -10,7 +9,6 @@
 using Microsoft.Extensions.Logging;
 using System;
 using System.Threading.Tasks;
-using FrotiX.Helpers;
 
 namespace FrotiX.Controllers
 {
@@ -23,58 +21,37 @@
         private readonly IUnitOfWork _unitOfWork;
         private readonly IHubContext<ImportacaoHub> _hubContext;
         private readonly FrotiXDbContext _context;
-        private readonly ILogService _log;
 
         public AbastecimentoImportController(
             ILogger<AbastecimentoImportController> logger,
             IWebHostEnvironment hostingEnvironment,
             IUnitOfWork unitOfWork,
             IHubContext<ImportacaoHub> hubContext,
-            FrotiXDbContext context,
-            ILogService log
-        )
+            FrotiXDbContext context)
         {
-            try
-            {
-                _logger = logger;
-                _hostingEnvironment = hostingEnvironment;
-                _unitOfWork = unitOfWork;
-                _hubContext = hubContext;
-                _context = context;
-                _log = log;
-            }
-            catch (Exception ex)
-            {
-                Alerta.TratamentoErroComLinha("AbastecimentoImportController.cs", "Constructor", ex);
-            }
+            _logger = logger;
+            _hostingEnvironment = hostingEnvironment;
+            _unitOfWork = unitOfWork;
+            _hubContext = hubContext;
+            _context = context;
         }
 
         [Route("ImportarDual")]
         [HttpPost]
         public async Task<ActionResult> ImportarDual()
         {
-            try
-            {
 
-                var mainController = new AbastecimentoController(
-                    _logger as ILogger<AbastecimentoController>,
-                    _hostingEnvironment,
-                    _unitOfWork,
-                    _hubContext,
-                    _context,
-                    _log
-                );
+            var mainController = new AbastecimentoController(
+                _logger as ILogger<AbastecimentoController>,
+                _hostingEnvironment,
+                _unitOfWork,
+                _hubContext,
+                _context
+            );
 
-                mainController.ControllerContext = this.ControllerContext;
+            mainController.ControllerContext = this.ControllerContext;
 
-                return await mainController.ImportarDualInternal();
-            }
-            catch (Exception error)
-            {
-                _log.Error("Erro ao processar importaÃ§Ã£o dual via proxy", error, "AbastecimentoImportController.cs", "ImportarDual");
-                Alerta.TratamentoErroComLinha("AbastecimentoImportController.cs", "ImportarDual", error);
-                return StatusCode(500, new { message = "Erro ao processar importaÃ§Ã£o dual" });
-            }
+            return await mainController.ImportarDualInternal();
         }
     }
 }
```

### REMOVER do Janeiro

```csharp
using FrotiX.Services;
using FrotiX.Helpers;
        private readonly ILogService _log;
            FrotiXDbContext context,
            ILogService log
        )
            try
            {
                _logger = logger;
                _hostingEnvironment = hostingEnvironment;
                _unitOfWork = unitOfWork;
                _hubContext = hubContext;
                _context = context;
                _log = log;
            }
            catch (Exception ex)
            {
                Alerta.TratamentoErroComLinha("AbastecimentoImportController.cs", "Constructor", ex);
            }
            try
            {
                var mainController = new AbastecimentoController(
                    _logger as ILogger<AbastecimentoController>,
                    _hostingEnvironment,
                    _unitOfWork,
                    _hubContext,
                    _context,
                    _log
                );
                mainController.ControllerContext = this.ControllerContext;
                return await mainController.ImportarDualInternal();
            }
            catch (Exception error)
            {
                _log.Error("Erro ao processar importaÃ§Ã£o dual via proxy", error, "AbastecimentoImportController.cs", "ImportarDual");
                Alerta.TratamentoErroComLinha("AbastecimentoImportController.cs", "ImportarDual", error);
                return StatusCode(500, new { message = "Erro ao processar importaÃ§Ã£o dual" });
            }
```


### ADICIONAR ao Janeiro

```csharp
            FrotiXDbContext context)
            _logger = logger;
            _hostingEnvironment = hostingEnvironment;
            _unitOfWork = unitOfWork;
            _hubContext = hubContext;
            _context = context;
            var mainController = new AbastecimentoController(
                _logger as ILogger<AbastecimentoController>,
                _hostingEnvironment,
                _unitOfWork,
                _hubContext,
                _context
            );
            mainController.ControllerContext = this.ControllerContext;
            return await mainController.ImportarDualInternal();
```
