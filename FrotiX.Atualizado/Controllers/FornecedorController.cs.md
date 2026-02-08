# Controllers/FornecedorController.cs

**Mudanca:** GRANDE | **+29** linhas | **-32** linhas

---

```diff
--- JANEIRO: Controllers/FornecedorController.cs
+++ ATUAL: Controllers/FornecedorController.cs
@@ -1,32 +1,31 @@
+using FrotiX.Helpers;
+
 using FrotiX.Models;
 using FrotiX.Repository.IRepository;
-using FrotiX.Services;
-using FrotiX.Helpers;
 using Microsoft.AspNetCore.Mvc;
 using System;
 
 namespace FrotiX.Controllers
 {
-
     [Route("api/[controller]")]
     [ApiController]
-    public class FornecedorController : Controller
+    public class FornecedorController :Controller
     {
         private readonly IUnitOfWork _unitOfWork;
-        private readonly ILogService _logService;
 
-        public FornecedorController(IUnitOfWork unitOfWork, ILogService logService)
+        public FornecedorController(IUnitOfWork unitOfWork)
         {
             try
             {
                 _unitOfWork = unitOfWork;
-                _logService = logService;
             }
             catch (Exception error)
             {
-
-                Alerta.TratamentoErroComLinha("FornecedorController.cs", "FornecedorController", error);
-                throw;
+                Alerta.TratamentoErroComLinha(
+                    "FornecedorController.cs" ,
+                    "FornecedorController" ,
+                    error
+                );
             }
         }
 
@@ -35,7 +34,6 @@
         {
             try
             {
-
                 return Json(new
                 {
                     data = _unitOfWork.Fornecedor.GetAll()
@@ -43,9 +41,8 @@
             }
             catch (Exception error)
             {
-                _logService.Error(error.Message, error, "FornecedorController.cs", "Get");
-                Alerta.TratamentoErroComLinha("FornecedorController.cs", "Get", error);
-                return StatusCode(500, new { success = false, message = "Erro interno ao buscar fornecedores" });
+                Alerta.TratamentoErroComLinha("FornecedorController.cs" , "Get" , error);
+                return StatusCode(500);
             }
         }
 
@@ -55,10 +52,8 @@
         {
             try
             {
-
                 if (model != null && model.FornecedorId != Guid.Empty)
                 {
-
                     var objFromDb = _unitOfWork.Fornecedor.GetFirstOrDefault(u =>
                         u.FornecedorId == model.FornecedorId
                     );
@@ -73,8 +68,8 @@
                             return Json(
                                 new
                                 {
-                                    success = false,
-                                    message = "Existem contratos associados a esse fornecedor"
+                                    success = false ,
+                                    message = "Existem contratos associados a esse fornecedor" ,
                                 }
                             );
                         }
@@ -83,7 +78,7 @@
                         return Json(
                             new
                             {
-                                success = true,
+                                success = true ,
                                 message = "Fornecedor removido com sucesso"
                             }
                         );
@@ -91,28 +86,24 @@
                 }
                 return Json(new
                 {
-                    success = false,
-                    message = "Erro ao apagar Fornecedor: Fornecedor não encontrado ou ID inválido"
+                    success = false ,
+                    message = "Erro ao apagar Fornecedor"
                 });
             }
             catch (Exception error)
             {
-                _logService.Error(error.Message, error, "FornecedorController.cs", "Delete");
-                Alerta.TratamentoErroComLinha("FornecedorController.cs", "Delete", error);
-                return StatusCode(500, new { success = false, message = "Erro interno ao excluir fornecedor" });
+                Alerta.TratamentoErroComLinha("FornecedorController.cs" , "Delete" , error);
+                return StatusCode(500);
             }
         }
 
         [Route("UpdateStatusFornecedor")]
-        [HttpPost]
         public JsonResult UpdateStatusFornecedor(Guid Id)
         {
             try
             {
-
                 if (Id != Guid.Empty)
                 {
-
                     var objFromDb = _unitOfWork.Fornecedor.GetFirstOrDefault(u =>
                         u.FornecedorId == Id
                     );
@@ -121,12 +112,11 @@
 
                     if (objFromDb != null)
                     {
-
                         if (objFromDb.Status == true)
                         {
                             objFromDb.Status = false;
                             Description = string.Format(
-                                "Atualizado Status do Fornecedor [Nome: {0}] (Inativo)",
+                                "Atualizado Status do Fornecedor [Nome: {0}] (Inativo)" ,
                                 objFromDb.DescricaoFornecedor
                             );
                             type = 1;
@@ -135,37 +125,37 @@
                         {
                             objFromDb.Status = true;
                             Description = string.Format(
-                                "Atualizado Status do Fornecedor [Nome: {0}] (Ativo)",
+                                "Atualizado Status do Fornecedor [Nome: {0}] (Ativo)" ,
                                 objFromDb.DescricaoFornecedor
                             );
                             type = 0;
                         }
                         _unitOfWork.Fornecedor.Update(objFromDb);
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
                 return Json(new
                 {
-                    success = false,
-                    message = "ID inválido"
+                    success = false
                 });
             }
             catch (Exception error)
             {
-                _logService.Error(error.Message, error, "FornecedorController.cs", "UpdateStatusFornecedor");
-                Alerta.TratamentoErroComLinha("FornecedorController.cs", "UpdateStatusFornecedor", error);
+                Alerta.TratamentoErroComLinha(
+                    "FornecedorController.cs" ,
+                    "UpdateStatusFornecedor" ,
+                    error
+                );
                 return new JsonResult(new
                 {
-                    success = false,
-                    message = "Erro ao atualizar status"
+                    sucesso = false
                 });
             }
         }
```

