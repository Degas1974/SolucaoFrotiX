# Controllers/EmpenhoController.cs

**Mudanca:** GRANDE | **+13** linhas | **-28** linhas

---

```diff
--- JANEIRO: Controllers/EmpenhoController.cs
+++ ATUAL: Controllers/EmpenhoController.cs
@@ -1,28 +1,25 @@
+using FrotiX.Helpers;
+
 using FrotiX.Models;
 using FrotiX.Repository.IRepository;
 using Microsoft.AspNetCore.Mvc;
 using System;
 using System.Linq;
-using FrotiX.Helpers;
-using FrotiX.Services;
 
 namespace FrotiX.Controllers
 {
-
     [Route("api/[controller]")]
     [ApiController]
     [IgnoreAntiforgeryToken]
     public class EmpenhoController : Controller
     {
         private readonly IUnitOfWork _unitOfWork;
-        private readonly ILogService _logService;
-
-        public EmpenhoController(IUnitOfWork unitOfWork, ILogService logService)
+
+        public EmpenhoController(IUnitOfWork unitOfWork)
         {
             try
             {
                 _unitOfWork = unitOfWork;
-                _logService = logService;
             }
             catch (Exception error)
             {
@@ -38,7 +35,6 @@
 
                 if (instrumento == "contrato")
                 {
-
                     var result = (
                         from ve in _unitOfWork.ViewEmpenhos.GetAll()
                         where ve.ContratoId == Id
@@ -63,7 +59,6 @@
                 }
                 else
                 {
-
                     var result = (
                         from ve in _unitOfWork.ViewEmpenhos.GetAll()
                         where ve.AtaId == Id
@@ -88,7 +83,6 @@
             }
             catch (Exception error)
             {
-                _logService.Error(error.Message, error, "EmpenhoController.cs", "Get");
                 Alerta.TratamentoErroComLinha("EmpenhoController.cs", "Get", error);
                 return StatusCode(500);
             }
@@ -102,11 +96,12 @@
             {
                 if (model != null && model.EmpenhoId != Guid.Empty)
                 {
-                    var objFromDb = _unitOfWork.Empenho.GetFirstOrDefaultWithTracking(u =>
+                    var objFromDb = _unitOfWork.Empenho.GetFirstOrDefault(u =>
                         u.EmpenhoId == model.EmpenhoId
                     );
                     if (objFromDb != null)
                     {
+
                         var notas = _unitOfWork.NotaFiscal.GetFirstOrDefault(u =>
                             u.EmpenhoId == model.EmpenhoId
                         );
@@ -154,7 +149,6 @@
             }
             catch (Exception error)
             {
-                _logService.Error(error.Message, error, "EmpenhoController.cs", "Delete");
                 Alerta.TratamentoErroComLinha("EmpenhoController.cs", "Delete", error);
                 return StatusCode(500);
             }
@@ -170,7 +164,7 @@
 
                 _unitOfWork.MovimentacaoEmpenho.Add(movimentacao);
 
-                var empenho = _unitOfWork.Empenho.GetFirstOrDefaultWithTracking(u =>
+                var empenho = _unitOfWork.Empenho.GetFirstOrDefault(u =>
                     u.EmpenhoId == movimentacao.EmpenhoId
                 );
                 empenho.SaldoFinal = empenho.SaldoFinal + movimentacao.Valor;
@@ -189,7 +183,6 @@
             }
             catch (Exception error)
             {
-                _logService.Error(error.Message, error, "EmpenhoController.cs", "Aporte");
                 Alerta.TratamentoErroComLinha("EmpenhoController.cs", "Aporte", error);
                 return StatusCode(500);
             }
@@ -202,7 +195,7 @@
         {
             try
             {
-                var movimentacaoDb = _unitOfWork.MovimentacaoEmpenho.GetFirstOrDefaultWithTracking(u =>
+                var movimentacaoDb = _unitOfWork.MovimentacaoEmpenho.GetFirstOrDefault(u =>
                     u.MovimentacaoId == movimentacao.MovimentacaoId
                 );
 
@@ -210,7 +203,7 @@
 
                 _unitOfWork.MovimentacaoEmpenho.Update(movimentacao);
 
-                var empenho = _unitOfWork.Empenho.GetFirstOrDefaultWithTracking(u =>
+                var empenho = _unitOfWork.Empenho.GetFirstOrDefault(u =>
                     u.EmpenhoId == movimentacao.EmpenhoId
                 );
                 empenho.SaldoFinal = empenho.SaldoFinal - valorAnterior + movimentacao.Valor;
@@ -229,7 +222,6 @@
             }
             catch (Exception error)
             {
-                _logService.Error(error.Message, error, "EmpenhoController.cs", "EditarAporte");
                 Alerta.TratamentoErroComLinha("EmpenhoController.cs", "EditarAporte", error);
                 return StatusCode(500);
             }
@@ -242,7 +234,7 @@
         {
             try
             {
-                var movimentacaoDb = _unitOfWork.MovimentacaoEmpenho.GetFirstOrDefaultWithTracking(u =>
+                var movimentacaoDb = _unitOfWork.MovimentacaoEmpenho.GetFirstOrDefault(u =>
                     u.MovimentacaoId == movimentacao.MovimentacaoId
                 );
 
@@ -250,7 +242,7 @@
 
                 _unitOfWork.MovimentacaoEmpenho.Update(movimentacao);
 
-                var empenho = _unitOfWork.Empenho.GetFirstOrDefaultWithTracking(u =>
+                var empenho = _unitOfWork.Empenho.GetFirstOrDefault(u =>
                     u.EmpenhoId == movimentacao.EmpenhoId
                 );
                 empenho.SaldoFinal = empenho.SaldoFinal + valorAnterior - movimentacao.Valor;
@@ -269,7 +261,6 @@
             }
             catch (Exception error)
             {
-                _logService.Error(error.Message, error, "EmpenhoController.cs", "EditarAnulacao");
                 Alerta.TratamentoErroComLinha("EmpenhoController.cs", "EditarAnulacao", error);
                 return StatusCode(500);
             }
@@ -283,12 +274,12 @@
             {
                 if (model.mEmpenho != null && model.mEmpenho.MovimentacaoId != Guid.Empty)
                 {
-                    var objFromDb = _unitOfWork.MovimentacaoEmpenho.GetFirstOrDefaultWithTracking(u =>
+                    var objFromDb = _unitOfWork.MovimentacaoEmpenho.GetFirstOrDefault(u =>
                         u.MovimentacaoId == model.mEmpenho.MovimentacaoId
                     );
                     if (objFromDb != null)
                     {
-                        var empenho = _unitOfWork.Empenho.GetFirstOrDefaultWithTracking(u =>
+                        var empenho = _unitOfWork.Empenho.GetFirstOrDefault(u =>
                             u.EmpenhoId == objFromDb.EmpenhoId
                         );
 
@@ -318,12 +309,12 @@
                     && model.mEmpenhoMulta.MovimentacaoId != Guid.Empty
                 )
                 {
-                    var objFromDb = _unitOfWork.MovimentacaoEmpenhoMulta.GetFirstOrDefaultWithTracking(u =>
+                    var objFromDb = _unitOfWork.MovimentacaoEmpenhoMulta.GetFirstOrDefault(u =>
                         u.MovimentacaoId == model.mEmpenhoMulta.MovimentacaoId
                     );
                     if (objFromDb != null)
                     {
-                        var empenhoMulta = _unitOfWork.EmpenhoMulta.GetFirstOrDefaultWithTracking(u =>
+                        var empenhoMulta = _unitOfWork.EmpenhoMulta.GetFirstOrDefault(u =>
                             u.EmpenhoMultaId == objFromDb.EmpenhoMultaId
                         );
 
@@ -356,7 +347,6 @@
             }
             catch (Exception error)
             {
-                _logService.Error(error.Message, error, "EmpenhoController.cs", "DeleteMovimentacao");
                 Alerta.TratamentoErroComLinha("EmpenhoController.cs", "DeleteMovimentacao", error);
                 return StatusCode(500);
             }
@@ -373,7 +363,7 @@
                 movimentacao.Valor = movimentacao.Valor * -1;
                 _unitOfWork.MovimentacaoEmpenho.Add(movimentacao);
 
-                var empenho = _unitOfWork.Empenho.GetFirstOrDefaultWithTracking(u =>
+                var empenho = _unitOfWork.Empenho.GetFirstOrDefault(u =>
                     u.EmpenhoId == movimentacao.EmpenhoId
                 );
                 empenho.SaldoFinal = empenho.SaldoFinal + movimentacao.Valor;
@@ -392,7 +382,6 @@
             }
             catch (Exception error)
             {
-                _logService.Error(error.Message, error, "EmpenhoController.cs", "Anulacao");
                 Alerta.TratamentoErroComLinha("EmpenhoController.cs", "Anulacao", error);
                 return StatusCode(500);
             }
@@ -426,7 +415,6 @@
             }
             catch (Exception error)
             {
-                _logService.Error(error.Message, error, "EmpenhoController.cs", "ListaAporte");
                 Alerta.TratamentoErroComLinha("EmpenhoController.cs", "ListaAporte", error);
                 return StatusCode(500);
             }
@@ -460,7 +448,6 @@
             }
             catch (Exception error)
             {
-                _logService.Error(error.Message, error, "EmpenhoController.cs", "ListaAnulacao");
                 Alerta.TratamentoErroComLinha("EmpenhoController.cs", "ListaAnulacao", error);
                 return StatusCode(500);
             }
@@ -472,6 +459,7 @@
         {
             try
             {
+
                 var notas = _unitOfWork.NotaFiscal.GetAll(u => u.EmpenhoId == Id);
 
                 double totalnotas = 0;
@@ -487,7 +475,6 @@
             }
             catch (Exception error)
             {
-                _logService.Error(error.Message, error, "EmpenhoController.cs", "SaldoNotas");
                 Alerta.TratamentoErroComLinha("EmpenhoController.cs", "SaldoNotas", error);
                 return StatusCode(500);
             }
@@ -553,7 +540,6 @@
             }
             catch (Exception error)
             {
-                _logService.Error(error.Message, error, "EmpenhoController.cs", "InsereEmpenho");
                 Alerta.TratamentoErroComLinha("EmpenhoController.cs", "InsereEmpenho", error);
                 return new JsonResult(new
                 {
@@ -618,7 +604,6 @@
             }
             catch (Exception error)
             {
-                _logService.Error(error.Message, error, "EmpenhoController.cs", "EditaEmpenho");
                 Alerta.TratamentoErroComLinha("EmpenhoController.cs", "EditaEmpenho", error);
                 return new JsonResult(new
                 {
```

