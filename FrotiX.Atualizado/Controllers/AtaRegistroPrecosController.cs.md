# Controllers/AtaRegistroPrecosController.cs

**Mudanca:** GRANDE | **+81** linhas | **-89** linhas

---

```diff
--- JANEIRO: Controllers/AtaRegistroPrecosController.cs
+++ ATUAL: Controllers/AtaRegistroPrecosController.cs
@@ -4,29 +4,29 @@
 using System;
 using System.Collections.Generic;
 using System.Linq;
-using FrotiX.Helpers;
-using FrotiX.Services;
 
 namespace FrotiX.Controllers
 {
 
     [Route("api/[controller]")]
     [ApiController]
-    public partial class AtaRegistroPrecosController : ControllerBase
+    public partial class AtaRegistroPrecosController :ControllerBase
     {
         private readonly IUnitOfWork _unitOfWork;
-        private readonly ILogService _log;
-
-        public AtaRegistroPrecosController(IUnitOfWork unitOfWork, ILogService log)
+
+        public AtaRegistroPrecosController(IUnitOfWork unitOfWork)
         {
             try
             {
                 _unitOfWork = unitOfWork;
-                _log = log;
-            }
-            catch (Exception ex)
-            {
-                Alerta.TratamentoErroComLinha("AtaRegistroPrecosController.cs", "AtaRegistroPrecosController", ex);
+            }
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha(
+                    "AtaRegistroPrecosController.cs" ,
+                    "AtaRegistroPrecosController" ,
+                    error
+                );
             }
         }
 
@@ -35,7 +35,6 @@
         {
             try
             {
-
                 var result = (
                     from a in _unitOfWork.AtaRegistroPrecos.GetAll()
                     join f in _unitOfWork.Fornecedor.GetAll()
@@ -43,18 +42,19 @@
                     orderby a.AnoAta descending
                     select new
                     {
-                        AtaCompleta = a.AnoAta + "/" + a.NumeroAta,
+                        AtaCompleta = a.AnoAta + "/" + a.NumeroAta ,
                         ProcessoCompleto = a.NumeroProcesso
                             + "/"
-                            + a.AnoProcesso.ToString().Substring(2, 2),
-                        a.Objeto,
-                        f.DescricaoFornecedor,
+                            + a.AnoProcesso.ToString().Substring(2 , 2) ,
+                        a.Objeto ,
+                        f.DescricaoFornecedor ,
                         Periodo = a.DataInicio?.ToString("dd/MM/yy")
                             + " a "
-                            + a.DataFim?.ToString("dd/MM/yy"),
-                        ValorFormatado = a.Valor?.ToString("C"),
-                        a.Status,
-                        a.AtaId,
+                            + a.DataFim?.ToString("dd/MM/yy") ,
+                        ValorFormatado = a.Valor?.ToString("C") ,
+                        a.Status ,
+                        a.AtaId ,
+
                         depItens = _unitOfWork.ItemVeiculoAta.GetAll(i => i.RepactuacaoAta.AtaId == a.AtaId).Count(),
                         depVeiculos = _unitOfWork.VeiculoAta.GetAll(v => v.AtaId == a.AtaId).Count()
                     }
@@ -65,10 +65,9 @@
                     data = result
                 });
             }
-            catch (Exception ex)
-            {
-                _log.Error("[AtaRegistroPrecosController] Erro em Get: {ex.Message}", ex);
-                Alerta.TratamentoErroComLinha("AtaRegistroPrecosController.cs", "Get", ex);
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha("AtaRegistroPrecosController.cs" , "Get" , error);
                 return StatusCode(500);
             }
         }
@@ -79,16 +78,13 @@
         {
             try
             {
-
                 if (model != null && model.AtaId != Guid.Empty)
                 {
-
                     var objFromDb = _unitOfWork.AtaRegistroPrecos.GetFirstOrDefault(u =>
                         u.AtaId == model.AtaId
                     );
                     if (objFromDb != null)
                     {
-
                         var veiculo = _unitOfWork.VeiculoAta.GetFirstOrDefault(u =>
                             u.AtaId == model.AtaId
                         );
@@ -97,8 +93,8 @@
                             return Ok(
                                 new
                                 {
-                                    success = false,
-                                    message = "Existem veículos associados a essa Ata",
+                                    success = false ,
+                                    message = "Existem veículos associados a essa Ata" ,
                                 }
                             );
                         }
@@ -108,7 +104,6 @@
                         );
                         foreach (var repactuacao in objRepactuacao)
                         {
-
                             var objItemRepactuacao = _unitOfWork.ItemVeiculoAta.GetAll(iva =>
                                 iva.RepactuacaoAtaId == repactuacao.RepactuacaoAtaId
                             );
@@ -123,21 +118,20 @@
                         _unitOfWork.Save();
                         return Ok(new
                         {
-                            success = true,
+                            success = true ,
                             message = "Ata removida com sucesso"
                         });
                     }
                 }
                 return Ok(new
                 {
-                    success = false,
+                    success = false ,
                     message = "Erro ao apagar Ata"
                 });
             }
-            catch (Exception ex)
-            {
-                _log.Error("[AtaRegistroPrecosController] Erro em Delete: {ex.Message}", ex);
-                Alerta.TratamentoErroComLinha("AtaRegistroPrecosController.cs", "Delete", ex);
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha("AtaRegistroPrecosController.cs" , "Delete" , error);
                 return StatusCode(500);
             }
         }
@@ -148,10 +142,8 @@
         {
             try
             {
-
                 if (Id != Guid.Empty)
                 {
-
                     var objFromDb = _unitOfWork.AtaRegistroPrecos.GetFirstOrDefault(u =>
                         u.AtaId == Id
                     );
@@ -160,12 +152,11 @@
 
                     if (objFromDb != null)
                     {
-
                         if (objFromDb.Status == true)
                         {
                             objFromDb.Status = false;
                             Description = string.Format(
-                                "Atualizado Status da Ata [Número: {0}] (Inativo)",
+                                "Atualizado Status da Ata [Número: {0}] (Inativo)" ,
                                 objFromDb.AnoAta + "/" + objFromDb.NumeroAta
                             );
                             type = 1;
@@ -174,7 +165,7 @@
                         {
                             objFromDb.Status = true;
                             Description = string.Format(
-                                "Atualizado Status da Ata [Número: {0}] (Ativo)",
+                                "Atualizado Status da Ata [Número: {0}] (Ativo)" ,
                                 objFromDb.AnoAta + "/" + objFromDb.NumeroAta
                             );
                             type = 0;
@@ -186,9 +177,9 @@
                     return Ok(
                         new
                         {
-                            success = true,
-                            message = Description,
-                            type,
+                            success = true ,
+                            message = Description ,
+                            type ,
                         }
                     );
                 }
@@ -197,13 +188,12 @@
                     success = false
                 });
             }
-            catch (Exception ex)
-            {
-                _log.Error("[AtaRegistroPrecosController] Erro em UpdateStatusAta: {ex.Message}", ex);
-                Alerta.TratamentoErroComLinha(
-                    "AtaRegistroPrecosController.cs",
-                    "UpdateStatusAta",
-                    ex
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha(
+                    "AtaRegistroPrecosController.cs" ,
+                    "UpdateStatusAta" ,
+                    error
                 );
                 return StatusCode(500);
             }
@@ -223,9 +213,9 @@
                     return Ok(
                         new
                         {
-                            success = false,
-                            data = "00000000-0000-0000-0000-000000000000",
-                            message = "Já existe uma ata com esse número!",
+                            success = false ,
+                            data = "00000000-0000-0000-0000-000000000000" ,
+                            message = "Já existe uma ata com esse número!" ,
                         }
                     );
                 }
@@ -243,15 +233,14 @@
                 return Ok(
                     new
                     {
-                        data = objRepactuacao.RepactuacaoAtaId,
-                        message = "Ata Adicionada com Sucesso",
-                    }
-                );
-            }
-            catch (Exception ex)
-            {
-                _log.Error("[AtaRegistroPrecosController] Erro em InsereAta: {ex.Message}", ex);
-                Alerta.TratamentoErroComLinha("AtaRegistroPrecosController.cs", "InsereAta", ex);
+                        data = objRepactuacao.RepactuacaoAtaId ,
+                        message = "Ata Adicionada com Sucesso" ,
+                    }
+                );
+            }
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha("AtaRegistroPrecosController.cs" , "InsereAta" , error);
                 return StatusCode(500);
             }
         }
@@ -270,8 +259,8 @@
                     return Ok(
                         new
                         {
-                            data = "00000000-0000-0000-0000-000000000000",
-                            message = "Já existe uma Ata com esse número",
+                            data = "00000000-0000-0000-0000-000000000000" ,
+                            message = "Já existe uma Ata com esse número" ,
                         }
                     );
                 }
@@ -281,14 +270,13 @@
 
                 return Ok(new
                 {
-                    data = ata,
+                    data = ata ,
                     message = "Ata Atualizada com Sucesso"
                 });
             }
-            catch (Exception ex)
-            {
-                _log.Error("[AtaRegistroPrecosController] Erro em EditaAta: {ex.Message}", ex);
-                Alerta.TratamentoErroComLinha("AtaRegistroPrecosController.cs", "EditaAta", ex);
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha("AtaRegistroPrecosController.cs" , "EditaAta" , error);
                 return StatusCode(500);
             }
         }
@@ -305,18 +293,17 @@
                 return Ok(
                     new
                     {
-                        data = itemveiculo.ItemVeiculoAtaId,
-                        message = "Item Veiculo Ata adicionado com sucesso",
-                    }
-                );
-            }
-            catch (Exception ex)
-            {
-                _log.Error("[AtaRegistroPrecosController] Erro em InsereItemAta: {ex.Message}", ex);
-                Alerta.TratamentoErroComLinha(
-                    "AtaRegistroPrecosController.cs",
-                    "InsereItemAta",
-                    ex
+                        data = itemveiculo.ItemVeiculoAtaId ,
+                        message = "Item Veiculo Ata adicionado com sucesso" ,
+                    }
+                );
+            }
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha(
+                    "AtaRegistroPrecosController.cs" ,
+                    "InsereItemAta" ,
+                    error
                 );
                 return StatusCode(500);
             }
@@ -334,11 +321,11 @@
                     orderby r.DataRepactuacao
                     select new
                     {
-                        r.RepactuacaoAtaId,
+                        r.RepactuacaoAtaId ,
                         Repactuacao = "("
                             + r.DataRepactuacao?.ToString("dd/MM/yy")
                             + ") "
-                            + r.Descricao,
+                            + r.Descricao ,
                     }
                 ).ToList();
 
@@ -347,13 +334,12 @@
                     data = RepactuacoList
                 });
             }
-            catch (Exception ex)
-            {
-                _log.Error("[AtaRegistroPrecosController] Erro em RepactuacaoList: {ex.Message}", ex);
-                Alerta.TratamentoErroComLinha(
-                    "AtaRegistroPrecosController.cs",
-                    "RepactuacaoList",
-                    ex
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha(
+                    "AtaRegistroPrecosController.cs" ,
+                    "RepactuacaoList" ,
+                    error
                 );
                 return StatusCode(500);
             }
@@ -373,13 +359,12 @@
                     data = AtaList
                 });
             }
-            catch (Exception ex)
-            {
-                _log.Error("[AtaRegistroPrecosController] Erro em OnGetListaAtas: {ex.Message}", ex);
-                Alerta.TratamentoErroComLinha(
-                    "AtaRegistroPrecosController.cs",
-                    "OnGetListaAtas",
-                    ex
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha(
+                    "AtaRegistroPrecosController.cs" ,
+                    "OnGetListaAtas" ,
+                    error
                 );
                 return StatusCode(500);
             }
```
