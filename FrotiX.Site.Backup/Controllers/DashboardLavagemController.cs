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
    *  #   MODULO:  DASHBOARD ANALÍTICO DE HIGIENE E LAVAGEM                                           #
    *  #   DATA:    2026 (Modernização FrotiX 2026)                                                   #
    *  #                                                                                               #
    *  #################################################################################################
    */

    /// <summary>
    /// <para>────────────────────────────────────────────────────────────────────────────────────────────</para>
    /// <para>CLASSE: <c>DashboardLavagemController</c></para>
    /// <para>DESCRIÇÃO: Interface analítica para operações de lavagem, performance de equipe e insumos.</para>
    /// <para>PADRÃO: FrotiX 2026 - (IA) Documented & Modernized </para>
    /// <para>────────────────────────────────────────────────────────────────────────────────────────────</para>
    /// </summary>
    [Authorize]
    public class DashboardLavagemController : Controller
    {
        private readonly FrotiXDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogService _log;

        /// <summary>
        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: DashboardLavagemController (Constructor)                           ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Inicializa o dashboard de lavagem com DbContext, Identity e log.          ║
        /// ║                                                                              ║
        /// ║ 🎯 IMPORTÂNCIA PARA A SOLUÇÃO:                                              ║
        /// ║    Habilita análise operacional de higiene e produtividade.                  ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • context (FrotiXDbContext): contexto EF Core.                             ║
        /// ║    • userManager (UserManager<IdentityUser>): identidade.                    ║
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
        public DashboardLavagemController(FrotiXDbContext context, UserManager<IdentityUser> userManager, ILogService log)
        {
            try
            {
                _context = context;
                _userManager = userManager;
                _log = log;
            }
            catch (Exception ex)
            {
                Alerta.TratamentoErroComLinha("DashboardLavagemController.cs", "DashboardLavagemController", ex);
            }
        }

        #region Estatísticas Gerais

        /// <summary>
        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: EstatisticasGerais                                                 ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Obtém KPIs consolidados de lavagem para o período informado.              ║
        /// ║                                                                              ║
        /// ║ 🎯 IMPORTÂNCIA PARA A SOLUÇÃO:                                              ║
        /// ║    Base analítica do dashboard de higiene veicular.                          ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • dataInicio (DateTime?): início do filtro.                               ║
        /// ║    • dataFim (DateTime?): fim do filtro.                                     ║
        /// ║                                                                              ║
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com indicadores e destaques.                        ║
        /// ║    • Consumidor: UI de Dashboard de Lavagem.                                 ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📞 FUNÇÕES QUE CHAMA:                                                        ║
        /// ║    • _context.Lavagem/LavadoresLavagem → consultas EF Core.                   ║
        /// ║    • _log.Error() / Alerta.TratamentoErroComLinha() → erros.                  ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📲 CHAMADA POR:                                                              ║
        /// ║    • GET /api/DashboardLavagem/EstatisticasGerais                             ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 🔗 ESCOPO: EXTERNA - Dashboard                                               ║
        /// ║    • Arquivos relacionados: Pages/Lavagem/DashboardLavagem.cshtml             ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        /// </summary>
        [HttpGet]
        [Route("api/DashboardLavagem/EstatisticasGerais")]
        public async Task<IActionResult> EstatisticasGerais(DateTime? dataInicio, DateTime? dataFim)
        {
            try
            {
                // [REGRA] Intervalo padrão quando datas não informadas
                if (!dataInicio.HasValue || !dataFim.HasValue)
                {
                    dataFim = DateTime.Now.Date.AddDays(1).AddSeconds(-1);
                    dataInicio = dataFim.Value.AddDays(-30);
                }

                // [DADOS] Busca lavagens persistidas no período
                var lavagens = await _context.Lavagem
                    .Include(l => l.Veiculo)
                    .Include(l => l.Motorista)
                    .Where(l => l.Data >= dataInicio && l.Data <= dataFim)
                    .ToListAsync();

                // [DADOS] Mapeia lavadores envolvidos no período
                var lavadoresLavagem = await _context.LavadoresLavagem
                    .Include(ll => ll.Lavador)
                    .ThenInclude(lav => lav.Contrato)
                    .Where(ll => lavagens.Select(l => l.LavagemId).Contains(ll.LavagemId))
                    .ToListAsync();

                // [LOGICA] KPIs básicos
                var totalLavagens = lavagens.Count;
                var veiculosLavados = lavagens.Select(l => l.VeiculoId).Distinct().Count();
                var lavadoresAtivos = lavadoresLavagem.Select(ll => ll.LavadorId).Distinct().Count();

                var diasPeriodo = (dataFim.Value - dataInicio.Value).Days;
                diasPeriodo = diasPeriodo > 0 ? diasPeriodo : 1;

                var mediaDiaria = Math.Round((double)totalLavagens / diasPeriodo, 1);
                var mediaPorVeiculo = veiculosLavados > 0 ? Math.Round((double)totalLavagens / veiculosLavados, 1) : 0;

                // [LOGICA] Lavador com maior volume
                var lavadorDestaque = lavadoresLavagem
                    .GroupBy(ll => new { ll.LavadorId, ll.Lavador?.Nome })
                    .Select(g => new { Nome = g.Key.Nome ?? "N/A", Quantidade = g.Count() })
                    .OrderByDescending(x => x.Quantidade)
                    .FirstOrDefault();

                // [LOGICA] Veículo mais lavado
                var veiculoMaisLavado = lavagens
                    .GroupBy(l => new {
                        l.VeiculoId,
                        Placa = l.Veiculo?.Placa,
                        IsPM = l.Veiculo?.PlacaBronzeId != null
                    })
                    .Select(g => new {
                        Placa = g.Key.IsPM ? $"{g.Key.Placa} (PM)" : g.Key.Placa ?? "N/A",
                        Quantidade = g.Count()
                    })
                    .OrderByDescending(x => x.Quantidade)
                    .FirstOrDefault();

                var diasSemana = new[] { "Domingo", "Segunda", "Terça", "Quarta", "Quinta", "Sexta", "Sábado" };
                var diaMaisMovimentado = lavagens
                    .Where(l => l.Data.HasValue)
                    .GroupBy(l => l.Data.Value.DayOfWeek)
                    .Select(g => new { Dia = diasSemana[(int)g.Key], Quantidade = g.Count() })
                    .OrderByDescending(x => x.Quantidade)
                    .FirstOrDefault();

                // [LOGICA] Horário de maior fluxo
                var horarioPico = lavagens
                    .Where(l => l.Data.HasValue)
                    .GroupBy(l => l.Data.Value.Hour)
                    .Select(g => new { Hora = $"{g.Key:D2}:00", Quantidade = g.Count() })
                    .OrderByDescending(x => x.Quantidade)
                    .FirstOrDefault();

                var dataInicioAnterior = dataInicio.Value.AddDays(-(diasPeriodo + 1));
                var dataFimAnterior = dataInicio.Value.AddSeconds(-1);

                var lavagensAnteriores = await _context.Lavagem
                    .Where(l => l.Data >= dataInicioAnterior && l.Data <= dataFimAnterior)
                    .CountAsync();

                return Json(new
                {
                    success = true,
                    totalLavagens,
                    veiculosLavados,
                    lavadoresAtivos,
                    mediaDiaria,
                    mediaPorVeiculo,
                    lavadorDestaque = lavadorDestaque ?? new { Nome = "N/A", Quantidade = 0 },
                    veiculoMaisLavado = veiculoMaisLavado ?? new { Placa = "N/A", Quantidade = 0 },
                    diaMaisMovimentado = diaMaisMovimentado?.Dia ?? "N/A",
                    horarioPico = horarioPico?.Hora ?? "N/A",
                    periodoAnterior = new
                    {
                        totalLavagens = lavagensAnteriores
                    }
                });
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message, ex, "DashboardLavagemController.cs", "EstatisticasGerais");
                Alerta.TratamentoErroComLinha("DashboardLavagemController.cs", "EstatisticasGerais", ex);
                return Json(new { success = false, message = ex.Message });
            }
        }

        #endregion

        #region Gráficos

        /// <summary>
        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: LavagensPorDiaSemana                                               ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Retorna volumetria de lavagens distribuída por dias da semana.            ║
        /// ║                                                                              ║
        /// ║ 🎯 IMPORTÂNCIA PARA A SOLUÇÃO:                                              ║
        /// ║    Suporta gráficos de sazonalidade semanal.                                 ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • dataInicio (DateTime?): início do filtro.                               ║
        /// ║    • dataFim (DateTime?): fim do filtro.                                     ║
        /// ║                                                                              ║
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com série por dia da semana.                         ║
        /// ║    • Consumidor: UI de Dashboard de Lavagem.                                 ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📞 FUNÇÕES QUE CHAMA:                                                        ║
        /// ║    • _context.Lavagem → consultas EF Core.                                   ║
        /// ║    • _log.Error() / Alerta.TratamentoErroComLinha() → erros.                  ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📲 CHAMADA POR:                                                              ║
        /// ║    • GET /api/DashboardLavagem/LavagensPorDiaSemana                           ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 🔗 ESCOPO: EXTERNA - Dashboard                                               ║
        /// ║    • Arquivos relacionados: Pages/Lavagem/DashboardLavagem.cshtml             ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        /// </summary>
        [HttpGet]
        [Route("api/DashboardLavagem/LavagensPorDiaSemana")]
        public async Task<IActionResult> LavagensPorDiaSemana(DateTime? dataInicio, DateTime? dataFim)
        {
            try
            {
                // [REGRA] Intervalo padrão quando datas não informadas
                if (!dataInicio.HasValue || !dataFim.HasValue)
                {
                    dataFim = DateTime.Now.Date.AddDays(1).AddSeconds(-1);
                    dataInicio = dataFim.Value.AddDays(-30);
                }

                // [DADOS] Carrega lavagens do período
                var lavagens = await _context.Lavagem
                    .Where(l => l.Data >= dataInicio && l.Data <= dataFim && l.Data.HasValue)
                    .ToListAsync();

                // [LOGICA] Série por dia da semana
                var diasSemana = new[] { "Dom", "Seg", "Ter", "Qua", "Qui", "Sex", "Sáb" };
                var resultado = Enumerable.Range(0, 7)
                    .Select(i => new
                    {
                        dia = diasSemana[i],
                        quantidade = lavagens.Count(l => (int)l.Data.Value.DayOfWeek == i)
                    })
                    .ToList();

                return Json(new { success = true, data = resultado });
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message, ex, "DashboardLavagemController.cs", "LavagensPorDiaSemana");
                Alerta.TratamentoErroComLinha("DashboardLavagemController.cs", "LavagensPorDiaSemana", ex);
                return Json(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: LavagensPorHorario                                                 ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Retorna volumetria de lavagens distribuída por faixas horárias.           ║
        /// ║                                                                              ║
        /// ║ 🎯 IMPORTÂNCIA PARA A SOLUÇÃO:                                              ║
        /// ║    Suporta análise de pico operacional por hora.                             ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • dataInicio (DateTime?): início do filtro.                               ║
        /// ║    • dataFim (DateTime?): fim do filtro.                                     ║
        /// ║                                                                              ║
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com série por hora.                                 ║
        /// ║    • Consumidor: UI de Dashboard de Lavagem.                                 ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📞 FUNÇÕES QUE CHAMA:                                                        ║
        /// ║    • _context.Lavagem → consultas EF Core.                                   ║
        /// ║    • _log.Error() / Alerta.TratamentoErroComLinha() → erros.                  ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📲 CHAMADA POR:                                                              ║
        /// ║    • GET /api/DashboardLavagem/LavagensPorHorario                             ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 🔗 ESCOPO: EXTERNA - Dashboard                                               ║
        /// ║    • Arquivos relacionados: Pages/Lavagem/DashboardLavagem.cshtml             ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        /// </summary>
        [HttpGet]
        [Route("api/DashboardLavagem/LavagensPorHorario")]
        public async Task<IActionResult> LavagensPorHorario(DateTime? dataInicio, DateTime? dataFim)
        {
            try
            {
                // [REGRA] Intervalo padrão quando datas não informadas
                if (!dataInicio.HasValue || !dataFim.HasValue)
                {
                    dataFim = DateTime.Now.Date.AddDays(1).AddSeconds(-1);
                    dataInicio = dataFim.Value.AddDays(-30);
                }

                // [DADOS] Carrega lavagens com horário
                var lavagens = await _context.Lavagem
                    .Where(l => l.Data >= dataInicio && l.Data <= dataFim && l.Data.HasValue)
                    .ToListAsync();

                // [LOGICA] Série por hora
                var resultado = Enumerable.Range(0, 24)
                    .Select(h => new
                    {
                        hora = $"{h:D2}:00",
                        quantidade = lavagens.Count(l => l.Data.Value.Hour == h)
                    })
                    .ToList();

                return Json(new { success = true, data = resultado });
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message, ex, "DashboardLavagemController.cs", "LavagensPorHorario");
                Alerta.TratamentoErroComLinha("DashboardLavagemController.cs", "LavagensPorHorario", ex);
                return Json(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// (IA) Fornece a linha do tempo mensal de lavagens para análise de tendência.
        /// </summary>
        /// <param name="meses">Profundidade histórica em meses.</param>
        [HttpGet]
        [Route("api/DashboardLavagem/EvolucaoMensal")]
        public async Task<IActionResult> EvolucaoMensal(int meses = 12)
        {
            try
            {
                var dataInicio = DateTime.Now.AddMonths(-meses).Date;
                dataInicio = new DateTime(dataInicio.Year, dataInicio.Month, 1);

                var lavagens = await _context.Lavagem
                    .Where(l => l.Data >= dataInicio && l.Data.HasValue)
                    .ToListAsync();

                var resultado = lavagens
                    .GroupBy(l => new { l.Data.Value.Year, l.Data.Value.Month })
                    .Select(g => new
                    {
                        mes = $"{g.Key.Month:D2}/{g.Key.Year}",
                        ano = g.Key.Year,
                        mesNum = g.Key.Month,
                        quantidade = g.Count()
                    })
                    .OrderBy(x => x.ano)
                    .ThenBy(x => x.mesNum)
                    .ToList();

                return Json(new { success = true, data = resultado });
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message, ex, "DashboardLavagemController.cs", "EvolucaoMensal");
                Alerta.TratamentoErroComLinha("DashboardLavagemController.cs", "EvolucaoMensal", ex);
                return Json(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// (IA) Ranking de produtividade dos lavadores.
        /// </summary>
        [HttpGet]
        [Route("api/DashboardLavagem/TopLavadores")]
        public async Task<IActionResult> TopLavadores(DateTime? dataInicio, DateTime? dataFim, int top = 10)
        {
            try
            {
                if (!dataInicio.HasValue || !dataFim.HasValue)
                {
                    dataFim = DateTime.Now.Date.AddDays(1).AddSeconds(-1);
                    dataInicio = dataFim.Value.AddDays(-30);
                }

                var lavagensIds = await _context.Lavagem
                    .Where(l => l.Data >= dataInicio && l.Data <= dataFim)
                    .Select(l => l.LavagemId)
                    .ToListAsync();

                var lavadoresLavagem = await _context.LavadoresLavagem
                    .Include(ll => ll.Lavador)
                    .Where(ll => lavagensIds.Contains(ll.LavagemId))
                    .ToListAsync();

                var resultado = lavadoresLavagem
                    .GroupBy(ll => new { ll.LavadorId, ll.Lavador?.Nome })
                    .Select(g => new
                    {
                        nome = g.Key.Nome ?? "N/A",
                        quantidade = g.Count()
                    })
                    .OrderByDescending(x => x.quantidade)
                    .Take(top)
                    .ToList();

                return Json(new { success = true, data = resultado });
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message, ex, "DashboardLavagemController.cs", "TopLavadores");
                Alerta.TratamentoErroComLinha("DashboardLavagemController.cs", "TopLavadores", ex);
                return Json(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// (IA) Ranking dos veículos que mais geram demanda de lavagem.
        /// </summary>
        [HttpGet]
        [Route("api/DashboardLavagem/TopVeiculos")]
        public async Task<IActionResult> TopVeiculos(DateTime? dataInicio, DateTime? dataFim, int top = 10)
        {
            try
            {
                if (!dataInicio.HasValue || !dataFim.HasValue)
                {
                    dataFim = DateTime.Now.Date.AddDays(1).AddSeconds(-1);
                    dataInicio = dataFim.Value.AddDays(-30);
                }

                var lavagens = await _context.Lavagem
                    .Include(l => l.Veiculo)
                    .ThenInclude(v => v.ModeloVeiculo)
                    .Where(l => l.Data >= dataInicio && l.Data <= dataFim)
                    .ToListAsync();

                var resultado = lavagens
                    .GroupBy(l => new {
                        l.VeiculoId,
                        l.Veiculo?.Placa,
                        Modelo = l.Veiculo?.ModeloVeiculo?.DescricaoModelo,
                        IsPM = l.Veiculo?.PlacaBronzeId != null
                    })
                    .Select(g => new
                    {
                        placa = g.Key.IsPM ? $"{g.Key.Placa} (PM)" : g.Key.Placa ?? "N/A",
                        modelo = g.Key.Modelo ?? "",
                        descricao = g.Key.IsPM
                            ? $"({g.Key.Placa}) PM - {g.Key.Modelo ?? ""}"
                            : $"({g.Key.Placa}) - {g.Key.Modelo ?? ""}",
                        quantidade = g.Count()
                    })
                    .OrderByDescending(x => x.quantidade)
                    .Take(top)
                    .ToList();

                return Json(new { success = true, data = resultado });
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message, ex, "DashboardLavagemController.cs", "TopVeiculos");
                Alerta.TratamentoErroComLinha("DashboardLavagemController.cs", "TopVeiculos", ex);
                return Json(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// (IA) Ranking de motoristas solicitantes de lavagem.
        /// </summary>
        [HttpGet]
        [Route("api/DashboardLavagem/TopMotoristas")]
        public async Task<IActionResult> TopMotoristas(DateTime? dataInicio, DateTime? dataFim, int top = 10)
        {
            try
            {
                if (!dataInicio.HasValue || !dataFim.HasValue)
                {
                    dataFim = DateTime.Now.Date.AddDays(1).AddSeconds(-1);
                    dataInicio = dataFim.Value.AddDays(-30);
                }

                var lavagens = await _context.Lavagem
                    .Include(l => l.Motorista)
                    .Where(l => l.Data >= dataInicio && l.Data <= dataFim)
                    .ToListAsync();

                var resultado = lavagens
                    .GroupBy(l => new { l.MotoristaId, l.Motorista?.Nome })
                    .Select(g => new
                    {
                        nome = g.Key.Nome ?? "N/A",
                        quantidade = g.Count()
                    })
                    .OrderByDescending(x => x.quantidade)
                    .Take(top)
                    .ToList();

                return Json(new { success = true, data = resultado });
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message, ex, "DashboardLavagemController.cs", "TopMotoristas");
                Alerta.TratamentoErroComLinha("DashboardLavagemController.cs", "TopMotoristas", ex);
                return Json(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// (IA) Gera a matriz do Heatmap cruzando dia da semana e hora do dia.
        /// </summary>
        [HttpGet]
        [Route("api/DashboardLavagem/HeatmapDiaHora")]
        public async Task<IActionResult> HeatmapDiaHora(DateTime? dataInicio, DateTime? dataFim)
        {
            try
            {
                if (!dataInicio.HasValue || !dataFim.HasValue)
                {
                    dataFim = DateTime.Now.Date.AddDays(1).AddSeconds(-1);
                    dataInicio = dataFim.Value.AddDays(-30);
                }

                var lavagens = await _context.Lavagem
                    .Where(l => l.Data >= dataInicio && l.Data <= dataFim && l.Data.HasValue)
                    .ToListAsync();

                var resultado = lavagens
                    .GroupBy(l => new { Dia = (int)l.Data.Value.DayOfWeek, Hora = l.Data.Value.Hour })
                    .Select(g => new
                    {
                        dia = g.Key.Dia,
                        hora = g.Key.Hora,
                        quantidade = g.Count()
                    })
                    .ToList();

                return Json(new { success = true, data = resultado });
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message, ex, "DashboardLavagemController.cs", "HeatmapDiaHora");
                Alerta.TratamentoErroComLinha("DashboardLavagemController.cs", "HeatmapDiaHora", ex);
                return Json(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// (IA) Distribuição de lavagens por contrato/fornecedor prestador de serviço.
        /// </summary>
        [HttpGet]
        [Route("api/DashboardLavagem/LavagensPorContrato")]
        public async Task<IActionResult> LavagensPorContrato(DateTime? dataInicio, DateTime? dataFim)
        {
            try
            {
                if (!dataInicio.HasValue || !dataFim.HasValue)
                {
                    dataFim = DateTime.Now.Date.AddDays(1).AddSeconds(-1);
                    dataInicio = dataFim.Value.AddDays(-30);
                }

                var lavagensIds = await _context.Lavagem
                    .Where(l => l.Data >= dataInicio && l.Data <= dataFim)
                    .Select(l => l.LavagemId)
                    .ToListAsync();

                var lavadoresLavagem = await _context.LavadoresLavagem
                    .Include(ll => ll.Lavador)
                    .ThenInclude(lav => lav.Contrato)
                    .ThenInclude(c => c.Fornecedor)
                    .Where(ll => lavagensIds.Contains(ll.LavagemId))
                    .ToListAsync();

                var resultado = lavadoresLavagem
                    .GroupBy(ll => ll.Lavador?.Contrato?.Fornecedor?.DescricaoFornecedor ?? "Sem Contrato")
                    .Select(g => new
                    {
                        contrato = g.Key,
                        quantidade = g.Select(x => x.LavagemId).Distinct().Count()
                    })
                    .OrderByDescending(x => x.quantidade)
                    .ToList();

                return Json(new { success = true, data = resultado });
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message, ex, "DashboardLavagemController.cs", "LavagensPorContrato");
                Alerta.TratamentoErroComLinha("DashboardLavagemController.cs", "LavagensPorContrato", ex);
                return Json(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// (IA) Agrupa o volume de lavagens pelas categorias de veículos (Passeio, Frota Própria, Utilitários).
        /// </summary>
        [HttpGet]
        [Route("api/DashboardLavagem/LavagensPorCategoria")]
        public async Task<IActionResult> LavagensPorCategoria(DateTime? dataInicio, DateTime? dataFim)
        {
            try
            {
                if (!dataInicio.HasValue || !dataFim.HasValue)
                {
                    dataFim = DateTime.Now.Date.AddDays(1).AddSeconds(-1);
                    dataInicio = dataFim.Value.AddDays(-30);
                }

                var lavagens = await _context.Lavagem
                    .Include(l => l.Veiculo)
                    .Where(l => l.Data >= dataInicio && l.Data <= dataFim)
                    .ToListAsync();

                var resultado = lavagens
                    .GroupBy(l =>
                    {
                        if (l.Veiculo?.PlacaBronzeId != null)
                            return "PM";
                        return l.Veiculo?.Categoria ?? "Não Informado";
                    })
                    .Select(g => new
                    {
                        categoria = g.Key,
                        quantidade = g.Count()
                    })
                    .OrderByDescending(x => x.quantidade)
                    .ToList();

                return Json(new { success = true, data = resultado });
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message, ex, "DashboardLavagemController.cs", "LavagensPorCategoria");
                Alerta.TratamentoErroComLinha("DashboardLavagemController.cs", "LavagensPorCategoria", ex);
                return Json(new { success = false, message = ex.Message });
            }
        }

        #endregion

        #region Tabelas

        /// <summary>
        /// (IA) Fornece dados tabulares detalhados sobre a performance individual de cada lavador.
        /// </summary>
        [HttpGet]
        [Route("api/DashboardLavagem/EstatisticasPorLavador")]
        public async Task<IActionResult> EstatisticasPorLavador(DateTime? dataInicio, DateTime? dataFim)
        {
            try
            {
                if (!dataInicio.HasValue || !dataFim.HasValue)
                {
                    dataFim = DateTime.Now.Date.AddDays(1).AddSeconds(-1);
                    dataInicio = dataFim.Value.AddDays(-30);
                }

                var diasPeriodo = (dataFim.Value - dataInicio.Value).Days;
                diasPeriodo = diasPeriodo > 0 ? diasPeriodo : 1;

                var lavagensIds = await _context.Lavagem
                    .Where(l => l.Data >= dataInicio && l.Data <= dataFim)
                    .Select(l => l.LavagemId)
                    .ToListAsync();

                var totalLavagens = lavagensIds.Count;

                var lavadoresLavagem = await _context.LavadoresLavagem
                    .Include(ll => ll.Lavador)
                    .Where(ll => lavagensIds.Contains(ll.LavagemId))
                    .ToListAsync();

                var resultado = lavadoresLavagem
                    .GroupBy(ll => new { ll.LavadorId, ll.Lavador?.Nome })
                    .Select(g => new
                    {
                        nome = g.Key.Nome ?? "N/A",
                        lavagens = g.Count(),
                        percentual = totalLavagens > 0 ? Math.Round((double)g.Count() / totalLavagens * 100, 1) : 0,
                        mediaDia = Math.Round((double)g.Count() / diasPeriodo, 2)
                    })
                    .OrderByDescending(x => x.lavagens)
                    .ToList();

                return Json(new { success = true, data = resultado });
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message, ex, "DashboardLavagemController.cs", "EstatisticasPorLavador");
                Alerta.TratamentoErroComLinha("DashboardLavagemController.cs", "EstatisticasPorLavador", ex);
                return Json(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// (IA) Fornece dados tabulares detalhados sobre o histórico de limpeza de cada veículo.
        /// </summary>
        [HttpGet]
        [Route("api/DashboardLavagem/EstatisticasPorVeiculo")]
        public async Task<IActionResult> EstatisticasPorVeiculo(DateTime? dataInicio, DateTime? dataFim)
        {
            try
            {
                if (!dataInicio.HasValue || !dataFim.HasValue)
                {
                    dataFim = DateTime.Now.Date.AddDays(1).AddSeconds(-1);
                    dataInicio = dataFim.Value.AddDays(-30);
                }

                var lavagens = await _context.Lavagem
                    .Include(l => l.Veiculo)
                    .ThenInclude(v => v.ModeloVeiculo)
                    .Where(l => l.Data >= dataInicio && l.Data <= dataFim)
                    .ToListAsync();

                var hoje = DateTime.Now.Date;

                var resultado = lavagens
                    .GroupBy(l => new {
                        l.VeiculoId,
                        l.Veiculo?.Placa,
                        Modelo = l.Veiculo?.ModeloVeiculo?.DescricaoModelo,
                        IsPM = l.Veiculo?.PlacaBronzeId != null
                    })
                    .Select(g => new
                    {
                        placa = g.Key.IsPM ? $"{g.Key.Placa} (PM)" : g.Key.Placa ?? "N/A",
                        modelo = g.Key.Modelo ?? "",
                        lavagens = g.Count(),
                        ultimaLavagem = g.Max(l => l.Data)?.ToString("dd/MM/yyyy") ?? "N/A",
                        diasSemLavar = g.Max(l => l.Data).HasValue
                            ? (hoje - g.Max(l => l.Data).Value.Date).Days
                            : -1
                    })
                    .OrderByDescending(x => x.lavagens)
                    .ToList();

                return Json(new { success = true, data = resultado });
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message, ex, "DashboardLavagemController.cs", "EstatisticasPorVeiculo");
                Alerta.TratamentoErroComLinha("DashboardLavagemController.cs", "EstatisticasPorVeiculo", ex);
                return Json(new { success = false, message = ex.Message });
            }
        }

        #endregion
    }
}
