# Controllers/OcorrenciaViagemController.Listar.cs

**Mudanca:** GRANDE | **+35** linhas | **-41** linhas

---

```diff
--- JANEIRO: Controllers/OcorrenciaViagemController.Listar.cs
+++ ATUAL: Controllers/OcorrenciaViagemController.Listar.cs
@@ -1,6 +1,5 @@
 using FrotiX.Models;
 using FrotiX.Repository.IRepository;
-using FrotiX.Services;
 using Microsoft.AspNetCore.Mvc;
 using System;
 using System.Linq;
@@ -17,7 +16,6 @@
         {
             try
             {
-
                 if (viagemId == Guid.Empty)
                 {
                     return new JsonResult(new
@@ -50,14 +48,13 @@
                     total = ocorrencias.Count
                 });
             }
-            catch (Exception ex)
-            {
-                _log.Error("OcorrenciaViagemController.ListarOcorrenciasModal", ex);
-                Alerta.TratamentoErroComLinha("OcorrenciaViagemController.Listar.cs", "ListarOcorrenciasModal", ex);
-                return new JsonResult(new
-                {
-                    success = false,
-                    message = "Erro ao listar ocorrências: " + ex.Message
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha("OcorrenciaViagemController.cs", "ListarOcorrenciasModal", error);
+                return new JsonResult(new
+                {
+                    success = false,
+                    message = "Erro ao listar ocorrências: " + error.Message
                 });
             }
         }
@@ -68,7 +65,6 @@
         {
             try
             {
-
                 if (veiculoId == Guid.Empty)
                 {
                     return new JsonResult(new
@@ -105,14 +101,13 @@
                     temOcorrencias = ocorrencias.Count > 0
                 });
             }
-            catch (Exception ex)
-            {
-                _log.Error("OcorrenciaViagemController.ListarOcorrenciasVeiculo", ex);
-                Alerta.TratamentoErroComLinha("OcorrenciaViagemController.Listar.cs", "ListarOcorrenciasVeiculo", ex);
-                return new JsonResult(new
-                {
-                    success = false,
-                    message = "Erro ao listar ocorrências: " + ex.Message
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha("OcorrenciaViagemController.cs", "ListarOcorrenciasVeiculo", error);
+                return new JsonResult(new
+                {
+                    success = false,
+                    message = "Erro ao listar ocorrências: " + error.Message
                 });
             }
         }
@@ -123,7 +118,6 @@
         {
             try
             {
-
                 if (veiculoId == Guid.Empty)
                 {
                     return new JsonResult(new
@@ -145,14 +139,13 @@
                     temOcorrencias = quantidade > 0
                 });
             }
-            catch (Exception ex)
-            {
-                _log.Error("OcorrenciaViagemController.VerificarOcorrenciasVeiculo", ex);
-                Alerta.TratamentoErroComLinha("OcorrenciaViagemController.Listar.cs", "VerificarOcorrenciasVeiculo", ex);
-                return new JsonResult(new
-                {
-                    success = false,
-                    message = "Erro ao verificar ocorrências: " + ex.Message
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha("OcorrenciaViagemController.cs", "VerificarOcorrenciasVeiculo", error);
+                return new JsonResult(new
+                {
+                    success = false,
+                    message = "Erro ao verificar ocorrências: " + error.Message
                 });
             }
         }
@@ -163,7 +156,6 @@
         {
             try
             {
-
                 if (dto == null || dto.OcorrenciaViagemId == Guid.Empty)
                 {
                     return new JsonResult(new
@@ -178,7 +170,6 @@
 
                 if (ocorrencia == null)
                 {
-
                     return new JsonResult(new
                     {
                         success = false,
@@ -189,29 +180,26 @@
                 _unitOfWork.OcorrenciaViagem.Remove(ocorrencia);
                 _unitOfWork.Save();
 
-                _log.Info($"OcorrenciaViagemController.ExcluirOcorrencia: Ocorrência {dto.OcorrenciaViagemId} removida.");
-
                 return new JsonResult(new
                 {
                     success = true,
                     message = "Ocorrência excluída com sucesso"
                 });
             }
-            catch (Exception ex)
-            {
-                _log.Error("OcorrenciaViagemController.ExcluirOcorrencia", ex);
-                Alerta.TratamentoErroComLinha("OcorrenciaViagemController.Listar.cs", "ExcluirOcorrencia", ex);
-                return new JsonResult(new
-                {
-                    success = false,
-                    message = "Erro ao excluir ocorrência: " + ex.Message
-                });
-            }
-        }
-
-        public class ExcluirOcorrenciaDTO
-        {
-            public Guid OcorrenciaViagemId { get; set; }
-        }
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha("OcorrenciaViagemController.cs", "ExcluirOcorrencia", error);
+                return new JsonResult(new
+                {
+                    success = false,
+                    message = "Erro ao excluir ocorrência: " + error.Message
+                });
+            }
+        }
+    }
+
+    public class ExcluirOcorrenciaDTO
+    {
+        public Guid OcorrenciaViagemId { get; set; }
     }
 }
```

