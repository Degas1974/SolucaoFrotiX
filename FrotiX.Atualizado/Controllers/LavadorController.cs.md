# Controllers/LavadorController.cs

**Mudanca:** MEDIA | **+9** linhas | **-20** linhas

---

```diff
--- JANEIRO: Controllers/LavadorController.cs
+++ ATUAL: Controllers/LavadorController.cs
@@ -1,6 +1,5 @@
 using FrotiX.Models;
 using FrotiX.Repository.IRepository;
-using FrotiX.Services;
 using Microsoft.AspNetCore.Mvc;
 using System;
 using System.Collections.Generic;
@@ -8,24 +7,21 @@
 
 namespace FrotiX.Controllers
 {
-
     [Route("api/[controller]")]
     [ApiController]
     public class LavadorController : Controller
     {
         private readonly IUnitOfWork _unitOfWork;
-        private readonly ILogService _log;
-
-        public LavadorController(IUnitOfWork unitOfWork, ILogService log)
+
+        public LavadorController(IUnitOfWork unitOfWork)
         {
             try
             {
                 _unitOfWork = unitOfWork;
-                _log = log;
-            }
-            catch (Exception error)
-            {
-                Alerta.TratamentoErroComLinha("LavadorController.cs", "LavadorController", error);
+            }
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha("LavadorController.cs" , "LavadorController" , error);
             }
         }
 
@@ -34,7 +30,6 @@
         {
             try
             {
-
                 var result = (
                     from l in _unitOfWork.Lavador.GetAll()
 
@@ -86,7 +81,6 @@
             }
             catch (Exception error)
             {
-                _log.Error(error.Message, error, "LavadorController.cs", "Get");
                 Alerta.TratamentoErroComLinha("LavadorController.cs" , "Get" , error);
                 return View();
             }
@@ -98,11 +92,9 @@
         {
             try
             {
-
                 if (model != null && model.LavadorId != Guid.Empty)
                 {
-
-                    var objFromDb = _unitOfWork.Lavador.GetFirstOrDefaultWithTracking(u =>
+                    var objFromDb = _unitOfWork.Lavador.GetFirstOrDefault(u =>
                         u.LavadorId == model.LavadorId
                     );
                     if (objFromDb != null)
@@ -113,7 +105,6 @@
                         );
                         if (lavadorContrato != null)
                         {
-
                             return Json(
                                 new
                                 {
@@ -134,7 +125,6 @@
                         );
                     }
                 }
-
                 return Json(new
                 {
                     success = false ,
@@ -143,7 +133,6 @@
             }
             catch (Exception error)
             {
-                _log.Error(error.Message, error, "LavadorController.cs", "Delete");
                 Alerta.TratamentoErroComLinha("LavadorController.cs" , "Delete" , error);
                 return View();
             }
@@ -154,11 +143,9 @@
         {
             try
             {
-
                 if (Id != Guid.Empty)
                 {
-
-                    var objFromDb = _unitOfWork.Lavador.GetFirstOrDefaultWithTracking(u => u.LavadorId == Id);
+                    var objFromDb = _unitOfWork.Lavador.GetFirstOrDefault(u => u.LavadorId == Id);
                     string Description = "";
                     int type = 0;
 
@@ -187,7 +174,6 @@
 
                         _unitOfWork.Lavador.Update(objFromDb);
                     }
-
                     return Json(
                         new
                         {
@@ -197,7 +183,6 @@
                         }
                     );
                 }
-
                 return Json(new
                 {
                     success = false
@@ -205,7 +190,6 @@
             }
             catch (Exception error)
             {
-                _log.Error(error.Message, error, "LavadorController.cs", "UpdateStatusLavador");
                 Alerta.TratamentoErroComLinha("LavadorController.cs" , "UpdateStatusLavador" , error);
                 return new JsonResult(new
                 {
@@ -220,31 +204,25 @@
         {
             try
             {
-
                 if (id != Guid.Empty)
                 {
-
                     var objFromDb = _unitOfWork.Lavador.GetFirstOrDefault(u =>
                         u.LavadorId == id
                     );
                     if (objFromDb.Foto != null)
                     {
-
                         objFromDb.Foto = this.GetImage(Convert.ToBase64String(objFromDb.Foto));
                         return Json(objFromDb);
                     }
-
                     return Json(false);
                 }
                 else
                 {
-
                     return Json(false);
                 }
             }
             catch (Exception error)
             {
-                _log.Error(error.Message, error, "LavadorController.cs", "PegaFoto");
                 Alerta.TratamentoErroComLinha("LavadorController.cs" , "PegaFoto" , error);
                 return new JsonResult(new
                 {
@@ -259,20 +237,16 @@
         {
             try
             {
-
                 var objFromDb = _unitOfWork.Lavador.GetFirstOrDefault(u => u.LavadorId == id);
                 if (objFromDb.Foto != null)
                 {
-
                     objFromDb.Foto = this.GetImage(Convert.ToBase64String(objFromDb.Foto));
                     return Json(objFromDb.Foto);
                 }
-
                 return Json(false);
             }
             catch (Exception error)
             {
-                _log.Error(error.Message, error, "LavadorController.cs", "PegaFotoModal");
                 Alerta.TratamentoErroComLinha("LavadorController.cs" , "PegaFotoModal" , error);
                 return new JsonResult(new
                 {
@@ -288,15 +262,12 @@
                 byte[] bytes = null;
                 if (!string.IsNullOrEmpty(sBase64String))
                 {
-
                     bytes = Convert.FromBase64String(sBase64String);
                 }
-
                 return bytes;
             }
             catch (Exception error)
             {
-                _log.Error(error.Message, error, "LavadorController.cs", "GetImage");
                 Alerta.TratamentoErroComLinha("LavadorController.cs" , "GetImage" , error);
                 return default(byte[]);
             }
@@ -308,7 +279,6 @@
         {
             try
             {
-
                 var result = (
                     from m in _unitOfWork.Lavador.GetAll()
 
@@ -334,7 +304,6 @@
             }
             catch (Exception error)
             {
-                _log.Error(error.Message, error, "LavadorController.cs", "LavadorContratos");
                 Alerta.TratamentoErroComLinha("LavadorController.cs" , "LavadorContratos" , error);
                 return View();
             }
@@ -346,28 +315,24 @@
         {
             try
             {
-
                 if (model != null && model.LavadorId != Guid.Empty)
                 {
-
-                    var objFromDb = _unitOfWork.Lavador.GetFirstOrDefaultWithTracking(u =>
+                    var objFromDb = _unitOfWork.Lavador.GetFirstOrDefault(u =>
                         u.LavadorId == model.LavadorId
                     );
                     if (objFromDb != null)
                     {
 
-                        var lavadorContrato = _unitOfWork.LavadorContrato.GetFirstOrDefaultWithTracking(u =>
+                        var lavadorContrato = _unitOfWork.LavadorContrato.GetFirstOrDefault(u =>
                             u.LavadorId == model.LavadorId && u.ContratoId == model.ContratoId
                         );
                         if (lavadorContrato != null)
                         {
                             if (objFromDb.ContratoId == model.ContratoId)
                             {
-
                                 objFromDb.ContratoId = Guid.Empty;
                                 _unitOfWork.Lavador.Update(objFromDb);
                             }
-
                             _unitOfWork.LavadorContrato.Remove(lavadorContrato);
                             _unitOfWork.Save();
                             return Json(
@@ -378,21 +343,18 @@
                                 }
                             );
                         }
-
                         return Json(new
                         {
                             success = false ,
                             message = "Erro ao remover lavador"
                         });
                     }
-
                     return Json(new
                     {
                         success = false ,
                         message = "Erro ao remover lavador"
                     });
                 }
-
                 return Json(new
                 {
                     success = false ,
@@ -401,7 +363,6 @@
             }
             catch (Exception error)
             {
-                _log.Error(error.Message, error, "LavadorController.cs", "DeleteContrato");
                 Alerta.TratamentoErroComLinha("LavadorController.cs" , "DeleteContrato" , error);
                 return View();
             }
```

