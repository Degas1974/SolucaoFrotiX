/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: LogService.cs                                                                           ║
   ║ 📂 CAMINHO: /Services                                                                               ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO: Implementação ILogService com persistência em banco de dados (SQL Server).             ║
   ║              Mantém fallback em TXT para resiliência quando banco não estiver disponível.           ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 CARACTERÍSTICAS:                                                                                 ║
   ║ • Gravação primária no banco de dados via ILogRepository                                            ║
   ║ • Fallback automático para arquivo TXT em caso de falha no banco                                   ║
   ║ • Gravação assíncrona fire-and-forget para não bloquear operações                                  ║
   ║ • Buffer com retry para logs que falharam                                                           ║
   ║ • Estatísticas em tempo real do banco de dados                                                      ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🔗 DEPS: ILogRepository, IWebHostEnvironment, IHttpContextAccessor                                  ║
   ║ 📅 31/01/2026 | 👤 Claude Code | 📝 v3.0                                                            ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝
*/

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using FrotiX.Models;
using FrotiX.Repository.IRepository;

namespace FrotiX.Services;

/// <summary>
/// ╭───────────────────────────────────────────────────────────────────────────────────────╮
/// │ ⚡ SERVICE: LogService                                                                │
/// │───────────────────────────────────────────────────────────────────────────────────────│
/// │ 🎯 DESCRIÇÃO: Serviço de logging centralizado com gravação em banco de dados          │
/// │              e fallback em arquivo TXT para resiliência.                              │
/// │───────────────────────────────────────────────────────────────────────────────────────│
/// │ 🔗 RASTREABILIDADE:                                                                   │
/// │    ⬅️ USADO POR : Controllers, Services, Pages, Middlewares, Filters                 │
/// │    ➡️ USA       : ILogRepository, IHttpContextAccessor                               │
/// ╰───────────────────────────────────────────────────────────────────────────────────────╯
/// </summary>
public class LogService : ILogService
{
    // ====== DEPENDÊNCIAS ======
    private readonly IServiceProvider _serviceProvider;
    private readonly IWebHostEnvironment _environment;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<LogService> _logger;

    // ====== CONFIGURAÇÃO ======
    private readonly string _logDirectory;
    private readonly object _lockObject = new();

    // ====== BUFFER PARA RETRY ======
    private readonly ConcurrentQueue<LogErro> _failedLogs = new();
    private readonly Timer _retryTimer;
    private const int RETRY_INTERVAL_MS = 30000; // 30 segundos
    private const int MAX_QUEUE_SIZE = 1000;

    // ====== EVENTO ======
    public event Action<string>? OnErrorOccurred;

    public LogService(
        IServiceProvider serviceProvider,
        IWebHostEnvironment environment,
        IHttpContextAccessor httpContextAccessor,
        ILogger<LogService> logger)
    {
        _serviceProvider = serviceProvider;
        _environment = environment;
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
        _logDirectory = Path.Combine(_environment.ContentRootPath, "Logs");

        // [INIT] Garante que a pasta de logs existe
        if (!Directory.Exists(_logDirectory))
        {
            Directory.CreateDirectory(_logDirectory);
        }

        // [INIT] Timer para retry de logs que falharam
        _retryTimer = new Timer(ProcessFailedLogs, null, RETRY_INTERVAL_MS, RETRY_INTERVAL_MS);

        // [INIT] Log de inicialização (apenas em TXT para evitar dependência circular)
        WriteToFile("INFO", "LogService v3.0 inicializado - Gravação em Banco de Dados ativada", null, "LogService.cs", "Constructor");
    }

    // ========== MÉTODOS PÚBLICOS ==========

    /// <summary>
    /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
    /// │ ⚡ MÉTODO: Info                                                                       │
    /// │───────────────────────────────────────────────────────────────────────────────────────│
    /// │ 🎯 DESCRIÇÃO: Registra uma mensagem informativa                                       │
    /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
    /// </summary>
    public void Info(string message, string? arquivo = null, string? metodo = null)
    {
        try
        {
            var logErro = CreateLogErro("INFO", "SERVER", message, arquivo, metodo);
            logErro.Nivel = "Information";
            SaveLogAsync(logErro);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao registrar log Info");
        }
    }