### REMOVER do Janeiro

```csharp
using FrotiX.Services;
            catch (Exception ex)
            {
                _log.Error("OcorrenciaViagemController.ListarOcorrenciasModal", ex);
                Alerta.TratamentoErroComLinha("OcorrenciaViagemController.Listar.cs", "ListarOcorrenciasModal", ex);
                return new JsonResult(new
                {
                    success = false,
                    message = "Erro ao listar ocorrências: " + ex.Message
            catch (Exception ex)
            {
                _log.Error("OcorrenciaViagemController.ListarOcorrenciasVeiculo", ex);
                Alerta.TratamentoErroComLinha("OcorrenciaViagemController.Listar.cs", "ListarOcorrenciasVeiculo", ex);
                return new JsonResult(new
                {
                    success = false,
                    message = "Erro ao listar ocorrências: " + ex.Message
            catch (Exception ex)
            {
                _log.Error("OcorrenciaViagemController.VerificarOcorrenciasVeiculo", ex);
                Alerta.TratamentoErroComLinha("OcorrenciaViagemController.Listar.cs", "VerificarOcorrenciasVeiculo", ex);
                return new JsonResult(new
                {
                    success = false,
                    message = "Erro ao verificar ocorrências: " + ex.Message
                _log.Info($"OcorrenciaViagemController.ExcluirOcorrencia: Ocorrência {dto.OcorrenciaViagemId} removida.");
            catch (Exception ex)
            {
                _log.Error("OcorrenciaViagemController.ExcluirOcorrencia", ex);
                Alerta.TratamentoErroComLinha("OcorrenciaViagemController.Listar.cs", "ExcluirOcorrencia", ex);
                return new JsonResult(new
                {
                    success = false,
                    message = "Erro ao excluir ocorrência: " + ex.Message
                });
            }
        }
        public class ExcluirOcorrenciaDTO
        {
            public Guid OcorrenciaViagemId { get; set; }
        }
```


### ADICIONAR ao Janeiro

```csharp
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("OcorrenciaViagemController.cs", "ListarOcorrenciasModal", error);
                return new JsonResult(new
                {
                    success = false,
                    message = "Erro ao listar ocorrências: " + error.Message
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("OcorrenciaViagemController.cs", "ListarOcorrenciasVeiculo", error);
                return new JsonResult(new
                {
                    success = false,
                    message = "Erro ao listar ocorrências: " + error.Message
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("OcorrenciaViagemController.cs", "VerificarOcorrenciasVeiculo", error);
                return new JsonResult(new
                {
                    success = false,
                    message = "Erro ao verificar ocorrências: " + error.Message
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("OcorrenciaViagemController.cs", "ExcluirOcorrencia", error);
                return new JsonResult(new
                {
                    success = false,
                    message = "Erro ao excluir ocorrência: " + error.Message
                });
            }
        }
    }
    public class ExcluirOcorrenciaDTO
    {
        public Guid OcorrenciaViagemId { get; set; }
```
