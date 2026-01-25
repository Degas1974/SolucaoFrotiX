using FrotiX.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FrotiX.Helpers;
using FrotiX.Services;

namespace FrotiX.Controllers
{
    /*
    *  #################################################################################################
    *  #                                                                                               #
    *  #   ███████╗██████╗  ██████╗ ████████╗██╗██╗  ██╗    ██████╗  ██████╗ ██████╗  ██████╗          #
    *  #   ██╔════╝██╔══██╗██╔═══██╗╚══██╔══╝██║╚██╗██╔╝    ╚════██╗██╔═████╗╚════██╗██╔════╝          #
    *  #   █████╗  ██████╔╝██║   ██║   ██║   ██║ ╚███╔╝      █████╔╝██║██╔██║ █████╔╝███████╗          #
    *  #   ██╔══╝  ██╔══██╗██║   ██║   ██║   ██║ ██╔██╗     ██╔═══╝ ████╔╝██║██╔═══╝ ██╔═══██╗          #
    *  #   ██║     ██║  ██║╚██████╔╝   ██║   ██║██╔╝ ██╗    ███████╗╚██████╔╝███████╗╚██████╔╝          #
    *  #   ╚═╝     ╚═╝  ╚═╝ ╚═════╝    ╚═╝   ╚═╝╚═╝  ╚═╝    ╚══════╝ ╚═════╝ ╚══════╝ ╚═════╝           #
    *  #                                                                                               #
    *  #   PROJETO: FROTIX - SOLUÇÃO INTEGRADA DE GESTÃO DE FROTAS                                     #
    *  #   MODULO:  DASHBOARD ANALÍTICO DE ATIVOS E VEÍCULOS                                           #
    *  #   DATA:    2026 (Modernização FrotiX 2026)                                                   #
    *  #                                                                                               #
    *  #################################################################################################
    */

    /// <summary>
    /// <para>────────────────────────────────────────────────────────────────────────────────────────────</para>
    /// <para>CLASSE: <c>DashboardVeiculosController</c></para>
    /// <para>DESCRIÇÃO: Interface de telemetria para gestão de ativos, custos operacionais e utilização de frota.</para>
    /// <para>PADRÃO: FrotiX 2026 - (IA) Documented & Modernized </para>
    /// <para>────────────────────────────────────────────────────────────────────────────────────────────</para>
    /// </summary>
    [Authorize]
    public class DashboardVeiculosController : Controller
    {
        private readonly FrotiXDbContext _context;
        private readonly ILogService _log;

        /// <summary>
        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: DashboardVeiculosController (Constructor)                          ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Inicializa o dashboard de veículos com DbContext e log centralizado.      ║
        /// ║                                                                              ║
        /// ║ 🎯 IMPORTÂNCIA PARA A SOLUÇÃO:                                              ║
        /// ║    Habilita análise patrimonial e operacional da frota.                       ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • context (FrotiXDbContext): contexto EF Core.                             ║
        /// ║    • log (ILogService): log centralizado.                                    ║
        /// ║                                                                              ║
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • Tipo: N/A                                                               ║
        /// ║    • Significado: N/A                                                        ║
        /// ║    • Consumidor: runtime do ASP.NET Core.                                    ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📞 FUNÇÕES QUE CHAMA:                                                        ║
        /// ║    • _log.Error() / Alerta.TratamentoErroComLinha() → erros.                  ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📲 CHAMADA POR:                                                              ║
        /// ║    • Injeção de dependência ao instanciar o controller.                       ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 🔗 ESCOPO: INTERNA ao módulo                                                 ║
        /// ║    • Arquivos relacionados: Program.cs                                       ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        /// </summary>
        public DashboardVeiculosController(FrotiXDbContext context, ILogService log)
        {
            try
            {
                _context = context;
                _log = log;
            }
            catch (Exception ex)
            {
                _log?.Error(ex.Message, ex, "DashboardVeiculosController.cs", "Constructor");
                Alerta.TratamentoErroComLinha("DashboardVeiculosController.cs", "Constructor", ex);
            }
        }

        #region Helper - Obter Período a partir de Ano/Mês ou Data

