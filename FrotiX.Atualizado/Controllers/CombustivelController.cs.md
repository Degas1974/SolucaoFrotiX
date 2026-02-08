# Controllers/CombustivelController.cs

**Mudanca:** GRANDE | **+39** linhas | **-35** linhas

---

```diff
--- JANEIRO: Controllers/CombustivelController.cs
+++ ATUAL: Controllers/CombustivelController.cs
@@ -2,28 +2,28 @@
 using FrotiX.Repository.IRepository;
 using Microsoft.AspNetCore.Mvc;
 using System;
-using FrotiX.Helpers;
-using FrotiX.Services;
 
 namespace FrotiX.Controllers
 {
     [Route("api/[controller]")]
     [ApiController]
-    public class CombustivelController : Controller
+    public class CombustivelController :Controller
     {
         private readonly IUnitOfWork _unitOfWork;
-        private readonly ILogService _log;
 
-        public CombustivelController(IUnitOfWork unitOfWork, ILogService log)
+        public CombustivelController(IUnitOfWork unitOfWork)
         {
             try
             {
                 _unitOfWork = unitOfWork;
-                _log = log;
             }
-            catch (Exception ex)
+            catch (Exception error)
             {
-                Alerta.TratamentoErroComLinha("CombustivelController.cs", "CombustivelController", ex);
+                Alerta.TratamentoErroComLinha(
+                    "CombustivelController.cs" ,
+                    "CombustivelController" ,
+                    error
+                );
             }
         }
 
@@ -32,16 +32,14 @@
         {
             try
             {
-
                 return Json(new
                 {
                     data = _unitOfWork.Combustivel.GetAll()
                 });
             }
-            catch (Exception ex)
+            catch (Exception error)
             {
-                _log.Error("[CombustivelController] Erro em Get: {ex.Message}", ex);
-                Alerta.TratamentoErroComLinha("CombustivelController.cs", "Get", ex);
+                Alerta.TratamentoErroComLinha("CombustivelController.cs" , "Get" , error);
                 return StatusCode(500);
             }
         }
@@ -52,34 +50,33 @@
         {
             try
             {
-
                 if (model != null && model.CombustivelId != Guid.Empty)
                 {
-
-                    var objFromDb = _unitOfWork.Combustivel.GetFirstOrDefaultWithTracking(u =>
+                    var objFromDb = _unitOfWork.Combustivel.GetFirstOrDefault(u =>
                         u.CombustivelId == model.CombustivelId
                     );
                     if (objFromDb != null)
                     {
-
                         var veiculo = _unitOfWork.Veiculo.GetFirstOrDefault(u =>
                             u.CombustivelId == model.CombustivelId
                         );
                         if (veiculo != null)
                         {
-                            return Json(new
-                            {
-                                success = false,
-                                message = "Existem veículos associados a esse combustível",
-                            });
+                            return Json(
+                                new
+                                {
+                                    success = false ,
+                                    message = "Existem veículos associados a essa combustível" ,
+                                }
+                            );
                         }
                         _unitOfWork.Combustivel.Remove(objFromDb);
                         _unitOfWork.Save();
                         return Json(
                             new
                             {
-                                success = true,
-                                message = "Tipo de Combustível removido com sucesso",
+                                success = true ,
+                                message = "Tipo de Combustível removido com sucesso" ,
                             }
                         );
                     }
@@ -87,30 +84,26 @@
                 return Json(
                     new
                     {
-                        success = false,
+                        success = false ,
                         message = "Erro ao apagar Tipo de Combustível"
                     }
                 );
             }
-            catch (Exception ex)
+            catch (Exception error)
             {
-                _log.Error("[CombustivelController] Erro em Delete: {ex.Message}", ex);
-                Alerta.TratamentoErroComLinha("CombustivelController.cs", "Delete", ex);
+                Alerta.TratamentoErroComLinha("CombustivelController.cs" , "Delete" , error);
                 return StatusCode(500);
             }
         }
 
         [Route("UpdateStatusCombustivel")]
-        [HttpPost]
         public JsonResult UpdateStatusCombustivel(Guid Id)
         {
             try
             {
-
                 if (Id != Guid.Empty)
                 {
-
-                    var objFromDb = _unitOfWork.Combustivel.GetFirstOrDefaultWithTracking(u =>
+                    var objFromDb = _unitOfWork.Combustivel.GetFirstOrDefault(u =>
                         u.CombustivelId == Id
                     );
                     string Description = "";
@@ -118,12 +111,11 @@
 
                     if (objFromDb != null)
                     {
-
                         if (objFromDb.Status == true)
                         {
                             objFromDb.Status = false;
                             Description = string.Format(
-                                "Atualizado Status do Tipo de Combustível [Nome: {0}] (Inativo)",
+                                "Atualizado Status do Tipo de Combustível [Nome: {0}] (Inativo)" ,
                                 objFromDb.Descricao
                             );
                             type = 1;
@@ -132,20 +124,19 @@
                         {
                             objFromDb.Status = true;
                             Description = string.Format(
-                                "Atualizado Status do Tipo de Combustível [Nome: {0}] (Ativo)",
+                                "Atualizado Status do Tipo de Combustível [Nome: {0}] (Ativo)" ,
                                 objFromDb.Descricao
                             );
                             type = 0;
                         }
                         _unitOfWork.Combustivel.Update(objFromDb);
-                        _unitOfWork.Save();
                     }
                     return Json(
                         new
                         {
-                            success = true,
-                            message = Description,
-                            type = type,
+                            success = true ,
+                            message = Description ,
+                            type = type ,
                         }
                     );
                 }
@@ -154,11 +145,17 @@
                     success = false
                 });
             }
-            catch (Exception ex)
+            catch (Exception error)
             {
-                _log.Error("[CombustivelController] Erro em UpdateStatusCombustivel: {ex.Message}", ex);
-                Alerta.TratamentoErroComLinha("CombustivelController.cs", "UpdateStatusCombustivel", ex);
-                return new JsonResult(new { sucesso = false });
+                Alerta.TratamentoErroComLinha(
+                    "CombustivelController.cs" ,
+                    "UpdateStatusCombustivel" ,
+                    error
+                );
+                return new JsonResult(new
+                {
+                    sucesso = false
+                });
             }
         }
     }
```

