# Controllers/TestePdfController.cs

**Mudanca:** MEDIA | **+1** linhas | **-25** linhas

---

```diff
--- JANEIRO: Controllers/TestePdfController.cs
+++ ATUAL: Controllers/TestePdfController.cs
@@ -1,6 +1,4 @@
-using FrotiX.Services;
 using Microsoft.AspNetCore.Mvc;
-using System;
 
 namespace FrotiX.Controllers
 {
@@ -9,38 +7,11 @@
     [Route("api/[controller]")]
     public class TestePdfController : Controller
     {
-        private readonly ILogService _log;
-
-        public TestePdfController(ILogService log)
-        {
-            try
-            {
-                _log = log;
-            }
-            catch (Exception error)
-            {
-                Alerta.TratamentoErroComLinha("TestePdfController.cs", "Constructor", error);
-            }
-        }
 
         [HttpGet("Ping")]
         public IActionResult Ping()
         {
-            try
-            {
-
-                _log.Info("Endpoint de Ping (TestePdf) acessado.");
-
-                return Ok(new { success = true, message = "TestePdf funcionando!" });
-            }
-            catch (Exception error)
-            {
-
-                Alerta.TratamentoErroComLinha("TestePdfController.cs", "Ping", error);
-                _log.Error("Erro no Ping de TestePdf", error);
-
-                return StatusCode(500, new { success = false, message = error.Message });
-            }
+            return Ok(new { success = true , message = "TestePdf funcionando!" });
         }
     }
 }
```

### REMOVER do Janeiro

```csharp
using FrotiX.Services;
using System;
        private readonly ILogService _log;
        public TestePdfController(ILogService log)
        {
            try
            {
                _log = log;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("TestePdfController.cs", "Constructor", error);
            }
        }
            try
            {
                _log.Info("Endpoint de Ping (TestePdf) acessado.");
                return Ok(new { success = true, message = "TestePdf funcionando!" });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("TestePdfController.cs", "Ping", error);
                _log.Error("Erro no Ping de TestePdf", error);
                return StatusCode(500, new { success = false, message = error.Message });
            }
```


### ADICIONAR ao Janeiro

```csharp
            return Ok(new { success = true , message = "TestePdf funcionando!" });
```
