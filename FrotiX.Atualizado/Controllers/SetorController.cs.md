# Controllers/SetorController.cs

**Mudanca:** MEDIA | **+9** linhas | **-20** linhas

---

```diff
--- JANEIRO: Controllers/SetorController.cs
+++ ATUAL: Controllers/SetorController.cs
@@ -1,5 +1,4 @@
 using FrotiX.Repository.IRepository;
-using FrotiX.Services;
 using Microsoft.AspNetCore.Mvc;
 using System;
 using System.Collections.Generic;
@@ -13,18 +12,16 @@
     public class SetorController : Controller
     {
         private readonly IUnitOfWork _unitOfWork;
-        private readonly ILogService _log;
-
-        public SetorController(IUnitOfWork unitOfWork, ILogService log)
+
+        public SetorController(IUnitOfWork unitOfWork)
         {
             try
             {
                 _unitOfWork = unitOfWork;
-                _log = log;
-            }
-            catch (Exception error)
-            {
-                Alerta.TratamentoErroComLinha("SetorController.cs", "SetorController", error);
+            }
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha("SetorController.cs" , "SetorController" , error);
             }
         }
 
@@ -34,7 +31,6 @@
         {
             try
             {
-
                 var setores = _unitOfWork
                     .SetorPatrimonial.GetAll()
                     .Join(
@@ -61,7 +57,6 @@
             catch (Exception error)
             {
                 Alerta.TratamentoErroComLinha("SetorController.cs" , "ListaSetores" , error);
-                _log.Error("Erro ao listar setores patrimoniais", error);
                 return Json(
                     new
                     {
@@ -78,10 +73,8 @@
         {
             try
             {
-
                 if (Id != Guid.Empty)
                 {
-
                     var objFromDb = _unitOfWork.SetorPatrimonial.GetFirstOrDefault(u =>
                         u.SetorId == Id
                     );
@@ -89,7 +82,6 @@
                     int type = 0;
                     if (objFromDb != null)
                     {
-
                         if (objFromDb.Status == true)
                         {
                             objFromDb.Status = false;
@@ -110,9 +102,7 @@
                         }
                         _unitOfWork.SetorPatrimonial.Update(objFromDb);
                         _unitOfWork.Save();
-                        _log.Info(Description);
-                    }
-
+                    }
                     return Json(
                         new
                         {
@@ -122,7 +112,6 @@
                         }
                     );
                 }
-
                 return Json(new
                 {
                     success = false
@@ -131,11 +120,10 @@
             catch (Exception error)
             {
                 Alerta.TratamentoErroComLinha(
-                    "SetorController.cs" ,
+                    "SetorPatrimonialController.cs" ,
                     "UpdateStatusSetor" ,
                     error
                 );
-                _log.Error($", errorErro ao alternar status do setor [ID: {Id}]");
                 return new JsonResult(new
                 {
                     sucesso = false
@@ -149,23 +137,18 @@
         {
             try
             {
-
                 if (id != Guid.Empty)
                 {
-
                     var objFromDb = _unitOfWork.SetorPatrimonial.GetFirstOrDefault(u =>
                         u.SetorId == id
                     );
                     if (objFromDb != null)
                     {
-
                         var secao = _unitOfWork.SecaoPatrimonial.GetFirstOrDefault(u =>
                             u.SetorId == id
                         );
                         if (secao != null)
                         {
-
-                            _log.Warning($"Tentativa de exclusão de setor com dependências: [Setor: {objFromDb.NomeSetor}] [ID: {id}]");
                             return Json(
                                 new
                                 {
@@ -174,12 +157,8 @@
                                 }
                             );
                         }
-
-                        var nomeSetor = objFromDb.NomeSetor;
                         _unitOfWork.SetorPatrimonial.Remove(objFromDb);
                         _unitOfWork.Save();
-                        _log.Info($"Setor patrimonial removido: [ID: {id}] [Nome: {nomeSetor}]");
-
                         return Json(
                             new
                             {
@@ -189,7 +168,6 @@
                         );
                     }
                 }
-
                 return Json(new
                 {
                     success = false ,
@@ -198,8 +176,7 @@
             }
             catch (Exception error)
             {
-                Alerta.TratamentoErroComLinha("SetorController.cs" , "Delete" , error);
-                _log.Error($", errorErro ao deletar setor patrimonial [ID: {id}]");
+                Alerta.TratamentoErroComLinha("SetorPatrimonialController.cs" , "Delete" , error);
                 return StatusCode(500);
             }
         }
@@ -210,7 +187,6 @@
         {
             try
             {
-
                 var setores = _unitOfWork
                     .SetorPatrimonial.GetAll()
                     .Where(s => s.Status == true)
@@ -226,8 +202,7 @@
             }
             catch (Exception error)
             {
-                Alerta.TratamentoErroComLinha("SetorController.cs" , "ListaSetoresCombo" , error);
-                _log.Error("Erro ao carregar combo de setores", error);
+                Alerta.TratamentoErroComLinha("SetorController.cs" , "ListaSetores" , error);
                 return Json(
                     new
                     {
```

### REMOVER do Janeiro

```csharp
using FrotiX.Services;
        private readonly ILogService _log;
        public SetorController(IUnitOfWork unitOfWork, ILogService log)
                _log = log;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("SetorController.cs", "SetorController", error);
                _log.Error("Erro ao listar setores patrimoniais", error);
                        _log.Info(Description);
                    }
                    "SetorController.cs" ,
                _log.Error($", errorErro ao alternar status do setor [ID: {Id}]");
                            _log.Warning($"Tentativa de exclusão de setor com dependências: [Setor: {objFromDb.NomeSetor}] [ID: {id}]");
                        var nomeSetor = objFromDb.NomeSetor;
                        _log.Info($"Setor patrimonial removido: [ID: {id}] [Nome: {nomeSetor}]");
                Alerta.TratamentoErroComLinha("SetorController.cs" , "Delete" , error);
                _log.Error($", errorErro ao deletar setor patrimonial [ID: {id}]");
                Alerta.TratamentoErroComLinha("SetorController.cs" , "ListaSetoresCombo" , error);
                _log.Error("Erro ao carregar combo de setores", error);
```


### ADICIONAR ao Janeiro

```csharp
        public SetorController(IUnitOfWork unitOfWork)
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("SetorController.cs" , "SetorController" , error);
                    }
                    "SetorPatrimonialController.cs" ,
                Alerta.TratamentoErroComLinha("SetorPatrimonialController.cs" , "Delete" , error);
                Alerta.TratamentoErroComLinha("SetorController.cs" , "ListaSetores" , error);
```
