# Controllers/ContratoController.cs

**Mudanca:** GRANDE | **+502** linhas | **-360** linhas

---

```diff
--- JANEIRO: Controllers/ContratoController.cs
+++ ATUAL: Controllers/ContratoController.cs
@@ -8,31 +8,27 @@
 using System.Collections.Generic;
 using System.Linq;
 using System.Threading.Tasks;
-using FrotiX.Helpers;
-using FrotiX.Services;
 
 namespace FrotiX.Controllers
 {
     [Route("api/[controller]")]
     [ApiController]
     [IgnoreAntiforgeryToken]
-    public partial class ContratoController : Controller
+    public partial class ContratoController :Controller
     {
         private readonly IUnitOfWork _unitOfWork;
         private readonly FrotiXDbContext _db;
-        private readonly ILogService _log;
-
-        public ContratoController(IUnitOfWork unitOfWork, FrotiXDbContext db, ILogService logService)
+
+        public ContratoController(IUnitOfWork unitOfWork , FrotiXDbContext db)
         {
             try
             {
                 _unitOfWork = unitOfWork;
                 _db = db;
-                _log = logService;
-            }
-            catch (Exception ex)
-            {
-                Alerta.TratamentoErroComLinha("ContratoController.cs", "ContratoController", ex);
+            }
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha("ContratoController.cs" , "ContratoController" , error);
             }
         }
 
@@ -49,115 +45,107 @@
                     orderby c.AnoContrato descending
                     select new
                     {
-                        ContratoCompleto = c.AnoContrato + "/" + c.NumeroContrato,
+                        ContratoCompleto = c.AnoContrato + "/" + c.NumeroContrato ,
                         ProcessoCompleto = c.NumeroProcesso
                             + "/"
-                            + c.AnoProcesso.ToString().Substring(2, 2),
-                        c.Objeto,
-                        f.DescricaoFornecedor,
+                            + c.AnoProcesso.ToString().Substring(2 , 2) ,
+                        c.Objeto ,
+                        f.DescricaoFornecedor ,
                         Periodo = c.DataInicio?.ToString("dd/MM/yy")
                             + " a "
-                            + c.DataFim?.ToString("dd/MM/yy"),
-                        ValorFormatado = c.Valor?.ToString("C"),
-                        ValorMensal = (c.Valor / 12)?.ToString("C"),
+                            + c.DataFim?.ToString("dd/MM/yy") ,
+                        ValorFormatado = c.Valor?.ToString("C") ,
+                        ValorMensal = (c.Valor / 12)?.ToString("C") ,
                         VigenciaCompleta = c.Vigencia
                             + "ª vigência + "
                             + c.Prorrogacao
-                            + " prorrog.",
-                        c.Status,
-                        c.ContratoId,
+                            + " prorrog." ,
+                        c.Status ,
+                        c.ContratoId ,
                     }
                 ).ToList();
 
                 var contratoIds = contratos.Select(c => c.ContratoId).ToList();
 
-                var veiculosContrato = new Dictionary<Guid, int>();
-                var encarregados = new Dictionary<Guid, int>();
-                var operadores = new Dictionary<Guid, int>();
-                var lavadores = new Dictionary<Guid, int>();
-                var motoristas = new Dictionary<Guid, int>();
-                var empenhosDict = new Dictionary<Guid, int>();
-                var notasFiscais = new Dictionary<Guid, int>();
-                var repactuacoes = new Dictionary<Guid, int>();
-                var itensContrato = new Dictionary<Guid, int>();
+                var veiculosContrato = new Dictionary<Guid , int>();
+                var encarregados = new Dictionary<Guid , int>();
+                var operadores = new Dictionary<Guid , int>();
+                var lavadores = new Dictionary<Guid , int>();
+                var motoristas = new Dictionary<Guid , int>();
+                var empenhosDict = new Dictionary<Guid , int>();
+                var notasFiscais = new Dictionary<Guid , int>();
+                var repactuacoes = new Dictionary<Guid , int>();
+                var itensContrato = new Dictionary<Guid , int>();
 
                 try
                 {
-
                     veiculosContrato = _unitOfWork.VeiculoContrato
                         .GetAll(x => contratoIds.Contains(x.ContratoId))
                         .GroupBy(x => x.ContratoId)
-                        .ToDictionary(g => g.Key, g => g.Count());
+                        .ToDictionary(g => g.Key , g => g.Count());
                 }
                 catch { }
 
                 try
                 {
-
                     encarregados = _unitOfWork.Encarregado
                         .GetAll(x => contratoIds.Contains(x.ContratoId))
                         .GroupBy(x => x.ContratoId)
-                        .ToDictionary(g => g.Key, g => g.Count());
+                        .ToDictionary(g => g.Key , g => g.Count());
                 }
                 catch { }
 
                 try
                 {
-
                     operadores = _unitOfWork.Operador
                         .GetAll(x => contratoIds.Contains(x.ContratoId))
                         .GroupBy(x => x.ContratoId)
-                        .ToDictionary(g => g.Key, g => g.Count());
+                        .ToDictionary(g => g.Key , g => g.Count());
                 }
                 catch { }
 
                 try
                 {
-
                     lavadores = _unitOfWork.Lavador
                         .GetAll(x => x.ContratoId.HasValue && contratoIds.Contains(x.ContratoId.Value))
                         .GroupBy(x => x.ContratoId.Value)
-                        .ToDictionary(g => g.Key, g => g.Count());
+                        .ToDictionary(g => g.Key , g => g.Count());
                 }
                 catch { }
 
                 try
                 {
-
                     motoristas = _unitOfWork.Motorista
                         .GetAll(x => x.ContratoId.HasValue && contratoIds.Contains(x.ContratoId.Value))
                         .GroupBy(x => x.ContratoId.Value)
-                        .ToDictionary(g => g.Key, g => g.Count());
+                        .ToDictionary(g => g.Key , g => g.Count());
                 }
                 catch { }
 
                 try
                 {
-
                     empenhosDict = _unitOfWork.Empenho
                         .GetAll(x => x.ContratoId.HasValue && contratoIds.Contains(x.ContratoId.Value))
                         .GroupBy(x => x.ContratoId.Value)
-                        .ToDictionary(g => g.Key, g => g.Count());
+                        .ToDictionary(g => g.Key , g => g.Count());
                 }
                 catch { }
 
                 try
                 {
-
                     notasFiscais = _unitOfWork.NotaFiscal
                         .GetAll(x => x.ContratoId.HasValue && contratoIds.Contains(x.ContratoId.Value))
                         .GroupBy(x => x.ContratoId.Value)
-                        .ToDictionary(g => g.Key, g => g.Count());
+                        .ToDictionary(g => g.Key , g => g.Count());
                 }
                 catch { }
 
                 try
                 {
-
                     repactuacoes = _unitOfWork.RepactuacaoContrato
                         .GetAll(x => contratoIds.Contains(x.ContratoId))
                         .GroupBy(x => x.ContratoId)
-                        .ToDictionary(g => g.Key, g => g.Count());
+                        .ToDictionary(g => g.Key , g => g.Count());
                 }
                 catch { }
 
@@ -189,26 +177,26 @@
 
                 var result = contratos.Select(c => new
                 {
-                    c.ContratoCompleto,
-                    c.ProcessoCompleto,
-                    c.Objeto,
-                    c.DescricaoFornecedor,
-                    c.Periodo,
-                    c.ValorFormatado,
-                    c.ValorMensal,
-                    c.VigenciaCompleta,
-                    c.Status,
-                    c.ContratoId,
-
-                    DepVeiculos = veiculosContrato.GetValueOrDefault(c.ContratoId, 0),
-                    DepEncarregados = encarregados.GetValueOrDefault(c.ContratoId, 0),
-                    DepOperadores = operadores.GetValueOrDefault(c.ContratoId, 0),
-                    DepLavadores = lavadores.GetValueOrDefault(c.ContratoId, 0),
-                    DepMotoristas = motoristas.GetValueOrDefault(c.ContratoId, 0),
-                    DepEmpenhos = empenhosDict.GetValueOrDefault(c.ContratoId, 0),
-                    DepNotasFiscais = notasFiscais.GetValueOrDefault(c.ContratoId, 0),
-                    DepRepactuacoes = repactuacoes.GetValueOrDefault(c.ContratoId, 0),
-                    DepItensContrato = itensContrato.GetValueOrDefault(c.ContratoId, 0)
+                    c.ContratoCompleto ,
+                    c.ProcessoCompleto ,
+                    c.Objeto ,
+                    c.DescricaoFornecedor ,
+                    c.Periodo ,
+                    c.ValorFormatado ,
+                    c.ValorMensal ,
+                    c.VigenciaCompleta ,
+                    c.Status ,
+                    c.ContratoId ,
+
+                    DepVeiculos = veiculosContrato.GetValueOrDefault(c.ContratoId , 0) ,
+                    DepEncarregados = encarregados.GetValueOrDefault(c.ContratoId , 0) ,
+                    DepOperadores = operadores.GetValueOrDefault(c.ContratoId , 0) ,
+                    DepLavadores = lavadores.GetValueOrDefault(c.ContratoId , 0) ,
+                    DepMotoristas = motoristas.GetValueOrDefault(c.ContratoId , 0) ,
+                    DepEmpenhos = empenhosDict.GetValueOrDefault(c.ContratoId , 0) ,
+                    DepNotasFiscais = notasFiscais.GetValueOrDefault(c.ContratoId , 0) ,
+                    DepRepactuacoes = repactuacoes.GetValueOrDefault(c.ContratoId , 0) ,
+                    DepItensContrato = itensContrato.GetValueOrDefault(c.ContratoId , 0)
                 }).OrderByDescending(c => c.ContratoCompleto);
 
                 return Json(new
@@ -216,10 +204,9 @@
                     data = result
                 });
             }
-            catch (Exception ex)
-            {
-                _log.Error("[ContratoController] Erro em Get: {ex.Message}", ex);
-                Alerta.TratamentoErroComLinha("ContratoController.cs", "Get", ex);
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha("ContratoController.cs" , "Get" , error);
                 return StatusCode(500);
             }
         }
@@ -235,46 +222,44 @@
                     var objFromDb = _unitOfWork.Contrato.GetFirstOrDefault(u =>
                         u.ContratoId == model.ContratoId
                     );
-
                     if (objFromDb != null)
                     {
-
                         var veiculo = _unitOfWork.Veiculo.GetFirstOrDefault(v =>
                             v.ContratoId == model.ContratoId
                         );
-
                         if (veiculo != null)
                         {
-                            return Json(new
-                            {
-                                success = false,
-                                message = "Existem veículos associados a esse contrato",
-                            });
+                            return Json(
+                                new
+                                {
+                                    success = false ,
+                                    message = "Existem veículos associados a esse contrato" ,
+                                }
+                            );
                         }
 
                         var empenho = _unitOfWork.Empenho.GetFirstOrDefault(u =>
                             u.ContratoId == model.ContratoId
                         );
-
                         if (empenho != null)
                         {
-                            return Json(new
-                            {
-                                success = false,
-                                message = "Existem empenhos associados a esse contrato",
-                            });
+                            return Json(
+                                new
+                                {
+                                    success = false ,
+                                    message = "Existem empenhos associados a esse contrato" ,
+                                }
+                            );
                         }
 
                         var objRepactuacao = _unitOfWork.RepactuacaoContrato.GetAll(riv =>
                             riv.ContratoId == model.ContratoId
                         );
-
                         foreach (var repactuacao in objRepactuacao)
                         {
                             var objItemRepactuacao = _unitOfWork.ItemVeiculoContrato.GetAll(ivc =>
                                 ivc.RepactuacaoContratoId == repactuacao.RepactuacaoContratoId
                             );
-
                             foreach (var itemveiculo in objItemRepactuacao)
                             {
                                 _unitOfWork.ItemVeiculoContrato.Remove(itemveiculo);
@@ -284,31 +269,29 @@
 
                         _unitOfWork.Contrato.Remove(objFromDb);
                         _unitOfWork.Save();
-
-                        return Json(new
-                        {
-                            success = true,
-                            message = "Contrato removido com sucesso"
-                        });
-                    }
-                }
-
+                        return Json(
+                            new
+                            {
+                                success = true ,
+                                message = "Contrato removido com sucesso"
+                            }
+                        );
+                    }
+                }
                 return Json(new
                 {
-                    success = false,
+                    success = false ,
                     message = "Erro ao apagar Contrato"
                 });
             }
-            catch (Exception ex)
-            {
-                _log.Error("[ContratoController] Erro em Delete: {ex.Message}", ex);
-                Alerta.TratamentoErroComLinha("ContratoController.cs", "Delete", ex);
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha("ContratoController.cs" , "Delete" , error);
                 return StatusCode(500);
             }
         }
 
         [Route("UpdateStatusContrato")]
-        [HttpPost]
         public JsonResult UpdateStatusContrato(Guid Id)
         {
             try
@@ -316,75 +299,92 @@
                 if (Id != Guid.Empty)
                 {
                     var objFromDb = _unitOfWork.Contrato.GetFirstOrDefault(u => u.ContratoId == Id);
-                    string description = "";
+                    string Description = "";
                     int type = 0;
 
                     if (objFromDb != null)
                     {
-
                         if (objFromDb.Status == true)
                         {
                             objFromDb.Status = false;
-                            description = $"Atualizado Status do Contrato [Nome: {objFromDb.AnoContrato}/{objFromDb.NumeroContrato}] (Inativo)";
+                            Description = string.Format(
+                                "Atualizado Status do Contrato [Nome: {0}] (Inativo)" ,
+                                objFromDb.AnoContrato + "/" + objFromDb.NumeroContrato
+                            );
                             type = 1;
                         }
                         else
                         {
                             objFromDb.Status = true;
-                            description = $"Atualizado Status do Contrato [Nome: {objFromDb.AnoContrato}/{objFromDb.NumeroContrato}] (Ativo)";
+                            Description = string.Format(
+                                "Atualizado Status do Contrato [Nome: {0}] (Ativo)" ,
+                                objFromDb.AnoContrato + "/" + objFromDb.NumeroContrato
+                            );
                             type = 0;
                         }
-
                         _unitOfWork.Contrato.Update(objFromDb);
-                        _unitOfWork.Save();
-                    }
-
-                    return Json(new
-                    {
-                        success = true,
-                        message = description,
-                        type = type,
-                    });
-                }
-
-                return Json(new { success = false });
-            }
-            catch (Exception ex)
-            {
-                _log.Error("[ContratoController] Erro em UpdateStatusContrato: {ex.Message}", ex);
-                Alerta.TratamentoErroComLinha("ContratoController.cs", "UpdateStatusContrato", ex);
-                return new JsonResult(new { sucesso = false });
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
+                    "ContratoController.cs" ,
+                    "UpdateStatusContrato" ,
+                    error
+                );
+                return new JsonResult(new
+                {
+                    sucesso = false
+                });
             }
         }
 
         [Route("ListaContratos")]
-        [HttpGet]
         public async Task<JsonResult> OnGetListaContratos(string? tipoContrato = "")
         {
             try
             {
-
                 var items = await _unitOfWork
                     .Contrato.GetDropDown(tipoContrato)
                     .ToListAsync();
 
-                return new JsonResult(new { data = items });
-            }
-            catch (Exception ex)
-            {
-                _log.Error("[ContratoController] Erro em OnGetListaContratos: {ex.Message}", ex);
-                Alerta.TratamentoErroComLinha("ContratoController.cs", "OnGetListaContratos", ex);
-                return new JsonResult(new { sucesso = false });
+                return new JsonResult(new
+                {
+                    data = items
+                });
+            }
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha(
+                    "ContratoController.cs" ,
+                    "OnGetListaContratos" ,
+                    error
+                );
+                return new JsonResult(new
+                {
+                    sucesso = false
+                });
             }
         }
 
         [Route("ListaContratosVeiculosGlosa")]
-        [HttpGet]
         public async Task<JsonResult> ListaContratosVeiculosGlosa(string? tipoContrato = "")
         {
             try
             {
-
                 var contratos = await _db.Set<Contrato>()
                     .AsNoTracking()
                     .Where(c => c.TipoContrato == "Locação" && c.Status)
@@ -393,18 +393,28 @@
                     .ThenByDescending(c => c.Fornecedor.DescricaoFornecedor)
                     .Select(c => new SelectListItem
                     {
-                        Value = c.ContratoId.ToString(),
-                        Text = $"{c.AnoContrato}/{c.NumeroContrato} - {c.Fornecedor.DescricaoFornecedor}",
+                        Value = c.ContratoId.ToString() ,
+                        Text =
+                            $"{c.AnoContrato}/{c.NumeroContrato} - {c.Fornecedor.DescricaoFornecedor}" ,
                     })
                     .ToListAsync();
 
-                return new JsonResult(new { data = contratos });
-            }
-            catch (Exception ex)
-            {
-                _log.Error("[ContratoController] Erro em ListaContratosVeiculosGlosa: {ex.Message}", ex);
-                Alerta.TratamentoErroComLinha("ContratoController.cs", "ListaContratosVeiculosGlosa", ex);
-                return new JsonResult(new { sucesso = false });
+                return new JsonResult(new
+                {
+                    data = contratos
+                });
+            }
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha(
+                    "ContratoController.cs" ,
+                    "ListaContratosVeiculosGlosa" ,
+                    error
+                );
+                return new JsonResult(new
+                {
+                    sucesso = false
+                });
             }
         }
 
@@ -414,26 +424,27 @@
         {
             try
             {
-
                 var result = (
                     from c in _unitOfWork.Contrato.GetAll()
                     where c.ContratoId == id
                     select new
                     {
-                        c.ContratoLavadores,
-                        c.ContratoMotoristas,
-                        c.ContratoOperadores,
-                        c.TipoContrato,
-                        c.ContratoId,
+                        c.ContratoLavadores ,
+                        c.ContratoMotoristas ,
+                        c.ContratoOperadores ,
+                        c.TipoContrato ,
+                        c.ContratoId ,
                     }
                 ).ToList();
 
-                return Json(new { data = result });
-            }
-            catch (Exception ex)
-            {
-                _log.Error("[ContratoController] Erro em PegaContrato: {ex.Message}", ex);
-                Alerta.TratamentoErroComLinha("ContratoController.cs", "PegaContrato", ex);
+                return Json(new
+                {
+                    data = result
+                });
+            }
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha("ContratoController.cs" , "PegaContrato" , error);
                 return StatusCode(500);
             }
         }
@@ -444,19 +455,19 @@
         {
             try
             {
-
                 var existeContrato = _unitOfWork.Contrato.GetFirstOrDefault(u =>
                     (u.AnoContrato == contrato.AnoContrato)
                     && (u.NumeroContrato == contrato.NumeroContrato)
                 );
-
                 if (existeContrato != null && existeContrato.ContratoId != contrato.ContratoId)
                 {
-                    return new JsonResult(new
-                    {
-                        data = "00000000-0000-0000-0000-000000000000",
-                        message = "Já existe um contrato com esse número",
-                    });
+                    return new JsonResult(
+                        new
+                        {
+                            data = "00000000-0000-0000-0000-000000000000" ,
+                            message = "Já existe um contrato com esse número" ,
+                        }
+                    );
                 }
 
                 _unitOfWork.Contrato.Add(contrato);
@@ -470,17 +481,21 @@
 
                 _unitOfWork.Save();
 
-                return new JsonResult(new
-                {
-                    data = objRepactuacao.RepactuacaoContratoId,
-                    message = "Contrato Adicionado com Sucesso",
-                });
-            }
-            catch (Exception ex)
-            {
-                _log.Error("[ContratoController] Erro em InsereContrato: {ex.Message}", ex);
-                Alerta.TratamentoErroComLinha("ContratoController.cs", "InsereContrato", ex);
-                return new JsonResult(new { sucesso = false });
+                return new JsonResult(
+                    new
+                    {
+                        data = objRepactuacao.RepactuacaoContratoId ,
+                        message = "Contrato Adicionado com Sucesso" ,
+                    }
+                );
+            }
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha("ContratoController.cs" , "InsereContrato" , error);
+                return new JsonResult(new
+                {
+                    sucesso = false
+                });
             }
         }
 
@@ -504,68 +519,76 @@
                     c.ContratoId == repactuacaoContrato.ContratoId
                 );
 
-                if (objContrato != null)
-                {
+                objContrato.Valor = repactuacaoContrato.Valor;
+                objContrato.DataRepactuacao = repactuacaoContrato.DataRepactuacao;
+                objContrato.Prorrogacao = repactuacaoContrato.Prorrogacao;
+                objContrato.Vigencia = repactuacaoContrato.Vigencia;
+
+                _unitOfWork.Contrato.Update(objContrato);
+
+                _unitOfWork.Save();
+
+                return new JsonResult(
+                    new
+                    {
+                        data = objRepactuacao.RepactuacaoContratoId ,
+                        message = "Repactuação Adicionada com Sucesso" ,
+                    }
+                );
+            }
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha("ContratoController.cs" , "InsereRepactuacao" , error);
+                return new JsonResult(new
+                {
+                    sucesso = false
+                });
+            }
+        }
+
+        [Route("AtualizaRepactuacao")]
+        [HttpPost]
+        public JsonResult AtualizaRepactuacao(RepactuacaoContrato repactuacaoContrato)
+        {
+            try
+            {
+                _unitOfWork.RepactuacaoContrato.Update(repactuacaoContrato);
+
+                if (repactuacaoContrato.AtualizaContrato == true)
+                {
+                    var objContrato = _unitOfWork.Contrato.GetFirstOrDefault(c =>
+                        c.ContratoId == repactuacaoContrato.ContratoId
+                    );
+
                     objContrato.Valor = repactuacaoContrato.Valor;
                     objContrato.DataRepactuacao = repactuacaoContrato.DataRepactuacao;
                     objContrato.Prorrogacao = repactuacaoContrato.Prorrogacao;
                     objContrato.Vigencia = repactuacaoContrato.Vigencia;
+
                     _unitOfWork.Contrato.Update(objContrato);
                 }
 
                 _unitOfWork.Save();
 
-                return new JsonResult(new
-                {
-                    data = objRepactuacao.RepactuacaoContratoId,
-                    message = "Repactuação Adicionada com Sucesso",
-                });
-            }
-            catch (Exception ex)
-            {
-                _log.Error("[ContratoController] Erro em InsereRepactuacao: {ex.Message}", ex);
-                Alerta.TratamentoErroComLinha("ContratoController.cs", "InsereRepactuacao", ex);
-                return new JsonResult(new { sucesso = false });
-            }
-        }
-
-        [Route("AtualizaRepactuacao")]
-        [HttpPost]
-        public JsonResult AtualizaRepactuacao(RepactuacaoContrato repactuacaoContrato)
-        {
-            try
-            {
-                _unitOfWork.RepactuacaoContrato.Update(repactuacaoContrato);
-
-                if (repactuacaoContrato.AtualizaContrato == true)
-                {
-                    var objContrato = _unitOfWork.Contrato.GetFirstOrDefault(c =>
-                        c.ContratoId == repactuacaoContrato.ContratoId
-                    );
-
-                    if (objContrato != null)
-                    {
-                        objContrato.Valor = repactuacaoContrato.Valor;
-                        objContrato.DataRepactuacao = repactuacaoContrato.DataRepactuacao;
-                        objContrato.Prorrogacao = repactuacaoContrato.Prorrogacao;
-                        objContrato.Vigencia = repactuacaoContrato.Vigencia;
-                        _unitOfWork.Contrato.Update(objContrato);
-                    }
-                }
-
-                _unitOfWork.Save();
-
-                return new JsonResult(new
-                {
-                    data = repactuacaoContrato.RepactuacaoContratoId,
-                    message = "Repactuação Atualizada com Sucesso",
-                });
-            }
-            catch (Exception ex)
-            {
-                _log.Error("[ContratoController] Erro em AtualizaRepactuacao: {ex.Message}", ex);
-                Alerta.TratamentoErroComLinha("ContratoController.cs", "AtualizaRepactuacao", ex);
-                return new JsonResult(new { sucesso = false });
+                return new JsonResult(
+                    new
+                    {
+                        data = repactuacaoContrato.RepactuacaoContratoId ,
+                        message = "Repactuação Adicionada com Sucesso" ,
+                    }
+                );
+            }
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha(
+                    "ContratoController.cs" ,
+                    "AtualizaRepactuacao" ,
+                    error
+                );
+                return new JsonResult(new
+                {
+                    sucesso = false
+                });
             }
         }
 
@@ -575,35 +598,40 @@
         {
             try
             {
-
                 var existeContrato = _unitOfWork.Contrato.GetFirstOrDefault(u =>
                     (u.AnoContrato == contrato.AnoContrato)
                     && (u.NumeroContrato == contrato.NumeroContrato)
                 );
-
                 if (existeContrato != null && existeContrato.ContratoId != contrato.ContratoId)
                 {
-                    return new JsonResult(new
-                    {
-                        data = "00000000-0000-0000-0000-000000000000",
-                        message = "Já existe um contrato com esse número",
-                    });
+                    return new JsonResult(
+                        new
+                        {
+                            data = "00000000-0000-0000-0000-000000000000" ,
+                            message = "Já existe um contrato com esse número" ,
+                        }
+                    );
                 }
 
                 _unitOfWork.Contrato.Update(contrato);
+
                 _unitOfWork.Save();
 
-                return new JsonResult(new
-                {
-                    data = contrato,
-                    message = "Contrato Atualizado com Sucesso"
-                });
-            }
-            catch (Exception ex)
-            {
-                _log.Error("[ContratoController] Erro em EditaContrato: {ex.Message}", ex);
-                Alerta.TratamentoErroComLinha("ContratoController.cs", "EditaContrato", ex);
-                return new JsonResult(new { sucesso = false });
+                return new JsonResult(
+                    new
+                    {
+                        data = contrato ,
+                        message = "Contrato Atualizado com Sucesso"
+                    }
+                );
+            }
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha("ContratoController.cs" , "EditaContrato" , error);
+                return new JsonResult(new
+                {
+                    sucesso = false
+                });
             }
         }
 
@@ -614,19 +642,24 @@
             try
             {
                 _unitOfWork.ItemVeiculoContrato.Add(itemveiculo);
+
                 _unitOfWork.Save();
 
-                return new JsonResult(new
-                {
-                    data = itemveiculo.ItemVeiculoId,
-                    message = "Item Veiculo Contrato adicionado com sucesso",
-                });
-            }
-            catch (Exception ex)
-            {
-                _log.Error("[ContratoController] Erro em InsereItemContrato: {ex.Message}", ex);
-                Alerta.TratamentoErroComLinha("ContratoController.cs", "InsereItemContrato", ex);
-                return new JsonResult(new { sucesso = false });
+                return new JsonResult(
+                    new
+                    {
+                        data = itemveiculo.ItemVeiculoId ,
+                        message = "Item Veiculo Contrato adicionado com sucesso" ,
+                    }
+                );
+            }
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha("ContratoController.cs" , "InsereItemContrato" , error);
+                return new JsonResult(new
+                {
+                    sucesso = false
+                });
             }
         }
 
@@ -638,7 +671,6 @@
             {
                 if (itemveiculo.ItemVeiculoId == Guid.Empty)
                 {
-
                     var newItemContrato = new ItemVeiculoContrato();
                     newItemContrato.NumItem = itemveiculo.NumItem;
                     newItemContrato.Quantidade = itemveiculo.Quantidade;
@@ -652,20 +684,27 @@
                 {
                     _unitOfWork.ItemVeiculoContrato.Update(itemveiculo);
                 }
-
                 _unitOfWork.Save();
 
-                return new JsonResult(new
-                {
-                    data = itemveiculo.ItemVeiculoId,
-                    message = "Item Veiculo Contrato atualizado com sucesso",
-                });
-            }
-            catch (Exception ex)
-            {
-                _log.Error("[ContratoController] Erro em AtualizaItemContrato: {ex.Message}", ex);
-                Alerta.TratamentoErroComLinha("ContratoController.cs", "AtualizaItemContrato", ex);
-                return new JsonResult(new { sucesso = false });
+                return new JsonResult(
+                    new
+                    {
+                        data = itemveiculo.ItemVeiculoId ,
+                        message = "Item Veiculo Contrato adicionado com sucesso" ,
+                    }
+                );
+            }
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha(
+                    "ContratoController.cs" ,
+                    "AtualizaItemContrato" ,
+                    error
+                );
+                return new JsonResult(new
+                {
+                    sucesso = false
+                });
             }
         }
 
@@ -676,65 +715,92 @@
             try
             {
                 _unitOfWork.ItemVeiculoContrato.Remove(itemveiculo);
+
                 _unitOfWork.Save();
 
-                return new JsonResult(new
-                {
-                    data = itemveiculo.ItemVeiculoId,
-                    message = "Item Veiculo Contrato Eliminado com sucesso",
-                });
-            }
-            catch (Exception ex)
-            {
-                _log.Error("[ContratoController] Erro em ApagaItemContrato: {ex.Message}", ex);
-                Alerta.TratamentoErroComLinha("ContratoController.cs", "ApagaItemContrato", ex);
-                return new JsonResult(new { sucesso = false });
+                return new JsonResult(
+                    new
+                    {
+                        data = itemveiculo.ItemVeiculoId ,
+                        message = "Item Veiculo Contrato Eliminado com sucesso" ,
+                    }
+                );
+            }
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha("ContratoController.cs" , "ApagaItemContrato" , error);
+                return new JsonResult(new
+                {
+                    sucesso = false
+                });
             }
         }
 
         [Route("InsereRepactuacaoTerceirizacao")]
         [HttpPost]
-        public JsonResult InsereRepactuacaoTerceirizacao(RepactuacaoTerceirizacao repactuacaoTerceirizacao)
+        public JsonResult InsereRepactuacaoTerceirizacao(
+            RepactuacaoTerceirizacao repactuacaoTerceirizacao
+        )
         {
             try
             {
                 _unitOfWork.RepactuacaoTerceirizacao.Add(repactuacaoTerceirizacao);
+
                 _unitOfWork.Save();
 
-                return new JsonResult(new
-                {
-                    data = repactuacaoTerceirizacao.RepactuacaoTerceirizacaoId,
-                    message = "Repactuação do Contrato adicionada com sucesso",
-                });
-            }
-            catch (Exception ex)
-            {
-                _log.Error("[ContratoController] Erro em InsereRepactuacaoTerceirizacao: {ex.Message}", ex);
-                Alerta.TratamentoErroComLinha("ContratoController.cs", "InsereRepactuacaoTerceirizacao", ex);
-                return new JsonResult(new { sucesso = false });
+                return new JsonResult(
+                    new
+                    {
+                        data = repactuacaoTerceirizacao.RepactuacaoTerceirizacaoId ,
+                        message = "Repactuação do Contrato adicionada com sucesso" ,
+                    }
+                );
+            }
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha(
+                    "ContratoController.cs" ,
+                    "InsereRepactuacaoTerceirizacao" ,
+                    error
+                );
+                return new JsonResult(new
+                {
+                    sucesso = false
+                });
             }
         }
 
         [Route("AtualizaRepactuacaoTerceirizacao")]
         [HttpPost]
-        public JsonResult AtualizaRepactuacaoTerceirizacao(RepactuacaoTerceirizacao repactuacaoTerceirizacao)
+        public JsonResult AtualizaRepactuacaoTerceirizacao(
+            RepactuacaoTerceirizacao repactuacaoTerceirizacao
+        )
         {
             try
             {
                 _unitOfWork.RepactuacaoTerceirizacao.Update(repactuacaoTerceirizacao);
+
                 _unitOfWork.Save();
 
-                return new JsonResult(new
-                {
-                    data = repactuacaoTerceirizacao.RepactuacaoTerceirizacaoId,
-                    message = "Repactuação do Contrato adicionada com sucesso",
-                });
-            }
-            catch (Exception ex)
-            {
-                _log.Error("[ContratoController] Erro em AtualizaRepactuacaoTerceirizacao: {ex.Message}", ex);
-                Alerta.TratamentoErroComLinha("ContratoController.cs", "AtualizaRepactuacaoTerceirizacao", ex);
-                return new JsonResult(new { sucesso = false });
+                return new JsonResult(
+                    new
+                    {
+                        data = repactuacaoTerceirizacao.RepactuacaoTerceirizacaoId ,
+                        message = "Repactuação do Contrato adicionada com sucesso" ,
+                    }
+                );
+            }
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha(
+                    "ContratoController.cs" ,
+                    "AtualizaRepactuacaoTerceirizacao" ,
+                    error
+                );
+                return new JsonResult(new
+                {
+                    sucesso = false
+                });
             }
         }
 
@@ -745,19 +811,28 @@
             try
             {
                 _unitOfWork.RepactuacaoServicos.Add(repactuacaoServicos);
+
                 _unitOfWork.Save();
 
-                return new JsonResult(new
-                {
-                    data = repactuacaoServicos.RepactuacaoServicoId,
-                    message = "Repactuação do Contrato adicionada com sucesso",
-                });
-            }
-            catch (Exception ex)
-            {
-                _log.Error("[ContratoController] Erro em InsereRepactuacaoServicos: {ex.Message}", ex);
-                Alerta.TratamentoErroComLinha("ContratoController.cs", "InsereRepactuacaoServicos", ex);
-                return new JsonResult(new { sucesso = false });
+                return new JsonResult(
+                    new
+                    {
+                        data = repactuacaoServicos.RepactuacaoServicoId ,
+                        message = "Repactuação do Contrato adicionada com sucesso" ,
+                    }
+                );
+            }
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha(
+                    "ContratoController.cs" ,
+                    "InsereRepactuacaoServicos" ,
+                    error
+                );
+                return new JsonResult(new
+                {
+                    sucesso = false
+                });
             }
         }
 
@@ -768,19 +843,28 @@
             try
             {
                 _unitOfWork.RepactuacaoServicos.Update(repactuacaoServicos);
+
                 _unitOfWork.Save();
 
-                return new JsonResult(new
-                {
-                    data = repactuacaoServicos.RepactuacaoServicoId,
-                    message = "Repactuação do Contrato adicionada com sucesso",
-                });
-            }
-            catch (Exception ex)
-            {
-                _log.Error("[ContratoController] Erro em AtualizaRepactuacaoServicos: {ex.Message}", ex);
-                Alerta.TratamentoErroComLinha("ContratoController.cs", "AtualizaRepactuacaoServicos", ex);
-                return new JsonResult(new { sucesso = false });
+                return new JsonResult(
+                    new
+                    {
+                        data = repactuacaoServicos.RepactuacaoServicoId ,
+                        message = "Repactuação do Contrato adicionada com sucesso" ,
+                    }
+                );
+            }
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha(
+                    "ContratoController.cs" ,
+                    "AtualizaRepactuacaoServicos" ,
+                    error
+                );
+                return new JsonResult(new
+                {
+                    sucesso = false
+                });
             }
         }
 
@@ -790,8 +874,7 @@
         {
             try
             {
-
-                var repactuacoes = (
+                var RepactuacaoList = (
                     from r in _unitOfWork.RepactuacaoContrato.GetAll()
                     where r.ContratoId == id
                     orderby r.DataRepactuacao descending, r.Prorrogacao descending
@@ -804,17 +887,25 @@
                         ValorMensal = (r.Valor / 12)?.ToString("C"),
                         r.Vigencia,
                         r.Prorrogacao,
-                        Repactuacao = "(" + r.DataRepactuacao?.ToString("dd/MM/yy") + ") " + r.Descricao,
+                        Repactuacao = "("
+                            + r.DataRepactuacao?.ToString("dd/MM/yy")
+                            + ") "
+                            + r.Descricao,
                     }
                 ).ToList();
 
-                return new JsonResult(new { data = repactuacoes });
-            }
-            catch (Exception ex)
-            {
-                _log.Error("[ContratoController] Erro em RepactuacaoList: {ex.Message}", ex);
-                Alerta.TratamentoErroComLinha("ContratoController.cs", "RepactuacaoList", ex);
-                return new JsonResult(new { sucesso = false });
+                return new JsonResult(new
+                {
+                    data = RepactuacaoList
+                });
+            }
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha("ContratoController.cs", "RepactuacaoList", error);
+                return new JsonResult(new
+                {
+                    sucesso = false
+                });
             }
         }
 
@@ -833,10 +924,9 @@
             }
             catch (Exception error)
             {
-                _log.Error(error.Message, error, "ContratoController.cs", "RecuperaTipoContrato");
                 Alerta.TratamentoErroComLinha(
-                    "ContratoController.cs",
-                    "RecuperaTipoContrato",
+                    "ContratoController.cs" ,
+                    "RecuperaTipoContrato" ,
                     error
                 );
                 return StatusCode(500);
@@ -849,16 +939,23 @@
         {
             try
             {
-                var objRepactuacaoTerceirizacao = _unitOfWork.RepactuacaoTerceirizacao.GetFirstOrDefault(r =>
-                    r.RepactuacaoContratoId == Guid.Parse(RepactuacaoContratoId)
-                );
-
-                return Json(new { objRepactuacaoTerceirizacao });
-            }
-            catch (Exception ex)
-            {
-                _log.Error("[ContratoController] Erro em RecuperaRepactuacaoTerceirizacao: {ex.Message}", ex);
-                Alerta.TratamentoErroComLinha("ContratoController.cs", "RecuperaRepactuacaoTerceirizacao", ex);
+                var objRepactuacaoTerceirizacao =
+                    _unitOfWork.RepactuacaoTerceirizacao.GetFirstOrDefault(r =>
+                        r.RepactuacaoContratoId == Guid.Parse(RepactuacaoContratoId)
+                    );
+
+                return Json(new
+                {
+                    objRepactuacaoTerceirizacao
+                });
+            }
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha(
+                    "ContratoController.cs" ,
+                    "RecuperaRepactuacaoTerceirizacao" ,
+                    error
+                );
                 return StatusCode(500);
             }
         }
@@ -869,21 +966,36 @@
         {
             try
             {
-
-                var vinculos = (
+                var objRepactuacaoLocacao = (
                     from v in _unitOfWork.Veiculo.GetAll()
                     join ivc in _unitOfWork.ItemVeiculoContrato.GetAll()
                         on v.ItemVeiculoId equals ivc.ItemVeiculoId
                     where ivc.RepactuacaoContratoId == RepactuacaoContratoId
-                    select v.VeiculoId
-                ).Any();
-
-                return Json(new { existeItem = vinculos });
-            }
-            catch (Exception ex)
-            {
-                _log.Error("[ContratoController] Erro em ExisteItem: {ex.Message}", ex);
-                Alerta.TratamentoErroComLinha("ContratoController.cs", "ExisteItem", ex);
+                    orderby ivc.NumItem
+                    select new
+                    {
+                        v.VeiculoId
+                    }
+                ).ToList();
+
+                if (objRepactuacaoLocacao.Count() == 0)
+                {
+                    return Json(new
+                    {
+                        existeItem = false
+                    });
+                }
+                else
+                {
+                    return Json(new
+                    {
+                        existeItem = true
+                    });
+                }
+            }
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha("ContratoController.cs" , "ExisteItem" , error);
                 return StatusCode(500);
             }
         }
@@ -896,7 +1008,6 @@
             {
                 try
                 {
-
                     var objRepactuacaoLocacao = _unitOfWork.ItemVeiculoContrato.GetAll(iv =>
                         iv.RepactuacaoContratoId == Id
                     );
@@ -924,25 +1035,36 @@
                     var objRepactuacao = _unitOfWork.RepactuacaoContrato.GetFirstOrDefault(rc =>
                         rc.RepactuacaoContratoId == Id
                     );
-
-                    if (objRepactuacao != null)
-                        _unitOfWork.RepactuacaoContrato.Remove(objRepactuacao);
+                    _unitOfWork.RepactuacaoContrato.Remove(objRepactuacao);
 
                     _unitOfWork.Save();
 
-                    return new JsonResult(new { success = true, message = "Repactuação Excluída com Sucesso!" });
+                    return new JsonResult(
+                        new
+                        {
+                            success = true ,
+                            message = "Repactuação Excluída com Sucesso!"
+                        }
+                    );
                 }
                 catch (Exception)
                 {
-
-                    return new JsonResult(new { success = false, message = "Existem Veículos Associados a Essa Repactuação!" });
-                }
-            }
-            catch (Exception ex)
-            {
-                _log.Error("[ContratoController] Erro em ApagaRepactuacao: {ex.Message}", ex);
-                Alerta.TratamentoErroComLinha("ContratoController.cs", "ApagaRepactuacao", ex);
-                return new JsonResult(new { sucesso = false });
+                    return new JsonResult(
+                        new
+                        {
+                            success = false ,
+                            message = "Existem Veículos Associados a Essa Repactuação!" ,
+                        }
+                    );
+                }
+            }
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha("ContratoController.cs" , "DeleteOS" , error);
+                return new JsonResult(new
+                {
+                    sucesso = false
+                });
             }
         }
 
@@ -959,17 +1081,18 @@
                         .OrderByDescending(rc => rc.DataRepactuacao)
                         .First();
 
-                    return Json(objRepactuacao.RepactuacaoContratoId);
+                    var objRepactuacaoContratoId = objRepactuacao.RepactuacaoContratoId;
+
+                    return Json(objRepactuacaoContratoId);
                 }
                 catch (Exception)
                 {
                     return Json(Guid.Empty);
                 }
             }
-            catch (Exception ex)
-            {
-                _log.Error("[ContratoController] Erro em UltimaRepactuacao: {ex.Message}", ex);
-                Alerta.TratamentoErroComLinha("ContratoController.cs", "UltimaRepactuacao", ex);
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha("ContratoController.cs" , "UltimaRepactuacao" , error);
                 return StatusCode(500);
             }
         }
@@ -980,7 +1103,6 @@
         {
             try
             {
-
                 var itens = _unitOfWork.ItemVeiculoContrato.GetAll()
                     .Where(ivc => ivc.RepactuacaoContratoId == repactuacaoContratoId)
                     .ToList();
@@ -1007,10 +1129,13 @@
 
                 return Json(result);
             }
-            catch (Exception ex)
-            {
-                _log.Error("[ContratoController] Erro em RecuperaItensUltimaRepactuacao: {ex.Message}", ex);
-                Alerta.TratamentoErroComLinha("ContratoController.cs", "RecuperaItensUltimaRepactuacao", ex);
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha(
+                    "ContratoController.cs",
+                    "RecuperaItensUltimaRepactuacao",
+                    error
+                );
                 return StatusCode(500);
             }
         }
@@ -1019,16 +1144,7 @@
         [HttpGet]
         public IActionResult ListaItensRepactuacao(Guid repactuacaoContratoId)
         {
-            try
-            {
-                return RecuperaItensUltimaRepactuacao(repactuacaoContratoId);
-            }
-            catch (Exception ex)
-            {
-                _log.Error("[ContratoController] Erro em ListaItensRepactuacao: {ex.Message}", ex);
-                Alerta.TratamentoErroComLinha("ContratoController.cs", "ListaItensRepactuacao", ex);
-                return StatusCode(500);
-            }
+            return RecuperaItensUltimaRepactuacao(repactuacaoContratoId);
         }
 
         [Route("RecuperaRepactuacaoCompleta")]
@@ -1101,10 +1217,9 @@
                     dadosEspecificos
                 });
             }
-            catch (Exception ex)
-            {
-                _log.Error("[ContratoController] Erro em RecuperaRepactuacaoCompleta: {ex.Message}", ex);
-                Alerta.TratamentoErroComLinha("ContratoController.cs", "RecuperaRepactuacaoCompleta", ex);
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha("ContratoController.cs", "RecuperaRepactuacaoCompleta", error);
                 return StatusCode(500);
             }
         }
@@ -1153,12 +1268,14 @@
 
                 foreach (var itemNovo in itensNovos)
                 {
+
                     var itensCorrespondentes = itensAnteriores
                         .Where(i => i.NumItem == itemNovo.NumItem)
                         .ToList();
 
                     foreach (var itemAnterior in itensCorrespondentes)
                     {
+
                         var veiculos = _unitOfWork.Veiculo.GetAll()
                             .Where(v => v.ItemVeiculoId == itemAnterior.ItemVeiculoId)
                             .ToList();
@@ -1181,13 +1298,14 @@
                         message = $"{veiculosMovidos} veículo(s) movido(s) para a nova repactuação"
                     });
                 }
-
-                return Json(new { success = false, message = "Nenhum veículo encontrado para mover" });
-            }
-            catch (Exception ex)
-            {
-                _log.Error("[ContratoController] Erro em MoverVeiculosRepactuacao: {ex.Message}", ex);
-                Alerta.TratamentoErroComLinha("ContratoController.cs", "MoverVeiculosRepactuacao", ex);
+                else
+                {
+                    return Json(new { success = false, message = "Nenhum veículo encontrado para mover" });
+                }
+            }
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha("ContratoController.cs", "MoverVeiculosRepactuacao", error);
                 return StatusCode(500);
             }
         }
```
