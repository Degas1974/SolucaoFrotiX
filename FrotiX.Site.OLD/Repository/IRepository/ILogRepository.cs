/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: ILogRepository.cs                                                                       ║
   ║ 📂 CAMINHO: /Repository/IRepository                                                                 ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO: Interface do repositório de logs de erros com métodos para CRUD, análises e métricas. ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 ÍNDICE DE FUNÇÕES:                                                                               ║
   ║ • CRUD: Add, GetById, GetAll, Delete, Update                                                        ║
   ║ • Filtros: GetByDate, GetByType, GetByUser, GetByUrl                                               ║
   ║ • Dashboard: GetStats, GetErrorsByHour, GetTopPages, GetTopUsers                                   ║
   ║ • Análises: GetSimilarErrors, GetTrends, GetAnomalies                                              ║
   ║ • Alertas: GetUnresolvedCritical, GetThresholdExceeded                                             ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📝 VERSÃO: 1.0 | DATA: 31/01/2026 | AUTOR: Claude Code                                             ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝
*/

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FrotiX.Models;

namespace FrotiX.Repository.IRepository;

/// <summary>
/// Interface do repositório de logs de erros
/// </summary>
public interface ILogRepository
{
    // ====== CRUD BÁSICO ======

    /// <summary>Adiciona um novo log de erro</summary>
    Task<LogErro> AddAsync(LogErro logErro);

    /// <summary>Adiciona múltiplos logs em batch (para performance)</summary>
    Task AddRangeAsync(IEnumerable<LogErro> logErros);

    /// <summary>Obtém log por ID</summary>
    Task<LogErro?> GetByIdAsync(long id);

    /// <summary>Atualiza um log existente</summary>
    Task UpdateAsync(LogErro logErro);

    /// <summary>Remove um log por ID</summary>
    Task DeleteAsync(long id);

    /// <summary>Remove logs anteriores a uma data (limpeza)</summary>
    Task<int> DeleteBeforeDateAsync(DateTime date);

    // ====== CONSULTAS COM FILTROS ======

    /// <summary>Obtém logs com filtros e paginação</summary>
    Task<LogQueryResult> GetLogsAsync(LogQueryFilter filter);

    /// <summary>Obtém logs por data específica</summary>
    Task<List<LogErro>> GetByDateAsync(DateTime date);

    /// <summary>Obtém logs por período</summary>
    Task<List<LogErro>> GetByPeriodAsync(DateTime startDate, DateTime endDate);

    /// <summary>Obtém logs por tipo (ERROR, WARN, INFO, etc.)</summary>
    Task<List<LogErro>> GetByTypeAsync(string tipo, int limit = 100);

    /// <summary>Obtém logs por usuário</summary>
    Task<List<LogErro>> GetByUserAsync(string usuario, int limit = 100);

    /// <summary>Obtém logs por URL</summary>
    Task<List<LogErro>> GetByUrlAsync(string url, int limit = 100);

    /// <summary>Busca logs por texto (mensagem, arquivo, stack)</summary>
    Task<List<LogErro>> SearchAsync(string searchTerm, int limit = 100);

    // ====== DASHBOARD E MÉTRICAS ======

    /// <summary>Obtém estatísticas gerais (contadores por tipo)</summary>
    Task<LogDashboardStats> GetDashboardStatsAsync(DateTime? startDate = null, DateTime? endDate = null);

    /// <summary>Obtém erros agrupados por hora (para gráfico de linha)</summary>
    Task<List<LogTimelineItem>> GetErrorsByHourAsync(DateTime date);

    /// <summary>Obtém erros agrupados por dia (para gráfico de barras)</summary>
    Task<List<LogTimelineItem>> GetErrorsByDayAsync(int days = 30);

    /// <summary>Obtém distribuição por tipo (para gráfico de pizza)</summary>
    Task<List<LogDistributionItem>> GetDistributionByTypeAsync(DateTime? startDate = null, DateTime? endDate = null);

    /// <summary>Obtém distribuição por origem (SERVER vs CLIENT)</summary>
    Task<List<LogDistributionItem>> GetDistributionByOriginAsync(DateTime? startDate = null, DateTime? endDate = null);

    /// <summary>Obtém top 10 páginas com mais erros</summary>
    Task<List<LogRankingItem>> GetTopPagesWithErrorsAsync(int top = 10, int days = 7);

    /// <summary>Obtém top 10 usuários com mais erros</summary>
    Task<List<LogRankingItem>> GetTopUsersWithErrorsAsync(int top = 10, int days = 7);

    /// <summary>Obtém top 10 erros mais frequentes (agrupados por hash)</summary>
    Task<List<LogRankingItem>> GetMostFrequentErrorsAsync(int top = 10, int days = 7);

    /// <summary>Obtém comparativo com período anterior</summary>
    Task<LogComparison> GetComparisonWithPreviousPeriodAsync(int days = 7);

    // ====== ANÁLISE DE PADRÕES ======

    /// <summary>Obtém erros similares (mesmo hash)</summary>
    Task<List<LogErro>> GetSimilarErrorsAsync(string hashErro, int limit = 50);

    /// <summary>Obtém tendências de erros (crescente/decrescente)</summary>
    Task<List<LogTrendItem>> GetErrorTrendsAsync(int days = 14);

    /// <summary>Obtém horários com mais erros (análise de sazonalidade)</summary>
    Task<List<LogHourAnalysis>> GetPeakHoursAsync(int days = 30);

