/* ****************************************************************************************
 * ⚡ ARQUIVO: LogErrosController.Dashboard.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Expor métricas do dashboard de logs (stats, timeline, rankings).
 *
 * 📥 ENTRADAS     : Filtros de período (dias).
 *
 * 📤 SAÍDAS       : JSON com estatísticas e séries.
 *
 * 🔗 CHAMADA POR  : Dashboard de logs.
 *
 * 🔄 CHAMA        : ILogRepository.
 **************************************************************************************** */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using FrotiX.Models;
using FrotiX.Repository.IRepository;
using FrotiX.Services;

namespace FrotiX.Controllers;

/****************************************************************************************
 * ⚡ CONTROLLER PARTIAL: LogErrosController.Dashboard
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Implementar endpoints de métricas de logs.
 *
 * 📥 ENTRADAS     : Dias de análise.
 *
 * 📤 SAÍDAS       : JSON com dados agregados.
 ****************************************************************************************/
public partial class LogErrosController
{
    // ====== DASHBOARD/STATS ======
    [HttpGet]
    [Route("Dashboard/Stats")]
    /****************************************************************************************
     * ⚡ FUNÇÃO: GetDashboardStats
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Executar GetDashboardStats.
     *
     * 📥 ENTRADAS     : Conforme assinatura do método.
     *
     * 📤 SAÍDAS       : JSON/resultado da operação.
     *
     * 🔗 CHAMADA POR  : Dashboard de logs.
     ****************************************************************************************/
    public async Task<IActionResult> GetDashboardStats([FromQuery] int dias = 7)
    {
        try
        {
            var repository = HttpContext.RequestServices.GetService<ILogRepository>();
            if (repository == null)
            {
                return Ok(new { success = false, error = "Repositório não disponível" });
            }

            var startDate = DateTime.Now.AddDays(-dias);
            var stats = await repository.GetDashboardStatsAsync(startDate, DateTime.Now);
            var comparison = await repository.GetComparisonWithPreviousPeriodAsync(dias);

            return Ok(new
            {
                success = true,
                stats = new
                {
                    totalLogs = stats.TotalLogs,
                    totalErros = stats.TotalErros,
                    totalWarnings = stats.TotalWarnings,
                    totalInfo = stats.TotalInfo,
                    totalJsErrors = stats.TotalJsErrors,
                    totalHttpErrors = stats.TotalHttpErrors,
                    totalConsole = stats.TotalConsole,
                    totalServidor = stats.TotalServidor,
                    totalCliente = stats.TotalCliente,
                    naoResolvidos = stats.NaoResolvidos,
                    resolvidosHoje = stats.ResolvidosHoje,
                    ultimoErro = stats.UltimoErro,
                    mediaErrosPorHora = stats.MediaErrosPorHora,
                    mediaErrosPorDia = stats.MediaErrosPorDia
                },
                comparison = new
                {
                    periodoAtual = comparison.PeriodoAtual,
                    periodoAnterior = comparison.PeriodoAnterior,
                    variacaoPercentual = comparison.VariacaoPercentual,
                    tendencia = comparison.Tendencia
                }
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter estatísticas do dashboard");
            return Ok(new { success = false, error = ex.Message });
        }
    }

    // ====== DASHBOARD/TIMELINE ======
    [HttpGet]
    [Route("Dashboard/Timeline")]
    /****************************************************************************************
     * ⚡ FUNÇÃO: GetDashboardTimeline
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Executar GetDashboardTimeline.
     *
     * 📥 ENTRADAS     : Conforme assinatura do método.
     *
     * 📤 SAÍDAS       : JSON/resultado da operação.
     *
     * 🔗 CHAMADA POR  : Dashboard de logs.
     ****************************************************************************************/
    public async Task<IActionResult> GetDashboardTimeline(
        [FromQuery] int dias = 7,
        [FromQuery] string? tipos = null,
        [FromQuery] string? origens = null)
    {
        try
        {
            var repository = HttpContext.RequestServices.GetService<ILogRepository>();
            if (repository == null)
            {
                return Ok(new { success = false, error = "Repositório não disponível" });
            }

            var tiposList = ParseFilterList(tipos);
            var origensList = ParseFilterList(origens);
            var hasFilters = tiposList.Any() || origensList.Any();

            if (!hasFilters)
            {
                // Sem filtros - usa método original
                var timeline = await repository.GetErrorsByDayAsync(dias);
                return Ok(new
                {
                    success = true,
                    timeline = timeline.Select(t => new
                    {
                        data = t.Data,
                        label = t.Label,
                        total = t.Total,
                        erros = t.Erros,
                        warnings = t.Warnings,
                        info = t.Info
                    })
                });
            }

            // Com filtros - busca logs e agrupa manualmente
            var startDate = DateTime.Now.AddDays(-dias);
            var logs = await repository.GetByPeriodAsync(startDate, DateTime.Now);

            // Aplica filtros
            logs = ApplyFilters(logs, tiposList, origensList);

            // Agrupa por dia
            var grouped = logs
                .GroupBy(l => l.DataHora.Date)
                .Select(g => new
                {
                    data = g.Key,
                    label = g.Key.ToString("dd/MM"),
                    total = g.Count(),
                    erros = g.Count(l => l.Tipo.Contains("ERROR")),
                    warnings = g.Count(l => l.Tipo.Contains("WARN")),
                    info = g.Count(l => l.Tipo.Contains("INFO"))
                })
                .OrderBy(x => x.data)
                .ToList();

            return Ok(new { success = true, timeline = grouped });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter timeline do dashboard");
            return Ok(new { success = false, error = ex.Message });
        }
    }

    // Helper: Parse comma-separated filter list
    private List<string> ParseFilterList(string? filters)
    {
        if (string.IsNullOrWhiteSpace(filters))
            return new List<string>();

        return filters.Split(',', StringSplitOptions.RemoveEmptyEntries)
                      .Select(f => f.Trim().ToUpper())
                      .ToList();
    }

    // Helper: Apply tipo and origem filters to log list
    private List<LogErro> ApplyFilters(List<LogErro> logs, List<string> tipos, List<string> origens)
    {
        var filtered = logs.AsEnumerable();

        if (tipos.Any())
        {
            filtered = filtered.Where(l =>
                tipos.Any(t => l.Tipo.ToUpper().Contains(t)));
        }

        if (origens.Any())
        {
            filtered = filtered.Where(l =>
                origens.Any(o => l.Origem.ToUpper() == o));
        }

        return filtered.ToList();
    }

    // ====== DASHBOARD/DISTRIBUTION ======
    [HttpGet]
    [Route("Dashboard/Distribution")]
    /****************************************************************************************
     * ⚡ FUNÇÃO: GetDashboardDistribution
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Executar GetDashboardDistribution.
     *
     * 📥 ENTRADAS     : Conforme assinatura do método.
     *
     * 📤 SAÍDAS       : JSON/resultado da operação.
     *
     * 🔗 CHAMADA POR  : Dashboard de logs.
     ****************************************************************************************/
    public async Task<IActionResult> GetDashboardDistribution(
        [FromQuery] int dias = 7,
        [FromQuery] string? tipos = null,
        [FromQuery] string? origens = null)
    {
        try
        {
            var repository = HttpContext.RequestServices.GetService<ILogRepository>();
            if (repository == null)
            {
                return Ok(new { success = false, error = "Repositório não disponível" });
            }

            var tiposList = ParseFilterList(tipos);
            var origensList = ParseFilterList(origens);
            var hasFilters = tiposList.Any() || origensList.Any();

            if (!hasFilters)
            {
                var startDate = DateTime.Now.AddDays(-dias);
                var distribution = await repository.GetDistributionByTypeAsync(startDate, DateTime.Now);
                return Ok(new
                {
                    success = true,
                    distribution = distribution.Select(d => new
                    {
                        label = d.Label,
                        count = d.Count,
                        percentage = d.Percentage,
                        color = d.Color,
                        icon = d.Icon
                    })
                });
            }

            // Com filtros - calcula distribuição manualmente
            var start = DateTime.Now.AddDays(-dias);
            var logs = await repository.GetByPeriodAsync(start, DateTime.Now);
            logs = ApplyFilters(logs, tiposList, origensList);

            var total = logs.Count;
            var typeColors = new Dictionary<string, string>
            {
                { "ERROR", "#dc3545" }, { "ERROR-JS", "#6f42c1" }, { "HTTP-ERROR", "#fd7e14" },
                { "WARN", "#ffc107" }, { "INFO", "#17a2b8" }, { "CONSOLE-ERROR", "#dc3545" },
                { "CONSOLE-WARN", "#ffc107" }, { "CONSOLE-INFO", "#17a2b8" }
            };

            var grouped = logs
                .GroupBy(l => l.Tipo)
                .Select(g => new
                {
                    label = g.Key,
                    count = g.Count(),
                    percentage = total > 0 ? Math.Round((double)g.Count() / total * 100, 1) : 0,
                    color = typeColors.GetValueOrDefault(g.Key, "#6c757d"),
                    icon = ""
                })
                .OrderByDescending(x => x.count)
                .ToList();

            return Ok(new { success = true, distribution = grouped });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter distribuição do dashboard");
            return Ok(new { success = false, error = ex.Message });
        }
    }

    // ====== DASHBOARD/TOPPAGES ======
    [HttpGet]
    [Route("Dashboard/TopPages")]
    /****************************************************************************************
     * ⚡ FUNÇÃO: GetDashboardTopPages
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Executar GetDashboardTopPages.
     *
     * 📥 ENTRADAS     : Conforme assinatura do método.
     *
     * 📤 SAÍDAS       : JSON/resultado da operação.
     *
     * 🔗 CHAMADA POR  : Dashboard de logs.
     ****************************************************************************************/
    public async Task<IActionResult> GetDashboardTopPages(
        [FromQuery] int dias = 7,
        [FromQuery] string? tipos = null,
        [FromQuery] string? origens = null)
    {
        try
        {
            var repository = HttpContext.RequestServices.GetService<ILogRepository>();
            if (repository == null)
            {
                return Ok(new { success = false, error = "Repositório não disponível" });
            }

            var tiposList = ParseFilterList(tipos);
            var origensList = ParseFilterList(origens);
            var hasFilters = tiposList.Any() || origensList.Any();

            if (!hasFilters)
            {
                var ranking = await repository.GetTopPagesWithErrorsAsync(10, dias);
                return Ok(new
                {
                    success = true,
                    ranking = ranking.Select(r => new
                    {
                        posicao = r.Posicao,
                        label = r.Label,
                        subLabel = r.SubLabel,
                        count = r.Count,
                        percentage = r.Percentage,
                        icon = r.Icon,
                        color = r.Color
                    })
                });
            }

            // Com filtros
            var startDate = DateTime.Now.AddDays(-dias);
            var logs = await repository.GetByPeriodAsync(startDate, DateTime.Now);
            logs = ApplyFilters(logs, tiposList, origensList);

            var total = logs.Count;
            var grouped = logs
                .Where(l => !string.IsNullOrEmpty(l.Url))
                .GroupBy(l => l.Url)
                .OrderByDescending(g => g.Count())
                .Take(10)
                .Select((g, idx) =>
                {
                    var firstLog = g.First();
                    var formattedLabel = FormatPageLabel(g.Key, firstLog.Arquivo, firstLog.Metodo);

                    return new
                    {
                        posicao = idx + 1,
                        label = formattedLabel,
                        subLabel = g.Key,
                        count = g.Count(),
                        percentage = total > 0 ? Math.Round((double)g.Count() / total * 100, 1) : 0,
                        icon = "",
                        color = ""
                    };
                })
                .ToList();

            return Ok(new { success = true, ranking = grouped });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter top páginas do dashboard");
            return Ok(new { success = false, error = ex.Message });
        }
    }

    private string TruncateUrl(string url)
    {
        if (string.IsNullOrEmpty(url)) return url;
        var path = url.Split('?')[0];
        return path.Length > 50 ? "..." + path.Substring(path.Length - 47) : path;
    }

    /****************************************************************************************
     * ⚡ FUNÇÃO: FormatPageLabel
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Formatar label de página para exibição (path a partir de /Pages + método).
     *
     * 📥 ENTRADAS     : url (URL completa), arquivo (caminho do arquivo), metodo (nome do método).
     *
     * 📤 SAÍDAS       : String formatada (ex: "Viagens/Create.OnPostAsync").
     ****************************************************************************************/
    private string FormatPageLabel(string? url, string? arquivo, string? metodo)
    {
        string path = "";

        // Tenta extrair path da URL primeiro
        if (!string.IsNullOrEmpty(url))
        {
            try
            {
                var uri = new Uri(url, UriKind.RelativeOrAbsolute);
                if (uri.IsAbsoluteUri)
                {
                    path = uri.AbsolutePath;
                }
                else
                {
                    path = url.Split('?')[0];
                }
            }
            catch
            {
                path = url.Split('?')[0];
            }
        }

        // Se não conseguiu da URL, tenta do campo Arquivo
        if (string.IsNullOrEmpty(path) && !string.IsNullOrEmpty(arquivo))
        {
            path = arquivo;
        }

        // Remove prefixos desnecessários
        if (!string.IsNullOrEmpty(path))
        {
            // Remove /Pages/ do início se existir
            if (path.Contains("/Pages/", StringComparison.OrdinalIgnoreCase))
            {
                var idx = path.IndexOf("/Pages/", StringComparison.OrdinalIgnoreCase);
                path = path.Substring(idx + 7); // 7 = length of "/Pages/"
            }
            else if (path.StartsWith("/"))
            {
                path = path.Substring(1);
            }

            // Remove extensões .cshtml
            path = path.Replace(".cshtml", "");
        }

        // Adiciona método se disponível
        if (!string.IsNullOrEmpty(metodo))
        {
            // Remove namespace/classe se houver
            var metodoCurto = metodo;
            if (metodo.Contains('.'))
            {
                var parts = metodo.Split('.');
                metodoCurto = parts[parts.Length - 1]; // Pega apenas o último segmento
            }

            path = $"{path}.{metodoCurto}";
        }

        return string.IsNullOrEmpty(path) ? "(desconhecido)" : path;
    }

    // ====== DASHBOARD/TOPERRORS ======
    [HttpGet]
    [Route("Dashboard/TopErrors")]
    /****************************************************************************************
     * ⚡ FUNÇÃO: GetDashboardTopErrors
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Executar GetDashboardTopErrors.
     *
     * 📥 ENTRADAS     : Conforme assinatura do método.
     *
     * 📤 SAÍDAS       : JSON/resultado da operação.
     *
     * 🔗 CHAMADA POR  : Dashboard de logs.
     ****************************************************************************************/
    public async Task<IActionResult> GetDashboardTopErrors(
        [FromQuery] int dias = 7,
        [FromQuery] string? tipos = null,
        [FromQuery] string? origens = null)
    {
        try
        {
            var repository = HttpContext.RequestServices.GetService<ILogRepository>();
            if (repository == null)
            {
                return Ok(new { success = false, error = "Repositório não disponível" });
            }

            var tiposList = ParseFilterList(tipos);
            var origensList = ParseFilterList(origens);
            var hasFilters = tiposList.Any() || origensList.Any();

            if (!hasFilters)
            {
                var ranking = await repository.GetMostFrequentErrorsAsync(10, dias);
                return Ok(new
                {
                    success = true,
                    ranking = ranking.Select(r => new
                    {
                        posicao = r.Posicao,
                        label = r.Label,
                        subLabel = r.SubLabel,
                        count = r.Count,
                        percentage = r.Percentage,
                        hashErro = r.HashErro,
                        icon = r.Icon,
                        color = r.Color
                    })
                });
            }

            // Com filtros
            var startDate = DateTime.Now.AddDays(-dias);
            var logs = await repository.GetByPeriodAsync(startDate, DateTime.Now);
            logs = ApplyFilters(logs, tiposList, origensList);

            var total = logs.Count;
            var grouped = logs
                .GroupBy(l => TruncateMessage(l.Mensagem, 100))
                .OrderByDescending(g => g.Count())
                .Take(10)
                .Select((g, idx) => new
                {
                    posicao = idx + 1,
                    label = g.Key,
                    subLabel = g.First().Tipo,
                    count = g.Count(),
                    percentage = total > 0 ? Math.Round((double)g.Count() / total * 100, 1) : 0,
                    hashErro = (string?)null,
                    icon = "",
                    color = ""
                })
                .ToList();

            return Ok(new { success = true, ranking = grouped });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter top erros do dashboard");
            return Ok(new { success = false, error = ex.Message });
        }
    }

    private string TruncateMessage(string msg, int maxLen)
    {
        if (string.IsNullOrEmpty(msg)) return "(sem mensagem)";
        return msg.Length > maxLen ? msg.Substring(0, maxLen) + "..." : msg;
    }

    // ====== DASHBOARD/PEAKHOURS ======
    [HttpGet]
    [Route("Dashboard/PeakHours")]
    /****************************************************************************************
     * ⚡ FUNÇÃO: GetDashboardPeakHours
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Executar GetDashboardPeakHours.
     *
     * 📥 ENTRADAS     : Conforme assinatura do método.
     *
     * 📤 SAÍDAS       : JSON/resultado da operação.
     *
     * 🔗 CHAMADA POR  : Dashboard de logs.
     ****************************************************************************************/
    public async Task<IActionResult> GetDashboardPeakHours(
        [FromQuery] int dias = 7,
        [FromQuery] string? tipos = null,
        [FromQuery] string? origens = null)
    {
        try
        {
            var repository = HttpContext.RequestServices.GetService<ILogRepository>();
            if (repository == null)
            {
                return Ok(new { success = false, error = "Repositório não disponível" });
            }

            var tiposList = ParseFilterList(tipos);
            var origensList = ParseFilterList(origens);
            var hasFilters = tiposList.Any() || origensList.Any();

            if (!hasFilters)
            {
                var peakHours = await repository.GetPeakHoursAsync(dias);
                return Ok(new
                {
                    success = true,
                    peakHours = peakHours.Select(p => new
                    {
                        hora = p.Hora,
                        horaFormatada = p.HoraFormatada,
                        totalErros = p.TotalErros,
                        mediaErros = p.MediaErros,
                        isPico = p.IsPico
                    })
                });
            }

            // Com filtros
            var startDate = DateTime.Now.AddDays(-dias);
            var logs = await repository.GetByPeriodAsync(startDate, DateTime.Now);
            logs = ApplyFilters(logs, tiposList, origensList);

            var grouped = logs
                .GroupBy(l => l.DataHora.Hour)
                .Select(g => new
                {
                    hora = g.Key,
                    horaFormatada = $"{g.Key:D2}:00",
                    totalErros = g.Count(),
                    mediaErros = Math.Round((double)g.Count() / dias, 2),
                    isPico = false
                })
                .OrderBy(x => x.hora)
                .ToList();

            // Preenche horas sem dados
            var result = Enumerable.Range(0, 24)
                .Select(h => grouped.FirstOrDefault(g => g.hora == h) ?? new
                {
                    hora = h,
                    horaFormatada = $"{h:D2}:00",
                    totalErros = 0,
                    mediaErros = 0.0,
                    isPico = false
                })
                .ToList();

            return Ok(new { success = true, peakHours = result });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter horários de pico do dashboard");
            return Ok(new { success = false, error = ex.Message });
        }
    }

    // ====== DASHBOARD/ANOMALIES ======
    [HttpGet]
    [Route("Dashboard/Anomalies")]
    /****************************************************************************************
     * ⚡ FUNÇÃO: GetDashboardAnomalies
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Executar GetDashboardAnomalies.
     *
     * 📥 ENTRADAS     : Conforme assinatura do método.
     *
     * 📤 SAÍDAS       : JSON/resultado da operação.
     *
     * 🔗 CHAMADA POR  : Dashboard de logs.
     ****************************************************************************************/
    public async Task<IActionResult> GetDashboardAnomalies(
        [FromQuery] int dias = 7,
        [FromQuery] string? tipos = null,
        [FromQuery] string? origens = null)
    {
        try
        {
            var repository = HttpContext.RequestServices.GetService<ILogRepository>();
            if (repository == null)
            {
                return Ok(new { success = false, error = "Repositório não disponível" });
            }

            var tiposList = ParseFilterList(tipos);
            var origensList = ParseFilterList(origens);
            var hasFilters = tiposList.Any() || origensList.Any();

            if (!hasFilters)
            {
                var anomalies = await repository.DetectAnomaliesAsync(dias);
                return Ok(new
                {
                    success = true,
                    anomalies = anomalies.Select(a => new
                    {
                        dataHora = a.DataHora,
                        countErros = a.CountErros,
                        media = a.Media,
                        desvioPadrao = a.DesvioPadrao,
                        zScore = a.ZScore,
                        severidade = a.Severidade,
                        mensagem = a.Mensagem
                    })
                });
            }

            // Com filtros - detecta anomalias manualmente
            var startDate = DateTime.Now.AddDays(-dias);
            var logs = await repository.GetByPeriodAsync(startDate, DateTime.Now);
            logs = ApplyFilters(logs, tiposList, origensList);

            // Agrupa por hora
            var hourlyGroups = logs
                .GroupBy(l => new { l.DataHora.Date, l.DataHora.Hour })
                .Select(g => new { DateTime = g.Key.Date.AddHours(g.Key.Hour), Count = g.Count() })
                .OrderBy(x => x.DateTime)
                .ToList();

            if (hourlyGroups.Count < 3)
            {
                return Ok(new { success = true, anomalies = new List<object>() });
            }

            // Calcula média e desvio padrão
            var counts = hourlyGroups.Select(h => (double)h.Count).ToList();
            var media = counts.Average();
            var desvioPadrao = Math.Sqrt(counts.Average(c => Math.Pow(c - media, 2)));

            // Detecta anomalias (Z-score > 2)
            var anomalias = hourlyGroups
                .Select(h =>
                {
                    var zScore = desvioPadrao > 0 ? (h.Count - media) / desvioPadrao : 0;
                    return new
                    {
                        dataHora = h.DateTime,
                        countErros = h.Count,
                        media = Math.Round(media, 2),
                        desvioPadrao = Math.Round(desvioPadrao, 2),
                        zScore = Math.Round(zScore, 2),
                        severidade = zScore > 3 ? "high" : (zScore > 2 ? "medium" : "low"),
                        mensagem = $"Pico de {h.Count} erros detectado"
                    };
                })
                .Where(a => a.zScore > 2)
                .OrderByDescending(a => a.zScore)
                .Take(10)
                .ToList();

            return Ok(new { success = true, anomalies = anomalias });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter anomalias do dashboard");
            return Ok(new { success = false, error = ex.Message });
        }
    }

    // ====== DASHBOARD/TOPUSERS ======
    [HttpGet]
    [Route("Dashboard/TopUsers")]
    /****************************************************************************************
     * ⚡ FUNÇÃO: GetDashboardTopUsers
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Executar GetDashboardTopUsers.
     *
     * 📥 ENTRADAS     : Conforme assinatura do método.
     *
     * 📤 SAÍDAS       : JSON/resultado da operação.
     *
     * 🔗 CHAMADA POR  : Dashboard de logs.
     ****************************************************************************************/
    public async Task<IActionResult> GetDashboardTopUsers([FromQuery] int dias = 7)
    {
        try
        {
            var repository = HttpContext.RequestServices.GetService<ILogRepository>();
            if (repository == null)
            {
                return Ok(new { success = false, error = "Repositório não disponível" });
            }

            var ranking = await repository.GetTopUsersWithErrorsAsync(10, dias);

            return Ok(new
            {
                success = true,
                ranking = ranking.Select(r => new
                {
                    posicao = r.Posicao,
                    label = r.Label,
                    subLabel = r.SubLabel,
                    count = r.Count,
                    percentage = r.Percentage,
                    icon = r.Icon,
                    color = r.Color
                })
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter top usuários do dashboard");
            return Ok(new { success = false, error = ex.Message });
        }
    }

    // ====== DASHBOARD/ALERTS ======
    [HttpGet]
    [Route("Dashboard/Alerts")]
    /****************************************************************************************
     * ⚡ FUNÇÃO: GetDashboardAlerts
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Executar GetDashboardAlerts.
     *
     * 📥 ENTRADAS     : Conforme assinatura do método.
     *
     * 📤 SAÍDAS       : JSON/resultado da operação.
     *
     * 🔗 CHAMADA POR  : Dashboard de logs.
     ****************************************************************************************/
    public async Task<IActionResult> GetDashboardAlerts()
    {
        try
        {
            var repository = HttpContext.RequestServices.GetService<ILogRepository>();
            if (repository == null)
            {
                return Ok(new { success = false, error = "Repositório não disponível" });
            }

            var config = new LogAlertConfig
            {
                ThresholdErrosPorHora = 50,
                ThresholdErrosPorMinuto = 10,
                ThresholdErrosCriticos = 5,
                ThresholdMesmoErro = 20,
                AlertarNovosErros = true,
                AlertarPicos = true
            };

            var alerts = await repository.CheckThresholdsAsync(config);

            return Ok(new
            {
                success = true,
                alerts = alerts.Select(a => new
                {
                    tipo = a.Tipo,
                    descricao = a.Descricao,
                    valorAtual = a.ValorAtual,
                    threshold = a.Threshold,
                    percentualExcedido = a.PercentualExcedido,
                    severidade = a.Severidade,
                    detectadoEm = a.DetectadoEm
                })
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao verificar alertas do dashboard");
            return Ok(new { success = false, error = ex.Message });
        }
    }

    // ====== EXPORT/EXCEL ======
    [HttpGet]
    [Route("Export/Excel")]
    /****************************************************************************************
     * ⚡ FUNÇÃO: ExportLogsToExcel
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Executar ExportLogsToExcel.
     *
     * 📥 ENTRADAS     : Conforme assinatura do método.
     *
     * 📤 SAÍDAS       : JSON/resultado da operação.
     *
     * 🔗 CHAMADA POR  : Dashboard de logs.
     ****************************************************************************************/
    public async Task<IActionResult> ExportLogsToExcel(
        [FromQuery] DateTime? startDate,
        [FromQuery] DateTime? endDate,
        [FromQuery] string? tipo,
        [FromQuery] string? origem,
        [FromQuery] string? usuario)
    {
        try
        {
            var exportService = HttpContext.RequestServices.GetService<ILogErrosExportService>();
            if (exportService == null)
            {
                return BadRequest("Serviço de exportação não disponível");
            }

            var filter = new LogQueryFilter
            {
                StartDate = startDate ?? DateTime.Now.AddDays(-7),
                EndDate = endDate ?? DateTime.Now,
                Tipo = tipo,
                Origem = origem,
                Usuario = usuario
            };

            var bytes = await exportService.ExportLogsToExcelAsync(filter);
            var fileName = $"FrotiX_Logs_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";

            return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao exportar logs para Excel");
            return BadRequest(new { success = false, error = ex.Message });
        }
    }

    // ====== EXPORT/CSV ======
    [HttpGet]
    [Route("Export/CSV")]
    /****************************************************************************************
     * ⚡ FUNÇÃO: ExportLogsToCsv
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Executar ExportLogsToCsv.
     *
     * 📥 ENTRADAS     : Conforme assinatura do método.
     *
     * 📤 SAÍDAS       : JSON/resultado da operação.
     *
     * 🔗 CHAMADA POR  : Dashboard de logs.
     ****************************************************************************************/
    public async Task<IActionResult> ExportLogsToCsv(
        [FromQuery] DateTime? startDate,
        [FromQuery] DateTime? endDate,
        [FromQuery] string? tipo,
        [FromQuery] string? origem,
        [FromQuery] string? usuario)
    {
        try
        {
            var exportService = HttpContext.RequestServices.GetService<ILogErrosExportService>();
            if (exportService == null)
            {
                return BadRequest("Serviço de exportação não disponível");
            }

            var filter = new LogQueryFilter
            {
                StartDate = startDate ?? DateTime.Now.AddDays(-7),
                EndDate = endDate ?? DateTime.Now,
                Tipo = tipo,
                Origem = origem,
                Usuario = usuario
            };

            var bytes = await exportService.ExportLogsToCsvAsync(filter);
            var fileName = $"FrotiX_Logs_{DateTime.Now:yyyyMMdd_HHmmss}.csv";

            return File(bytes, "text/csv", fileName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao exportar logs para CSV");
            return BadRequest(new { success = false, error = ex.Message });
        }
    }

    // ====== EXPORT/REPORT/PDF ======
    [HttpGet]
    [Route("Export/Report/PDF")]
    /****************************************************************************************
     * ⚡ FUNÇÃO: ExportReportPdf
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Executar ExportReportPdf.
     *
     * 📥 ENTRADAS     : Conforme assinatura do método.
     *
     * 📤 SAÍDAS       : JSON/resultado da operação.
     *
     * 🔗 CHAMADA POR  : Dashboard de logs.
     ****************************************************************************************/
    public async Task<IActionResult> ExportReportPdf([FromQuery] int dias = 7)
    {
        try
        {
            var exportService = HttpContext.RequestServices.GetService<ILogErrosExportService>();
            if (exportService == null)
            {
                return BadRequest("Serviço de exportação não disponível");
            }

            var endDate = DateTime.Now;
            var startDate = endDate.AddDays(-dias);

            var bytes = await exportService.ExportExecutiveReportPdfAsync(startDate, endDate);
            var fileName = $"FrotiX_Relatorio_Executivo_{DateTime.Now:yyyyMMdd}.pdf";

            return File(bytes, "application/pdf", fileName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao gerar relatório PDF");
            return BadRequest(new { success = false, error = ex.Message });
        }
    }

    // ====== EXPORT/REPORT/EXCEL ======
    [HttpGet]
    [Route("Export/Report/Excel")]
    /****************************************************************************************
     * ⚡ FUNÇÃO: ExportReportExcel
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Executar ExportReportExcel.
     *
     * 📥 ENTRADAS     : Conforme assinatura do método.
     *
     * 📤 SAÍDAS       : JSON/resultado da operação.
     *
     * 🔗 CHAMADA POR  : Dashboard de logs.
     ****************************************************************************************/
    public async Task<IActionResult> ExportReportExcel([FromQuery] int dias = 7)
    {
        try
        {
            var exportService = HttpContext.RequestServices.GetService<ILogErrosExportService>();
            if (exportService == null)
            {
                return BadRequest("Serviço de exportação não disponível");
            }

            var endDate = DateTime.Now;
            var startDate = endDate.AddDays(-dias);

            var bytes = await exportService.ExportExecutiveReportExcelAsync(startDate, endDate);
            var fileName = $"FrotiX_Relatorio_Executivo_{DateTime.Now:yyyyMMdd}.xlsx";

            return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao gerar relatório Excel");
            return BadRequest(new { success = false, error = ex.Message });
        }
    }

    // ====== DETALHES DE ERROS POR PÁGINA ======
    [HttpGet]
    [Route("Dashboard/PageErrors")]
    /****************************************************************************************
     * ⚡ FUNÇÃO: GetPageErrors
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Obter lista de erros de uma página específica para exibição no modal.
     *
     * 📥 ENTRADAS     : url (URL da página), dias (período de análise).
     *
     * 📤 SAÍDAS       : JSON com lista de erros detalhados.
     *
     * 🔗 CHAMADA POR  : Modal de detalhes do Dashboard.
     ****************************************************************************************/
    public async Task<IActionResult> GetPageErrors(
        [FromQuery] string url,
        [FromQuery] int dias = 7,
        [FromQuery] int limit = 50)
    {
        try
        {
            if (string.IsNullOrEmpty(url))
            {
                return Ok(new { success = false, error = "URL não informada" });
            }

            var repository = HttpContext.RequestServices.GetService<ILogRepository>();
            if (repository == null)
            {
                return Ok(new { success = false, error = "Repositório não disponível" });
            }

            var errors = await repository.GetByUrlAsync(url, limit);

            // Filtra por período
            var startDate = DateTime.Now.AddDays(-dias);
            errors = errors.Where(e => e.DataHora >= startDate).ToList();

            return Ok(new
            {
                success = true,
                url = url,
                totalErrors = errors.Count,
                errors = errors.Select(e => new
                {
                    id = e.LogErroId,
                    dataHora = e.DataHora,
                    tipo = e.Tipo,
                    origem = e.Origem,
                    nivel = e.Nivel,
                    mensagem = e.Mensagem,
                    mensagemCurta = TruncateMessage(e.Mensagem, 150),
                    arquivo = e.Arquivo,
                    metodo = e.Metodo,
                    linha = e.Linha,
                    coluna = e.Coluna,
                    exceptionType = e.ExceptionType,
                    exceptionMessage = e.ExceptionMessage,
                    stackTrace = e.StackTrace,
                    innerException = e.InnerException,
                    usuario = e.Usuario,
                    userAgent = e.UserAgent,
                    httpMethod = e.HttpMethod,
                    statusCode = e.StatusCode,
                    dadosAdicionais = e.DadosAdicionais,
                    hashErro = e.HashErro,
                    resolvido = e.Resolvido,
                    cor = e.Cor,
                    icone = e.Icone
                }).ToList()
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter erros da página");
            return Ok(new { success = false, error = ex.Message });
        }
    }

    // ====== DETALHES DE UM ERRO ESPECÍFICO ======
    [HttpGet]
    [Route("Dashboard/ErrorDetails/{id}")]
    /****************************************************************************************
     * ⚡ FUNÇÃO: GetErrorDetails
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Obter detalhes completos de um erro específico.
     *
     * 📥 ENTRADAS     : id (ID do erro).
     *
     * 📤 SAÍDAS       : JSON com detalhes completos do erro.
     *
     * 🔗 CHAMADA POR  : Modal de detalhes do Dashboard.
     ****************************************************************************************/
    public async Task<IActionResult> GetErrorDetails(long id)
    {
        try
        {
            var repository = HttpContext.RequestServices.GetService<ILogRepository>();
            if (repository == null)
            {
                return Ok(new { success = false, error = "Repositório não disponível" });
            }

            var error = await repository.GetByIdAsync(id);
            if (error == null)
            {
                return Ok(new { success = false, error = "Erro não encontrado" });
            }

            return Ok(new
            {
                success = true,
                error = new
                {
                    id = error.LogErroId,
                    dataHora = error.DataHora,
                    tipo = error.Tipo,
                    origem = error.Origem,
                    nivel = error.Nivel,
                    categoria = error.Categoria,
                    mensagem = error.Mensagem,
                    arquivo = error.Arquivo,
                    metodo = error.Metodo,
                    linha = error.Linha,
                    coluna = error.Coluna,
                    exceptionType = error.ExceptionType,
                    exceptionMessage = error.ExceptionMessage,
                    stackTrace = error.StackTrace,
                    innerException = error.InnerException,
                    url = error.Url,
                    httpMethod = error.HttpMethod,
                    statusCode = error.StatusCode,
                    userAgent = error.UserAgent,
                    ipAddress = error.IpAddress,
                    usuario = error.Usuario,
                    sessionId = error.SessionId,
                    dadosAdicionais = error.DadosAdicionais,
                    hashErro = error.HashErro,
                    resolvido = error.Resolvido,
                    dataResolucao = error.DataResolucao,
                    resolvidoPor = error.ResolvidoPor,
                    observacoes = error.Observacoes,
                    cor = error.Cor,
                    icone = error.Icone
                }
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter detalhes do erro #{Id}", id);
            return Ok(new { success = false, error = ex.Message });
        }
    }

    // ====== ANÁLISE DE ERRO COM CLAUDE AI ======
    [HttpPost]
    [Route("Dashboard/AnalyzeError/{id}")]
    /****************************************************************************************
     * ⚡ FUNÇÃO: AnalyzeError
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Enviar erro para análise do Claude AI e retornar sugestões de correção.
     *
     * 📥 ENTRADAS     : id (ID do erro).
     *
     * 📤 SAÍDAS       : JSON com análise do Claude (diagnóstico, correção, prevenção).
     *
     * 🔗 CHAMADA POR  : Botão "Analisar com IA" no modal do Dashboard.
     ****************************************************************************************/
    public async Task<IActionResult> AnalyzeError(long id)
    {
        try
        {
            var repository = HttpContext.RequestServices.GetService<ILogRepository>();
            var claudeService = HttpContext.RequestServices.GetService<IClaudeAnalysisService>();

            if (repository == null)
            {
                return Ok(new { success = false, error = "Repositório não disponível" });
            }

            if (claudeService == null)
            {
                return Ok(new { success = false, error = "Serviço de análise não disponível" });
            }

            if (!claudeService.IsConfigured)
            {
                return Ok(new { success = false, error = "API Key do Claude não configurada. Configure em appsettings.json na seção 'ClaudeAI'." });
            }

            var error = await repository.GetByIdAsync(id);
            if (error == null)
            {
                return Ok(new { success = false, error = "Erro não encontrado" });
            }

            _logger.LogInformation("Iniciando análise do erro #{Id} com Claude AI", id);

            var result = await claudeService.AnalyzeErrorAsync(error);

            if (!result.Success)
            {
                return Ok(new
                {
                    success = false,
                    error = result.Error
                });
            }

            return Ok(new
            {
                success = true,
                analysis = result.Analysis,
                model = result.Model,
                tokens = new
                {
                    input = result.InputTokens,
                    output = result.OutputTokens,
                    total = result.InputTokens + result.OutputTokens
                },
                analyzedAt = result.AnalyzedAt
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao analisar erro #{Id} com Claude", id);
            return Ok(new { success = false, error = ex.Message });
        }
    }

    // ====== VERIFICAR STATUS DO SERVIÇO CLAUDE ======
    [HttpGet]
    [Route("Dashboard/ClaudeStatus")]
    /****************************************************************************************
     * ⚡ FUNÇÃO: GetClaudeStatus
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Verificar se o serviço Claude AI está configurado e disponível.
     *
     * 📤 SAÍDAS       : JSON com status do serviço.
     ****************************************************************************************/
    public IActionResult GetClaudeStatus()
    {
        try
        {
            var claudeService = HttpContext.RequestServices.GetService<IClaudeAnalysisService>();

            if (claudeService == null)
            {
                return Ok(new { success = true, configured = false, message = "Serviço não registrado" });
            }

            return Ok(new
            {
                success = true,
                configured = claudeService.IsConfigured,
                message = claudeService.IsConfigured
                    ? "Claude AI configurado e pronto para uso"
                    : "Configure a API Key do Claude em appsettings.json"
            });
        }
        catch (Exception ex)
        {
            return Ok(new { success = false, error = ex.Message });
        }
    }
}
