# Controllers/EscalaController.cs

**Mudanca:** GRANDE | **+21** linhas | **-35** linhas

---

```diff
--- JANEIRO: Controllers/EscalaController.cs
+++ ATUAL: Controllers/EscalaController.cs
@@ -1,3 +1,5 @@
+using FrotiX.Helpers;
+
 using System;
 using System.Collections.Generic;
 using System.Linq;
@@ -12,12 +14,12 @@
 using Microsoft.AspNetCore.SignalR;
 using Microsoft.Extensions.Logging;
 using EscalaDiaria = FrotiX.Models.EscalaDiaria;
+
 using FolgaRecesso = FrotiX.Models.FolgaRecesso;
 using CoberturaFolga = FrotiX.Models.CoberturaFolga;
 
 namespace FrotiX.Controllers
 {
-
     public partial class EscalaController : Controller
     {
         private readonly IUnitOfWork _unitOfWork;
@@ -26,41 +28,26 @@
 
         public EscalaController(IUnitOfWork unitOfWork, ILogger<EscalaController> logger, IHubContext<EscalaHub> hubContext)
         {
-            try
-            {
-                _unitOfWork = unitOfWork;
-                _logger = logger;
-                _hubContext = hubContext;
-            }
-            catch (Exception ex)
-            {
-                Alerta.TratamentoErroComLinha("EscalaController.cs", "EscalaController", ex);
-            }
+            _unitOfWork = unitOfWork;
+            _logger = logger;
+            _hubContext = hubContext;
         }
 
         public IActionResult Create()
         {
-            try
-            {
-                var model = new EscalaDiariaViewModel
-                {
-                    DataEscala = DateTime.Today,
-                    MotoristaList = _unitOfWork.Motorista.GetMotoristaListForDropDown(),
-                    VeiculoList = _unitOfWork.Veiculo.GetVeiculoListForDropDown(),
-                    TipoServicoList = _unitOfWork.TipoServico.GetTipoServicoListForDropDown(),
-                    TurnoList = _unitOfWork.Turno.GetTurnoListForDropDown(),
-                    RequisitanteList = _unitOfWork.Requisitante.GetRequisitanteListForDropDown(),
-                    LotacaoList = GetLotacaoList(),
-                    StatusList = GetStatusList()
-                };
-
-                return View(model);
-            }
-            catch (Exception ex)
-            {
-                Alerta.TratamentoErroComLinha("EscalaController.cs", "Create (GET)", ex);
-                return RedirectToAction("Index");
-            }
+            var model = new EscalaDiariaViewModel
+            {
+                DataEscala = DateTime.Today,
+                MotoristaList = _unitOfWork.Motorista.GetMotoristaListForDropDown(),
+                VeiculoList = _unitOfWork.Veiculo.GetVeiculoListForDropDown(),
+                TipoServicoList = _unitOfWork.TipoServico.GetTipoServicoListForDropDown(),
+                TurnoList = _unitOfWork.Turno.GetTurnoListForDropDown(),
+                RequisitanteList = _unitOfWork.Requisitante.GetRequisitanteListForDropDown(),
+                LotacaoList = GetLotacaoList(),
+                StatusList = GetStatusList()
+            };
+
+            return View(model);
         }
 
         [HttpPost]
@@ -77,6 +64,7 @@
 
                     if (model.MotoristaId.HasValue)
                     {
+
                         var conflito = await _unitOfWork.EscalaDiaria.ExisteEscalaConflitanteAsync(
                             model.MotoristaId.Value, model.DataEscala, horaInicio, horaFim, null);
 
@@ -90,6 +78,7 @@
 
                         if (associacao == null && model.VeiculoId.HasValue)
                         {
+
                             associacao = new VAssociado
                             {
                                 MotoristaId = model.MotoristaId.Value,
@@ -143,7 +132,7 @@
                 }
                 catch (Exception ex)
                 {
-                    Alerta.TratamentoErroComLinha("EscalaController.cs", "Create (POST)", ex);
+                    _logger.LogError(ex, "Erro ao criar escala");
                     TempData["error"] = "Erro ao criar escala: " + ex.Message;
                 }
             }
@@ -262,7 +251,7 @@
                 }
                 catch (Exception ex)
                 {
-                    Alerta.TratamentoErroComLinha("EscalaController.cs", "Edit (POST)", ex);
+                    _logger.LogError(ex, "Erro ao atualizar escala");
                     TempData["error"] = "Erro ao atualizar escala: " + ex.Message;
                 }
             }
@@ -318,7 +307,7 @@
             }
             catch (Exception ex)
             {
-                Alerta.TratamentoErroComLinha("EscalaController.cs", "DeleteConfirmed", ex);
+                _logger.LogError(ex, "Erro ao excluir escala");
                 TempData["error"] = "Erro ao excluir escala: " + ex.Message;
             }
 
@@ -353,7 +342,7 @@
             }
             catch (Exception ex)
             {
-                Alerta.TratamentoErroComLinha("EscalaController.cs", "AtualizarStatus", ex);
+                _logger.LogError(ex, "Erro ao atualizar status");
                 return Json(new { success = false, message = "Erro ao atualizar status" });
             }
         }
@@ -444,7 +433,7 @@
             }
             catch (Exception ex)
             {
-                Alerta.TratamentoErroComLinha("EscalaController.cs", "NotificarAtualizacaoEscalas", ex);
+                _logger.LogError(ex, "Erro ao notificar via SignalR");
             }
         }
 
```