    /// <summary>Detecta anomalias (picos de erros)</summary>
    Task<List<LogAnomaly>> DetectAnomaliesAsync(int days = 7, double threshold = 2.0);

    // ====== ALERTAS E MONITORAMENTO ======

    /// <summary>Obtém erros críticos não resolvidos</summary>
    Task<List<LogErro>> GetUnresolvedCriticalAsync(int limit = 100);

    /// <summary>Verifica se threshold de erros foi excedido</summary>
    Task<List<LogThresholdAlert>> CheckThresholdsAsync(LogAlertConfig config);

    /// <summary>Marca erro como resolvido</summary>
    Task MarkAsResolvedAsync(long logErroId, string resolvidoPor, string? observacoes = null);

    /// <summary>Marca múltiplos erros similares como resolvidos</summary>
    Task MarkSimilarAsResolvedAsync(string hashErro, string resolvidoPor, string? observacoes = null);

    // ====== EXPORTAÇÃO ======

    /// <summary>Obtém logs formatados para exportação Excel</summary>
    Task<List<LogExportItem>> GetForExportAsync(LogQueryFilter filter);

    /// <summary>Obtém resumo para relatório gerencial</summary>
    Task<LogExecutiveReport> GetExecutiveReportAsync(DateTime startDate, DateTime endDate);
}

// ====== DTOs PARA CONSULTAS E RESPOSTAS ======

/// <summary>Filtro de consulta de logs</summary>
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

/// <summary>Resultado paginado de logs</summary>
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

/// <summary>Estatísticas para Dashboard</summary>
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

/// <summary>Item de timeline (erros por hora/dia)</summary>
public class LogTimelineItem
{
    public DateTime Data { get; set; }
    public string Label { get; set; } = "";
    public int Total { get; set; }
    public int Erros { get; set; }
    public int Warnings { get; set; }
    public int Info { get; set; }
}

/// <summary>Item de distribuição (para gráfico de pizza)</summary>
public class LogDistributionItem
{
    public string Label { get; set; } = "";
    public int Count { get; set; }
    public double Percentage { get; set; }
    public string Color { get; set; } = "";
    public string Icon { get; set; } = "";
}

/// <summary>Item de ranking (top N)</summary>
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

/// <summary>Comparativo com período anterior</summary>
public class LogComparison
{
    public int PeriodoAtual { get; set; }
    public int PeriodoAnterior { get; set; }
    public double VariacaoPercentual { get; set; }
    public string Tendencia { get; set; } = ""; // "up", "down", "stable"
    public string TendenciaIcone { get; set; } = "";
    public string TendenciaCor { get; set; } = "";
}

/// <summary>Item de tendência</summary>
public class LogTrendItem
{
    public string Tipo { get; set; } = "";
    public int CountAtual { get; set; }
    public int CountAnterior { get; set; }
    public double VariacaoPercentual { get; set; }
    public string Tendencia { get; set; } = "";
}

/// <summary>Análise de horários de pico</summary>
public class LogHourAnalysis
{
    public int Hora { get; set; }
    public string HoraFormatada { get; set; } = "";
    public int TotalErros { get; set; }
    public double MediaErros { get; set; }
    public bool IsPico { get; set; }
}

/// <summary>Anomalia detectada</summary>
public class LogAnomaly
{
    public DateTime DataHora { get; set; }
    public int CountErros { get; set; }
    public double Media { get; set; }
    public double DesvioPadrao { get; set; }
    public double ZScore { get; set; }
    public string Severidade { get; set; } = ""; // "low", "medium", "high"
    public string Mensagem { get; set; } = "";
}

/// <summary>Alerta de threshold excedido</summary>
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

/// <summary>Configuração de alertas</summary>
public class LogAlertConfig
{
    public int ThresholdErrosPorHora { get; set; } = 50;
    public int ThresholdErrosPorMinuto { get; set; } = 10;
    public int ThresholdErrosCriticos { get; set; } = 5;
    public int ThresholdMesmoErro { get; set; } = 20;
    public bool AlertarNovosErros { get; set; } = true;
    public bool AlertarPicos { get; set; } = true;
}

/// <summary>Item para exportação Excel</summary>
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

/// <summary>Relatório executivo</summary>
public class LogExecutiveReport
{
    public DateTime DataInicio { get; set; }
    public DateTime DataFim { get; set; }
    public int TotalDias { get; set; }

    // Resumo
    public int TotalLogs { get; set; }
    public int TotalErros { get; set; }
    public double MediaErrosPorDia { get; set; }
    public double TaxaResolucao { get; set; }

    // Comparativo
    public LogComparison ComparativoErros { get; set; } = new();
    public LogComparison ComparativoWarnings { get; set; } = new();

    // Top problemas
    public List<LogRankingItem> TopPaginas { get; set; } = new();
    public List<LogRankingItem> TopErros { get; set; } = new();
    public List<LogRankingItem> TopUsuarios { get; set; } = new();

    // Distribuições
    public List<LogDistributionItem> DistribuicaoPorTipo { get; set; } = new();
    public List<LogDistributionItem> DistribuicaoPorOrigem { get; set; } = new();

    // Timeline
    public List<LogTimelineItem> ErrosPorDia { get; set; } = new();

    // Horários de pico
    public List<LogHourAnalysis> HorariosPico { get; set; } = new();

    // Alertas
    public int AlertasGerados { get; set; }
    public int AnomaliasPeriodo { get; set; }
}
