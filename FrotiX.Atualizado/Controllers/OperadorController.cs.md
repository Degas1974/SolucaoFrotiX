# Controllers/OperadorController.cs

**Mudanca:** MEDIA | **+7** linhas | **-22** linhas

---

```diff
--- JANEIRO: Controllers/OperadorController.cs
+++ ATUAL: Controllers/OperadorController.cs
@@ -1,6 +1,5 @@
 using FrotiX.Models;
 using FrotiX.Repository.IRepository;
-using FrotiX.Services;
 using Microsoft.AspNetCore.Mvc;
 using System;
 using System.Collections.Generic;
@@ -11,21 +10,19 @@
 
     [Route("api/[controller]")]
     [ApiController]
-    public class OperadorController : Controller
+    public class OperadorController :Controller
     {
         private readonly IUnitOfWork _unitOfWork;
-        private readonly ILogService _log;
-
-        public OperadorController(IUnitOfWork unitOfWork, ILogService log)
+
+        public OperadorController(IUnitOfWork unitOfWork)
         {
             try
             {
                 _unitOfWork = unitOfWork;
-                _log = log;
-            }
-            catch (Exception error)
-            {
-                Alerta.TratamentoErroComLinha("OperadorController.cs", "OperadorController", error);
+            }
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha("OperadorController.cs" , "OperadorController" , error);
             }
         }
 
@@ -34,7 +31,6 @@
         {
             try
             {
-
                 var result = (
                     from o in _unitOfWork.Operador.GetAll()
 
@@ -86,7 +82,6 @@
             }
             catch (Exception error)
             {
-                _log.Error("OperadorController.Get", error);
                 Alerta.TratamentoErroComLinha("OperadorController.cs" , "Get" , error);
                 return Json(new
                 {
@@ -101,22 +96,18 @@
         {
             try
             {
-
                 if (model != null && model.OperadorId != Guid.Empty)
                 {
-
                     var objFromDb = _unitOfWork.Operador.GetFirstOrDefault(u =>
                         u.OperadorId == model.OperadorId
                     );
                     if (objFromDb != null)
                     {
-
                         var operadorContrato = _unitOfWork.OperadorContrato.GetFirstOrDefault(u =>
                             u.OperadorId == model.OperadorId
                         );
                         if (operadorContrato != null)
                         {
-
                             return Json(
                                 new
                                 {
@@ -128,8 +119,6 @@
 
                         _unitOfWork.Operador.Remove(objFromDb);
                         _unitOfWork.Save();
-
-                        _log.Info($"OperadorController.Delete: Operador {objFromDb.Nome} ({objFromDb.OperadorId}) removido.");
                         return Json(
                             new
                             {
@@ -139,7 +128,6 @@
                         );
                     }
                 }
-
                 return Json(new
                 {
                     success = false ,
@@ -148,7 +136,6 @@
             }
             catch (Exception error)
             {
-                _log.Error("OperadorController.Delete", error);
                 Alerta.TratamentoErroComLinha("OperadorController.cs" , "Delete" , error);
                 return Json(new
                 {
@@ -163,10 +150,8 @@
         {
             try
             {
-
                 if (Id != Guid.Empty)
                 {
-
                     var objFromDb = _unitOfWork.Operador.GetFirstOrDefault(u => u.OperadorId == Id);
                     string Description = "";
                     int type = 0;
@@ -175,7 +160,6 @@
                     {
                         if (objFromDb.Status == true)
                         {
-
                             objFromDb.Status = false;
                             Description = string.Format(
                                 "Atualizado Status do Operador [Nome: {0}] (Inativo)" ,
@@ -185,7 +169,6 @@
                         }
                         else
                         {
-
                             objFromDb.Status = true;
                             Description = string.Format(
                                 "Atualizado Status do Operador [Nome: {0}] (Ativo)" ,
@@ -193,13 +176,8 @@
                             );
                             type = 0;
                         }
-
                         _unitOfWork.Operador.Update(objFromDb);
-                        _unitOfWork.Save();
-
-                        _log.Info($"OperadorController.UpdateStatusOperador: {Description}");
-                    }
-
+                    }
                     return Json(
                         new
                         {
@@ -209,7 +187,6 @@
                         }
                     );
                 }
-
                 return Json(new
                 {
                     success = false
@@ -217,7 +194,6 @@
             }
             catch (Exception error)
             {
-                _log.Error("OperadorController.UpdateStatusOperador", error);
                 Alerta.TratamentoErroComLinha("OperadorController.cs" , "UpdateStatusOperador" , error);
                 return new JsonResult(new
                 {
@@ -232,7 +208,6 @@
         {
             try
             {
-
                 if (id != Guid.Empty)
                 {
                     var objFromDb = _unitOfWork.Operador.GetFirstOrDefault(u =>
@@ -252,7 +227,6 @@
             }
             catch (Exception error)
             {
-                _log.Error("OperadorController.PegaFoto", error);
                 Alerta.TratamentoErroComLinha("OperadorController.cs" , "PegaFoto" , error);
                 return new JsonResult(new
                 {
@@ -277,7 +251,6 @@
             }
             catch (Exception error)
             {
-                _log.Error("OperadorController.PegaFotoModal", error);
                 Alerta.TratamentoErroComLinha("OperadorController.cs" , "PegaFotoModal" , error);
                 return new JsonResult(new
                 {
@@ -299,7 +272,6 @@
             }
             catch (Exception error)
             {
-                _log.Error("OperadorController.GetImage", error);
                 Alerta.TratamentoErroComLinha("OperadorController.cs" , "GetImage" , error);
                 return default(byte[]);
             }
@@ -336,7 +308,6 @@
             }
             catch (Exception error)
             {
-                _log.Error("OperadorController.OperadorContratos", error);
                 Alerta.TratamentoErroComLinha("OperadorController.cs" , "OperadorContratos" , error);
                 return Json(new
                 {
@@ -370,7 +341,6 @@
                             }
                             _unitOfWork.OperadorContrato.Remove(operadorContrato);
                             _unitOfWork.Save();
-                            _log.Info($"OperadorController.DeleteContrato: Vínculo do Operador {objFromDb.Nome} com contrato {model.ContratoId} removido.");
                             return Json(
                                 new
                                 {
@@ -399,7 +369,6 @@
             }
             catch (Exception error)
             {
-                _log.Error("OperadorController.DeleteContrato", error);
                 Alerta.TratamentoErroComLinha("OperadorController.cs" , "DeleteContrato" , error);
                 return Json(new
                 {
```

