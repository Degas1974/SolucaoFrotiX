# Controllers/UnidadeController.cs

**Mudanca:** GRANDE | **+50** linhas | **-62** linhas

---

```diff
--- JANEIRO: Controllers/UnidadeController.cs
+++ ATUAL: Controllers/UnidadeController.cs
@@ -1,7 +1,6 @@
 using AspNetCoreHero.ToastNotification.Abstractions;
 using FrotiX.Models;
 using FrotiX.Repository.IRepository;
-using FrotiX.Services;
 using Microsoft.AspNetCore.Mvc;
 using System;
 using System.Linq;
@@ -11,23 +10,21 @@
 
     [Route("api/[controller]")]
     [ApiController]
-    public class UnidadeController : Controller
+    public class UnidadeController :Controller
     {
         private readonly IUnitOfWork _unitOfWork;
         private readonly INotyfService _notyf;
-        private readonly ILogService _log;
-
-        public UnidadeController(IUnitOfWork unitOfWork, INotyfService notyf, ILogService log)
+
+        public UnidadeController(IUnitOfWork unitOfWork , INotyfService notyf)
         {
             try
             {
                 _unitOfWork = unitOfWork;
                 _notyf = notyf;
-                _log = log;
-            }
-            catch (Exception error)
-            {
-                Alerta.TratamentoErroComLinha("UnidadeController.cs", "UnidadeController", error);
+            }
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha("UnidadeController.cs" , "UnidadeController" , error);
             }
         }
 
@@ -36,16 +33,14 @@
         {
             try
             {
-
-                var data = _unitOfWork.Unidade.GetAll();
-
-                return Json(new { data });
-            }
-            catch (Exception error)
-            {
-                _log.Error("Erro", error, "UnidadeController", "Get");
+                return Json(new
+                {
+                    data = _unitOfWork.Unidade.GetAll()
+                });
+            }
+            catch (Exception error)
+            {
                 Alerta.TratamentoErroComLinha("UnidadeController.cs" , "Get" , error);
-
                 return Json(new
                 {
                     success = false ,
@@ -60,24 +55,18 @@
         {
             try
             {
-
                 if (model != null && model.UnidadeId != Guid.Empty)
                 {
-
                     var objFromDb = _unitOfWork.Unidade.GetFirstOrDefault(u =>
                         u.UnidadeId == model.UnidadeId
                     );
-
                     if (objFromDb != null)
                     {
-
                         var veiculo = _unitOfWork.Veiculo.GetFirstOrDefault(u =>
                             u.UnidadeId == model.UnidadeId
                         );
-
                         if (veiculo != null)
                         {
-
                             return Json(
                                 new
                                 {
@@ -86,12 +75,8 @@
                                 }
                             );
                         }
-
                         _unitOfWork.Unidade.Remove(objFromDb);
                         _unitOfWork.Save();
-
-                        _log.Info($"Unidade removida com sucesso: {objFromDb.Descricao} (ID: {model.UnidadeId})", "UnidadeController", "Delete");
-
                         return Json(
                             new
                             {
@@ -101,7 +86,6 @@
                         );
                     }
                 }
-
                 return Json(new
                 {
                     success = false ,
@@ -110,9 +94,7 @@
             }
             catch (Exception error)
             {
-                _log.Error("Erro", error, "UnidadeController", "Delete");
                 Alerta.TratamentoErroComLinha("UnidadeController.cs" , "Delete" , error);
-
                 return Json(new
                 {
                     success = false ,
@@ -126,35 +108,43 @@
         {
             try
             {
-
                 if (Id != Guid.Empty)
                 {
-
                     var objFromDb = _unitOfWork.Unidade.GetFirstOrDefault(u => u.UnidadeId == Id);
+                    string Description = "";
                     int type = 0;
 
                     if (objFromDb != null)
                     {
-
-                        objFromDb.Status = !objFromDb.Status;
-                        type = objFromDb.Status ? 0 : 1;
-
+                        if (objFromDb.Status == true)
+                        {
+                            objFromDb.Status = false;
+                            Description = string.Format(
+                                "Atualizado Status da Unidade [Nome: {0}] (Inativo)" ,
+                                objFromDb.Descricao
+                            );
+                            type = 1;
+                        }
+                        else
+                        {
+                            objFromDb.Status = true;
+                            Description = string.Format(
+                                "Atualizado Status da Unidade [Nome: {0}] (Ativo)" ,
+                                objFromDb.Descricao
+                            );
+                            type = 0;
+                        }
                         _unitOfWork.Unidade.Update(objFromDb);
-                        _unitOfWork.Save();
-
-                        string statusMsg = objFromDb.Status ? "Ativo" : "Inativo";
-                        _log.Info($"Status da Unidade atualizado para {statusMsg}: {objFromDb.Descricao} (ID: {Id})", "UnidadeController", "UpdateStatus");
-                    }
-
+                    }
                     return Json(
                         new
                         {
                             success = true ,
+
                             type = type ,
                         }
                     );
                 }
-
                 return Json(new
                 {
                     success = false
@@ -162,9 +152,7 @@
             }
             catch (Exception error)
             {
-                _log.Error("Erro", error, "UnidadeController", "UpdateStatus");
                 Alerta.TratamentoErroComLinha("UnidadeController.cs" , "UpdateStatus" , error);
-
                 return new JsonResult(new
                 {
                     success = false
@@ -178,12 +166,10 @@
         {
             try
             {
-
                 var result = _unitOfWork.ViewLotacaoMotorista.GetAll(lm => lm.MotoristaId == Guid.Empty);
 
                 if (motoristaId != null)
                 {
-
                     result = _unitOfWork.ViewLotacaoMotorista.GetAll(lm =>
                         lm.MotoristaId == Guid.Parse(motoristaId)
                     );
@@ -196,7 +182,6 @@
             }
             catch (Exception error)
             {
-                _log.Error("Erro", error, "UnidadeController", "ListaLotacao");
                 Alerta.TratamentoErroComLinha("UnidadeController.cs" , "ListaLotacao" , error);
                 return Json(new
                 {
@@ -219,16 +204,13 @@
         {
             try
             {
-
                 var existeLotacao = _unitOfWork.LotacaoMotorista.GetFirstOrDefault(lm =>
                     (lm.MotoristaId == Guid.Parse(MotoristaId))
                     && (lm.UnidadeId == Guid.Parse(UnidadeId))
                     && lm.DataInicio.ToString() == DataInicio
                 );
-
                 if (existeLotacao != null)
                 {
-
                     _notyf.Error("Já existe uma lotação com essas informações!" , 3);
                     return new JsonResult(new
                     {
@@ -257,8 +239,6 @@
 
                 _unitOfWork.Save();
 
-                _log.Info($"Motorista {MotoristaId} lotado na Unidade {UnidadeId} com sucesso.", "UnidadeController", "LotaMotorista");
-
                 return new JsonResult(
                     new
                     {
@@ -270,7 +250,6 @@
             }
             catch (Exception error)
             {
-                _log.Error("Erro", error, "UnidadeController", "LotaMotorista");
                 Alerta.TratamentoErroComLinha("UnidadeController.cs" , "LotaMotorista" , error);
                 return Json(new
                 {
@@ -294,15 +273,12 @@
         {
             try
             {
-
                 var objLotacaoMotorista = _unitOfWork.LotacaoMotorista.GetFirstOrDefault(lm =>
                     (lm.LotacaoMotoristaId == Guid.Parse(LotacaoId))
                 );
-
                 objLotacaoMotorista.MotoristaId = Guid.Parse(MotoristaId);
                 objLotacaoMotorista.UnidadeId = Guid.Parse(UnidadeId);
                 objLotacaoMotorista.DataInicio = DateTime.Parse(DataInicio);
-
                 if (DataFim != null)
                 {
                     objLotacaoMotorista.DataFim = DateTime.Parse(DataFim);
@@ -311,7 +287,6 @@
                 {
                     objLotacaoMotorista.DataFim = null;
                 }
-
                 objLotacaoMotorista.Lotado = Lotado;
                 objLotacaoMotorista.Motivo = Motivo;
                 _unitOfWork.LotacaoMotorista.Update(objLotacaoMotorista);
@@ -324,8 +299,6 @@
 
                 _unitOfWork.Save();
 
-                _log.Info($"Lotação {LotacaoId} editada com sucesso para o Motorista {MotoristaId}.", "UnidadeController", "EditaLotacao");
-
                 return new JsonResult(
                     new
                     {
@@ -336,7 +309,6 @@
             }
             catch (Exception error)
             {
-                _log.Error("Erro", error, "UnidadeController", "EditaLotacao");
                 Alerta.TratamentoErroComLinha("UnidadeController.cs" , "EditaLotacao" , error);
                 return Json(new
                 {
@@ -352,11 +324,9 @@
         {
             try
             {
-
                 var objFromDb = _unitOfWork.LotacaoMotorista.GetFirstOrDefault(u =>
                     u.LotacaoMotoristaId == Guid.Parse(Id)
                 );
-
                 var motoristaId = objFromDb.MotoristaId;
                 _unitOfWork.LotacaoMotorista.Remove(objFromDb);
                 _unitOfWork.Save();
@@ -366,9 +336,6 @@
                 );
                 obJMotorista.UnidadeId = Guid.Empty;
                 _unitOfWork.Motorista.Update(obJMotorista);
-                _unitOfWork.Save();
-
-                _log.Info($"Lotação {Id} removida para o Motorista {motoristaId}.", "UnidadeController", "DeleteLotacao");
 
                 return Json(
                     new
@@ -381,9 +348,7 @@
             }
             catch (Exception error)
             {
-                _log.Error("Erro", error, "UnidadeController", "DeleteLotacao");
                 Alerta.TratamentoErroComLinha("UnidadeController.cs" , "DeleteLotacao" , error);
-
                 return Json(new
                 {
                     success = false ,
@@ -407,7 +372,6 @@
             {
                 if (UnidadeNovaId == null)
                 {
-
                     var obJMotorista = _unitOfWork.Motorista.GetFirstOrDefault(m =>
                         m.MotoristaId == Guid.Parse(MotoristaId)
                     );
@@ -416,21 +380,13 @@
 
                     var obJLotacao = _unitOfWork.LotacaoMotorista.GetFirstOrDefault(lm =>
                         lm.UnidadeId == Guid.Parse(UnidadeAtualId)
-                        && lm.MotoristaId == Guid.Parse(MotoristaId)
-                        && lm.Lotado == true
-                    );
-
-                    if (obJLotacao != null)
-                    {
-
-                        obJLotacao.Lotado = false;
-                        obJLotacao.DataFim = DateTime.Parse(DataFimLotacaoAnterior);
-                        _unitOfWork.LotacaoMotorista.Update(obJLotacao);
-                    }
+                    );
+                    obJLotacao.Lotado = false;
+                    obJLotacao.DataFim = DateTime.Parse(DataFimLotacaoAnterior);
+                    _unitOfWork.LotacaoMotorista.Update(obJLotacao);
                 }
                 else if (UnidadeAtualId != UnidadeNovaId)
                 {
-
                     var obJMotorista = _unitOfWork.Motorista.GetFirstOrDefault(m =>
                         m.MotoristaId == Guid.Parse(MotoristaId)
                     );
@@ -439,17 +395,10 @@
 
                     var obJLotacao = _unitOfWork.LotacaoMotorista.GetFirstOrDefault(lm =>
                         lm.UnidadeId == Guid.Parse(UnidadeAtualId)
-                        && lm.MotoristaId == Guid.Parse(MotoristaId)
-                        && lm.Lotado == true
-                    );
-
-                    if (obJLotacao != null)
-                    {
-
-                        obJLotacao.Lotado = false;
-                        obJLotacao.DataFim = DateTime.Parse(DataFimLotacaoAnterior);
-                        _unitOfWork.LotacaoMotorista.Update(obJLotacao);
-                    }
+                    );
+                    obJLotacao.Lotado = false;
+                    obJLotacao.DataFim = DateTime.Parse(DataFimLotacaoAnterior);
+                    _unitOfWork.LotacaoMotorista.Update(obJLotacao);
 
                     var objLotacaoMotorista = new LotacaoMotorista();
                     objLotacaoMotorista.MotoristaId = Guid.Parse(MotoristaId);
@@ -462,8 +411,6 @@
 
                 _unitOfWork.Save();
 
-                _log.Info($"Lotação atualizada para o Motorista {MotoristaId}.", "UnidadeController", "AtualizaMotoristaLotacaoAtual");
-
                 return new JsonResult(
                     new
                     {
@@ -474,13 +421,11 @@
             }
             catch (Exception error)
             {
-                _log.Error("Erro", error, "UnidadeController", "AtualizaMotoristaLotacaoAtual");
                 Alerta.TratamentoErroComLinha(
                     "UnidadeController.cs" ,
                     "AtualizaMotoristaLotacaoAtual" ,
                     error
                 );
-
                 return Json(new
                 {
                     success = false ,
@@ -507,7 +452,6 @@
                 var objMotoristaAtual = _unitOfWork.LotacaoMotorista.GetFirstOrDefault(lm =>
                     (lm.MotoristaId == Guid.Parse(MotoristaId) && lm.Lotado == true)
                 );
-
                 if (objMotoristaAtual != null)
                 {
                     objMotoristaAtual.DataFim = DateTime.Parse(DataFimLotacao);
@@ -528,7 +472,7 @@
                 objMotoristaLotacaoNova.Motivo = "Férias";
                 if (MotoristaCoberturaId != null)
                 {
-                    objMotoristaLotacaoNova.MotoristaCoberturaId = Guid.Parse(MotoristaCoberturaId);
+                    objMotoristaAtual.MotoristaCoberturaId = Guid.Parse(MotoristaCoberturaId);
                 }
                 _unitOfWork.LotacaoMotorista.Add(objMotoristaLotacaoNova);
 
@@ -559,8 +503,6 @@
 
                 _unitOfWork.Save();
 
-                _log.Info($"Cobertura alocada para o Motorista {MotoristaId} com o Motorista {MotoristaCoberturaId}.", "UnidadeController", "AlocaMotoristaCobertura");
-
                 return new JsonResult(
                     new
                     {
@@ -571,13 +513,11 @@
             }
             catch (Exception error)
             {
-                _log.Error("Erro", error, "UnidadeController", "AlocaMotoristaCobertura");
                 Alerta.TratamentoErroComLinha(
                     "UnidadeController.cs" ,
                     "AlocaMotoristaCobertura" ,
                     error
                 );
-
                 return Json(new
                 {
                     success = false ,
@@ -592,7 +532,6 @@
         {
             try
             {
-
                 var result = _unitOfWork
                     .ViewLotacoes.GetAll()
                     .OrderBy(vl => vl.NomeCategoria)
@@ -601,7 +540,6 @@
 
                 if (categoriaId != null)
                 {
-
                     result = _unitOfWork
                         .ViewLotacoes.GetAll(vl => vl.NomeCategoria == categoriaId)
                         .OrderBy(O => O.NomeCategoria)
@@ -616,7 +554,6 @@
             }
             catch (Exception error)
             {
-                _log.Error("Erro", error, "UnidadeController", "ListaLotacoes");
                 Alerta.TratamentoErroComLinha("UnidadeController.cs" , "ListaLotacoes" , error);
                 return Json(new
                 {
@@ -630,26 +567,26 @@
         {
             try
             {
-
                 var lotacoesAnteriores = _unitOfWork.LotacaoMotorista.GetAll(lm =>
                     lm.MotoristaId == Guid.Parse(motoristaId)
                     && lm.Lotado == true
                 );
-
                 foreach (var lotacao in lotacoesAnteriores)
                 {
                     if (lotacao.LotacaoMotoristaId == lotacaoAtualId)
+                    {
                         continue;
-
-                    lotacao.Lotado = false;
-                    _unitOfWork.LotacaoMotorista.Update(lotacao);
-                }
-
+                    }
+                    else
+                    {
+                        lotacao.Lotado = false;
+                        _unitOfWork.LotacaoMotorista.Update(lotacao);
+                    }
+                }
                 _unitOfWork.Save();
             }
             catch (Exception error)
             {
-                _log.Error("Erro", error, "UnidadeController", "DesativarLotacoes");
                 Alerta.TratamentoErroComLinha("UnidadeController.cs" , "DesativarLotacoes" , error);
                 return;
             }
@@ -661,11 +598,7 @@
         {
             try
             {
-
                 DesativarLotacoes(motoristaId , lotacaoAtualId);
-
-                _log.Info($"Lotações anteriores removidas para o Motorista {motoristaId}.", "UnidadeController", "RemoveLotacoes");
-
                 return new JsonResult(new
                 {
                     success = true
@@ -673,9 +606,7 @@
             }
             catch (Exception error)
             {
-                _log.Error("Erro", error, "UnidadeController", "RemoveLotacoes");
                 Alerta.TratamentoErroComLinha("UnidadeController.cs" , "RemoveLotacoes" , error);
-
                 return Json(new
                 {
                     success = false ,
```
