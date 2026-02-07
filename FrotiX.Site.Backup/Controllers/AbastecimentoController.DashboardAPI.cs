using FrotiX.Models;
using FrotiX.Models.Estatisticas;
using FrotiX.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using FrotiX.Helpers; 
using FrotiX.Services;

namespace FrotiX.Controllers
{
    /*
    *  #################################################################################################
    *  #                                                                                               #
    *  #   PROJETO: FROTIX - SOLUÇÃO INTEGRADA DE GESTÃO DE FROTAS                                     #
    *  #   MODULO:  GESTÃO DE ABASTECIMENTOS (DASHBOARD API)                                           #
    *  #   DATA:    2026 (Modernização FrotiX 2026)                                                   #
    *  #                                                                                               #
    *  #################################################################################################
    */

    public partial class AbastecimentoController
    {
        #region Dashboard - Dados Gerais (OTIMIZADO)

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: DashboardDados (GET)                                          ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    KPIs consolidados do dashboard de abastecimentos.                        ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • ano (int?), mes (int?)                                                ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com KPIs e séries.                                 ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [Route("DashboardDados")]
        [HttpGet]
        public IActionResult DashboardDados(int? ano, int? mes)
        {
            try
            {
                // [LOGICA] Recuperar Anos disponíveis com dados reais (> 0)
                var anosDisponiveis = _context.EstatisticaAbastecimentoMensal
                    .Where(e => e.ValorTotal > 0)
                    .GroupBy(e => e.Ano)
                    .Select(g => g.Key)
                    .OrderByDescending(a => a)
                    .ToList();

                // [REGRA] Fallback se não houver estatísticas
                if (!anosDisponiveis.Any())
                {
                    return DashboardDadosFallback(ano, mes);
                }

                // FILTRO PADRÃO
                if ((!ano.HasValue || ano == 0) && (!mes.HasValue || mes == 0))
                {
                    var ultimoMes = _context.EstatisticaAbastecimentoMensal
                        .Where(e => e.Ano > 0 && e.Mes > 0 && e.ValorTotal > 0)
                        .OrderByDescending(e => e.Ano)
                        .ThenByDescending(e => e.Mes)
                        .FirstOrDefault();

                    if (ultimoMes != null)
                    {
                        ano = ultimoMes.Ano;
                        mes = ultimoMes.Mes;
                    }
                }

                // [DADOS] Resumo Anual
                var resumoPorAno = _context.EstatisticaAbastecimentoMensal
                    .GroupBy(e => e.Ano)
                    .Select(g => new
                    {
                        ano = g.Key,
                        valor = g.Sum(e => e.ValorTotal ?? 0),
                        litros = g.Sum(e => e.LitrosTotal ?? 0)
                    })
                    .Where(r => r.valor > 0)
                    .OrderBy(r => r.ano)
                    .ToList();

                // Filtrar estatísticas
                var queryEstatMensal = _context.EstatisticaAbastecimentoMensal.AsQueryable();
                var queryEstatComb = _context.EstatisticaAbastecimentoCombustivel.AsQueryable();
                var queryEstatCat = _context.EstatisticaAbastecimentoCategoria.AsQueryable();

                if (ano.HasValue && ano > 0)
                {
                    queryEstatMensal = queryEstatMensal.Where(e => e.Ano == ano.Value);
                    queryEstatComb = queryEstatComb.Where(e => e.Ano == ano.Value);
                    queryEstatCat = queryEstatCat.Where(e => e.Ano == ano.Value);
                }

                if (mes.HasValue && mes > 0)
                {
                    queryEstatMensal = queryEstatMensal.Where(e => e.Mes == mes.Value);
                    queryEstatComb = queryEstatComb.Where(e => e.Mes == mes.Value);
                    queryEstatCat = queryEstatCat.Where(e => e.Mes == mes.Value);
                }

                var totaisMensal = queryEstatMensal.ToList();
                var totais = new
                {
                    valorTotal = totaisMensal.Sum(e => e.ValorTotal ?? 0),
                    litrosTotal = totaisMensal.Sum(e => e.LitrosTotal ?? 0),
                    qtdAbastecimentos = totaisMensal.Sum(e => e.TotalAbastecimentos)
                };

                var combustivelData = queryEstatComb.ToList();
                var mediaLitro = combustivelData
                    .GroupBy(e => e.TipoCombustivel)
                    .Select(g => new
                    {
                        combustivel = g.Key,
                        media = g.Average(e => e.MediaValorLitro ?? 0)
                    })
                    .OrderBy(m => m.combustivel)
                    .ToList();

                // Valor por categoria (Real Time)
                var queryAbastecimentos = _unitOfWork.ViewAbastecimentos.GetAll().AsQueryable();
                if (ano.HasValue && ano > 0)
                    queryAbastecimentos = queryAbastecimentos.Where(a => a.DataHora.HasValue && a.DataHora.Value.Year == ano.Value);
                if (mes.HasValue && mes > 0)
                    queryAbastecimentos = queryAbastecimentos.Where(a => a.DataHora.HasValue && a.DataHora.Value.Month == mes.Value);

                var dadosAbastecimentos = queryAbastecimentos.ToList();

                var veiculosCategorias = _unitOfWork.ViewVeiculos.GetAll()
                    .Where(v => !string.IsNullOrEmpty(v.Categoria))
                    .ToDictionary(v => v.VeiculoId, v => v.Categoria ?? "Sem Categoria");

                var valorPorCategoriaList = dadosAbastecimentos
                    .Where(a => a.VeiculoId != Guid.Empty && veiculosCategorias.ContainsKey(a.VeiculoId))
                    .GroupBy(a => veiculosCategorias[a.VeiculoId])
                    .Select(g => new
                    {
                        categoria = g.Key,
                        valor = g.Sum(a => ParseDecimal(a.ValorTotal))
                    })
                    .OrderByDescending(v => v.valor)
                    .ToList();

                var semCategoria = dadosAbastecimentos
                    .Where(a => a.VeiculoId == Guid.Empty || !veiculosCategorias.ContainsKey(a.VeiculoId))
                    .Sum(a => ParseDecimal(a.ValorTotal));

                if (semCategoria > 0)
                {
                    valorPorCategoriaList.Add(new { categoria = "Sem Categoria", valor = semCategoria });
                    valorPorCategoriaList = valorPorCategoriaList.OrderByDescending(v => v.valor).ToList();
                }

                var valorPorCategoria = valorPorCategoriaList;

                var valorLitroPorMes = combustivelData
                    .Select(e => new
                    {
                        mes = e.Mes,
                        combustivel = e.TipoCombustivel,
                        media = e.MediaValorLitro ?? 0
                    })
                    .OrderBy(v => v.mes)
                    .ToList();

                var litrosPorMes = combustivelData
                    .Select(e => new
                    {
                        mes = e.Mes,
                        combustivel = e.TipoCombustivel,
                        litros = e.LitrosTotal ?? 0
                    })
                    .OrderBy(l => l.mes)
                    .ToList();

                var consumoPorMes = totaisMensal
                    .GroupBy(e => e.Mes)
                    .Select(g => new
                    {
                        mes = g.Key,
                        valor = g.Sum(e => e.ValorTotal ?? 0)
                    })
                    .OrderBy(c => c.mes)
                    .ToList();

                var resultado = new
                {
                    anosDisponiveis,
                    resumoPorAno,
                    mediaLitro,
                    valorPorCategoria,
                    valorLitroPorMes,
                    litrosPorMes,
                    consumoPorMes,
                    totais,
                    filtroAplicado = new
                    {
                        ano = ano ?? 0,
                        mes = mes ?? 0
                    }
                };

                return Ok(resultado);
            }
            catch (Exception error)
            {
                _logger.LogError(error.Message, error, "AbastecimentoController.DashboardAPI.cs", "DashboardDados");
                Alerta.TratamentoErroComLinha("AbastecimentoController.DashboardAPI.cs", "DashboardDados", error);
                return StatusCode(500, new { message = "Erro ao carregar dados do dashboard" });
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: DashboardDadosPeriodo                                               ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Carrega dados do dashboard para um período específico (data início/fim).  ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • dataInicio (DateTime?): Data inicial.                                   ║
        /// ║    • dataFim (DateTime?): Data final.                                        ║
        /// ║                                                                              ║
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON consolidado do período.                             ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [Route("DashboardDadosPeriodo")]
        [HttpGet]
        public IActionResult DashboardDadosPeriodo(DateTime? dataInicio, DateTime? dataFim)
        {
            try
            {
                if (!dataInicio.HasValue || !dataFim.HasValue)
                {
                    return BadRequest(new { message = "Data início e fim são obrigatórias para busca por período" });
                }

                var query = _unitOfWork.ViewAbastecimentos.GetAll()
                    .Where(a => a.DataHora.HasValue && a.DataHora.Value >= dataInicio.Value && a.DataHora.Value <= dataFim.Value.AddDays(1).AddSeconds(-1));

                var dados = query.ToList();

                var veiculosCategoriasPeriodo = _unitOfWork.ViewVeiculos.GetAll()
                    .Where(v => !string.IsNullOrEmpty(v.Categoria))
                    .ToDictionary(v => v.VeiculoId, v => v.Categoria ?? "Sem Categoria");

                var valorPorCategoriaPeriodo = dados
                    .Where(a => a.VeiculoId != Guid.Empty && veiculosCategoriasPeriodo.ContainsKey(a.VeiculoId))
                    .GroupBy(a => veiculosCategoriasPeriodo[a.VeiculoId])
                    .Select(g => new
                    {
                        categoria = g.Key,
                        valor = g.Sum(a => ParseDecimal(a.ValorTotal))
                    })
                    .OrderByDescending(v => v.valor)
                    .ToList();

                var semCategoriaPeriodo = dados
                    .Where(a => a.VeiculoId == Guid.Empty || !veiculosCategoriasPeriodo.ContainsKey(a.VeiculoId))
                    .Sum(a => ParseDecimal(a.ValorTotal));

                if (semCategoriaPeriodo > 0)
                {
                    valorPorCategoriaPeriodo.Add(new { categoria = "Sem Categoria", valor = semCategoriaPeriodo });
                    valorPorCategoriaPeriodo = valorPorCategoriaPeriodo.OrderByDescending(v => v.valor).ToList();
                }

                var resultado = new
                {
                    anosDisponiveis = _unitOfWork.ViewAbastecimentos.GetAll()
                        .Where(a => a.DataHora.HasValue)
                        .Select(a => a.DataHora.Value.Year)
                        .Distinct()
                        .OrderByDescending(a => a)
                        .ToList(),

                    resumoPorAno = dados
                        .Where(a => a.DataHora.HasValue)
                        .GroupBy(a => a.DataHora.Value.Year)
                        .Select(g => new
                        {
                            ano = g.Key,
                            valor = g.Sum(a => ParseDecimal(a.ValorTotal)),
                            litros = g.Sum(a => ParseDecimal(a.Litros))
                        })
                        .OrderBy(r => r.ano)
                        .ToList(),

                    mediaLitro = dados
                        .Where(a => !string.IsNullOrEmpty(a.TipoCombustivel))
                        .GroupBy(a => a.TipoCombustivel)
                        .Select(g => new
                        {
                            combustivel = g.Key,
                            media = g.Average(a => ParseDecimal(a.ValorUnitario))
                        })
                        .OrderBy(m => m.combustivel)
                        .ToList(),

                    valorPorCategoria = valorPorCategoriaPeriodo,

                    valorLitroPorMes = dados
                        .Where(a => a.DataHora.HasValue && !string.IsNullOrEmpty(a.TipoCombustivel))
                        .GroupBy(a => new { Mes = a.DataHora.Value.Month, Combustivel = a.TipoCombustivel })
                        .Select(g => new
                        {
                            mes = g.Key.Mes,
                            combustivel = g.Key.Combustivel,
                            media = g.Average(a => ParseDecimal(a.ValorUnitario))
                        })
                        .OrderBy(v => v.mes)
                        .ToList(),

                    litrosPorMes = dados
                        .Where(a => a.DataHora.HasValue)
                        .GroupBy(a => new { Mes = a.DataHora.Value.Month, Combustivel = a.TipoCombustivel ?? "Outros" })
                        .Select(g => new
                        {
                            mes = g.Key.Mes,
                            combustivel = g.Key.Combustivel,
                            litros = g.Sum(a => ParseDecimal(a.Litros))
                        })
                        .OrderBy(l => l.mes)
                        .ToList(),

                    consumoPorMes = dados
                        .Where(a => a.DataHora.HasValue)
                        .GroupBy(a => a.DataHora.Value.Month)
                        .Select(g => new
                        {
                            mes = g.Key,
                            valor = g.Sum(a => ParseDecimal(a.ValorTotal))
                        })
                        .OrderBy(c => c.mes)
                        .ToList(),

                    totais = new
                    {
                        valorTotal = dados.Sum(a => ParseDecimal(a.ValorTotal)),
                        litrosTotal = dados.Sum(a => ParseDecimal(a.Litros)),
                        qtdAbastecimentos = dados.Count
                    }
                };

                return Ok(resultado);
            }
            catch (Exception error)
            {
                _logger.LogError(error.Message, error, "AbastecimentoController.DashboardAPI.cs", "DashboardDadosPeriodo");
                Alerta.TratamentoErroComLinha("AbastecimentoController.DashboardAPI.cs", "DashboardDadosPeriodo", error);
                return StatusCode(500, new { message = "Erro ao carregar dados do dashboard por período" });
            }
        }

        private IActionResult DashboardDadosFallback(int? ano, int? mes)
        {
            if ((!ano.HasValue || ano == 0) && (!mes.HasValue || mes == 0))
            {
                var ultimaData = _unitOfWork.ViewAbastecimentos.GetAll()
                    .Where(a => a.DataHora.HasValue)
                    .OrderByDescending(a => a.DataHora)
                    .Select(a => a.DataHora!.Value)
                    .FirstOrDefault();

                if (ultimaData != default)
                {
                    ano = ultimaData.Year;
                    mes = ultimaData.Month;
                }
            }

            var query = _unitOfWork.ViewAbastecimentos.GetAll().AsQueryable();

            if (ano.HasValue && ano > 0)
                query = query.Where(a => a.DataHora.HasValue && a.DataHora.Value.Year == ano.Value);

            if (mes.HasValue && mes > 0)
                query = query.Where(a => a.DataHora.HasValue && a.DataHora.Value.Month == mes.Value);

            var dados = query.ToList();

            var veiculosCategoriasFallback = _unitOfWork.ViewVeiculos.GetAll()
                .Where(v => !string.IsNullOrEmpty(v.Categoria))
                .ToDictionary(v => v.VeiculoId, v => v.Categoria ?? "Sem Categoria");

            var valorPorCategoriaFallback = dados
                .Where(a => a.VeiculoId != Guid.Empty && veiculosCategoriasFallback.ContainsKey(a.VeiculoId))
                .GroupBy(a => veiculosCategoriasFallback[a.VeiculoId])
                .Select(g => new
                {
                    categoria = g.Key,
                    valor = g.Sum(a => ParseDecimal(a.ValorTotal))
                })
                .OrderByDescending(v => v.valor)
                .ToList();

            var semCategoriaFallback = dados
                .Where(a => a.VeiculoId == Guid.Empty || !veiculosCategoriasFallback.ContainsKey(a.VeiculoId))
                .Sum(a => ParseDecimal(a.ValorTotal));

            if (semCategoriaFallback > 0)
            {
                valorPorCategoriaFallback.Add(new { categoria = "Sem Categoria", valor = semCategoriaFallback });
                valorPorCategoriaFallback = valorPorCategoriaFallback.OrderByDescending(v => v.valor).ToList();
            }

            var resultado = new
            {
                anosDisponiveis = _unitOfWork.ViewAbastecimentos.GetAll()
                    .Where(a => a.DataHora.HasValue)
                    .Select(a => a.DataHora.Value.Year)
                    .Distinct()
                    .OrderByDescending(a => a)
                    .ToList(),

                resumoPorAno = (from a in _unitOfWork.ViewAbastecimentos.GetAll()
                                where a.DataHora.HasValue
                                group a by a.DataHora.Value.Year into g
                                orderby g.Key descending
                                select new
                                {
                                    ano = g.Key,
                                    valor = g.Sum(a => ParseDecimal(a.ValorTotal)),
                                    litros = g.Sum(a => ParseDecimal(a.Litros))
                                })
                               .Take(3)
                               .OrderBy(r => r.ano)
                               .ToList(),

                mediaLitro = dados
                    .Where(a => !string.IsNullOrEmpty(a.TipoCombustivel))
                    .GroupBy(a => a.TipoCombustivel)
                    .Select(g => new
                    {
                        combustivel = g.Key,
                        media = g.Average(a => ParseDecimal(a.ValorUnitario))
                    })
                    .OrderBy(m => m.combustivel)
                    .ToList(),

                valorPorCategoria = valorPorCategoriaFallback,

                valorLitroPorMes = dados
                    .Where(a => a.DataHora.HasValue && !string.IsNullOrEmpty(a.TipoCombustivel))
                    .GroupBy(a => new { Mes = a.DataHora.Value.Month, Combustivel = a.TipoCombustivel })
                    .Select(g => new
                    {
                        mes = g.Key.Mes,
                        combustivel = g.Key.Combustivel,
                        media = g.Average(a => ParseDecimal(a.ValorUnitario))
                    })
                    .OrderBy(v => v.mes)
                    .ToList(),

                litrosPorMes = dados
                    .Where(a => a.DataHora.HasValue)
                    .GroupBy(a => new { Mes = a.DataHora.Value.Month, Combustivel = a.TipoCombustivel ?? "Outros" })
                    .Select(g => new
                    {
                        mes = g.Key.Mes,
                        combustivel = g.Key.Combustivel,
                        litros = g.Sum(a => ParseDecimal(a.Litros))
                    })
                    .OrderBy(l => l.mes)
                    .ToList(),

                consumoPorMes = dados
                    .Where(a => a.DataHora.HasValue)
                    .GroupBy(a => a.DataHora.Value.Month)
                    .Select(g => new
                    {
                        mes = g.Key,
                        valor = g.Sum(a => ParseDecimal(a.ValorTotal))
                    })
                    .OrderBy(c => c.mes)
                    .ToList(),

                totais = new
                {
                    valorTotal = dados.Sum(a => ParseDecimal(a.ValorTotal)),
                    litrosTotal = dados.Sum(a => ParseDecimal(a.Litros)),
                    qtdAbastecimentos = dados.Count
                },

                filtroAplicado = new
                {
                    ano = ano ?? 0,
                    mes = mes ?? 0
                }
            };

            return Ok(resultado);
        }

        #endregion

        /* > ---------------------------------------------------------------------------------------
         > 📊 **DASHBOARD - CONSUMO MENSAL**
         > --------------------------------------------------------------------------------------- */
        #region Dashboard - Consumo Mensal (OTIMIZADO)

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: DashboardMensal                                                     ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Retorna dados específicos para a aba Consumo Mensal.                      ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • ano (int): Ano para filtro.                                             ║
        /// ║    • mes (int?): Mês para filtro.                                            ║
        /// ║                                                                              ║
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com dados mensais detalhados.                       ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [Route("DashboardMensal")]
        [HttpGet]
        public IActionResult DashboardMensal(int ano, int? mes)
        {
            try
            {
                var temDadosEstatisticos = _context.EstatisticaAbastecimentoMensal
                    .Any(e => e.Ano == ano);

                if (!temDadosEstatisticos)
                {
                    _logger.LogWarning($"Dados estatísticos não encontrados para o ano {ano}. Iniciando Fallback.");
                    return DashboardMensalFallback(ano, mes);
                }

                var queryMensal = _context.EstatisticaAbastecimentoMensal
                    .Where(e => e.Ano == ano);

                var queryComb = _context.EstatisticaAbastecimentoCombustivel
                    .Where(e => e.Ano == ano);

                var queryCat = _context.EstatisticaAbastecimentoCategoria
                    .Where(e => e.Ano == ano);

                var queryTipo = _context.EstatisticaAbastecimentoTipoVeiculo
                    .Where(e => e.Ano == ano);

                if (mes.HasValue && mes > 0)
                {
                    queryMensal = queryMensal.Where(e => e.Mes == mes.Value);
                    queryComb = queryComb.Where(e => e.Mes == mes.Value);
                    queryCat = queryCat.Where(e => e.Mes == mes.Value);
                    queryTipo = queryTipo.Where(e => e.Mes == mes.Value);
                }

                var estatMensal = queryMensal.ToList();
                var estatComb = queryComb.ToList();
                var estatCat = queryCat.ToList();
                
                var valorTotal = estatMensal.Sum(e => e.ValorTotal ?? 0);
                var litrosTotal = estatMensal.Sum(e => e.LitrosTotal ?? 0);

                var porCombustivel = estatComb
                    .GroupBy(e => e.TipoCombustivel)
                    .Select(g => new
                    {
                        combustivel = g.Key,
                        valor = g.Sum(e => e.ValorTotal ?? 0),
                        litros = g.Sum(e => e.LitrosTotal ?? 0)
                    })
                    .OrderByDescending(p => p.valor)
                    .ToList();

                var mediaLitro = estatComb
                    .GroupBy(e => e.TipoCombustivel)
                    .Select(g => new
                    {
                        combustivel = g.Key,
                        media = g.Average(e => e.MediaValorLitro ?? 0)
                    })
                    .ToList();

                var litrosPorDia = new List<object>();
                if (mes.HasValue && mes > 0)
                {
                    var dadosMes = _unitOfWork.ViewAbastecimentos.GetAll()
                        .Where(a => a.DataHora.HasValue && a.DataHora.Value.Year == ano && a.DataHora.Value.Month == mes.Value)
                        .ToList();

                    litrosPorDia = dadosMes
                        .GroupBy(a => new { Dia = a.DataHora.Value.Day, Combustivel = a.TipoCombustivel ?? "Outros" })
                        .Select(g => (object)new
                        {
                            dia = g.Key.Dia,
                            combustivel = g.Key.Combustivel,
                            litros = g.Sum(a => ParseDecimal(a.Litros))
                        })
                        .OrderBy(l => ((dynamic)l).dia)
                        .ToList();
                }

                var queryAbastecimentosView = _unitOfWork.ViewAbastecimentos.GetAll()
                    .Where(a => a.DataHora.HasValue && a.DataHora.Value.Year == ano);

                if (mes.HasValue && mes > 0)
                    queryAbastecimentosView = queryAbastecimentosView.Where(a => a.DataHora!.Value.Month == mes.Value);

                var dadosAbastecimentos = queryAbastecimentosView.ToList();

                var valorPorUnidade = dadosAbastecimentos
                    .Where(a => !string.IsNullOrEmpty(a.Sigla))
                    .GroupBy(a => a.Sigla)
                    .Select(g => new
                    {
                        unidade = g.Key,
                        valor = g.Sum(a => ParseDecimal(a.ValorTotal))
                    })
                    .OrderByDescending(v => v.valor)
                    .Take(15)
                    .ToList();

                var valorPorPlaca = dadosAbastecimentos
                    .Where(a => !string.IsNullOrEmpty(a.Placa))
                    .GroupBy(a => new { a.VeiculoId, a.Placa, a.TipoVeiculo })
                    .Select(g => new
                    {
                        veiculoId = g.Key.VeiculoId,
                        placa = g.Key.Placa ?? "",
                        tipoVeiculo = g.Key.TipoVeiculo ?? "",
                        valor = g.Sum(a => ParseDecimal(a.ValorTotal))
                    })
                    .OrderByDescending(v => v.valor)
                    .Take(15)
                    .ToList();

                var consumoPorCategoria = estatCat
                    .GroupBy(e => e.Categoria)
                    .Select(g => new
                    {
                        categoria = g.Key,
                        valor = g.Sum(e => e.ValorTotal ?? 0)
                    })
                    .OrderByDescending(c => c.valor)
                    .ToList();

                var resultado = new
                {
                    valorTotal,
                    litrosTotal,
                    porCombustivel,
                    mediaLitro,
                    litrosPorDia,
                    valorPorUnidade,
                    valorPorPlaca,
                    consumoPorCategoria
                };

                return Ok(resultado);
            }
            catch (Exception error)
            {
                _logger.LogError($", errorErro ao carregar DashboardMensal para ano={ano}, mes={mes}");
                Alerta.TratamentoErroComLinha("AbastecimentoController.DashboardAPI.cs", "DashboardMensal", error);
                return StatusCode(500, new { message = "Erro ao carregar dados mensais" });
            }
        }

        private IActionResult DashboardMensalFallback(int ano, int? mes)
        {
            var query = _unitOfWork.ViewAbastecimentos.GetAll()
                .Where(a => a.DataHora.HasValue && a.DataHora.Value.Year == ano);

            if (mes.HasValue && mes > 0)
                query = query.Where(a => a.DataHora.Value.Month == mes.Value);

            var dados = query.ToList();

            var todosVeiculos = _unitOfWork.ViewVeiculos.GetAll()
                .Where(v => !string.IsNullOrEmpty(v.Categoria))
                .ToList();

            var veiculosCategorias = todosVeiculos.ToDictionary(
                v => v.VeiculoId,
                v => v.Categoria ?? "Sem Categoria"
            );

            var consumoPorCategoriaReal = dados
                .Where(a => a.VeiculoId != Guid.Empty && veiculosCategorias.ContainsKey(a.VeiculoId))
                .GroupBy(a => veiculosCategorias[a.VeiculoId])
                .Select(g => new
                {
                    categoria = g.Key,
                    valor = g.Sum(a => ParseDecimal(a.ValorTotal))
                })
                .OrderByDescending(c => c.valor)
                .ToList();

            var semCategoria = dados
                .Where(a => a.VeiculoId == Guid.Empty || !veiculosCategorias.ContainsKey(a.VeiculoId))
                .Sum(a => ParseDecimal(a.ValorTotal));

            if (semCategoria > 0)
            {
                var lista = consumoPorCategoriaReal.ToList();
                lista.Add(new { categoria = "Sem Categoria", valor = semCategoria });
                consumoPorCategoriaReal = lista.OrderByDescending(c => c.valor).ToList();
            }

            var resultado = new
            {
                valorTotal = dados.Sum(a => ParseDecimal(a.ValorTotal)),
                litrosTotal = dados.Sum(a => ParseDecimal(a.Litros)),

                porCombustivel = dados
                    .Where(a => !string.IsNullOrEmpty(a.TipoCombustivel))
                    .GroupBy(a => a.TipoCombustivel)
                    .Select(g => new
                    {
                        combustivel = g.Key,
                        valor = g.Sum(a => ParseDecimal(a.ValorTotal)),
                        litros = g.Sum(a => ParseDecimal(a.Litros))
                    })
                    .OrderByDescending(p => p.valor)
                    .ToList(),

                mediaLitro = dados
                    .Where(a => !string.IsNullOrEmpty(a.TipoCombustivel))
                    .GroupBy(a => a.TipoCombustivel)
                    .Select(g => new
                    {
                        combustivel = g.Key,
                        media = g.Average(a => ParseDecimal(a.ValorUnitario))
                    })
                    .ToList(),

                litrosPorDia = dados
                    .Where(a => a.DataHora.HasValue)
                    .GroupBy(a => new { Dia = a.DataHora.Value.Day, Combustivel = a.TipoCombustivel ?? "Outros" })
                    .Select(g => new
                    {
                        dia = g.Key.Dia,
                        combustivel = g.Key.Combustivel,
                        litros = g.Sum(a => ParseDecimal(a.Litros))
                    })
                    .OrderBy(l => l.dia)
                    .ToList(),

                valorPorUnidade = dados
                    .Where(a => !string.IsNullOrEmpty(a.Sigla))
                    .GroupBy(a => a.Sigla)
                    .Select(g => new
                    {
                        unidade = g.Key,
                        valor = g.Sum(a => ParseDecimal(a.ValorTotal))
                    })
                    .OrderByDescending(v => v.valor)
                    .Take(15)
                    .ToList(),

                valorPorPlaca = dados
                    .Where(a => !string.IsNullOrEmpty(a.Placa))
                    .GroupBy(a => new { a.VeiculoId, a.Placa, a.TipoVeiculo })
                    .Select(g => new
                    {
                        veiculoId = g.Key.VeiculoId,
                        placa = g.Key.Placa,
                        tipoVeiculo = g.Key.TipoVeiculo ?? "",
                        valor = g.Sum(a => ParseDecimal(a.ValorTotal))
                    })
                    .OrderByDescending(v => v.valor)
                    .Take(15)
                    .ToList(),

                consumoPorCategoria = consumoPorCategoriaReal
            };

            return Ok(resultado);
        }

        #endregion

        /* > ---------------------------------------------------------------------------------------
         > 🚗 **DASHBOARD - CONSUMO POR VEÍCULO**
         > --------------------------------------------------------------------------------------- */
        #region Dashboard - Consumo por Veículo (OTIMIZADO)

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: DashboardVeiculo                                                    ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Retorna dados específicos para a aba Consumo por Veículo.                   ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • ano (int): Ano para filtro.                                             ║
        /// ║    • mes (int?): Mês para filtro.                                            ║
        /// ║    • veiculoId (Guid?): ID do veículo específico.                            ║
        /// ║    • tipoVeiculo (string?): Tipo de veículo.                                 ║
        /// ║                                                                              ║
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com dados detalhados por veículo.                   ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [Route("DashboardVeiculo")]
        [HttpGet]
        public IActionResult DashboardVeiculo(int ano, int? mes, Guid? veiculoId, string? tipoVeiculo)
        {
            try
            {
                var temDadosEstatisticos = _context.EstatisticaAbastecimentoVeiculo
                    .Any(e => e.Ano == ano);

                if (!temDadosEstatisticos)
                {
                    _logger.LogWarning($"Dados estatísticos de veículo não encontrados para o ano {ano}. Iniciando Fallback.");
                    return DashboardVeiculoFallback(ano, mes, veiculoId, tipoVeiculo);
                }

                List<string?> modelosDisponiveis;
                List<object> placasDisponiveis;

                if (mes.HasValue && mes > 0)
                {
                    var veiculosComAbastecimentoNoMes = _context.EstatisticaAbastecimentoVeiculoMensal
                        .Where(e => e.Ano == ano && e.Mes == mes.Value)
                        .Select(e => e.VeiculoId)
                        .Distinct()
                        .ToList();

                    modelosDisponiveis = _context.EstatisticaAbastecimentoVeiculo
                        .Where(e => e.Ano == ano && !string.IsNullOrEmpty(e.TipoVeiculo) && veiculosComAbastecimentoNoMes.Contains(e.VeiculoId))
                        .Select(e => e.TipoVeiculo)
                        .Distinct()
                        .OrderBy(m => m)
                        .ToList();

                    placasDisponiveis = _context.EstatisticaAbastecimentoVeiculo
                        .Where(e => e.Ano == ano && !string.IsNullOrEmpty(e.Placa) && veiculosComAbastecimentoNoMes.Contains(e.VeiculoId))
                        .Select(e => new { e.VeiculoId, e.Placa, e.TipoVeiculo })
                        .Distinct()
                        .OrderBy(p => p.Placa)
                        .ToList<object>();
                }
                else
                {
                    modelosDisponiveis = _context.EstatisticaAbastecimentoTipoVeiculo
                        .Where(e => e.Ano == ano && !string.IsNullOrEmpty(e.TipoVeiculo))
                        .Select(e => e.TipoVeiculo)
                        .Distinct()
                        .OrderBy(m => m)
                        .ToList();

                    placasDisponiveis = _context.EstatisticaAbastecimentoVeiculo
                        .Where(e => e.Ano == ano && !string.IsNullOrEmpty(e.Placa))
                        .Select(e => new { e.VeiculoId, e.Placa, e.TipoVeiculo })
                        .Distinct()
                        .OrderBy(p => p.Placa)
                        .ToList<object>();
                }

                var queryVeiculos = _context.EstatisticaAbastecimentoVeiculo
                    .Where(e => e.Ano == ano);

                if (!string.IsNullOrEmpty(tipoVeiculo))
                    queryVeiculos = queryVeiculos.Where(e => e.TipoVeiculo == tipoVeiculo);

                var veiculosComValor = queryVeiculos
                    .OrderByDescending(v => v.ValorTotal)
                    .Select(v => new
                    {
                        veiculoId = v.VeiculoId,
                        placa = v.Placa,
                        tipoVeiculo = v.TipoVeiculo,
                        valor = v.ValorTotal ?? 0
                    })
                    .ToList();

                decimal valorTotal = 0;
                decimal litrosTotal = 0;
                string descricaoVeiculo = "Todos os veículos";
                string categoriaVeiculo = "-";
                var consumoMensalLitros = new List<object>();
                var valorMensal = new List<object>();

                if (veiculoId.HasValue && veiculoId.Value != Guid.Empty)
                {
                    var veiculoMensalTodos = _context.EstatisticaAbastecimentoVeiculoMensal
                        .Where(e => e.Ano == ano && e.VeiculoId == veiculoId.Value)
                        .ToList();

                    if (mes.HasValue && mes > 0)
                    {
                        var dadosFiltrados = veiculoMensalTodos.Where(e => e.Mes == mes.Value).ToList();
                        valorTotal = dadosFiltrados.Sum(e => e.ValorTotal ?? 0);
                        litrosTotal = dadosFiltrados.Sum(e => e.LitrosTotal ?? 0);
                    }
                    else
                    {
                        valorTotal = veiculoMensalTodos.Sum(e => e.ValorTotal ?? 0);
                        litrosTotal = veiculoMensalTodos.Sum(e => e.LitrosTotal ?? 0);
                    }

                    var veiculoEstat = _context.EstatisticaAbastecimentoVeiculo
                        .FirstOrDefault(e => e.Ano == ano && e.VeiculoId == veiculoId.Value);

                    if (veiculoEstat != null)
                    {
                        descricaoVeiculo = veiculoEstat.Placa + " - " + veiculoEstat.TipoVeiculo;
                        categoriaVeiculo = veiculoEstat.TipoVeiculo ?? "-";
                    }

                    var dadosViewTodos = _unitOfWork.ViewAbastecimentos.GetAll()
                        .Where(a => a.DataHora.HasValue && a.DataHora.Value.Year == ano && a.VeiculoId == veiculoId.Value)
                        .ToList();

                    consumoMensalLitros = dadosViewTodos
                        .GroupBy(a => new { Mes = a.DataHora.Value.Month, Combustivel = a.TipoCombustivel ?? "Outros" })
                        .Select(g => (object)new
                        {
                            mes = g.Key.Mes,
                            combustivel = g.Key.Combustivel,
                            litros = g.Sum(a => ParseDecimal(a.Litros))
                        })
                        .OrderBy(c => ((dynamic)c).mes)
                        .ToList();

                    valorMensal = veiculoMensalTodos
                        .Select(e => (object)new
                        {
                            mes = e.Mes,
                            valor = e.ValorTotal ?? 0
                        })
                        .OrderBy(v => ((dynamic)v).mes)
                        .ToList();
                }
                else if (!string.IsNullOrEmpty(tipoVeiculo))
                {
                    descricaoVeiculo = tipoVeiculo;
                    categoriaVeiculo = tipoVeiculo;

                    var tipoQuery = _context.EstatisticaAbastecimentoTipoVeiculo
                        .Where(e => e.Ano == ano && e.TipoVeiculo == tipoVeiculo);

                    if (mes.HasValue && mes > 0)
                        tipoQuery = tipoQuery.Where(e => e.Mes == mes.Value);

                    var tipoData = tipoQuery.ToList();
                    valorTotal = tipoData.Sum(e => e.ValorTotal ?? 0);
                    litrosTotal = tipoData.Sum(e => e.LitrosTotal ?? 0);

                    var dadosView = _unitOfWork.ViewAbastecimentos.GetAll()
                        .Where(a => a.DataHora.HasValue && a.DataHora.Value.Year == ano && a.TipoVeiculo == tipoVeiculo)
                        .ToList();

                    if (mes.HasValue && mes > 0)
                        dadosView = dadosView.Where(a => a.DataHora.Value.Month == mes.Value).ToList();

                    consumoMensalLitros = dadosView
                        .GroupBy(a => new { Mes = a.DataHora.Value.Month, Combustivel = a.TipoCombustivel ?? "Outros" })
                        .Select(g => (object)new
                        {
                            mes = g.Key.Mes,
                            combustivel = g.Key.Combustivel,
                            litros = g.Sum(a => ParseDecimal(a.Litros))
                        })
                        .OrderBy(c => ((dynamic)c).mes)
                        .ToList();

                    valorMensal = tipoData
                        .Select(e => (object)new
                        {
                            mes = e.Mes,
                            valor = e.ValorTotal ?? 0
                        })
                        .OrderBy(v => ((dynamic)v).mes)
                        .ToList();
                }
                else
                {
                    var todosMesesData = _context.EstatisticaAbastecimentoMensal
                        .Where(e => e.Ano == ano)
                        .ToList();

                    if (mes.HasValue && mes > 0)
                    {
                        var dadosFiltrados = todosMesesData.Where(e => e.Mes == mes.Value).ToList();
                        valorTotal = dadosFiltrados.Sum(e => e.ValorTotal ?? 0);
                        litrosTotal = dadosFiltrados.Sum(e => e.LitrosTotal ?? 0);
                    }
                    else
                    {
                        valorTotal = todosMesesData.Sum(e => e.ValorTotal ?? 0);
                        litrosTotal = todosMesesData.Sum(e => e.LitrosTotal ?? 0);
                    }

                    var todosCombData = _context.EstatisticaAbastecimentoCombustivel
                        .Where(e => e.Ano == ano)
                        .ToList();

                    consumoMensalLitros = todosCombData
                        .Select(e => (object)new
                        {
                            mes = e.Mes,
                            combustivel = e.TipoCombustivel,
                            litros = e.LitrosTotal ?? 0
                        })
                        .OrderBy(c => ((dynamic)c).mes)
                        .ToList();

                    valorMensal = todosMesesData
                        .Select(e => (object)new
                        {
                            mes = e.Mes,
                            valor = e.ValorTotal ?? 0
                        })
                        .OrderBy(v => ((dynamic)v).mes)
                        .ToList();
                }

                var resultado = new
                {
                    valorTotal,
                    litrosTotal,
                    descricaoVeiculo,
                    categoriaVeiculo,
                    consumoMensalLitros,
                    valorMensal,
                    veiculosComValor,
                    modelosDisponiveis,
                    placasDisponiveis,
                    mesSelecionado = mes ?? 0
                };

                return Ok(resultado);
            }
            catch (Exception error)
            {
                _logger.LogError($", errorErro ao carregar DashboardVeiculo para ano={ano}, mes={mes}");
                Alerta.TratamentoErroComLinha("AbastecimentoController.DashboardAPI.cs", "DashboardVeiculo", error);
                return StatusCode(500, new { message = "Erro ao carregar dados por veículo" });
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: DashboardDadosVeiculoPeriodo                                        ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Endpoint para dados de veículo filtrados por período personalizado.       ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • dataInicio (DateTime?): Data inicial.                                   ║
        /// ║    • dataFim (DateTime?): Data final.                                        ║
        /// ║                                                                              ║
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON filtrado por período.                               ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [Route("DashboardDadosVeiculoPeriodo")]
        [HttpGet]
        public IActionResult DashboardDadosVeiculoPeriodo(DateTime? dataInicio, DateTime? dataFim)
        {
            try
            {
                if (!dataInicio.HasValue || !dataFim.HasValue)
                {
                    return BadRequest(new { message = "Data de início e fim são obrigatórias" });
                }

                var query = _unitOfWork.ViewAbastecimentos.GetAll()
                    .Where(a => a.DataHora.HasValue &&
                           a.DataHora.Value.Date >= dataInicio.Value.Date &&
                           a.DataHora.Value.Date <= dataFim.Value.Date);

                var dados = query.ToList();

                var modelosDisponiveis = dados
                    .Where(a => !string.IsNullOrEmpty(a.TipoVeiculo))
                    .Select(a => a.TipoVeiculo)
                    .Distinct()
                    .OrderBy(m => m)
                    .ToList();

                var placasDisponiveis = dados
                    .Where(a => !string.IsNullOrEmpty(a.Placa))
                    .Select(a => new { a.VeiculoId, a.Placa })
                    .Distinct()
                    .OrderBy(p => p.Placa)
                    .ToList();

                var veiculosComValor = dados
                    .GroupBy(a => new { a.VeiculoId, a.Placa, a.TipoVeiculo })
                    .Select(g => new
                    {
                        veiculoId = g.Key.VeiculoId,
                        placa = g.Key.Placa,
                        tipoVeiculo = g.Key.TipoVeiculo,
                        valor = g.Sum(a => ParseDecimal(a.ValorTotal))
                    })
                    .OrderByDescending(v => v.valor)
                    .ToList();

                decimal valorTotal = dados.Sum(a => ParseDecimal(a.ValorTotal));
                decimal litrosTotal = dados.Sum(a => ParseDecimal(a.Litros));

                var consumoMensalLitros = dados
                    .GroupBy(a => new { Mes = a.DataHora!.Value.Month, Combustivel = a.TipoCombustivel ?? "Outros" })
                    .Select(g => (object)new
                    {
                        mes = g.Key.Mes,
                        combustivel = g.Key.Combustivel,
                        litros = g.Sum(a => ParseDecimal(a.Litros))
                    })
                    .OrderBy(c => ((dynamic)c).mes)
                    .ToList();

                var valorMensal = dados
                    .GroupBy(a => a.DataHora!.Value.Month)
                    .Select(g => (object)new
                    {
                        mes = g.Key,
                        valor = g.Sum(a => ParseDecimal(a.ValorTotal))
                    })
                    .OrderBy(v => ((dynamic)v).mes)
                    .ToList();

                var resultado = new
                {
                    valorTotal,
                    litrosTotal,
                    descricaoVeiculo = "Todos os veículos",
                    categoriaVeiculo = "-",
                    consumoMensalLitros,
                    valorMensal,
                    veiculosComValor,
                    modelosDisponiveis,
                    placasDisponiveis
                };

                return Ok(resultado);
            }
            catch (Exception error)
            {
                _logger.LogError($", errorErro ao carregar DashboardDadosVeiculoPeriodo para {dataInicio} até {dataFim}");
                Alerta.TratamentoErroComLinha("AbastecimentoController.DashboardAPI.cs", "DashboardDadosVeiculoPeriodo", error);
                return StatusCode(500, new { message = "Erro ao carregar dados por veículo por período" });
            }
        }

        private IActionResult DashboardVeiculoFallback(int ano, int? mes, Guid? veiculoId, string? tipoVeiculo)
        {
            var query = _unitOfWork.ViewAbastecimentos.GetAll()
                .Where(a => a.DataHora.HasValue && a.DataHora.Value.Year == ano);

            if (mes.HasValue && mes > 0)
                query = query.Where(a => a.DataHora.Value.Month == mes.Value);

            if (veiculoId.HasValue && veiculoId.Value != Guid.Empty)
                query = query.Where(a => a.VeiculoId == veiculoId.Value);

            if (!string.IsNullOrEmpty(tipoVeiculo))
                query = query.Where(a => a.TipoVeiculo == tipoVeiculo);

            var dados = query.ToList();

            string descricaoVeiculo = "Todos os veículos";
            string categoriaVeiculo = "-";

            if (veiculoId.HasValue && veiculoId.Value != Guid.Empty)
            {
                var veiculoInfo = dados.FirstOrDefault();
                if (veiculoInfo != null)
                {
                    descricaoVeiculo = veiculoInfo.Placa + " - " + veiculoInfo.TipoVeiculo;
                    categoriaVeiculo = veiculoInfo.TipoVeiculo ?? "-";
                }
            }
            else if (!string.IsNullOrEmpty(tipoVeiculo))
            {
                descricaoVeiculo = tipoVeiculo;
                categoriaVeiculo = tipoVeiculo;
            }

            var resultado = new
            {
                valorTotal = dados.Sum(a => ParseDecimal(a.ValorTotal)),
                litrosTotal = dados.Sum(a => ParseDecimal(a.Litros)),
                descricaoVeiculo,
                categoriaVeiculo,

                consumoMensalLitros = dados
                    .Where(a => a.DataHora.HasValue)
                    .GroupBy(a => new { Mes = a.DataHora.Value.Month, Combustivel = a.TipoCombustivel ?? "Outros" })
                    .Select(g => new
                    {
                        mes = g.Key.Mes,
                        combustivel = g.Key.Combustivel,
                        litros = g.Sum(a => ParseDecimal(a.Litros))
                    })
                    .OrderBy(c => c.mes)
                    .ToList(),

                valorMensal = dados
                    .Where(a => a.DataHora.HasValue)
                    .GroupBy(a => a.DataHora.Value.Month)
                    .Select(g => new
                    {
                        mes = g.Key,
                        valor = g.Sum(a => ParseDecimal(a.ValorTotal))
                    })
                    .OrderBy(v => v.mes)
                    .ToList(),

                veiculosComValor = _unitOfWork.ViewAbastecimentos.GetAll()
                    .Where(a => a.DataHora.HasValue && a.DataHora.Value.Year == ano)
                    .Where(a => !mes.HasValue || mes <= 0 || a.DataHora.Value.Month == mes.Value)
                    .Where(a => !string.IsNullOrEmpty(a.Placa))
                    .GroupBy(a => new { a.VeiculoId, a.Placa, a.TipoVeiculo })
                    .Select(g => new
                    {
                        veiculoId = g.Key.VeiculoId,
                        placa = g.Key.Placa,
                        tipoVeiculo = g.Key.TipoVeiculo,
                        valor = g.Sum(a => ParseDecimal(a.ValorTotal))
                    })
                    .OrderByDescending(v => v.valor)
                    .ToList(),

                modelosDisponiveis = _unitOfWork.ViewAbastecimentos.GetAll()
                    .Where(a => a.DataHora.HasValue && a.DataHora.Value.Year == ano)
                    .Where(a => !mes.HasValue || mes <= 0 || a.DataHora.Value.Month == mes.Value)
                    .Where(a => !string.IsNullOrEmpty(a.TipoVeiculo))
                    .Select(a => a.TipoVeiculo)
                    .Distinct()
                    .OrderBy(m => m)
                    .ToList(),

                placasDisponiveis = _unitOfWork.ViewAbastecimentos.GetAll()
                    .Where(a => a.DataHora.HasValue && a.DataHora.Value.Year == ano)
                    .Where(a => !mes.HasValue || mes <= 0 || a.DataHora.Value.Month == mes.Value)
                    .Where(a => !string.IsNullOrEmpty(a.Placa))
                    .Select(a => new { a.VeiculoId, a.Placa, a.TipoVeiculo })
                    .Distinct()
                    .OrderBy(p => p.Placa)
                    .ToList()
            };

            return Ok(resultado);
        }

        #endregion

        /* > ---------------------------------------------------------------------------------------
         > 🔍 **DASHBOARD - DETALHES**
         > --------------------------------------------------------------------------------------- */
        #region Dashboard - Detalhes dos Abastecimentos

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: DashboardDetalhes                                                   ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Retorna lista detalhada de abastecimentos para drill-down de gráficos.    ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • ano (int?): Filtro de ano.                                              ║
        /// ║    • mes (int?): Filtro de mês.                                              ║
        /// ║    • categoria (string?): Filtro de categoria.                               ║
        /// ║    • tipoVeiculo (string?): Filtro de tipo de veículo.                       ║
        /// ║    • placa (string?): Filtro de placa.                                       ║
        /// ║    • diaSemana (int?): Filtro de dia da semana.                              ║
        /// ║    • hora (int?): Filtro de hora.                                            ║
        /// ║                                                                              ║
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com registros detalhados.                           ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [Route("DashboardDetalhes")]
        [HttpGet]
        public IActionResult DashboardDetalhes(int? ano, int? mes, string? categoria, string? tipoVeiculo, string? placa, int? diaSemana, int? hora)
        {
            try
            {
                var query = _unitOfWork.ViewAbastecimentos.GetAll()
                    .Where(a => a.DataHora.HasValue);

                if (ano.HasValue && ano > 0)
                    query = query.Where(a => a.DataHora.Value.Year == ano.Value);

                if (mes.HasValue && mes > 0)
                    query = query.Where(a => a.DataHora.Value.Month == mes.Value);

                if (!string.IsNullOrEmpty(tipoVeiculo))
                    query = query.Where(a => a.TipoVeiculo == tipoVeiculo);

                if (!string.IsNullOrEmpty(placa))
                    query = query.Where(a => a.Placa == placa);

                if (diaSemana.HasValue)
                    query = query.Where(a => (int)a.DataHora.Value.DayOfWeek == diaSemana.Value);

                if (hora.HasValue)
                    query = query.Where(a => a.DataHora.Value.Hour == hora.Value);

                var dados = query.OrderByDescending(a => a.DataHora).Take(100).ToList();

                if (!string.IsNullOrEmpty(categoria))
                {
                    var veiculosCategorias = _unitOfWork.ViewVeiculos.GetAll()
                        .Where(v => v.Categoria == categoria)
                        .Select(v => v.VeiculoId)
                        .ToHashSet();

                    dados = dados.Where(a => veiculosCategorias.Contains(a.VeiculoId)).ToList();
                }

                var resultado = new
                {
                    registros = dados.Select(a => new
                    {
                        data = a.DataHora?.ToString("dd/MM/yyyy HH:mm") ?? "-",
                        placa = a.Placa ?? "-",
                        tipoVeiculo = a.TipoVeiculo ?? "-",
                        litros = ParseDecimal(a.Litros),
                        valorUnitario = ParseDecimal(a.ValorUnitario),
                        valorTotal = ParseDecimal(a.ValorTotal)
                    }).ToList(),
                    totais = new
                    {
                        quantidade = dados.Count,
                        litros = dados.Sum(a => ParseDecimal(a.Litros)),
                        valor = dados.Sum(a => ParseDecimal(a.ValorTotal))
                    }
                };

                return Ok(resultado);
            }
            catch (Exception error)
            {
                _logger.LogError($", errorErro ao carregar DashboardDetalhes");
                Alerta.TratamentoErroComLinha("AbastecimentoController.DashboardAPI.cs", "DashboardDetalhes", error);
                return StatusCode(500, new { message = "Erro ao carregar detalhes" });
            }
        }

        #endregion

        /* > ---------------------------------------------------------------------------------------
         > 🔥 **DASHBOARD - MAPAS DE CALOR**
         > --------------------------------------------------------------------------------------- */
        #region Dashboard - Mapas de Calor (OTIMIZADO)

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: DashboardHeatmapHora                                                ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Retorna dados para o Mapa de Calor: Dia da Semana x Hora.                 ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • ano (int?): Ano filtro.                                                 ║
        /// ║    • mes (int?): Mês filtro.                                                 ║
        /// ║                                                                              ║
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com matriz de calor.                                ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [Route("DashboardHeatmapHora")]
        [HttpGet]
        public IActionResult DashboardHeatmapHora(int? ano, int? mes)
        {
            try
            {
                var queryHeatmap = _context.HeatmapAbastecimentoMensal
                    .Where(h => h.VeiculoId == null && h.TipoVeiculo == null);

                if (ano.HasValue && ano > 0)
                    queryHeatmap = queryHeatmap.Where(h => h.Ano == ano.Value);

                if (mes.HasValue && mes > 0)
                    queryHeatmap = queryHeatmap.Where(h => h.Mes == mes.Value);

                var heatmapData = queryHeatmap.ToList();

                if (heatmapData.Any())
                {
                    var diasSemana = new[] { "Dom", "Seg", "Ter", "Qua", "Qui", "Sex", "Sáb" };

                    var agrupado = heatmapData
                        .GroupBy(h => new { h.DiaSemana, h.Hora })
                        .Select(g => new
                        {
                            diaSemana = g.Key.DiaSemana,
                            hora = g.Key.Hora,
                            valor = g.Sum(h => h.ValorTotal ?? 0)
                        })
                        .ToList();

                    var matriz = new decimal[7, 24];
                    foreach (var item in agrupado)
                    {
                        matriz[item.diaSemana, item.hora] = item.valor;
                    }

                    var data = new List<object>();
                    for (int dia = 0; dia < 7; dia++)
                    {
                        for (int hora = 0; hora < 24; hora++)
                        {
                            data.Add(new { x = diasSemana[dia], y = hora.ToString("00") + "h", value = matriz[dia, hora] });
                        }
                    }

                    return Ok(new
                    {
                        xLabels = diasSemana,
                        yLabels = Enumerable.Range(0, 24).Select(h => h.ToString("00") + "h").ToArray(),
                        data
                    });
                }

                _logger.LogWarning($"Dados de Heatmap de Hora não encontrados para ano={ano}. Iniciando Fallback.");
                return DashboardHeatmapHoraFallback(ano, mes);
            }
            catch (Exception error)
            {
                _logger.LogError($", errorErro ao carregar DashboardHeatmapHora");
                Alerta.TratamentoErroComLinha("AbastecimentoController.DashboardAPI.cs", "DashboardHeatmapHora", error);
                return StatusCode(500, new { message = "Erro ao carregar mapa de calor" });
            }
        }

        private IActionResult DashboardHeatmapHoraFallback(int? ano, int? mes)
        {
            var query = _unitOfWork.ViewAbastecimentos.GetAll()
                .Where(a => a.DataHora.HasValue);

            if (ano.HasValue && ano > 0)
                query = query.Where(a => a.DataHora.Value.Year == ano.Value);

            if (mes.HasValue && mes > 0)
                query = query.Where(a => a.DataHora.Value.Month == mes.Value);

            var dados = query.ToList();

            var agrupado = dados
                .GroupBy(a => new { DiaSemana = (int)a.DataHora.Value.DayOfWeek, Hora = a.DataHora.Value.Hour })
                .Select(g => new
                {
                    diaSemana = g.Key.DiaSemana,
                    hora = g.Key.Hora,
                    valor = g.Sum(a => ParseDecimal(a.ValorTotal)),
                    quantidade = g.Count()
                })
                .ToList();

            var diasSemana = new[] { "Dom", "Seg", "Ter", "Qua", "Qui", "Sex", "Sáb" };

            var matriz = new decimal[7, 24];
            foreach (var item in agrupado)
            {
                matriz[item.diaSemana, item.hora] = item.valor;
            }

            var heatmapData = new List<object>();
            for (int dia = 0; dia < 7; dia++)
            {
                for (int hora = 0; hora < 24; hora++)
                {
                    heatmapData.Add(new { x = diasSemana[dia], y = hora.ToString("00") + "h", value = matriz[dia, hora] });
                }
            }

            return Ok(new
            {
                xLabels = diasSemana,
                yLabels = Enumerable.Range(0, 24).Select(h => h.ToString("00") + "h").ToArray(),
                data = heatmapData
            });
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: DashboardHeatmapCategoria                                           ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Retorna dados para o Mapa de Calor: Categoria x Mês.                      ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • ano (int): Ano filtro.                                                  ║
        /// ║                                                                              ║
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com matriz de calor.                                ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [Route("DashboardHeatmapCategoria")]
        [HttpGet]
        public IActionResult DashboardHeatmapCategoria(int ano)
        {
            try
            {
                var estatCat = _context.EstatisticaAbastecimentoCategoria
                    .Where(e => e.Ano == ano)
                    .ToList();

                if (estatCat.Any())
                {
                    var categorias = estatCat.Select(e => e.Categoria).Distinct().OrderBy(c => c).ToList();
                    var meses = new[] { "Jan", "Fev", "Mar", "Abr", "Mai", "Jun", "Jul", "Ago", "Set", "Out", "Nov", "Dez" };

                    var heatmapData = new List<object>();
                    foreach (var cat in categorias)
                    {
                        for (int mes = 1; mes <= 12; mes++)
                        {
                            var item = estatCat.FirstOrDefault(e => e.Categoria == cat && e.Mes == mes);
                            heatmapData.Add(new { x = cat, y = meses[mes - 1], value = item?.ValorTotal ?? 0 });
                        }
                    }

                    return Ok(new
                    {
                        xLabels = categorias.ToArray(),
                        yLabels = meses,
                        data = heatmapData
                    });
                }

                _logger.LogWarning($"Dados de Heatmap de Categoria não encontrados para ano={ano}. Iniciando Fallback.");
                return DashboardHeatmapCategoriaFallback(ano);
            }
            catch (Exception error)
            {
                _logger.LogError($", errorErro ao carregar DashboardHeatmapCategoria");
                Alerta.TratamentoErroComLinha("AbastecimentoController.DashboardAPI.cs", "DashboardHeatmapCategoria", error);
                return StatusCode(500, new { message = "Erro ao carregar mapa de calor" });
            }
        }

        private IActionResult DashboardHeatmapCategoriaFallback(int ano)
        {
            var dados = _unitOfWork.ViewAbastecimentos.GetAll()
                .Where(a => a.DataHora.HasValue && a.DataHora.Value.Year == ano)
                .ToList();

            var veiculosCategorias = _unitOfWork.ViewVeiculos.GetAll()
                .Where(v => !string.IsNullOrEmpty(v.Categoria))
                .ToDictionary(v => v.VeiculoId, v => v.Categoria ?? "Sem Categoria");

            var agrupado = dados
                .Where(a => veiculosCategorias.ContainsKey(a.VeiculoId))
                .GroupBy(a => new { Categoria = veiculosCategorias[a.VeiculoId], Mes = a.DataHora.Value.Month })
                .Select(g => new
                {
                    categoria = g.Key.Categoria,
                    mes = g.Key.Mes,
                    valor = g.Sum(a => ParseDecimal(a.ValorTotal))
                })
                .ToList();

            var categorias = agrupado.Select(a => a.categoria).Distinct().OrderBy(c => c).ToList();
            var meses = new[] { "Jan", "Fev", "Mar", "Abr", "Mai", "Jun", "Jul", "Ago", "Set", "Out", "Nov", "Dez" };

            var heatmapData = new List<object>();
            foreach (var cat in categorias)
            {
                for (int mes = 1; mes <= 12; mes++)
                {
                    var item = agrupado.FirstOrDefault(a => a.categoria == cat && a.mes == mes);
                    heatmapData.Add(new { x = cat, y = meses[mes - 1], value = item?.valor ?? 0 });
                }
            }

            return Ok(new
            {
                xLabels = categorias.ToArray(),
                yLabels = meses,
                data = heatmapData
            });
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: DashboardHeatmapVeiculo                                             ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Retorna dados para o Mapa de Calor: Dia da Semana x Hora (Veículo/Tipo).  ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • ano (int?): Ano filtro.                                                 ║
        /// ║    • placa (string?): Filtro por placa.                                      ║
        /// ║    • tipoVeiculo (string?): Filtro por tipo de veículo.                      ║
        /// ║                                                                              ║
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com matriz de calor filtrada.                       ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [Route("DashboardHeatmapVeiculo")]
        [HttpGet]
        public IActionResult DashboardHeatmapVeiculo(int? ano, string? placa, string? tipoVeiculo)
        {
            try
            {
                if (string.IsNullOrEmpty(placa) && string.IsNullOrEmpty(tipoVeiculo))
                {
                    return Ok(new { xLabels = new string[0], yLabels = new string[0], data = new object[0] });
                }

                var query = _unitOfWork.ViewAbastecimentos.GetAll()
                    .Where(a => a.DataHora.HasValue);

                if (!string.IsNullOrEmpty(placa))
                    query = query.Where(a => a.Placa == placa);
                else if (!string.IsNullOrEmpty(tipoVeiculo))
                    query = query.Where(a => a.TipoVeiculo == tipoVeiculo);

                if (ano.HasValue && ano > 0)
                    query = query.Where(a => a.DataHora.Value.Year == ano.Value);

                var dados = query.ToList();

                var agrupado = dados
                    .GroupBy(a => new { DiaSemana = (int)a.DataHora.Value.DayOfWeek, Hora = a.DataHora.Value.Hour })
                    .Select(g => new
                    {
                        diaSemana = g.Key.DiaSemana,
                        hora = g.Key.Hora,
                        valor = g.Sum(a => ParseDecimal(a.ValorTotal))
                    })
                    .ToList();

                var diasSemana = new[] { "Dom", "Seg", "Ter", "Qua", "Qui", "Sex", "Sáb" };

                var matriz = new decimal[7, 24];
                foreach (var item in agrupado)
                {
                    matriz[item.diaSemana, item.hora] = item.valor;
                }

                var heatmapData = new List<object>();
                for (int dia = 0; dia < 7; dia++)
                {
                    for (int hora = 0; hora < 24; hora++)
                    {
                        heatmapData.Add(new { x = diasSemana[dia], y = hora.ToString("00") + "h", value = matriz[dia, hora] });
                    }
                }

                return Ok(new
                {
                    xLabels = diasSemana,
                    yLabels = Enumerable.Range(0, 24).Select(h => h.ToString("00") + "h").ToArray(),
                    data = heatmapData
                });
            }
            catch (Exception error)
            {
                _logger.LogError($", errorErro ao carregar DashboardHeatmapVeiculo");
                Alerta.TratamentoErroComLinha("AbastecimentoController.DashboardAPI.cs", "DashboardHeatmapVeiculo", error);
                return StatusCode(500, new { message = "Erro ao carregar mapa de calor de veículo" });
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: DashboardMesesDisponiveis                                           ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Retorna meses que possuem dados para um ano específico.                   ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • ano (int): Ano para verificação.                                        ║
        /// ║                                                                              ║
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com lista de meses disponíveis.                     ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [Route("DashboardMesesDisponiveis")]
        [HttpGet]
        public IActionResult DashboardMesesDisponiveis(int ano)
        {
            try
            {
                var mesesEstatisticos = _context.EstatisticaAbastecimentoMensal
                    .Where(e => e.Ano == ano && e.ValorTotal > 0)
                    .Select(e => e.Mes)
                    .Distinct()
                    .OrderBy(m => m)
                    .ToList();

                if (mesesEstatisticos.Any())
                {
                    return Ok(new { meses = mesesEstatisticos });
                }

                var mesesView = _unitOfWork.ViewAbastecimentos.GetAll()
                    .Where(a => a.DataHora.HasValue && a.DataHora.Value.Year == ano)
                    .Select(a => a.DataHora.Value.Month)
                    .Distinct()
                    .OrderBy(m => m)
                    .ToList();

                return Ok(new { meses = mesesView });
            }
            catch (Exception error)
            {
                _logger.LogError(error.Message, error, "AbastecimentoController.DashboardAPI.cs", "DashboardMesesDisponiveis");
                Alerta.TratamentoErroComLinha("AbastecimentoController.DashboardAPI.cs", "DashboardMesesDisponiveis", error);
                return StatusCode(500, new { message = "Erro ao carregar meses disponíveis" });
            }
        }

        #endregion

        #region Dashboard - Viagens do Veículo

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: DashboardViagensVeiculo                                             ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Retorna viagens de um veículo para cruzamento com dados de abastecimento. ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • veiculoId (Guid): ID do veículo.                                        ║
        /// ║    • ano (int): Ano filtro.                                                  ║
        /// ║    • mes (int?): Mês filtro.                                                 ║
        /// ║                                                                              ║
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com viagens, abastecimentos e totais.               ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [Route("DashboardViagensVeiculo")]
        [HttpGet]
        public IActionResult DashboardViagensVeiculo(Guid veiculoId, int ano, int? mes)
        {
            try
            {
                if (veiculoId == Guid.Empty)
                {
                    return BadRequest(new { message = "VeiculoId é obrigatório" });
                }

                var veiculo = _unitOfWork.ViewVeiculos.GetAll()
                    .FirstOrDefault(v => v.VeiculoId == veiculoId);

                if (veiculo == null)
                {
                    return NotFound(new { message = "Veículo não encontrado" });
                }

                var queryViagens = _context.Set<Viagem>().AsNoTracking()
                    .Where(v => v.VeiculoId == veiculoId && v.DataInicial.HasValue && v.DataInicial.Value.Year == ano);

                if (mes.HasValue && mes > 0)
                {
                    queryViagens = queryViagens.Where(v => v.DataInicial!.Value.Month == mes.Value);
                }

                var viagens = queryViagens
                    .OrderBy(v => v.DataInicial)
                    .Select(v => new
                    {
                        viagemId = v.ViagemId,
                        noFichaVistoria = v.NoFichaVistoria ?? 0,
                        dataInicial = v.DataInicial,
                        horaInicio = v.HoraInicio,
                        dataFinal = v.DataFinal,
                        horaFim = v.HoraFim,
                        kmInicial = v.KmInicial ?? 0,
                        kmFinal = v.KmFinal ?? 0,
                        kmRodado = (v.KmFinal ?? 0) - (v.KmInicial ?? 0),
                        origem = v.Origem ?? "",
                        destino = v.Destino ?? "",
                        status = v.Status ?? ""
                    })
                    .ToList();

                var totalKmRodado = viagens.Sum(v => v.kmRodado);
                var totalViagens = viagens.Count;

                var queryAbastecimentos = _unitOfWork.ViewAbastecimentos.GetAll()
                    .Where(a => a.VeiculoId == veiculoId && a.DataHora.HasValue && a.DataHora.Value.Year == ano);

                if (mes.HasValue && mes > 0)
                {
                    queryAbastecimentos = queryAbastecimentos.Where(a => a.DataHora!.Value.Month == mes.Value);
                }

                var abastecimentos = queryAbastecimentos
                    .OrderBy(a => a.DataHora)
                    .Select(a => new
                    {
                        data = a.DataHora,
                        litros = a.Litros,
                        valorUnitario = a.ValorUnitario,
                        valorTotal = a.ValorTotal,
                        combustivel = a.TipoCombustivel,
                        kmRodado = a.KmRodado
                    })
                    .ToList();

                var totalAbastecimentos = abastecimentos.Count;
                var totalLitros = abastecimentos.Sum(a => ParseDecimal(a.litros));
                var totalValorAbastecimentos = abastecimentos.Sum(a => ParseDecimal(a.valorTotal));

                return Ok(new
                {
                    veiculo = new
                    {
                        placa = veiculo.Placa,
                        marcaModelo = veiculo.MarcaModelo,
                        categoria = veiculo.Categoria
                    },
                    periodo = new
                    {
                        ano,
                        mes = mes ?? 0,
                        descricao = mes.HasValue && mes > 0
                            ? $"{ObterNomeMes(mes.Value)}/{ano}"
                            : $"Ano {ano}"
                    },
                    totais = new
                    {
                        totalViagens,
                        totalKmRodado,
                        totalAbastecimentos,
                        totalLitros,
                        totalValorAbastecimentos
                    },
                    viagens,
                    abastecimentos
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("AbastecimentoController.DashboardAPI.cs", "DashboardViagensVeiculo", error);
                return StatusCode(500, new { message = "Erro ao carregar viagens do veículo" });
            }
        }

        private static string ObterNomeMes(int mes)
        {
            return mes switch
            {
                1 => "Janeiro",
                2 => "Fevereiro",
                3 => "Março",
                4 => "Abril",
                5 => "Maio",
                6 => "Junho",
                7 => "Julho",
                8 => "Agosto",
                9 => "Setembro",
                10 => "Outubro",
                11 => "Novembro",
                12 => "Dezembro",
                _ => ""
            };
        }

        #endregion

        #region Dashboard - Abastecimentos da Unidade

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: DashboardAbastecimentosUnidade                                      ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Retorna abastecimentos de uma unidade para análise de consumo.            ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • unidade (string): Sigla/Nome da unidade.                                ║
        /// ║    • ano (int): Ano filtro.                                                  ║
        /// ║    • mes (int?): Mês filtro.                                                 ║
        /// ║                                                                              ║
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com abastecimentos e resumo por veículo.            ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [Route("DashboardAbastecimentosUnidade")]
        [HttpGet]
        public IActionResult DashboardAbastecimentosUnidade(string unidade, int ano, int? mes)
        {
            try
            {
                if (string.IsNullOrEmpty(unidade))
                {
                    return BadRequest(new { message = "Unidade é obrigatória" });
                }

                var queryAbastecimentos = _unitOfWork.ViewAbastecimentos.GetAll()
                    .Where(a => a.Sigla == unidade && a.DataHora.HasValue && a.DataHora.Value.Year == ano);

                if (mes.HasValue && mes > 0)
                {
                    queryAbastecimentos = queryAbastecimentos.Where(a => a.DataHora!.Value.Month == mes.Value);
                }

                var abastecimentos = queryAbastecimentos
                    .OrderByDescending(a => a.DataHora)
                    .Select(a => new
                    {
                        data = a.DataHora,
                        placa = a.Placa ?? "-",
                        tipoVeiculo = a.TipoVeiculo ?? "-",
                        motorista = a.MotoristaCondutor ?? "-",
                        combustivel = a.TipoCombustivel ?? "-",
                        litros = a.Litros,
                        valorUnitario = a.ValorUnitario,
                        valorTotal = a.ValorTotal,
                        kmRodado = a.KmRodado
                    })
                    .ToList();

                var totalAbastecimentos = abastecimentos.Count;
                var totalLitros = abastecimentos.Sum(a => ParseDecimal(a.litros));
                var totalValor = abastecimentos.Sum(a => ParseDecimal(a.valorTotal));

                var porVeiculo = abastecimentos
                    .GroupBy(a => a.placa)
                    .Select(g => new
                    {
                        placa = g.Key,
                        tipoVeiculo = g.First().tipoVeiculo,
                        qtdAbastecimentos = g.Count(),
                        litros = g.Sum(a => ParseDecimal(a.litros)),
                        valor = g.Sum(a => ParseDecimal(a.valorTotal))
                    })
                    .OrderByDescending(v => v.valor)
                    .ToList();

                return Ok(new
                {
                    unidade,
                    periodo = new
                    {
                        ano,
                        mes = mes ?? 0,
                        descricao = mes.HasValue && mes > 0
                            ? $"{ObterNomeMes(mes.Value)}/{ano}"
                            : $"Ano {ano}"
                    },
                    totais = new
                    {
                        totalAbastecimentos,
                        totalLitros,
                        totalValor
                    },
                    abastecimentos,
                    porVeiculo
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("AbastecimentoController.DashboardAPI.cs", "DashboardAbastecimentosUnidade", error);
                return StatusCode(500, new { message = "Erro ao carregar abastecimentos da unidade" });
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: DashboardAbastecimentosCategoria                                    ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Retorna abastecimentos filtrados por categoria de veículo.                ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • categoria (string): Nome da categoria.                                  ║
        /// ║    • ano (int): Ano filtro.                                                  ║
        /// ║    • mes (int?): Mês filtro.                                                 ║
        /// ║                                                                              ║
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com abastecimentos e resumo por veículo.            ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [Route("DashboardAbastecimentosCategoria")]
        [HttpGet]
        public IActionResult DashboardAbastecimentosCategoria(string categoria, int ano, int? mes)
        {
            try
            {
                if (string.IsNullOrEmpty(categoria))
                {
                    return BadRequest(new { message = "Categoria é obrigatória" });
                }

                var veiculosDaCategoria = _unitOfWork.ViewVeiculos.GetAll(v => v.Categoria == categoria)
                                                    .Select(v => v.VeiculoId)
                                                    .ToList();

                if (!veiculosDaCategoria.Any())
                {
                    return Ok(new
                    {
                        categoria,
                        periodo = new { ano, mes = mes ?? 0, descricao = mes.HasValue && mes > 0 ? $"{ObterNomeMes(mes.Value)}/{ano}" : $"Ano {ano}" },
                        totais = new { totalAbastecimentos = 0, totalLitros = 0m, totalValor = 0m },
                        abastecimentos = new List<object>(),
                        porVeiculo = new List<object>()
                    });
                }

                var queryAbastecimentos = _unitOfWork.ViewAbastecimentos.GetAll()
                    .Where(a => a.DataHora.HasValue && a.DataHora.Value.Year == ano && veiculosDaCategoria.Contains(a.VeiculoId));

                if (mes.HasValue && mes > 0)
                {
                    queryAbastecimentos = queryAbastecimentos.Where(a => a.DataHora!.Value.Month == mes.Value);
                }

                var abastecimentos = queryAbastecimentos
                    .OrderByDescending(a => a.DataHora)
                    .Select(a => new
                    {
                        data = a.DataHora,
                        placa = a.Placa ?? "-",
                        tipoVeiculo = a.TipoVeiculo ?? "-",
                        motorista = a.MotoristaCondutor ?? "-",
                        combustivel = a.TipoCombustivel ?? "-",
                        litros = a.Litros,
                        valorUnitario = a.ValorUnitario,
                        valorTotal = a.ValorTotal,
                        kmRodado = a.KmRodado
                    })
                    .ToList();

                var totalAbastecimentos = abastecimentos.Count;
                var totalLitros = abastecimentos.Sum(a => ParseDecimal(a.litros));
                var totalValor = abastecimentos.Sum(a => ParseDecimal(a.valorTotal));

                var porVeiculo = abastecimentos
                    .GroupBy(a => a.placa)
                    .Select(g => new
                    {
                        placa = g.Key,
                        tipoVeiculo = g.First().tipoVeiculo,
                        qtdAbastecimentos = g.Count(),
                        litros = g.Sum(a => ParseDecimal(a.litros)),
                        valor = g.Sum(a => ParseDecimal(a.valorTotal))
                    })
                    .OrderByDescending(v => v.valor)
                    .ToList();

                return Ok(new
                {
                    categoria,
                    periodo = new
                    {
                        ano,
                        mes = mes ?? 0,
                        descricao = mes.HasValue && mes > 0
                            ? $"{ObterNomeMes(mes.Value)}/{ano}"
                            : $"Ano {ano}"
                    },
                    totais = new
                    {
                        totalAbastecimentos,
                        totalLitros,
                        totalValor,
                        totalVeiculos = porVeiculo.Count
                    },
                    abastecimentos,
                    porVeiculo
                });
            }
            catch (Exception error)
            {
                _logger.LogError($", errorErro ao carregar DashboardAbastecimentosCategoria para categoria={categoria}");
                Alerta.TratamentoErroComLinha("AbastecimentoController.DashboardAPI.cs", "DashboardAbastecimentosCategoria", error);
                return StatusCode(500, new { message = "Erro ao carregar abastecimentos da categoria" });
            }
        }

        #endregion

        #region Helpers

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: ParseDecimal                                                        ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Converte string monetária/numérica para decimal, tratando formatos        ║
        /// ║    brasileiros e americanos automaticamente.                                 ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • valor (string?): Texto numérico a ser convertido.                       ║
        /// ║                                                                              ║
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • decimal: Valor convertido ou 0 em caso de erro/vazio.                   ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        private static decimal ParseDecimal(string? valor)
        {
            try
            {
                if (string.IsNullOrEmpty(valor))
                    return 0;

                var valorLimpo = valor
                    .Replace("R$", "")
                    .Replace(" ", "")
                    .Trim();

                if (string.IsNullOrEmpty(valorLimpo))
                    return 0;

                // Detectar formato brasileiro vs americano
                // Formato BR: 1.234,56 (ponto = milhares, vírgula = decimal)
                // Formato US: 1,234.56 (vírgula = milhares, ponto = decimal)

                bool temVirgula = valorLimpo.Contains(',');
                bool temPonto = valorLimpo.Contains('.');

                if (temVirgula && temPonto)
                {
                    // Ambos presentes - verificar qual vem por último (esse é o decimal)
                    int posVirgula = valorLimpo.LastIndexOf(',');
                    int posPonto = valorLimpo.LastIndexOf('.');

                    if (posVirgula > posPonto)
                    {
                        // Formato BR: 1.234,56 - vírgula é decimal
                        valorLimpo = valorLimpo.Replace(".", "").Replace(",", ".");
                    }
                    else
                    {
                        // Formato US: 1,234.56 - ponto é decimal
                        valorLimpo = valorLimpo.Replace(",", "");
                    }
                }
                else if (temVirgula)
                {
                    // Só vírgula - é decimal BR (ex: "123,45")
                    valorLimpo = valorLimpo.Replace(",", ".");
                }
                else if (temPonto)
                {
                    // Só ponto - pode ser milhar BR (ex: "1.234") ou decimal US (ex: "1.5")
                    // Se o ponto separa grupos de 3 dígitos, é separador de milhares
                    var partes = valorLimpo.Split('.');
                    bool ehSeparadorMilhar = partes.Length > 1 && partes.Skip(1).All(p => p.Length == 3);

                    if (ehSeparadorMilhar)
                    {
                        // É separador de milhares BR (ex: "1.234" ou "1.234.567")
                        valorLimpo = valorLimpo.Replace(".", "");
                    }
                    // Se não é separador de milhares, deixa como está (decimal US)
                }

                if (decimal.TryParse(valorLimpo, System.Globalization.NumberStyles.Any,
                    System.Globalization.CultureInfo.InvariantCulture, out decimal result))
                {
                    return result;
                }

                return 0;
            }
            catch (Exception error)
            {
                // Método estático auxiliar, logamos mas retornamos 0 para não quebrar fluxos de soma
                Alerta.TratamentoErroComLinha("AbastecimentoController.DashboardAPI.cs", "ParseDecimal", error);
                return 0;
            }
        }

        #endregion
    }
}
