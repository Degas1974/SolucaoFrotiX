using System.Collections.Concurrent;

namespace DeepSeekIDE.Core.Services;

/// <summary>
/// Serviço de logging centralizado para o DeepSeek IDE
/// </summary>
public class LogService : IDisposable
{
    private static LogService? _instance;
    private static readonly object _lock = new();

    private readonly ConcurrentQueue<LogEntry> _logEntries = new();
    private readonly string _logDirectory;
    private readonly string _logFilePath;
    private readonly int _maxLogEntries = 10000;
    private bool _disposed;

    public event EventHandler<LogEntry>? OnLogAdded;

    public static LogService Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    _instance ??= new LogService();
                }
            }
            return _instance;
        }
    }

    private LogService()
    {
        _logDirectory = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "DeepSeekIDE",
            "Logs"
        );

        Directory.CreateDirectory(_logDirectory);
        _logFilePath = Path.Combine(_logDirectory, $"deepseekide_{DateTime.Now:yyyyMMdd}.log");
    }

    public void Log(LogLevel level, string message, string? source = null, Exception? exception = null)
    {
        var entry = new LogEntry
        {
            Timestamp = DateTime.Now,
            Level = level,
            Message = message,
            Source = source ?? "DeepSeekIDE",
            Exception = exception
        };

        _logEntries.Enqueue(entry);

        // Limita o tamanho do buffer em memória
        while (_logEntries.Count > _maxLogEntries)
        {
            _logEntries.TryDequeue(out _);
        }

        // Escreve no arquivo
        WriteToFile(entry);

        // Notifica listeners
        OnLogAdded?.Invoke(this, entry);
    }

    public void Debug(string message, string? source = null) => Log(LogLevel.Debug, message, source);
    public void Info(string message, string? source = null) => Log(LogLevel.Information, message, source);
    public void Warning(string message, string? source = null, Exception? ex = null) => Log(LogLevel.Warning, message, source, ex);
    public void Error(string message, string? source = null, Exception? ex = null) => Log(LogLevel.Error, message, source, ex);
    public void Error(Exception ex, string? source = null) => Log(LogLevel.Error, ex.Message, source, ex);

    private void WriteToFile(LogEntry entry)
    {
        try
        {
            var line = $"[{entry.Timestamp:yyyy-MM-dd HH:mm:ss.fff}] [{entry.Level}] [{entry.Source}] {entry.Message}";
            if (entry.Exception != null)
            {
                line += $"\n  Exception: {entry.Exception.GetType().Name}: {entry.Exception.Message}";
                line += $"\n  StackTrace: {entry.Exception.StackTrace}";
            }
            line += Environment.NewLine;

            File.AppendAllText(_logFilePath, line);
        }
        catch
        {
            // Ignora erros de escrita no log
        }
    }

    public IEnumerable<LogEntry> GetRecentLogs(int count = 100, LogLevel? minLevel = null)
    {
        var entries = _logEntries.ToArray().Reverse();

        if (minLevel.HasValue)
        {
            entries = entries.Where(e => e.Level >= minLevel.Value);
        }

        return entries.Take(count);
    }

    public string GetLogFilePath() => _logFilePath;

    public string GetLogDirectory() => _logDirectory;

    public void ClearLogs()
    {
        _logEntries.Clear();
    }

    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;
        GC.SuppressFinalize(this);
    }
}

/// <summary>
/// Entrada de log
/// </summary>
public class LogEntry
{
    public DateTime Timestamp { get; set; }
    public LogLevel Level { get; set; }
    public string Message { get; set; } = string.Empty;
    public string Source { get; set; } = string.Empty;
    public Exception? Exception { get; set; }
}

/// <summary>
/// Níveis de log
/// </summary>
public enum LogLevel
{
    Debug = 0,
    Information = 1,
    Warning = 2,
    Error = 3,
    Critical = 4
}
