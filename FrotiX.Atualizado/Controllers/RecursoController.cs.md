# Controllers/RecursoController.cs

**Mudanca:** MEDIA | **+4** linhas | **-13** linhas

---

```diff
--- JANEIRO: Controllers/RecursoController.cs
+++ ATUAL: Controllers/RecursoController.cs
@@ -3,29 +3,25 @@
 using Microsoft.AspNetCore.Mvc;
 using System;
 using System.Linq;
-using FrotiX.Helpers;
-using FrotiX.Services;
 
 namespace FrotiX.Controllers
 {
 
     [Route("api/[controller]")]
     [ApiController]
-    public class RecursoController : Controller
+    public class RecursoController :Controller
     {
         private readonly IUnitOfWork _unitOfWork;
-        private readonly ILogService _log;
 
-        public RecursoController(IUnitOfWork unitOfWork, ILogService log)
+        public RecursoController(IUnitOfWork unitOfWork)
         {
             try
             {
                 _unitOfWork = unitOfWork;
-                _log = log;
             }
-            catch (Exception ex)
+            catch (Exception error)
             {
-                Alerta.TratamentoErroComLinha("RecursoController.cs", "Constructor", ex);
+                Alerta.TratamentoErroComLinha("RecursoController.cs" , "RecursoController" , error);
             }
         }
 
@@ -34,7 +30,6 @@
         {
             try
             {
-
                 var result = (
                     from r in _unitOfWork.Recurso.GetAll()
                     select new
@@ -54,7 +49,6 @@
             }
             catch (Exception error)
             {
-                _log.Error("RecursoController.Get", error);
                 Alerta.TratamentoErroComLinha("RecursoController.cs" , "Get" , error);
                 return Json(new
                 {
@@ -70,23 +64,18 @@
         {
             try
             {
-
                 if (model != null && model.RecursoId != Guid.Empty)
                 {
-
                     var objFromDb = _unitOfWork.Recurso.GetFirstOrDefault(r =>
                         r.RecursoId == model.RecursoId
                     );
                     if (objFromDb != null)
                     {
-
                         var objControleAcesso = _unitOfWork.ControleAcesso.GetFirstOrDefault(ca =>
                             ca.RecursoId == model.RecursoId
                         );
                         if (objControleAcesso != null)
                         {
-
-                            _log.Warning($"RecursoController.Delete: Tentativa de remoção bloqueada. Recurso em uso: {objFromDb.Nome}");
                             return Json(
                                 new
                                 {
@@ -96,11 +85,8 @@
                             );
                         }
 
-                        string nomeRecurso = objFromDb.Nome;
                         _unitOfWork.Recurso.Remove(objFromDb);
                         _unitOfWork.Save();
-
-                        _log.Info($"RecursoController.Delete: Recurso ({nomeRecurso}) removido com sucesso.");
                         return Json(
                             new
                             {
@@ -110,7 +96,6 @@
                         );
                     }
                 }
-
                 return Json(new
                 {
                     success = false ,
@@ -119,7 +104,6 @@
             }
             catch (Exception error)
             {
-                _log.Error("RecursoController.Delete", error);
                 Alerta.TratamentoErroComLinha("RecursoController.cs" , "Delete" , error);
                 return Json(new
                 {
```

### REMOVER do Janeiro

```csharp
using FrotiX.Helpers;
using FrotiX.Services;
    public class RecursoController : Controller
        private readonly ILogService _log;
        public RecursoController(IUnitOfWork unitOfWork, ILogService log)
                _log = log;
            catch (Exception ex)
                Alerta.TratamentoErroComLinha("RecursoController.cs", "Constructor", ex);
                _log.Error("RecursoController.Get", error);
                            _log.Warning($"RecursoController.Delete: Tentativa de remoção bloqueada. Recurso em uso: {objFromDb.Nome}");
                        string nomeRecurso = objFromDb.Nome;
                        _log.Info($"RecursoController.Delete: Recurso ({nomeRecurso}) removido com sucesso.");
                _log.Error("RecursoController.Delete", error);
```


### ADICIONAR ao Janeiro

```csharp
    public class RecursoController :Controller
        public RecursoController(IUnitOfWork unitOfWork)
            catch (Exception error)
                Alerta.TratamentoErroComLinha("RecursoController.cs" , "RecursoController" , error);
```
