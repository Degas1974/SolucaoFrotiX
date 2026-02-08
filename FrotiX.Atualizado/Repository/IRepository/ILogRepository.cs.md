# Repository/IRepository/ILogRepository.cs

**ARQUIVO NOVO** | 201 linhas de codigo

> Copiar integralmente para o Janeiro.

---

```csharp
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FrotiX.Models;

namespace FrotiX.Repository.IRepository;

public interface ILogRepository
{

    Task<LogErro> AddAsync(LogErro logErro);

    Task AddRangeAsync(IEnumerable<LogErro> logErros);

    Task<LogErro?> GetByIdAsync(long id);

    Task UpdateAsync(LogErro logErro);

    Task DeleteAsync(long id);

    Task<int> DeleteBeforeDateAsync(DateTime date);

    Task<LogQueryResult> GetLogsAsync(LogQueryFilter filter);

    Task<List<LogErro>> GetByDateAsync(DateTime date);

    Task<List<LogErro>> GetByPeriodAsync(DateTime startDate, DateTime endDate);

    Task<List<LogErro>> GetByTypeAsync(string tipo, int limit = 100);

    Task<List<LogErro>> GetByUserAsync(string usuario, int limit = 100);

    Task<List<LogErro>> GetByUrlAsync(string url, int limit = 100);

    Task<List<LogErro>> SearchAsync(string searchTerm, int limit = 100);

    Task<LogDashboardStats> GetDashboardStatsAsync(DateTime? startDate = null, DateTime? endDate = null);

    Task<List<LogTimelineItem>> GetErrorsByHourAsync(DateTime date);

    Task<List<LogTimelineItem>> GetErrorsByDayAsync(int days = 30);

    Task<List<LogDistributionItem>> GetDistributionByTypeAsync(DateTime? startDate = null, DateTime? endDate = null);

    Task<List<LogDistributionItem>> GetDistributionByOriginAsync(DateTime? startDate = null, DateTime? endDate = null);

    Task<List<LogRankingItem>> GetTopPagesWithErrorsAsync(int top = 10, int days = 7);

    Task<List<LogRankingItem>> GetTopUsersWithErrorsAsync(int top = 10, int days = 7);

    Task<List<LogRankingItem>> GetMostFrequentErrorsAsync(int top = 10, int days = 7);

    Task<LogComparison> GetComparisonWithPreviousPeriodAsync(int days = 7);

    Task<List<LogErro>> GetSimilarErrorsAsync(string hashErro, int limit = 50);

    Task<List<LogTrendItem>> GetErrorTrendsAsync(int days = 14);

    Task<List<LogHourAnalysis>> GetPeakHoursAsync(int days = 30);

    Task<List<LogAnomaly>> DetectAnomaliesAsync(int days = 7, double threshold = 2.0);

    Task<List<LogErro>> GetUnresolvedCriticalAsync(int limit = 100);

    Task<List<LogThresholdAlert>> CheckThresholdsAsync(LogAlertConfig config);

    Task MarkAsResolvedAsync(long logErroId, string resolvidoPor, string? observacoes = null);

    Task MarkSimilarAsResolvedAsync(string hashErro, string resolvidoPor, string? observacoes = null);

    Task<List<LogExportItem>> GetForExportAsync(LogQueryFilter filter);

    Task<LogExecutiveReport> GetExecutiveReportAsync(DateTime startDate, DateTime endDate);
}

public class LogQueryFilter
{
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Tipo { get; set; }
    public string? Origem { get; set; }
    public string? Nivel { get; set; }
    public string? Usuario { get; set; }
    public string? Url { get; set; }
    public string? SearchTerm { get; set; }
    public bool? ApenasNaoResolvidos { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 50;
    public string OrderBy { get; set; } = "DataHora";
    public bool OrderDesc { get; set; } = true;
}

public class LogQueryResult
{
    public List<LogErro> Logs { get; set; } = new();
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
    public bool HasNextPage => Page < TotalPages;
    public bool HasPreviousPage => Page > 1;
}

public class LogDashboardStats
{
    public int TotalLogs { get; set; }
    public int TotalErros { get; set; }
    public int TotalWarnings { get; set; }
    public int TotalInfo { get; set; }
    public int TotalJsErrors { get; set; }
    public int TotalHttpErrors { get; set; }
    public int TotalConsole { get; set; }
    public int TotalServidor { get; set; }
    public int TotalCliente { get; set; }
    public int NaoResolvidos { get; set; }
    public int ResolvidosHoje { get; set; }
    public DateTime? UltimoErro { get; set; }
    public double MediaErrosPorHora { get; set; }
    public double MediaErrosPorDia { get; set; }
}

public class LogTimelineItem
{
    public DateTime Data { get; set; }
    public string Label { get; set; } = "";
    public int Total { get; set; }
    public int Erros { get; set; }
    public int Warnings { get; set; }
    public int Info { get; set; }
}

public class LogDistributionItem
{
    public string Label { get; set; } = "";
    public int Count { get; set; }
    public double Percentage { get; set; }
    public string Color { get; set; } = "";
    public string Icon { get; set; } = "";
}

public class LogRankingItem
{
    public int Posicao { get; set; }
    public string Label { get; set; } = "";
    public string? SubLabel { get; set; }
    public int Count { get; set; }
    public double Percentage { get; set; }
    public string? HashErro { get; set; }
    public string Icon { get; set; } = "";
    public string Color { get; set; } = "";
}

public class LogComparison
{
    public int PeriodoAtual { get; set; }
    public int PeriodoAnterior { get; set; }
    public double VariacaoPercentual { get; set; }
    public string Tendencia { get; set; } = "";
    public string TendenciaIcone { get; set; } = "";
    public string TendenciaCor { get; set; } = "";
}

public class LogTrendItem
{
    public string Tipo { get; set; } = "";
    public int CountAtual { get; set; }
    public int CountAnterior { get; set; }
    public double VariacaoPercentual { get; set; }
    public string Tendencia { get; set; } = "";
}

public class LogHourAnalysis
{
    public int Hora { get; set; }
    public string HoraFormatada { get; set; } = "";
    public int TotalErros { get; set; }
    public double MediaErros { get; set; }
    public bool IsPico { get; set; }
}

public class LogAnomaly
{
    public DateTime DataHora { get; set; }
    public int CountErros { get; set; }
    public double Media { get; set; }
    public double DesvioPadrao { get; set; }
    public double ZScore { get; set; }
    public string Severidade { get; set; } = "";
    public string Mensagem { get; set; } = "";
}

public class LogThresholdAlert
{
    public string Tipo { get; set; } = "";
    public string Descricao { get; set; } = "";
    public int ValorAtual { get; set; }
    public int Threshold { get; set; }
    public double PercentualExcedido { get; set; }
    public string Severidade { get; set; } = "";
    public DateTime DetectadoEm { get; set; }
}

public class LogAlertConfig
{
    public int ThresholdErrosPorHora { get; set; } = 50;
    public int ThresholdErrosPorMinuto { get; set; } = 10;
    public int ThresholdErrosCriticos { get; set; } = 5;
    public int ThresholdMesmoErro { get; set; } = 20;
    public bool AlertarNovosErros { get; set; } = true;
    public bool AlertarPicos { get; set; } = true;
}

public class LogExportItem
{
    public long Id { get; set; }
    public string DataHora { get; set; } = "";
    public string Tipo { get; set; } = "";
    public string Origem { get; set; } = "";
    public string Nivel { get; set; } = "";
    public string Mensagem { get; set; } = "";
    public string Arquivo { get; set; } = "";
    public string Metodo { get; set; } = "";
    public string Linha { get; set; } = "";
    public string Usuario { get; set; } = "";
    public string Url { get; set; } = "";
    public string Resolvido { get; set; } = "";
}

public class LogExecutiveReport
{
    public DateTime DataInicio { get; set; }
    public DateTime DataFim { get; set; }
    public int TotalDias { get; set; }

    public int TotalLogs { get; set; }
    public int TotalErros { get; set; }
    public double MediaErrosPorDia { get; set; }
    public double TaxaResolucao { get; set; }

    public LogComparison ComparativoErros { get; set; } = new();
    public LogComparison ComparativoWarnings { get; set; } = new();

    public List<LogRankingItem> TopPaginas { get; set; } = new();
    public List<LogRankingItem> TopErros { get; set; } = new();
    public List<LogRankingItem> TopUsuarios { get; set; } = new();

    public List<LogDistributionItem> DistribuicaoPorTipo { get; set; } = new();
    public List<LogDistributionItem> DistribuicaoPorOrigem { get; set; } = new();

    public List<LogTimelineItem> ErrosPorDia { get; set; } = new();

    public List<LogHourAnalysis> HorariosPico { get; set; } = new();

    public int AlertasGerados { get; set; }
    public int AnomaliasPeriodo { get; set; }
}
```