### REMOVER do Janeiro

```csharp
using FrotiX.Services;
using FrotiX.Helpers;
    public class FornecedorController : Controller
        private readonly ILogService _logService;
        public FornecedorController(IUnitOfWork unitOfWork, ILogService logService)
                _logService = logService;
                Alerta.TratamentoErroComLinha("FornecedorController.cs", "FornecedorController", error);
                throw;
                _logService.Error(error.Message, error, "FornecedorController.cs", "Get");
                Alerta.TratamentoErroComLinha("FornecedorController.cs", "Get", error);
                return StatusCode(500, new { success = false, message = "Erro interno ao buscar fornecedores" });
                                    success = false,
                                    message = "Existem contratos associados a esse fornecedor"
                                success = true,
                    success = false,
                    message = "Erro ao apagar Fornecedor: Fornecedor não encontrado ou ID inválido"
                _logService.Error(error.Message, error, "FornecedorController.cs", "Delete");
                Alerta.TratamentoErroComLinha("FornecedorController.cs", "Delete", error);
                return StatusCode(500, new { success = false, message = "Erro interno ao excluir fornecedor" });
        [HttpPost]
                                "Atualizado Status do Fornecedor [Nome: {0}] (Inativo)",
                                "Atualizado Status do Fornecedor [Nome: {0}] (Ativo)",
                        _unitOfWork.Save();
                            success = true,
                            message = Description,
                            type = type,
                    success = false,
                    message = "ID inválido"
                _logService.Error(error.Message, error, "FornecedorController.cs", "UpdateStatusFornecedor");
                Alerta.TratamentoErroComLinha("FornecedorController.cs", "UpdateStatusFornecedor", error);
                    success = false,
                    message = "Erro ao atualizar status"
```


### ADICIONAR ao Janeiro

```csharp
using FrotiX.Helpers;
    public class FornecedorController :Controller
        public FornecedorController(IUnitOfWork unitOfWork)
                Alerta.TratamentoErroComLinha(
                    "FornecedorController.cs" ,
                    "FornecedorController" ,
                    error
                );
                Alerta.TratamentoErroComLinha("FornecedorController.cs" , "Get" , error);
                return StatusCode(500);
                                    success = false ,
                                    message = "Existem contratos associados a esse fornecedor" ,
                                success = true ,
                    success = false ,
                    message = "Erro ao apagar Fornecedor"
                Alerta.TratamentoErroComLinha("FornecedorController.cs" , "Delete" , error);
                return StatusCode(500);
                                "Atualizado Status do Fornecedor [Nome: {0}] (Inativo)" ,
                                "Atualizado Status do Fornecedor [Nome: {0}] (Ativo)" ,
                            success = true ,
                            message = Description ,
                            type = type ,
                    success = false
                Alerta.TratamentoErroComLinha(
                    "FornecedorController.cs" ,
                    "UpdateStatusFornecedor" ,
                    error
                );
                    sucesso = false
```
