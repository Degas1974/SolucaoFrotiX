# Controllers/VeiculoController.cs

**Mudanca:** GRANDE | **+89** linhas | **-33** linhas

---

```diff
--- JANEIRO: Controllers/VeiculoController.cs
+++ ATUAL: Controllers/VeiculoController.cs
@@ -1,7 +1,9 @@
 using FrotiX.Models;
+using FrotiX.Models.Api;
 using FrotiX.Repository.IRepository;
 using FrotiX.Services;
 using Microsoft.AspNetCore.Mvc;
+using Microsoft.Extensions.Logging;
 using System;
 using System.Collections.Generic;
 using System.Linq;
@@ -11,30 +13,94 @@
 
     [Route("api/[controller]")]
     [ApiController]
-    public class VeiculoController : Controller
+    public class VeiculoController :Controller
     {
         private readonly IUnitOfWork _unitOfWork;
-        private readonly ILogService _log;
-
-        public VeiculoController(IUnitOfWork unitOfWork, ILogService log)
+        private readonly ILogService _logService;
+        private readonly ILogger<VeiculoController> _logger;
+
+        public VeiculoController(IUnitOfWork unitOfWork, ILogService logService, ILogger<VeiculoController> logger)
         {
             try
             {
                 _unitOfWork = unitOfWork;
-                _log = log;
-            }
-            catch (Exception error)
-            {
-                Alerta.TratamentoErroComLinha("VeiculoController.cs", "VeiculoController", error);
-            }
-        }
-
-        [HttpGet]
+                _logService = logService;
+                _logger = logger;
+            }
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha("VeiculoController.cs" , "VeiculoController" , error);
+            }
+        }
+
+        [HttpGet]
+        [Route("GetAll")]
+        public IActionResult GetAll()
+        {
+            var requestId = Guid.NewGuid().ToString("N")[..8];
+
+            try
+            {
+                _logger.LogInformation("[{RequestId}] Iniciando GetAll de veículos", requestId);
+
+                var veiculos = _unitOfWork.ViewVeiculos.GetAll();
+
+                var result = veiculos
+                    .Where(v => v.VeiculoId != Guid.Empty && v.Status == true)
+                    .Select(v => new
+                    {
+                        v.VeiculoId,
+                        v.Placa,
+                        v.VeiculoCompleto,
+                        v.MarcaModelo,
+                        v.Categoria,
+                        v.Sigla,
+                        v.Status
+                    })
+                    .OrderBy(v => v.Placa)
+                    .ToList();
+
+                _logger.LogInformation("[{RequestId}] Retornando {Count} veículos", requestId, result.Count);
+
+                Response.Headers["X-Request-Id"] = requestId;
+
+                return Ok(new ApiResponse<object>
+                {
+                    Success = true,
+                    Data = result,
+                    Message = $"{result.Count} veículo(s) encontrado(s)",
+                    RequestId = requestId
+                });
+            }
+            catch (Exception ex)
+            {
+                _logService.Error(
+                    $"[{requestId}] Erro ao carregar veículos: {ex.Message}",
+                    ex,
+                    "VeiculoController.cs",
+                    nameof(GetAll)
+                );
+
+                _logger.LogError(ex, "[{RequestId}] Erro ao carregar veículos", requestId);
+
+                Response.Headers["X-Request-Id"] = requestId;
+
+                return StatusCode(500, ApiResponse<object>.FromException(ex,
+#if DEBUG
+                    includeDetails: true
+#else
+                    includeDetails: false
+#endif
+                ));
+            }
+        }
+
+        [HttpGet]
+        [Route("")]
         public IActionResult Get()
         {
             try
             {
-
                 var objVeiculos = _unitOfWork
                     .ViewVeiculos.GetAllReduced(selector: vv => new
                     {
@@ -62,12 +128,10 @@
             }
             catch (Exception error)
             {
-                _log.Error("Erro", error, "VeiculoController", "Get");
                 Alerta.TratamentoErroComLinha("VeiculoController.cs" , "Get" , error);
-
-                return Json(new
-                {
-                    success = false,
+                return Json(new
+                {
+                    success = false ,
                     message = "Erro ao carregar dados"
                 });
             }
@@ -79,23 +143,18 @@
         {
             try
             {
-
                 if (model != null && model.VeiculoId != Guid.Empty)
                 {
-
                     var objFromDb = _unitOfWork.Veiculo.GetFirstOrDefault(u =>
                         u.VeiculoId == model.VeiculoId
                     );
-
                     if (objFromDb != null)
                     {
-
                         var veiculoContrato = _unitOfWork.VeiculoContrato.GetFirstOrDefault(u =>
                             u.VeiculoId == model.VeiculoId
                         );
                         if (veiculoContrato != null)
                         {
-
                             return Json(
                                 new
                                 {
@@ -110,7 +169,6 @@
                         );
                         if (objViagem != null)
                         {
-
                             return Json(
                                 new
                                 {
@@ -122,9 +180,6 @@
 
                         _unitOfWork.Veiculo.Remove(objFromDb);
                         _unitOfWork.Save();
-
-                        _log.Info($"Veículo removido com sucesso: {objFromDb.Placa} (ID: {model.VeiculoId})", "VeiculoController", "Delete");
-
                         return Json(
                             new
                             {
@@ -134,7 +189,6 @@
                         );
                     }
                 }
-
                 return Json(new
                 {
                     success = false ,
@@ -143,9 +197,7 @@
             }
             catch (Exception error)
             {
-                _log.Error("Erro", error, "VeiculoController", "Delete");
                 Alerta.TratamentoErroComLinha("VeiculoController.cs" , "Delete" , error);
-
                 return Json(new
                 {
                     success = false ,
@@ -159,31 +211,34 @@
         {
             try
             {
-
                 if (Id != Guid.Empty)
                 {
-
                     var objFromDb = _unitOfWork.Veiculo.GetFirstOrDefault(u => u.VeiculoId == Id);
                     string Description = "";
                     int type = 0;
 
                     if (objFromDb != null)
                     {
-
-                        objFromDb.Status = !objFromDb.Status;
-                        type = objFromDb.Status ? 0 : 1;
-                        Description = string.Format(
-                            "Atualizado Status do Veículo [Placa: {0}] ({1})" ,
-                            objFromDb.Placa,
-                            objFromDb.Status ? "Ativo" : "Inativo"
-                        );
-
+                        if (objFromDb.Status == true)
+                        {
+                            objFromDb.Status = false;
+                            Description = string.Format(
+                                "Atualizado Status do Veículo [Nome: {0}] (Inativo)" ,
+                                objFromDb.Placa
+                            );
+                            type = 1;
+                        }
+                        else
+                        {
+                            objFromDb.Status = true;
+                            Description = string.Format(
+                                "Atualizado Status do Veículo [Nome: {0}] (Ativo)" ,
+                                objFromDb.Placa
+                            );
+                            type = 0;
+                        }
                         _unitOfWork.Veiculo.Update(objFromDb);
-                        _unitOfWork.Save();
-
-                        _log.Info(Description, "VeiculoController", "UpdateStatusVeiculo");
                     }
-
                     return Json(
                         new
                         {
@@ -193,7 +248,6 @@
                         }
                     );
                 }
-
                 return Json(new
                 {
                     success = false
@@ -201,9 +255,7 @@
             }
             catch (Exception error)
             {
-                _log.Error("Erro", error, "VeiculoController", "UpdateStatusVeiculo");
                 Alerta.TratamentoErroComLinha("VeiculoController.cs" , "UpdateStatusVeiculo" , error);
-
                 return new JsonResult(new
                 {
                     success = false
@@ -217,7 +269,6 @@
         {
             try
             {
-
                 var result = (
                     from v in _unitOfWork.Veiculo.GetAll()
                     join vc in _unitOfWork.VeiculoContrato.GetAll()
@@ -249,9 +300,7 @@
             }
             catch (Exception error)
             {
-                _log.Error("Erro", error, "VeiculoController", "VeiculoContratos");
                 Alerta.TratamentoErroComLinha("VeiculoController.cs" , "VeiculoContratos" , error);
-
                 return Json(new
                 {
                     success = false ,
@@ -266,9 +315,7 @@
         {
             try
             {
-
                 var manutencoes = _unitOfWork.Manutencao.GetAll();
-
                 var veiculosElegiveis = new HashSet<Guid>(
                     manutencoes
                         .Where(m =>
@@ -316,9 +363,7 @@
             }
             catch (Exception error)
             {
-                _log.Error("Erro", error, "VeiculoController", "VeiculosDoContrato");
                 Alerta.TratamentoErroComLinha("VeiculoController.cs" , "VeiculosDoContrato" , error);
-
                 return View();
             }
         }
@@ -329,36 +374,25 @@
         {
             try
             {
-
                 if (model != null && model.VeiculoId != Guid.Empty)
                 {
-
                     var objFromDb = _unitOfWork.Veiculo.GetFirstOrDefault(u =>
                         u.VeiculoId == model.VeiculoId
                     );
-
                     if (objFromDb != null)
                     {
-
                         var veiculoContrato = _unitOfWork.VeiculoContrato.GetFirstOrDefault(u =>
                             u.VeiculoId == model.VeiculoId && u.ContratoId == model.ContratoId
                         );
-
                         if (veiculoContrato != null)
                         {
-
                             if (objFromDb.ContratoId == model.ContratoId)
                             {
-
                                 objFromDb.ContratoId = Guid.Empty;
                                 _unitOfWork.Veiculo.Update(objFromDb);
                             }
-
                             _unitOfWork.VeiculoContrato.Remove(veiculoContrato);
                             _unitOfWork.Save();
-
-                            _log.Info($"Vínculo de contrato removido para o veículo: {objFromDb.Placa} (ID Contrato: {model.ContratoId})", "VeiculoController", "DeleteContrato");
-
                             return Json(
                                 new
                                 {
@@ -367,21 +401,18 @@
                                 }
                             );
                         }
-
                         return Json(new
                         {
                             success = false ,
                             message = "Erro ao remover veículo"
                         });
                     }
-
                     return Json(new
                     {
                         success = false ,
                         message = "Erro ao remover veículo"
                     });
                 }
-
                 return Json(new
                 {
                     success = false ,
@@ -390,9 +421,7 @@
             }
             catch (Exception error)
             {
-                _log.Error("Erro", error, "VeiculoController", "DeleteContrato");
                 Alerta.TratamentoErroComLinha("VeiculoController.cs" , "DeleteContrato" , error);
-
                 return Json(new
                 {
                     success = false ,
@@ -407,7 +436,6 @@
         {
             try
             {
-
                 var ItemAta = _unitOfWork.ItemVeiculoAta.GetFirstOrDefault(i =>
                     i.ItemVeiculoAtaId == itemAta
                 );
@@ -419,13 +447,11 @@
             }
             catch (Exception error)
             {
-                _log.Error("Erro", error, "VeiculoController", "SelecionaValorMensalAta");
                 Alerta.TratamentoErroComLinha(
                     "VeiculoController.cs" ,
                     "SelecionaValorMensalAta" ,
                     error
                 );
-
                 return new JsonResult(new
                 {
                     success = false
@@ -439,7 +465,6 @@
         {
             try
             {
-
                 var ItemContrato = _unitOfWork.ItemVeiculoContrato.GetFirstOrDefault(i =>
                     i.ItemVeiculoId == itemContrato
                 );
@@ -451,13 +476,11 @@
             }
             catch (Exception error)
             {
-                _log.Error("Erro", error, "VeiculoController", "SelecionaValorMensalContrato");
                 Alerta.TratamentoErroComLinha(
                     "VeiculoController.cs" ,
                     "SelecionaValorMensalContrato" ,
                     error
                 );
-
                 return new JsonResult(new
                 {
                     success = false
```