### REMOVER do Janeiro

```csharp
using FrotiX.Helpers;
using FrotiX.Services;
    public class CombustivelController : Controller
        private readonly ILogService _log;
        public CombustivelController(IUnitOfWork unitOfWork, ILogService log)
                _log = log;
            catch (Exception ex)
                Alerta.TratamentoErroComLinha("CombustivelController.cs", "CombustivelController", ex);
            catch (Exception ex)
                _log.Error("[CombustivelController] Erro em Get: {ex.Message}", ex);
                Alerta.TratamentoErroComLinha("CombustivelController.cs", "Get", ex);
                    var objFromDb = _unitOfWork.Combustivel.GetFirstOrDefaultWithTracking(u =>
                            return Json(new
                            {
                                success = false,
                                message = "Existem veículos associados a esse combustível",
                            });
                                success = true,
                                message = "Tipo de Combustível removido com sucesso",
                        success = false,
            catch (Exception ex)
                _log.Error("[CombustivelController] Erro em Delete: {ex.Message}", ex);
                Alerta.TratamentoErroComLinha("CombustivelController.cs", "Delete", ex);
        [HttpPost]
                    var objFromDb = _unitOfWork.Combustivel.GetFirstOrDefaultWithTracking(u =>
                                "Atualizado Status do Tipo de Combustível [Nome: {0}] (Inativo)",
                                "Atualizado Status do Tipo de Combustível [Nome: {0}] (Ativo)",
                        _unitOfWork.Save();
                            success = true,
                            message = Description,
                            type = type,
            catch (Exception ex)
                _log.Error("[CombustivelController] Erro em UpdateStatusCombustivel: {ex.Message}", ex);
                Alerta.TratamentoErroComLinha("CombustivelController.cs", "UpdateStatusCombustivel", ex);
                return new JsonResult(new { sucesso = false });
```


### ADICIONAR ao Janeiro

```csharp
    public class CombustivelController :Controller
        public CombustivelController(IUnitOfWork unitOfWork)
            catch (Exception error)
                Alerta.TratamentoErroComLinha(
                    "CombustivelController.cs" ,
                    "CombustivelController" ,
                    error
                );
            catch (Exception error)
                Alerta.TratamentoErroComLinha("CombustivelController.cs" , "Get" , error);
                    var objFromDb = _unitOfWork.Combustivel.GetFirstOrDefault(u =>
                            return Json(
                                new
                                {
                                    success = false ,
                                    message = "Existem veículos associados a essa combustível" ,
                                }
                            );
                                success = true ,
                                message = "Tipo de Combustível removido com sucesso" ,
                        success = false ,
            catch (Exception error)
                Alerta.TratamentoErroComLinha("CombustivelController.cs" , "Delete" , error);
                    var objFromDb = _unitOfWork.Combustivel.GetFirstOrDefault(u =>
                                "Atualizado Status do Tipo de Combustível [Nome: {0}] (Inativo)" ,
                                "Atualizado Status do Tipo de Combustível [Nome: {0}] (Ativo)" ,
                            success = true ,
                            message = Description ,
                            type = type ,
            catch (Exception error)
                Alerta.TratamentoErroComLinha(
                    "CombustivelController.cs" ,
                    "UpdateStatusCombustivel" ,
                    error
                );
                return new JsonResult(new
                {
                    sucesso = false
                });
```
