# Hubs/AlertasHub.cs

**Mudanca:** GRANDE | **+44** linhas | **-9** linhas

---

```diff
--- JANEIRO: Hubs/AlertasHub.cs
+++ ATUAL: Hubs/AlertasHub.cs
@@ -1,25 +1,60 @@
 using Microsoft.AspNetCore.SignalR;
 using System;
 using System.Threading.Tasks;
-using FrotiX.Helpers;
 
 namespace FrotiX.Hubs
 {
 
-    public class AlertasHub : Hub
+    public class AlertasHub :Hub
     {
 
-        public async Task MarcarComoLido(string alertaId, string usuarioId)
+        public async Task MarcarComoLido(string alertaId , string usuarioId)
         {
             try
             {
-
-                await Clients.User(usuarioId).SendAsync("AlertaMarcadoComoLido", alertaId);
+                await Clients.User(usuarioId).SendAsync("AlertaMarcadoComoLido" , alertaId);
             }
             catch (Exception ex)
             {
+                Console.WriteLine($"Erro em MarcarComoLido: {ex.Message}");
+            }
+        }
 
-                Alerta.TratamentoErroComLinha("AlertasHub.cs", "MarcarComoLido", ex);
+        public async Task EnviarAlertaLogErro(object alertaPayload)
+        {
+            try
+            {
+                await Clients.Group("admin_logs").SendAsync("AlertaLogErro", alertaPayload);
+            }
+            catch (Exception ex)
+            {
+                Console.WriteLine($"Erro em EnviarAlertaLogErro: {ex.Message}");
+            }
+        }
+
+        public async Task InscreverAlertasLog()
+        {
+            try
+            {
+                await Groups.AddToGroupAsync(Context.ConnectionId, "admin_logs");
+                Console.WriteLine($"Usuário {Context.UserIdentifier} inscrito em alertas de logs");
+            }
+            catch (Exception ex)
+            {
+                Console.WriteLine($"Erro em InscreverAlertasLog: {ex.Message}");
+            }
+        }
+
+        public async Task DesinscreverAlertasLog()
+        {
+            try
+            {
+                await Groups.RemoveFromGroupAsync(Context.ConnectionId, "admin_logs");
+                Console.WriteLine($"Usuário {Context.UserIdentifier} desinscrito de alertas de logs");
+            }
+            catch (Exception ex)
+            {
+                Console.WriteLine($"Erro em DesinscreverAlertasLog: {ex.Message}");
             }
         }
 
@@ -27,7 +62,6 @@
         {
             try
             {
-
                 Console.WriteLine("=== CLIENTE CONECTANDO ===");
 
                 var usuarioId = Context.UserIdentifier;
@@ -43,11 +77,10 @@
                 {
                     var groupName = $"user_{usuarioId}";
                     Console.WriteLine($"Adicionando ao grupo: {groupName}");
-                    await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
+                    await Groups.AddToGroupAsync(Context.ConnectionId , groupName);
                 }
                 else
                 {
-
                     Console.WriteLine("AVISO: UserIdentifier está nulo!");
                 }
 
@@ -56,8 +89,8 @@
             }
             catch (Exception ex)
             {
-
-                Alerta.TratamentoErroComLinha("AlertasHub.cs", "OnConnectedAsync", ex);
+                Console.WriteLine($"❌ ERRO em OnConnectedAsync: {ex.Message}");
+                Console.WriteLine($"Stack: {ex.StackTrace}");
                 throw;
             }
         }
@@ -66,12 +99,10 @@
         {
             try
             {
-
                 Console.WriteLine("=== CLIENTE DESCONECTANDO ===");
 
                 if (exception != null)
                 {
-
                     Console.WriteLine($"Razão: {exception.Message}");
                 }
 
@@ -79,15 +110,14 @@
 
                 if (!string.IsNullOrEmpty(usuarioId))
                 {
-                    await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"user_{usuarioId}");
+                    await Groups.RemoveFromGroupAsync(Context.ConnectionId , $"user_{usuarioId}");
                 }
 
                 await base.OnDisconnectedAsync(exception);
             }
             catch (Exception ex)
             {
-
-                Alerta.TratamentoErroComLinha("AlertasHub.cs", "OnDisconnectedAsync", ex);
+                Console.WriteLine($"❌ ERRO em OnDisconnectedAsync: {ex.Message}");
             }
         }
     }
```

### REMOVER do Janeiro

```csharp
using FrotiX.Helpers;
    public class AlertasHub : Hub
        public async Task MarcarComoLido(string alertaId, string usuarioId)
                await Clients.User(usuarioId).SendAsync("AlertaMarcadoComoLido", alertaId);
                Alerta.TratamentoErroComLinha("AlertasHub.cs", "MarcarComoLido", ex);
                    await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
                Alerta.TratamentoErroComLinha("AlertasHub.cs", "OnConnectedAsync", ex);
                    await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"user_{usuarioId}");
                Alerta.TratamentoErroComLinha("AlertasHub.cs", "OnDisconnectedAsync", ex);
```


### ADICIONAR ao Janeiro

```csharp
    public class AlertasHub :Hub
        public async Task MarcarComoLido(string alertaId , string usuarioId)
                await Clients.User(usuarioId).SendAsync("AlertaMarcadoComoLido" , alertaId);
                Console.WriteLine($"Erro em MarcarComoLido: {ex.Message}");
            }
        }
        public async Task EnviarAlertaLogErro(object alertaPayload)
        {
            try
            {
                await Clients.Group("admin_logs").SendAsync("AlertaLogErro", alertaPayload);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro em EnviarAlertaLogErro: {ex.Message}");
            }
        }
        public async Task InscreverAlertasLog()
        {
            try
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, "admin_logs");
                Console.WriteLine($"Usuário {Context.UserIdentifier} inscrito em alertas de logs");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro em InscreverAlertasLog: {ex.Message}");
            }
        }
        public async Task DesinscreverAlertasLog()
        {
            try
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, "admin_logs");
                Console.WriteLine($"Usuário {Context.UserIdentifier} desinscrito de alertas de logs");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro em DesinscreverAlertasLog: {ex.Message}");
                    await Groups.AddToGroupAsync(Context.ConnectionId , groupName);
                Console.WriteLine($"❌ ERRO em OnConnectedAsync: {ex.Message}");
                Console.WriteLine($"Stack: {ex.StackTrace}");
                    await Groups.RemoveFromGroupAsync(Context.ConnectionId , $"user_{usuarioId}");
                Console.WriteLine($"❌ ERRO em OnDisconnectedAsync: {ex.Message}");
```
