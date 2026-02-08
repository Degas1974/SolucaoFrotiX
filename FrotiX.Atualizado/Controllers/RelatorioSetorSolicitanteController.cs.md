# Controllers/RelatorioSetorSolicitanteController.cs

**Mudanca:** GRANDE | **+26** linhas | **-27** linhas

---

```diff
--- JANEIRO: Controllers/RelatorioSetorSolicitanteController.cs
+++ ATUAL: Controllers/RelatorioSetorSolicitanteController.cs
@@ -5,8 +5,6 @@
 using Microsoft.AspNetCore.Mvc;
 using Stimulsoft.Report;
 using Stimulsoft.Report.Mvc;
-using FrotiX.Helpers;
-using FrotiX.Services;
 
 namespace FrotiX.Controllers
 {
@@ -14,30 +12,23 @@
     [Route("SetorSolicitante/RelatorioSetorSolicitante")]
     public class RelatorioSetorSolicitanteController : Controller
     {
-        private readonly ILogService _log;
 
         static RelatorioSetorSolicitanteController()
         {
             try
             {
 
-                Stimulsoft.Base.StiLicense.Key = "6vJhGtLLLz2GNviWmUTrhSqnOItdDwjBylQzQcAOiHkgpgFGkUl79uxVs8X+uspx6K+tqdtOB5G1S6PFPRrlVNvMUiSiNYl724EZbrUAWwAYHlGLRbvxMviMExTh2l9xZJ2xc4K1z3ZVudRpQpuDdFq+fe0wKXSKlB6okl0hUd2ikQHfyzsAN8fJltqvGRa5LI8BFkA/f7tffwK6jzW5xYYhHxQpU3hy4fmKo/BSg6yKAoUq3yMZTG6tWeKnWcI6ftCDxEHd30EjMISNn1LCdLN0/4YmedTjM7x+0dMiI2Qif/yI+y8gmdbostOE8S2ZjrpKsgxVv2AAZPdzHEkzYSzx81RHDzZBhKRZc5mwWAmXsWBFRQol9PdSQ8BZYLqvJ4Jzrcrext+t1ZD7HE1RZPLPAqErO9eo+7Zn9Cvu5O73+b9dxhE2sRyAv9Tl1lV2WqMezWRsO55Q3LntawkPq0HvBkd9f8uVuq9zk7VKegetCDLb0wszBAs1mjWzN+ACVHiPVKIk94/QlCkj31dWCg8YTrT5btsKcLibxog7pv1+2e4yocZKWsposmcJbgG0";
+                Stimulsoft.Base.StiLicense.Key =
+                    "6vJhGtLLLz2GNviWmUTrhSqnOItdDwjBylQzQcAOiHkgpgFGkUl79uxVs8X+uspx6K+tqdtOB5G1S6PFPRrlVNvMUiSiNYl724EZbrUAWwAYHlGLRbvxMviMExTh2l9xZJ2xc4K1z3ZVudRpQpuDdFq+fe0wKXSKlB6okl0hUd2ikQHfyzsAN8fJltqvGRa5LI8BFkA/f7tffwK6jzW5xYYhHxQpU3hy4fmKo/BSg6yKAoUq3yMZTG6tWeKnWcI6ftCDxEHd30EjMISNn1LCdLN0/4YmedTjM7x+0dMiI2Qif/yI+y8gmdbostOE8S2ZjrpKsgxVv2AAZPdzHEkzYSzx81RHDzZBhKRZc5mwWAmXsWBFRQol9PdSQ8BZYLqvJ4Jzrcrext+t1ZD7HE1RZPLPAqErO9eo+7Zn9Cvu5O73+b9dxhE2sRyAv9Tl1lV2WqMezWRsO55Q3LntawkPq0HvBkd9f8uVuq9zk7VKegetCDLb0wszBAs1mjWzN+ACVHiPVKIk94/QlCkj31dWCg8YTrT5btsKcLibxog7pv1+2e4yocZKWsposmcJbgG0";
+
             }
-            catch (Exception ex)
+            catch (Exception error)
             {
-                Alerta.TratamentoErroComLinha("RelatorioSetorSolicitanteController.cs", "Static Constructor", ex);
-            }
-        }
-
-        public RelatorioSetorSolicitanteController(ILogService log)
-        {
-            try
-            {
-                _log = log;
-            }
-            catch (Exception ex)
-            {
-                Alerta.TratamentoErroComLinha("RelatorioSetorSolicitanteController.cs", "Constructor", ex);
+                Alerta.TratamentoErroComLinha(
+                    "RelatorioSetorSolicitanteController.cs",
+                    "RelatorioSetorSolicitanteController",
+                    error
+                );
             }
         }
 
@@ -46,14 +37,15 @@
         {
             try
             {
-
                 return View();
             }
-            catch (Exception ex)
+            catch (Exception error)
             {
-                _log.Error("RelatorioSetorSolicitanteController.Index", ex);
-                Alerta.TratamentoErroComLinha("RelatorioSetorSolicitanteController.cs", "Index", ex);
-
+                Alerta.TratamentoErroComLinha(
+                    "RelatorioSetorSolicitanteController.cs",
+                    "Index",
+                    error
+                );
                 return View();
             }
         }
@@ -63,22 +55,19 @@
         {
             try
             {
-
-                _log.Info("RelatorioSetorSolicitanteController.GetReport: Carregando relatório de Setores Solicitantes.");
-
                 StiReport report = new StiReport();
-
                 report.Dictionary.DataStore.Clear();
-
                 report.Load(StiNetCoreHelper.MapPath(this, "Reports/SetoresSolicitantes.mrt"));
 
                 return StiNetCoreViewer.GetReportResult(this, report);
             }
-            catch (Exception ex)
+            catch (Exception error)
             {
-                _log.Error("RelatorioSetorSolicitanteController.GetReport", ex);
-                Alerta.TratamentoErroComLinha("RelatorioSetorSolicitanteController.cs", "GetReport", ex);
-
+                Alerta.TratamentoErroComLinha(
+                    "RelatorioSetorSolicitanteController.cs",
+                    "GetReport",
+                    error
+                );
                 return View();
             }
         }
@@ -88,14 +77,15 @@
         {
             try
             {
-
                 return StiNetCoreViewer.ViewerEventResult(this);
             }
-            catch (Exception ex)
+            catch (Exception error)
             {
-                _log.Error("RelatorioSetorSolicitanteController.ViewerEvent", ex);
-                Alerta.TratamentoErroComLinha("RelatorioSetorSolicitanteController.cs", "ViewerEvent", ex);
-
+                Alerta.TratamentoErroComLinha(
+                    "RelatorioSetorSolicitanteController.cs",
+                    "ViewerEvent",
+                    error
+                );
                 return View();
             }
         }
```