    /// <summary>
    /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
    /// │ ⚡ MÉTODO: Warning                                                                    │
    /// │───────────────────────────────────────────────────────────────────────────────────────│
    /// │ 🎯 DESCRIÇÃO: Registra um aviso                                                       │
    /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
    /// </summary>
    public void Warning(string message, string? arquivo = null, string? metodo = null)
    {
        try
        {
            var logErro = CreateLogErro("WARN", "SERVER", message, arquivo, metodo);
            logErro.Nivel = "Warning";
            SaveLogAsync(logErro);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao registrar log Warning");
        }
    }

    /// <summary>
    /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
    /// │ ⚡ MÉTODO: Error                                                                      │
    /// │───────────────────────────────────────────────────────────────────────────────────────│
    /// │ 🎯 DESCRIÇÃO: Registra um erro com exceção opcional                                   │
    /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
    /// </summary>
    public void Error(string message, Exception? exception = null, string? arquivo = null, string? metodo = null, int? linha = null)
    {
        try
        {
            var logErro = CreateLogErro("ERROR", "SERVER", message, arquivo, metodo, linha);
            logErro.Nivel = "Error";

            if (exception != null)
            {
                logErro.ExceptionType = exception.GetType().FullName;
                logErro.ExceptionMessage = exception.Message;
                logErro.StackTrace = exception.StackTrace;

                if (exception.InnerException != null)
                {
                    logErro.InnerException = $"{exception.InnerException.GetType().Name}: {exception.InnerException.Message}";
                }

                // [DADOS] Extrair arquivo e linha do StackTrace se não informados
                if (string.IsNullOrEmpty(arquivo) && !string.IsNullOrEmpty(exception.StackTrace))
                {
                    var match = Regex.Match(exception.StackTrace, @"in (.+):line (\d+)");
                    if (match.Success)
                    {
                        logErro.Arquivo = match.Groups[1].Value;
                        logErro.Linha = int.Parse(match.Groups[2].Value);
                    }
                }
            }

            SaveLogAsync(logErro);
            OnErrorOccurred?.Invoke(message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao registrar log Error");
        }
    }

    /// <summary>
    /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
    /// │ ⚡ MÉTODO: ErrorJS                                                                    │
    /// │───────────────────────────────────────────────────────────────────────────────────────│
    /// │ 🎯 DESCRIÇÃO: Registra erro de JavaScript (client-side)                              │
    /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
    /// </summary>
    public void ErrorJS(string message, string? arquivo = null, string? metodo = null, int? linha = null, int? coluna = null, string? stack = null, string? userAgent = null, string? url = null)
    {
        try
        {
            var logErro = CreateLogErro("ERROR-JS", "CLIENT", message, arquivo, metodo, linha);
            logErro.Nivel = "Error";
            logErro.Coluna = coluna;
            logErro.StackTrace = stack;
            logErro.UserAgent = userAgent;
            logErro.Categoria = "JavaScript";

            if (!string.IsNullOrEmpty(url))
            {
                logErro.Url = url;
            }

            SaveLogAsync(logErro);
            OnErrorOccurred?.Invoke($"[JS] {message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao registrar log ErrorJS");
        }
    }

    /// <summary>
    /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
    /// │ ⚡ MÉTODO: LogConsole                                                                 │
    /// │───────────────────────────────────────────────────────────────────────────────────────│
    /// │ 🎯 DESCRIÇÃO: Registra log do console do navegador                                    │
    /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
    /// </summary>
    public void LogConsole(string tipo, string message, string? arquivo = null, string? metodo = null, int? linha = null, int? coluna = null, string? stack = null, string? userAgent = null, string? url = null)
    {
        try
        {
            var tipoFormatado = $"CONSOLE-{tipo.ToUpper()}";
            var nivel = tipo.ToUpper() switch
            {
                "ERROR" => "Error",
                "WARN" => "Warning",
                "DEBUG" => "Debug",
                _ => "Information"
            };

            var logErro = CreateLogErro(tipoFormatado, "CLIENT", message, arquivo, metodo, linha);
            logErro.Nivel = nivel;
            logErro.Coluna = coluna;
            logErro.StackTrace = stack;
            logErro.UserAgent = userAgent;
            logErro.Categoria = "Console";

            if (!string.IsNullOrEmpty(url))
            {
                logErro.Url = url;
            }

            SaveLogAsync(logErro);

            // [REGRA] Disparar evento apenas para erros do console
            if (tipo.ToUpper() == "ERROR")
            {
                OnErrorOccurred?.Invoke($"[CONSOLE] {message}");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao registrar log do console");
        }
    }

    /// <summary>
    /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
    /// │ ⚡ MÉTODO: Debug                                                                      │
    /// │───────────────────────────────────────────────────────────────────────────────────────│
    /// │ 🎯 DESCRIÇÃO: Registra uma mensagem de debug (apenas em modo DEBUG)                  │
    /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
    /// </summary>
    public void Debug(string message, string? arquivo = null)
    {
#if DEBUG
        try
        {
            var logErro = CreateLogErro("DEBUG", "SERVER", message, arquivo, null);
            logErro.Nivel = "Debug";
            SaveLogAsync(logErro);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao registrar log Debug");
        }
#endif
    }

    /// <summary>
    /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
    /// │ ⚡ MÉTODO: OperationStart                                                             │
    /// │───────────────────────────────────────────────────────────────────────────────────────│
    /// │ 🎯 DESCRIÇÃO: Registra o início de uma operação                                       │
    /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
    /// </summary>
    public void OperationStart(string operationName, string? arquivo = null)
    {
        try
        {
            var logErro = CreateLogErro("OPERATION", "SERVER", $"▶️ Iniciando: {operationName}", arquivo, null);
            logErro.Nivel = "Information";
            logErro.Categoria = "Operation";
            SaveLogAsync(logErro);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao registrar início de operação");
        }
    }

    /// <summary>
    /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
    /// │ ⚡ MÉTODO: OperationSuccess                                                           │
    /// │───────────────────────────────────────────────────────────────────────────────────────│
    /// │ 🎯 DESCRIÇÃO: Registra o sucesso de uma operação                                      │
    /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
    /// </summary>
    public void OperationSuccess(string operationName, string? details = null)
    {
        try
        {
            var message = $"✅ Sucesso: {operationName}";
            if (!string.IsNullOrEmpty(details))
                message += $" - {details}";

            var logErro = CreateLogErro("OPERATION", "SERVER", message, null, null);
            logErro.Nivel = "Information";
            logErro.Categoria = "Operation";
            SaveLogAsync(logErro);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao registrar sucesso de operação");
        }
    }

    /// <summary>
    /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
    /// │ ⚡ MÉTODO: OperationFailed                                                            │
    /// │───────────────────────────────────────────────────────────────────────────────────────│
    /// │ 🎯 DESCRIÇÃO: Registra a falha de uma operação                                        │
    /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
    /// </summary>
    public void OperationFailed(string operationName, Exception exception, string? arquivo = null)
    {
        try
        {
            var logErro = CreateLogErro("OPERATION-FAIL", "SERVER", $"❌ Falha: {operationName}", arquivo, null);
            logErro.Nivel = "Error";
            logErro.Categoria = "Operation";
            logErro.ExceptionType = exception.GetType().FullName;
            logErro.ExceptionMessage = exception.Message;
            logErro.StackTrace = exception.StackTrace;

            if (exception.InnerException != null)
            {
                logErro.InnerException = $"{exception.InnerException.GetType().Name}: {exception.InnerException.Message}";
            }

            SaveLogAsync(logErro);
            OnErrorOccurred?.Invoke($"Falha: {operationName}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao registrar falha de operação");
        }
    }

    /// <summary>
    /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
    /// │ ⚡ MÉTODO: UserAction                                                                 │
    /// │───────────────────────────────────────────────────────────────────────────────────────│
    /// │ 🎯 DESCRIÇÃO: Registra uma ação do usuário                                            │
    /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
    /// </summary>
    public void UserAction(string action, string? details = null, string? usuario = null)
    {
        try
        {
            var user = usuario ?? GetCurrentUser();
            var message = $"👤 {user} - {action}";
            if (!string.IsNullOrEmpty(details))
                message += $" - {details}";

            var logErro = CreateLogErro("USER", "SERVER", message, null, null);
            logErro.Nivel = "Information";
            logErro.Categoria = "UserAction";
            logErro.Usuario = user;
            SaveLogAsync(logErro);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao registrar ação do usuário");
        }
    }

    /// <summary>
    /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
    /// │ ⚡ MÉTODO: HttpError                                                                  │
    /// │───────────────────────────────────────────────────────────────────────────────────────│
    /// │ 🎯 DESCRIÇÃO: Registra erro de requisição HTTP                                        │
    /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
    /// </summary>
    public void HttpError(int statusCode, string path, string method, string? message = null, string? usuario = null)
    {
        try
        {
            var logMessage = $"🌐 Status: {statusCode} | {method} {path}";
            if (!string.IsNullOrEmpty(message))
                logMessage += $" | {message}";

            var logErro = CreateLogErro("HTTP-ERROR", "SERVER", logMessage, null, null);
            logErro.Nivel = statusCode >= 500 ? "Error" : "Warning";
            logErro.Categoria = "HTTP";
            logErro.StatusCode = statusCode;
            logErro.Url = path;
            logErro.HttpMethod = method;
            logErro.Usuario = usuario ?? GetCurrentUser();

            SaveLogAsync(logErro);
            OnErrorOccurred?.Invoke($"[HTTP {statusCode}] {path}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao registrar HttpError");
        }
    }

    // ========== MÉTODOS DE CONSULTA ==========

    /// <summary>
    /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
    /// │ ⚡ MÉTODO: GetAllLogs                                                                 │
    /// │───────────────────────────────────────────────────────────────────────────────────────│
    /// │ 🎯 DESCRIÇÃO: Obtém todos os logs do dia atual (formatados como texto)               │
    /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
    /// </summary>
    public string GetAllLogs()
    {
        try
        {
            using var scope = _serviceProvider.CreateScope();
            var repository = scope.ServiceProvider.GetService<ILogRepository>();

            if (repository != null)
            {
                var logs = repository.GetByDateAsync(DateTime.Today).GetAwaiter().GetResult();
                return FormatLogsAsText(logs);
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Fallback para leitura de arquivo TXT");
        }

        // [FALLBACK] Ler do arquivo TXT
        return GetLogsFromFile();
    }

    /// <summary>
    /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
    /// │ ⚡ MÉTODO: GetLogsByDate                                                              │
    /// │───────────────────────────────────────────────────────────────────────────────────────│
    /// │ 🎯 DESCRIÇÃO: Obtém logs filtrados por data                                           │
    /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
    /// </summary>
    public string GetLogsByDate(DateTime date)
    {
        try
        {
            using var scope = _serviceProvider.CreateScope();
            var repository = scope.ServiceProvider.GetService<ILogRepository>();

            if (repository != null)
            {
                var logs = repository.GetByDateAsync(date).GetAwaiter().GetResult();
                return FormatLogsAsText(logs);
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Fallback para leitura de arquivo TXT");
        }

        // [FALLBACK] Ler do arquivo TXT
        return GetLogsFromFile(date);
    }

    /// <summary>
    /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
    /// │ ⚡ MÉTODO: GetLogFiles                                                                │
    /// │───────────────────────────────────────────────────────────────────────────────────────│
    /// │ 🎯 DESCRIÇÃO: Obtém lista de arquivos de log disponíveis                              │
    /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
    /// </summary>
    public List<LogFileInfo> GetLogFiles()
    {
        var files = new List<LogFileInfo>();
        try
        {
            // [DADOS] Buscar datas disponíveis do banco de dados
            using var scope = _serviceProvider.CreateScope();
            var repository = scope.ServiceProvider.GetService<ILogRepository>();

            if (repository != null)
            {
                // Buscar últimos 30 dias que têm logs
                for (int i = 0; i < 30; i++)
                {
                    var date = DateTime.Today.AddDays(-i);
                    var logs = repository.GetByDateAsync(date).GetAwaiter().GetResult();

                    if (logs.Any())
                    {
                        files.Add(new LogFileInfo
                        {
                            FileName = $"frotix_log_{date:yyyy-MM-dd}.db",
                            Date = date,
                            SizeBytes = logs.Count * 500 // Estimativa
                        });
                    }
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Fallback para listagem de arquivos TXT");
        }

        // [FALLBACK] Adicionar arquivos TXT também
        try
        {
            if (Directory.Exists(_logDirectory))
            {
                var logFiles = Directory.GetFiles(_logDirectory, "frotix_log_*.txt")
                    .OrderByDescending(f => f);

                foreach (var file in logFiles)
                {
                    var fileInfo = new FileInfo(file);
                    var fileName = Path.GetFileNameWithoutExtension(file);
                    var dateStr = fileName.Replace("frotix_log_", "");

                    if (DateTime.TryParse(dateStr, out var date))
                    {
                        // Só adiciona se não tiver no banco
                        if (!files.Any(f => f.Date.Date == date.Date))
                        {
                            files.Add(new LogFileInfo
                            {
                                FileName = fileInfo.Name,
                                Date = date,
                                SizeBytes = fileInfo.Length
                            });
                        }
                    }
                }
            }
        }
        catch { }

        return files.OrderByDescending(f => f.Date).ToList();
    }

    /// <summary>
    /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
    /// │ ⚡ MÉTODO: ClearLogs                                                                  │
    /// │───────────────────────────────────────────────────────────────────────────────────────│
    /// │ 🎯 DESCRIÇÃO: Limpa todos os logs do dia atual                                        │
    /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
    /// </summary>
    public void ClearLogs()
    {
        try
        {
            using var scope = _serviceProvider.CreateScope();
            var repository = scope.ServiceProvider.GetService<ILogRepository>();

            if (repository != null)
            {
                // Marcar como resolvidos ao invés de deletar
                var logs = repository.GetByDateAsync(DateTime.Today).GetAwaiter().GetResult();
                foreach (var log in logs)
                {
                    repository.MarkAsResolvedAsync(log.LogErroId, GetCurrentUser(), "Limpo via LogService.ClearLogs").GetAwaiter().GetResult();
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Fallback para limpeza de arquivo TXT");
        }

        // [FALLBACK] Limpar arquivo TXT
        try
        {
            var logPath = GetLogFilePath();
            lock (_lockObject)
            {
                if (File.Exists(logPath))
                    File.Delete(logPath);
            }
        }
        catch { }

        Info("========== LOGS LIMPOS ==========", "LogService.cs", "ClearLogs");
    }

    /// <summary>
    /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
    /// │ ⚡ MÉTODO: ClearLogsBefore                                                            │
    /// │───────────────────────────────────────────────────────────────────────────────────────│
    /// │ 🎯 DESCRIÇÃO: Limpa logs anteriores a uma data                                        │
    /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
    /// </summary>
    public void ClearLogsBefore(DateTime date)
    {
        try
        {
            using var scope = _serviceProvider.CreateScope();
            var repository = scope.ServiceProvider.GetService<ILogRepository>();

            if (repository != null)
            {
                var deletedCount = repository.DeleteBeforeDateAsync(date).GetAwaiter().GetResult();
                Info($"Logs anteriores a {date:dd/MM/yyyy} foram limpos ({deletedCount} registros)", "LogService.cs", "ClearLogsBefore");
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Fallback para limpeza de arquivos TXT");
        }

        // [FALLBACK] Limpar arquivos TXT antigos
        try
        {
            if (Directory.Exists(_logDirectory))
            {
                var logFiles = Directory.GetFiles(_logDirectory, "frotix_log_*.txt");
                foreach (var file in logFiles)
                {
                    var fileName = Path.GetFileNameWithoutExtension(file);
                    var dateStr = fileName.Replace("frotix_log_", "");

                    if (DateTime.TryParse(dateStr, out var fileDate) && fileDate < date)
                    {
                        File.Delete(file);
                    }
                }
            }
        }
        catch { }
    }

    /// <summary>
    /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
    /// │ ⚡ MÉTODO: GetErrorCount                                                              │
    /// │───────────────────────────────────────────────────────────────────────────────────────│
    /// │ 🎯 DESCRIÇÃO: Obtém a contagem atual de erros                                         │
    /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
    /// </summary>
    public int GetErrorCount()
    {
        try
        {
            using var scope = _serviceProvider.CreateScope();
            var repository = scope.ServiceProvider.GetService<ILogRepository>();

            if (repository != null)
            {
                var stats = repository.GetDashboardStatsAsync(DateTime.Today, DateTime.Now).GetAwaiter().GetResult();
                return stats.TotalErros;
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Fallback para contagem de arquivo TXT");
        }

        // [FALLBACK] Contar do arquivo
        return CountErrorsFromFile();
    }

    /// <summary>
    /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
    /// │ ⚡ MÉTODO: GetStats                                                                   │
    /// │───────────────────────────────────────────────────────────────────────────────────────│
    /// │ 🎯 DESCRIÇÃO: Obtém estatísticas dos logs                                             │
    /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
    /// </summary>
    public LogStats GetStats()
    {
        var stats = new LogStats();
        try
        {
            using var scope = _serviceProvider.CreateScope();
            var repository = scope.ServiceProvider.GetService<ILogRepository>();

            if (repository != null)
            {
                var dbStats = repository.GetDashboardStatsAsync(DateTime.Today, DateTime.Now).GetAwaiter().GetResult();

                stats.TotalLogs = dbStats.TotalLogs;
                stats.ErrorCount = dbStats.TotalErros;
                stats.WarningCount = dbStats.TotalWarnings;
                stats.InfoCount = dbStats.TotalInfo;
                stats.JSErrorCount = dbStats.TotalJsErrors;
                stats.HttpErrorCount = dbStats.TotalHttpErrors;
                stats.ConsoleCount = dbStats.TotalConsole;
                stats.LastLogDate = dbStats.UltimoErro;
                stats.FirstLogDate = DateTime.Today;

                return stats;
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Fallback para estatísticas de arquivo TXT");
        }

        // [FALLBACK] Calcular do arquivo
        return GetStatsFromFile();
    }

    // ========== MÉTODOS PRIVADOS - PERSISTÊNCIA ==========

    /// <summary>
    /// Cria objeto LogErro com dados do contexto HTTP
    /// </summary>
    private LogErro CreateLogErro(string tipo, string origem, string mensagem, string? arquivo, string? metodo, int? linha = null)
    {
        var logErro = new LogErro
        {
            DataHora = DateTime.Now,
            Tipo = tipo,
            Origem = origem,
            Mensagem = mensagem,
            Arquivo = arquivo,
            Metodo = metodo,
            Linha = linha,
            Usuario = GetCurrentUser(),
            CriadoEm = DateTime.Now
        };

        // [DADOS] Preencher contexto HTTP se disponível
        try
        {
            var context = _httpContextAccessor.HttpContext;
            if (context != null)
            {
                logErro.Url = $"{context.Request.Path}{context.Request.QueryString}";
                logErro.HttpMethod = context.Request.Method;
                logErro.UserAgent = context.Request.Headers["User-Agent"].FirstOrDefault();
                logErro.IpAddress = context.Connection.RemoteIpAddress?.ToString();
                logErro.SessionId = context.Session?.Id;
            }
        }
        catch { }

        return logErro;
    }

    /// <summary>
    /// Salva log de forma assíncrona (fire-and-forget) com fallback para TXT
    /// </summary>
    private void SaveLogAsync(LogErro logErro)
    {
        // [ASYNC] Fire-and-forget para não bloquear
        _ = Task.Run(async () =>
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var repository = scope.ServiceProvider.GetService<ILogRepository>();

                if (repository != null)
                {
                    await repository.AddAsync(logErro);
                    System.Diagnostics.Debug.WriteLine($"[DB] {logErro.Tipo}: {logErro.Mensagem?.Substring(0, Math.Min(50, logErro.Mensagem.Length))}...");
                    return;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[FALLBACK] Erro ao salvar no banco: {ex.Message}");

                // [RETRY] Adicionar à fila de retry
                if (_failedLogs.Count < MAX_QUEUE_SIZE)
                {
                    _failedLogs.Enqueue(logErro);
                }
            }

            // [FALLBACK] Salvar no arquivo TXT
            WriteToFile(logErro.Tipo, logErro.Mensagem, logErro.StackTrace, logErro.Arquivo, logErro.Metodo, logErro.Linha);
        });
    }

    /// <summary>
    /// Processa logs que falharam (timer callback)
    /// </summary>
    private void ProcessFailedLogs(object? state)
    {
        if (_failedLogs.IsEmpty) return;

        try
        {
            using var scope = _serviceProvider.CreateScope();
            var repository = scope.ServiceProvider.GetService<ILogRepository>();

            if (repository == null) return;

            var logsToProcess = new List<LogErro>();
            while (_failedLogs.TryDequeue(out var log) && logsToProcess.Count < 100)
            {
                logsToProcess.Add(log);
            }

            if (logsToProcess.Any())
            {
                repository.AddRangeAsync(logsToProcess).GetAwaiter().GetResult();
                System.Diagnostics.Debug.WriteLine($"[RETRY] Processados {logsToProcess.Count} logs pendentes");
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[RETRY] Falha ao processar logs pendentes: {ex.Message}");
        }
    }

    // ========== MÉTODOS PRIVADOS - ARQUIVO TXT (FALLBACK) ==========

    private string GetLogFilePath(DateTime? date = null)
    {
        var logDate = date ?? DateTime.Now;
        return Path.Combine(_logDirectory, $"frotix_log_{logDate:yyyy-MM-dd}.txt");
    }

    private string GetCurrentUser()
    {
        try
        {
            return _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "Anônimo";
        }
        catch
        {
            return "Anônimo";
        }
    }

    private void WriteToFile(string tipo, string mensagem, string? stackTrace = null, string? arquivo = null, string? metodo = null, int? linha = null)
    {
        try
        {
            var sb = new StringBuilder();
            sb.Append($"[{tipo}] {mensagem}");
            if (!string.IsNullOrEmpty(arquivo)) sb.Append($" | Arquivo: {arquivo}");
            if (!string.IsNullOrEmpty(metodo)) sb.Append($" | Método: {metodo}");
            if (linha.HasValue) sb.Append($" | Linha: {linha}");
            if (!string.IsNullOrEmpty(stackTrace))
            {
                sb.AppendLine();
                sb.AppendLine($"  📚 StackTrace:");
                foreach (var line in stackTrace.Split('\n').Take(10))
                {
                    sb.AppendLine($"      {line.Trim()}");
                }
            }

            var logPath = GetLogFilePath();
            var logMessage = $"[{DateTime.Now:HH:mm:ss.fff}] {sb}";

            lock (_lockObject)
            {
                File.AppendAllText(logPath, logMessage + Environment.NewLine, Encoding.UTF8);
            }

            System.Diagnostics.Debug.WriteLine(logMessage);
        }
        catch { }
    }

    private string GetLogsFromFile(DateTime? date = null)
    {
        try
        {
            var logPath = GetLogFilePath(date);
            if (File.Exists(logPath))
            {
                lock (_lockObject)
                {
                    return File.ReadAllText(logPath, Encoding.UTF8);
                }
            }
            return $"Nenhum log disponível para {(date ?? DateTime.Today):dd/MM/yyyy}.";
        }
        catch (Exception ex)
        {
            return $"Erro ao obter logs: {ex.Message}";
        }
    }

    private int CountErrorsFromFile()
    {
        try
        {
            var logs = GetLogsFromFile();
            if (string.IsNullOrEmpty(logs))
                return 0;

            return Regex.Matches(logs, @"\[ERROR", RegexOptions.IgnoreCase).Count;
        }
        catch
        {
            return 0;
        }
    }

    private LogStats GetStatsFromFile()
    {
        var stats = new LogStats();
        try
        {
            var logs = GetLogsFromFile();
            if (string.IsNullOrEmpty(logs) || logs.StartsWith("Nenhum log"))
                return stats;

            var lines = logs.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            var regexEntrada = new Regex(@"^\[\d{2}:\d{2}:\d{2}\.\d{3}\]\s*\[([A-Z-]+)\]");
            var entradasPrincipais = lines.Where(l => regexEntrada.IsMatch(l)).ToList();

            stats.TotalLogs = entradasPrincipais.Count;
            stats.ErrorCount = entradasPrincipais.Count(l =>
                regexEntrada.Match(l).Groups[1].Value == "ERROR" ||
                l.Contains("[OPERATION-FAIL]"));
            stats.WarningCount = entradasPrincipais.Count(l =>
                regexEntrada.Match(l).Groups[1].Value == "WARN");
            stats.InfoCount = entradasPrincipais.Count(l =>
            {
                var match = regexEntrada.Match(l);
                var tipo = match.Success ? match.Groups[1].Value : "";
                return tipo == "INFO" || tipo == "USER" || tipo == "OPERATION" || tipo == "DEBUG";
            });
            stats.JSErrorCount = entradasPrincipais.Count(l =>
                regexEntrada.Match(l).Groups[1].Value == "ERROR-JS");
            stats.HttpErrorCount = entradasPrincipais.Count(l =>
                regexEntrada.Match(l).Groups[1].Value == "HTTP-ERROR");
            stats.ConsoleCount = entradasPrincipais.Count(l =>
            {
                var match = regexEntrada.Match(l);
                var tipo = match.Success ? match.Groups[1].Value : "";
                return tipo.StartsWith("CONSOLE-");
            });

            stats.FirstLogDate = DateTime.Today;
            stats.LastLogDate = DateTime.Now;
        }
        catch { }
        return stats;
    }

    /// <summary>
    /// Formata lista de logs do banco como texto (para compatibilidade)
    /// </summary>
    private string FormatLogsAsText(List<LogErro> logs)
    {
        if (!logs.Any())
            return $"Nenhum log disponível para {DateTime.Today:dd/MM/yyyy}.";

        var sb = new StringBuilder();
        foreach (var log in logs.OrderBy(l => l.DataHora))
        {
            // [FORMATO] Emojis baseados no tipo
            string emoji = log.Tipo switch
            {
                "ERROR" => "❌",
                "ERROR-JS" => "❌",
                "WARN" => "⚠️",
                "INFO" => "ℹ️",
                "DEBUG" => "🐛",
                var t when t.StartsWith("CONSOLE-") => "🖥️",
                "HTTP-ERROR" => "🌐",
                "OPERATION" => "▶️",
                "OPERATION-FAIL" => "❌",
                "USER" => "👤",
                _ => "📝"
            };

            // [FORMATO] Badge de origem
            string origemBadge = log.Origem == "CLIENT" ? "[🌐 CLIENT]" : "[🖥️ SERVER]";

            sb.AppendLine($"[{log.DataHora:HH:mm:ss.fff}] [{log.Tipo}] {emoji} {origemBadge} {log.Mensagem}");

            if (!string.IsNullOrEmpty(log.Arquivo))
                sb.AppendLine($"  📄 Arquivo: {log.Arquivo}");
            if (!string.IsNullOrEmpty(log.Metodo))
                sb.AppendLine($"  🔧 Método: {log.Metodo}");
            if (log.Linha.HasValue)
                sb.AppendLine($"  📍 Linha: {log.Linha}" + (log.Coluna.HasValue ? $", Coluna: {log.Coluna}" : ""));
            if (!string.IsNullOrEmpty(log.Url))
                sb.AppendLine($"  🌐 URL: {log.Url}");
            if (!string.IsNullOrEmpty(log.Usuario))
                sb.AppendLine($"  👤 Usuário: {log.Usuario}");
            if (!string.IsNullOrEmpty(log.ExceptionType))
                sb.AppendLine($"  ⚡ Exception: {log.ExceptionType}");
            if (!string.IsNullOrEmpty(log.ExceptionMessage))
                sb.AppendLine($"  💬 Message: {log.ExceptionMessage}");
            if (!string.IsNullOrEmpty(log.StackTrace))
            {
                sb.AppendLine($"  📚 StackTrace:");
                foreach (var line in log.StackTrace.Split('\n').Take(10))
                {
                    sb.AppendLine($"      {line.Trim()}");
                }
            }
        }

        return sb.ToString();
    }
}
