# Controllers/ViagemController.AtualizarDados.cs

**Mudanca:** MEDIA | **+4** linhas | **-3** linhas

---

```diff
--- JANEIRO: Controllers/ViagemController.AtualizarDados.cs
+++ ATUAL: Controllers/ViagemController.AtualizarDados.cs
@@ -16,7 +16,6 @@
         {
             try
             {
-
                 var viagem = _unitOfWork.Viagem.GetFirstOrDefault(v => v.ViagemId == id);
 
                 if (viagem == null)
@@ -53,8 +52,7 @@
             }
             catch (Exception error)
             {
-
-                Alerta.TratamentoErroComLinha("ViagemController.AtualizarDados.cs", "GetViagem", error);
+                Alerta.TratamentoErroComLinha("ViagemController.cs", "GetViagem", error);
                 return Json(new
                 {
                     success = false,
@@ -69,7 +67,6 @@
         {
             try
             {
-
                 if (request == null || request.ViagemId == Guid.Empty)
                 {
                     return Json(new
@@ -175,15 +172,15 @@
             }
             catch (Exception error)
             {
-
-                Alerta.TratamentoErroComLinha("ViagemController.AtualizarDados.cs", "AtualizarDadosViagem", error);
+                Alerta.TratamentoErroComLinha("ViagemController.cs", "AtualizarDadosViagem", error);
                 return Json(new
                 {
                     success = false,
                     message = "Erro ao atualizar viagem: " + error.Message
                 });
             }
-        } }
+        }
+    }
 
     public class AtualizarDadosViagemRequest
     {
```

### REMOVER do Janeiro

```csharp
                Alerta.TratamentoErroComLinha("ViagemController.AtualizarDados.cs", "GetViagem", error);
                Alerta.TratamentoErroComLinha("ViagemController.AtualizarDados.cs", "AtualizarDadosViagem", error);
        } }
```


### ADICIONAR ao Janeiro

```csharp
                Alerta.TratamentoErroComLinha("ViagemController.cs", "GetViagem", error);
                Alerta.TratamentoErroComLinha("ViagemController.cs", "AtualizarDadosViagem", error);
        }
    }
```
