# Controllers/RequisitanteController.cs

**Mudanca:** GRANDE | **+10** linhas | **-25** linhas

---

```diff
--- JANEIRO: Controllers/RequisitanteController.cs
+++ ATUAL: Controllers/RequisitanteController.cs
@@ -1,6 +1,5 @@
 using FrotiX.Models;
 using FrotiX.Repository.IRepository;
-using FrotiX.Services;
 using Microsoft.AspNetCore.Mvc;
 using System;
 using System.Collections.Generic;
@@ -14,18 +13,20 @@
     public class RequisitanteController : Controller
     {
         private readonly IUnitOfWork _unitOfWork;
-        private readonly ILogService _log;
-
-        public RequisitanteController(IUnitOfWork unitOfWork, ILogService log)
+
+        public RequisitanteController(IUnitOfWork unitOfWork)
         {
             try
             {
                 _unitOfWork = unitOfWork;
-                _log = log;
-            }
-            catch (Exception error)
-            {
-                Alerta.TratamentoErroComLinha("RequisitanteController.cs", "RequisitanteController", error);
+            }
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha(
+                    "RequisitanteController.cs" ,
+                    "RequisitanteController" ,
+                    error
+                );
             }
         }
 
@@ -34,7 +35,6 @@
         {
             try
             {
-
                 var result = (
                     from r in _unitOfWork.Requisitante.GetAll()
                     join s in _unitOfWork.SetorSolicitante.GetAll()
@@ -59,7 +59,6 @@
             catch (Exception error)
             {
                 Alerta.TratamentoErroComLinha("RequisitanteController.cs" , "Get" , error);
-                _log.Error("Erro ao listar requisitantes (DataTables legacy)", error);
                 return View();
             }
         }
@@ -70,7 +69,6 @@
         {
             try
             {
-
                 var result = (
                     from r in _unitOfWork.Requisitante.GetAll()
                     join s in _unitOfWork.SetorSolicitante.GetAll()
@@ -96,7 +94,6 @@
             catch (Exception error)
             {
                 Alerta.TratamentoErroComLinha("RequisitanteController.cs" , "GetAll" , error);
-                _log.Error("Erro ao listar requisitantes (API JSON)", error);
                 return Json(new { success = false , message = "Erro ao listar requisitantes" });
             }
         }
@@ -107,7 +104,6 @@
         {
             try
             {
-
                 if (string.IsNullOrEmpty(id) || !Guid.TryParse(id , out Guid guidId))
                 {
                     return Json(new { success = false , message = "ID inválido" });
@@ -138,7 +134,6 @@
             catch (Exception error)
             {
                 Alerta.TratamentoErroComLinha("RequisitanteController.cs" , "GetById" , error);
-                _log.Error($", errorErro ao buscar requisitante [ID: {id}]");
                 return Json(new { success = false , message = "Erro ao buscar requisitante" });
             }
         }
@@ -149,14 +144,12 @@
         {
             try
             {
-
                 if (model == null || string.IsNullOrEmpty(model.Nome))
                 {
                     return Json(new { success = false , message = "Nome é obrigatório" });
                 }
 
                 Requisitante requisitante;
-
                 bool isNew = string.IsNullOrEmpty(model.RequisitanteId) || model.RequisitanteId == Guid.Empty.ToString();
 
                 Guid setorId = Guid.Empty;
@@ -169,7 +162,6 @@
 
                 if (isNew)
                 {
-
                     requisitante = new Requisitante
                     {
                         RequisitanteId = Guid.NewGuid() ,
@@ -182,11 +174,9 @@
                         UsuarioIdAlteracao = usuarioId
                     };
                     _unitOfWork.Requisitante.Add(requisitante);
-                    _log.Info($"Criado novo requisitante: [Nome: {model.Nome}] [Ponto: {model.Ponto}]");
                 }
                 else
                 {
-
                     var id = Guid.Parse(model.RequisitanteId);
                     requisitante = _unitOfWork.Requisitante.GetFirstOrDefault(r => r.RequisitanteId == id);
 
@@ -204,7 +194,6 @@
                     requisitante.UsuarioIdAlteracao = usuarioId;
 
                     _unitOfWork.Requisitante.Update(requisitante);
-                    _log.Info($"Atualizado requisitante: [ID: {model.RequisitanteId}] [Nome: {model.Nome}]");
                 }
 
                 _unitOfWork.Save();
@@ -218,7 +207,6 @@
             catch (Exception error)
             {
                 Alerta.TratamentoErroComLinha("RequisitanteController.cs" , "Upsert" , error);
-                _log.Error($", errorErro ao realizar Upsert de requisitante [Nome: {model?.Nome}]");
                 var innerMsg = error.InnerException != null ? error.InnerException.Message : "";
                 return Json(new { success = false , message = $"Erro: {error.Message} | {innerMsg}" });
             }
@@ -230,7 +218,6 @@
         {
             try
             {
-
                 var setores = _unitOfWork.SetorSolicitante.GetAll()
                     .Where(s => s.Status)
                     .OrderBy(s => s.Nome)
@@ -246,7 +233,6 @@
             catch (Exception error)
             {
                 Alerta.TratamentoErroComLinha("RequisitanteController.cs" , "GetSetores" , error);
-                _log.Error("Erro ao buscar setores ativos", error);
                 return Json(new List<object>());
             }
         }
@@ -257,21 +243,15 @@
         {
             try
             {
-
                 if (model != null && model.RequisitanteId != Guid.Empty)
                 {
-
                     var objFromDb = _unitOfWork.Requisitante.GetFirstOrDefault(u =>
                         u.RequisitanteId == model.RequisitanteId
                     );
                     if (objFromDb != null)
                     {
-
-                        var nome = objFromDb.Nome;
                         _unitOfWork.Requisitante.Remove(objFromDb);
                         _unitOfWork.Save();
-                        _log.Info($"Requisitante removido: [ID: {model.RequisitanteId}] [Nome: {nome}]");
-
                         return Json(
                             new
                             {
@@ -281,7 +261,6 @@
                         );
                     }
                 }
-
                 return Json(new
                 {
                     success = false ,
@@ -291,7 +270,6 @@
             catch (Exception error)
             {
                 Alerta.TratamentoErroComLinha("RequisitanteController.cs" , "Delete" , error);
-                _log.Error($", errorErro ao deletar requisitante [ID: {model?.RequisitanteId}]");
                 return Json(new { success = false , message = "Erro ao deletar requisitante" });
             }
         }
@@ -302,7 +280,6 @@
         {
             try
             {
-
                 var todosSetores = _unitOfWork.SetorSolicitante.GetAll()
                     .Where(s => s.Status)
                     .ToList();
@@ -318,14 +295,12 @@
             catch (Exception error)
             {
                 Alerta.TratamentoErroComLinha("RequisitanteController.cs" , "GetSetoresHierarquia" , error);
-                _log.Error("Erro ao buscar hierarquia de setores", error);
                 return Json(new List<object>());
             }
         }
 
         private object MontarHierarquiaSetor(SetorSolicitante setor , List<SetorSolicitante> todosSetores)
         {
-
             var filhos = todosSetores
                 .Where(s => s.SetorPaiId == setor.SetorSolicitanteId)
                 .OrderBy(s => s.Nome)
@@ -363,7 +338,6 @@
         {
             try
             {
-
                 if (dto.RequisitanteId == Guid.Empty)
                 {
                     return Json(new
@@ -401,13 +375,10 @@
 
                 if (houveMudanca)
                 {
-
                     requisitante.DataAlteracao = DateTime.Now;
 
                     _unitOfWork.Requisitante.Update(requisitante);
                     _unitOfWork.Save();
-
-                    _log.Info($"Atualizado Ramal/Setor do requisitante: [ID: {dto.RequisitanteId}] [Nome: {requisitante.Nome}]");
 
                     return Json(new
                     {
@@ -425,7 +396,6 @@
             catch (Exception error)
             {
                 Alerta.TratamentoErroComLinha("RequisitanteController.cs" , "AtualizarRequisitanteRamalSetor" , error);
-                _log.Error($", errorErro ao atualizar ramal/setor do requisitante [ID: {dto?.RequisitanteId}]");
                 return Json(new
                 {
                     success = false ,
@@ -439,10 +409,8 @@
         {
             try
             {
-
                 if (Id != Guid.Empty)
                 {
-
                     var objFromDb = _unitOfWork.Requisitante.GetFirstOrDefault(u =>
                         u.RequisitanteId == Id
                     );
@@ -451,7 +419,6 @@
 
                     if (objFromDb != null)
                     {
-
                         if (objFromDb.Status == true)
                         {
                             objFromDb.Status = false;
@@ -471,10 +438,7 @@
                             type = 0;
                         }
                         _unitOfWork.Requisitante.Update(objFromDb);
-                        _unitOfWork.Save();
-                        _log.Info(Description);
-                    }
-
+                    }
                     return Json(
                         new
                         {
@@ -484,7 +448,6 @@
                         }
                     );
                 }
-
                 return Json(new
                 {
                     success = false
@@ -497,7 +460,6 @@
                     "UpdateStatusRequisitante" ,
                     error
                 );
-                _log.Error($", errorErro ao alternar status do requisitante [ID: {Id}]");
                 return new JsonResult(new
                 {
                     sucesso = false
```