        /// <summary>
        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: ObterPeriodo                                                       ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Calcula datas de início/fim baseadas em Ano/Mês ou intervalo direto.      ║
        /// ║                                                                              ║
        /// ║ 🎯 IMPORTÂNCIA PARA A SOLUÇÃO:                                              ║
        /// ║    Padroniza filtros temporais no dashboard de veículos.                     ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • dataInicio (DateTime?): início do intervalo.                            ║
        /// ║    • dataFim (DateTime?): fim do intervalo.                                  ║
        /// ║    • ano (int?): ano de referência.                                          ║
        /// ║    • mes (int?): mês de referência.                                          ║
        /// ║                                                                              ║
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • (DateTime dataInicio, DateTime dataFim): período calculado.             ║
        /// ║                                                                              ║
        /// ║ 📞 FUNÇÕES QUE CHAMA:                                                        ║
        /// ║    • _log.Error() / Alerta.TratamentoErroComLinha() → erros.                  ║
        /// ║                                                                              ║
        /// ║ 📲 CHAMADA POR:                                                              ║
        /// ║    • Métodos do dashboard que exigem filtro temporal.                         ║
        /// ║                                                                              ║
        /// ║ 🔗 ESCOPO: INTERNA ao módulo                                                 ║
        /// ║    • Arquivos relacionados: DashboardVeiculosController.cs                   ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        /// </summary>
        private (DateTime dataInicio, DateTime dataFim) ObterPeriodo(DateTime? dataInicio, DateTime? dataFim, int? ano, int? mes)
        {
            try
            {
                // [LOGICA] Calcula período por ano/mês ou intervalo
                if (ano.HasValue)
                {
                    if (mes.HasValue)
                    {
                        var inicio = new DateTime(ano.Value, mes.Value, 1);
                        var fim = inicio.AddMonths(1).AddSeconds(-1);
                        return (inicio, fim);
                    }
                    else
                    {
                        var inicio = new DateTime(ano.Value, 1, 1);
                        var fim = new DateTime(ano.Value, 12, 31, 23, 59, 59);
                        return (inicio, fim);
                    }
                }
                else if (dataInicio.HasValue && dataFim.HasValue)
                {
                    return (dataInicio.Value, dataFim.Value);
                }
                else
                {
                    var fim = DateTime.Now.Date.AddDays(1).AddSeconds(-1);
                    var inicio = fim.AddDays(-30);
                    return (inicio, fim);
                }
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message, ex, "DashboardVeiculosController.cs", "ObterPeriodo");
                Alerta.TratamentoErroComLinha("DashboardVeiculosController.cs", "ObterPeriodo", ex);
                return (DateTime.Now.Date.AddDays(-30), DateTime.Now.Date.AddDays(1).AddSeconds(-1));
            }
        }

        #endregion

        #region Lista de Veículos para Filtro

