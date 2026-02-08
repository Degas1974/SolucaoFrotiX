# Controllers/EncarregadoController.cs

**Mudanca:** MEDIA | **+2** linhas | **-13** linhas

---

```diff
--- JANEIRO: Controllers/EncarregadoController.cs
+++ ATUAL: Controllers/EncarregadoController.cs
@@ -1,28 +1,25 @@
+using FrotiX.Helpers;
+
 using FrotiX.Models;
 using FrotiX.Repository.IRepository;
 using Microsoft.AspNetCore.Mvc;
 using System;
 using System.Collections.Generic;
 using System.Linq;
-using FrotiX.Helpers;
-using FrotiX.Services;
 
 namespace FrotiX.Controllers
 {
-
     [Route("api/[controller]")]
     [ApiController]
     public class EncarregadoController : Controller
     {
         private readonly IUnitOfWork _unitOfWork;
-        private readonly ILogService _logService;
-
-        public EncarregadoController(IUnitOfWork unitOfWork, ILogService logService)
+
+        public EncarregadoController(IUnitOfWork unitOfWork)
         {
             try
             {
                 _unitOfWork = unitOfWork;
-                _logService = logService;
             }
             catch (Exception error)
             {
@@ -87,7 +84,6 @@
             }
             catch (Exception error)
             {
-                _logService.Error(error.Message, error, "EncarregadoController.cs", "Get");
                 Alerta.TratamentoErroComLinha("EncarregadoController.cs", "Get", error);
                 return Json(new
                 {
@@ -142,7 +138,6 @@
             }
             catch (Exception error)
             {
-                _logService.Error(error.Message, error, "EncarregadoController.cs", "Delete");
                 Alerta.TratamentoErroComLinha("EncarregadoController.cs", "Delete", error);
                 return Json(new
                 {
@@ -201,7 +196,6 @@
             }
             catch (Exception error)
             {
-                _logService.Error(error.Message, error, "EncarregadoController.cs", "UpdateStatusEncarregado");
                 Alerta.TratamentoErroComLinha("EncarregadoController.cs", "UpdateStatusEncarregado", error);
                 return new JsonResult(new
                 {
@@ -223,6 +217,7 @@
                     );
                     if (objFromDb.Foto != null)
                     {
+
                         objFromDb.Foto = this.GetImage(Convert.ToBase64String(objFromDb.Foto));
                         return Json(objFromDb);
                     }
@@ -235,7 +230,6 @@
             }
             catch (Exception error)
             {
-                _logService.Error(error.Message, error, "EncarregadoController.cs", "PegaFoto");
                 Alerta.TratamentoErroComLinha("EncarregadoController.cs", "PegaFoto", error);
                 return new JsonResult(new
                 {
@@ -260,7 +254,6 @@
             }
             catch (Exception error)
             {
-                _logService.Error(error.Message, error, "EncarregadoController.cs", "PegaFotoModal");
                 Alerta.TratamentoErroComLinha("EncarregadoController.cs", "PegaFotoModal", error);
                 return new JsonResult(new
                 {
@@ -282,7 +275,6 @@
             }
             catch (Exception error)
             {
-                _logService.Error(error.Message, error, "EncarregadoController.cs", "GetImage");
                 Alerta.TratamentoErroComLinha("EncarregadoController.cs", "GetImage", error);
                 return default(byte[]);
             }
@@ -319,7 +311,6 @@
             }
             catch (Exception error)
             {
-                _logService.Error(error.Message, error, "EncarregadoController.cs", "EncarregadoContratos");
                 Alerta.TratamentoErroComLinha("EncarregadoController.cs", "EncarregadoContratos", error);
                 return Json(new
                 {
@@ -381,7 +372,6 @@
             }
             catch (Exception error)
             {
-                _logService.Error(error.Message, error, "EncarregadoController.cs", "DeleteContrato");
                 Alerta.TratamentoErroComLinha("EncarregadoController.cs", "DeleteContrato", error);
                 return Json(new
                 {
```

### REMOVER do Janeiro

```csharp
using FrotiX.Helpers;
using FrotiX.Services;
        private readonly ILogService _logService;
        public EncarregadoController(IUnitOfWork unitOfWork, ILogService logService)
                _logService = logService;
                _logService.Error(error.Message, error, "EncarregadoController.cs", "Get");
                _logService.Error(error.Message, error, "EncarregadoController.cs", "Delete");
                _logService.Error(error.Message, error, "EncarregadoController.cs", "UpdateStatusEncarregado");
                _logService.Error(error.Message, error, "EncarregadoController.cs", "PegaFoto");
                _logService.Error(error.Message, error, "EncarregadoController.cs", "PegaFotoModal");
                _logService.Error(error.Message, error, "EncarregadoController.cs", "GetImage");
                _logService.Error(error.Message, error, "EncarregadoController.cs", "EncarregadoContratos");
                _logService.Error(error.Message, error, "EncarregadoController.cs", "DeleteContrato");
```


### ADICIONAR ao Janeiro

```csharp
using FrotiX.Helpers;
        public EncarregadoController(IUnitOfWork unitOfWork)
```
