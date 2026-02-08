# Controllers/OcorrenciaViagemController.cs

**Mudanca:** GRANDE | **+12** linhas | **-50** linhas

---

```diff
--- JANEIRO: Controllers/OcorrenciaViagemController.cs
+++ ATUAL: Controllers/OcorrenciaViagemController.cs
@@ -7,8 +7,6 @@
 using Microsoft.AspNetCore.Mvc;
 using FrotiX.Repository.IRepository;
 using FrotiX.Models;
-using FrotiX.Helpers;
-using FrotiX.Services;
 
 namespace FrotiX.Controllers
 {
@@ -18,19 +16,10 @@
     public partial class OcorrenciaViagemController : ControllerBase
     {
         private readonly IUnitOfWork _unitOfWork;
-        private readonly ILogService _log;
-
-        public OcorrenciaViagemController(IUnitOfWork unitOfWork, ILogService log)
-        {
-            try
-            {
-                _unitOfWork = unitOfWork;
-                _log = log;
-            }
-            catch (Exception ex)
-            {
-                Alerta.TratamentoErroComLinha("OcorrenciaViagemController.cs", "Constructor", ex);
-            }
+
+        public OcorrenciaViagemController(IUnitOfWork unitOfWork)
+        {
+            _unitOfWork = unitOfWork;
         }
 
         [HttpGet]
@@ -39,7 +28,6 @@
         {
             try
             {
-
                 var ocorrencias = _unitOfWork.ViewOcorrenciasViagem
                     .GetAll(o => o.ViagemId == viagemId)
                     .OrderByDescending(o => o.DataCriacao)
@@ -68,11 +56,9 @@
 
                 return Ok(new { success = true , data = ocorrencias });
             }
-            catch (Exception error)
-            {
-                _log.Error(error.Message, error, "OcorrenciaViagemController.cs", "ListarPorViagem");
-                Alerta.TratamentoErroComLinha("OcorrenciaViagemController.cs", "ListarPorViagem", error);
-                return Ok(new { success = false , message = "Erro ao listar ocorrências: " + error.Message });
+            catch (Exception ex)
+            {
+                return Ok(new { success = false , message = "Erro ao listar ocorrências: " + ex.Message });
             }
         }
 
@@ -82,7 +68,6 @@
         {
             try
             {
-
                 var ocorrencias = _unitOfWork.ViewOcorrenciasAbertasVeiculo
                     .GetAll(o => o.VeiculoId == veiculoId)
                     .OrderByDescending(o => o.DataCriacao)
@@ -106,11 +91,9 @@
 
                 return Ok(new { success = true , data = ocorrencias });
             }
-            catch (Exception error)
-            {
-                _log.Error(error.Message, error, "OcorrenciaViagemController.cs", "ListarAbertasPorVeiculo");
-                Alerta.TratamentoErroComLinha("OcorrenciaViagemController.cs", "ListarAbertasPorVeiculo", error);
-                return Ok(new { success = false , message = "Erro ao listar ocorrências abertas: " + error.Message });
+            catch (Exception ex)
+            {
+                return Ok(new { success = false , message = "Erro ao listar ocorrências abertas: " + ex.Message });
             }
         }
 
@@ -120,18 +103,15 @@
         {
             try
             {
-
                 var count = _unitOfWork.ViewOcorrenciasAbertasVeiculo
                     .GetAll(o => o.VeiculoId == veiculoId)
                     .Count();
 
                 return Ok(new { success = true , count = count });
             }
-            catch (Exception error)
-            {
-                _log.Error(error.Message, error, "OcorrenciaViagemController.cs", "ContarAbertasPorVeiculo");
-                Alerta.TratamentoErroComLinha("OcorrenciaViagemController.cs", "ContarAbertasPorVeiculo", error);
-                return Ok(new { success = false , message = "Erro ao contar ocorrências: " + error.Message });
+            catch (Exception ex)
+            {
+                return Ok(new { success = false , message = "Erro ao contar ocorrências: " + ex.Message });
             }
         }
 
@@ -141,7 +121,6 @@
         {
             try
             {
-
                 var ocorrencia = new OcorrenciaViagem
                 {
                     OcorrenciaViagemId = Guid.NewGuid() ,
@@ -159,14 +138,10 @@
                 _unitOfWork.OcorrenciaViagem.Add(ocorrencia);
                 _unitOfWork.Save();
 
-                _log.Info($"OcorrenciaViagemController.Criar: Ocorrência {ocorrencia.OcorrenciaViagemId} criada para viagem {dto.ViagemId}.");
-
                 return Ok(new { success = true , message = "Ocorrência criada com sucesso!" , id = ocorrencia.OcorrenciaViagemId });
             }
             catch (Exception ex)
             {
-                _log.Error("OcorrenciaViagemController.Criar", ex);
-                Alerta.TratamentoErroComLinha("OcorrenciaViagemController.cs", "Criar", ex);
                 return Ok(new { success = false , message = "Erro ao criar ocorrência: " + ex.Message });
             }
         }
@@ -200,14 +175,10 @@
 
                 _unitOfWork.Save();
 
-                _log.Info($"OcorrenciaViagemController.CriarMultiplas: {criadas} ocorrências criadas.");
-
                 return Ok(new { success = true , message = $"{criadas} ocorrência(s) criada(s) com sucesso!" });
             }
             catch (Exception ex)
             {
-                _log.Error("OcorrenciaViagemController.CriarMultiplas", ex);
-                Alerta.TratamentoErroComLinha("OcorrenciaViagemController.cs", "CriarMultiplas", ex);
                 return Ok(new { success = false , message = "Erro ao criar ocorrências: " + ex.Message });
             }
         }
@@ -229,14 +200,10 @@
                 _unitOfWork.OcorrenciaViagem.Update(ocorrencia);
                 _unitOfWork.Save();
 
-                _log.Info($"OcorrenciaViagemController.DarBaixa: Ocorrência {ocorrenciaId} baixada.");
-
                 return Ok(new { success = true , message = "Ocorrência baixada com sucesso!" });
             }
             catch (Exception ex)
             {
-                _log.Error("OcorrenciaViagemController.DarBaixa", ex);
-                Alerta.TratamentoErroComLinha("OcorrenciaViagemController.cs", "DarBaixa", ex);
                 return Ok(new { success = false , message = "Erro ao dar baixa: " + ex.Message });
             }
         }
@@ -258,14 +225,10 @@
                 _unitOfWork.OcorrenciaViagem.Update(ocorrencia);
                 _unitOfWork.Save();
 
-                _log.Info($"OcorrenciaViagemController.Reabrir: Ocorrência {ocorrenciaId} reaberta.");
-
                 return Ok(new { success = true , message = "Ocorrência reaberta com sucesso!" });
             }
             catch (Exception ex)
             {
-                _log.Error("OcorrenciaViagemController.Reabrir", ex);
-                Alerta.TratamentoErroComLinha("OcorrenciaViagemController.cs", "Reabrir", ex);
                 return Ok(new { success = false , message = "Erro ao reabrir: " + ex.Message });
             }
         }
@@ -283,14 +246,10 @@
                 _unitOfWork.OcorrenciaViagem.Remove(ocorrencia);
                 _unitOfWork.Save();
 
-                _log.Info($"OcorrenciaViagemController.Excluir: Ocorrência {ocorrenciaId} excluída.");
-
                 return Ok(new { success = true , message = "Ocorrência excluída com sucesso!" });
             }
             catch (Exception ex)
             {
-                _log.Error("OcorrenciaViagemController.Excluir", ex);
-                Alerta.TratamentoErroComLinha("OcorrenciaViagemController.cs", "Excluir", ex);
                 return Ok(new { success = false , message = "Erro ao excluir: " + ex.Message });
             }
         }
@@ -316,14 +275,10 @@
                 _unitOfWork.OcorrenciaViagem.Update(ocorrencia);
                 _unitOfWork.Save();
 
-                _log.Info($"OcorrenciaViagemController.Atualizar: Ocorrência {dto.OcorrenciaViagemId} atualizada.");
-
                 return Ok(new { success = true , message = "Ocorrência atualizada com sucesso!" });
             }
             catch (Exception ex)
             {
-                _log.Error("OcorrenciaViagemController.Atualizar", ex);
-                Alerta.TratamentoErroComLinha("OcorrenciaViagemController.cs", "Atualizar", ex);
                 return Ok(new { success = false , message = "Erro ao atualizar: " + ex.Message });
             }
         }
@@ -357,14 +312,10 @@
 
                 var urlRelativa = "/uploads/ocorrencias/" + nomeArquivo;
 
-                _log.Info($"OcorrenciaViagemController.UploadImagem: Upload realizado: {urlRelativa}");
-
                 return Ok(new { success = true , url = urlRelativa });
             }
             catch (Exception ex)
             {
-                _log.Error("OcorrenciaViagemController.UploadImagem", ex);
-                Alerta.TratamentoErroComLinha("OcorrenciaViagemController.cs", "UploadImagem", ex);
                 return Ok(new { success = false , message = "Erro no upload: " + ex.Message });
             }
         }
```

