/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: LogRepository.cs                                                                        ║
   ║ 📂 CAMINHO: /Repository                                                                             ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO: Implementação do ILogRepository com acesso a dados de logs de erros.                  ║
   ║              Inclui consultas otimizadas, métricas, análises e alertas.                            ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📝 VERSÃO: 1.0 | DATA: 31/01/2026 | AUTOR: Claude Code                                             ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FrotiX.Data;
using FrotiX.Models;
using FrotiX.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace FrotiX.Repository;

/// <summary>
/// Implementação do repositório de logs de erros
/// </summary>
public class LogRepository : ILogRepository
{
    private readonly FrotiXDbContext _context;

    public LogRepository(FrotiXDbContext context)
    {
        _context = context;
    }

    // ====== CRUD BÁSICO ======

    public async Task<LogErro> AddAsync(LogErro logErro)
    {
        try
        {
            logErro.CriadoEm = DateTime.Now;
            _context.LogErros.Add(logErro);
            await _context.SaveChangesAsync();
            return logErro;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[LogRepository] Erro ao adicionar log: {ex.Message}");
            throw;
        }
    }

    public async Task AddRangeAsync(IEnumerable<LogErro> logErros)
    {
        try
        {
            foreach (var log in logErros)
            {
                log.CriadoEm = DateTime.Now;
            }
            _context.LogErros.AddRange(logErros);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[LogRepository] Erro ao adicionar logs em batch: {ex.Message}");
            throw;
        }
    }

    public async Task<LogErro?> GetByIdAsync(long id)
    {
        try
        {
            return await _context.LogErros.FindAsync(id);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[LogRepository] Erro ao buscar log por ID: {ex.Message}");
            throw;
        }
    }

    public async Task UpdateAsync(LogErro logErro)
    {
        try
        {
            _context.LogErros.Update(logErro);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[LogRepository] Erro ao atualizar log: {ex.Message}");
            throw;
        }
    }

