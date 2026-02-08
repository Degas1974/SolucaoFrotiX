# Controllers/NormalizeController.cs

**Mudanca:** MEDIA | **+3** linhas | **-11** linhas

---

```diff
--- JANEIRO: Controllers/NormalizeController.cs
+++ ATUAL: Controllers/NormalizeController.cs
@@ -1,4 +1,3 @@
-using FrotiX.Services;
 using FrotiX.TextNormalization;
 using Microsoft.AspNetCore.Mvc;
 using System;
@@ -9,21 +8,19 @@
 
     [ApiController]
     [Route("api/[controller]")]
-    public class NormalizeController : ControllerBase
+    public class NormalizeController :ControllerBase
     {
         private readonly NormalizationService _normalizer;
-        private readonly ILogService _log;
 
-        public NormalizeController(NormalizationService normalizer, ILogService log)
+        public NormalizeController(NormalizationService normalizer)
         {
             try
             {
                 _normalizer = normalizer;
-                _log = log;
             }
             catch (Exception error)
             {
-                Alerta.TratamentoErroComLinha("NormalizeController.cs", "NormalizeController", error);
+                Alerta.TratamentoErroComLinha("NormalizeController.cs" , "NormalizeController" , error);
             }
         }
 
@@ -36,20 +33,13 @@
             {
 
                 if (body is null || string.IsNullOrWhiteSpace(body.Text))
-                {
-                    _log.Warning("NormalizeController.Post: Requisição sem texto.");
                     return BadRequest("Text is required.");
-                }
 
                 var result = await _normalizer.NormalizeAsync(body.Text);
-
-                _log.Info($"NormalizeController.Post: Texto normalizado com sucesso. Entrada: {body.Text} | Saída: {result}");
-
                 return Ok(result);
             }
             catch (Exception error)
             {
-                _log.Error("NormalizeController.Post", error);
                 Alerta.TratamentoErroComLinha("NormalizeController.cs" , "Post" , error);
                 return StatusCode(500 , "Erro ao normalizar texto");
             }
```

### REMOVER do Janeiro

```csharp
using FrotiX.Services;
    public class NormalizeController : ControllerBase
        private readonly ILogService _log;
        public NormalizeController(NormalizationService normalizer, ILogService log)
                _log = log;
                Alerta.TratamentoErroComLinha("NormalizeController.cs", "NormalizeController", error);
                {
                    _log.Warning("NormalizeController.Post: Requisição sem texto.");
                }
                _log.Info($"NormalizeController.Post: Texto normalizado com sucesso. Entrada: {body.Text} | Saída: {result}");
                _log.Error("NormalizeController.Post", error);
```


### ADICIONAR ao Janeiro

```csharp
    public class NormalizeController :ControllerBase
        public NormalizeController(NormalizationService normalizer)
                Alerta.TratamentoErroComLinha("NormalizeController.cs" , "NormalizeController" , error);
```
