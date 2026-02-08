# Controllers/DashboardVeiculosController.cs

**Mudanca:** GRANDE | **+402** linhas | **-249** linhas

---

```diff
--- JANEIRO: Controllers/DashboardVeiculosController.cs
+++ ATUAL: Controllers/DashboardVeiculosController.cs
@@ -1,317 +1,488 @@
-using FrotiX.Data;
-using Microsoft.AspNetCore.Authorization;
+using FrotiX.Models;
+using FrotiX.Repository.IRepository;
 using Microsoft.AspNetCore.Mvc;
-using Microsoft.EntityFrameworkCore;
 using System;
 using System.Collections.Generic;
 using System.Linq;
-using System.Threading.Tasks;
 using FrotiX.Helpers;
-using FrotiX.Services;
 
 namespace FrotiX.Controllers
 {
-
-    [Authorize]
-    public class DashboardVeiculosController : Controller
+    [Route("api/[controller]")]
+    [ApiController]
+    public class DashboardVeiculosController : ControllerBase
     {
-        private readonly FrotiXDbContext _context;
-        private readonly ILogService _log;
-
-        public DashboardVeiculosController(FrotiXDbContext context, ILogService log)
+        private readonly IUnitOfWork _unitOfWork;
+
+        public DashboardVeiculosController(IUnitOfWork unitOfWork)
+        {
+            _unitOfWork = unitOfWork;
+        }
+
+        [Route("DashboardDados")]
+        [HttpGet]
+        public IActionResult DashboardDados()
         {
             try
             {
-                _context = context;
-                _log = log;
-            }
-            catch (Exception ex)
-            {
-                _log?.Error(ex.Message, ex, "DashboardVeiculosController.cs", "Constructor");
-                Alerta.TratamentoErroComLinha("DashboardVeiculosController.cs", "Constructor", ex);
+                var veiculos = _unitOfWork.ViewVeiculos.GetAll().ToList();
+                var veiculosModel = _unitOfWork.Veiculo.GetAll().ToList();
+
+                var totalVeiculos = veiculos.Count;
+                var veiculosAtivos = veiculos.Count(v => v.Status == true);
+                var veiculosInativos = veiculos.Count(v => v.Status == false);
+                var veiculosReserva = veiculos.Count(v => v.VeiculoReserva == "Reserva");
+                var veiculosEfetivos = veiculos.Count(v => v.VeiculoReserva == "Efetivo");
+                var veiculosProprios = veiculos.Count(v => v.VeiculoProprio == true);
+                var veiculosLocados = veiculos.Count(v => v.VeiculoProprio == false);
+
+                var anoAtual = DateTime.Now.Year;
+                var veiculosComAno = veiculosModel.Where(v => v.AnoFabricacao.HasValue && v.AnoFabricacao > 1990).ToList();
+                var idadeMedia = veiculosComAno.Any()
+                    ? veiculosComAno.Average(v => anoAtual - v.AnoFabricacao.Value)
+                    : 0;
+
+                var porCategoria = veiculos
+                    .Where(v => !string.IsNullOrEmpty(v.Categoria))
+                    .GroupBy(v => v.Categoria)
+                    .Select(g => new
+                    {
+                        categoria = g.Key,
+                        quantidade = g.Count()
+                    })
+                    .OrderByDescending(c => c.quantidade)
+                    .ToList();
+
+                var porStatus = new[]
+                {
+                    new { status = "Ativos", quantidade = veiculosAtivos },
+                    new { status = "Inativos", quantidade = veiculosInativos }
+                };
+
+                var porTipo = new[]
+                {
+                    new { tipo = "Efetivos", quantidade = veiculosEfetivos },
+                    new { tipo = "Reserva", quantidade = veiculosReserva }
+                };
+
+                var porOrigem = veiculos
+                    .Where(v => !string.IsNullOrEmpty(v.OrigemVeiculo))
+                    .GroupBy(v => v.OrigemVeiculo)
+                    .Select(g => new
+                    {
+                        origem = g.Key,
+                        quantidade = g.Count()
+                    })
+                    .OrderByDescending(o => o.quantidade)
+                    .ToList();
+
+                var porModelo = veiculos
+                    .Where(v => !string.IsNullOrEmpty(v.MarcaModelo))
+                    .GroupBy(v => v.MarcaModelo)
+                    .Select(g => new
+                    {
+                        modelo = g.Key,
+                        quantidade = g.Count()
+                    })
+                    .OrderByDescending(m => m.quantidade)
+                    .Take(15)
+                    .ToList();
+
+                var porAnoFabricacao = veiculosModel
+                    .Where(v => v.AnoFabricacao.HasValue && v.AnoFabricacao > 1990)
+                    .GroupBy(v => v.AnoFabricacao.Value)
+                    .Select(g => new
+                    {
+                        ano = g.Key,
+                        quantidade = g.Count()
+                    })
+                    .OrderBy(a => a.ano)
+                    .ToList();
+
+                var porCombustivel = veiculos
+                    .Where(v => !string.IsNullOrEmpty(v.Descricao))
+                    .GroupBy(v => v.Descricao)
+                    .Select(g => new
+                    {
+                        combustivel = g.Key,
+                        quantidade = g.Count()
+                    })
+                    .OrderByDescending(c => c.quantidade)
+                    .ToList();
+
+                var porUnidade = veiculos
+                    .Where(v => !string.IsNullOrEmpty(v.Sigla))
+                    .GroupBy(v => v.Sigla)
+                    .Select(g => new
+                    {
+                        unidade = g.Key,
+                        quantidade = g.Count()
+                    })
+                    .OrderByDescending(u => u.quantidade)
+                    .Take(10)
+                    .ToList();
+
+                var topKm = veiculos
+                    .Where(v => v.Quilometragem.HasValue && v.Quilometragem > 0)
+                    .OrderByDescending(v => v.Quilometragem)
+                    .Take(10)
+                    .Select(v => new
+                    {
+                        placa = v.Placa ?? "-",
+                        modelo = v.MarcaModelo ?? "-",
+                        km = v.Quilometragem ?? 0
+                    })
+                    .ToList();
+
+                var valorMensalTotal = veiculos
+                    .Where(v => v.ValorMensal.HasValue)
+                    .Sum(v => v.ValorMensal ?? 0);
+
+                var resultado = new
+                {
+                    totais = new
+                    {
+                        totalVeiculos,
+                        veiculosAtivos,
+                        veiculosInativos,
+                        veiculosReserva,
+                        veiculosEfetivos,
+                        veiculosProprios,
+                        veiculosLocados,
+                        idadeMedia = Math.Round(idadeMedia, 1),
+                        valorMensalTotal = Math.Round(valorMensalTotal, 2)
+                    },
+                    porCategoria,
+                    porStatus,
+                    porTipo,
+                    porOrigem,
+                    porModelo,
+                    porAnoFabricacao,
+                    porCombustivel,
+                    porUnidade,
+                    topKm
+                };
+
+                return Ok(resultado);
+            }
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha("DashboardVeiculosController.cs", "DashboardDados", error);
+                return StatusCode(500, new { message = "Erro ao carregar dados do dashboard" });
             }
         }
 
-        private (DateTime dataInicio, DateTime dataFim) ObterPeriodo(DateTime? dataInicio, DateTime? dataFim, int? ano, int? mes)
+        [Route("DashboardUso")]
+        [HttpGet]
+        public IActionResult DashboardUso(int? ano, int? mes, DateTime? dataInicio, DateTime? dataFim)
         {
             try
             {
 
-                if (ano.HasValue)
-                {
-                    if (mes.HasValue)
-                    {
-                        var inicio = new DateTime(ano.Value, mes.Value, 1);
-                        var fim = inicio.AddMonths(1).AddSeconds(-1);
-                        return (inicio, fim);
+                var queryViagens = _unitOfWork.Viagem.GetAll()
+                    .Where(v => v.DataInicial.HasValue);
+
+                var queryAbastecimentos = _unitOfWork.ViewAbastecimentos.GetAll()
+                    .Where(a => a.DataHora.HasValue);
+
+                if (dataInicio.HasValue && dataFim.HasValue)
+                {
+                    var dataFimAjustada = dataFim.Value.Date.AddDays(1).AddSeconds(-1);
+                    queryViagens = queryViagens.Where(v => v.DataInicial.Value >= dataInicio.Value && v.DataInicial.Value <= dataFimAjustada);
+                    queryAbastecimentos = queryAbastecimentos.Where(a => a.DataHora.Value >= dataInicio.Value && a.DataHora.Value <= dataFimAjustada);
+                }
+
+                else
+                {
+                    if (ano.HasValue && ano > 0)
+                    {
+                        queryViagens = queryViagens.Where(v => v.DataInicial.Value.Year == ano.Value);
+                        queryAbastecimentos = queryAbastecimentos.Where(a => a.DataHora.Value.Year == ano.Value);
                     }
-                    else
-                    {
-                        var inicio = new DateTime(ano.Value, 1, 1);
-                        var fim = new DateTime(ano.Value, 12, 31, 23, 59, 59);
-                        return (inicio, fim);
+
+                    if (mes.HasValue && mes > 0)
+                    {
+                        queryViagens = queryViagens.Where(v => v.DataInicial.Value.Month == mes.Value);
+                        queryAbastecimentos = queryAbastecimentos.Where(a => a.DataHora.Value.Month == mes.Value);
                     }
                 }
-                else if (dataInicio.HasValue && dataFim.HasValue)
-                {
-                    return (dataInicio.Value, dataFim.Value);
-                }
-                else
-                {
-                    var fim = DateTime.Now.Date.AddDays(1).AddSeconds(-1);
-                    var inicio = fim.AddDays(-30);
-                    return (inicio, fim);
-                }
-            }
-            catch (Exception ex)
-            {
-                _log.Error(ex.Message, ex, "DashboardVeiculosController.cs", "ObterPeriodo");
-                Alerta.TratamentoErroComLinha("DashboardVeiculosController.cs", "ObterPeriodo", ex);
-                return (DateTime.Now.Date.AddDays(-30), DateTime.Now.Date.AddDays(1).AddSeconds(-1));
+
+                var viagens = queryViagens.ToList();
+                var abastecimentos = queryAbastecimentos.ToList();
+
+                var topViagensPorVeiculo = viagens
+                    .Where(v => v.VeiculoId.HasValue)
+                    .GroupBy(v => v.VeiculoId)
+                    .Select(g => new
+                    {
+                        veiculoId = g.Key,
+                        quantidade = g.Count(),
+                        kmTotal = g.Sum(v => (v.KmFinal ?? 0) - (v.KmInicial ?? 0))
+                    })
+                    .OrderByDescending(v => v.quantidade)
+                    .Take(10)
+                    .ToList();
+
+                var veiculosIds = topViagensPorVeiculo.Select(v => v.veiculoId).ToList();
+                var veiculosInfo = _unitOfWork.ViewVeiculos.GetAll()
+                    .Where(v => veiculosIds.Contains(v.VeiculoId))
+                    .ToDictionary(v => v.VeiculoId);
+
+                var topViagens = topViagensPorVeiculo.Select(v => new
+                {
+                    placa = veiculosInfo.ContainsKey(v.veiculoId.Value) ? veiculosInfo[v.veiculoId.Value].Placa : "-",
+                    modelo = veiculosInfo.ContainsKey(v.veiculoId.Value) ? veiculosInfo[v.veiculoId.Value].MarcaModelo : "-",
+                    quantidade = v.quantidade,
+                    kmTotal = v.kmTotal
+                }).ToList();
+
+                var topAbastecimento = abastecimentos
+                    .Where(a => a.VeiculoId != Guid.Empty)
+                    .GroupBy(a => new { a.VeiculoId, a.Placa, a.TipoVeiculo })
+                    .Select(g => new
+                    {
+                        placa = g.Key.Placa ?? "-",
+                        modelo = g.Key.TipoVeiculo ?? "-",
+                        valor = g.Sum(a => ParseDecimal(a.ValorTotal)),
+                        litros = g.Sum(a => ParseDecimal(a.Litros))
+                    })
+                    .OrderByDescending(a => a.valor)
+                    .Take(10)
+                    .ToList();
+
+                var viagensPorMes = viagens
+                    .Where(v => v.DataInicial.HasValue)
+                    .GroupBy(v => v.DataInicial.Value.Month)
+                    .Select(g => new
+                    {
+                        mes = g.Key,
+                        quantidade = g.Count()
+                    })
+                    .OrderBy(v => v.mes)
+                    .ToList();
+
+                var abastecimentoPorMes = abastecimentos
+                    .Where(a => a.DataHora.HasValue)
+                    .GroupBy(a => a.DataHora.Value.Month)
+                    .Select(g => new
+                    {
+                        mes = g.Key,
+                        valor = g.Sum(a => ParseDecimal(a.ValorTotal))
+                    })
+                    .OrderBy(a => a.mes)
+                    .ToList();
+
+                var topLitrosAbastecidos = abastecimentos
+                    .Where(a => a.VeiculoId != Guid.Empty)
+                    .GroupBy(a => new { a.VeiculoId, a.Placa, a.TipoVeiculo })
+                    .Select(g => new
+                    {
+                        placa = g.Key.Placa ?? "-",
+                        modelo = g.Key.TipoVeiculo ?? "-",
+                        litros = g.Sum(a => ParseDecimal(a.Litros)),
+                        qtdAbastecimentos = g.Count()
+                    })
+                    .OrderByDescending(a => a.litros)
+                    .Take(10)
+                    .ToList();
+
+                var kmPorVeiculo = viagens
+                    .Where(v => v.VeiculoId.HasValue)
+                    .GroupBy(v => v.VeiculoId.Value)
+                    .ToDictionary(
+                        g => g.Key,
+                        g => g.Sum(v => (v.KmFinal ?? 0) - (v.KmInicial ?? 0))
+                    );
+
+                var litrosPorVeiculo = abastecimentos
+                    .Where(a => a.VeiculoId != Guid.Empty)
+                    .GroupBy(a => a.VeiculoId)
+                    .ToDictionary(
+                        g => g.Key,
+                        g => new { litros = g.Sum(a => ParseDecimal(a.Litros)), placa = g.First().Placa, modelo = g.First().TipoVeiculo }
+                    );
+
+                var topConsumo = litrosPorVeiculo
+                    .Where(l => l.Value.litros > 0 && kmPorVeiculo.ContainsKey(l.Key) && kmPorVeiculo[l.Key] > 0)
+                    .Select(l => new
+                    {
+                        placa = l.Value.placa ?? "-",
+                        modelo = l.Value.modelo ?? "-",
+                        kmRodado = kmPorVeiculo[l.Key],
+                        litros = l.Value.litros,
+                        consumo = Math.Round((decimal)kmPorVeiculo[l.Key] / l.Value.litros, 2)
+                    })
+                    .OrderBy(c => c.consumo)
+                    .Take(10)
+                    .ToList();
+
+                var topEficiencia = litrosPorVeiculo
+                    .Where(l => l.Value.litros > 0 && kmPorVeiculo.ContainsKey(l.Key) && kmPorVeiculo[l.Key] > 0)
+                    .Select(l => new
+                    {
+                        placa = l.Value.placa ?? "-",
+                        modelo = l.Value.modelo ?? "-",
+                        kmRodado = kmPorVeiculo[l.Key],
+                        litros = l.Value.litros,
+                        consumo = Math.Round((decimal)kmPorVeiculo[l.Key] / l.Value.litros, 2)
+                    })
+                    .OrderByDescending(c => c.consumo)
+                    .Take(10)
+                    .ToList();
+
+                var anosDisponiveis = _unitOfWork.Viagem.GetAll()
+                    .Where(v => v.DataInicial.HasValue)
+                    .Select(v => v.DataInicial.Value.Year)
+                    .Distinct()
+                    .OrderByDescending(a => a)
+                    .ToList();
+
+                var resultado = new
+                {
+                    anoSelecionado = ano,
+                    anosDisponiveis,
+                    topViagens,
+                    topAbastecimento,
+                    topLitrosAbastecidos,
+                    topConsumo,
+                    topEficiencia,
+                    viagensPorMes,
+                    abastecimentoPorMes,
+                    totais = new
+                    {
+                        totalViagens = viagens.Count,
+                        totalAbastecimentos = abastecimentos.Count,
+                        valorTotalAbastecimento = abastecimentos.Sum(a => ParseDecimal(a.ValorTotal)),
+                        kmTotalRodado = viagens.Sum(v => (v.KmFinal ?? 0) - (v.KmInicial ?? 0)),
+                        totalLitros = abastecimentos.Sum(a => ParseDecimal(a.Litros))
+                    }
+                };
+
+                return Ok(resultado);
+            }
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha("DashboardVeiculosController.cs", "DashboardUso", error);
+                return StatusCode(500, new { message = "Erro ao carregar estatísticas de uso" });
             }
         }
 
+        [Route("DashboardCustos")]
         [HttpGet]
-        [Route("api/DashboardVeiculos/ObterListaVeiculos")]
-        public async Task<IActionResult> ObterListaVeiculos()
+        public IActionResult DashboardCustos(int? ano)
         {
             try
             {
-
-                var veiculos = await _context.Veiculo
-                    .Where(v => v.Status == true)
-                    .Select(v => new
-                    {
-                        veiculoId = v.VeiculoId,
-                        placa = v.Placa,
-                        modelo = v.ModeloVeiculo != null ? v.ModeloVeiculo.DescricaoModelo : "N/A"
-                    })
-                    .OrderBy(v => v.placa)
-                    .AsNoTracking()
-                    .ToListAsync();
-
-                return Json(new { success = true, data = veiculos });
-            }
-            catch (Exception ex)
-            {
-                _log.Error(ex.Message, ex, "DashboardVeiculosController.cs", "ObterListaVeiculos");
-                Alerta.TratamentoErroComLinha("DashboardVeiculosController.cs", "ObterListaVeiculos", ex);
-                return Json(new { success = false, message = ex.Message });
+                var anoFiltro = ano ?? DateTime.Now.Year;
+
+                var abastecimentos = _unitOfWork.ViewAbastecimentos.GetAll()
+                    .Where(a => a.DataHora.HasValue && a.DataHora.Value.Year == anoFiltro)
+                    .ToList();
+
+                var manutencoes = _unitOfWork.Manutencao.GetAll()
+                    .Where(m => m.DataSolicitacao.HasValue && m.DataSolicitacao.Value.Year == anoFiltro)
+                    .ToList();
+
+                var veiculosCategorias = _unitOfWork.ViewVeiculos.GetAll()
+                    .Where(v => !string.IsNullOrEmpty(v.Categoria))
+                    .ToDictionary(v => v.VeiculoId, v => v.Categoria ?? "Sem Categoria");
+
+                var custoPorCategoria = abastecimentos
+                    .Where(a => a.VeiculoId != Guid.Empty && veiculosCategorias.ContainsKey(a.VeiculoId))
+                    .GroupBy(a => veiculosCategorias[a.VeiculoId])
+                    .Select(g => new
+                    {
+                        categoria = g.Key,
+                        valorAbastecimento = g.Sum(a => ParseDecimal(a.ValorTotal))
+                    })
+                    .OrderByDescending(c => c.valorAbastecimento)
+                    .ToList();
+
+                var custoAbastecimentoMes = abastecimentos
+                    .Where(a => a.DataHora.HasValue)
+                    .GroupBy(a => a.DataHora.Value.Month)
+                    .Select(g => new
+                    {
+                        mes = g.Key,
+                        valor = g.Sum(a => ParseDecimal(a.ValorTotal))
+                    })
+                    .OrderBy(c => c.mes)
+                    .ToList();
+
+                var manutencoesPorMes = manutencoes
+                    .Where(m => m.DataSolicitacao.HasValue)
+                    .GroupBy(m => m.DataSolicitacao.Value.Month)
+                    .Select(g => new
+                    {
+                        mes = g.Key,
+                        quantidade = g.Count()
+                    })
+                    .OrderBy(c => c.mes)
+                    .ToList();
+
+                var comparativoMensal = new List<object>();
+                for (int mes = 1; mes <= 12; mes++)
+                {
+                    var abast = custoAbastecimentoMes.FirstOrDefault(c => c.mes == mes);
+                    var manut = manutencoesPorMes.FirstOrDefault(c => c.mes == mes);
+                    comparativoMensal.Add(new
+                    {
+                        mes,
+                        abastecimento = abast?.valor ?? 0,
+                        manutencao = manut?.quantidade ?? 0
+                    });
+                }
+
+                var resultado = new
+                {
+                    anoSelecionado = ano,
+                    custoPorCategoria,
+                    comparativoMensal,
+                    totais = new
+                    {
+                        totalAbastecimento = abastecimentos.Sum(a => ParseDecimal(a.ValorTotal)),
+                        totalManutencao = 0m,
+                        qtdAbastecimentos = abastecimentos.Count,
+                        qtdManutencoes = manutencoes.Count
+                    }
+                };
+
+                return Ok(resultado);
+            }
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha("DashboardVeiculosController.cs", "DashboardCustos", error);
+                return StatusCode(500, new { message = "Erro ao carregar dados de custos" });
             }
         }
 
-        [HttpGet]
-        [Route("api/DashboardVeiculos/ObterEstatisticasGerais")]
-        public async Task<IActionResult> ObterEstatisticasGerais(DateTime? dataInicio, DateTime? dataFim, int? ano, int? mes)
+        private static decimal ParseDecimal(string? valor)
         {
-            try
-            {
-
-                var periodo = ObterPeriodo(dataInicio, dataFim, ano, mes);
-                dataInicio = periodo.dataInicio;
-                dataFim = periodo.dataFim;
-
-                var totalVeiculos = await _context.Veiculo.CountAsync(v => v.Status == true);
-                var veiculosProprios = await _context.Veiculo.CountAsync(v => v.Status == true && v.VeiculoProprio == true);
-                var veiculosTerceirizados = await _context.Veiculo.CountAsync(v => v.Status == true && v.VeiculoProprio == false);
-                var veiculosManutencao = await _context.Manutencao
-                    .Where(m => m.DataSolicitacao >= dataInicio && (m.DataDevolucao == null || m.DataDevolucao <= dataFim))
-                    .Select(m => m.VeiculoId)
-                    .Distinct()
-                    .CountAsync();
-
-                var custoAbastecimento = await _context.Abastecimento
-                    .Where(a => a.DataHora >= dataInicio && a.DataHora <= dataFim)
-                    .SumAsync(a => (decimal?)(a.Litros * a.ValorUnitario) ?? 0);
-
-                var custoManutencao = 0m;
-
-                var custoLavagem = await _context.Viagem
-                    .Where(v => v.DataInicial >= dataInicio && v.DataInicial <= dataFim)
-                    .SumAsync(v => (decimal?)(v.CustoLavador ?? 0) ?? 0);
-
-                var kmTotal = await _context.Viagem
-                    .Where(v => v.DataInicial >= dataInicio && v.DataInicial <= dataFim)
-                    .Where(v => v.KmInicial.HasValue && v.KmFinal.HasValue)
-                    .SumAsync(v => (decimal)((v.KmFinal ?? 0) - (v.KmInicial ?? 0)));
-
-                return Json(new
-                {
-                    success = true,
-                    totalVeiculos,
-                    veiculosProprios,
-                    veiculosTerceirizados,
-                    veiculosManutencao,
-                    custoAbastecimento,
-                    custoManutencao,
-                    custoLavagem,
-                    kmTotal
-                });
-            }
-            catch (Exception ex)
-            {
-                _log.Error(ex.Message, ex, "DashboardVeiculosController.cs", "ObterEstatisticasGerais");
-                Alerta.TratamentoErroComLinha("DashboardVeiculosController.cs", "ObterEstatisticasGerais", ex);
-                return Json(new { success = false, message = ex.Message });
-            }
-        }
-
-        [HttpGet]
-        [Route("api/DashboardVeiculos/ObterDadosVeiculo")]
-        public async Task<IActionResult> ObterDadosVeiculo(Guid veiculoId, DateTime? dataInicio, DateTime? dataFim, int? ano, int? mes)
-        {
-            try
-            {
-
-                var periodo = ObterPeriodo(dataInicio, dataFim, ano, mes);
-                dataInicio = periodo.dataInicio;
-                dataFim = periodo.dataFim;
-
-                var veiculo = await _context.Veiculo
-                    .Include(v => v.ModeloVeiculo)
-                    .Include(v => v.MarcaVeiculo)
-                    .Where(v => v.VeiculoId == veiculoId)
-                    .FirstOrDefaultAsync();
-
-                if (veiculo == null)
-                {
-                    return Json(new { success = false, message = "Veículo não encontrado" });
-                }
-
-                var viagens = await _context.Viagem
-                    .Where(v => v.VeiculoId == veiculoId && v.DataInicial >= dataInicio && v.DataInicial <= dataFim)
-                    .ToListAsync();
-
-                var kmPercorrido = viagens
-                    .Where(v => v.KmInicial.HasValue && v.KmFinal.HasValue)
-                    .Sum(v => (v.KmFinal ?? 0) - (v.KmInicial ?? 0));
-
-                var qtdViagens = viagens.Count;
-
-                var abastecimentos = await _context.Abastecimento
-                    .Where(a => a.VeiculoId == veiculoId && a.DataHora >= dataInicio && a.DataHora <= dataFim)
-                    .ToListAsync();
-
-                var litrosAbastecidos = abastecimentos.Sum(a => a.Litros ?? 0);
-                var valorAbastecimento = abastecimentos.Sum(a => (a.Litros * a.ValorUnitario) ?? 0);
-                var mediaConsumo = litrosAbastecidos > 0 ? (double)kmPercorrido / (double)litrosAbastecidos : 0;
-
-                var manutencoes = await _context.Manutencao
-                    .Where(m => m.VeiculoId == veiculoId && m.DataSolicitacao >= dataInicio && m.DataSolicitacao <= dataFim)
-                    .ToListAsync();
-
-                var valorManutencao = 0m;
-                var qtdManutencoes = manutencoes.Count;
-
-                return Json(new
-                {
-                    success = true,
-                    veiculo = new
-                    {
-                        veiculo.Placa,
-                        Modelo = veiculo.ModeloVeiculo?.DescricaoModelo ?? "N/A",
-                        Marca = veiculo.MarcaVeiculo?.DescricaoMarca ?? "N/A",
-                        veiculo.AnoFabricacao,
-                        veiculo.Renavam,
-                        veiculo.Quilometragem,
-                        Proprio = veiculo.VeiculoProprio == true ? "Sim" : "Não"
-                    },
-                    estatisticas = new
-                    {
-                        kmPercorrido,
-                        qtdViagens,
-                        litrosAbastecidos = Math.Round((double)litrosAbastecidos, 2),
-                        valorAbastecimento = Math.Round((double)valorAbastecimento, 2),
-                        mediaConsumo = Math.Round(mediaConsumo, 2),
-                        valorManutencao = Math.Round((double)valorManutencao, 2),
-                        qtdManutencoes
-                    }
-                });
-            }
-            catch (Exception ex)
-            {
-                _log.Error(ex.Message, ex, "DashboardVeiculosController.cs", "ObterDadosVeiculo");
-                Alerta.TratamentoErroComLinha("DashboardVeiculosController.cs", "ObterDadosVeiculo", ex);
-                return Json(new { success = false, message = ex.Message });
-            }
-        }
-
-        [HttpGet]
-        [Route("api/DashboardVeiculos/ObterTop10VeiculosKm")]
-        public async Task<IActionResult> ObterTop10VeiculosKm(DateTime? dataInicio, DateTime? dataFim, int? ano, int? mes)
-        {
-            try
-            {
-                var periodo = ObterPeriodo(dataInicio, dataFim, ano, mes);
-
-                var ranking = await _context.Viagem
-                    .Where(v => v.DataInicial >= periodo.dataInicio && v.DataInicial <= periodo.dataFim && v.VeiculoId.HasValue)
-                    .GroupBy(v => new { v.Veiculo.Placa, v.Veiculo.ModeloVeiculo.DescricaoModelo })
-                    .Select(g => new
-                    {
-                        Veiculo = $"{g.Key.Placa} - {g.Key.DescricaoModelo}",
-                        KmTotal = g.Sum(v => (v.KmFinal ?? 0) - (v.KmInicial ?? 0))
-                    })
-                    .OrderByDescending(r => r.KmTotal)
-                    .Take(10)
-                    .ToListAsync();
-
-                return Json(new { success = true, data = ranking });
-            }
-            catch (Exception ex)
-            {
-                _log.Error(ex.Message, ex, "DashboardVeiculosController.cs", "ObterTop10VeiculosKm");
-                Alerta.TratamentoErroComLinha("DashboardVeiculosController.cs", "ObterTop10VeiculosKm", ex);
-                return Json(new { success = false, message = ex.Message });
-            }
-        }
-
-        [HttpGet]
-        [Route("api/DashboardVeiculos/ObterTop10CustoVeiculos")]
-        public async Task<IActionResult> ObterTop10CustoVeiculos(DateTime? dataInicio, DateTime? dataFim, int? ano, int? mes)
-        {
-            try
-            {
-                var periodo = ObterPeriodo(dataInicio, dataFim, ano, mes);
-
-                var abastecimentos = await _context.Abastecimento
-                    .Where(a => a.DataHora >= periodo.dataInicio && a.DataHora <= periodo.dataFim && a.VeiculoId != Guid.Empty)
-                    .GroupBy(a => a.VeiculoId)
-                    .Select(g => new { VeiculoId = g.Key, Custo = g.Sum(a => (a.Litros * a.ValorUnitario) ?? 0) })
-                    .ToListAsync();
-
-                var custosTotais = abastecimentos
-                    .Select(a => new { a.VeiculoId, Custo = a.Custo })
-                    .GroupBy(x => x.VeiculoId)
-                    .Select(g => new { VeiculoId = g.Key, CustoTotal = g.Sum(x => x.Custo) })
-                    .OrderByDescending(x => x.CustoTotal)
-                    .Take(10)
-                    .ToList();
-
-                var veiculoIds = custosTotais.Select(c => c.VeiculoId).ToList();
-                var veiculosInfo = await _context.Veiculo
-                    .Where(v => veiculoIds.Contains(v.VeiculoId))
-                    .Select(v => new { v.VeiculoId, v.Placa, Modelo = v.ModeloVeiculo.DescricaoModelo })
-                    .ToListAsync();
-
-                var resultado = custosTotais.Join(veiculosInfo,
-                    c => c.VeiculoId,
-                    v => v.VeiculoId,
-                    (c, v) => new
-                    {
-                        Veiculo = $"{v.Placa} - {v.Modelo}",
-                        CustoTotal = Math.Round(c.CustoTotal, 2)
-                    })
-                    .ToList();
-
-                return Json(new { success = true, data = resultado });
-            }
-            catch (Exception ex)
-            {
-                _log.Error(ex.Message, ex, "DashboardVeiculosController.cs", "ObterTop10CustoVeiculos");
-                Alerta.TratamentoErroComLinha("DashboardVeiculosController.cs", "ObterTop10CustoVeiculos", ex);
-                return Json(new { success = false, message = ex.Message });
-            }
+            if (string.IsNullOrEmpty(valor))
+                return 0;
+
+            var valorLimpo = valor
+                .Replace("R$", "")
+                .Replace(" ", "")
+                .Trim();
+
+            if (string.IsNullOrEmpty(valorLimpo))
+                return 0;
+
+            bool temVirgula = valorLimpo.Contains(',');
+
+            if (temVirgula)
+            {
+                valorLimpo = valorLimpo.Replace(".", "").Replace(",", ".");
+            }
+
+            if (decimal.TryParse(valorLimpo, System.Globalization.NumberStyles.Any,
+                System.Globalization.CultureInfo.InvariantCulture, out decimal result))
+            {
+                return result;
+            }
+
+            return 0;
         }
 
     }
```