### REMOVER do Janeiro

```csharp
using FrotiX.Services;
        private readonly ILogService _log;
        public RequisitanteController(IUnitOfWork unitOfWork, ILogService log)
                _log = log;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("RequisitanteController.cs", "RequisitanteController", error);
                _log.Error("Erro ao listar requisitantes (DataTables legacy)", error);
                _log.Error("Erro ao listar requisitantes (API JSON)", error);
                _log.Error($", errorErro ao buscar requisitante [ID: {id}]");
                    _log.Info($"Criado novo requisitante: [Nome: {model.Nome}] [Ponto: {model.Ponto}]");
                    _log.Info($"Atualizado requisitante: [ID: {model.RequisitanteId}] [Nome: {model.Nome}]");
                _log.Error($", errorErro ao realizar Upsert de requisitante [Nome: {model?.Nome}]");
                _log.Error("Erro ao buscar setores ativos", error);
                        var nome = objFromDb.Nome;
                        _log.Info($"Requisitante removido: [ID: {model.RequisitanteId}] [Nome: {nome}]");
                _log.Error($", errorErro ao deletar requisitante [ID: {model?.RequisitanteId}]");
                _log.Error("Erro ao buscar hierarquia de setores", error);
                    _log.Info($"Atualizado Ramal/Setor do requisitante: [ID: {dto.RequisitanteId}] [Nome: {requisitante.Nome}]");
                _log.Error($", errorErro ao atualizar ramal/setor do requisitante [ID: {dto?.RequisitanteId}]");
                        _unitOfWork.Save();
                        _log.Info(Description);
                    }
                _log.Error($", errorErro ao alternar status do requisitante [ID: {Id}]");
```


### ADICIONAR ao Janeiro

```csharp
        public RequisitanteController(IUnitOfWork unitOfWork)
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha(
                    "RequisitanteController.cs" ,
                    "RequisitanteController" ,
                    error
                );
                    }
```
