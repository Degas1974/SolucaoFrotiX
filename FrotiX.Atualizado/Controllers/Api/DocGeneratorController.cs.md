# Controllers/Api/DocGeneratorController.cs

**Mudanca:** MEDIA | **+5** linhas | **-11** linhas

---

```diff
--- JANEIRO: Controllers/Api/DocGeneratorController.cs
+++ ATUAL: Controllers/Api/DocGeneratorController.cs
@@ -7,8 +7,6 @@
 using Microsoft.AspNetCore.Mvc;
 using Microsoft.Extensions.Logging;
 using Microsoft.Extensions.Options;
-using FrotiX.Helpers;
-using FrotiX.Services;
 
 namespace FrotiX.Controllers.Api
 {
@@ -22,15 +20,12 @@
         private readonly IDocGeneratorOrchestrator _orchestrator;
         private readonly IDocCacheService _cacheService;
         private readonly ILogger<DocGeneratorController> _logger;
-        private readonly ILogService _log;
 
         public DocGeneratorController(
             IFileDiscoveryService discoveryService,
             IDocGeneratorOrchestrator orchestrator,
             IDocCacheService cacheService,
-            ILogger<DocGeneratorController> logger,
-            ILogService log
-        )
+            ILogger<DocGeneratorController> logger)
         {
             try
             {
@@ -38,11 +33,10 @@
                 _orchestrator = orchestrator;
                 _cacheService = cacheService;
                 _logger = logger;
-                _log = log;
-            }
-            catch (Exception ex)
-            {
-                Alerta.TratamentoErroComLinha("DocGeneratorController.cs", "Constructor", ex);
+            }
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha("DocGeneratorController.cs", ".ctor", error);
             }
         }
 
@@ -51,7 +45,6 @@
         {
             try
             {
-
                 var result = await _discoveryService.DiscoverAsync(ct);
                 return Json(new
                 {
@@ -93,7 +86,6 @@
         {
             try
             {
-
                 _logger.LogInformation(
                     "Generate request: ForceRegen={Force}, UseAi={UseAi}, Provider={Provider}, Paths={Paths}",
                     request.ForceRegenerate,
@@ -139,7 +131,6 @@
         {
             try
             {
-
                 var job = await _orchestrator.GetJobStatusAsync(jobId, ct);
 
                 if (job == null)
```

### REMOVER do Janeiro

```csharp
using FrotiX.Helpers;
using FrotiX.Services;
        private readonly ILogService _log;
            ILogger<DocGeneratorController> logger,
            ILogService log
        )
                _log = log;
            }
            catch (Exception ex)
            {
                Alerta.TratamentoErroComLinha("DocGeneratorController.cs", "Constructor", ex);
```


### ADICIONAR ao Janeiro

```csharp
            ILogger<DocGeneratorController> logger)
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("DocGeneratorController.cs", ".ctor", error);
```
