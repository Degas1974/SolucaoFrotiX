# Controllers/LogErrosController.Dashboard.cs

**ARQUIVO NOVO** | 935 linhas de codigo

> Copiar integralmente para o Janeiro.

---

```csharp
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

public partial class LogErrosController
{

    [HttpGet]
    [Route("Dashboard/Stats")]

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

    [HttpGet]
    [Route("Dashboard/Timeline")]

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

            var startDate = DateTime.Now.AddDays(-dias);
            var logs = await repository.GetByPeriodAsync(startDate, DateTime.Now);

            logs = ApplyFilters(logs, tiposList, origensList);

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

    private List<string> ParseFilterList(string? filters)
    {
        if (string.IsNullOrWhiteSpace(filters))
            return new List<string>();

        return filters.Split(',', StringSplitOptions.RemoveEmptyEntries)
                      .Select(f => f.Trim().ToUpper())
                      .ToList();
    }

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

    [HttpGet]
    [Route("Dashboard/Distribution")]

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

    [HttpGet]
    [Route("Dashboard/TopPages")]

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

    private string FormatPageLabel(string? url, string? arquivo, string? metodo)
    {
        string path = "";

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

        if (string.IsNullOrEmpty(path) && !string.IsNullOrEmpty(arquivo))
        {
            path = arquivo;
        }

        if (!string.IsNullOrEmpty(path))
        {

            if (path.Contains("/Pages/", StringComparison.OrdinalIgnoreCase))
            {
                var idx = path.IndexOf("/Pages/", StringComparison.OrdinalIgnoreCase);
                path = path.Substring(idx + 7);
            }
            else if (path.StartsWith("/"))
            {
                path = path.Substring(1);
            }

            path = path.Replace(".cshtml", "");
        }

        if (!string.IsNullOrEmpty(metodo))
        {

            var metodoCurto = metodo;
            if (metodo.Contains('.'))
            {
                var parts = metodo.Split('.');
                metodoCurto = parts[parts.Length - 1];
            }

            path = $"{path}.{metodoCurto}";
        }

        return string.IsNullOrEmpty(path) ? "(desconhecido)" : path;
    }

    [HttpGet]
    [Route("Dashboard/TopErrors")]

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

    [HttpGet]
    [Route("Dashboard/PeakHours")]

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

            var startDate = DateTime.Now.AddDays(-dias);
            var logs = await repository.GetByPeriodAsync(startDate, DateTime.Now);
            logs = ApplyFilters(logs, tiposList, origensList);

            var grouped = logs

... (+435 linhas)
```