### REMOVER do Janeiro

```csharp
using FrotiX.Services;
        private readonly ILogService _log;
        public LavadorController(IUnitOfWork unitOfWork, ILogService log)
                _log = log;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("LavadorController.cs", "LavadorController", error);
                _log.Error(error.Message, error, "LavadorController.cs", "Get");
                    var objFromDb = _unitOfWork.Lavador.GetFirstOrDefaultWithTracking(u =>
                _log.Error(error.Message, error, "LavadorController.cs", "Delete");
                    var objFromDb = _unitOfWork.Lavador.GetFirstOrDefaultWithTracking(u => u.LavadorId == Id);
                _log.Error(error.Message, error, "LavadorController.cs", "UpdateStatusLavador");
                _log.Error(error.Message, error, "LavadorController.cs", "PegaFoto");
                _log.Error(error.Message, error, "LavadorController.cs", "PegaFotoModal");
                _log.Error(error.Message, error, "LavadorController.cs", "GetImage");
                _log.Error(error.Message, error, "LavadorController.cs", "LavadorContratos");
                    var objFromDb = _unitOfWork.Lavador.GetFirstOrDefaultWithTracking(u =>
                        var lavadorContrato = _unitOfWork.LavadorContrato.GetFirstOrDefaultWithTracking(u =>
                _log.Error(error.Message, error, "LavadorController.cs", "DeleteContrato");
```


### ADICIONAR ao Janeiro

```csharp
        public LavadorController(IUnitOfWork unitOfWork)
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("LavadorController.cs" , "LavadorController" , error);
                    var objFromDb = _unitOfWork.Lavador.GetFirstOrDefault(u =>
                    var objFromDb = _unitOfWork.Lavador.GetFirstOrDefault(u => u.LavadorId == Id);
                    var objFromDb = _unitOfWork.Lavador.GetFirstOrDefault(u =>
                        var lavadorContrato = _unitOfWork.LavadorContrato.GetFirstOrDefault(u =>
```
