using FrotiX.Data;
using FrotiX.Models;
using FrotiX.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Syncfusion.Drawing;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Grid;
using System.Text.Json;
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
    *  #   MODULO:  DASHBOARD ANALÍTICO DE VIAGENS E OPERAÇÕES                                         #
    *  #   DATA:    2026 (Modernização FrotiX 2026)                                                   #
    *  #                                                                                               #
    *  #################################################################################################
    */

    /// <summary>
    /// <para>────────────────────────────────────────────────────────────────────────────────────────────</para>
    /// <para>CLASSE: <c>DashboardViagensController</c></para>
    /// <para>DESCRIÇÃO: Interface analítica para monitoramento de rotas, custos e produtividade operacional.</para>
    /// <para>PADRÃO: FrotiX 2026 - (IA) Documented & Modernized </para>
    /// <para>────────────────────────────────────────────────────────────────────────────────────────────</para>
    /// </summary>
    [Authorize]
    public partial class DashboardViagensController : Controller
    {
        private readonly FrotiXDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogService _log;

        // Constante para filtro de outliers - viagens com mais de 2000 km são consideradas erro
        private const decimal KM_MAXIMO_POR_VIAGEM = 2000m;

        /// <summary>
        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: DashboardViagensController (Constructor)                           ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Inicializa o dashboard de viagens com DbContext, Identity e log.          ║
        /// ║                                                                              ║
        /// ║ 🎯 IMPORTÂNCIA PARA A SOLUÇÃO:                                              ║
        /// ║    Habilita análises de rotas, custos e produtividade.                        ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • context (FrotiXDbContext): contexto EF Core.                             ║
        /// ║    • userManager (UserManager<IdentityUser>): identidade.                    ║
        /// ║    • logService (ILogService): log centralizado.                              ║
        /// ║                                                                              ║
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • Tipo: N/A                                                               ║
        /// ║    • Significado: N/A                                                        ║
        /// ║    • Consumidor: runtime do ASP.NET Core.                                    ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📞 FUNÇÕES QUE CHAMA:                                                        ║
        /// ║    • Alerta.TratamentoErroComLinha() → tratamento de erro.                    ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📲 CHAMADA POR:                                                              ║
        /// ║    • Injeção de dependência ao instanciar o controller.                       ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 🔗 ESCOPO: INTERNA ao módulo                                                 ║
        /// ║    • Arquivos relacionados: Program.cs                                       ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        /// </summary>
        public DashboardViagensController(FrotiXDbContext context, UserManager<IdentityUser> userManager, ILogService logService)
        {
            try
            {
                _context = context;
                _userManager = userManager;
                _log = logService;
            }
            catch (Exception ex)
            {
                Alerta.TratamentoErroComLinha("DashboardViagensController.cs", "Constructor", ex);
            }
        }

        #region Estatísticas Gerais

        /// <summary>
        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: ObterEstatisticasGerais                                             ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Retorna KPIs de viagens e custos, com comparativo do período anterior.    ║
        /// ║                                                                              ║
        /// ║ 🎯 IMPORTÂNCIA PARA A SOLUÇÃO:                                              ║
        /// ║    Base analítica do dashboard de viagens.                                   ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • dataInicio (DateTime?): início do período.                              ║
        /// ║    • dataFim (DateTime?): fim do período.                                    ║
        /// ║                                                                              ║
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com KPIs e comparativo.                             ║
        /// ║    • Consumidor: UI de Dashboard de Viagens.                                 ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📞 FUNÇÕES QUE CHAMA:                                                        ║
        /// ║    • _context.Viagem → consultas EF Core.                                    ║
        /// ║    • _log.Error() / Alerta.TratamentoErroComLinha() → erros.                  ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📲 CHAMADA POR:                                                              ║
        /// ║    • GET /api/DashboardViagens/ObterEstatisticasGerais                        ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 🔗 ESCOPO: EXTERNA - Dashboard                                               ║
        /// ║    • Arquivos relacionados: Pages/Viagens/DashboardViagens.cshtml             ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        /// </summary>
        [HttpGet]
        [Route("api/DashboardViagens/ObterEstatisticasGerais")]
        public async Task<IActionResult> ObterEstatisticasGerais(DateTime? dataInicio, DateTime? dataFim)
        {
            try
            {
                // [REGRA] Intervalo padrão quando datas não informadas
                if (!dataInicio.HasValue || !dataFim.HasValue)
                {
                    dataFim = DateTime.Now.Date.AddDays(1).AddSeconds(-1);
                    dataInicio = dataFim.Value.AddDays(-30);
                }

                // [DADOS] Coleta viagens do período
                var viagens = await _context.Viagem
                    .AsNoTracking()
                    .Where(v => v.DataInicial >= dataInicio && v.DataInicial <= dataFim)
                    .ToListAsync();

                // [LOGICA] KPIs por status
                var totalViagens = viagens.Count;
                var viagensFinalizadas = viagens.Count(v => v.Status == "Realizada");
                var viagensAgendadas = viagens.Count(v => v.Status == "Agendada");
                var viagensCanceladas = viagens.Count(v => v.Status == "Cancelada");
                var viagensEmAndamento = viagens.Count(v => v.Status == "Aberta");

                // [LOGICA] Filtra km válido (sem outliers)
                var viagensComKmValido = viagens
                    .Where(v => v.KmInicial.HasValue && v.KmFinal.HasValue)
                    .Where(v => v.KmFinal.Value >= v.KmInicial.Value)
                    .Where(v => (v.KmFinal.Value - v.KmInicial.Value) <= KM_MAXIMO_POR_VIAGEM)
                    .ToList();

                var kmTotal = viagensComKmValido.Sum(v => (v.KmFinal ?? 0) - (v.KmInicial ?? 0));

                var viagensParaMedia = viagensComKmValido.Count;

                var viagensRealizadasComKmValido = viagensComKmValido
                    .Where(v => v.Status == "Realizada")
                    .ToList();
                var custoCombustivel = viagensRealizadasComKmValido.Sum(v => v.CustoCombustivel ?? 0);

                var viagensRealizadas = viagens.Where(v => v.Status == "Realizada").ToList();
                var custoLavador = viagensRealizadas.Sum(v => v.CustoLavador ?? 0);
                var custoMotorista = viagensRealizadas.Sum(v => v.CustoMotorista ?? 0);
                var custoOperador = viagensRealizadas.Sum(v => v.CustoOperador ?? 0);
                var custoVeiculo = viagensRealizadas.Sum(v => v.CustoVeiculo ?? 0);

                var custoTotal = custoCombustivel + custoLavador + custoMotorista + custoOperador + custoVeiculo;

                var custoMedioPorViagem = viagensFinalizadas > 0 ? custoTotal / viagensFinalizadas : 0;
                var kmMedioPorViagem = viagensParaMedia > 0 ? (double)kmTotal / viagensParaMedia : 0;

                // [LOGICA] Período anterior para comparativo
                var diasPeriodo = (dataFim.Value - dataInicio.Value).Days;
                var dataInicioAnterior = dataInicio.Value.AddDays(-(diasPeriodo + 1));
                var dataFimAnterior = dataInicio.Value.AddSeconds(-1);

                // [DADOS] Viagens do período anterior
                var viagensAnteriores = await _context.Viagem
                    .AsNoTracking()
                    .Where(v => v.DataInicial >= dataInicioAnterior && v.DataInicial <= dataFimAnterior)
                    .ToListAsync();

                var totalViagensAnteriores = viagensAnteriores.Count;
                var viagensFinalizadasAnterior = viagensAnteriores.Count(v => v.Status == "Realizada");
                var viagensAgendadasAnterior = viagensAnteriores.Count(v => v.Status == "Agendada");
                var viagensCanceladasAnterior = viagensAnteriores.Count(v => v.Status == "Cancelada");
                var viagensEmAndamentoAnterior = viagensAnteriores.Count(v => v.Status == "Aberta");

                var viagensAnterioresComKmValido = viagensAnteriores
                    .Where(v => v.KmInicial.HasValue && v.KmFinal.HasValue)
                    .Where(v => v.KmFinal.Value >= v.KmInicial.Value)
                    .Where(v => (v.KmFinal.Value - v.KmInicial.Value) <= KM_MAXIMO_POR_VIAGEM)
                    .ToList();

                var kmTotalAnterior = viagensAnterioresComKmValido.Sum(v => (v.KmFinal ?? 0) - (v.KmInicial ?? 0));
                var viagensParaMediaAnterior = viagensAnterioresComKmValido.Count;
                var kmMedioPorViagemAnterior = viagensParaMediaAnterior > 0 ? (double)kmTotalAnterior / viagensParaMediaAnterior : 0;

                var viagensAnterioresRealizadasComKmValido = viagensAnterioresComKmValido
                    .Where(v => v.Status == "Realizada")
                    .ToList();
                var custoCombustivelAnterior = viagensAnterioresRealizadasComKmValido.Sum(v => v.CustoCombustivel ?? 0);
                var viagensAnterioresRealizadas = viagensAnteriores.Where(v => v.Status == "Realizada").ToList();
                var custoLavadorAnterior = viagensAnterioresRealizadas.Sum(v => v.CustoLavador ?? 0);
                var custoMotoristaAnterior = viagensAnterioresRealizadas.Sum(v => v.CustoMotorista ?? 0);
                var custoOperadorAnterior = viagensAnterioresRealizadas.Sum(v => v.CustoOperador ?? 0);
                var custoVeiculoAnterior = viagensAnterioresRealizadas.Sum(v => v.CustoVeiculo ?? 0);
                var custoTotalAnterior = custoCombustivelAnterior + custoLavadorAnterior + custoMotoristaAnterior + custoOperadorAnterior + custoVeiculoAnterior;

                var custoMedioPorViagemAnterior = viagensFinalizadasAnterior > 0 ? custoTotalAnterior / viagensFinalizadasAnterior : 0;

                return Json(new
                {
                    success = true,
                    totalViagens,
                    viagensFinalizadas,
                    viagensAgendadas,
                    viagensCanceladas,
                    viagensEmAndamento,
                    custoTotal = Math.Round(custoTotal, 2),
                    custoCombustivel = Math.Round(custoCombustivel, 2),
                    custoLavador = Math.Round(custoLavador, 2),
                    custoMotorista = Math.Round(custoMotorista, 2),
                    custoOperador = Math.Round(custoOperador, 2),
                    custoVeiculo = Math.Round(custoVeiculo, 2),
                    kmTotal,
                    kmMedioPorViagem = Math.Round(kmMedioPorViagem, 2),
                    custoMedioPorViagem = Math.Round(custoMedioPorViagem, 2),
                    viagensComKm = viagens.Count(v => v.KmInicial.HasValue && v.KmFinal.HasValue),
                    viagensKmValido = viagensParaMedia,
                    periodoAnterior = new
                    {
                        totalViagens = totalViagensAnteriores,
                        viagensFinalizadas = viagensFinalizadasAnterior,
                        viagensAgendadas = viagensAgendadasAnterior,
                        viagensCanceladas = viagensCanceladasAnterior,
                        viagensEmAndamento = viagensEmAndamentoAnterior,
                        custoTotal = Math.Round(custoTotalAnterior, 2),
                        custoCombustivel = Math.Round(custoCombustivelAnterior, 2),
                        custoLavador = Math.Round(custoLavadorAnterior, 2),
                        custoMotorista = Math.Round(custoMotoristaAnterior, 2),
                        custoOperador = Math.Round(custoOperadorAnterior, 2),
                        custoVeiculo = Math.Round(custoVeiculoAnterior, 2),
                        custoMedioPorViagem = Math.Round(custoMedioPorViagemAnterior, 2),
                        kmTotal = kmTotalAnterior,
                        kmMedioPorViagem = Math.Round(kmMedioPorViagemAnterior, 2)
                    }
                });
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message, ex, "DashboardViagensController.cs", "ObterEstatisticasGerais");
                Alerta.TratamentoErroComLinha("DashboardViagensController.cs", "ObterEstatisticasGerais", ex);
                return Json(new { success = false, message = ex.Message });
            }
        }

        #endregion

        #region Viagens por Dia da Semana

        /// <summary>
        /// (IA) Agrupa o volume de viagens por dia da semana para análise de sazonalidade semanal.
        /// </summary>
        [HttpGet]
        [Route("api/DashboardViagens/ObterViagensPorDia")]
        public async Task<IActionResult> ObterViagensPorDia(DateTime? dataInicio, DateTime? dataFim)
        {
            try
            {
                if (!dataInicio.HasValue || !dataFim.HasValue)
                {
                    dataFim = DateTime.Now.Date.AddDays(1).AddSeconds(-1);
                    dataInicio = dataFim.Value.AddDays(-30);
                }

                var viagens = await _context.Viagem
                    .AsNoTracking()
                    .Where(v => v.DataInicial >= dataInicio && v.DataInicial <= dataFim && v.DataInicial.HasValue)
                    .ToListAsync();

                var diasSemana = new Dictionary<DayOfWeek, (string Nome, int Ordem)>
                {
                    { DayOfWeek.Monday, ("Segunda", 1) },
                    { DayOfWeek.Tuesday, ("Terça", 2) },
                    { DayOfWeek.Wednesday, ("Quarta", 3) },
                    { DayOfWeek.Thursday, ("Quinta", 4) },
                    { DayOfWeek.Friday, ("Sexta", 5) },
                    { DayOfWeek.Saturday, ("Sábado", 6) },
                    { DayOfWeek.Sunday, ("Domingo", 7) }
                };

                var viagensPorDiaSemana = viagens
                    .GroupBy(v => v.DataInicial.Value.DayOfWeek)
                    .Select(g => new
                    {
                        diaSemana = diasSemana[g.Key].Nome,
                        ordem = diasSemana[g.Key].Ordem,
                        total = g.Count(),
                        finalizadas = g.Count(v => v.Status == "Realizada"),
                        agendadas = g.Count(v => v.Status == "Agendada"),
                        canceladas = g.Count(v => v.Status == "Cancelada"),
                        emAndamento = g.Count(v => v.Status == "Aberta")
                    })
                    .OrderBy(x => x.ordem)
                    .ToList();

                return Json(new { success = true, data = viagensPorDiaSemana });
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message, ex, "DashboardViagensController.cs", "ObterViagensPorDia");
                Alerta.TratamentoErroComLinha("DashboardViagensController.cs", "ObterViagensPorDia", ex);
                return Json(new { success = false, message = ex.Message });
            }
        }

        #endregion

        #region Viagens por Status

        /// <summary>
        /// (IA) Retorna a distribuição de viagens agrupadas por seu status operacional.
        /// </summary>
        [HttpGet]
        [Route("api/DashboardViagens/ObterViagensPorStatus")]
        public async Task<IActionResult> ObterViagensPorStatus(DateTime? dataInicio, DateTime? dataFim)
        {
            try
            {
                if (!dataInicio.HasValue || !dataFim.HasValue)
                {
                    dataFim = DateTime.Now.Date.AddDays(1).AddSeconds(-1);
                    dataInicio = dataFim.Value.AddDays(-30);
                }

                var viagens = await _context.Viagem
                    .AsNoTracking()
                    .Where(v => v.DataInicial >= dataInicio && v.DataInicial <= dataFim)
                    .ToListAsync();

                var viagensPorStatus = viagens
                    .GroupBy(v => v.Status ?? "Não Informado")
                    .Select(g => new
                    {
                        status = g.Key,
                        total = g.Count()
                    })
                    .OrderByDescending(x => x.total)
                    .ToList();

                return Json(new { success = true, data = viagensPorStatus });
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message, ex, "DashboardViagensController.cs", "ObterViagensPorStatus");
                Alerta.TratamentoErroComLinha("DashboardViagensController.cs", "ObterViagensPorStatus", ex);
                return Json(new { success = false, message = ex.Message });
            }
        }

        #endregion

        #region Top 10 Motoristas

        /// <summary>
        /// (IA) Retorna o ranking de motoristas com maior volume de viagens, utilizando cache JSON de estatísticas para performance.
        /// </summary>
        [HttpGet]
        [Route("api/DashboardViagens/ObterViagensPorMotorista")]
        public async Task<IActionResult> ObterViagensPorMotorista(DateTime? dataInicio, DateTime? dataFim, int top = 10)
        {
            try
            {
                if (!dataInicio.HasValue || !dataFim.HasValue)
                {
                    dataFim = DateTime.Now.Date.AddDays(1).AddSeconds(-1);
                    dataInicio = dataFim.Value.AddDays(-30);
                }

                var estatisticas = await _context.ViagemEstatistica
                    .AsNoTracking()
                    .Where(v => v.DataReferencia >= dataInicio.Value.Date && v.DataReferencia <= dataFim.Value.Date)
                    .ToListAsync();

                var motoristaDict = new Dictionary<string, int>();

                foreach (var est in estatisticas)
                {
                    if (!string.IsNullOrEmpty(est.ViagensPorMotoristaJson))
                    {
                        try
                        {
                            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                            var lista = JsonSerializer.Deserialize<List<Dictionary<string, JsonElement>>>(est.ViagensPorMotoristaJson, options);

                            if (lista != null)
                            {
                                foreach (var item in lista)
                                {
                                    string motorista = null;
                                    int totalViagens = 0;

                                    if (item.ContainsKey("motorista"))
                                        motorista = item["motorista"].GetString();

                                    if (item.ContainsKey("totalViagens"))
                                        totalViagens = item["totalViagens"].GetInt32();
                                    else if (item.ContainsKey("quantidade"))
                                        totalViagens = item["quantidade"].GetInt32();

                                    if (motorista != null && totalViagens > 0)
                                    {
                                        if (!motoristaDict.ContainsKey(motorista))
                                            motoristaDict[motorista] = 0;

                                        motoristaDict[motorista] += totalViagens;
                                    }
                                }
                            }
                        }
                        catch
                        {
                            // (IA) Fallback silencioso para erros de serialização parcial.
                        }
                    }
                }

                var dados = motoristaDict
                    .Select(kv => new { motorista = kv.Key, totalViagens = kv.Value })
                    .OrderByDescending(x => x.totalViagens)
                    .Take(top)
                    .ToList();

                return Json(new { success = true, data = dados });
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message, ex, "DashboardViagensController.cs", "ObterViagensPorMotorista");
                Alerta.TratamentoErroComLinha("DashboardViagensController.cs", "ObterViagensPorMotorista", ex);
                return Json(new { success = false, message = ex.Message });
            }
        }

        #endregion

        #region Viagens por Setor

        /// <summary>
        /// (IA) Retorna o ranking de setores solicitantes por volume de viagens, com normalização de nomes para identificar o CTRAN.
        /// </summary>
        [HttpGet]
        [Route("api/DashboardViagens/ObterViagensPorSetor")]
        public async Task<IActionResult> ObterViagensPorSetor(DateTime? dataInicio, DateTime? dataFim, int top = 6)
        {
            try
            {
                if (!dataInicio.HasValue || !dataFim.HasValue)
                {
                    dataFim = DateTime.Now.Date.AddDays(1).AddSeconds(-1);
                    dataInicio = dataFim.Value.AddDays(-30);
                }

                var setoresDb = await _context.SetorSolicitante
                    .AsNoTracking()
                    .Where(s => s.Status == true && !string.IsNullOrEmpty(s.Nome))
                    .Select(s => new { s.Nome, s.Sigla })
                    .ToListAsync();

                var dictNomeParaSigla = setoresDb
                    .GroupBy(s => s.Nome.Trim().ToUpper())
                    .ToDictionary(
                        g => g.Key,
                        g => !string.IsNullOrEmpty(g.First().Sigla) ? g.First().Sigla.Trim() : g.First().Nome.Trim()
                    );

                var estatisticas = await _context.ViagemEstatistica
                    .AsNoTracking()
                    .Where(v => v.DataReferencia >= dataInicio.Value.Date && v.DataReferencia <= dataFim.Value.Date)
                    .ToListAsync();

                var setorDict = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

                foreach (var est in estatisticas)
                {
                    if (!string.IsNullOrEmpty(est.ViagensPorSetorJson))
                    {
                        try
                        {
                            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                            var lista = JsonSerializer.Deserialize<List<Dictionary<string, JsonElement>>>(est.ViagensPorSetorJson, options);

                            if (lista != null)
                            {
                                foreach (var item in lista)
                                {
                                    string setor = null;
                                    int totalViagens = 0;

                                    if (item.ContainsKey("setor"))
                                        setor = item["setor"].GetString();

                                    if (item.ContainsKey("totalViagens"))
                                        totalViagens = item["totalViagens"].GetInt32();
                                    else if (item.ContainsKey("quantidade"))
                                        totalViagens = item["quantidade"].GetInt32();

                                    if (!string.IsNullOrEmpty(setor) && totalViagens > 0)
                                    {
                                        var chaveSetor = setor.Trim().ToUpper();
                                        var siglaOuNome = dictNomeParaSigla.ContainsKey(chaveSetor)
                                            ? dictNomeParaSigla[chaveSetor]
                                            : setor;

                                        if (!setorDict.ContainsKey(siglaOuNome))
                                            setorDict[siglaOuNome] = 0;

                                        setorDict[siglaOuNome] += totalViagens;
                                    }
                                }
                            }
                        }
                        catch { }
                    }
                }

                var chaveCtran = setorDict.Keys
                    .FirstOrDefault(k => k.ToUpper().Contains("COORDENAÇÃO DE TRANSPORTES") ||
                                        k.ToUpper().Contains("COORDENACAO DE TRANSPORTES") ||
                                        k.ToUpper().Contains("CTRAN"));

                int viagensCtran = 0;
                if (chaveCtran != null)
                {
                    viagensCtran = setorDict[chaveCtran];
                    setorDict.Remove(chaveCtran);
                }

                var dados = setorDict
                    .Select(kv => new { setor = kv.Key, totalViagens = kv.Value })
                    .OrderByDescending(x => x.totalViagens)
                    .Take(top)
                    .ToList();

                return Json(new { success = true, data = dados, viagensCtran = viagensCtran });
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message, ex, "DashboardViagensController.cs", "ObterViagensPorSetor");
                Alerta.TratamentoErroComLinha("DashboardViagensController.cs", "ObterViagensPorSetor", ex);
                return Json(new { success = false, message = ex.Message });
            }
        }

        #endregion

        #region Custos por Motorista

        /// <summary>
        /// (IA) Agrega os custos totais por motorista a partir dos snapshots diários de estatísticas.
        /// </summary>
        [HttpGet]
        [Route("api/DashboardViagens/ObterCustosPorMotorista")]
        public async Task<IActionResult> ObterCustosPorMotorista(DateTime? dataInicio, DateTime? dataFim, int top = 10)
        {
            try
            {
                if (!dataInicio.HasValue || !dataFim.HasValue)
                {
                    dataFim = DateTime.Now.Date.AddDays(1).AddSeconds(-1);
                    dataInicio = dataFim.Value.AddDays(-30);
                }

                var estatisticas = await _context.ViagemEstatistica
                    .AsNoTracking()
                    .Where(v => v.DataReferencia >= dataInicio.Value.Date && v.DataReferencia <= dataFim.Value.Date)
                    .ToListAsync();

                var motoristaDict = new Dictionary<string, decimal>();

                foreach (var est in estatisticas)
                {
                    if (!string.IsNullOrEmpty(est.CustosPorMotoristaJson))
                    {
                        try
                        {
                            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                            var lista = JsonSerializer.Deserialize<List<Dictionary<string, JsonElement>>>(est.CustosPorMotoristaJson, options);

                            if (lista != null)
                            {
                                foreach (var item in lista)
                                {
                                    if (item.ContainsKey("motorista") && item.ContainsKey("custoTotal"))
                                    {
                                        var motorista = item["motorista"].GetString();
                                        var custoTotal = item["custoTotal"].GetDecimal();

                                        if (!motoristaDict.ContainsKey(motorista))
                                            motoristaDict[motorista] = 0;

                                        motoristaDict[motorista] += custoTotal;
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            _log.Error($"Erro ao deserializar custos: {ex.Message}", ex, "DashboardViagensController.cs", "ObterCustosPorMotorista");
                        }
                    }
                }

                var dados = motoristaDict
                    .Select(kv => new
                    {
                        motorista = kv.Key,
                        custoTotal = Math.Round(kv.Value, 2)
                    })
                    .OrderByDescending(x => x.custoTotal)
                    .Take(top)
                    .ToList();

                return Json(new
                {
                    success = true,
                    data = dados,
                    debug = new
                    {
                        totalEstatisticas = estatisticas.Count,
                        totalMotoristas = motoristaDict.Count
                    }
                });
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message, ex, "DashboardViagensController.cs", "ObterCustosPorMotorista");
                Alerta.TratamentoErroComLinha("DashboardViagensController.cs", "ObterCustosPorMotorista", ex);
                return Json(new { success = false, message = ex.Message });
            }
        }

        #endregion Custos por Motorista

        #region Custos por Veículo

        /// <summary>
        /// (IA) Retorna o ranking de custos operacionais por veículo no período selecionado.
        /// </summary>
        [HttpGet]
        [Route("api/DashboardViagens/ObterCustosPorVeiculo")]
        public async Task<IActionResult> ObterCustosPorVeiculo(DateTime? dataInicio, DateTime? dataFim, int top = 10)
        {
            try
            {
                if (!dataInicio.HasValue || !dataFim.HasValue)
                {
                    dataFim = DateTime.Now.Date.AddDays(1).AddSeconds(-1);
                    dataInicio = dataFim.Value.AddDays(-30);
                }

                var viagens = await _context.Viagem
                    .AsNoTracking()
                    .Where(v => v.DataInicial >= dataInicio && v.DataInicial <= dataFim && v.VeiculoId != null)
                    .Where(v => v.Status == "Realizada")
                    .Include(v => v.Veiculo)
                    .ThenInclude(vei => vei.ModeloVeiculo)
                    .ThenInclude(mod => mod.MarcaVeiculo)
                    .ToListAsync();

                var custosPorVeiculo = viagens
                    .GroupBy(v => new
                    {
                        v.VeiculoId,
                        v.Veiculo.Placa,
                        Descricao = v.Veiculo.ModeloVeiculo != null && v.Veiculo.ModeloVeiculo.MarcaVeiculo != null
                            ? $"{v.Veiculo.ModeloVeiculo.MarcaVeiculo.DescricaoMarca} {v.Veiculo.ModeloVeiculo.DescricaoModelo} - {v.Veiculo.Placa}"
                            : v.Veiculo.Placa ?? "Não informado"
                    })
                    .Select(g => new
                    {
                        veiculoId = g.Key.VeiculoId,
                        veiculo = g.Key.Descricao ?? "Não informado",
                        custoTotal = Math.Round(
                            g.Sum(v => v.CustoCombustivel ?? 0) +
                            g.Sum(v => v.CustoLavador ?? 0) +
                            g.Sum(v => v.CustoMotorista ?? 0) +
                            g.Sum(v => v.CustoOperador ?? 0) +
                            g.Sum(v => v.CustoVeiculo ?? 0), 2)
                    })
                    .OrderByDescending(x => x.custoTotal)
                    .Take(top)
                    .ToList();

                return Json(new { success = true, data = custosPorVeiculo });
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message, ex, "DashboardViagensController.cs", "ObterCustosPorVeiculo");
                Alerta.TratamentoErroComLinha("DashboardViagensController.cs", "ObterCustosPorVeiculo", ex);
                return Json(new { success = false, message = ex.Message });
            }
        }

        #endregion

        #region Top 10 Viagens Mais Caras

        /// <summary>
        /// (IA) Lista as 10 viagens de maior valor agregado (Custo total) no período informado.
        /// </summary>
        [HttpGet]
        [Route("api/DashboardViagens/ObterTop10ViagensMaisCaras")]
        public async Task<IActionResult> ObterTop10ViagensMaisCaras(DateTime? dataInicio, DateTime? dataFim)
        {
            try
            {
                if (!dataInicio.HasValue || !dataFim.HasValue)
                {
                    dataFim = DateTime.Now.Date.AddDays(1).AddSeconds(-1);
                    dataInicio = dataFim.Value.AddDays(-30);
                }

                var viagens = await _context.Viagem
                    .AsNoTracking()
                    .Where(v => v.DataInicial >= dataInicio && v.DataInicial <= dataFim)
                    .Where(v => v.Status == "Realizada")
                    .Where(v => v.KmInicial.HasValue && v.KmFinal.HasValue)
                    .Where(v => v.KmFinal.Value >= v.KmInicial.Value)
                    .Where(v => (v.KmFinal.Value - v.KmInicial.Value) <= KM_MAXIMO_POR_VIAGEM)
                    .Include(v => v.Motorista)
                    .Include(v => v.Veiculo)
                        .ThenInclude(vei => vei.ModeloVeiculo)
                            .ThenInclude(mod => mod.MarcaVeiculo)
                    .ToListAsync();

                var top10Viagens = viagens
                    .OrderByDescending(v => (v.CustoCombustivel ?? 0d) + (v.CustoLavador ?? 0d) + (v.CustoMotorista ?? 0d) + (v.CustoOperador ?? 0d) + (v.CustoVeiculo ?? 0d))
                    .Take(10)
                    .ToList();

                var top10 = top10Viagens.Select(v => new
                {
                    viagemId = v.ViagemId.ToString(),
                    noFichaVistoria = v.NoFichaVistoria?.ToString() ?? "N/A",
                    status = v.Status ?? "-",
                    dataInicial = v.DataInicial?.ToString("dd/MM/yyyy") ?? "N/A",
                    dataFinal = v.DataFinal?.ToString("dd/MM/yyyy") ?? "N/A",
                    motorista = v.Motorista != null ? v.Motorista.Nome : "Não informado",
                    veiculo = v.Veiculo != null && v.Veiculo.ModeloVeiculo != null && v.Veiculo.ModeloVeiculo.MarcaVeiculo != null
                        ? "(" + v.Veiculo.Placa + ") - " + v.Veiculo.ModeloVeiculo.MarcaVeiculo.DescricaoMarca + "/" + v.Veiculo.ModeloVeiculo.DescricaoModelo
                        : v.Veiculo != null ? v.Veiculo.Placa : "Não informado",
                    kmRodado = v.KmInicial.HasValue && v.KmFinal.HasValue
                        ? v.KmFinal.Value - v.KmInicial.Value
                        : 0m,
                    minutos = v.Minutos ?? 0,
                    finalidade = v.Finalidade ?? "-",
                    custoCombustivel = Math.Round(v.CustoCombustivel ?? 0d, 2),
                    custoVeiculo = Math.Round(v.CustoVeiculo ?? 0d, 2),
                    custoMotorista = Math.Round(v.CustoMotorista ?? 0d, 2),
                    custoOperador = Math.Round(v.CustoOperador ?? 0d, 2),
                    custoLavador = Math.Round(v.CustoLavador ?? 0d, 2),
                    custoTotal = Math.Round(
                        (v.CustoCombustivel ?? 0d) + 
                        (v.CustoLavador ?? 0d) + 
                        (v.CustoMotorista ?? 0d) + 
                        (v.CustoOperador ?? 0d) + 
                        (v.CustoVeiculo ?? 0d), 2)
                }).ToList();

                return Json(new { success = true, data = top10 });
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message, ex, "DashboardViagensController.cs", "ObterTop10ViagensMaisCaras");
                Alerta.TratamentoErroComLinha("DashboardViagensController.cs", "ObterTop10ViagensMaisCaras", ex);
                return Json(new { success = false, message = ex.Message });
            }
        }

        #endregion

        #region Custos por Dia (Evolução de Custos)

        /// <summary>
        /// (IA) Retorna a evolução temporal dos custos operacionais, estratificada por categoria.
        /// </summary>
        [HttpGet]
        [Route("api/DashboardViagens/ObterCustosPorDia")]
        public async Task<IActionResult> ObterCustosPorDia(DateTime? dataInicio, DateTime? dataFim)
        {
            try
            {
                if (!dataInicio.HasValue || !dataFim.HasValue)
                {
                    dataFim = DateTime.Now.Date.AddDays(1).AddSeconds(-1);
                    dataInicio = dataFim.Value.AddDays(-30);
                }

                var estatisticas = await _context.ViagemEstatistica
                    .AsNoTracking()
                    .Where(v => v.DataReferencia >= dataInicio.Value.Date && v.DataReferencia <= dataFim.Value.Date)
                    .OrderBy(v => v.DataReferencia)
                    .ToListAsync();

                var dados = estatisticas.Select(e => new
                {
                    data = e.DataReferencia.ToString("yyyy-MM-dd"),
                    combustivel = Math.Round(e.CustoCombustivel, 2),
                    motorista = Math.Round(e.CustoMotorista, 2),
                    operador = Math.Round(e.CustoOperador, 2),
                    lavador = Math.Round(e.CustoLavador, 2),
                    veiculo = Math.Round(e.CustoVeiculo, 2)
                }).ToList();

                return Json(new { success = true, data = dados });
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message, ex, "DashboardViagensController.cs", "ObterCustosPorDia");
                Alerta.TratamentoErroComLinha("DashboardViagensController.cs", "ObterCustosPorDia", ex);
                return Json(new { success = false, message = ex.Message });
            }
        }

        #endregion

        #region Custos por Tipo (Distribuição de Custos)

        /// <summary>
        /// (IA) Retorna a distribuição percentual e absoluta dos custos por categoria (Combustível, Pessoal, etc).
        /// </summary>
        [HttpGet]
        [Route("api/DashboardViagens/ObterCustosPorTipo")]
        public async Task<IActionResult> ObterCustosPorTipo(DateTime? dataInicio, DateTime? dataFim)
        {
            try
            {
                if (!dataInicio.HasValue || !dataFim.HasValue)
                {
                    dataFim = DateTime.Now.Date.AddDays(1).AddSeconds(-1);
                    dataInicio = dataFim.Value.AddDays(-30);
                }

                var estatisticas = await _context.ViagemEstatistica
                    .AsNoTracking()
                    .Where(v => v.DataReferencia >= dataInicio.Value.Date && v.DataReferencia <= dataFim.Value.Date)
                    .ToListAsync();

                var totais = new
                {
                    custoCombustivel = Math.Round(estatisticas.Sum(e => e.CustoCombustivel), 2),
                    custoMotorista = Math.Round(estatisticas.Sum(e => e.CustoMotorista), 2),
                    custoOperador = Math.Round(estatisticas.Sum(e => e.CustoOperador), 2),
                    custoLavador = Math.Round(estatisticas.Sum(e => e.CustoLavador), 2),
                    custoVeiculo = Math.Round(estatisticas.Sum(e => e.CustoVeiculo), 2)
                };

                var dados = new[]
                {
                    new { tipo = "Combustível", custo = totais.custoCombustivel },
                    new { tipo = "Motorista", custo = totais.custoMotorista },
                    new { tipo = "Operador", custo = totais.custoOperador },
                    new { tipo = "Lavador", custo = totais.custoLavador },
                    new { tipo = "Veículo", custo = totais.custoVeiculo }
                }.Where(x => x.custo > 0).ToList();

                return Json(new { success = true, data = dados });
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message, ex, "DashboardViagensController.cs", "ObterCustosPorTipo");
                Alerta.TratamentoErroComLinha("DashboardViagensController.cs", "ObterCustosPorTipo", ex);
                return Json(new { success = false, message = ex.Message });
            }
        }

        #endregion Custos por Tipo (Distribuição de Custos)

        #region Viagens por Veículo (Top 10 Veículos)

        /// <summary>
        /// (IA) Retorna o ranking de veículos com maior volumetria de viagens realizadas no período.
        /// </summary>
        [HttpGet]
        [Route("api/DashboardViagens/ObterViagensPorVeiculo")]
        public async Task<IActionResult> ObterViagensPorVeiculo(DateTime? dataInicio, DateTime? dataFim, int top = 10)
        {
            try
            {
                if (!dataInicio.HasValue || !dataFim.HasValue)
                {
                    dataFim = DateTime.Now.Date.AddDays(1).AddSeconds(-1);
                    dataInicio = dataFim.Value.AddDays(-30);
                }

                var estatisticas = await _context.ViagemEstatistica
                    .AsNoTracking()
                    .Where(v => v.DataReferencia >= dataInicio.Value.Date && v.DataReferencia <= dataFim.Value.Date)
                    .ToListAsync();

                var veiculoDict = new Dictionary<string, int>();

                foreach (var est in estatisticas)
                {
                    if (!string.IsNullOrEmpty(est.ViagensPorVeiculoJson))
                    {
                        try
                        {
                            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                            var lista = JsonSerializer.Deserialize<List<Dictionary<string, JsonElement>>>(est.ViagensPorVeiculoJson, options);

                            if (lista != null)
                            {
                                foreach (var item in lista)
                                {
                                    string veiculo = null;
                                    int totalViagens = 0;

                                    if (item.ContainsKey("veiculo"))
                                        veiculo = item["veiculo"].GetString();
                                    else if (item.ContainsKey("placa"))
                                        veiculo = item["placa"].GetString();

                                    if (item.ContainsKey("totalViagens"))
                                        totalViagens = item["totalViagens"].GetInt32();
                                    else if (item.ContainsKey("quantidade"))
                                        totalViagens = item["quantidade"].GetInt32();

                                    if (veiculo != null && totalViagens > 0)
                                    {
                                        if (!veiculoDict.ContainsKey(veiculo))
                                            veiculoDict[veiculo] = 0;

                                        veiculoDict[veiculo] += totalViagens;
                                    }
                                }
                            }
                        }
                        catch { }
                    }
                }

                var dados = veiculoDict
                    .Select(kv => new { veiculo = kv.Key, totalViagens = kv.Value })
                    .OrderByDescending(x => x.totalViagens)
                    .Take(top)
                    .ToList();

                return Json(new { success = true, data = dados });
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message, ex, "DashboardViagensController.cs", "ObterViagensPorVeiculo");
                Alerta.TratamentoErroComLinha("DashboardViagensController.cs", "ObterViagensPorVeiculo", ex);
                return Json(new { success = false, message = ex.Message });
            }
        }

        #endregion

        #region Viagens por Finalidade

        /// <summary>
        /// (IA) Retorna a distribuição de viagens agrupadas pela finalidade declarada no agendamento.
        /// </summary>
        [HttpGet]
        [Route("api/DashboardViagens/ObterViagensPorFinalidade")]
        public async Task<IActionResult> ObterViagensPorFinalidade(DateTime? dataInicio, DateTime? dataFim)
        {
            try
            {
                if (!dataInicio.HasValue || !dataFim.HasValue)
                {
                    dataFim = DateTime.Now.Date.AddDays(1).AddSeconds(-1);
                    dataInicio = dataFim.Value.AddDays(-30);
                }

                var estatisticas = await _context.ViagemEstatistica
                    .AsNoTracking()
                    .Where(v => v.DataReferencia >= dataInicio.Value.Date && v.DataReferencia <= dataFim.Value.Date)
                    .ToListAsync();

                var finalidadeDict = new Dictionary<string, int>();

                foreach (var est in estatisticas)
                {
                    if (!string.IsNullOrEmpty(est.ViagensPorFinalidadeJson))
                    {
                        try
                        {
                            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                            var lista = JsonSerializer.Deserialize<List<Dictionary<string, JsonElement>>>(est.ViagensPorFinalidadeJson, options);

                            if (lista != null)
                            {
                                foreach (var item in lista)
                                {
                                    string finalidade = null;
                                    int totalViagens = 0;

                                    if (item.ContainsKey("finalidade"))
                                        finalidade = item["finalidade"].GetString();

                                    if (item.ContainsKey("totalViagens"))
                                        totalViagens = item["totalViagens"].GetInt32();
                                    else if (item.ContainsKey("quantidade"))
                                        totalViagens = item["quantidade"].GetInt32();

                                    if (finalidade != null && totalViagens > 0)
                                    {
                                        if (!finalidadeDict.ContainsKey(finalidade))
                                            finalidadeDict[finalidade] = 0;

                                        finalidadeDict[finalidade] += totalViagens;
                                    }
                                }
                            }
                        }
                        catch { }
                    }
                }

                var dados = finalidadeDict
                    .Select(kv => new { finalidade = kv.Key, total = kv.Value })
                    .OrderByDescending(x => x.total)
                    .Take(15)
                    .ToList();

                return Json(new { success = true, data = dados });
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message, ex, "DashboardViagensController.cs", "ObterViagensPorFinalidade");
                Alerta.TratamentoErroComLinha("DashboardViagensController.cs", "ObterViagensPorFinalidade", ex);
                return Json(new { success = false, message = ex.Message });
            }
        }

        #endregion

        #region KM por Veículo

        /// <summary>
        /// (IA) Retorna o ranking de veículos com maior quilometragem acumulada no período especificado.
        /// </summary>
        [HttpGet]
        [Route("api/DashboardViagens/ObterKmPorVeiculo")]
        public async Task<IActionResult> ObterKmPorVeiculo(DateTime? dataInicio, DateTime? dataFim, int top = 10)
        {
            try
            {
                if (!dataInicio.HasValue || !dataFim.HasValue)
                {
                    dataFim = DateTime.Now.Date.AddDays(1).AddSeconds(-1);
                    dataInicio = dataFim.Value.AddDays(-30);
                }

                var estatisticas = await _context.ViagemEstatistica
                    .AsNoTracking()
                    .Where(v => v.DataReferencia >= dataInicio.Value.Date && v.DataReferencia <= dataFim.Value.Date)
                    .ToListAsync();

                var kmDict = new Dictionary<string, decimal>();

                foreach (var est in estatisticas)
                {
                    if (!string.IsNullOrEmpty(est.KmPorVeiculoJson))
                    {
                        try
                        {
                            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                            var lista = JsonSerializer.Deserialize<List<Dictionary<string, JsonElement>>>(est.KmPorVeiculoJson, options);

                            if (lista != null)
                            {
                                foreach (var item in lista)
                                {
                                    string veiculo = null;
                                    decimal kmTotal = 0;

                                    if (item.ContainsKey("veiculo"))
                                        veiculo = item["veiculo"].GetString();
                                    else if (item.ContainsKey("placa"))
                                        veiculo = item["placa"].GetString();

                                    if (item.ContainsKey("kmTotal"))
                                        kmTotal = item["kmTotal"].GetDecimal();
                                    else if (item.ContainsKey("km"))
                                        kmTotal = item["km"].GetDecimal();

                                    if (veiculo != null && kmTotal > 0)
                                    {
                                        if (!kmDict.ContainsKey(veiculo))
                                            kmDict[veiculo] = 0;

                                        kmDict[veiculo] += kmTotal;
                                    }
                                }
                            }
                        }
                        catch { }
                    }
                }

                var dados = kmDict
                    .Select(kv => new { veiculo = kv.Key, kmTotal = Math.Round(kv.Value, 0) })
                    .OrderByDescending(x => x.kmTotal)
                    .Take(top)
                    .ToList();

                return Json(new { success = true, data = dados });
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message, ex, "DashboardViagensController.cs", "ObterKmPorVeiculo");
                Alerta.TratamentoErroComLinha("DashboardViagensController.cs", "ObterKmPorVeiculo", ex);
                return Json(new { success = false, message = ex.Message });
            }
        }

        #endregion

        #region Viagens por Requisitante

        /// <summary>
        /// <para><b>NOME:</b> <c>ObterViagensPorRequisitante</c></para>
        /// <para><b>DESCRIÇÃO:</b> Retorna a volumetria de viagens por requisitante, com tratamento especial para a CTRAN.</para>
        /// </summary>
        [HttpGet]
        [Route("api/DashboardViagens/ObterViagensPorRequisitante")]
        public async Task<IActionResult> ObterViagensPorRequisitante(DateTime? dataInicio, DateTime? dataFim, int top = 6)
        {
            try
            {
                if (!dataInicio.HasValue || !dataFim.HasValue)
                {
                    dataFim = DateTime.Now.Date.AddDays(1).AddSeconds(-1);
                    dataInicio = dataFim.Value.AddDays(-30);
                }

                var estatisticas = await _context.ViagemEstatistica
                    .AsNoTracking()
                    .Where(v => v.DataReferencia >= dataInicio.Value.Date && v.DataReferencia <= dataFim.Value.Date)
                    .ToListAsync();

                var requisitanteDict = new Dictionary<string, int>();

                foreach (var est in estatisticas)
                {
                    if (!string.IsNullOrEmpty(est.ViagensPorRequisitanteJson))
                    {
                        try
                        {
                            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                            var lista = JsonSerializer.Deserialize<List<Dictionary<string, JsonElement>>>(est.ViagensPorRequisitanteJson, options);

                            if (lista != null)
                            {
                                foreach (var item in lista)
                                {
                                    string requisitante = null;
                                    int totalViagens = 0;

                                    if (item.ContainsKey("requisitante"))
                                        requisitante = item["requisitante"].GetString();

                                    if (item.ContainsKey("totalViagens"))
                                        totalViagens = item["totalViagens"].GetInt32();
                                    else if (item.ContainsKey("quantidade"))
                                        totalViagens = item["quantidade"].GetInt32();

                                    if (requisitante != null && totalViagens > 0)
                                    {
                                        if (!requisitanteDict.ContainsKey(requisitante))
                                            requisitanteDict[requisitante] = 0;

                                        requisitanteDict[requisitante] += totalViagens;
                                    }
                                }
                            }
                        }
                        catch { }
                    }
                }

                var chaveCtran = requisitanteDict.Keys
                    .FirstOrDefault(k => k.ToUpper().Contains("COORDENAÇÃO DE TRANSPORTES") ||
                                        k.ToUpper().Contains("COORDENACAO DE TRANSPORTES") ||
                                        k.ToUpper().Contains("CTRAN"));

                int viagensCtran = 0;
                if (chaveCtran != null)
                {
                    viagensCtran = requisitanteDict[chaveCtran];
                    requisitanteDict.Remove(chaveCtran);
                }

                string PegarDoisPrimeirosNomes(string nomeCompleto)
                {
                    if (string.IsNullOrEmpty(nomeCompleto))
                        return nomeCompleto;

                    var partes = nomeCompleto.Split(new[] { '(', '-' }, StringSplitOptions.RemoveEmptyEntries);
                    var nome = partes[0].Trim();

                    var nomes = nome.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    if (nomes.Length <= 2)
                        return nome;

                    return $"{nomes[0]} {nomes[1]}";
                }

                var dados = requisitanteDict
                    .Select(kv => new { requisitante = PegarDoisPrimeirosNomes(kv.Key), totalViagens = kv.Value })
                    .OrderByDescending(x => x.totalViagens)
                    .Take(top)
                    .ToList();

                return Json(new { success = true, data = dados, viagensCtran = viagensCtran });
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message, ex, "DashboardViagensController.cs", "ObterViagensPorRequisitante");
                Alerta.TratamentoErroComLinha("DashboardViagensController.cs", "ObterViagensPorRequisitante", ex);
                return Json(new { success = false, message = ex.Message });
            }
        }

        #endregion

        #region Heatmap de Viagens

        /// <summary>
        /// (IA) Retorna uma matriz de densidade de viagens por dia da semana e hora, para visualização em Heatmap.
        /// </summary>
        [HttpGet]
        [Route("api/DashboardViagens/ObterHeatmapViagens")]
        public async Task<IActionResult> ObterHeatmapViagens(DateTime? dataInicio, DateTime? dataFim)
        {
            try
            {
                if (!dataInicio.HasValue || !dataFim.HasValue)
                {
                    dataFim = DateTime.Now.Date.AddDays(1).AddSeconds(-1);
                    dataInicio = dataFim.Value.AddDays(-30);
                }

                var viagens = await _context.Viagem
                    .AsNoTracking()
                    .Where(v => v.DataInicial >= dataInicio && v.DataInicial <= dataFim)
                    .Where(v => v.HoraInicio.HasValue)
                    .Where(v => v.Status == "Realizada" || v.Status == "Agendada" || v.Status == "Aberta")
                    .Select(v => new
                    {
                        DiaSemana = v.DataInicial.HasValue ? (int)v.DataInicial.Value.DayOfWeek : 0,
                        Hora = v.HoraInicio.HasValue ? v.HoraInicio.Value.Hour : 0
                    })
                    .ToListAsync();

                var heatmap = new int[7, 24];
                int maxValor = 0;

                foreach (var v in viagens)
                {
                    int diaIndex = v.DiaSemana == 0 ? 6 : v.DiaSemana - 1; 
                    int horaIndex = Math.Clamp(v.Hora, 0, 23);

                    heatmap[diaIndex, horaIndex]++;

                    if (heatmap[diaIndex, horaIndex] > maxValor)
                        maxValor = heatmap[diaIndex, horaIndex];
                }

                var dados = new List<object>();
                var diasNomes = new[] { "Segunda", "Terça", "Quarta", "Quinta", "Sexta", "Sábado", "Domingo" };

                for (int dia = 0; dia < 7; dia++)
                {
                    var horasArray = new int[24];
                    for (int hora = 0; hora < 24; hora++)
                    {
                        horasArray[hora] = heatmap[dia, hora];
                    }

                    dados.Add(new
                    {
                        diaSemana = diasNomes[dia],
                        diaIndex = dia,
                        horas = horasArray
                    });
                }

                return Json(new
                {
                    success = true,
                    data = dados,
                    maxValor = maxValor,
                    totalViagens = viagens.Count
                });
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message, ex, "DashboardViagensController.cs", "ObterHeatmapViagens");
                Alerta.TratamentoErroComLinha("DashboardViagensController.cs", "ObterHeatmapViagens", ex);
                return Json(new { success = false, message = ex.Message });
            }
        }

        #endregion

        #region Top 10 Veículos por KM

        /// <summary>
        /// (IA) Retorna o ranking detalhado dos 10 veículos com maior quilometragem rodada no período.
        /// </summary>
        [HttpGet]
        [Route("api/DashboardViagens/ObterTop10VeiculosPorKm")]
        public async Task<IActionResult> ObterTop10VeiculosPorKm(DateTime? dataInicio, DateTime? dataFim)
        {
            try
            {
                if (!dataInicio.HasValue || !dataFim.HasValue)
                {
                    dataFim = DateTime.Now.Date.AddDays(1).AddSeconds(-1);
                    dataInicio = dataFim.Value.AddDays(-30);
                }

                var viagens = await _context.Viagem
                    .AsNoTracking()
                    .Where(v => v.DataInicial >= dataInicio && v.DataInicial <= dataFim)
                    .Where(v => v.Status == "Realizada")
                    .Where(v => v.KmInicial.HasValue && v.KmFinal.HasValue)
                    .Where(v => v.KmFinal.Value >= v.KmInicial.Value)
                    .Where(v => (v.KmFinal.Value - v.KmInicial.Value) <= KM_MAXIMO_POR_VIAGEM)
                    .Where(v => v.VeiculoId.HasValue)
                    .Include(v => v.Veiculo)
                        .ThenInclude(vei => vei.ModeloVeiculo)
                            .ThenInclude(mod => mod.MarcaVeiculo)
                    .ToListAsync();

                var top10 = viagens
                    .GroupBy(v => new
                    {
                        v.VeiculoId,
                        Placa = v.Veiculo?.Placa ?? "N/A",
                        MarcaModelo = v.Veiculo?.ModeloVeiculo != null && v.Veiculo.ModeloVeiculo.MarcaVeiculo != null
                            ? $"{v.Veiculo.ModeloVeiculo.MarcaVeiculo.DescricaoMarca} {v.Veiculo.ModeloVeiculo.DescricaoModelo}"
                            : "N/A"
                    })
                    .Select(g => new
                    {
                        placa = g.Key.Placa,
                        marcaModelo = g.Key.MarcaModelo,
                        totalKm = g.Sum(v => (v.KmFinal ?? 0) - (v.KmInicial ?? 0)),
                        totalViagens = g.Count(),
                        mediaKmPorViagem = g.Count() > 0
                            ? Math.Round((double)g.Sum(v => (v.KmFinal ?? 0) - (v.KmInicial ?? 0)) / g.Count(), 1)
                            : 0
                    })
                    .OrderByDescending(x => x.totalKm)
                    .Take(10)
                    .ToList();

                return Json(new { success = true, data = top10 });
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message, ex, "DashboardViagensController.cs", "ObterTop10VeiculosPorKm");
                Alerta.TratamentoErroComLinha("DashboardViagensController.cs", "ObterTop10VeiculosPorKm", ex);
                return Json(new { success = false, message = ex.Message });
            }
        }

        #endregion

        #region Custo Médio por Finalidade

        /// <summary>
        /// (IA) Retorna o custo total e médio das viagens agrupado por finalidade produtiva.
        /// </summary>
        [HttpGet]
        [Route("api/DashboardViagens/ObterCustoMedioPorFinalidade")]
        public async Task<IActionResult> ObterCustoMedioPorFinalidade(DateTime? dataInicio, DateTime? dataFim, int top = 10)
        {
            try
            {
                if (!dataInicio.HasValue || !dataFim.HasValue)
                {
                    dataFim = DateTime.Now.Date.AddDays(1).AddSeconds(-1);
                    dataInicio = dataFim.Value.AddDays(-30);
                }

                var viagens = await _context.Viagem
                    .AsNoTracking()
                    .Where(v => v.DataInicial >= dataInicio && v.DataInicial <= dataFim)
                    .Where(v => v.Status == "Realizada")
                    .Where(v => !string.IsNullOrEmpty(v.Finalidade))
                    .ToListAsync();

                var custosPorFinalidade = viagens
                    .GroupBy(v => v.Finalidade ?? "Não informado")
                    .Select(g =>
                    {
                        var custoTotal = g.Sum(v =>
                            (v.CustoCombustivel ?? 0) +
                            (v.CustoLavador ?? 0) +
                            (v.CustoMotorista ?? 0));

                        return new
                        {
                            finalidade = g.Key.Length > 30 ? g.Key.Substring(0, 27) + "..." : g.Key,
                            finalidadeCompleta = g.Key,
                            totalViagens = g.Count(),
                            custoTotal = Math.Round(custoTotal, 2),
                            custoMedio = g.Count() > 0 ? Math.Round(custoTotal / g.Count(), 2) : 0
                        };
                    })
                    .Where(x => x.custoTotal > 0)
                    .OrderByDescending(x => x.custoTotal)
                    .Take(top)
                    .ToList();

                return Json(new { success = true, data = custosPorFinalidade });
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message, ex, "DashboardViagensController.cs", "ObterCustoMedioPorFinalidade");
                Alerta.TratamentoErroComLinha("DashboardViagensController.cs", "ObterCustoMedioPorFinalidade", ex);
                return Json(new { success = false, message = ex.Message });
            }
        }

        #endregion Custo Médio por Finalidade
    }
}