### REMOVER do Janeiro

```csharp
using FrotiX.Helpers;
using FrotiX.Services;
        private readonly ILogService _log;
                Stimulsoft.Base.StiLicense.Key = "6vJhGtLLLz2GNviWmUTrhSqnOItdDwjBylQzQcAOiHkgpgFGkUl79uxVs8X+uspx6K+tqdtOB5G1S6PFPRrlVNvMUiSiNYl724EZbrUAWwAYHlGLRbvxMviMExTh2l9xZJ2xc4K1z3ZVudRpQpuDdFq+fe0wKXSKlB6okl0hUd2ikQHfyzsAN8fJltqvGRa5LI8BFkA/f7tffwK6jzW5xYYhHxQpU3hy4fmKo/BSg6yKAoUq3yMZTG6tWeKnWcI6ftCDxEHd30EjMISNn1LCdLN0/4YmedTjM7x+0dMiI2Qif/yI+y8gmdbostOE8S2ZjrpKsgxVv2AAZPdzHEkzYSzx81RHDzZBhKRZc5mwWAmXsWBFRQol9PdSQ8BZYLqvJ4Jzrcrext+t1ZD7HE1RZPLPAqErO9eo+7Zn9Cvu5O73+b9dxhE2sRyAv9Tl1lV2WqMezWRsO55Q3LntawkPq0HvBkd9f8uVuq9zk7VKegetCDLb0wszBAs1mjWzN+ACVHiPVKIk94/QlCkj31dWCg8YTrT5btsKcLibxog7pv1+2e4yocZKWsposmcJbgG0";
            catch (Exception ex)
                Alerta.TratamentoErroComLinha("RelatorioSetorSolicitanteController.cs", "Static Constructor", ex);
            }
        }
        public RelatorioSetorSolicitanteController(ILogService log)
        {
            try
            {
                _log = log;
            }
            catch (Exception ex)
            {
                Alerta.TratamentoErroComLinha("RelatorioSetorSolicitanteController.cs", "Constructor", ex);
            catch (Exception ex)
                _log.Error("RelatorioSetorSolicitanteController.Index", ex);
                Alerta.TratamentoErroComLinha("RelatorioSetorSolicitanteController.cs", "Index", ex);
                _log.Info("RelatorioSetorSolicitanteController.GetReport: Carregando relatório de Setores Solicitantes.");
            catch (Exception ex)
                _log.Error("RelatorioSetorSolicitanteController.GetReport", ex);
                Alerta.TratamentoErroComLinha("RelatorioSetorSolicitanteController.cs", "GetReport", ex);
            catch (Exception ex)
                _log.Error("RelatorioSetorSolicitanteController.ViewerEvent", ex);
                Alerta.TratamentoErroComLinha("RelatorioSetorSolicitanteController.cs", "ViewerEvent", ex);
```


