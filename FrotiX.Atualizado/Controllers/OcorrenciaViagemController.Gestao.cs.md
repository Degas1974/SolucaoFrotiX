# Controllers/OcorrenciaViagemController.Gestao.cs

**Mudanca:** GRANDE | **+19** linhas | **-29** linhas

---

```diff
--- JANEIRO: Controllers/OcorrenciaViagemController.Gestao.cs
+++ ATUAL: Controllers/OcorrenciaViagemController.Gestao.cs
@@ -1,8 +1,6 @@
 using FrotiX.Models;
 using FrotiX.Repository.IRepository;
-using FrotiX.Services;
 using FrotiX.TextNormalization;
-using FrotiX.Helpers;
 using Microsoft.AspNetCore.Mvc;
 using System;
 using System.Collections.Generic;
@@ -28,7 +26,6 @@
         {
             try
             {
-
                 Guid? veiculoGuid = null, motoristaGuid = null;
                 if (!string.IsNullOrWhiteSpace(veiculoId) && Guid.TryParse(veiculoId , out var vg))
                     veiculoGuid = vg;
@@ -120,13 +117,11 @@
 
                 if (dataUnica.HasValue)
                 {
-
                     var dia = dataUnica.Value.Date;
                     ocorrenciasQuery = ocorrenciasQuery.Where(x => x.DataCriacao.Date == dia);
                 }
                 else if (dtIni.HasValue && dtFim.HasValue)
                 {
-
                     var ini = dtIni.Value.Date;
                     var fim = dtFim.Value.Date;
                     ocorrenciasQuery = ocorrenciasQuery.Where(x => x.DataCriacao.Date >= ini && x.DataCriacao.Date <= fim);
@@ -138,7 +133,6 @@
 
                 if (!ocorrenciasFiltradas.Any())
                 {
-
                     return new JsonResult(new { data = new List<object>() });
                 }
 
@@ -220,10 +214,9 @@
 
                 return new JsonResult(new { data = result });
             }
-            catch (Exception ex)
-            {
-                _log.Error("OcorrenciaViagemController.ListarGestao", ex);
-                Alerta.TratamentoErroComLinha("OcorrenciaViagemController.Gestao.cs" , "ListarGestao" , ex);
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha("OcorrenciaViagemController.cs" , "ListarGestao" , error);
                 return new JsonResult(new { data = new List<object>() });
             }
         }
@@ -289,22 +282,19 @@
                 _unitOfWork.OcorrenciaViagem.Update(ocorrencia);
                 _unitOfWork.Save();
 
-                _log.Info($"OcorrenciaViagemController.EditarOcorrencia: Ocorrência {dto.OcorrenciaViagemId} atualizada.");
-
                 return new JsonResult(new
                 {
                     success = true ,
                     message = "Ocorrência atualizada com sucesso"
                 });
             }
-            catch (Exception ex)
-            {
-                _log.Error("OcorrenciaViagemController.EditarOcorrencia", ex);
-                Alerta.TratamentoErroComLinha("OcorrenciaViagemController.Gestao.cs" , "EditarOcorrencia" , ex);
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha("OcorrenciaViagemController.cs" , "EditarOcorrencia" , error);
                 return new JsonResult(new
                 {
                     success = false ,
-                    message = "Erro ao editar ocorrência: " + ex.Message
+                    message = "Erro ao editar ocorrência: " + error.Message
                 });
             }
         }
@@ -354,22 +344,19 @@
                 _unitOfWork.OcorrenciaViagem.Update(ocorrencia);
                 _unitOfWork.Save();
 
-                _log.Info($"OcorrenciaViagemController.BaixarOcorrenciaGestao: Ocorrência {dto.OcorrenciaViagemId} baixada via gestão.");
-
                 return new JsonResult(new
                 {
                     success = true ,
                     message = "Ocorrência baixada com sucesso"
                 });
             }
-            catch (Exception ex)
-            {
-                _log.Error("OcorrenciaViagemController.BaixarOcorrenciaGestao", ex);
-                Alerta.TratamentoErroComLinha("OcorrenciaViagemController.Gestao.cs" , "BaixarOcorrenciaGestao" , ex);
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha("OcorrenciaViagemController.cs" , "BaixarOcorrenciaGestao" , error);
                 return new JsonResult(new
                 {
                     success = false ,
-                    message = "Erro ao baixar ocorrência: " + ex.Message
+                    message = "Erro ao baixar ocorrência: " + error.Message
                 });
             }
         }
@@ -424,22 +411,19 @@
                 _unitOfWork.OcorrenciaViagem.Update(ocorrencia);
                 _unitOfWork.Save();
 
-                _log.Info($"OcorrenciaViagemController.BaixarOcorrenciaComSolucao: Ocorrência {dto.OcorrenciaViagemId} baixada com solução rápida.");
-
                 return new JsonResult(new
                 {
                     success = true ,
                     message = "Ocorrência baixada com sucesso"
                 });
             }
-            catch (Exception ex)
-            {
-                _log.Error("OcorrenciaViagemController.BaixarOcorrenciaComSolucao", ex);
-                Alerta.TratamentoErroComLinha("OcorrenciaViagemController.Gestao.cs" , "BaixarOcorrenciaComSolucao" , ex);
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha("OcorrenciaViagemController.cs" , "BaixarOcorrenciaComSolucao" , error);
                 return new JsonResult(new
                 {
                     success = false ,
-                    message = "Erro ao baixar ocorrência: " + ex.Message
+                    message = "Erro ao baixar ocorrência: " + error.Message
                 });
             }
         }
@@ -467,14 +451,13 @@
                     baixadas = baixadas
                 });
             }
-            catch (Exception ex)
-            {
-                _log.Error("OcorrenciaViagemController.ContarOcorrencias", ex);
-                Alerta.TratamentoErroComLinha("OcorrenciaViagemController.Gestao.cs" , "ContarOcorrencias" , ex);
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha("OcorrenciaViagemController.cs" , "ContarOcorrencias" , error);
                 return new JsonResult(new
                 {
                     success = false ,
-                    message = ex.Message
+                    message = error.Message
                 });
             }
         }
```