    public async Task DeleteAsync(long id)
    {
        try
        {
            var log = await _context.LogErros.FindAsync(id);
            if (log != null)
            {
                _context.LogErros.Remove(log);
                await _context.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[LogRepository] Erro ao deletar log: {ex.Message}");
            throw;
        }
    }

    public async Task<int> DeleteBeforeDateAsync(DateTime date)
    {
        try
        {
            var logsToDelete = await _context.LogErros
                .Where(l => l.DataHora < date)
                .ToListAsync();

            _context.LogErros.RemoveRange(logsToDelete);
            return await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[LogRepository] Erro ao deletar logs antigos: {ex.Message}");
            throw;
        }
    }

    // ====== CONSULTAS COM FILTROS ======

    public async Task<LogQueryResult> GetLogsAsync(LogQueryFilter filter)
    {
        try
        {
            var query = _context.LogErros.AsQueryable();

            // Aplicar filtros
            if (filter.StartDate.HasValue)
                query = query.Where(l => l.DataHora >= filter.StartDate.Value);

            if (filter.EndDate.HasValue)
                query = query.Where(l => l.DataHora <= filter.EndDate.Value);

            if (!string.IsNullOrEmpty(filter.Tipo))
            {
                if (filter.Tipo == "CONSOLE")
                    query = query.Where(l => l.Tipo.StartsWith("CONSOLE-"));
                else if (filter.Tipo == "ERROR")
                    query = query.Where(l => l.Tipo == "ERROR" || l.Tipo == "OPERATION-FAIL");
                else
                    query = query.Where(l => l.Tipo == filter.Tipo);
            }

            if (!string.IsNullOrEmpty(filter.Origem))
                query = query.Where(l => l.Origem == filter.Origem);

            if (!string.IsNullOrEmpty(filter.Nivel))
                query = query.Where(l => l.Nivel == filter.Nivel);

            if (!string.IsNullOrEmpty(filter.Usuario))
                query = query.Where(l => l.Usuario != null && l.Usuario.Contains(filter.Usuario));

            if (!string.IsNullOrEmpty(filter.Url))
                query = query.Where(l => l.Url != null && l.Url.Contains(filter.Url));

            if (!string.IsNullOrEmpty(filter.SearchTerm))
            {
                var term = filter.SearchTerm.ToLower();
                query = query.Where(l =>
                    l.Mensagem.ToLower().Contains(term) ||
                    (l.Arquivo != null && l.Arquivo.ToLower().Contains(term)) ||
                    (l.Metodo != null && l.Metodo.ToLower().Contains(term)) ||
                    (l.StackTrace != null && l.StackTrace.ToLower().Contains(term)));
            }

            if (filter.ApenasNaoResolvidos == true)
                query = query.Where(l => l.Resolvido == false);

            // Contar total
            var totalCount = await query.CountAsync();

            // Ordenação
            query = filter.OrderDesc
                ? query.OrderByDescending(l => l.DataHora)
                : query.OrderBy(l => l.DataHora);

            // Paginação
            var logs = await query
                .Skip((filter.Page - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync();

            return new LogQueryResult
            {
                Logs = logs,
                TotalCount = totalCount,
                Page = filter.Page,
                PageSize = filter.PageSize
            };
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[LogRepository] Erro ao buscar logs com filtros: {ex.Message}");
            throw;
        }
    }

    public async Task<List<LogErro>> GetByDateAsync(DateTime date)
    {
        try
        {
            return await _context.LogErros
                .Where(l => l.DataHora.Date == date.Date)
                .OrderByDescending(l => l.DataHora)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[LogRepository] Erro ao buscar logs por data: {ex.Message}");
            throw;
        }
    }

    public async Task<List<LogErro>> GetByPeriodAsync(DateTime startDate, DateTime endDate)
    {
        try
        {
            return await _context.LogErros
                .Where(l => l.DataHora >= startDate && l.DataHora <= endDate)
                .OrderByDescending(l => l.DataHora)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[LogRepository] Erro ao buscar logs por período: {ex.Message}");
            throw;
        }
    }

    public async Task<List<LogErro>> GetByTypeAsync(string tipo, int limit = 100)
    {
        try
        {
            return await _context.LogErros
                .Where(l => l.Tipo == tipo)
                .OrderByDescending(l => l.DataHora)
                .Take(limit)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[LogRepository] Erro ao buscar logs por tipo: {ex.Message}");
            throw;
        }
    }

    public async Task<List<LogErro>> GetByUserAsync(string usuario, int limit = 100)
    {
        try
        {
            return await _context.LogErros
                .Where(l => l.Usuario != null && l.Usuario.Contains(usuario))
                .OrderByDescending(l => l.DataHora)
                .Take(limit)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[LogRepository] Erro ao buscar logs por usuário: {ex.Message}");
            throw;
        }
    }

    public async Task<List<LogErro>> GetByUrlAsync(string url, int limit = 100)
    {
        try
        {
            return await _context.LogErros
                .Where(l => l.Url != null && l.Url.Contains(url))
                .OrderByDescending(l => l.DataHora)
                .Take(limit)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[LogRepository] Erro ao buscar logs por URL: {ex.Message}");
            throw;
        }
    }

    public async Task<List<LogErro>> SearchAsync(string searchTerm, int limit = 100)
    {
        try
        {
            var term = searchTerm.ToLower();
            return await _context.LogErros
                .Where(l =>
                    l.Mensagem.ToLower().Contains(term) ||
                    (l.Arquivo != null && l.Arquivo.ToLower().Contains(term)) ||
                    (l.Metodo != null && l.Metodo.ToLower().Contains(term)))
                .OrderByDescending(l => l.DataHora)
                .Take(limit)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[LogRepository] Erro na busca de logs: {ex.Message}");
            throw;
        }
    }

    // ====== DASHBOARD E MÉTRICAS ======

    public async Task<LogDashboardStats> GetDashboardStatsAsync(DateTime? startDate = null, DateTime? endDate = null)
    {
        try
        {
            var query = _context.LogErros.AsQueryable();

            if (startDate.HasValue)
                query = query.Where(l => l.DataHora >= startDate.Value);
            if (endDate.HasValue)
                query = query.Where(l => l.DataHora <= endDate.Value);

            var hoje = DateTime.Today;

            var stats = new LogDashboardStats
            {
                TotalLogs = await query.CountAsync(),
                TotalErros = await query.CountAsync(l => l.Tipo == "ERROR" || l.Tipo == "OPERATION-FAIL"),
                TotalWarnings = await query.CountAsync(l => l.Tipo == "WARN"),
                TotalInfo = await query.CountAsync(l => l.Tipo == "INFO" || l.Tipo == "USER" || l.Tipo == "OPERATION"),
                TotalJsErrors = await query.CountAsync(l => l.Tipo == "ERROR-JS"),
                TotalHttpErrors = await query.CountAsync(l => l.Tipo == "HTTP-ERROR"),
                TotalConsole = await query.CountAsync(l => l.Tipo.StartsWith("CONSOLE-")),
                TotalServidor = await query.CountAsync(l => l.Origem == "SERVER"),
                TotalCliente = await query.CountAsync(l => l.Origem == "CLIENT"),
                NaoResolvidos = await query.CountAsync(l => !l.Resolvido && (l.Tipo.Contains("ERROR") || l.Tipo == "OPERATION-FAIL")),
                ResolvidosHoje = await query.CountAsync(l => l.Resolvido && l.DataResolucao != null && l.DataResolucao.Value.Date == hoje),
                UltimoErro = await query.Where(l => l.Tipo.Contains("ERROR")).MaxAsync(l => (DateTime?)l.DataHora)
            };

            // Calcular médias
            if (startDate.HasValue && endDate.HasValue)
            {
                var dias = (endDate.Value - startDate.Value).TotalDays;
                var horas = (endDate.Value - startDate.Value).TotalHours;

                if (dias > 0) stats.MediaErrosPorDia = stats.TotalErros / dias;
                if (horas > 0) stats.MediaErrosPorHora = stats.TotalErros / horas;
            }

            return stats;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[LogRepository] Erro ao obter estatísticas do dashboard: {ex.Message}");
            throw;
        }
    }

    public async Task<List<LogTimelineItem>> GetErrorsByHourAsync(DateTime date)
    {
        try
        {
            var startDate = date.Date;
            var endDate = startDate.AddDays(1);

            var logs = await _context.LogErros
                .Where(l => l.DataHora >= startDate && l.DataHora < endDate)
                .ToListAsync();

            return Enumerable.Range(0, 24)
                .Select(h =>
                {
                    var horaLogs = logs.Where(l => l.DataHora.Hour == h).ToList();
                    return new LogTimelineItem
                    {
                        Data = startDate.AddHours(h),
                        Label = $"{h:D2}:00",
                        Total = horaLogs.Count,
                        Erros = horaLogs.Count(l => l.Tipo.Contains("ERROR") || l.Tipo == "OPERATION-FAIL"),
                        Warnings = horaLogs.Count(l => l.Tipo == "WARN"),
                        Info = horaLogs.Count(l => l.Tipo == "INFO" || l.Tipo == "USER")
                    };
                })
                .ToList();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[LogRepository] Erro ao obter erros por hora: {ex.Message}");
            throw;
        }
    }

    public async Task<List<LogTimelineItem>> GetErrorsByDayAsync(int days = 30)
    {
        try
        {
            var endDate = DateTime.Now;
            var startDate = endDate.AddDays(-days);

            var logs = await _context.LogErros
                .Where(l => l.DataHora >= startDate)
                .ToListAsync();

            return Enumerable.Range(0, days)
                .Select(d =>
                {
                    var dia = startDate.AddDays(d).Date;
                    var diaLogs = logs.Where(l => l.DataHora.Date == dia).ToList();
                    return new LogTimelineItem
                    {
                        Data = dia,
                        Label = dia.ToString("dd/MM"),
                        Total = diaLogs.Count,
                        Erros = diaLogs.Count(l => l.Tipo.Contains("ERROR") || l.Tipo == "OPERATION-FAIL"),
                        Warnings = diaLogs.Count(l => l.Tipo == "WARN"),
                        Info = diaLogs.Count(l => l.Tipo == "INFO" || l.Tipo == "USER")
                    };
                })
                .ToList();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[LogRepository] Erro ao obter erros por dia: {ex.Message}");
            throw;
        }
    }

    public async Task<List<LogDistributionItem>> GetDistributionByTypeAsync(DateTime? startDate = null, DateTime? endDate = null)
    {
        try
        {
            var query = _context.LogErros.AsQueryable();
            if (startDate.HasValue) query = query.Where(l => l.DataHora >= startDate.Value);
            if (endDate.HasValue) query = query.Where(l => l.DataHora <= endDate.Value);

            var total = await query.CountAsync();
            if (total == 0) return new List<LogDistributionItem>();

            var grupos = await query
                .GroupBy(l => l.Tipo)
                .Select(g => new { Tipo = g.Key, Count = g.Count() })
                .ToListAsync();

            var tipoConfig = new Dictionary<string, (string Color, string Icon)>
            {
                {"ERROR", ("#dc3545", "fa-circle-exclamation")},
                {"ERROR-JS", ("#6f42c1", "fa-js")},
                {"HTTP-ERROR", ("#fd7e14", "fa-globe")},
                {"WARN", ("#ffc107", "fa-triangle-exclamation")},
                {"INFO", ("#17a2b8", "fa-circle-info")},
                {"CONSOLE-INFO", ("#9333ea", "fa-browser")},
                {"CONSOLE-WARN", ("#9333ea", "fa-browser")},
                {"CONSOLE-ERROR", ("#9333ea", "fa-browser")},
            };

            return grupos.Select(g =>
            {
                var hasConfig = tipoConfig.TryGetValue(g.Tipo ?? "", out var config);
                return new LogDistributionItem
                {
                    Label = g.Tipo ?? "UNKNOWN",
                    Count = g.Count,
                    Percentage = Math.Round((double)g.Count / total * 100, 1),
                    Color = hasConfig ? config.Color : "#6c757d",
                    Icon = hasConfig ? config.Icon : "fa-file-lines"
                };
            })
            .OrderByDescending(x => x.Count)
            .ToList();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[LogRepository] Erro ao obter distribuição por tipo: {ex.Message}");
            throw;
        }
    }

    public async Task<List<LogDistributionItem>> GetDistributionByOriginAsync(DateTime? startDate = null, DateTime? endDate = null)
    {
        try
        {
            var query = _context.LogErros.AsQueryable();
            if (startDate.HasValue) query = query.Where(l => l.DataHora >= startDate.Value);
            if (endDate.HasValue) query = query.Where(l => l.DataHora <= endDate.Value);

            var total = await query.CountAsync();
            if (total == 0) return new List<LogDistributionItem>();

            var servidor = await query.CountAsync(l => l.Origem == "SERVER");
            var cliente = await query.CountAsync(l => l.Origem == "CLIENT");

            return new List<LogDistributionItem>
            {
                new()
                {
                    Label = "Servidor",
                    Count = servidor,
                    Percentage = Math.Round((double)servidor / total * 100, 1),
                    Color = "#3b82f6",
                    Icon = "fa-server"
                },
                new()
                {
                    Label = "Cliente",
                    Count = cliente,
                    Percentage = Math.Round((double)cliente / total * 100, 1),
                    Color = "#8b5cf6",
                    Icon = "fa-browser"
                }
            };
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[LogRepository] Erro ao obter distribuição por origem: {ex.Message}");
            throw;
        }
    }

    public async Task<List<LogRankingItem>> GetTopPagesWithErrorsAsync(int top = 10, int days = 7)
    {
        try
        {
            var startDate = DateTime.Now.AddDays(-days);
            var total = await _context.LogErros
                .Where(l => l.DataHora >= startDate && l.Url != null)
                .CountAsync();

            var grupos = await _context.LogErros
                .Where(l => l.DataHora >= startDate && l.Url != null && l.Tipo.Contains("ERROR"))
                .GroupBy(l => l.Url)
                .Select(g => new
                {
                    Url = g.Key,
                    Count = g.Count(),
                    Arquivo = g.First().Arquivo,
                    Metodo = g.First().Metodo
                })
                .OrderByDescending(g => g.Count)
                .Take(top)
                .ToListAsync();

            return grupos.Select((g, i) =>
            {
                var label = FormatPagePath(g.Url, g.Arquivo, g.Metodo);

                return new LogRankingItem
                {
                    Posicao = i + 1,
                    Label = label,
                    SubLabel = g.Url ?? "(Sem URL)",
                    Count = g.Count,
                    Percentage = total > 0 ? Math.Round((double)g.Count / total * 100, 1) : 0,
                    Icon = "fa-file-code",
                    Color = "#dc3545"
                };
            }).ToList();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[LogRepository] Erro ao obter top páginas: {ex.Message}");
            throw;
        }
    }

    public async Task<List<LogRankingItem>> GetTopUsersWithErrorsAsync(int top = 10, int days = 7)
    {
        try
        {
            var startDate = DateTime.Now.AddDays(-days);
            var total = await _context.LogErros
                .Where(l => l.DataHora >= startDate && l.Usuario != null)
                .CountAsync();

            var grupos = await _context.LogErros
                .Where(l => l.DataHora >= startDate && l.Usuario != null && l.Tipo.Contains("ERROR"))
                .GroupBy(l => l.Usuario)
                .Select(g => new { Usuario = g.Key, Count = g.Count() })
                .OrderByDescending(g => g.Count)
                .Take(top)
                .ToListAsync();

            return grupos.Select((g, i) => new LogRankingItem
            {
                Posicao = i + 1,
                Label = g.Usuario ?? "(Anônimo)",
                SubLabel = $"{g.Count} erros",
                Count = g.Count,
                Percentage = total > 0 ? Math.Round((double)g.Count / total * 100, 1) : 0,
                Icon = "fa-user",
                Color = "#f59e0b"
            }).ToList();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[LogRepository] Erro ao obter top usuários: {ex.Message}");
            throw;
        }
    }

    public async Task<List<LogRankingItem>> GetMostFrequentErrorsAsync(int top = 10, int days = 7)
    {
        try
        {
            var startDate = DateTime.Now.AddDays(-days);

            var grupos = await _context.LogErros
                .Where(l => l.DataHora >= startDate && l.Tipo.Contains("ERROR") && l.HashErro != null)
                .GroupBy(l => new { l.HashErro, l.Mensagem, l.Arquivo, l.Linha })
                .Select(g => new
                {
                    g.Key.HashErro,
                    MensagemCurta = g.Key.Mensagem.Length > 100 ? g.Key.Mensagem.Substring(0, 100) + "..." : g.Key.Mensagem,
                    g.Key.Arquivo,
                    g.Key.Linha,
                    Count = g.Count()
                })
                .OrderByDescending(g => g.Count)
                .Take(top)
                .ToListAsync();

            return grupos.Select((g, i) => new LogRankingItem
            {
                Posicao = i + 1,
                Label = g.MensagemCurta,
                SubLabel = $"{g.Arquivo}:{g.Linha}",
                Count = g.Count,
                HashErro = g.HashErro,
                Icon = "fa-bug",
                Color = "#ef4444"
            }).ToList();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[LogRepository] Erro ao obter erros mais frequentes: {ex.Message}");
            throw;
        }
    }

    public async Task<LogComparison> GetComparisonWithPreviousPeriodAsync(int days = 7)
    {
        try
        {
            var hoje = DateTime.Now;
            var inicioAtual = hoje.AddDays(-days);
            var inicioAnterior = inicioAtual.AddDays(-days);

            var countAtual = await _context.LogErros
                .Where(l => l.DataHora >= inicioAtual && l.Tipo.Contains("ERROR"))
                .CountAsync();

            var countAnterior = await _context.LogErros
                .Where(l => l.DataHora >= inicioAnterior && l.DataHora < inicioAtual && l.Tipo.Contains("ERROR"))
                .CountAsync();

            var variacao = countAnterior > 0
                ? Math.Round(((double)countAtual - countAnterior) / countAnterior * 100, 1)
                : (countAtual > 0 ? 100.0 : 0.0);

            string tendencia, icone, cor;
            if (variacao > 5)
            {
                tendencia = "up";
                icone = "fa-arrow-trend-up";
                cor = "#dc3545"; // Vermelho (mais erros = ruim)
            }
            else if (variacao < -5)
            {
                tendencia = "down";
                icone = "fa-arrow-trend-down";
                cor = "#22c55e"; // Verde (menos erros = bom)
            }
            else
            {
                tendencia = "stable";
                icone = "fa-minus";
                cor = "#6b7280";
            }

            return new LogComparison
            {
                PeriodoAtual = countAtual,
                PeriodoAnterior = countAnterior,
                VariacaoPercentual = variacao,
                Tendencia = tendencia,
                TendenciaIcone = icone,
                TendenciaCor = cor
            };
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[LogRepository] Erro ao obter comparativo: {ex.Message}");
            throw;
        }
    }

    // ====== ANÁLISE DE PADRÕES ======

    public async Task<List<LogErro>> GetSimilarErrorsAsync(string hashErro, int limit = 50)
    {
        try
        {
            return await _context.LogErros
                .Where(l => l.HashErro == hashErro)
                .OrderByDescending(l => l.DataHora)
                .Take(limit)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[LogRepository] Erro ao buscar erros similares: {ex.Message}");
            throw;
        }
    }

    public async Task<List<LogTrendItem>> GetErrorTrendsAsync(int days = 14)
    {
        try
        {
            var metadeDias = days / 2;
            var hoje = DateTime.Now;
            var inicioAtual = hoje.AddDays(-metadeDias);
            var inicioAnterior = inicioAtual.AddDays(-metadeDias);

            var tipos = new[] { "ERROR", "ERROR-JS", "HTTP-ERROR", "WARN" };
            var trends = new List<LogTrendItem>();

            foreach (var tipo in tipos)
            {
                var countAtual = await _context.LogErros
                    .Where(l => l.DataHora >= inicioAtual &&
                                (tipo == "ERROR" ? (l.Tipo == "ERROR" || l.Tipo == "OPERATION-FAIL") : l.Tipo == tipo))
                    .CountAsync();

                var countAnterior = await _context.LogErros
                    .Where(l => l.DataHora >= inicioAnterior && l.DataHora < inicioAtual &&
                                (tipo == "ERROR" ? (l.Tipo == "ERROR" || l.Tipo == "OPERATION-FAIL") : l.Tipo == tipo))
                    .CountAsync();

                var variacao = countAnterior > 0
                    ? Math.Round(((double)countAtual - countAnterior) / countAnterior * 100, 1)
                    : (countAtual > 0 ? 100.0 : 0.0);

                trends.Add(new LogTrendItem
                {
                    Tipo = tipo,
                    CountAtual = countAtual,
                    CountAnterior = countAnterior,
                    VariacaoPercentual = variacao,
                    Tendencia = variacao > 5 ? "up" : (variacao < -5 ? "down" : "stable")
                });
            }

            return trends;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[LogRepository] Erro ao obter tendências: {ex.Message}");
            throw;
        }
    }

    public async Task<List<LogHourAnalysis>> GetPeakHoursAsync(int days = 30)
    {
        try
        {
            var startDate = DateTime.Now.AddDays(-days);

            var logs = await _context.LogErros
                .Where(l => l.DataHora >= startDate && l.Tipo.Contains("ERROR"))
                .ToListAsync();

            var porHora = logs
                .GroupBy(l => l.DataHora.Hour)
                .Select(g => new
                {
                    Hora = g.Key,
                    Total = g.Count(),
                    Media = (double)g.Count() / days
                })
                .ToList();

            var mediaGeral = porHora.Average(h => h.Media);
            var desvioPadrao = Math.Sqrt(porHora.Average(h => Math.Pow(h.Media - mediaGeral, 2)));

            return Enumerable.Range(0, 24)
                .Select(h =>
                {
                    var dados = porHora.FirstOrDefault(p => p.Hora == h);
                    var media = dados?.Media ?? 0;
                    return new LogHourAnalysis
                    {
                        Hora = h,
                        HoraFormatada = $"{h:D2}:00",
                        TotalErros = dados?.Total ?? 0,
                        MediaErros = Math.Round(media, 2),
                        IsPico = media > mediaGeral + desvioPadrao
                    };
                })
                .ToList();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[LogRepository] Erro ao obter horários de pico: {ex.Message}");
            throw;
        }
    }

    public async Task<List<LogAnomaly>> DetectAnomaliesAsync(int days = 7, double threshold = 2.0)
    {
        try
        {
            var startDate = DateTime.Now.AddDays(-days);
            var anomalies = new List<LogAnomaly>();

            // Agrupa erros por hora
            var logs = await _context.LogErros
                .Where(l => l.DataHora >= startDate && l.Tipo.Contains("ERROR"))
                .ToListAsync();

            var porHora = logs
                .GroupBy(l => new { l.DataHora.Date, l.DataHora.Hour })
                .Select(g => new { DataHora = g.Key.Date.AddHours(g.Key.Hour), Count = g.Count() })
                .OrderBy(g => g.DataHora)
                .ToList();

            if (porHora.Count < 3) return anomalies;

            var media = porHora.Average(h => h.Count);
            var desvioPadrao = Math.Sqrt(porHora.Average(h => Math.Pow(h.Count - media, 2)));

            foreach (var hora in porHora)
            {
                var zScore = desvioPadrao > 0 ? (hora.Count - media) / desvioPadrao : 0;

                if (Math.Abs(zScore) > threshold)
                {
                    var severidade = Math.Abs(zScore) > 3 ? "high" : (Math.Abs(zScore) > 2 ? "medium" : "low");
                    anomalies.Add(new LogAnomaly
                    {
                        DataHora = hora.DataHora,
                        CountErros = hora.Count,
                        Media = Math.Round(media, 2),
                        DesvioPadrao = Math.Round(desvioPadrao, 2),
                        ZScore = Math.Round(zScore, 2),
                        Severidade = severidade,
                        Mensagem = zScore > 0
                            ? $"Pico de erros: {hora.Count} erros ({Math.Round((hora.Count - media) / media * 100)}% acima da média)"
                            : $"Queda atípica: {hora.Count} erros ({Math.Round((media - hora.Count) / media * 100)}% abaixo da média)"
                    });
                }
            }

            return anomalies.OrderByDescending(a => a.DataHora).ToList();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[LogRepository] Erro ao detectar anomalias: {ex.Message}");
            throw;
        }
    }

    // ====== ALERTAS E MONITORAMENTO ======

    public async Task<List<LogErro>> GetUnresolvedCriticalAsync(int limit = 100)
    {
        try
        {
            return await _context.LogErros
                .Where(l => !l.Resolvido && (l.Tipo == "ERROR" || l.Tipo == "OPERATION-FAIL" || l.Nivel == "Critical"))
                .OrderByDescending(l => l.DataHora)
                .Take(limit)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[LogRepository] Erro ao buscar erros críticos não resolvidos: {ex.Message}");
            throw;
        }
    }

    public async Task<List<LogThresholdAlert>> CheckThresholdsAsync(LogAlertConfig config)
    {
        try
        {
            var alerts = new List<LogThresholdAlert>();
            var agora = DateTime.Now;

            // Verificar erros na última hora
            var errosUltimaHora = await _context.LogErros
                .CountAsync(l => l.DataHora >= agora.AddHours(-1) && l.Tipo.Contains("ERROR"));

            if (errosUltimaHora > config.ThresholdErrosPorHora)
            {
                alerts.Add(new LogThresholdAlert
                {
                    Tipo = "ERROS_POR_HORA",
                    Descricao = $"Erros na última hora: {errosUltimaHora} (limite: {config.ThresholdErrosPorHora})",
                    ValorAtual = errosUltimaHora,
                    Threshold = config.ThresholdErrosPorHora,
                    PercentualExcedido = Math.Round((double)(errosUltimaHora - config.ThresholdErrosPorHora) / config.ThresholdErrosPorHora * 100, 1),
                    Severidade = errosUltimaHora > config.ThresholdErrosPorHora * 2 ? "high" : "medium",
                    DetectadoEm = agora
                });
            }

            // Verificar erros no último minuto
            var errosUltimoMinuto = await _context.LogErros
                .CountAsync(l => l.DataHora >= agora.AddMinutes(-1) && l.Tipo.Contains("ERROR"));

            if (errosUltimoMinuto > config.ThresholdErrosPorMinuto)
            {
                alerts.Add(new LogThresholdAlert
                {
                    Tipo = "ERROS_POR_MINUTO",
                    Descricao = $"Erros no último minuto: {errosUltimoMinuto} (limite: {config.ThresholdErrosPorMinuto})",
                    ValorAtual = errosUltimoMinuto,
                    Threshold = config.ThresholdErrosPorMinuto,
                    PercentualExcedido = Math.Round((double)(errosUltimoMinuto - config.ThresholdErrosPorMinuto) / config.ThresholdErrosPorMinuto * 100, 1),
                    Severidade = "high",
                    DetectadoEm = agora
                });
            }

            // Verificar erros críticos não resolvidos
            var errosCriticos = await _context.LogErros
                .CountAsync(l => !l.Resolvido && (l.Nivel == "Critical" || l.Tipo == "OPERATION-FAIL"));

            if (errosCriticos > config.ThresholdErrosCriticos)
            {
                alerts.Add(new LogThresholdAlert
                {
                    Tipo = "ERROS_CRITICOS",
                    Descricao = $"Erros críticos não resolvidos: {errosCriticos} (limite: {config.ThresholdErrosCriticos})",
                    ValorAtual = errosCriticos,
                    Threshold = config.ThresholdErrosCriticos,
                    PercentualExcedido = Math.Round((double)(errosCriticos - config.ThresholdErrosCriticos) / config.ThresholdErrosCriticos * 100, 1),
                    Severidade = "high",
                    DetectadoEm = agora
                });
            }

            return alerts;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[LogRepository] Erro ao verificar thresholds: {ex.Message}");
            throw;
        }
    }

    public async Task MarkAsResolvedAsync(long logErroId, string resolvidoPor, string? observacoes = null)
    {
        try
        {
            var log = await _context.LogErros.FindAsync(logErroId);
            if (log != null)
            {
                log.Resolvido = true;
                log.DataResolucao = DateTime.Now;
                log.ResolvidoPor = resolvidoPor;
                log.Observacoes = observacoes;
                await _context.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[LogRepository] Erro ao marcar como resolvido: {ex.Message}");
            throw;
        }
    }

    public async Task MarkSimilarAsResolvedAsync(string hashErro, string resolvidoPor, string? observacoes = null)
    {
        try
        {
            var logs = await _context.LogErros
                .Where(l => l.HashErro == hashErro && !l.Resolvido)
                .ToListAsync();

            foreach (var log in logs)
            {
                log.Resolvido = true;
                log.DataResolucao = DateTime.Now;
                log.ResolvidoPor = resolvidoPor;
                log.Observacoes = observacoes;
            }

            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[LogRepository] Erro ao marcar similares como resolvidos: {ex.Message}");
            throw;
        }
    }

    // ====== EXPORTAÇÃO ======

    public async Task<List<LogExportItem>> GetForExportAsync(LogQueryFilter filter)
    {
        try
        {
            var result = await GetLogsAsync(new LogQueryFilter
            {
                StartDate = filter.StartDate,
                EndDate = filter.EndDate,
                Tipo = filter.Tipo,
                Origem = filter.Origem,
                Usuario = filter.Usuario,
                Url = filter.Url,
                SearchTerm = filter.SearchTerm,
                Page = 1,
                PageSize = 10000 // Limite para exportação
            });

            return result.Logs.Select(l => new LogExportItem
            {
                Id = l.LogErroId,
                DataHora = l.DataHora.ToString("dd/MM/yyyy HH:mm:ss"),
                Tipo = l.Tipo,
                Origem = l.Origem,
                Nivel = l.Nivel ?? "",
                Mensagem = l.Mensagem,
                Arquivo = l.Arquivo ?? "",
                Metodo = l.Metodo ?? "",
                Linha = l.Linha?.ToString() ?? "",
                Usuario = l.Usuario ?? "",
                Url = l.Url ?? "",
                Resolvido = l.Resolvido ? "Sim" : "Não"
            }).ToList();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[LogRepository] Erro ao exportar logs: {ex.Message}");
            throw;
        }
    }

    public async Task<LogExecutiveReport> GetExecutiveReportAsync(DateTime startDate, DateTime endDate)
    {
        try
        {
            var report = new LogExecutiveReport
            {
                DataInicio = startDate,
                DataFim = endDate,
                TotalDias = (int)(endDate - startDate).TotalDays
            };

            // Estatísticas gerais
            var stats = await GetDashboardStatsAsync(startDate, endDate);
            report.TotalLogs = stats.TotalLogs;
            report.TotalErros = stats.TotalErros;
            report.MediaErrosPorDia = stats.MediaErrosPorDia;

            // Taxa de resolução
            var resolvidos = await _context.LogErros
                .CountAsync(l => l.DataHora >= startDate && l.DataHora <= endDate && l.Resolvido);
            report.TaxaResolucao = stats.TotalErros > 0 ? Math.Round((double)resolvidos / stats.TotalErros * 100, 1) : 100;

            // Comparativos
            report.ComparativoErros = await GetComparisonWithPreviousPeriodAsync(report.TotalDias);

            // Top problemas
            report.TopPaginas = await GetTopPagesWithErrorsAsync(5, report.TotalDias);
            report.TopErros = await GetMostFrequentErrorsAsync(5, report.TotalDias);
            report.TopUsuarios = await GetTopUsersWithErrorsAsync(5, report.TotalDias);

            // Distribuições
            report.DistribuicaoPorTipo = await GetDistributionByTypeAsync(startDate, endDate);
            report.DistribuicaoPorOrigem = await GetDistributionByOriginAsync(startDate, endDate);

            // Timeline
            report.ErrosPorDia = await GetErrorsByDayAsync(report.TotalDias);

            // Horários de pico
            report.HorariosPico = (await GetPeakHoursAsync(report.TotalDias)).Where(h => h.IsPico).ToList();

            // Alertas e anomalias
            var anomalias = await DetectAnomaliesAsync(report.TotalDias);
            report.AnomaliasPeriodo = anomalias.Count;

            return report;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[LogRepository] Erro ao gerar relatório executivo: {ex.Message}");
            throw;
        }
    }

    /****************************************************************************************
     * ⚡ FUNÇÃO: FormatPagePath
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Formatar path de página para exibição (path a partir de /Pages + método).
     *
     * 📥 ENTRADAS     : url (URL completa), arquivo (caminho do arquivo), metodo (nome do método).
     *
     * 📤 SAÍDAS       : String formatada (ex: "Viagens/Create.OnPostAsync").
     ****************************************************************************************/
    private string FormatPagePath(string? url, string? arquivo, string? metodo)
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
}
