# Controllers/MultaController.cs

**Mudanca:** GRANDE | **+10** linhas | **-60** linhas

---

```diff
--- JANEIRO: Controllers/MultaController.cs
+++ ATUAL: Controllers/MultaController.cs
@@ -9,54 +9,35 @@
 
 namespace FrotiX.Controllers
 {
-
     [Route("api/[controller]")]
     [ApiController]
     [IgnoreAntiforgeryToken]
     public class MultaController : Controller
     {
         [BindProperty]
-        public MovimentacaoEmpenhoMultaViewModel MovimentacaoObj { get; set; }
+        public MovimentacaoEmpenhoMultaViewModel MovimentacaoObj
+        {
+            get; set;
+        }
 
         private readonly IUnitOfWork _unitOfWork;
-        private readonly ILogService _log;
-
-        public MultaController(IUnitOfWork unitOfWork, ILogService log)
+
+        public MultaController(IUnitOfWork unitOfWork)
         {
             try
             {
                 _unitOfWork = unitOfWork;
-                _log = log;
-            }
-            catch (Exception error)
-            {
-                Alerta.TratamentoErroComLinha("MultaController.cs", "MultaController", error);
+            }
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha("MultaController.cs" , "MultaController" , error);
             }
         }
 
         [HttpGet("Test")]
         public IActionResult Test()
         {
-            try
-            {
-
-                return Ok(
-                    new
-                    {
-                        success = true ,
-                        message = "MultaPdfViewer está funcionando!"
-                    }
-                );
-            }
-            catch (Exception error)
-            {
-                _log.Error(error.Message , error , "MultaController.cs" , "Test");
-                Alerta.TratamentoErroComLinha("MultaController.cs" , "Test" , error);
-                return BadRequest(new
-                {
-                    success = false
-                });
-            }
+            return Ok(new { success = true , message = "MultaPdfViewer está funcionando!" });
         }
 
         [Route("ListaMultas")]
@@ -72,7 +53,6 @@
         {
             try
             {
-
                 var result = (
                     from vm in _unitOfWork.viewMultas.GetAll()
                     where vm.Fase == Fase
@@ -149,7 +129,6 @@
             }
             catch (Exception error)
             {
-                _log.Error(error.Message , error , "MultaController.cs" , "ListaMultas");
                 Alerta.TratamentoErroComLinha("MultaController.cs" , "ListaMultas" , error);
                 return Json(new
                 {
@@ -165,7 +144,6 @@
         {
             try
             {
-
                 var result = (
                     from tm in _unitOfWork.TipoMulta.GetAll()
                     select new
@@ -185,7 +163,6 @@
             }
             catch (Exception error)
             {
-                _log.Error(error.Message , error , "MultaController.cs" , "PegaTipoMulta");
                 Alerta.TratamentoErroComLinha("MultaController.cs" , "PegaTipoMulta" , error);
                 return Json(new
                 {
@@ -210,7 +187,6 @@
             }
             catch (Exception error)
             {
-                _log.Error(error.Message , error , "MultaController.cs" , "PegaOrgaoAutuante");
                 Alerta.TratamentoErroComLinha("MultaController.cs" , "PegaOrgaoAutuante" , error);
                 return Json(new
                 {
@@ -260,7 +236,6 @@
             }
             catch (Exception error)
             {
-                _log.Error(error.Message , error , "MultaController.cs" , "DeleteTipoMulta");
                 Alerta.TratamentoErroComLinha("MultaController.cs" , "DeleteTipoMulta" , error);
                 return Json(new
                 {
@@ -307,7 +282,6 @@
             }
             catch (Exception error)
             {
-                _log.Error(error.Message , error , "MultaController.cs" , "DeleteOrgaoAutuante");
                 Alerta.TratamentoErroComLinha("MultaController.cs" , "DeleteOrgaoAutuante" , error);
                 return Json(new
                 {
@@ -353,7 +327,6 @@
             }
             catch (Exception error)
             {
-                _log.Error(error.Message , error , "MultaController.cs" , "PegaEmpenhos");
                 Alerta.TratamentoErroComLinha("MultaController.cs" , "PegaEmpenhos" , error);
                 return Json(new
                 {
@@ -427,7 +400,6 @@
             }
             catch (Exception error)
             {
-                _log.Error(error.Message , error , "MultaController.cs" , "Delete");
                 Alerta.TratamentoErroComLinha("MultaController.cs" , "Delete" , error);
                 return Json(
                     new
@@ -533,7 +505,6 @@
             }
             catch (Exception error)
             {
-                _log.Error(error.Message , error , "MultaController.cs" , "TransformaPenalidade");
                 Alerta.TratamentoErroComLinha("MultaController.cs" , "TransformaPenalidade" , error);
                 return Json(
                     new
@@ -678,7 +649,6 @@
             }
             catch (Exception error)
             {
-                _log.Error(error.Message , error , "MultaController.cs" , "ProcuraViagem");
                 Alerta.TratamentoErroComLinha("MultaController.cs" , "ProcuraViagem" , error);
                 return Json(
                     new
@@ -733,7 +703,6 @@
             }
             catch (Exception error)
             {
-                _log.Error(error.Message , error , "MultaController.cs" , "ProcuraFicha");
                 Alerta.TratamentoErroComLinha("MultaController.cs" , "ProcuraFicha" , error);
                 return Json(
                     new
@@ -815,7 +784,6 @@
             }
             catch (Exception error)
             {
-                _log.Error(error.Message , error , "MultaController.cs" , "PegaImagemFichaVistoria");
                 Alerta.TratamentoErroComLinha("MultaController.cs" , "PegaImagemFichaVistoria" , error);
                 return Json(new
                 {
@@ -850,7 +818,6 @@
             }
             catch (Exception error)
             {
-                _log.Error(error.Message , error , "MultaController.cs" , "OnGetMultaExistente");
                 Alerta.TratamentoErroComLinha("MultaController.cs" , "OnGetMultaExistente" , error);
                 return new JsonResult(new
                 {
@@ -894,7 +861,6 @@
             }
             catch (Exception error)
             {
-                _log.Error(error.Message , error , "MultaController.cs" , "OnPostAlteraStatus");
                 Alerta.TratamentoErroComLinha("MultaController.cs" , "OnPostAlteraStatus" , error);
                 return new JsonResult(new
                 {
@@ -956,7 +922,6 @@
             }
             catch (Exception error)
             {
-                _log.Error(error.Message , error , "MultaController.cs" , "OnPostPegaStatus");
                 Alerta.TratamentoErroComLinha("MultaController.cs" , "OnPostPegaStatus" , error);
                 return new JsonResult(new
                 {
@@ -1036,7 +1001,6 @@
             }
             catch (Exception error)
             {
-                _log.Error(error.Message , error , "MultaController.cs" , "OnPostPegaInstrumentoVeiculo");
                 Alerta.TratamentoErroComLinha(
                     "MultaController.cs" ,
                     "OnPostPegaInstrumentoVeiculo" ,
@@ -1083,7 +1047,6 @@
             }
             catch (Exception error)
             {
-                _log.Error(error.Message , error , "MultaController.cs" , "OnPostValidaContratoVeiculo");
                 Alerta.TratamentoErroComLinha(
                     "MultaController.cs" ,
                     "OnPostValidaContratoVeiculo" ,
@@ -1129,7 +1092,6 @@
             }
             catch (Exception error)
             {
-                _log.Error(error.Message , error , "MultaController.cs" , "OnPostValidaAtaVeiculo");
                 Alerta.TratamentoErroComLinha(
                     "MultaController.cs" ,
                     "OnPostValidaAtaVeiculo" ,
@@ -1188,7 +1150,6 @@
             }
             catch (Exception error)
             {
-                _log.Error(error.Message , error , "MultaController.cs" , "OnPostPegaContratoMotorista");
                 Alerta.TratamentoErroComLinha(
                     "MultaController.cs" ,
                     "OnPostPegaContratoMotorista" ,
@@ -1235,7 +1196,6 @@
             }
             catch (Exception error)
             {
-                _log.Error(error.Message , error , "MultaController.cs" , "OnPostValidaContratoMotorista");
                 Alerta.TratamentoErroComLinha(
                     "MultaController.cs" ,
                     "OnPostValidaContratoMotorista" ,
@@ -1293,7 +1253,6 @@
             }
             catch (Exception error)
             {
-                _log.Error(error.Message , error , "MultaController.cs" , "OnPostPegaValor");
                 Alerta.TratamentoErroComLinha("MultaController.cs" , "OnPostPegaValor" , error);
                 return new JsonResult(new
                 {
@@ -1349,7 +1308,6 @@
             }
             catch (Exception error)
             {
-                _log.Error(error.Message , error , "MultaController.cs" , "OnPostPegaEmpenhoMultaId");
                 Alerta.TratamentoErroComLinha(
                     "MultaController.cs" ,
                     "OnPostPegaEmpenhoMultaId" ,
@@ -1435,7 +1393,6 @@
             }
             catch (Exception error)
             {
-                _log.Error(error.Message , error , "MultaController.cs" , "OnPostRegistraPagamento");
                 Alerta.TratamentoErroComLinha(
                     "MultaController.cs" ,
                     "OnPostRegistraPagamento" ,
@@ -1498,7 +1455,6 @@
             }
             catch (Exception error)
             {
-                _log.Error(error.Message , error , "MultaController.cs" , "OnPostPegaObservacao");
                 Alerta.TratamentoErroComLinha("MultaController.cs" , "OnPostPegaObservacao" , error);
                 return new JsonResult(new
                 {
@@ -1534,7 +1490,6 @@
             }
             catch (Exception error)
             {
-                _log.Error(error.Message , error , "MultaController.cs" , "MultaEmpenho");
                 Alerta.TratamentoErroComLinha("MultaController.cs" , "MultaEmpenho" , error);
                 return Json(new
                 {
@@ -1570,7 +1525,6 @@
             }
             catch (Exception error)
             {
-                _log.Error(error.Message , error , "MultaController.cs" , "MultaEmpenhoPagas");
                 Alerta.TratamentoErroComLinha("MultaController.cs" , "MultaEmpenhoPagas" , error);
                 return Json(new
                 {
@@ -1601,7 +1555,6 @@
             }
             catch (Exception error)
             {
-                _log.Error(error.Message , error , "MultaController.cs" , "SaldoMultas");
                 Alerta.TratamentoErroComLinha("MultaController.cs" , "SaldoMultas" , error);
                 return Json(new
                 {
@@ -1637,7 +1590,6 @@
             }
             catch (Exception error)
             {
-                _log.Error(error.Message , error , "MultaController.cs" , "ListaAporte");
                 Alerta.TratamentoErroComLinha("MultaController.cs" , "ListaAporte" , error);
                 return Json(new
                 {
@@ -1673,7 +1625,6 @@
             }
             catch (Exception error)
             {
-                _log.Error(error.Message , error , "MultaController.cs" , "ListaAnulacao");
                 Alerta.TratamentoErroComLinha("MultaController.cs" , "ListaAnulacao" , error);
                 return Json(new
                 {
@@ -1710,7 +1661,6 @@
             }
             catch (Exception error)
             {
-                _log.Error(error.Message , error , "MultaController.cs" , "Aporte");
                 Alerta.TratamentoErroComLinha("MultaController.cs" , "Aporte" , error);
                 return Json(new
                 {
@@ -1748,7 +1698,6 @@
             }
             catch (Exception error)
             {
-                _log.Error(error.Message , error , "MultaController.cs" , "Anulacao");
                 Alerta.TratamentoErroComLinha("MultaController.cs" , "Anulacao" , error);
                 return Json(new
                 {
@@ -1793,7 +1742,6 @@
             }
             catch (Exception error)
             {
-                _log.Error(error.Message , error , "MultaController.cs" , "EditarAporte");
                 Alerta.TratamentoErroComLinha("MultaController.cs" , "EditarAporte" , error);
                 return Json(new
                 {
@@ -1835,7 +1783,6 @@
             }
             catch (Exception error)
             {
-                _log.Error(error.Message , error , "MultaController.cs" , "VerificaPDFExiste");
                 Alerta.TratamentoErroComLinha("MultaController.cs" , "VerificaPDFExiste" , error);
                 return Json(new
                 {
@@ -1881,7 +1828,6 @@
             }
             catch (Exception error)
             {
-                _log.Error(error.Message , error , "MultaController.cs" , "EditarAnulacao");
                 Alerta.TratamentoErroComLinha("MultaController.cs" , "EditarAnulacao" , error);
                 return Json(new
                 {
```

