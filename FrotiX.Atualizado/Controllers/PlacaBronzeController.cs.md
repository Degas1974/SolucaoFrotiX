# Controllers/PlacaBronzeController.cs

**Mudanca:** GRANDE | **+115** linhas | **-59** linhas

---

```diff
--- JANEIRO: Controllers/PlacaBronzeController.cs
+++ ATUAL: Controllers/PlacaBronzeController.cs
@@ -3,29 +3,29 @@
 using Microsoft.AspNetCore.Mvc;
 using System;
 using System.Linq;
-using FrotiX.Helpers;
-using FrotiX.Services;
 
 namespace FrotiX.Controllers
 {
 
     [Route("api/[controller]")]
     [ApiController]
-    public class PlacaBronzeController : Controller
+    public class PlacaBronzeController :Controller
     {
         private readonly IUnitOfWork _unitOfWork;
-        private readonly ILogService _log;
-
-        public PlacaBronzeController(IUnitOfWork unitOfWork, ILogService log)
+
+        public PlacaBronzeController(IUnitOfWork unitOfWork)
         {
             try
             {
                 _unitOfWork = unitOfWork;
-                _log = log;
-            }
-            catch (Exception ex)
-            {
-                Alerta.TratamentoErroComLinha("PlacaBronzeController.cs", "Constructor", ex);
+            }
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha(
+                    "PlacaBronzeController.cs" ,
+                    "PlacaBronzeController" ,
+                    error
+                );
             }
         }
 
@@ -34,7 +34,6 @@
         {
             try
             {
-
                 var result = (
                     from p in _unitOfWork.PlacaBronze.GetAll()
                     join v in _unitOfWork.Veiculo.GetAll()
@@ -43,20 +42,26 @@
                     from pbResult in pb.DefaultIfEmpty()
                     select new
                     {
-                        p.PlacaBronzeId,
-                        p.DescricaoPlaca,
-                        p.Status,
-                        PlacaVeiculo = pbResult != null ? pbResult.Placa : "",
+                        p.PlacaBronzeId ,
+                        p.DescricaoPlaca ,
+                        p.Status ,
+                        PlacaVeiculo = pbResult != null ? pbResult.Placa : "" ,
                     }
                 ).ToList();
 
-                return Json(new { data = result });
-            }
-            catch (Exception ex)
-            {
-                _log.Error("PlacaBronzeController.Get", ex);
-                Alerta.TratamentoErroComLinha("PlacaBronzeController.cs", "Get", ex);
-                return Json(new { success = false, message = "Erro ao carregar dados" });
+                return Json(new
+                {
+                    data = result
+                });
+            }
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha("PlacaBronzeController.cs" , "Get" , error);
+                return Json(new
+                {
+                    success = false ,
+                    message = "Erro ao carregar dados"
+                });
             }
         }
 
@@ -66,42 +71,51 @@
         {
             try
             {
-
                 if (model != null && model.PlacaBronzeId != Guid.Empty)
                 {
-
                     var objFromDb = _unitOfWork.PlacaBronze.GetFirstOrDefault(u =>
                         u.PlacaBronzeId == model.PlacaBronzeId
                     );
                     if (objFromDb != null)
                     {
-
                         var modelo = _unitOfWork.Veiculo.GetFirstOrDefault(u =>
                             u.PlacaBronzeId == model.PlacaBronzeId
                         );
                         if (modelo != null)
                         {
-
-                            _log.Warning($"PlacaBronzeController.Delete: Tentativa de remoção de placa com veículo vinculado ({objFromDb.DescricaoPlaca})");
-                            return Json(new { success = false, message = "Existem veículos associados a essa placa" });
-                        }
-
-                        string descricao = objFromDb.DescricaoPlaca;
+                            return Json(
+                                new
+                                {
+                                    success = false ,
+                                    message = "Existem veículos associados a essa placa" ,
+                                }
+                            );
+                        }
                         _unitOfWork.PlacaBronze.Remove(objFromDb);
                         _unitOfWork.Save();
-
-                        _log.Info($"PlacaBronzeController.Delete: Placa de Bronze ({descricao}) removida com sucesso.");
-                        return Json(new { success = true, message = "Placa de Bronze removida com sucesso" });
+                        return Json(
+                            new
+                            {
+                                success = true ,
+                                message = "Placa de Bronze removida com sucesso"
+                            }
+                        );
                     }
                 }
-
-                return Json(new { success = false, message = "Erro ao apagar placa de bronze" });
-            }
-            catch (Exception ex)
-            {
-                _log.Error("PlacaBronzeController.Delete", ex);
-                Alerta.TratamentoErroComLinha("PlacaBronzeController.cs", "Delete", ex);
-                return Json(new { success = false, message = "Erro ao deletar placa de bronze" });
+                return Json(new
+                {
+                    success = false ,
+                    message = "Erro ao apagar placa de bronze"
+                });
+            }
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha("PlacaBronzeController.cs" , "Delete" , error);
+                return Json(new
+                {
+                    success = false ,
+                    message = "Erro ao deletar placa de bronze"
+                });
             }
         }
 
@@ -110,10 +124,8 @@
         {
             try
             {
-
                 if (Id != Guid.Empty)
                 {
-
                     var objFromDb = _unitOfWork.PlacaBronze.GetFirstOrDefault(u =>
                         u.PlacaBronzeId == Id
                     );
@@ -124,35 +136,49 @@
                     {
                         if (objFromDb.Status == true)
                         {
-
                             objFromDb.Status = false;
-                            Description = string.Format("Atualizado Status da Placa [Nome: {0}] (Inativo)", objFromDb.DescricaoPlaca);
+                            Description = string.Format(
+                                "Atualizado Status da Placa [Nome: {0}] (Inativo)" ,
+                                objFromDb.DescricaoPlaca
+                            );
                             type = 1;
                         }
                         else
                         {
-
                             objFromDb.Status = true;
-                            Description = string.Format("Atualizado Status da Placa [Nome: {0}] (Ativo)", objFromDb.DescricaoPlaca);
+                            Description = string.Format(
+                                "Atualizado Status da Marca [Nome: {0}] (Ativo)" ,
+                                objFromDb.DescricaoPlaca
+                            );
                             type = 0;
                         }
-
                         _unitOfWork.PlacaBronze.Update(objFromDb);
-                        _unitOfWork.Save();
-
-                        _log.Info($"PlacaBronzeController.UpdateStatusPlacaBronze: {Description}");
-                    }
-
-                    return Json(new { success = true, message = Description, type = type });
+                    }
+                    return Json(
+                        new
+                        {
+                            success = true ,
+                            message = Description ,
+                            type = type ,
+                        }
+                    );
                 }
-
-                return Json(new { success = false });
-            }
-            catch (Exception ex)
-            {
-                _log.Error("PlacaBronzeController.UpdateStatusPlacaBronze", ex);
-                Alerta.TratamentoErroComLinha("PlacaBronzeController.cs", "UpdateStatusPlacaBronze", ex);
-                return Json(new { success = false });
+                return Json(new
+                {
+                    success = false
+                });
+            }
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha(
+                    "PlacaBronzeController.cs" ,
+                    "UpdateStatusPlacaBronze" ,
+                    error
+                );
+                return new JsonResult(new
+                {
+                    success = false
+                });
             }
         }
 
@@ -162,10 +188,8 @@
         {
             try
             {
-
                 if (model.PlacaBronzeId != Guid.Empty)
                 {
-
                     var objFromDb = _unitOfWork.Veiculo.GetFirstOrDefault(u =>
                         u.PlacaBronzeId == model.PlacaBronzeId
                     );
@@ -174,26 +198,36 @@
 
                     if (objFromDb != null)
                     {
-
                         objFromDb.PlacaBronzeId = Guid.Empty;
-                        Description = string.Format("Placa de Bronze desassociada com sucesso do veículo {0}!", objFromDb.Placa);
+                        Description = string.Format(
+                            "Placa de Bronze desassociada com sucesso!" ,
+                            objFromDb.Placa
+                        );
                         type = 1;
                         _unitOfWork.Veiculo.Update(objFromDb);
-                        _unitOfWork.Save();
-
-                        _log.Info($"PlacaBronzeController.Desvincula: {Description}");
-                    }
-
-                    return Json(new { success = true, message = Description, type = type });
+                    }
+                    return Json(
+                        new
+                        {
+                            success = true ,
+                            message = Description ,
+                            type = type ,
+                        }
+                    );
                 }
-
-                return Json(new { success = false });
-            }
-            catch (Exception ex)
-            {
-                _log.Error("PlacaBronzeController.Desvincula", ex);
-                Alerta.TratamentoErroComLinha("PlacaBronzeController.cs", "Desvincula", ex);
-                return Json(new { success = false, message = "Erro ao desvincular placa" });
+                return Json(new
+                {
+                    success = false
+                });
+            }
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha("PlacaBronzeController.cs" , "Desvincula" , error);
+                return Json(new
+                {
+                    success = false ,
+                    message = "Erro ao desvincular placa"
+                });
             }
         }
     }
```
