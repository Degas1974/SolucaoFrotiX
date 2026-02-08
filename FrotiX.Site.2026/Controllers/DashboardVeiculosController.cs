/* ****************************************************************************************
 * ⚡ ARQUIVO: DashboardVeiculosController.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Estatísticas e análises da frota de veículos (idade média, status,
 *                   tipos e indicadores gerais).
 *
 * 📥 ENTRADAS     : Nenhuma (dados gerais sem filtro de data).
 *
 * 📤 SAÍDAS       : JSON com estatísticas agregadas de veículos.
 *
 * 🔗 CHAMADA POR  : Frontend do Dashboard de Veículos.
 *
 * 🔄 CHAMA        : ViewVeiculos e repositórios de veículos via IUnitOfWork.
 *
 * 📦 DEPENDÊNCIAS : IUnitOfWork, Repository Pattern.
 *
 * 📝 OBSERVAÇÕES  : Métricas incluem veículos ativos, locados e idade média.
 **************************************************************************************** */

using FrotiX.Models;
using FrotiX.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FrotiX.Controllers
{
    /****************************************************************************************
     * ⚡ CONTROLLER: DashboardVeiculosController
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Fornecer estatísticas e análises da frota de veículos
     * 📥 ENTRADAS     : Nenhuma (dados gerais da frota, sem filtro de data)
     * 📤 SAÍDAS       : JSON com estatísticas agregadas de veículos
     * 🔗 CHAMADA POR  : Frontend do Dashboard de Veículos
     * 🔄 CHAMA        : ViewVeiculos, Veiculo (via UnitOfWork)
     * 📦 DEPENDÊNCIAS : IUnitOfWork, Repository Pattern
     * --------------------------------------------------------------------------------------
     * [DOC] Dashboard específico para análise da frota de veículos
     * [DOC] Estatísticas: total, ativos/inativos, próprios/locados, reserva/efetivos
     * [DOC] Calcula idade média da frota (baseado em AnoFabricacao)
     * [DOC] Tema visual: Verde Sage/Oliva
     * [DOC] Usa ViewVeiculos para dados agregados otimizados
     ****************************************************************************************/
    /// <summary>
    /// Controller com endpoints da API para o Dashboard de Veículos
    /// Tema: Verde Sage/Oliva
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardVeiculosController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public DashboardVeiculosController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #region Dashboard - Dados Gerais

        /// <summary>
        /// Retorna todos os dados agregados para o Dashboard de Veículos
        /// </summary>
        [Route("DashboardDados")]
        [HttpGet]
        public IActionResult DashboardDados()
        {
            try
            {
                var veiculos = _unitOfWork.ViewVeiculos.GetAll().ToList();
                var veiculosModel = _unitOfWork.Veiculo.GetAll().ToList();

                // Totais gerais
                var totalVeiculos = veiculos.Count;
                var veiculosAtivos = veiculos.Count(v => v.Status == true);
                var veiculosInativos = veiculos.Count(v => v.Status == false);
                var veiculosReserva = veiculos.Count(v => v.VeiculoReserva == "Reserva");
                var veiculosEfetivos = veiculos.Count(v => v.VeiculoReserva == "Efetivo");
                var veiculosProprios = veiculos.Count(v => v.VeiculoProprio == true);
                var veiculosLocados = veiculos.Count(v => v.VeiculoProprio == false);

                // Idade média da frota
                var anoAtual = DateTime.Now.Year;
                var veiculosComAno = veiculosModel.Where(v => v.AnoFabricacao.HasValue && v.AnoFabricacao > 1990).ToList();
                var idadeMedia = veiculosComAno.Any()
                    ? veiculosComAno.Average(v => anoAtual - v.AnoFabricacao.Value)
                    : 0;

                // Distribuição por categoria
                var porCategoria = veiculos
                    .Where(v => !string.IsNullOrEmpty(v.Categoria))
                    .GroupBy(v => v.Categoria)
                    .Select(g => new
                    {
                        categoria = g.Key,
                        quantidade = g.Count()
                    })
                    .OrderByDescending(c => c.quantidade)
                    .ToList();

                // Distribuição por status
                var porStatus = new[]
                {
                    new { status = "Ativos", quantidade = veiculosAtivos },
                    new { status = "Inativos", quantidade = veiculosInativos }
                };

                // Distribuição por tipo (Efetivo/Reserva)
                var porTipo = new[]
                {
                    new { tipo = "Efetivos", quantidade = veiculosEfetivos },
                    new { tipo = "Reserva", quantidade = veiculosReserva }
                };

                // Distribuição por origem (Próprio/Locado)
                var porOrigem = veiculos
                    .Where(v => !string.IsNullOrEmpty(v.OrigemVeiculo))
                    .GroupBy(v => v.OrigemVeiculo)
                    .Select(g => new
                    {
                        origem = g.Key,
                        quantidade = g.Count()
                    })
                    .OrderByDescending(o => o.quantidade)
                    .ToList();

                // Distribuição por modelo (MarcaModelo)
                var porModelo = veiculos
                    .Where(v => !string.IsNullOrEmpty(v.MarcaModelo))
                    .GroupBy(v => v.MarcaModelo)
                    .Select(g => new
                    {
                        modelo = g.Key,
                        quantidade = g.Count()
                    })
                    .OrderByDescending(m => m.quantidade)
                    .Take(15)
                    .ToList();

                // Distribuição por ano de fabricação
                var porAnoFabricacao = veiculosModel
                    .Where(v => v.AnoFabricacao.HasValue && v.AnoFabricacao > 1990)
                    .GroupBy(v => v.AnoFabricacao.Value)
                    .Select(g => new
                    {
                        ano = g.Key,
                        quantidade = g.Count()
                    })
                    .OrderBy(a => a.ano)
                    .ToList();

                // Distribuição por combustível
                var porCombustivel = veiculos
                    .Where(v => !string.IsNullOrEmpty(v.Descricao))
                    .GroupBy(v => v.Descricao)
                    .Select(g => new
                    {
                        combustivel = g.Key,
                        quantidade = g.Count()
                    })
                    .OrderByDescending(c => c.quantidade)
                    .ToList();

                // Distribuição por unidade
                var porUnidade = veiculos
                    .Where(v => !string.IsNullOrEmpty(v.Sigla))
                    .GroupBy(v => v.Sigla)
                    .Select(g => new
                    {
                        unidade = g.Key,
                        quantidade = g.Count()
                    })
                    .OrderByDescending(u => u.quantidade)
                    .Take(10)
                    .ToList();

                // Top 10 veículos por quilometragem
                var topKm = veiculos
                    .Where(v => v.Quilometragem.HasValue && v.Quilometragem > 0)
                    .OrderByDescending(v => v.Quilometragem)
                    .Take(10)
                    .Select(v => new
                    {
                        placa = v.Placa ?? "-",
                        modelo = v.MarcaModelo ?? "-",
                        km = v.Quilometragem ?? 0
                    })
                    .ToList();

                // Valor mensal total (para veículos locados)
                var valorMensalTotal = veiculos
                    .Where(v => v.ValorMensal.HasValue)
                    .Sum(v => v.ValorMensal ?? 0);

                var resultado = new
                {
                    totais = new
                    {
                        totalVeiculos,
                        veiculosAtivos,
                        veiculosInativos,
                        veiculosReserva,
                        veiculosEfetivos,
                        veiculosProprios,
                        veiculosLocados,
                        idadeMedia = Math.Round(idadeMedia, 1),
                        valorMensalTotal = Math.Round(valorMensalTotal, 2)
                    },
                    porCategoria,
                    porStatus,
                    porTipo,
                    porOrigem,
                    porModelo,
                    porAnoFabricacao,
                    porCombustivel,
                    porUnidade,
                    topKm
                };

                return Ok(resultado);
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("DashboardVeiculosController.cs", "DashboardDados", error);
                return StatusCode(500, new { message = "Erro ao carregar dados do dashboard" });
            }
        }

        #endregion

        #region Dashboard - Estatísticas de Uso

        /// <summary>
        /// Retorna estatísticas de uso dos veículos (viagens e abastecimentos)
        /// Aceita filtros: ano, mês, dataInicio, dataFim
        /// </summary>
        [Route("DashboardUso")]
        [HttpGet]
        public IActionResult DashboardUso(int? ano, int? mes, DateTime? dataInicio, DateTime? dataFim)
        {
            try
            {
                // Construir query de viagens
                var queryViagens = _unitOfWork.Viagem.GetAll()
                    .Where(v => v.DataInicial.HasValue);

                // Construir query de abastecimentos
                var queryAbastecimentos = _unitOfWork.ViewAbastecimentos.GetAll()
                    .Where(a => a.DataHora.HasValue);

                // Aplicar filtro por período personalizado (prioridade)
                if (dataInicio.HasValue && dataFim.HasValue)
                {
                    var dataFimAjustada = dataFim.Value.Date.AddDays(1).AddSeconds(-1);
                    queryViagens = queryViagens.Where(v => v.DataInicial.Value >= dataInicio.Value && v.DataInicial.Value <= dataFimAjustada);
                    queryAbastecimentos = queryAbastecimentos.Where(a => a.DataHora.Value >= dataInicio.Value && a.DataHora.Value <= dataFimAjustada);
                }
                // Senão, aplicar filtro por ano/mês
                else
                {
                    if (ano.HasValue && ano > 0)
                    {
                        queryViagens = queryViagens.Where(v => v.DataInicial.Value.Year == ano.Value);
                        queryAbastecimentos = queryAbastecimentos.Where(a => a.DataHora.Value.Year == ano.Value);
                    }

                    if (mes.HasValue && mes > 0)
                    {
                        queryViagens = queryViagens.Where(v => v.DataInicial.Value.Month == mes.Value);
                        queryAbastecimentos = queryAbastecimentos.Where(a => a.DataHora.Value.Month == mes.Value);
                    }
                }

                var viagens = queryViagens.ToList();
                var abastecimentos = queryAbastecimentos.ToList();

                // Top 10 veículos por quantidade de viagens
                var topViagensPorVeiculo = viagens
                    .Where(v => v.VeiculoId.HasValue)
                    .GroupBy(v => v.VeiculoId)
                    .Select(g => new
                    {
                        veiculoId = g.Key,
                        quantidade = g.Count(),
                        kmTotal = g.Sum(v => (v.KmFinal ?? 0) - (v.KmInicial ?? 0))
                    })
                    .OrderByDescending(v => v.quantidade)
                    .Take(10)
                    .ToList();

                // Obter informações dos veículos
                var veiculosIds = topViagensPorVeiculo.Select(v => v.veiculoId).ToList();
                var veiculosInfo = _unitOfWork.ViewVeiculos.GetAll()
                    .Where(v => veiculosIds.Contains(v.VeiculoId))
                    .ToDictionary(v => v.VeiculoId);

                var topViagens = topViagensPorVeiculo.Select(v => new
                {
                    placa = veiculosInfo.ContainsKey(v.veiculoId.Value) ? veiculosInfo[v.veiculoId.Value].Placa : "-",
                    modelo = veiculosInfo.ContainsKey(v.veiculoId.Value) ? veiculosInfo[v.veiculoId.Value].MarcaModelo : "-",
                    quantidade = v.quantidade,
                    kmTotal = v.kmTotal
                }).ToList();

                // Top 10 veículos por valor de abastecimento
                var topAbastecimento = abastecimentos
                    .Where(a => a.VeiculoId != Guid.Empty)
                    .GroupBy(a => new { a.VeiculoId, a.Placa, a.TipoVeiculo })
                    .Select(g => new
                    {
                        placa = g.Key.Placa ?? "-",
                        modelo = g.Key.TipoVeiculo ?? "-",
                        valor = g.Sum(a => ParseDecimal(a.ValorTotal)),
                        litros = g.Sum(a => ParseDecimal(a.Litros))
                    })
                    .OrderByDescending(a => a.valor)
                    .Take(10)
                    .ToList();

                // Viagens por mês
                var viagensPorMes = viagens
                    .Where(v => v.DataInicial.HasValue)
                    .GroupBy(v => v.DataInicial.Value.Month)
                    .Select(g => new
                    {
                        mes = g.Key,
                        quantidade = g.Count()
                    })
                    .OrderBy(v => v.mes)
                    .ToList();

                // Abastecimentos por mês (em valor)
                var abastecimentoPorMes = abastecimentos
                    .Where(a => a.DataHora.HasValue)
                    .GroupBy(a => a.DataHora.Value.Month)
                    .Select(g => new
                    {
                        mes = g.Key,
                        valor = g.Sum(a => ParseDecimal(a.ValorTotal))
                    })
                    .OrderBy(a => a.mes)
                    .ToList();

                // TOP 10 veículos por Litros Abastecidos (decrescente)
                var topLitrosAbastecidos = abastecimentos
                    .Where(a => a.VeiculoId != Guid.Empty)
                    .GroupBy(a => new { a.VeiculoId, a.Placa, a.TipoVeiculo })
                    .Select(g => new
                    {
                        placa = g.Key.Placa ?? "-",
                        modelo = g.Key.TipoVeiculo ?? "-",
                        litros = g.Sum(a => ParseDecimal(a.Litros)),
                        qtdAbastecimentos = g.Count()
                    })
                    .OrderByDescending(a => a.litros)
                    .Take(10)
                    .ToList();

                // TOP 10 veículos por Consumo (km/l) - Menos Eficientes (menor km/l = pior consumo)
                // Cruza viagens (km rodado) com abastecimentos (litros)
                var kmPorVeiculo = viagens
                    .Where(v => v.VeiculoId.HasValue)
                    .GroupBy(v => v.VeiculoId.Value)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Sum(v => (v.KmFinal ?? 0) - (v.KmInicial ?? 0))
                    );

                var litrosPorVeiculo = abastecimentos
                    .Where(a => a.VeiculoId != Guid.Empty)
                    .GroupBy(a => a.VeiculoId)
                    .ToDictionary(
                        g => g.Key,
                        g => new { litros = g.Sum(a => ParseDecimal(a.Litros)), placa = g.First().Placa, modelo = g.First().TipoVeiculo }
                    );

                var topConsumo = litrosPorVeiculo
                    .Where(l => l.Value.litros > 0 && kmPorVeiculo.ContainsKey(l.Key) && kmPorVeiculo[l.Key] > 0)
                    .Select(l => new
                    {
                        placa = l.Value.placa ?? "-",
                        modelo = l.Value.modelo ?? "-",
                        kmRodado = kmPorVeiculo[l.Key],
                        litros = l.Value.litros,
                        consumo = Math.Round((decimal)kmPorVeiculo[l.Key] / l.Value.litros, 2) // km/l
                    })
                    .OrderBy(c => c.consumo) // Menor consumo = menos eficiente = primeiro
                    .Take(10)
                    .ToList();

                // TOP 10 veículos Mais Eficientes (maior km/l)
                var topEficiencia = litrosPorVeiculo
                    .Where(l => l.Value.litros > 0 && kmPorVeiculo.ContainsKey(l.Key) && kmPorVeiculo[l.Key] > 0)
                    .Select(l => new
                    {
                        placa = l.Value.placa ?? "-",
                        modelo = l.Value.modelo ?? "-",
                        kmRodado = kmPorVeiculo[l.Key],
                        litros = l.Value.litros,
                        consumo = Math.Round((decimal)kmPorVeiculo[l.Key] / l.Value.litros, 2) // km/l
                    })
                    .OrderByDescending(c => c.consumo) // Maior consumo = mais eficiente = primeiro
                    .Take(10)
                    .ToList();

                // Anos disponíveis
                var anosDisponiveis = _unitOfWork.Viagem.GetAll()
                    .Where(v => v.DataInicial.HasValue)
                    .Select(v => v.DataInicial.Value.Year)
                    .Distinct()
                    .OrderByDescending(a => a)
                    .ToList();

                var resultado = new
                {
                    anoSelecionado = ano,
                    anosDisponiveis,
                    topViagens,
                    topAbastecimento,
                    topLitrosAbastecidos,
                    topConsumo,        // Menos eficientes (menor km/l)
                    topEficiencia,     // Mais eficientes (maior km/l)
                    viagensPorMes,
                    abastecimentoPorMes,
                    totais = new
                    {
                        totalViagens = viagens.Count,
                        totalAbastecimentos = abastecimentos.Count,
                        valorTotalAbastecimento = abastecimentos.Sum(a => ParseDecimal(a.ValorTotal)),
                        kmTotalRodado = viagens.Sum(v => (v.KmFinal ?? 0) - (v.KmInicial ?? 0)),
                        totalLitros = abastecimentos.Sum(a => ParseDecimal(a.Litros))
                    }
                };

                return Ok(resultado);
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("DashboardVeiculosController.cs", "DashboardUso", error);
                return StatusCode(500, new { message = "Erro ao carregar estatísticas de uso" });
            }
        }

        #endregion

        #region Dashboard - Custos

        /// <summary>
        /// Retorna dados de custos dos veículos (abastecimento + manutenção)
        /// </summary>
        [Route("DashboardCustos")]
        [HttpGet]
        public IActionResult DashboardCustos(int? ano)
        {
            try
            {
                var anoFiltro = ano ?? DateTime.Now.Year;

                // Abastecimentos
                var abastecimentos = _unitOfWork.ViewAbastecimentos.GetAll()
                    .Where(a => a.DataHora.HasValue && a.DataHora.Value.Year == anoFiltro)
                    .ToList();

                // Manutenções
                var manutencoes = _unitOfWork.Manutencao.GetAll()
                    .Where(m => m.DataSolicitacao.HasValue && m.DataSolicitacao.Value.Year == anoFiltro)
                    .ToList();

                // Custo total por categoria de veículo (abastecimento)
                var veiculosCategorias = _unitOfWork.ViewVeiculos.GetAll()
                    .Where(v => !string.IsNullOrEmpty(v.Categoria))
                    .ToDictionary(v => v.VeiculoId, v => v.Categoria ?? "Sem Categoria");

                var custoPorCategoria = abastecimentos
                    .Where(a => a.VeiculoId != Guid.Empty && veiculosCategorias.ContainsKey(a.VeiculoId))
                    .GroupBy(a => veiculosCategorias[a.VeiculoId])
                    .Select(g => new
                    {
                        categoria = g.Key,
                        valorAbastecimento = g.Sum(a => ParseDecimal(a.ValorTotal))
                    })
                    .OrderByDescending(c => c.valorAbastecimento)
                    .ToList();

                // Custo de abastecimento por mês
                var custoAbastecimentoMes = abastecimentos
                    .Where(a => a.DataHora.HasValue)
                    .GroupBy(a => a.DataHora.Value.Month)
                    .Select(g => new
                    {
                        mes = g.Key,
                        valor = g.Sum(a => ParseDecimal(a.ValorTotal))
                    })
                    .OrderBy(c => c.mes)
                    .ToList();

                // Quantidade de manutenções por mês (sem valor, pois o modelo não tem campo de custo)
                var manutencoesPorMes = manutencoes
                    .Where(m => m.DataSolicitacao.HasValue)
                    .GroupBy(m => m.DataSolicitacao.Value.Month)
                    .Select(g => new
                    {
                        mes = g.Key,
                        quantidade = g.Count()
                    })
                    .OrderBy(c => c.mes)
                    .ToList();

                // Comparativo mensal (abastecimento em valor, manutenção em quantidade)
                var comparativoMensal = new List<object>();
                for (int mes = 1; mes <= 12; mes++)
                {
                    var abast = custoAbastecimentoMes.FirstOrDefault(c => c.mes == mes);
                    var manut = manutencoesPorMes.FirstOrDefault(c => c.mes == mes);
                    comparativoMensal.Add(new
                    {
                        mes,
                        abastecimento = abast?.valor ?? 0,
                        manutencao = manut?.quantidade ?? 0 // Quantidade, não valor
                    });
                }

                var resultado = new
                {
                    anoSelecionado = ano,
                    custoPorCategoria,
                    comparativoMensal,
                    totais = new
                    {
                        totalAbastecimento = abastecimentos.Sum(a => ParseDecimal(a.ValorTotal)),
                        totalManutencao = 0m, // Não há campo de valor na tabela Manutencao
                        qtdAbastecimentos = abastecimentos.Count,
                        qtdManutencoes = manutencoes.Count
                    }
                };

                return Ok(resultado);
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("DashboardVeiculosController.cs", "DashboardCustos", error);
                return StatusCode(500, new { message = "Erro ao carregar dados de custos" });
            }
        }

        #endregion

        #region Helpers

        private static decimal ParseDecimal(string? valor)
        {
            if (string.IsNullOrEmpty(valor))
                return 0;

            var valorLimpo = valor
                .Replace("R$", "")
                .Replace(" ", "")
                .Trim();

            if (string.IsNullOrEmpty(valorLimpo))
                return 0;

            bool temVirgula = valorLimpo.Contains(',');

            if (temVirgula)
            {
                valorLimpo = valorLimpo.Replace(".", "").Replace(",", ".");
            }

            if (decimal.TryParse(valorLimpo, System.Globalization.NumberStyles.Any,
                System.Globalization.CultureInfo.InvariantCulture, out decimal result))
            {
                return result;
            }

            return 0;
        }

        #endregion
    }
}
