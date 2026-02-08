# Controllers/ReportsController.cs

**Mudanca:** MEDIA | **+3** linhas | **-13** linhas

---

```diff
--- JANEIRO: Controllers/ReportsController.cs
+++ ATUAL: Controllers/ReportsController.cs
@@ -1,29 +1,18 @@
 using Microsoft.AspNetCore.Mvc;
 using Telerik.Reporting.Services;
 using Telerik.Reporting.Services.AspNetCore;
-using FrotiX.Helpers;
-using FrotiX.Services;
 
 namespace FrotiX.Controllers
 {
 
     [Route("api/reports")]
-    public class ReportsController : ReportsControllerBase
+    public class ReportsController :ReportsControllerBase
     {
-        private readonly ILogService _log;
 
-        public ReportsController(IReportServiceConfiguration reportServiceConfiguration, ILogService log)
+        public ReportsController(IReportServiceConfiguration reportServiceConfiguration)
             : base(reportServiceConfiguration)
         {
-            try
-            {
-
-                _log = log;
-            }
-            catch (System.Exception ex)
-            {
-                Alerta.TratamentoErroComLinha("ReportsController.cs", "Constructor", ex);
-            }
+            System.Diagnostics.Debug.WriteLine("🔧 ReportsController inicializado!");
         }
     }
 }
```

### REMOVER do Janeiro

```csharp
using FrotiX.Helpers;
using FrotiX.Services;
    public class ReportsController : ReportsControllerBase
        private readonly ILogService _log;
        public ReportsController(IReportServiceConfiguration reportServiceConfiguration, ILogService log)
            try
            {
                _log = log;
            }
            catch (System.Exception ex)
            {
                Alerta.TratamentoErroComLinha("ReportsController.cs", "Constructor", ex);
            }
```


### ADICIONAR ao Janeiro

```csharp
    public class ReportsController :ReportsControllerBase
        public ReportsController(IReportServiceConfiguration reportServiceConfiguration)
            System.Diagnostics.Debug.WriteLine("🔧 ReportsController inicializado!");
```