### REMOVER do Janeiro

```csharp
using FrotiX.Helpers;
using FrotiX.Services;
        private readonly ILogService _logService;
        public EmpenhoController(IUnitOfWork unitOfWork, ILogService logService)
                _logService = logService;
                _logService.Error(error.Message, error, "EmpenhoController.cs", "Get");
                    var objFromDb = _unitOfWork.Empenho.GetFirstOrDefaultWithTracking(u =>
                _logService.Error(error.Message, error, "EmpenhoController.cs", "Delete");
                var empenho = _unitOfWork.Empenho.GetFirstOrDefaultWithTracking(u =>
                _logService.Error(error.Message, error, "EmpenhoController.cs", "Aporte");
                var movimentacaoDb = _unitOfWork.MovimentacaoEmpenho.GetFirstOrDefaultWithTracking(u =>
                var empenho = _unitOfWork.Empenho.GetFirstOrDefaultWithTracking(u =>
                _logService.Error(error.Message, error, "EmpenhoController.cs", "EditarAporte");
                var movimentacaoDb = _unitOfWork.MovimentacaoEmpenho.GetFirstOrDefaultWithTracking(u =>
                var empenho = _unitOfWork.Empenho.GetFirstOrDefaultWithTracking(u =>
                _logService.Error(error.Message, error, "EmpenhoController.cs", "EditarAnulacao");
                    var objFromDb = _unitOfWork.MovimentacaoEmpenho.GetFirstOrDefaultWithTracking(u =>
                        var empenho = _unitOfWork.Empenho.GetFirstOrDefaultWithTracking(u =>
                    var objFromDb = _unitOfWork.MovimentacaoEmpenhoMulta.GetFirstOrDefaultWithTracking(u =>
                        var empenhoMulta = _unitOfWork.EmpenhoMulta.GetFirstOrDefaultWithTracking(u =>
                _logService.Error(error.Message, error, "EmpenhoController.cs", "DeleteMovimentacao");
                var empenho = _unitOfWork.Empenho.GetFirstOrDefaultWithTracking(u =>
                _logService.Error(error.Message, error, "EmpenhoController.cs", "Anulacao");
                _logService.Error(error.Message, error, "EmpenhoController.cs", "ListaAporte");
                _logService.Error(error.Message, error, "EmpenhoController.cs", "ListaAnulacao");
                _logService.Error(error.Message, error, "EmpenhoController.cs", "SaldoNotas");
                _logService.Error(error.Message, error, "EmpenhoController.cs", "InsereEmpenho");
                _logService.Error(error.Message, error, "EmpenhoController.cs", "EditaEmpenho");
```


### ADICIONAR ao Janeiro

```csharp
using FrotiX.Helpers;
        public EmpenhoController(IUnitOfWork unitOfWork)
                    var objFromDb = _unitOfWork.Empenho.GetFirstOrDefault(u =>
                var empenho = _unitOfWork.Empenho.GetFirstOrDefault(u =>
                var movimentacaoDb = _unitOfWork.MovimentacaoEmpenho.GetFirstOrDefault(u =>
                var empenho = _unitOfWork.Empenho.GetFirstOrDefault(u =>
                var movimentacaoDb = _unitOfWork.MovimentacaoEmpenho.GetFirstOrDefault(u =>
                var empenho = _unitOfWork.Empenho.GetFirstOrDefault(u =>
                    var objFromDb = _unitOfWork.MovimentacaoEmpenho.GetFirstOrDefault(u =>
                        var empenho = _unitOfWork.Empenho.GetFirstOrDefault(u =>
                    var objFromDb = _unitOfWork.MovimentacaoEmpenhoMulta.GetFirstOrDefault(u =>
                        var empenhoMulta = _unitOfWork.EmpenhoMulta.GetFirstOrDefault(u =>
                var empenho = _unitOfWork.Empenho.GetFirstOrDefault(u =>
```
