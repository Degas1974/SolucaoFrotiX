# Services/DocGenerator/Services/FileTrackingService.cs

**Mudanca:** GRANDE | **+1** linhas | **-30** linhas

---

```diff
--- JANEIRO: Services/DocGenerator/Services/FileTrackingService.cs
+++ ATUAL: Services/DocGenerator/Services/FileTrackingService.cs
@@ -1,7 +1,7 @@
 using System;
 using System.Collections.Generic;
 using System.Data;
-using SqlConnection = Microsoft.Data.SqlClient.SqlConnection;
+using Microsoft.Data.SqlClient;
 using System.IO;
 using System.Linq;
 using System.Security.Cryptography;
@@ -42,10 +42,6 @@
         private const int DebounceDelayMs = 2000;
 
         private static readonly string[] MonitoredExtensions = { ".cs", ".cshtml", ".js", ".css" };
-
-        private bool _storedProceduresExist = true;
-        private int _storedProcedureErrorCount = 0;
-        private const int MaxStoredProcedureErrors = 3;
 
         private static readonly string[] MonitoredFolders =
         {
@@ -135,18 +131,6 @@
         {
             try
             {
-
-                if (!_storedProceduresExist)
-                {
-                    return new FileChangeDetectionResult
-                    {
-                        FilePath = filePath,
-                        ChangeType = FileChangeType.Error,
-                        NeedsUpdate = true,
-                        ErrorMessage = "DocGenerator stored procedures não disponíveis"
-                    };
-                }
-
                 if (!File.Exists(filePath))
                 {
                     return new FileChangeDetectionResult
@@ -194,23 +178,6 @@
                     NeedsUpdate = needsUpdate,
                     UpdateReason = updateReason,
                     DetectedAt = DateTime.Now
-                };
-            }
-            catch (Microsoft.Data.SqlClient.SqlException sqlEx) when (sqlEx.Number == 2812)
-            {
-
-                _storedProcedureErrorCount++;
-                if (_storedProcedureErrorCount >= MaxStoredProcedureErrors)
-                {
-                    _storedProceduresExist = false;
-                    _logger.LogWarning("DocGenerator: Stored procedures não encontradas. Serviço de tracking desabilitado. Execute o script DocGeneratorCacheSystem.sql para habilitar.");
-                }
-                return new FileChangeDetectionResult
-                {
-                    FilePath = filePath,
-                    ChangeType = FileChangeType.Error,
-                    NeedsUpdate = true,
-                    ErrorMessage = "Stored procedure não encontrada"
                 };
             }
             catch (Exception error)
```

### REMOVER do Janeiro

```csharp
using SqlConnection = Microsoft.Data.SqlClient.SqlConnection;
        private bool _storedProceduresExist = true;
        private int _storedProcedureErrorCount = 0;
        private const int MaxStoredProcedureErrors = 3;
                if (!_storedProceduresExist)
                {
                    return new FileChangeDetectionResult
                    {
                        FilePath = filePath,
                        ChangeType = FileChangeType.Error,
                        NeedsUpdate = true,
                        ErrorMessage = "DocGenerator stored procedures não disponíveis"
                    };
                }
                };
            }
            catch (Microsoft.Data.SqlClient.SqlException sqlEx) when (sqlEx.Number == 2812)
            {
                _storedProcedureErrorCount++;
                if (_storedProcedureErrorCount >= MaxStoredProcedureErrors)
                {
                    _storedProceduresExist = false;
                    _logger.LogWarning("DocGenerator: Stored procedures não encontradas. Serviço de tracking desabilitado. Execute o script DocGeneratorCacheSystem.sql para habilitar.");
                }
                return new FileChangeDetectionResult
                {
                    FilePath = filePath,
                    ChangeType = FileChangeType.Error,
                    NeedsUpdate = true,
                    ErrorMessage = "Stored procedure não encontrada"
```


### ADICIONAR ao Janeiro

```csharp
using Microsoft.Data.SqlClient;
```
