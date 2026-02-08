# Controllers/UsuarioController.cs

**Mudanca:** GRANDE | **+63** linhas | **-55** linhas

---

```diff
--- JANEIRO: Controllers/UsuarioController.cs
+++ ATUAL: Controllers/UsuarioController.cs
@@ -1,6 +1,5 @@
 using FrotiX.Models;
 using FrotiX.Repository.IRepository;
-using FrotiX.Services;
 using Microsoft.AspNetCore.Mvc;
 using System;
 using System.Collections.Generic;
@@ -14,18 +13,16 @@
     public partial class UsuarioController : Controller
     {
         private readonly IUnitOfWork _unitOfWork;
-        private readonly ILogService _log;
-
-        public UsuarioController(IUnitOfWork unitOfWork, ILogService log)
+
+        public UsuarioController(IUnitOfWork unitOfWork)
         {
             try
             {
                 _unitOfWork = unitOfWork;
-                _log = log;
-            }
-            catch (Exception error)
-            {
-                Alerta.TratamentoErroComLinha("UsuarioController.cs", "UsuarioController", error);
+            }
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha("UsuarioController.cs" , "UsuarioController" , error);
             }
         }
 
@@ -34,9 +31,7 @@
         {
             try
             {
-
                 var usuarios = _unitOfWork.AspNetUsers.GetAll().ToList();
-
                 var result = new List<object>();
 
                 foreach (var u in usuarios)
@@ -102,9 +97,7 @@
             }
             catch (Exception error)
             {
-                _log.Error("Erro", error, "UsuarioController", "Get");
                 Alerta.TratamentoErroComLinha("UsuarioController.cs" , "Get" , error);
-
                 return Json(new
                 {
                     success = false ,
@@ -119,9 +112,7 @@
         {
             try
             {
-
                 var objFromDb = _unitOfWork.AspNetUsers.GetFirstOrDefault(u => u.Id == users.Id);
-
                 if (objFromDb == null)
                 {
                     return Json(new
@@ -179,7 +170,6 @@
                 if (vinculos.Any())
                 {
                     var mensagemVinculos = string.Join(", ", vinculos);
-
                     return Json(
                         new
                         {
@@ -197,8 +187,6 @@
                 _unitOfWork.AspNetUsers.Remove(objFromDb);
                 _unitOfWork.Save();
 
-                _log.Info($"Usuário removido com sucesso: {objFromDb.NomeCompleto} (ID: {users.Id})", "UsuarioController", "Delete");
-
                 return Json(new
                 {
                     success = true ,
@@ -207,9 +195,7 @@
             }
             catch (Exception error)
             {
-                _log.Error("Erro", error, "UsuarioController", "Delete");
                 Alerta.TratamentoErroComLinha("UsuarioController.cs" , "Delete" , error);
-
                 return Json(new
                 {
                     success = false ,
@@ -223,69 +209,71 @@
         {
             try
             {
-
                 if (Id != "")
                 {
-
-                    var objFromDb = _unitOfWork.AspNetUsers.GetFirstOrDefault(u => u.Id == Id);
-                    int type = 0;
-
-                    if (objFromDb != null)
-                    {
-
-                        objFromDb.Status = !(objFromDb.Status ?? false);
-
-                        type = (objFromDb.Status ?? false) ? 0 : 1;
-
-                        _unitOfWork.AspNetUsers.Update(objFromDb);
-                        _unitOfWork.Save();
-
-                        string statusMsg = (objFromDb.Status ?? false) ? "Ativo" : "Inativo";
-                        _log.Info($"Status do Usuário atualizado para {statusMsg}: {objFromDb.NomeCompleto} (ID: {Id})", "UsuarioController", "UpdateStatusUsuario");
-                    }
-
-                    return Json(
-                        new
-                        {
-                            success = true ,
-                            type = type ,
-                        }
-                    );
-                }
-
-                return Json(new
-                {
-                    success = false
-                });
-            }
-            catch (Exception error)
-            {
-                _log.Error("Erro", error, "UsuarioController", "UpdateStatusUsuario");
-                Alerta.TratamentoErroComLinha("UsuarioController.cs" , "UpdateStatusUsuario" , error);
-
-                return new JsonResult(new
-                {
-                    success = false
-                });
-            }
-        }
-
-        [Route("UpdateCargaPatrimonial")]
-        public JsonResult UpdateCargaPatrimonial(String Id)
-        {
-            try
-            {
-
-                if (Id != "")
-                {
-
                     var objFromDb = _unitOfWork.AspNetUsers.GetFirstOrDefault(u => u.Id == Id);
                     string Description = "";
                     int type = 0;
 
                     if (objFromDb != null)
                     {
-
+                        if (objFromDb.Status == true)
+                        {
+                            objFromDb.Status = false;
+                            Description = string.Format(
+                                "Atualizado Status do Usuário [Nome: {0}] (Inativo)" ,
+                                objFromDb.NomeCompleto
+                            );
+                            type = 1;
+                        }
+                        else
+                        {
+                            objFromDb.Status = true;
+                            Description = string.Format(
+                                "Atualizado Status do Usuário [Nome: {0}] (Ativo)" ,
+                                objFromDb.NomeCompleto
+                            );
+                            type = 0;
+                        }
+                        _unitOfWork.AspNetUsers.Update(objFromDb);
+                    }
+                    return Json(
+                        new
+                        {
+                            success = true ,
+                            message = Description ,
+                            type = type ,
+                        }
+                    );
+                }
+                return Json(new
+                {
+                    success = false
+                });
+            }
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha("UsuarioController.cs" , "UpdateStatusUsuario" , error);
+                return new JsonResult(new
+                {
+                    success = false
+                });
+            }
+        }
+
+        [Route("UpdateCargaPatrimonial")]
+        public JsonResult UpdateCargaPatrimonial(String Id)
+        {
+            try
+            {
+                if (Id != "")
+                {
+                    var objFromDb = _unitOfWork.AspNetUsers.GetFirstOrDefault(u => u.Id == Id);
+                    string Description = "";
+                    int type = 0;
+
+                    if (objFromDb != null)
+                    {
                         if (objFromDb.DetentorCargaPatrimonial == true)
                         {
                             objFromDb.DetentorCargaPatrimonial = false;
@@ -304,11 +292,8 @@
                             );
                             type = 0;
                         }
-
                         _unitOfWork.AspNetUsers.Update(objFromDb);
-                        _unitOfWork.Save();
-                    }
-
+                    }
                     return Json(
                         new
                         {
@@ -318,7 +303,6 @@
                         }
                     );
                 }
-
                 return Json(new
                 {
                     success = false
@@ -331,7 +315,6 @@
                     "UpdateCargaPatrimonial" ,
                     error
                 );
-
                 return new JsonResult(new
                 {
                     success = false
@@ -344,9 +327,9 @@
         {
             try
             {
-
                 string inputString = IDS;
                 char separator = '|';
+
                 string[] parts = inputString.Split(separator);
 
                 string usuarioId = parts[0];
@@ -360,7 +343,6 @@
 
                 if (objFromDb != null)
                 {
-
                     if (objFromDb.Acesso == true)
                     {
                         objFromDb.Acesso = false;
@@ -377,11 +359,9 @@
                         );
                         type = 0;
                     }
-
                     _unitOfWork.Save();
                     _unitOfWork.ControleAcesso.Update(objFromDb);
                 }
-
                 return Json(
                     new
                     {
@@ -394,7 +374,6 @@
             catch (Exception error)
             {
                 Alerta.TratamentoErroComLinha("UsuarioController.cs" , "UpdateStatusAcesso" , error);
-
                 return new JsonResult(new
                 {
                     success = false
@@ -408,7 +387,6 @@
         {
             try
             {
-
                 var objRecursos = _unitOfWork.ViewControleAcesso.GetAll(vca =>
                     vca.UsuarioId == UsuarioId
                 );
@@ -421,7 +399,6 @@
             catch (Exception error)
             {
                 Alerta.TratamentoErroComLinha("UsuarioController.cs" , "PegaRecursosUsuario" , error);
-
                 return Json(new
                 {
                     success = false ,
@@ -436,7 +413,6 @@
         {
             try
             {
-
                 var objRecursos = _unitOfWork
                     .ViewControleAcesso.GetAll(vca => vca.RecursoId == Guid.Parse(RecursoId))
                     .OrderBy(vca => vca.NomeCompleto);
@@ -449,7 +425,6 @@
             catch (Exception error)
             {
                 Alerta.TratamentoErroComLinha("UsuarioController.cs" , "PegaUsuariosRecurso" , error);
-
                 return Json(new
                 {
                     success = false ,
@@ -464,7 +439,6 @@
         {
             try
             {
-
                 var objUsuarios = (
                     from u in _unitOfWork.AspNetUsers.GetAll()
                     select new
@@ -481,10 +455,8 @@
 
                 foreach (var usuario in objUsuarios)
                 {
-
                     foreach (var recurso in objRecursos)
                     {
-
                         var objAcesso = new ControleAcesso();
 
                         objAcesso.UsuarioId = usuario.UsuarioId;
@@ -508,7 +480,6 @@
                     "InsereRecursosUsuario" ,
                     error
                 );
-
                 return Json(new
                 {
                     success = false ,
@@ -518,12 +489,12 @@
         }
 
         [HttpGet]
+
         [Route("listaUsuariosDetentores")]
         public IActionResult listaUsuariosDetentores()
         {
             try
             {
-
                 var result = (
                     from u in _unitOfWork.AspNetUsers.GetAll(u =>
                         u.DetentorCargaPatrimonial == true && u.Status == true
@@ -548,7 +519,6 @@
                     "listaUsuariosDetentores" ,
                     error
                 );
-
                 return Json(new
                 {
                     success = false ,
@@ -563,20 +533,16 @@
         {
             try
             {
-
                 var objRecursos = _unitOfWork.Recurso.GetFirstOrDefault(r =>
                     r.RecursoId == Guid.Parse(RecursoId)
                 );
-
                 if (objRecursos != null)
                 {
-
                     var objControleAcesso = _unitOfWork.ControleAcesso.GetFirstOrDefault(ca =>
                         ca.RecursoId == objRecursos.RecursoId
                     );
                     if (objControleAcesso != null)
                     {
-
                         return Json(
                             new
                             {
@@ -588,7 +554,6 @@
 
                     _unitOfWork.Recurso.Remove(objRecursos);
                     _unitOfWork.Save();
-
                     return Json(new
                     {
                         success = true ,
@@ -605,12 +570,12 @@
             catch (Exception error)
             {
                 Alerta.TratamentoErroComLinha("UsuarioController.cs" , "DeleteRecurso" , error);
-
                 return Json(new
                 {
                     success = false ,
                     message = "Erro ao deletar recurso"
                 });
             }
-        } }
+        }
+    }
 }
```
