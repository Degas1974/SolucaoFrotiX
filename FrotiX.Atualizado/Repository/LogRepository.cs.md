# Repository/LogRepository.cs

**ARQUIVO NOVO** | 993 linhas de codigo

> Copiar integralmente para o Janeiro.

---

```csharp
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FrotiX.Data;
using FrotiX.Models;
using FrotiX.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace FrotiX.Repository;

public class LogRepository : ILogRepository
{
    private readonly FrotiXDbContext _context;

    public LogRepository(FrotiXDbContext context)
    {
        _context = context;
    }

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

    public async Task<LogQueryResult> GetLogsAsync(LogQueryFilter filter)
    {
        try
        {
            var query = _context.LogErros.AsQueryable();

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

            var totalCount = await query.CountAsync();

            query = filter.OrderDesc
                ? query.OrderByDescending(l => l.DataHora)
                : query.OrderBy(l => l.DataHora);

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

... (+493 linhas)
```