### ADICIONAR ao Janeiro

```csharp
                Stimulsoft.Base.StiLicense.Key =
                    "6vJhGtLLLz2GNviWmUTrhSqnOItdDwjBylQzQcAOiHkgpgFGkUl79uxVs8X+uspx6K+tqdtOB5G1S6PFPRrlVNvMUiSiNYl724EZbrUAWwAYHlGLRbvxMviMExTh2l9xZJ2xc4K1z3ZVudRpQpuDdFq+fe0wKXSKlB6okl0hUd2ikQHfyzsAN8fJltqvGRa5LI8BFkA/f7tffwK6jzW5xYYhHxQpU3hy4fmKo/BSg6yKAoUq3yMZTG6tWeKnWcI6ftCDxEHd30EjMISNn1LCdLN0/4YmedTjM7x+0dMiI2Qif/yI+y8gmdbostOE8S2ZjrpKsgxVv2AAZPdzHEkzYSzx81RHDzZBhKRZc5mwWAmXsWBFRQol9PdSQ8BZYLqvJ4Jzrcrext+t1ZD7HE1RZPLPAqErO9eo+7Zn9Cvu5O73+b9dxhE2sRyAv9Tl1lV2WqMezWRsO55Q3LntawkPq0HvBkd9f8uVuq9zk7VKegetCDLb0wszBAs1mjWzN+ACVHiPVKIk94/QlCkj31dWCg8YTrT5btsKcLibxog7pv1+2e4yocZKWsposmcJbgG0";
            catch (Exception error)
                Alerta.TratamentoErroComLinha(
                    "RelatorioSetorSolicitanteController.cs",
                    "RelatorioSetorSolicitanteController",
                    error
                );
            catch (Exception error)
                Alerta.TratamentoErroComLinha(
                    "RelatorioSetorSolicitanteController.cs",
                    "Index",
                    error
                );
            catch (Exception error)
                Alerta.TratamentoErroComLinha(
                    "RelatorioSetorSolicitanteController.cs",
                    "GetReport",
                    error
                );
            catch (Exception error)
                Alerta.TratamentoErroComLinha(
                    "RelatorioSetorSolicitanteController.cs",
                    "ViewerEvent",
                    error
                );
```