### REMOVER do Janeiro

```csharp
using FrotiX.Services;
using FrotiX.Helpers;
            catch (Exception ex)
            {
                _log.Error("OcorrenciaViagemController.ListarGestao", ex);
                Alerta.TratamentoErroComLinha("OcorrenciaViagemController.Gestao.cs" , "ListarGestao" , ex);
                _log.Info($"OcorrenciaViagemController.EditarOcorrencia: Ocorrência {dto.OcorrenciaViagemId} atualizada.");
            catch (Exception ex)
            {
                _log.Error("OcorrenciaViagemController.EditarOcorrencia", ex);
                Alerta.TratamentoErroComLinha("OcorrenciaViagemController.Gestao.cs" , "EditarOcorrencia" , ex);
                    message = "Erro ao editar ocorrência: " + ex.Message
                _log.Info($"OcorrenciaViagemController.BaixarOcorrenciaGestao: Ocorrência {dto.OcorrenciaViagemId} baixada via gestão.");
            catch (Exception ex)
            {
                _log.Error("OcorrenciaViagemController.BaixarOcorrenciaGestao", ex);
                Alerta.TratamentoErroComLinha("OcorrenciaViagemController.Gestao.cs" , "BaixarOcorrenciaGestao" , ex);
                    message = "Erro ao baixar ocorrência: " + ex.Message
                _log.Info($"OcorrenciaViagemController.BaixarOcorrenciaComSolucao: Ocorrência {dto.OcorrenciaViagemId} baixada com solução rápida.");
            catch (Exception ex)
            {
                _log.Error("OcorrenciaViagemController.BaixarOcorrenciaComSolucao", ex);
                Alerta.TratamentoErroComLinha("OcorrenciaViagemController.Gestao.cs" , "BaixarOcorrenciaComSolucao" , ex);
                    message = "Erro ao baixar ocorrência: " + ex.Message
            catch (Exception ex)
            {
                _log.Error("OcorrenciaViagemController.ContarOcorrencias", ex);
                Alerta.TratamentoErroComLinha("OcorrenciaViagemController.Gestao.cs" , "ContarOcorrencias" , ex);
                    message = ex.Message
```


### ADICIONAR ao Janeiro

```csharp
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("OcorrenciaViagemController.cs" , "ListarGestao" , error);
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("OcorrenciaViagemController.cs" , "EditarOcorrencia" , error);
                    message = "Erro ao editar ocorrência: " + error.Message
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("OcorrenciaViagemController.cs" , "BaixarOcorrenciaGestao" , error);
                    message = "Erro ao baixar ocorrência: " + error.Message
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("OcorrenciaViagemController.cs" , "BaixarOcorrenciaComSolucao" , error);
                    message = "Erro ao baixar ocorrência: " + error.Message
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("OcorrenciaViagemController.cs" , "ContarOcorrencias" , error);
                    message = error.Message
```
