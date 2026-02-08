# Services/CustomReportSourceResolver.cs

**Mudanca:** GRANDE | **+27** linhas | **-15** linhas

---

```diff
--- JANEIRO: Services/CustomReportSourceResolver.cs
+++ ATUAL: Services/CustomReportSourceResolver.cs
@@ -1,4 +1,5 @@
 using Microsoft.AspNetCore.Hosting;
+using System;
 using System.Collections.Generic;
 using System.IO;
 using Telerik.Reporting;
@@ -17,30 +18,41 @@
 
         public ReportSource Resolve(string reportId , OperationOrigin operationOrigin , IDictionary<string , object> currentParameterValues)
         {
+            try
+            {
 
-            var reportsPath = Path.Combine(_environment.ContentRootPath , "Reports");
-            var reportPath = Path.Combine(reportsPath , reportId);
+                var reportsPath = Path.Combine(_environment.ContentRootPath , "Reports");
+                var reportPath = Path.Combine(reportsPath , reportId);
 
-            if (!reportPath.EndsWith(".trdp") && !reportPath.EndsWith(".trdx"))
-                reportPath += ".trdp";
+                if (!reportPath.EndsWith(".trdp") && !reportPath.EndsWith(".trdx"))
+                    reportPath += ".trdp";
 
-            if (!File.Exists(reportPath))
-                throw new FileNotFoundException($"Relatório não encontrado: {reportId}");
+                if (!File.Exists(reportPath))
+                    throw new FileNotFoundException($"Relatório não encontrado: {reportId}");
 
-            var reportPackageSource = new UriReportSource
+                var reportPackageSource = new UriReportSource
+                {
+                    Uri = reportPath
+                };
+
+                if (currentParameterValues != null)
+                {
+                    foreach (var param in currentParameterValues)
+                    {
+                        reportPackageSource.Parameters.Add(param.Key, param.Value);
+                    }
+                }
+
+                return reportPackageSource;
+            }
+            catch (FileNotFoundException)
             {
-                Uri = reportPath
-            };
-
-            if (currentParameterValues != null)
+                throw;
+            }
+            catch (Exception ex)
             {
-                foreach (var param in currentParameterValues)
-                {
-                    reportPackageSource.Parameters.Add(param.Key, param.Value);
-                }
+                throw new InvalidOperationException($"Erro ao resolver relatório '{reportId}': {ex.Message}", ex);
             }
-
-            return reportPackageSource;
         }
     }
 }
```

### REMOVER do Janeiro

```csharp
            var reportsPath = Path.Combine(_environment.ContentRootPath , "Reports");
            var reportPath = Path.Combine(reportsPath , reportId);
            if (!reportPath.EndsWith(".trdp") && !reportPath.EndsWith(".trdx"))
                reportPath += ".trdp";
            if (!File.Exists(reportPath))
                throw new FileNotFoundException($"Relatório não encontrado: {reportId}");
            var reportPackageSource = new UriReportSource
                Uri = reportPath
            };
            if (currentParameterValues != null)
                foreach (var param in currentParameterValues)
                {
                    reportPackageSource.Parameters.Add(param.Key, param.Value);
                }
            return reportPackageSource;
```


### ADICIONAR ao Janeiro

```csharp
using System;
            try
            {
                var reportsPath = Path.Combine(_environment.ContentRootPath , "Reports");
                var reportPath = Path.Combine(reportsPath , reportId);
                if (!reportPath.EndsWith(".trdp") && !reportPath.EndsWith(".trdx"))
                    reportPath += ".trdp";
                if (!File.Exists(reportPath))
                    throw new FileNotFoundException($"Relatório não encontrado: {reportId}");
                var reportPackageSource = new UriReportSource
                {
                    Uri = reportPath
                };
                if (currentParameterValues != null)
                {
                    foreach (var param in currentParameterValues)
                    {
                        reportPackageSource.Parameters.Add(param.Key, param.Value);
                    }
                }
                return reportPackageSource;
            }
            catch (FileNotFoundException)
                throw;
            }
            catch (Exception ex)
                throw new InvalidOperationException($"Erro ao resolver relatório '{reportId}': {ex.Message}", ex);
```
