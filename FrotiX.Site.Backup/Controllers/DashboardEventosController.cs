using FrotiX.Data;
using FrotiX.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
    *  #   MODULO:  DASHBOARD ANALÍTICO DE EVENTOS                                                     #
    *  #   DATA:    2026 (Modernização FrotiX 2026)                                                   #
    *  #                                                                                               #
    *  #################################################################################################
    */

    /// <summary>
    /// <para>────────────────────────────────────────────────────────────────────────────────────────────</para>
    /// <para>CLASSE: <c>DashboardEventosController</c></para>
    /// <para>DESCRIÇÃO: Controlador para Dashboard de Eventos (KPIs, Gráficos e Relatórios Logísticos).</para>
    /// <para>PADRÃO: FrotiX 2026 - (IA) Documented & Modernized </para>
    /// <para>────────────────────────────────────────────────────────────────────────────────────────────</para>
    /// </summary>
    [Authorize]
    public partial class DashboardEventosController : Controller
    {
        private readonly FrotiXDbContext _context;
        private readonly UserManager<IdentityUser> _user;
        private readonly ILogService _log;

        /// <summary>
        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: DashboardEventosController (Constructor)                           ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Inicializa o controlador de dashboard de eventos com DbContext, Identity  ║
        /// ║    e serviço de log centralizado.                                            ║
        /// ║                                                                              ║
        /// ║ 🎯 IMPORTÂNCIA PARA A SOLUÇÃO:                                              ║
        /// ║    Habilita consultas analíticas e rastreabilidade do módulo.                ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • context (FrotiXDbContext): contexto EF Core.                             ║
        /// ║    • user (UserManager<IdentityUser>): identidade/autorização.               ║
        /// ║    • log (ILogService): log centralizado.                                    ║
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
        public DashboardEventosController(FrotiXDbContext context, UserManager<IdentityUser> user, ILogService log)
        {
            try
            {
                _context = context;
                _user = user;
                _log = log;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("DashboardEventosController.cs", "DashboardEventosController", error);
            }
        }

        #region Página Principal

        /// <summary>
        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: Index                                                               ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Retorna a view principal do dashboard de eventos.                          ║
        /// ║                                                                              ║
        /// ║ 🎯 IMPORTÂNCIA PARA A SOLUÇÃO:                                              ║
        /// ║    Ponto de entrada do painel analítico de eventos.                           ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • N/A                                                                     ║
        /// ║                                                                              ║
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: View Razor do dashboard.                                 ║
        /// ║    • Consumidor: UI de Dashboard de Eventos.                                 ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📞 FUNÇÕES QUE CHAMA:                                                        ║
        /// ║    • _log.Error() / Alerta.TratamentoErroComLinha() → erros.                  ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📲 CHAMADA POR:                                                              ║
        /// ║    • GET /DashboardEventos                                                   ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 🔗 ESCOPO: EXTERNA - Dashboard                                               ║
        /// ║    • Arquivos relacionados: Pages/Eventos/DashboardEventos.cshtml             ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        /// </summary>
        [HttpGet]
        [Route("DashboardEventos")]
        public IActionResult Index()
        {
            try
            {
                return View("/Pages/Eventos/DashboardEventos.cshtml");
            }
            catch (Exception error)
            {
                _log.Error(error.Message, error, "DashboardEventosController.cs", "Index");
                Alerta.TratamentoErroComLinha("DashboardEventosController.cs", "Index", error);
                return View("Error");
            }
        }

        #endregion Página Principal

        #region Estatísticas Gerais

        /// <summary>
        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: ObterEstatisticasGerais                                             ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Retorna KPIs de eventos e comparativo com período anterior.               ║
        /// ║                                                                              ║
        /// ║ 🎯 IMPORTÂNCIA PARA A SOLUÇÃO:                                              ║
        /// ║    Base de indicadores do dashboard analítico.                               ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • dataInicio (DateTime?): início do período.                              ║
        /// ║    • dataFim (DateTime?): fim do período.                                    ║
        /// ║                                                                              ║
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com KPIs e comparativo.                             ║
        /// ║    • Consumidor: UI de Dashboard de Eventos.                                 ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📞 FUNÇÕES QUE CHAMA:                                                        ║
        /// ║    • _context.Evento (Include/Where/ToListAsync) → consultas EF Core.         ║
        /// ║    • _log.Error() / Alerta.TratamentoErroComLinha() → erros.                  ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📲 CHAMADA POR:                                                              ║
        /// ║    • GET /api/DashboardEventos/ObterEstatisticasGerais                        ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 🔗 ESCOPO: EXTERNA - Dashboard                                               ║
        /// ║    • Arquivos relacionados: Pages/Eventos/DashboardEventos.cshtml             ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        /// </summary>
        [HttpGet]
        [Route("api/DashboardEventos/ObterEstatisticasGerais")]
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

                // [DADOS] Coleta eventos com relacionamentos
                var eventos = await _context.Evento
                    .AsNoTracking()
                    .Include(e => e.SetorSolicitante)
                    .Include(e => e.Requisitante)
                    .Where(e => e.DataInicial >= dataInicio && e.DataInicial <= dataFim)
                    .ToListAsync();

                // [LOGICA] KPIs do período
                var totalEventos = eventos.Count;
                var eventosAtivos = eventos.Count(e => e.Status == "Ativo" || e.Status == "Em Andamento");
                var eventosConcluidos = eventos.Count(e => e.Status == "Concluído" || e.Status == "Finalizado");
                var eventosCancelados = eventos.Count(e => e.Status == "Cancelado");
                var eventosAgendados = eventos.Count(e => e.Status == "Agendado");

                var totalParticipantes = eventos.Sum(e => e.QtdParticipantes ?? 0);
                var mediaParticipantesPorEvento = totalEventos > 0 ? (double)totalParticipantes / totalEventos : 0;

                // [LOGICA] Período anterior para comparativo
                var diasPeriodo = (dataFim.Value - dataInicio.Value).Days;
                var dataInicioAnterior = dataInicio.Value.AddDays(-(diasPeriodo + 1));
                var dataFimAnterior = dataInicio.Value.AddSeconds(-1);

                // [DADOS] Eventos do período anterior
                var eventosAnteriores = await _context.Evento
                    .Where(e => e.DataInicial >= dataInicioAnterior && e.DataInicial <= dataFimAnterior)
                    .ToListAsync();

                var totalEventosAnteriores = eventosAnteriores.Count;
                var totalParticipantesAnteriores = eventosAnteriores.Sum(e => e.QtdParticipantes ?? 0);

                return Json(new
                {
                    success = true,
                    totalEventos,
                    eventosAtivos,
                    eventosConcluidos,
                    eventosCancelados,
                    eventosAgendados,
                    totalParticipantes,
                    mediaParticipantesPorEvento = Math.Round(mediaParticipantesPorEvento, 1),
                    periodoAnterior = new
                    {
                        totalEventos = totalEventosAnteriores,
                        totalParticipantes = totalParticipantesAnteriores
                    }
                });
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message, ex, "DashboardEventosController.cs", "ObterEstatisticasGerais");
                Alerta.TratamentoErroComLinha("DashboardEventosController.cs", "ObterEstatisticasGerais", ex);
                return Json(new { success = false, message = ex.Message });
            }
        }

        #endregion Estatísticas Gerais

        #region Eventos por Status

        /// <summary>
        /// (IA) Retorna a volumetria de eventos agrupada por status para composição de gráficos.
        /// </summary>
        [HttpGet]
        [Route("api/DashboardEventos/ObterEventosPorStatus")]
        public async Task<IActionResult> ObterEventosPorStatus(DateTime? dataInicio, DateTime? dataFim)
        {
            try
            {
                if (!dataInicio.HasValue || !dataFim.HasValue)
                {
                    dataFim = DateTime.Now.Date.AddDays(1).AddSeconds(-1);
                    dataInicio = dataFim.Value.AddDays(-30);
                }

                var eventos = await _context.Evento
                    .AsNoTracking()
                    .Where(e => e.DataInicial >= dataInicio && e.DataInicial <= dataFim)
                    .GroupBy(e => e.Status)
                    .Select(g => new
                    {
                        status = g.Key ?? "Sem Status",
                        quantidade = g.Count(),
                        participantes = g.Sum(e => e.QtdParticipantes ?? 0)
                    })
                    .OrderByDescending(x => x.quantidade)
                    .ToListAsync();

                return Json(new { success = true, dados = eventos });
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message, ex, "DashboardEventosController.cs", "ObterEventosPorStatus");
                Alerta.TratamentoErroComLinha("DashboardEventosController.cs", "ObterEventosPorStatus", ex);
                return Json(new { success = false, message = ex.Message });
            }
        }

        #endregion

        #region Eventos por Setor

        /// <summary>
        /// (IA) Retorna o ranking de setores com maior demanda de eventos, incluindo taxa de conclusão.
        /// </summary>
        [HttpGet]
        [Route("api/DashboardEventos/ObterEventosPorSetor")]
        public async Task<IActionResult> ObterEventosPorSetor(DateTime? dataInicio, DateTime? dataFim)
        {
            try
            {
                if (!dataInicio.HasValue || !dataFim.HasValue)
                {
                    dataFim = DateTime.Now.Date.AddDays(1).AddSeconds(-1);
                    dataInicio = dataFim.Value.AddDays(-30);
                }

                var eventos = await _context.Evento
                    .AsNoTracking()
                    .Include(e => e.SetorSolicitante)
                    .Where(e => e.DataInicial >= dataInicio && e.DataInicial <= dataFim)
                    .ToListAsync();

                var eventosPorSetor = eventos
                    .GroupBy(e => e.SetorSolicitante != null ? e.SetorSolicitante.Nome : "Sem Setor")
                    .Select(g => new
                    {
                        setor = g.Key,
                        quantidade = g.Count(),
                        participantes = g.Sum(e => e.QtdParticipantes ?? 0),
                        concluidos = g.Count(e => e.Status == "Concluído" || e.Status == "Finalizado"),
                        taxaConclusao = g.Count() > 0 ? Math.Round((double)g.Count(e => e.Status == "Concluído" || e.Status == "Finalizado") / g.Count() * 100, 1) : 0
                    })
                    .OrderByDescending(x => x.quantidade)
                    .Take(10)
                    .ToList();

                return Json(new { success = true, dados = eventosPorSetor });
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message, ex, "DashboardEventosController.cs", "ObterEventosPorSetor");
                Alerta.TratamentoErroComLinha("DashboardEventosController.cs", "ObterEventosPorSetor", ex);
                return Json(new { success = false, message = ex.Message });
            }
        }

        #endregion

        #region Eventos por Requisitante

        /// <summary>
        /// (IA) Retorna o ranking de requisitantes mais ativos e o impacto em número de participantes.
        /// </summary>
        [HttpGet]
        [Route("api/DashboardEventos/ObterEventosPorRequisitante")]
        public async Task<IActionResult> ObterEventosPorRequisitante(DateTime? dataInicio, DateTime? dataFim)
        {
            try
            {
                if (!dataInicio.HasValue || !dataFim.HasValue)
                {
                    dataFim = DateTime.Now.Date.AddDays(1).AddSeconds(-1);
                    dataInicio = dataFim.Value.AddDays(-30);
                }

                var eventos = await _context.Evento
                    .AsNoTracking()
                    .Include(e => e.Requisitante)
                    .Where(e => e.DataInicial >= dataInicio && e.DataInicial <= dataFim)
                    .ToListAsync();

                var eventosPorRequisitante = eventos
                    .GroupBy(e => e.Requisitante != null ? e.Requisitante.Nome : "Sem Requisitante")
                    .Select(g => new
                    {
                        requisitante = g.Key,
                        quantidade = g.Count(),
                        participantes = g.Sum(e => e.QtdParticipantes ?? 0)
                    })
                    .OrderByDescending(x => x.quantidade)
                    .Take(10)
                    .ToList();

                return Json(new { success = true, dados = eventosPorRequisitante });
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message, ex, "DashboardEventosController.cs", "ObterEventosPorRequisitante");
                Alerta.TratamentoErroComLinha("DashboardEventosController.cs", "ObterEventosPorRequisitante", ex);
                return Json(new { success = false, message = ex.Message });
            }
        }

        #endregion

        #region Eventos por Mês

        /// <summary>
        /// (IA) Retorna a evolução temporal da realização de eventos e participação acumulada.
        /// </summary>
        [HttpGet]
        [Route("api/DashboardEventos/ObterEventosPorMes")]
        public async Task<IActionResult> ObterEventosPorMes(DateTime? dataInicio, DateTime? dataFim)
        {
            try
            {
                if (!dataInicio.HasValue || !dataFim.HasValue)
                {
                    dataFim = DateTime.Now.Date.AddDays(1).AddSeconds(-1);
                    dataInicio = dataFim.Value.AddMonths(-12);
                }

                var eventos = await _context.Evento
                    .AsNoTracking()
                    .Where(e => e.DataInicial >= dataInicio && e.DataInicial <= dataFim)
                    .ToListAsync();

                var eventosPorMes = eventos
                    .Where(e => e.DataInicial.HasValue)
                    .GroupBy(e => new { Ano = e.DataInicial.Value.Year, Mes = e.DataInicial.Value.Month })
                    .Select(g => new
                    {
                        mes = $"{g.Key.Ano}-{g.Key.Mes:D2}",
                        mesNome = new DateTime(g.Key.Ano, g.Key.Mes, 1).ToString("MM/yyyy"),
                        quantidade = g.Count(),
                        participantes = g.Sum(e => e.QtdParticipantes ?? 0)
                    })
                    .OrderBy(x => x.mes)
                    .ToList();

                return Json(new { success = true, dados = eventosPorMes });
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message, ex, "DashboardEventosController.cs", "ObterEventosPorMes");
                Alerta.TratamentoErroComLinha("DashboardEventosController.cs", "ObterEventosPorMes", ex);
                return Json(new { success = false, message = ex.Message });
            }
        }

        #endregion

        #region Top 10 Eventos

        /// <summary>
        /// (IA) Retorna os 10 eventos de maior magnitude (participantes) no período selecionado.
        /// </summary>
        [HttpGet]
        [Route("api/DashboardEventos/ObterTop10EventosMaiores")]
        public async Task<IActionResult> ObterTop10EventosMaiores(DateTime? dataInicio, DateTime? dataFim)
        {
            try
            {
                if (!dataInicio.HasValue || !dataFim.HasValue)
                {
                    dataFim = DateTime.Now.Date.AddDays(1).AddSeconds(-1);
                    dataInicio = dataFim.Value.AddDays(-30);
                }

                var eventos = await _context.Evento
                    .AsNoTracking()
                    .Include(e => e.SetorSolicitante)
                    .Include(e => e.Requisitante)
                    .Where(e => e.DataInicial >= dataInicio && e.DataInicial <= dataFim)
                    .OrderByDescending(e => e.QtdParticipantes)
                    .Take(10)
                    .Select(e => new
                    {
                        e.EventoId,
                        e.Nome,
                        DataInicial = e.DataInicial.HasValue ? e.DataInicial.Value.ToString("dd/MM/yyyy HH:mm") : "Não definido",
                        DataFinal = e.DataFinal.HasValue ? e.DataFinal.Value.ToString("dd/MM/yyyy HH:mm") : "Não definido",
                        participantes = e.QtdParticipantes ?? 0,
                        setor = e.SetorSolicitante != null ? e.SetorSolicitante.Nome : "Sem Setor",
                        requisitante = e.Requisitante != null ? e.Requisitante.Nome : "Sem Requisitante",
                        e.Status
                    })
                    .ToListAsync();

                return Json(new { success = true, dados = eventos });
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message, ex, "DashboardEventosController.cs", "ObterTop10EventosMaiores");
                Alerta.TratamentoErroComLinha("DashboardEventosController.cs", "ObterTop10EventosMaiores", ex);
                return Json(new { success = false, message = ex.Message });
            }
        }

        #endregion

        #region Estatísticas por Dia

        /// <summary>
        /// (IA) Retorna a distribuição diária de eventos para análise de fluxo e picos de ocupação.
        /// </summary>
        [HttpGet]
        [Route("api/DashboardEventos/ObterEventosPorDia")]
        public async Task<IActionResult> ObterEventosPorDia(DateTime? dataInicio, DateTime? dataFim)
        {
            try
            {
                if (!dataInicio.HasValue || !dataFim.HasValue)
                {
                    dataFim = DateTime.Now.Date.AddDays(1).AddSeconds(-1);
                    dataInicio = dataFim.Value.AddDays(-30);
                }

                var eventos = await _context.Evento
                    .AsNoTracking()
                    .Where(e => e.DataInicial >= dataInicio && e.DataInicial <= dataFim)
                    .ToListAsync();

                var eventosPorDia = eventos
                    .GroupBy(e => e.DataInicial)
                    .Select(g => new
                    {
                        data = g.Key.HasValue ? g.Key.Value.Date.ToString("dd/MM/yyyy") : "",
                        quantidade = g.Count(),
                        participantes = g.Sum(e => e.QtdParticipantes ?? 0)
                    })
                    .OrderBy(x => x.data)
                    .ToList();

                return Json(new { success = true, dados = eventosPorDia });
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message, ex, "DashboardEventosController.cs", "ObterEventosPorDia");
                Alerta.TratamentoErroComLinha("DashboardEventosController.cs", "ObterEventosPorDia", ex);
                return Json(new { success = false, message = ex.Message });
            }
        }

        #endregion
    }
}
