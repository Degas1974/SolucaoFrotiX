# Controllers/NotaFiscalController.cs

**Mudanca:** GRANDE | **+35** linhas | **-50** linhas

---

```diff
--- JANEIRO: Controllers/NotaFiscalController.cs
+++ ATUAL: Controllers/NotaFiscalController.cs
@@ -1,6 +1,5 @@
 using FrotiX.Models;
 using FrotiX.Repository.IRepository;
-using FrotiX.Services;
 using Microsoft.AspNetCore.Mvc;
 using System;
 using System.Collections.Generic;
@@ -16,18 +15,16 @@
     public partial class NotaFiscalController : Controller
     {
         private readonly IUnitOfWork _unitOfWork;
-        private readonly ILogService _log;
-
-        public NotaFiscalController(IUnitOfWork unitOfWork, ILogService log)
+
+        public NotaFiscalController(IUnitOfWork unitOfWork)
         {
             try
             {
                 _unitOfWork = unitOfWork;
-                _log = log;
-            }
-            catch (Exception ex)
-            {
-                Alerta.TratamentoErroComLinha("NotaFiscalController.cs", "NotaFiscalController", ex);
+            }
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha("NotaFiscalController.cs", "NotaFiscalController", error);
             }
         }
 
@@ -36,12 +33,10 @@
         {
             try
             {
-
-            }
-            catch (Exception ex)
-            {
-                _log.Error("NotaFiscalController.Get", ex);
-                Alerta.TratamentoErroComLinha("NotaFiscalController.cs", "Get", ex);
+            }
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha("NotaFiscalController.cs", "Get", error);
             }
         }
 
@@ -51,27 +46,21 @@
         {
             try
             {
-
                 if (model != null && model.NotaFiscalId != Guid.Empty)
                 {
-
                     var objFromDb = _unitOfWork.NotaFiscal.GetFirstOrDefault(u =>
                         u.NotaFiscalId == model.NotaFiscalId
                     );
                     if (objFromDb != null)
                     {
-
                         var empenho = _unitOfWork.Empenho.GetFirstOrDefault(u =>
                             u.EmpenhoId == objFromDb.EmpenhoId
                         );
                         if (empenho != null)
                         {
 
-                            double valorEstornado = (objFromDb.ValorNF ?? 0) - (objFromDb.ValorGlosa ?? 0);
-                            empenho.SaldoFinal = empenho.SaldoFinal + valorEstornado;
+                            empenho.SaldoFinal = empenho.SaldoFinal + ((objFromDb.ValorNF ?? 0) - (objFromDb.ValorGlosa ?? 0));
                             _unitOfWork.Empenho.Update(empenho);
-
-                            _log.Info($"NotaFiscalController.Delete: NF {objFromDb.NumeroNF} removida. Valor estornado ao empenho ID {empenho.EmpenhoId}: {valorEstornado:C}");
                         }
 
                         _unitOfWork.NotaFiscal.Remove(objFromDb);
@@ -83,17 +72,15 @@
                         });
                     }
                 }
-
                 return Json(new
                 {
                     success = false,
                     message = "Erro ao apagar Nota Fiscal"
                 });
             }
-            catch (Exception ex)
-            {
-                _log.Error("NotaFiscalController.Delete", ex);
-                Alerta.TratamentoErroComLinha("NotaFiscalController.cs", "Delete", ex);
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha("NotaFiscalController.cs", "Delete", error);
                 return Json(new
                 {
                     success = false,
@@ -108,14 +95,12 @@
         {
             try
             {
-
                 var notaFiscal = _unitOfWork.NotaFiscal.GetFirstOrDefault(u =>
                     u.NotaFiscalId == id
                 );
 
                 if (notaFiscal == null)
                 {
-
                     return Json(new
                     {
                         success = false,
@@ -135,10 +120,9 @@
                     temGlosa = (notaFiscal.ValorGlosa ?? 0) > 0
                 });
             }
-            catch (Exception ex)
-            {
-                _log.Error("NotaFiscalController.GetGlosa", ex);
-                Alerta.TratamentoErroComLinha("NotaFiscalController.cs", "GetGlosa", ex);
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha("NotaFiscalController.cs", "GetGlosa", error);
                 return Json(new
                 {
                     success = false,
@@ -161,7 +145,6 @@
 
                 if (notaFiscal == null)
                 {
-
                     return Json(new
                     {
                         success = false,
@@ -212,8 +195,6 @@
 
                     empenho.SaldoFinal = empenho.SaldoFinal + diferencaGlosa;
                     _unitOfWork.Empenho.Update(empenho);
-
-                    _log.Info($"NotaFiscalController.Glosa: Glosa registrada na NF {notaFiscal.NumeroNF}. Modo: {glosanota.ModoGlosa}. Diferença Saldo Empenho: {diferencaGlosa:C}");
                 }
 
                 _unitOfWork.Save();
@@ -230,14 +211,13 @@
                     novaGlosaFormatada = novaGlosa.ToString("N2")
                 });
             }
-            catch (Exception ex)
-            {
-                _log.Error("NotaFiscalController.Glosa", ex);
-                Alerta.TratamentoErroComLinha("NotaFiscalController.cs", "Glosa", ex);
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha("NotaFiscalController.cs", "Glosa", error);
                 return Json(new
                 {
                     success = false,
-                    message = "Erro ao realizar glosa: " + ex.Message
+                    message = "Erro ao realizar glosa: " + error.Message
                 });
             }
         }
@@ -254,10 +234,9 @@
                     data = EmpenhoList
                 });
             }
-            catch (Exception ex)
-            {
-                _log.Error("NotaFiscalController.EmpenhoList", ex);
-                Alerta.TratamentoErroComLinha("NotaFiscalController.cs", "EmpenhoList", ex);
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha("NotaFiscalController.cs", "EmpenhoList", error);
                 return new JsonResult(new
                 {
                     data = new List<object>()
@@ -277,10 +256,9 @@
                     data = EmpenhoList
                 });
             }
-            catch (Exception ex)
-            {
-                _log.Error("NotaFiscalController.EmpenhoListAta", ex);
-                Alerta.TratamentoErroComLinha("NotaFiscalController.cs", "EmpenhoListAta", ex);
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha("NotaFiscalController.cs", "EmpenhoListAta", error);
                 return new JsonResult(new
                 {
                     data = new List<object>()
@@ -299,10 +277,9 @@
                     data = objContrato
                 });
             }
-            catch (Exception ex)
-            {
-                _log.Error("NotaFiscalController.GetContrato", ex);
-                Alerta.TratamentoErroComLinha("NotaFiscalController.cs", "GetContrato", ex);
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha("NotaFiscalController.cs", "GetContrato", error);
                 return new JsonResult(new
                 {
                     data = new List<object>()
@@ -339,10 +316,9 @@
                     data = NFList
                 });
             }
-            catch (Exception ex)
-            {
-                _log.Error("NotaFiscalController.NFContratos", ex);
-                Alerta.TratamentoErroComLinha("NotaFiscalController.cs", "NFContratos", ex);
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha("NotaFiscalController.cs", "NFContratos", error);
                 return Json(new
                 {
                     data = new List<object>()
@@ -379,10 +355,9 @@
                     data = NFList
                 });
             }
-            catch (Exception ex)
-            {
-                _log.Error("NotaFiscalController.NFEmpenhos", ex);
-                Alerta.TratamentoErroComLinha("NotaFiscalController.cs", "NFEmpenhos", ex);
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha("NotaFiscalController.cs", "NFEmpenhos", error);
                 return Json(new
                 {
                     data = new List<object>()
```
