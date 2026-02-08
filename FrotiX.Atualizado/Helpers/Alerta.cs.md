# Helpers/Alerta.cs

**Mudanca:** GRANDE | **+36** linhas | **-27** linhas

---

```diff
--- JANEIRO: Helpers/Alerta.cs
+++ ATUAL: Helpers/Alerta.cs
@@ -1,6 +1,7 @@
 using FrotiX.Models;
 using Microsoft.AspNetCore.Http;
 using Microsoft.AspNetCore.Mvc.ViewFeatures;
+using Microsoft.Extensions.DependencyInjection;
 using Microsoft.Extensions.Logging;
 using System;
 using System.Diagnostics;
@@ -17,11 +18,18 @@
         {
             get; set;
         }
+
         public static ITempDataDictionaryFactory TempFactory
         {
             get; set;
         }
+
         public static ILoggerFactory LoggerFactory
+        {
+            get; set;
+        }
+
+        public static IServiceProvider ServiceProvider
         {
             get; set;
         }
@@ -75,50 +83,55 @@
                 ? funcao
                 : (info.member ?? "função desconhecida");
 
-            string linhaText = info.line.HasValue ? $" (linha {info.line.Value})" : string.Empty;
-            string msg =
-                $"{fileName}::{member}{linhaText}: {error.GetType().Name} - {error.Message}";
-
-            ILogger useLogger = logger;
-            if (useLogger == null)
-            {
-                try
-                {
-                    useLogger = LoggerFactory?.CreateLogger("Alerta");
-                }
-                catch (ObjectDisposedException)
-                {
-
-                    useLogger = null;
-                }
-            }
-
-            if (useLogger != null)
-                useLogger.LogError(error , msg);
-            else
-                Debug.WriteLine(msg);
-
+            int? lineNumber = info.line;
+            string msg = $"{fileName}::{member}: {error.GetType().Name} - {error.Message}";
+
+            bool loggedViaLogService = false;
             try
             {
 
-                var logService = HttpCtx?.HttpContext?.RequestServices.GetService(typeof(FrotiX.Services.ILogService)) as FrotiX.Services.ILogService;
+                var logService = ServiceProvider?.GetService(typeof(FrotiX.Services.ILogService)) as FrotiX.Services.ILogService;
+
+                if (logService == null && HttpCtx?.HttpContext?.RequestServices != null)
+                {
+                    logService = HttpCtx.HttpContext.RequestServices.GetService(typeof(FrotiX.Services.ILogService)) as FrotiX.Services.ILogService;
+                }
 
                 if (logService != null)
                 {
 
                     logService.Error(
-                        message: error.Message,
+                        message: $"[SERVER] {error.Message}",
                         exception: error,
                         arquivo: fileName,
                         metodo: member,
-                        linha: info.line
+                        linha: lineNumber
                     );
-                }
-            }
-            catch (Exception logEx)
-            {
-
-                Debug.WriteLine($"[Alerta] Falha ao registrar log: {logEx.Message}");
+                    loggedViaLogService = true;
+                }
+            }
+            catch
+            {
+
+            }
+
+            if (!loggedViaLogService)
+            {
+                string linhaText = lineNumber.HasValue ? $" (linha {lineNumber.Value})" : string.Empty;
+                string fullMsg = $"{fileName}::{member}{linhaText}: {error.GetType().Name} - {error.Message}";
+
+                var useLogger = logger ?? LoggerFactory?.CreateLogger("Alerta");
+                if (useLogger != null)
+                {
+                    useLogger.LogError(error, fullMsg);
+                }
+                else
+                {
+
+                    Debug.WriteLine($"[ALERTA-FALLBACK] {fullMsg}");
+                    Debug.WriteLine($"[ALERTA-FALLBACK] Stack: {error.StackTrace}");
+                    Console.Error.WriteLine($"[ALERTA-FALLBACK] {fullMsg}");
+                }
             }
 
             SetErrorUnexpectedAlert(fileName , member , error);
```

### REMOVER do Janeiro

```csharp
            string linhaText = info.line.HasValue ? $" (linha {info.line.Value})" : string.Empty;
            string msg =
                $"{fileName}::{member}{linhaText}: {error.GetType().Name} - {error.Message}";
            ILogger useLogger = logger;
            if (useLogger == null)
            {
                try
                {
                    useLogger = LoggerFactory?.CreateLogger("Alerta");
                }
                catch (ObjectDisposedException)
                {
                    useLogger = null;
                }
            }
            if (useLogger != null)
                useLogger.LogError(error , msg);
            else
                Debug.WriteLine(msg);
                var logService = HttpCtx?.HttpContext?.RequestServices.GetService(typeof(FrotiX.Services.ILogService)) as FrotiX.Services.ILogService;
                        message: error.Message,
                        linha: info.line
                }
            }
            catch (Exception logEx)
            {
                Debug.WriteLine($"[Alerta] Falha ao registrar log: {logEx.Message}");
```


### ADICIONAR ao Janeiro

```csharp
using Microsoft.Extensions.DependencyInjection;
        {
            get; set;
        }
        public static IServiceProvider ServiceProvider
            int? lineNumber = info.line;
            string msg = $"{fileName}::{member}: {error.GetType().Name} - {error.Message}";
            bool loggedViaLogService = false;
                var logService = ServiceProvider?.GetService(typeof(FrotiX.Services.ILogService)) as FrotiX.Services.ILogService;
                if (logService == null && HttpCtx?.HttpContext?.RequestServices != null)
                {
                    logService = HttpCtx.HttpContext.RequestServices.GetService(typeof(FrotiX.Services.ILogService)) as FrotiX.Services.ILogService;
                }
                        message: $"[SERVER] {error.Message}",
                        linha: lineNumber
                    loggedViaLogService = true;
                }
            }
            catch
            {
            }
            if (!loggedViaLogService)
            {
                string linhaText = lineNumber.HasValue ? $" (linha {lineNumber.Value})" : string.Empty;
                string fullMsg = $"{fileName}::{member}{linhaText}: {error.GetType().Name} - {error.Message}";
                var useLogger = logger ?? LoggerFactory?.CreateLogger("Alerta");
                if (useLogger != null)
                {
                    useLogger.LogError(error, fullMsg);
                }
                else
                {
                    Debug.WriteLine($"[ALERTA-FALLBACK] {fullMsg}");
                    Debug.WriteLine($"[ALERTA-FALLBACK] Stack: {error.StackTrace}");
                    Console.Error.WriteLine($"[ALERTA-FALLBACK] {fullMsg}");
                }
```
