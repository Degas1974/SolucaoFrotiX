# Controllers/SetorSolicitanteController.cs

**Mudanca:** MEDIA | **+7** linhas | **-10** linhas

---

```diff
--- JANEIRO: Controllers/SetorSolicitanteController.cs
+++ ATUAL: Controllers/SetorSolicitanteController.cs
@@ -1,6 +1,5 @@
 using FrotiX.Models;
 using FrotiX.Repository.IRepository;
-using FrotiX.Services;
 using Microsoft.AspNetCore.Mvc;
 using System;
 
@@ -9,21 +8,23 @@
 
     [Route("api/[controller]")]
     [ApiController]
-    public partial class SetorSolicitanteController : Controller
+    public partial class SetorSolicitanteController :Controller
     {
         private readonly IUnitOfWork _unitOfWork;
-        private readonly ILogService _log;
 
-        public SetorSolicitanteController(IUnitOfWork unitOfWork, ILogService log)
+        public SetorSolicitanteController(IUnitOfWork unitOfWork)
         {
             try
             {
                 _unitOfWork = unitOfWork;
-                _log = log;
             }
             catch (Exception error)
             {
-                Alerta.TratamentoErroComLinha("SetorSolicitanteController.cs", "SetorSolicitanteController", error);
+                Alerta.TratamentoErroComLinha(
+                    "SetorSolicitanteController.cs" ,
+                    "SetorSolicitanteController" ,
+                    error
+                );
             }
         }
 
@@ -33,10 +34,8 @@
         {
             try
             {
-
                 if (model != null && model.SetorSolicitanteId != Guid.Empty)
                 {
-
                     var objFromDb = _unitOfWork.SetorSolicitante.GetFirstOrDefault(u =>
                         u.SetorSolicitanteId == model.SetorSolicitanteId
                     );
@@ -48,8 +47,6 @@
                         );
                         if (filhos != null)
                         {
-
-                            _log.Warning($"Tentativa de exclusão de setor solicitante com filhos: [Setor: {objFromDb.Nome}] [ID: {model.SetorSolicitanteId}]");
                             return Json(
                                 new
                                 {
@@ -59,11 +56,8 @@
                             );
                         }
 
-                        var nomeSetor = objFromDb.Nome;
                         _unitOfWork.SetorSolicitante.Remove(objFromDb);
                         _unitOfWork.Save();
-                        _log.Info($"Setor solicitante removido: [ID: {model.SetorSolicitanteId}] [Nome: {nomeSetor}]");
-
                         return Json(
                             new
                             {
@@ -73,7 +67,6 @@
                         );
                     }
                 }
-
                 return Json(new
                 {
                     success = false ,
@@ -83,7 +76,6 @@
             catch (Exception error)
             {
                 Alerta.TratamentoErroComLinha("SetorSolicitanteController.cs" , "Delete" , error);
-                _log.Error($", errorErro ao deletar setor solicitante [ID: {model?.SetorSolicitanteId}]");
                 return Json(new
                 {
                     success = false ,
```

### REMOVER do Janeiro

```csharp
using FrotiX.Services;
    public partial class SetorSolicitanteController : Controller
        private readonly ILogService _log;
        public SetorSolicitanteController(IUnitOfWork unitOfWork, ILogService log)
                _log = log;
                Alerta.TratamentoErroComLinha("SetorSolicitanteController.cs", "SetorSolicitanteController", error);
                            _log.Warning($"Tentativa de exclusão de setor solicitante com filhos: [Setor: {objFromDb.Nome}] [ID: {model.SetorSolicitanteId}]");
                        var nomeSetor = objFromDb.Nome;
                        _log.Info($"Setor solicitante removido: [ID: {model.SetorSolicitanteId}] [Nome: {nomeSetor}]");
                _log.Error($", errorErro ao deletar setor solicitante [ID: {model?.SetorSolicitanteId}]");
```


### ADICIONAR ao Janeiro

```csharp
    public partial class SetorSolicitanteController :Controller
        public SetorSolicitanteController(IUnitOfWork unitOfWork)
                Alerta.TratamentoErroComLinha(
                    "SetorSolicitanteController.cs" ,
                    "SetorSolicitanteController" ,
                    error
                );
```
