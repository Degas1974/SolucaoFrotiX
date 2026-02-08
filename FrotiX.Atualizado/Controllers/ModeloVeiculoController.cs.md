# Controllers/ModeloVeiculoController.cs

**Mudanca:** MEDIA | **+7** linhas | **-9** linhas

---

```diff
--- JANEIRO: Controllers/ModeloVeiculoController.cs
+++ ATUAL: Controllers/ModeloVeiculoController.cs
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
-    public class ModeloVeiculoController : Controller
+    public class ModeloVeiculoController :Controller
     {
         private readonly IUnitOfWork _unitOfWork;
-        private readonly ILogService _log;
 
-        public ModeloVeiculoController(IUnitOfWork unitOfWork, ILogService log)
+        public ModeloVeiculoController(IUnitOfWork unitOfWork)
         {
             try
             {
                 _unitOfWork = unitOfWork;
-                _log = log;
             }
             catch (Exception error)
             {
-                Alerta.TratamentoErroComLinha("ModeloVeiculoController.cs", "ModeloVeiculoController", error);
+                Alerta.TratamentoErroComLinha(
+                    "ModeloVeiculoController.cs" ,
+                    "ModeloVeiculoController" ,
+                    error
+                );
             }
         }
 
@@ -32,7 +32,6 @@
         {
             try
             {
-
                 return Json(
                     new
                     {
@@ -42,7 +41,6 @@
             }
             catch (Exception error)
             {
-                _log.Error(error.Message , error , "ModeloVeiculoController.cs" , "Get");
                 Alerta.TratamentoErroComLinha("ModeloVeiculoController.cs" , "Get" , error);
                 return View();
             }
@@ -54,10 +52,8 @@
         {
             try
             {
-
                 if (model != null && model.ModeloId != Guid.Empty)
                 {
-
                     var objFromDb = _unitOfWork.ModeloVeiculo.GetFirstOrDefault(u =>
                         u.ModeloId == model.ModeloId
                     );
@@ -69,7 +65,6 @@
                         );
                         if (veiculo != null)
                         {
-
                             return Json(
                                 new
                                 {
@@ -78,7 +73,6 @@
                                 }
                             );
                         }
-
                         _unitOfWork.ModeloVeiculo.Remove(objFromDb);
                         _unitOfWork.Save();
                         return Json(
@@ -90,7 +84,6 @@
                         );
                     }
                 }
-
                 return Json(new
                 {
                     success = false ,
@@ -99,7 +92,6 @@
             }
             catch (Exception error)
             {
-                _log.Error(error.Message , error , "ModeloVeiculoController.cs" , "Delete");
                 Alerta.TratamentoErroComLinha("ModeloVeiculoController.cs" , "Delete" , error);
                 return View();
             }
@@ -110,10 +102,8 @@
         {
             try
             {
-
                 if (Id != Guid.Empty)
                 {
-
                     var objFromDb = _unitOfWork.ModeloVeiculo.GetFirstOrDefault(u =>
                         u.ModeloId == Id
                     );
@@ -145,7 +135,6 @@
 
                         _unitOfWork.ModeloVeiculo.Update(objFromDb);
                     }
-
                     return Json(
                         new
                         {
@@ -155,7 +144,6 @@
                         }
                     );
                 }
-
                 return Json(new
                 {
                     success = false
@@ -163,7 +151,6 @@
             }
             catch (Exception error)
             {
-                _log.Error(error.Message , error , "ModeloVeiculoController.cs" , "UpdateStatusModeloVeiculo");
                 Alerta.TratamentoErroComLinha(
                     "ModeloVeiculoController.cs" ,
                     "UpdateStatusModeloVeiculo" ,
```

### REMOVER do Janeiro

```csharp
using FrotiX.Services;
    public class ModeloVeiculoController : Controller
        private readonly ILogService _log;
        public ModeloVeiculoController(IUnitOfWork unitOfWork, ILogService log)
                _log = log;
                Alerta.TratamentoErroComLinha("ModeloVeiculoController.cs", "ModeloVeiculoController", error);
                _log.Error(error.Message , error , "ModeloVeiculoController.cs" , "Get");
                _log.Error(error.Message , error , "ModeloVeiculoController.cs" , "Delete");
                _log.Error(error.Message , error , "ModeloVeiculoController.cs" , "UpdateStatusModeloVeiculo");
```


### ADICIONAR ao Janeiro

```csharp
    public class ModeloVeiculoController :Controller
        public ModeloVeiculoController(IUnitOfWork unitOfWork)
                Alerta.TratamentoErroComLinha(
                    "ModeloVeiculoController.cs" ,
                    "ModeloVeiculoController" ,
                    error
                );
```
