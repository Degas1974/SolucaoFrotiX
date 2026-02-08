# Controllers/MotoristaController.cs

**Mudanca:** GRANDE | **+196** linhas | **-94** linhas

---

```diff
--- JANEIRO: Controllers/MotoristaController.cs
+++ ATUAL: Controllers/MotoristaController.cs
@@ -1,6 +1,5 @@
 using FrotiX.Models;
 using FrotiX.Repository.IRepository;
-using FrotiX.Services;
 using Microsoft.AspNetCore.Mvc;
 using System;
 using System.Collections.Generic;
@@ -8,24 +7,25 @@
 
 namespace FrotiX.Controllers
 {
-
     [Route("api/[controller]")]
     [ApiController]
-    public class MotoristaController : Controller
+    public class MotoristaController :Controller
     {
         private readonly IUnitOfWork _unitOfWork;
-        private readonly ILogService _log;
-
-        public MotoristaController(IUnitOfWork unitOfWork, ILogService log)
+
+        public MotoristaController(IUnitOfWork unitOfWork)
         {
             try
             {
                 _unitOfWork = unitOfWork;
-                _log = log;
-            }
-            catch (Exception error)
-            {
-                Alerta.TratamentoErroComLinha("MotoristaController.cs", "MotoristaController", error);
+            }
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha(
+                    "MotoristaController.cs" ,
+                    "MotoristaController" ,
+                    error
+                );
             }
         }
 
@@ -34,9 +34,9 @@
         {
             try
             {
-
                 var result = (
                     from vm in _unitOfWork.ViewMotoristas.GetAll()
+
                     select new
                     {
                         vm.MotoristaId ,
@@ -45,26 +45,42 @@
                         vm.CNH ,
                         vm.Celular01 ,
                         vm.CategoriaCNH ,
+
                         Sigla = vm.Sigla != null ? vm.Sigla : "" ,
+
                         ContratoMotorista = vm.AnoContrato != null
-                            ? (vm.AnoContrato + "/" + vm.NumeroContrato + " - " + vm.DescricaoFornecedor)
-                            : vm.TipoCondutor != null ? vm.TipoCondutor
-                            : "(sem contrato)" ,
+                            ? (
+                                vm.AnoContrato
+                                + "/"
+                                + vm.NumeroContrato
+                                + " - "
+                                + vm.DescricaoFornecedor
+                            )
+                        : vm.TipoCondutor != null ? vm.TipoCondutor
+                        : "(sem contrato)" ,
+
                         vm.Status ,
-                        DatadeAlteracao = vm.DataAlteracao?.ToString("dd/MM/yy") ?? string.Empty ,
+
+                        DatadeAlteracao = vm.DataAlteracao?.ToString("dd/MM/yy")
+                            ?? string.Empty ,
+
                         vm.NomeCompleto ,
+
                         vm.EfetivoFerista ,
+
                         vm.Foto ,
                     }
                 ).ToList();
 
-                return Json(new { data = result });
-            }
-            catch (Exception error)
-            {
-                _log.Error("Erro", error, "MotoristaController", "Get");
+                return Json(new
+                {
+                    data = result
+                });
+            }
+            catch (Exception error)
+            {
                 Alerta.TratamentoErroComLinha("MotoristaController.cs" , "Get" , error);
-                return Json(new { success = false, message = "Erro ao listar motoristas" });
+                return View();
             }
         }
 
@@ -74,37 +90,49 @@
         {
             try
             {
-
                 if (model != null && model.MotoristaId != Guid.Empty)
                 {
-
-                    var objFromDb = _unitOfWork.Motorista.GetFirstOrDefault(u => u.MotoristaId == model.MotoristaId);
+                    var objFromDb = _unitOfWork.Motorista.GetFirstOrDefault(u =>
+                        u.MotoristaId == model.MotoristaId
+                    );
                     if (objFromDb != null)
                     {
 
-                        var motoristaContrato = _unitOfWork.MotoristaContrato.GetFirstOrDefault(u => u.MotoristaId == model.MotoristaId);
+                        var motoristaContrato = _unitOfWork.MotoristaContrato.GetFirstOrDefault(u =>
+                            u.MotoristaId == model.MotoristaId
+                        );
                         if (motoristaContrato != null)
                         {
-
-                            return Json(new { success = false , message = "Não foi possível remover o motorista. Ele está associado a um ou mais contratos!" });
+                            return Json(
+                                new
+                                {
+                                    success = false ,
+                                    message = "Não foi possível remover o motorista. Ele está associado a um ou mais contratos!" ,
+                                }
+                            );
                         }
 
                         _unitOfWork.Motorista.Remove(objFromDb);
                         _unitOfWork.Save();
-
-                        _log.Warning($"Motorista removido. ID: {model.MotoristaId}, Nome: {objFromDb.Nome}", "MotoristaController", "Delete");
-
-                        return Json(new { success = true , message = "Motorista removido com sucesso" });
-                    }
-                }
-
-                return Json(new { success = false , message = "Erro ao apagar motorista" });
-            }
-            catch (Exception error)
-            {
-                _log.Error("Erro", error, "MotoristaController", "Delete");
+                        return Json(
+                            new
+                            {
+                                success = true ,
+                                message = "Motorista removido com sucesso"
+                            }
+                        );
+                    }
+                }
+                return Json(new
+                {
+                    success = false ,
+                    message = "Erro ao apagar motorista"
+                });
+            }
+            catch (Exception error)
+            {
                 Alerta.TratamentoErroComLinha("MotoristaController.cs" , "Delete" , error);
-                return Json(new { success = false, message = "Erro interno ao apagar motorista" });
+                return View();
             }
         }
 
@@ -113,37 +141,64 @@
         {
             try
             {
-
                 if (Id != Guid.Empty)
                 {
-
-                    var objFromDb = _unitOfWork.Motorista.GetFirstOrDefault(u => u.MotoristaId == Id);
+                    var objFromDb = _unitOfWork.Motorista.GetFirstOrDefault(u =>
+                        u.MotoristaId == Id
+                    );
                     string Description = "";
                     int type = 0;
 
                     if (objFromDb != null)
                     {
-
-                        objFromDb.Status = !objFromDb.Status;
-                        Description = string.Format("Atualizado Status do Motorista [Nome: {0}] ({1})", objFromDb.Nome, objFromDb.Status ? "Ativo" : "Inativo");
-                        type = objFromDb.Status ? 0 : 1;
+                        if (objFromDb.Status == true)
+                        {
+
+                            objFromDb.Status = false;
+                            Description = string.Format(
+                                "Atualizado Status do Motorista [Nome: {0}] (Inativo)" ,
+                                objFromDb.Nome
+                            );
+                            type = 1;
+                        }
+                        else
+                        {
+
+                            objFromDb.Status = true;
+                            Description = string.Format(
+                                "Atualizado Status do Motorista [Nome: {0}] (Ativo)" ,
+                                objFromDb.Nome
+                            );
+                            type = 0;
+                        }
 
                         _unitOfWork.Motorista.Update(objFromDb);
-                        _unitOfWork.Save();
-
-                        _log.Info($"Status do motorista alterado. ID: {Id}, Novo Status: {objFromDb.Status}", "MotoristaController", "UpdateStatusMotorista");
-                    }
-
-                    return Json(new { success = true , message = Description , type = type });
-                }
-
-                return Json(new { success = false });
-            }
-            catch (Exception error)
-            {
-                _log.Error("Erro", error, "MotoristaController", "UpdateStatusMotorista");
-                Alerta.TratamentoErroComLinha("MotoristaController.cs" , "UpdateStatusMotorista" , error);
-                return Json(new { success = false });
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
+                Alerta.TratamentoErroComLinha(
+                    "MotoristaController.cs" ,
+                    "UpdateStatusMotorista" ,
+                    error
+                );
+                return new JsonResult(new
+                {
+                    sucesso = false
+                });
             }
         }
 
@@ -153,26 +208,30 @@
         {
             try
             {
-
                 if (id != Guid.Empty)
                 {
-
-                    var objFromDb = _unitOfWork.Motorista.GetFirstOrDefault(u => u.MotoristaId == id);
-                    if (objFromDb?.Foto != null)
-                    {
-
+                    var objFromDb = _unitOfWork.Motorista.GetFirstOrDefault(u =>
+                        u.MotoristaId == id
+                    );
+                    if (objFromDb.Foto != null)
+                    {
                         objFromDb.Foto = this.GetImage(Convert.ToBase64String(objFromDb.Foto));
                         return Json(objFromDb);
                     }
-                }
-
-                return Json(false);
-            }
-            catch (Exception error)
-            {
-                _log.Error("Erro", error, "MotoristaController", "PegaFoto");
+                    return Json(false);
+                }
+                else
+                {
+                    return Json(false);
+                }
+            }
+            catch (Exception error)
+            {
                 Alerta.TratamentoErroComLinha("MotoristaController.cs" , "PegaFoto" , error);
-                return Json(false);
+                return new JsonResult(new
+                {
+                    sucesso = false
+                });
             }
         }
 
@@ -182,22 +241,21 @@
         {
             try
             {
-
                 var objFromDb = _unitOfWork.Motorista.GetFirstOrDefault(u => u.MotoristaId == id);
-                if (objFromDb?.Foto != null)
-                {
-
-                    var fotoBase64 = this.GetImage(Convert.ToBase64String(objFromDb.Foto));
-                    return Json(fotoBase64);
-                }
-
+                if (objFromDb.Foto != null)
+                {
+                    objFromDb.Foto = this.GetImage(Convert.ToBase64String(objFromDb.Foto));
+                    return Json(objFromDb.Foto);
+                }
                 return Json(false);
             }
             catch (Exception error)
             {
-                _log.Error("Erro", error, "MotoristaController", "PegaFotoModal");
                 Alerta.TratamentoErroComLinha("MotoristaController.cs" , "PegaFotoModal" , error);
-                return Json(false);
+                return new JsonResult(new
+                {
+                    sucesso = false
+                });
             }
         }
 
@@ -205,19 +263,17 @@
         {
             try
             {
+                byte[] bytes = null;
                 if (!string.IsNullOrEmpty(sBase64String))
                 {
-
-                    return Convert.FromBase64String(sBase64String);
-                }
-
-                return null;
-            }
-            catch (Exception error)
-            {
-                _log.Error("Erro", error, "MotoristaController", "GetImage");
+                    bytes = Convert.FromBase64String(sBase64String);
+                }
+                return bytes;
+            }
+            catch (Exception error)
+            {
                 Alerta.TratamentoErroComLinha("MotoristaController.cs" , "GetImage" , error);
-                return null;
+                return default(byte[]);
             }
         }
 
@@ -227,12 +283,14 @@
         {
             try
             {
-
                 var result = (
                     from vm in _unitOfWork.ViewMotoristas.GetAll()
+
                     join mc in _unitOfWork.MotoristaContrato.GetAll()
                         on vm.MotoristaId equals mc.MotoristaId
+
                     where mc.ContratoId == Id
+
                     select new
                     {
                         vm.MotoristaId ,
@@ -241,23 +299,41 @@
                         vm.CNH ,
                         vm.Celular01 ,
                         vm.CategoriaCNH ,
+
                         Sigla = vm.Sigla != null ? vm.Sigla : "" ,
+
                         ContratoMotorista = vm.AnoContrato != null
-                            ? (vm.AnoContrato + "/" + vm.NumeroContrato + " - " + vm.DescricaoFornecedor)
+                            ? (
+                                vm.AnoContrato
+                                + "/"
+                                + vm.NumeroContrato
+                                + " - "
+                                + vm.DescricaoFornecedor
+                            )
                             : "<b>(Veículo Próprio)</b>" ,
+
                         vm.Status ,
-                        DatadeAlteracao = vm.DataAlteracao?.ToString("dd/MM/yy") ?? string.Empty ,
+
+                        DatadeAlteracao = vm.DataAlteracao?.ToString("dd/MM/yy")
+                            ?? string.Empty ,
+
                         vm.NomeCompleto ,
                     }
                 ).ToList();
 
-                return Json(new { data = result });
-            }
-            catch (Exception error)
-            {
-                _log.Error("Erro", error, "MotoristaController", "MotoristaContratos");
-                Alerta.TratamentoErroComLinha("MotoristaController.cs" , "MotoristaContratos" , error);
-                return Json(new { success = false, message = "Erro ao listar contratos do motorista" });
+                return Json(new
+                {
+                    data = result
+                });
+            }
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha(
+                    "MotoristaController.cs" ,
+                    "MotoristaContratos" ,
+                    error
+                );
+                return View();
             }
         }
 
@@ -267,11 +343,11 @@
         {
             try
             {
-
                 if (model != null && model.MotoristaId != Guid.Empty)
                 {
-
-                    var objFromDb = _unitOfWork.Motorista.GetFirstOrDefault(u => u.MotoristaId == model.MotoristaId);
+                    var objFromDb = _unitOfWork.Motorista.GetFirstOrDefault(u =>
+                        u.MotoristaId == model.MotoristaId
+                    );
                     if (objFromDb != null)
                     {
 
@@ -282,28 +358,41 @@
                         {
                             if (objFromDb.ContratoId == model.ContratoId)
                             {
-
                                 objFromDb.ContratoId = Guid.Empty;
                                 _unitOfWork.Motorista.Update(objFromDb);
                             }
-
                             _unitOfWork.MotoristaContrato.Remove(motoristaContrato);
                             _unitOfWork.Save();
-
-                            _log.Warning($"Vínculo de contrato removido para o motorista. ID Motorista: {model.MotoristaId}, ID Contrato: {model.ContratoId}", "MotoristaController", "DeleteContrato");
-
-                            return Json(new { success = true , message = "Motorista removido com sucesso" });
-                        }
-                    }
-                }
-
-                return Json(new { success = false , message = "Erro ao remover motorista" });
-            }
-            catch (Exception error)
-            {
-                _log.Error("Erro", error, "MotoristaController", "DeleteContrato");
+                            return Json(
+                                new
+                                {
+                                    success = true ,
+                                    message = "Motorista removido com sucesso"
+                                }
+                            );
+                        }
+                        return Json(new
+                        {
+                            success = false ,
+                            message = "Erro ao remover motorista"
+                        });
+                    }
+                    return Json(new
+                    {
+                        success = false ,
+                        message = "Erro ao remover motorista"
+                    });
+                }
+                return Json(new
+                {
+                    success = false ,
+                    message = "Erro ao remover motorista"
+                });
+            }
+            catch (Exception error)
+            {
                 Alerta.TratamentoErroComLinha("MotoristaController.cs" , "DeleteContrato" , error);
-                return Json(new { success = false, message = "Erro interno ao remover contrato" });
+                return View();
             }
         }
     }
```
