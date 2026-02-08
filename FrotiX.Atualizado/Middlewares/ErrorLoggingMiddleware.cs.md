# Middlewares/ErrorLoggingMiddleware.cs

**Mudanca:** GRANDE | **+80** linhas | **-2** linhas

---

```diff
--- JANEIRO: Middlewares/ErrorLoggingMiddleware.cs
+++ ATUAL: Middlewares/ErrorLoggingMiddleware.cs
@@ -1,8 +1,12 @@
 using System;
+using System.IO;
+using System.Net;
 using System.Text.RegularExpressions;
+using System.Threading;
 using System.Threading.Tasks;
 using FrotiX.Services;
 using Microsoft.AspNetCore.Builder;
+using Microsoft.AspNetCore.Connections;
 using Microsoft.AspNetCore.Http;
 using Microsoft.Extensions.Logging;
 
@@ -19,6 +23,8 @@
         _logger = logger;
     }
 
+    private const int StatusCodeClientClosedRequest = 499;
+
     public async Task InvokeAsync(HttpContext context, ILogService logService)
     {
         try
@@ -26,8 +32,9 @@
 
             await _next(context);
 
-            if (context.Response.StatusCode >= 400)
-            {
+            if (context.Response.StatusCode >= 400 && context.Response.StatusCode != StatusCodeClientClosedRequest)
+            {
+
                 var statusCode = context.Response.StatusCode;
                 var path = context.Request.Path.Value ?? "";
                 var method = context.Request.Method;
@@ -35,6 +42,44 @@
 
                 logService.HttpError(statusCode, path, method, message);
             }
+        }
+        catch (Exception ex) when (IsClientDisconnectException(ex) || context.RequestAborted.IsCancellationRequested)
+        {
+
+            _logger.LogDebug(
+                "Cliente desconectou durante requisição: {Method} {Path}",
+                context.Request.Method,
+                context.Request.Path.Value ?? "/"
+            );
+
+            if (!context.Response.HasStarted)
+            {
+                context.Response.StatusCode = StatusCodeClientClosedRequest;
+            }
+
+            logService.Info(
+                $"Cliente desconectou: {context.Request.Method} {context.Request.Path.Value ?? "/"}",
+                "ErrorLoggingMiddleware",
+                "InvokeAsync"
+            );
+
+            return;
+        }
+        catch (OperationCanceledException) when (context.RequestAborted.IsCancellationRequested)
+        {
+
+            _logger.LogDebug(
+                "Requisição cancelada pelo cliente: {Method} {Path}",
+                context.Request.Method,
+                context.Request.Path.Value ?? "/"
+            );
+
+            if (!context.Response.HasStarted)
+            {
+                context.Response.StatusCode = StatusCodeClientClosedRequest;
+            }
+
+            return;
         }
         catch (Exception ex)
         {
@@ -73,11 +118,61 @@
         }
     }
 
+    private static bool IsClientDisconnectException(Exception ex)
+    {
+
+        var current = ex;
+        while (current != null)
+        {
+
+            if (current is ConnectionResetException)
+                return true;
+
+            if (current.GetType().Name == "ConnectionAbortedException")
+                return true;
+
+            if (current is IOException ioEx)
+            {
+                var message = ioEx.Message?.ToLowerInvariant() ?? "";
+                if (message.Contains("connection reset") ||
+                    message.Contains("broken pipe") ||
+                    message.Contains("an existing connection was forcibly closed") ||
+                    message.Contains("the client has disconnected") ||
+                    message.Contains("connection was aborted"))
+                {
+                    return true;
+                }
+            }
+
+            if (current is System.Net.Sockets.SocketException socketEx)
+            {
+
+                if (socketEx.SocketErrorCode == System.Net.Sockets.SocketError.ConnectionReset ||
+                    socketEx.SocketErrorCode == System.Net.Sockets.SocketError.ConnectionAborted ||
+                    socketEx.SocketErrorCode == System.Net.Sockets.SocketError.Shutdown)
+                {
+                    return true;
+                }
+            }
+
+            var msg = current.Message?.ToLowerInvariant() ?? "";
+            if (msg.Contains("the client has disconnected") ||
+                msg.Contains("connection reset by peer") ||
+                msg.Contains("an established connection was aborted"))
+            {
+                return true;
+            }
+
+            current = current.InnerException;
+        }
+
+        return false;
+    }
+
     private static string GetStatusMessage(int statusCode)
     {
         return statusCode switch
         {
-
             400 => "Bad Request - Requisição inválida",
             401 => "Unauthorized - Não autorizado",
             403 => "Forbidden - Acesso negado",
@@ -88,13 +183,12 @@
             415 => "Unsupported Media Type - Tipo de mídia não suportado",
             422 => "Unprocessable Entity - Entidade não processável",
             429 => "Too Many Requests - Muitas requisições",
-
+            499 => "Client Closed Request - Cliente desconectou",
             500 => "Internal Server Error - Erro interno do servidor",
             501 => "Not Implemented - Não implementado",
             502 => "Bad Gateway - Gateway inválido",
             503 => "Service Unavailable - Serviço indisponível",
             504 => "Gateway Timeout - Timeout do gateway",
-
             _ => $"HTTP Error {statusCode}"
         };
     }
@@ -105,7 +199,6 @@
 
     public static IApplicationBuilder UseErrorLogging(this IApplicationBuilder builder)
     {
-
         return builder.UseMiddleware<ErrorLoggingMiddleware>();
     }
 }
```