### REMOVER do Janeiro

```csharp
            try
            {
                _unitOfWork = unitOfWork;
                _logger = logger;
                _hubContext = hubContext;
            }
            catch (Exception ex)
            {
                Alerta.TratamentoErroComLinha("EscalaController.cs", "EscalaController", ex);
            }
            try
            {
                var model = new EscalaDiariaViewModel
                {
                    DataEscala = DateTime.Today,
                    MotoristaList = _unitOfWork.Motorista.GetMotoristaListForDropDown(),
                    VeiculoList = _unitOfWork.Veiculo.GetVeiculoListForDropDown(),
                    TipoServicoList = _unitOfWork.TipoServico.GetTipoServicoListForDropDown(),
                    TurnoList = _unitOfWork.Turno.GetTurnoListForDropDown(),
                    RequisitanteList = _unitOfWork.Requisitante.GetRequisitanteListForDropDown(),
                    LotacaoList = GetLotacaoList(),
                    StatusList = GetStatusList()
                };
                return View(model);
            }
            catch (Exception ex)
            {
                Alerta.TratamentoErroComLinha("EscalaController.cs", "Create (GET)", ex);
                return RedirectToAction("Index");
            }
                    Alerta.TratamentoErroComLinha("EscalaController.cs", "Create (POST)", ex);
                    Alerta.TratamentoErroComLinha("EscalaController.cs", "Edit (POST)", ex);
                Alerta.TratamentoErroComLinha("EscalaController.cs", "DeleteConfirmed", ex);
                Alerta.TratamentoErroComLinha("EscalaController.cs", "AtualizarStatus", ex);
                Alerta.TratamentoErroComLinha("EscalaController.cs", "NotificarAtualizacaoEscalas", ex);
```


### ADICIONAR ao Janeiro

```csharp
using FrotiX.Helpers;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _hubContext = hubContext;
            var model = new EscalaDiariaViewModel
            {
                DataEscala = DateTime.Today,
                MotoristaList = _unitOfWork.Motorista.GetMotoristaListForDropDown(),
                VeiculoList = _unitOfWork.Veiculo.GetVeiculoListForDropDown(),
                TipoServicoList = _unitOfWork.TipoServico.GetTipoServicoListForDropDown(),
                TurnoList = _unitOfWork.Turno.GetTurnoListForDropDown(),
                RequisitanteList = _unitOfWork.Requisitante.GetRequisitanteListForDropDown(),
                LotacaoList = GetLotacaoList(),
                StatusList = GetStatusList()
            };
            return View(model);
                    _logger.LogError(ex, "Erro ao criar escala");
                    _logger.LogError(ex, "Erro ao atualizar escala");
                _logger.LogError(ex, "Erro ao excluir escala");
                _logger.LogError(ex, "Erro ao atualizar status");
                _logger.LogError(ex, "Erro ao notificar via SignalR");
```