        /// <summary>
        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: ObterListaVeiculos                                                 ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Retorna veículos ativos para filtros no frontend.                          ║
        /// ║                                                                              ║
        /// ║ 🎯 IMPORTÂNCIA PARA A SOLUÇÃO:                                              ║
        /// ║    Preenche selects e filtros de análise.                                    ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • N/A                                                                     ║
        /// ║                                                                              ║
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com veículos ativos.                                ║
        /// ║    • Consumidor: UI de Dashboard de Veículos.                                ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📞 FUNÇÕES QUE CHAMA:                                                        ║
        /// ║    • _context.Veiculo → consulta EF Core.                                    ║
        /// ║    • _log.Error() / Alerta.TratamentoErroComLinha() → erros.                  ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📲 CHAMADA POR:                                                              ║
        /// ║    • GET /api/DashboardVeiculos/ObterListaVeiculos                            ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 🔗 ESCOPO: EXTERNA - Dashboard                                               ║
        /// ║    • Arquivos relacionados: Pages/Veiculos/DashboardVeiculos.cshtml           ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        /// </summary>
        [HttpGet]
        [Route("api/DashboardVeiculos/ObterListaVeiculos")]
        public async Task<IActionResult> ObterListaVeiculos()
        {
            try
            {
                // [DADOS] Carrega veículos ativos
                var veiculos = await _context.Veiculo
                    .Where(v => v.Status == true)
                    .Select(v => new
                    {
                        veiculoId = v.VeiculoId,
                        placa = v.Placa,
                        modelo = v.ModeloVeiculo != null ? v.ModeloVeiculo.DescricaoModelo : "N/A"
                    })
                    .OrderBy(v => v.placa)
                    .AsNoTracking()
                    .ToListAsync();

                return Json(new { success = true, data = veiculos });
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message, ex, "DashboardVeiculosController.cs", "ObterListaVeiculos");
                Alerta.TratamentoErroComLinha("DashboardVeiculosController.cs", "ObterListaVeiculos", ex);
                return Json(new { success = false, message = ex.Message });
            }
        }

        #endregion

        #region Estatísticas Gerais de Veículos

        /// <summary>
        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: ObterEstatisticasGerais                                            ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Consolida métricas macro de frota ativa e custos no período.              ║
        /// ║                                                                              ║
        /// ║ 🎯 IMPORTÂNCIA PARA A SOLUÇÃO:                                              ║
        /// ║    Base de KPIs financeiros e operacionais do dashboard.                     ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • dataInicio (DateTime?): início do filtro.                               ║
        /// ║    • dataFim (DateTime?): fim do filtro.                                     ║
        /// ║    • ano (int?): ano de referência.                                          ║
        /// ║    • mes (int?): mês de referência.                                          ║
        /// ║                                                                              ║
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com métricas consolidadas.                           ║
        /// ║    • Consumidor: UI de Dashboard de Veículos.                                ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📞 FUNÇÕES QUE CHAMA:                                                        ║
        /// ║    • ObterPeriodo() → cálculo de intervalo.                                  ║
        /// ║    • _context.Veiculo/Manutencao/Abastecimento/Viagem → consultas EF Core.   ║
        /// ║    • _log.Error() / Alerta.TratamentoErroComLinha() → erros.                  ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📲 CHAMADA POR:                                                              ║
        /// ║    • GET /api/DashboardVeiculos/ObterEstatisticasGerais                       ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 🔗 ESCOPO: EXTERNA - Dashboard                                               ║
        /// ║    • Arquivos relacionados: Pages/Veiculos/DashboardVeiculos.cshtml           ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        /// </summary>
        [HttpGet]
        [Route("api/DashboardVeiculos/ObterEstatisticasGerais")]
        public async Task<IActionResult> ObterEstatisticasGerais(DateTime? dataInicio, DateTime? dataFim, int? ano, int? mes)
        {
            try
            {
                // [LOGICA] Determina período de consulta
                var periodo = ObterPeriodo(dataInicio, dataFim, ano, mes);
                dataInicio = periodo.dataInicio;
                dataFim = periodo.dataFim;

                // [DADOS] KPIs de frota
                var totalVeiculos = await _context.Veiculo.CountAsync(v => v.Status == true);
                var veiculosProprios = await _context.Veiculo.CountAsync(v => v.Status == true && v.VeiculoProprio == true);
                var veiculosTerceirizados = await _context.Veiculo.CountAsync(v => v.Status == true && v.VeiculoProprio == false);
                var veiculosManutencao = await _context.Manutencao
                    .Where(m => m.DataSolicitacao >= dataInicio && (m.DataDevolucao == null || m.DataDevolucao <= dataFim))
                    .Select(m => m.VeiculoId)
                    .Distinct()
                    .CountAsync();

                // [DADOS] Custos do período
                var custoAbastecimento = await _context.Abastecimento
                    .Where(a => a.DataHora >= dataInicio && a.DataHora <= dataFim)
                    .SumAsync(a => (decimal?)(a.Litros * a.ValorUnitario) ?? 0);

                // [REGRA] Custo de manutenção não rastreado financeiramente (locadoras)
                var custoManutencao = 0m;

                var custoLavagem = await _context.Viagem
                    .Where(v => v.DataInicial >= dataInicio && v.DataInicial <= dataFim)
                    .SumAsync(v => (decimal?)(v.CustoLavador ?? 0) ?? 0);

                var kmTotal = await _context.Viagem
                    .Where(v => v.DataInicial >= dataInicio && v.DataInicial <= dataFim)
                    .Where(v => v.KmInicial.HasValue && v.KmFinal.HasValue)
                    .SumAsync(v => (decimal)((v.KmFinal ?? 0) - (v.KmInicial ?? 0)));

                return Json(new
                {
                    success = true,
                    totalVeiculos,
                    veiculosProprios,
                    veiculosTerceirizados,
                    veiculosManutencao,
                    custoAbastecimento,
                    custoManutencao,
                    custoLavagem,
                    kmTotal
                });
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message, ex, "DashboardVeiculosController.cs", "ObterEstatisticasGerais");
                Alerta.TratamentoErroComLinha("DashboardVeiculosController.cs", "ObterEstatisticasGerais", ex);
                return Json(new { success = false, message = ex.Message });
            }
        }

        #endregion

        #region Dados Individuais do Veículo

        /// <summary>
        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: ObterDadosVeiculo                                                  ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Retorna perfil detalhado e indicadores específicos de um veículo.         ║
        /// ║                                                                              ║
        /// ║ 🎯 IMPORTÂNCIA PARA A SOLUÇÃO:                                              ║
        /// ║    Habilita análise individual de performance e custos.                      ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • veiculoId (Guid): identificador do veículo.                             ║
        /// ║    • dataInicio (DateTime?): início do filtro.                               ║
        /// ║    • dataFim (DateTime?): fim do filtro.                                     ║
        /// ║    • ano (int?): ano de referência.                                          ║
        /// ║    • mes (int?): mês de referência.                                          ║
        /// ║                                                                              ║
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com indicadores do veículo.                          ║
        /// ║    • Consumidor: UI de Dashboard de Veículos.                                ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📞 FUNÇÕES QUE CHAMA:                                                        ║
        /// ║    • ObterPeriodo() → cálculo de intervalo.                                  ║
        /// ║    • _context.Veiculo/Viagem/Abastecimento → consultas EF Core.              ║
        /// ║    • _log.Error() / Alerta.TratamentoErroComLinha() → erros.                  ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📲 CHAMADA POR:                                                              ║
        /// ║    • GET /api/DashboardVeiculos/ObterDadosVeiculo                             ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 🔗 ESCOPO: EXTERNA - Dashboard                                               ║
        /// ║    • Arquivos relacionados: Pages/Veiculos/DashboardVeiculos.cshtml           ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        /// </summary>
        [HttpGet]
        [Route("api/DashboardVeiculos/ObterDadosVeiculo")]
        public async Task<IActionResult> ObterDadosVeiculo(Guid veiculoId, DateTime? dataInicio, DateTime? dataFim, int? ano, int? mes)
        {
            try
            {
            // [LOGICA] Determina período de consulta
                var periodo = ObterPeriodo(dataInicio, dataFim, ano, mes);
                dataInicio = periodo.dataInicio;
                dataFim = periodo.dataFim;

                var veiculo = await _context.Veiculo
                    .Include(v => v.ModeloVeiculo)
                    .Include(v => v.MarcaVeiculo)
                    .Where(v => v.VeiculoId == veiculoId)
                    .FirstOrDefaultAsync();

                if (veiculo == null)
                {
                    return Json(new { success = false, message = "Veículo não encontrado" });
                }

                var viagens = await _context.Viagem
                    .Where(v => v.VeiculoId == veiculoId && v.DataInicial >= dataInicio && v.DataInicial <= dataFim)
                    .ToListAsync();

                var kmPercorrido = viagens
                    .Where(v => v.KmInicial.HasValue && v.KmFinal.HasValue)
                    .Sum(v => (v.KmFinal ?? 0) - (v.KmInicial ?? 0));

                var qtdViagens = viagens.Count;

                var abastecimentos = await _context.Abastecimento
                    .Where(a => a.VeiculoId == veiculoId && a.DataHora >= dataInicio && a.DataHora <= dataFim)
                    .ToListAsync();

                var litrosAbastecidos = abastecimentos.Sum(a => a.Litros ?? 0);
                var valorAbastecimento = abastecimentos.Sum(a => (a.Litros * a.ValorUnitario) ?? 0);
                var mediaConsumo = litrosAbastecidos > 0 ? (double)kmPercorrido / (double)litrosAbastecidos : 0;

                var manutencoes = await _context.Manutencao
                    .Where(m => m.VeiculoId == veiculoId && m.DataSolicitacao >= dataInicio && m.DataSolicitacao <= dataFim)
                    .ToListAsync();

                var valorManutencao = 0m;
                var qtdManutencoes = manutencoes.Count;

                return Json(new
                {
                    success = true,
                    veiculo = new
                    {
                        veiculo.Placa,
                        Modelo = veiculo.ModeloVeiculo?.DescricaoModelo ?? "N/A",
                        Marca = veiculo.MarcaVeiculo?.DescricaoMarca ?? "N/A",
                        veiculo.AnoFabricacao,
                        veiculo.Renavam,
                        veiculo.Quilometragem,
                        Proprio = veiculo.VeiculoProprio == true ? "Sim" : "Não"
                    },
                    estatisticas = new
                    {
                        kmPercorrido,
                        qtdViagens,
                        litrosAbastecidos = Math.Round((double)litrosAbastecidos, 2),
                        valorAbastecimento = Math.Round((double)valorAbastecimento, 2),
                        mediaConsumo = Math.Round(mediaConsumo, 2),
                        valorManutencao = Math.Round((double)valorManutencao, 2),
                        qtdManutencoes
                    }
                });
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message, ex, "DashboardVeiculosController.cs", "ObterDadosVeiculo");
                Alerta.TratamentoErroComLinha("DashboardVeiculosController.cs", "ObterDadosVeiculo", ex);
                return Json(new { success = false, message = ex.Message });
            }
        }

        #endregion

        #region Ranking de Veículos (Top 10 KM)

        /// <summary>
        /// (IA) Ranking dos veículos com maior quilometragem percorrida no período selecionado.
        /// </summary>
        [HttpGet]
        [Route("api/DashboardVeiculos/ObterTop10VeiculosKm")]
        public async Task<IActionResult> ObterTop10VeiculosKm(DateTime? dataInicio, DateTime? dataFim, int? ano, int? mes)
        {
            try
            {
                var periodo = ObterPeriodo(dataInicio, dataFim, ano, mes);

                var ranking = await _context.Viagem
                    .Where(v => v.DataInicial >= periodo.dataInicio && v.DataInicial <= periodo.dataFim && v.VeiculoId.HasValue)
                    .GroupBy(v => new { v.Veiculo.Placa, v.Veiculo.ModeloVeiculo.DescricaoModelo })
                    .Select(g => new
                    {
                        Veiculo = $"{g.Key.Placa} - {g.Key.DescricaoModelo}",
                        KmTotal = g.Sum(v => (v.KmFinal ?? 0) - (v.KmInicial ?? 0))
                    })
                    .OrderByDescending(r => r.KmTotal)
                    .Take(10)
                    .ToListAsync();

                return Json(new { success = true, data = ranking });
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message, ex, "DashboardVeiculosController.cs", "ObterTop10VeiculosKm");
                Alerta.TratamentoErroComLinha("DashboardVeiculosController.cs", "ObterTop10VeiculosKm", ex);
                return Json(new { success = false, message = ex.Message });
            }
        }

        #endregion

        #region Custo Total por Veículo (Top 10)

        /// <summary>
        /// (IA) Ranking de custos operacionais (abastecimento) por veículo no período.
        /// </summary>
        [HttpGet]
        [Route("api/DashboardVeiculos/ObterTop10CustoVeiculos")]
        public async Task<IActionResult> ObterTop10CustoVeiculos(DateTime? dataInicio, DateTime? dataFim, int? ano, int? mes)
        {
            try
            {
                var periodo = ObterPeriodo(dataInicio, dataFim, ano, mes);

                // Custo Abastecimento
                var abastecimentos = await _context.Abastecimento
                    .Where(a => a.DataHora >= periodo.dataInicio && a.DataHora <= periodo.dataFim && a.VeiculoId != Guid.Empty)
                    .GroupBy(a => a.VeiculoId)
                    .Select(g => new { VeiculoId = g.Key, Custo = g.Sum(a => (a.Litros * a.ValorUnitario) ?? 0) })
                    .ToListAsync();

                // Custo de manutenção não é rastreado (veículos são alugados) - removido do cálculo

                // Unir Custos
                var custosTotais = abastecimentos
                    .Select(a => new { a.VeiculoId, Custo = a.Custo })
                    .GroupBy(x => x.VeiculoId)
                    .Select(g => new { VeiculoId = g.Key, CustoTotal = g.Sum(x => x.Custo) })
                    .OrderByDescending(x => x.CustoTotal)
                    .Take(10)
                    .ToList();

                // Buscar detalhes dos veículos
                var veiculoIds = custosTotais.Select(c => c.VeiculoId).ToList();
                var veiculosInfo = await _context.Veiculo
                    .Where(v => veiculoIds.Contains(v.VeiculoId))
                    .Select(v => new { v.VeiculoId, v.Placa, Modelo = v.ModeloVeiculo.DescricaoModelo })
                    .ToListAsync();

                var resultado = custosTotais.Join(veiculosInfo,
                    c => c.VeiculoId,
                    v => v.VeiculoId,
                    (c, v) => new
                    {
                        Veiculo = $"{v.Placa} - {v.Modelo}",
                        CustoTotal = Math.Round(c.CustoTotal, 2)
                    })
                    .ToList();

                return Json(new { success = true, data = resultado });
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message, ex, "DashboardVeiculosController.cs", "ObterTop10CustoVeiculos");
                Alerta.TratamentoErroComLinha("DashboardVeiculosController.cs", "ObterTop10CustoVeiculos", ex);
                return Json(new { success = false, message = ex.Message });
            }
        }

        #endregion
    }
}

