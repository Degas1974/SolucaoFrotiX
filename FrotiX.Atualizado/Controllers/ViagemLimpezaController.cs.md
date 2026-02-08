# Controllers/ViagemLimpezaController.cs

**Mudanca:** MEDIA | **+7** linhas | **-6** linhas

---

```diff
--- JANEIRO: Controllers/ViagemLimpezaController.cs
+++ ATUAL: Controllers/ViagemLimpezaController.cs
@@ -3,28 +3,29 @@
 using System;
 using System.Collections.Generic;
 using System.Threading.Tasks;
-using FrotiX.Services;
 
 namespace FrotiX.Controllers
 {
 
     [Route("api/[controller]")]
     [ApiController]
-    public class ViagemLimpezaController : ControllerBase
+    public class ViagemLimpezaController :ControllerBase
     {
         private readonly IViagemRepository _viagemRepo;
-        private readonly ILogService _log;
 
-        public ViagemLimpezaController(IViagemRepository viagemRepo, ILogService log)
+        public ViagemLimpezaController(IViagemRepository viagemRepo)
         {
             try
             {
                 _viagemRepo = viagemRepo;
-                _log = log;
             }
             catch (Exception error)
             {
-                Alerta.TratamentoErroComLinha("ViagemLimpezaController.cs", "ViagemLimpezaController", error);
+                Alerta.TratamentoErroComLinha(
+                    "ViagemLimpezaController.cs" ,
+                    "ViagemLimpezaController" ,
+                    error
+                );
             }
         }
 
@@ -33,9 +34,7 @@
         {
             try
             {
-
                 var origens = await _viagemRepo.GetDistinctOrigensAsync();
-
                 return Ok(origens);
             }
             catch (Exception error)
@@ -54,9 +53,7 @@
         {
             try
             {
-
                 var destinos = await _viagemRepo.GetDistinctDestinosAsync();
-
                 return Ok(destinos);
             }
             catch (Exception error)
@@ -75,9 +72,7 @@
         {
             try
             {
-
                 await _viagemRepo.CorrigirOrigemAsync(request.Anteriores , request.NovoValor);
-
                 return NoContent();
             }
             catch (Exception error)
@@ -100,9 +95,7 @@
         {
             try
             {
-
                 await _viagemRepo.CorrigirDestinoAsync(request.Anteriores , request.NovoValor);
-
                 return NoContent();
             }
             catch (Exception error)
```

### REMOVER do Janeiro

```csharp
using FrotiX.Services;
    public class ViagemLimpezaController : ControllerBase
        private readonly ILogService _log;
        public ViagemLimpezaController(IViagemRepository viagemRepo, ILogService log)
                _log = log;
                Alerta.TratamentoErroComLinha("ViagemLimpezaController.cs", "ViagemLimpezaController", error);
```


### ADICIONAR ao Janeiro

```csharp
    public class ViagemLimpezaController :ControllerBase
        public ViagemLimpezaController(IViagemRepository viagemRepo)
                Alerta.TratamentoErroComLinha(
                    "ViagemLimpezaController.cs" ,
                    "ViagemLimpezaController" ,
                    error
                );
```
