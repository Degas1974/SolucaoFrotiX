# Middlewares/UiExceptionMiddleware.cs

**Mudanca:** GRANDE | **+60** linhas | **-0** linhas

---

```diff
--- JANEIRO: Middlewares/UiExceptionMiddleware.cs
+++ ATUAL: Middlewares/UiExceptionMiddleware.cs
@@ -1,8 +1,10 @@
 using System;
+using System.IO;
 using System.Linq;
 using System.Net.Mime;
 using System.Text.Json;
 using System.Threading.Tasks;
+using Microsoft.AspNetCore.Connections;
 using Microsoft.AspNetCore.Http;
 
 namespace FrotiX.Middlewares
@@ -14,10 +16,11 @@
 
         public UiExceptionMiddleware(RequestDelegate next)
         {
-
             ArgumentNullException.ThrowIfNull(next);
             _next = next;
         }
+
+        private const int StatusCodeClientClosedRequest = 499;
 
         public async Task Invoke(HttpContext http)
         {
@@ -25,6 +28,24 @@
             {
 
                 await _next(http);
+            }
+            catch (Exception ex) when (IsClientDisconnectException(ex) || http.RequestAborted.IsCancellationRequested)
+            {
+
+                if (!http.Response.HasStarted)
+                {
+                    http.Response.StatusCode = StatusCodeClientClosedRequest;
+                }
+                return;
+            }
+            catch (OperationCanceledException) when (http.RequestAborted.IsCancellationRequested)
+            {
+
+                if (!http.Response.HasStarted)
+                {
+                    http.Response.StatusCode = StatusCodeClientClosedRequest;
+                }
+                return;
             }
             catch (Exception ex)
             {
@@ -83,5 +104,54 @@
                 }
             }
         }
+
+        private static bool IsClientDisconnectException(Exception ex)
+        {
+            var current = ex;
+            while (current != null)
+            {
+
+                if (current is ConnectionResetException)
+                    return true;
+
+                if (current.GetType().Name == "ConnectionAbortedException")
+                    return true;
+
+                if (current is IOException ioEx)
+                {
+                    var message = ioEx.Message?.ToLowerInvariant() ?? "";
+                    if (message.Contains("connection reset") ||
+                        message.Contains("broken pipe") ||
+                        message.Contains("an existing connection was forcibly closed") ||
+                        message.Contains("the client has disconnected") ||
+                        message.Contains("connection was aborted"))
+                    {
+                        return true;
+                    }
+                }
+
+                if (current is System.Net.Sockets.SocketException socketEx)
+                {
+                    if (socketEx.SocketErrorCode == System.Net.Sockets.SocketError.ConnectionReset ||
+                        socketEx.SocketErrorCode == System.Net.Sockets.SocketError.ConnectionAborted ||
+                        socketEx.SocketErrorCode == System.Net.Sockets.SocketError.Shutdown)
+                    {
+                        return true;
+                    }
+                }
+
+                var msg = current.Message?.ToLowerInvariant() ?? "";
+                if (msg.Contains("the client has disconnected") ||
+                    msg.Contains("connection reset by peer") ||
+                    msg.Contains("an established connection was aborted"))
+                {
+                    return true;
+                }
+
+                current = current.InnerException;
+            }
+
+            return false;
+        }
     }
 }
```

### ADICIONAR ao Janeiro

```csharp
using System.IO;
using Microsoft.AspNetCore.Connections;
        private const int StatusCodeClientClosedRequest = 499;
            }
            catch (Exception ex) when (IsClientDisconnectException(ex) || http.RequestAborted.IsCancellationRequested)
            {
                if (!http.Response.HasStarted)
                {
                    http.Response.StatusCode = StatusCodeClientClosedRequest;
                }
                return;
            }
            catch (OperationCanceledException) when (http.RequestAborted.IsCancellationRequested)
            {
                if (!http.Response.HasStarted)
                {
                    http.Response.StatusCode = StatusCodeClientClosedRequest;
                }
                return;
        private static bool IsClientDisconnectException(Exception ex)
        {
            var current = ex;
            while (current != null)
            {
                if (current is ConnectionResetException)
                    return true;
                if (current.GetType().Name == "ConnectionAbortedException")
                    return true;
                if (current is IOException ioEx)
                {
                    var message = ioEx.Message?.ToLowerInvariant() ?? "";
                    if (message.Contains("connection reset") ||
                        message.Contains("broken pipe") ||
                        message.Contains("an existing connection was forcibly closed") ||
                        message.Contains("the client has disconnected") ||
                        message.Contains("connection was aborted"))
                    {
                        return true;
                    }
                }
                if (current is System.Net.Sockets.SocketException socketEx)
                {
                    if (socketEx.SocketErrorCode == System.Net.Sockets.SocketError.ConnectionReset ||
                        socketEx.SocketErrorCode == System.Net.Sockets.SocketError.ConnectionAborted ||
                        socketEx.SocketErrorCode == System.Net.Sockets.SocketError.Shutdown)
                    {
                        return true;
                    }
                }
                var msg = current.Message?.ToLowerInvariant() ?? "";
                if (msg.Contains("the client has disconnected") ||
                    msg.Contains("connection reset by peer") ||
                    msg.Contains("an established connection was aborted"))
                {
                    return true;
                }
                current = current.InnerException;
            }
            return false;
        }
```