### REMOVER do Janeiro

```csharp
        public MovimentacaoEmpenhoMultaViewModel MovimentacaoObj { get; set; }
        private readonly ILogService _log;
        public MultaController(IUnitOfWork unitOfWork, ILogService log)
                _log = log;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("MultaController.cs", "MultaController", error);
            try
            {
                return Ok(
                    new
                    {
                        success = true ,
                        message = "MultaPdfViewer está funcionando!"
                    }
                );
            }
            catch (Exception error)
            {
                _log.Error(error.Message , error , "MultaController.cs" , "Test");
                Alerta.TratamentoErroComLinha("MultaController.cs" , "Test" , error);
                return BadRequest(new
                {
                    success = false
                });
            }
                _log.Error(error.Message , error , "MultaController.cs" , "ListaMultas");
                _log.Error(error.Message , error , "MultaController.cs" , "PegaTipoMulta");
                _log.Error(error.Message , error , "MultaController.cs" , "PegaOrgaoAutuante");
                _log.Error(error.Message , error , "MultaController.cs" , "DeleteTipoMulta");
                _log.Error(error.Message , error , "MultaController.cs" , "DeleteOrgaoAutuante");
                _log.Error(error.Message , error , "MultaController.cs" , "PegaEmpenhos");
                _log.Error(error.Message , error , "MultaController.cs" , "Delete");
                _log.Error(error.Message , error , "MultaController.cs" , "TransformaPenalidade");
                _log.Error(error.Message , error , "MultaController.cs" , "ProcuraViagem");
                _log.Error(error.Message , error , "MultaController.cs" , "ProcuraFicha");
                _log.Error(error.Message , error , "MultaController.cs" , "PegaImagemFichaVistoria");
                _log.Error(error.Message , error , "MultaController.cs" , "OnGetMultaExistente");
                _log.Error(error.Message , error , "MultaController.cs" , "OnPostAlteraStatus");
                _log.Error(error.Message , error , "MultaController.cs" , "OnPostPegaStatus");
                _log.Error(error.Message , error , "MultaController.cs" , "OnPostPegaInstrumentoVeiculo");
                _log.Error(error.Message , error , "MultaController.cs" , "OnPostValidaContratoVeiculo");
                _log.Error(error.Message , error , "MultaController.cs" , "OnPostValidaAtaVeiculo");
                _log.Error(error.Message , error , "MultaController.cs" , "OnPostPegaContratoMotorista");
                _log.Error(error.Message , error , "MultaController.cs" , "OnPostValidaContratoMotorista");
                _log.Error(error.Message , error , "MultaController.cs" , "OnPostPegaValor");
                _log.Error(error.Message , error , "MultaController.cs" , "OnPostPegaEmpenhoMultaId");
                _log.Error(error.Message , error , "MultaController.cs" , "OnPostRegistraPagamento");
                _log.Error(error.Message , error , "MultaController.cs" , "OnPostPegaObservacao");
                _log.Error(error.Message , error , "MultaController.cs" , "MultaEmpenho");
                _log.Error(error.Message , error , "MultaController.cs" , "MultaEmpenhoPagas");
                _log.Error(error.Message , error , "MultaController.cs" , "SaldoMultas");
                _log.Error(error.Message , error , "MultaController.cs" , "ListaAporte");
                _log.Error(error.Message , error , "MultaController.cs" , "ListaAnulacao");
                _log.Error(error.Message , error , "MultaController.cs" , "Aporte");
                _log.Error(error.Message , error , "MultaController.cs" , "Anulacao");
                _log.Error(error.Message , error , "MultaController.cs" , "EditarAporte");
                _log.Error(error.Message , error , "MultaController.cs" , "VerificaPDFExiste");
                _log.Error(error.Message , error , "MultaController.cs" , "EditarAnulacao");
```


### ADICIONAR ao Janeiro

```csharp
        public MovimentacaoEmpenhoMultaViewModel MovimentacaoObj
        {
            get; set;
        }
        public MultaController(IUnitOfWork unitOfWork)
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("MultaController.cs" , "MultaController" , error);
            return Ok(new { success = true , message = "MultaPdfViewer está funcionando!" });
```
