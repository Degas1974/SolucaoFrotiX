# Controllers/OcorrenciaViagemController.Debug.cs

**Mudanca:** MEDIA | **+10** linhas | **-16** linhas

---

```diff
--- JANEIRO: Controllers/OcorrenciaViagemController.Debug.cs
+++ ATUAL: Controllers/OcorrenciaViagemController.Debug.cs
@@ -1,6 +1,5 @@
 using FrotiX.Models;
 using FrotiX.Repository.IRepository;
-using FrotiX.Services;
 using Microsoft.AspNetCore.Mvc;
 using System;
 using System.Collections.Generic;
@@ -55,15 +54,13 @@
 
                 return new JsonResult(resultado);
             }
-            catch (Exception ex)
+            catch (Exception error)
             {
-                _log.Error("OcorrenciaViagemController.DebugListar", ex);
-                Alerta.TratamentoErroComLinha("OcorrenciaViagemController.Debug.cs", "DebugListar", ex);
                 return new JsonResult(new
                 {
                     success = false,
-                    message = ex.Message,
-                    stackTrace = ex.StackTrace
+                    message = error.Message,
+                    stackTrace = error.StackTrace
                 });
             }
         }
@@ -74,7 +71,6 @@
         {
             try
             {
-
                 var todos = _unitOfWork.OcorrenciaViagem.GetAll().ToList();
 
                 var porStatusString = todos.Where(x => x.Status == "Aberta").Count();
@@ -104,15 +100,13 @@
                     valoresUnicosStatusBool = todos.Select(x => x.StatusOcorrencia).Distinct().ToList()
                 });
             }
-            catch (Exception ex)
+            catch (Exception error)
             {
-                _log.Error("OcorrenciaViagemController.DebugAbertas", ex);
-                Alerta.TratamentoErroComLinha("OcorrenciaViagemController.Debug.cs", "DebugAbertas", ex);
                 return new JsonResult(new
                 {
                     success = false,
-                    message = ex.Message,
-                    stackTrace = ex.StackTrace
+                    message = error.Message,
+                    stackTrace = error.StackTrace
                 });
             }
         }
@@ -123,7 +117,6 @@
         {
             try
             {
-
                 var ocorrencias = _unitOfWork.OcorrenciaViagem
                     .GetAll()
                     .OrderByDescending(x => x.DataCriacao)
@@ -132,7 +125,6 @@
 
                 if (!ocorrencias.Any())
                 {
-
                     return new JsonResult(new { data = new List<object>(), mensagem = "Nenhum registro encontrado na tabela OcorrenciaViagem" });
                 }
 
@@ -184,15 +176,14 @@
 
                 return new JsonResult(new { data = result });
             }
-            catch (Exception ex)
+            catch (Exception error)
             {
-                _log.Error("OcorrenciaViagemController.DebugListarTodos", ex);
-                Alerta.TratamentoErroComLinha("OcorrenciaViagemController.Debug.cs", "DebugListarTodos", ex);
+                Alerta.TratamentoErroComLinha("OcorrenciaViagemController.cs", "DebugListarTodos", error);
                 return new JsonResult(new
                 {
                     data = new List<object>(),
-                    erro = ex.Message,
-                    stackTrace = ex.StackTrace
+                    erro = error.Message,
+                    stackTrace = error.StackTrace
                 });
             }
         }
```

### REMOVER do Janeiro

```csharp
using FrotiX.Services;
            catch (Exception ex)
                _log.Error("OcorrenciaViagemController.DebugListar", ex);
                Alerta.TratamentoErroComLinha("OcorrenciaViagemController.Debug.cs", "DebugListar", ex);
                    message = ex.Message,
                    stackTrace = ex.StackTrace
            catch (Exception ex)
                _log.Error("OcorrenciaViagemController.DebugAbertas", ex);
                Alerta.TratamentoErroComLinha("OcorrenciaViagemController.Debug.cs", "DebugAbertas", ex);
                    message = ex.Message,
                    stackTrace = ex.StackTrace
            catch (Exception ex)
                _log.Error("OcorrenciaViagemController.DebugListarTodos", ex);
                Alerta.TratamentoErroComLinha("OcorrenciaViagemController.Debug.cs", "DebugListarTodos", ex);
                    erro = ex.Message,
                    stackTrace = ex.StackTrace
```


### ADICIONAR ao Janeiro

```csharp
            catch (Exception error)
                    message = error.Message,
                    stackTrace = error.StackTrace
            catch (Exception error)
                    message = error.Message,
                    stackTrace = error.StackTrace
            catch (Exception error)
                Alerta.TratamentoErroComLinha("OcorrenciaViagemController.cs", "DebugListarTodos", error);
                    erro = error.Message,
                    stackTrace = error.StackTrace
```