### REMOVER do Janeiro

```csharp
using FrotiX.Services;
    public class OperadorController : Controller
        private readonly ILogService _log;
        public OperadorController(IUnitOfWork unitOfWork, ILogService log)
                _log = log;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("OperadorController.cs", "OperadorController", error);
                _log.Error("OperadorController.Get", error);
                        _log.Info($"OperadorController.Delete: Operador {objFromDb.Nome} ({objFromDb.OperadorId}) removido.");
                _log.Error("OperadorController.Delete", error);
                        _unitOfWork.Save();
                        _log.Info($"OperadorController.UpdateStatusOperador: {Description}");
                    }
                _log.Error("OperadorController.UpdateStatusOperador", error);
                _log.Error("OperadorController.PegaFoto", error);
                _log.Error("OperadorController.PegaFotoModal", error);
                _log.Error("OperadorController.GetImage", error);
                _log.Error("OperadorController.OperadorContratos", error);
                            _log.Info($"OperadorController.DeleteContrato: Vínculo do Operador {objFromDb.Nome} com contrato {model.ContratoId} removido.");
                _log.Error("OperadorController.DeleteContrato", error);
```


### ADICIONAR ao Janeiro

```csharp
    public class OperadorController :Controller
        public OperadorController(IUnitOfWork unitOfWork)
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("OperadorController.cs" , "OperadorController" , error);
                    }
```
