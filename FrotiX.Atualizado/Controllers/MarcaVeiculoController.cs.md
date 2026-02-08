# Controllers/MarcaVeiculoController.cs

**Mudanca:** MEDIA | **+9** linhas | **-11** linhas

---

```diff
--- JANEIRO: Controllers/MarcaVeiculoController.cs
+++ ATUAL: Controllers/MarcaVeiculoController.cs
@@ -1,29 +1,29 @@
 using FrotiX.Models;
 using FrotiX.Repository.IRepository;
-using FrotiX.Services;
 using Microsoft.AspNetCore.Mvc;
 using System;
 
 namespace FrotiX.Controllers
 {
-
     [Route("api/[controller]")]
     [ApiController]
-    public class MarcaVeiculoController : Controller
+    public class MarcaVeiculoController :Controller
     {
         private readonly IUnitOfWork _unitOfWork;
-        private readonly ILogService _log;
 
-        public MarcaVeiculoController(IUnitOfWork unitOfWork, ILogService log)
+        public MarcaVeiculoController(IUnitOfWork unitOfWork)
         {
             try
             {
                 _unitOfWork = unitOfWork;
-                _log = log;
             }
             catch (Exception error)
             {
-                Alerta.TratamentoErroComLinha("MarcaVeiculoController.cs", "MarcaVeiculoController", error);
+                Alerta.TratamentoErroComLinha(
+                    "MarcaVeiculoController.cs" ,
+                    "MarcaVeiculoController" ,
+                    error
+                );
             }
         }
 
@@ -32,7 +32,6 @@
         {
             try
             {
-
                 return Json(new
                 {
                     data = _unitOfWork.MarcaVeiculo.GetAll()
@@ -40,7 +39,6 @@
             }
             catch (Exception error)
             {
-                _log.Error(error.Message , error , "MarcaVeiculoController.cs" , "Get");
                 Alerta.TratamentoErroComLinha("MarcaVeiculoController.cs" , "Get" , error);
                 return View();
             }
@@ -52,22 +50,18 @@
         {
             try
             {
-
                 if (model != null && model.MarcaId != Guid.Empty)
                 {
-
-                    var objFromDb = _unitOfWork.MarcaVeiculo.GetFirstOrDefaultWithTracking(u =>
+                    var objFromDb = _unitOfWork.MarcaVeiculo.GetFirstOrDefault(u =>
                         u.MarcaId == model.MarcaId
                     );
                     if (objFromDb != null)
                     {
-
                         var modelo = _unitOfWork.ModeloVeiculo.GetFirstOrDefault(u =>
                             u.MarcaId == model.MarcaId
                         );
                         if (modelo != null)
                         {
-
                             return Json(
                                 new
                                 {
@@ -76,7 +70,6 @@
                                 }
                             );
                         }
-
                         _unitOfWork.MarcaVeiculo.Remove(objFromDb);
                         _unitOfWork.Save();
                         return Json(
@@ -88,7 +81,6 @@
                         );
                     }
                 }
-
                 return Json(new
                 {
                     success = false ,
@@ -97,7 +89,6 @@
             }
             catch (Exception error)
             {
-                _log.Error(error.Message , error , "MarcaVeiculoController.cs" , "Delete");
                 Alerta.TratamentoErroComLinha("MarcaVeiculoController.cs" , "Delete" , error);
                 return View();
             }
@@ -108,11 +99,9 @@
         {
             try
             {
-
                 if (Id != Guid.Empty)
                 {
-
-                    var objFromDb = _unitOfWork.MarcaVeiculo.GetFirstOrDefaultWithTracking(u =>
+                    var objFromDb = _unitOfWork.MarcaVeiculo.GetFirstOrDefault(u =>
                         u.MarcaId == Id
                     );
                     string Description = "";
@@ -143,7 +132,6 @@
 
                         _unitOfWork.MarcaVeiculo.Update(objFromDb);
                     }
-
                     return Json(
                         new
                         {
@@ -153,7 +141,6 @@
                         }
                     );
                 }
-
                 return Json(new
                 {
                     success = false
@@ -161,7 +148,6 @@
             }
             catch (Exception error)
             {
-                _log.Error(error.Message , error , "MarcaVeiculoController.cs" , "UpdateStatusMarcaVeiculo");
                 Alerta.TratamentoErroComLinha(
                     "MarcaVeiculoController.cs" ,
                     "UpdateStatusMarcaVeiculo" ,
```

### REMOVER do Janeiro

```csharp
using FrotiX.Services;
    public class MarcaVeiculoController : Controller
        private readonly ILogService _log;
        public MarcaVeiculoController(IUnitOfWork unitOfWork, ILogService log)
                _log = log;
                Alerta.TratamentoErroComLinha("MarcaVeiculoController.cs", "MarcaVeiculoController", error);
                _log.Error(error.Message , error , "MarcaVeiculoController.cs" , "Get");
                    var objFromDb = _unitOfWork.MarcaVeiculo.GetFirstOrDefaultWithTracking(u =>
                _log.Error(error.Message , error , "MarcaVeiculoController.cs" , "Delete");
                    var objFromDb = _unitOfWork.MarcaVeiculo.GetFirstOrDefaultWithTracking(u =>
                _log.Error(error.Message , error , "MarcaVeiculoController.cs" , "UpdateStatusMarcaVeiculo");
```


### ADICIONAR ao Janeiro

```csharp
    public class MarcaVeiculoController :Controller
        public MarcaVeiculoController(IUnitOfWork unitOfWork)
                Alerta.TratamentoErroComLinha(
                    "MarcaVeiculoController.cs" ,
                    "MarcaVeiculoController" ,
                    error
                );
                    var objFromDb = _unitOfWork.MarcaVeiculo.GetFirstOrDefault(u =>
                    var objFromDb = _unitOfWork.MarcaVeiculo.GetFirstOrDefault(u =>
```