### REMOVER do Janeiro

```csharp
using FrotiX.Helpers;
using FrotiX.Services;
        private readonly ILogService _log;
        public OcorrenciaViagemController(IUnitOfWork unitOfWork, ILogService log)
        {
            try
            {
                _unitOfWork = unitOfWork;
                _log = log;
            }
            catch (Exception ex)
            {
                Alerta.TratamentoErroComLinha("OcorrenciaViagemController.cs", "Constructor", ex);
            }
            catch (Exception error)
            {
                _log.Error(error.Message, error, "OcorrenciaViagemController.cs", "ListarPorViagem");
                Alerta.TratamentoErroComLinha("OcorrenciaViagemController.cs", "ListarPorViagem", error);
                return Ok(new { success = false , message = "Erro ao listar ocorrências: " + error.Message });
            catch (Exception error)
            {
                _log.Error(error.Message, error, "OcorrenciaViagemController.cs", "ListarAbertasPorVeiculo");
                Alerta.TratamentoErroComLinha("OcorrenciaViagemController.cs", "ListarAbertasPorVeiculo", error);
                return Ok(new { success = false , message = "Erro ao listar ocorrências abertas: " + error.Message });
            catch (Exception error)
            {
                _log.Error(error.Message, error, "OcorrenciaViagemController.cs", "ContarAbertasPorVeiculo");
                Alerta.TratamentoErroComLinha("OcorrenciaViagemController.cs", "ContarAbertasPorVeiculo", error);
                return Ok(new { success = false , message = "Erro ao contar ocorrências: " + error.Message });
                _log.Info($"OcorrenciaViagemController.Criar: Ocorrência {ocorrencia.OcorrenciaViagemId} criada para viagem {dto.ViagemId}.");
                _log.Error("OcorrenciaViagemController.Criar", ex);
                Alerta.TratamentoErroComLinha("OcorrenciaViagemController.cs", "Criar", ex);
                _log.Info($"OcorrenciaViagemController.CriarMultiplas: {criadas} ocorrências criadas.");
                _log.Error("OcorrenciaViagemController.CriarMultiplas", ex);
                Alerta.TratamentoErroComLinha("OcorrenciaViagemController.cs", "CriarMultiplas", ex);
                _log.Info($"OcorrenciaViagemController.DarBaixa: Ocorrência {ocorrenciaId} baixada.");
                _log.Error("OcorrenciaViagemController.DarBaixa", ex);
                Alerta.TratamentoErroComLinha("OcorrenciaViagemController.cs", "DarBaixa", ex);
                _log.Info($"OcorrenciaViagemController.Reabrir: Ocorrência {ocorrenciaId} reaberta.");
                _log.Error("OcorrenciaViagemController.Reabrir", ex);
                Alerta.TratamentoErroComLinha("OcorrenciaViagemController.cs", "Reabrir", ex);
                _log.Info($"OcorrenciaViagemController.Excluir: Ocorrência {ocorrenciaId} excluída.");
                _log.Error("OcorrenciaViagemController.Excluir", ex);
                Alerta.TratamentoErroComLinha("OcorrenciaViagemController.cs", "Excluir", ex);
                _log.Info($"OcorrenciaViagemController.Atualizar: Ocorrência {dto.OcorrenciaViagemId} atualizada.");
                _log.Error("OcorrenciaViagemController.Atualizar", ex);
                Alerta.TratamentoErroComLinha("OcorrenciaViagemController.cs", "Atualizar", ex);
                _log.Info($"OcorrenciaViagemController.UploadImagem: Upload realizado: {urlRelativa}");
                _log.Error("OcorrenciaViagemController.UploadImagem", ex);
                Alerta.TratamentoErroComLinha("OcorrenciaViagemController.cs", "UploadImagem", ex);
```


### ADICIONAR ao Janeiro

```csharp
        public OcorrenciaViagemController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            catch (Exception ex)
            {
                return Ok(new { success = false , message = "Erro ao listar ocorrências: " + ex.Message });
            catch (Exception ex)
            {
                return Ok(new { success = false , message = "Erro ao listar ocorrências abertas: " + ex.Message });
            catch (Exception ex)
            {
                return Ok(new { success = false , message = "Erro ao contar ocorrências: " + ex.Message });
```
