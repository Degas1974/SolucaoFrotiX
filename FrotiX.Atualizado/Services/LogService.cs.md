# Services/LogService.cs

**Mudanca:** GRANDE | **+516** linhas | **-263** linhas

---

```diff
--- JANEIRO: Services/LogService.cs
+++ ATUAL: Services/LogService.cs
@@ -1,90 +1,72 @@
 using System;
+using System.Collections.Concurrent;
 using System.Collections.Generic;
 using System.IO;
 using System.Linq;
 using System.Text;
 using System.Text.RegularExpressions;
+using System.Threading;
+using System.Threading.Tasks;
 using Microsoft.AspNetCore.Hosting;
 using Microsoft.AspNetCore.Http;
+using Microsoft.Extensions.DependencyInjection;
+using Microsoft.Extensions.Logging;
+using FrotiX.Models;
+using FrotiX.Repository.IRepository;
 
 namespace FrotiX.Services;
 
 public class LogService : ILogService
 {
+
+    private readonly IServiceProvider _serviceProvider;
     private readonly IWebHostEnvironment _environment;
     private readonly IHttpContextAccessor _httpContextAccessor;
+    private readonly ILogger<LogService> _logger;
+
     private readonly string _logDirectory;
     private readonly object _lockObject = new();
 
+    private readonly ConcurrentQueue<LogErro> _failedLogs = new();
+    private readonly Timer _retryTimer;
+    private const int RETRY_INTERVAL_MS = 30000;
+    private const int MAX_QUEUE_SIZE = 1000;
+
     public event Action<string>? OnErrorOccurred;
 
-    public LogService(IWebHostEnvironment environment, IHttpContextAccessor httpContextAccessor)
-    {
-        try
-        {
-            _environment = environment;
-            _httpContextAccessor = httpContextAccessor;
-            _logDirectory = Path.Combine(_environment.ContentRootPath, "Logs");
-
-            if (!Directory.Exists(_logDirectory))
-            {
-                Directory.CreateDirectory(_logDirectory);
-            }
-
-            Info("LogService inicializado", "LogService.cs", "Constructor");
-        }
-        catch (Exception error)
-        {
-            Alerta.TratamentoErroComLinha("LogService.cs", "LogService", error);
-        }
-    }
-
-    private string GetLogFilePath(DateTime? date = null)
-    {
-        var logDate = date ?? DateTime.Now;
-        return Path.Combine(_logDirectory, $"frotix_log_{logDate:yyyy-MM-dd}.txt");
-    }
-
-    private string GetCurrentUser()
-    {
-        try
-        {
-            return _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "Anônimo";
-        }
-        catch
-        {
-            return "Anônimo";
-        }
-    }
-
-    private string GetCurrentUrl()
-    {
-        try
-        {
-            var request = _httpContextAccessor.HttpContext?.Request;
-            if (request != null)
-            {
-                return $"{request.Path}{request.QueryString}";
-            }
-        }
-        catch { }
-        return "";
+    public LogService(
+        IServiceProvider serviceProvider,
+        IWebHostEnvironment environment,
+        IHttpContextAccessor httpContextAccessor,
+        ILogger<LogService> logger)
+    {
+        _serviceProvider = serviceProvider;
+        _environment = environment;
+        _httpContextAccessor = httpContextAccessor;
+        _logger = logger;
+        _logDirectory = Path.Combine(_environment.ContentRootPath, "Logs");
+
+        if (!Directory.Exists(_logDirectory))
+        {
+            Directory.CreateDirectory(_logDirectory);
+        }
+
+        _retryTimer = new Timer(ProcessFailedLogs, null, RETRY_INTERVAL_MS, RETRY_INTERVAL_MS);
+
+        WriteToFile("INFO", "LogService v3.0 inicializado - Gravação em Banco de Dados ativada", null, "LogService.cs", "Constructor");
     }
 
     public void Info(string message, string? arquivo = null, string? metodo = null)
     {
         try
         {
-            var sb = new StringBuilder();
-            sb.Append($"[INFO] {message}");
-            if (!string.IsNullOrEmpty(arquivo)) sb.Append($" | Arquivo: {arquivo}");
-            if (!string.IsNullOrEmpty(metodo)) sb.Append($" | Método: {metodo}");
-
-            WriteLog(sb.ToString());
-        }
-        catch (Exception ex)
-        {
-            System.Diagnostics.Debug.WriteLine($"Erro ao registrar log Info: {ex.Message}");
+            var logErro = CreateLogErro("INFO", "SERVER", message, arquivo, metodo);
+            logErro.Nivel = "Information";
+            SaveLogAsync(logErro);
+        }
+        catch (Exception ex)
+        {
+            _logger.LogError(ex, "Erro ao registrar log Info");
         }
     }
 
@@ -92,16 +74,13 @@
     {
         try
         {
-            var sb = new StringBuilder();
-            sb.Append($"[WARN] ⚠️ {message}");
-            if (!string.IsNullOrEmpty(arquivo)) sb.Append($" | Arquivo: {arquivo}");
-            if (!string.IsNullOrEmpty(metodo)) sb.Append($" | Método: {metodo}");
-
-            WriteLog(sb.ToString());
-        }
-        catch (Exception ex)
-        {
-            System.Diagnostics.Debug.WriteLine($"Erro ao registrar log Warning: {ex.Message}");
+            var logErro = CreateLogErro("WARN", "SERVER", message, arquivo, metodo);
+            logErro.Nivel = "Warning";
+            SaveLogAsync(logErro);
+        }
+        catch (Exception ex)
+        {
+            _logger.LogError(ex, "Erro ao registrar log Warning");
         }
     }
 
@@ -109,12 +88,37 @@
     {
         try
         {
-            WriteLogError("ERROR", message, exception, arquivo, metodo, linha);
+            var logErro = CreateLogErro("ERROR", "SERVER", message, arquivo, metodo, linha);
+            logErro.Nivel = "Error";
+
+            if (exception != null)
+            {
+                logErro.ExceptionType = exception.GetType().FullName;
+                logErro.ExceptionMessage = exception.Message;
+                logErro.StackTrace = exception.StackTrace;
+
+                if (exception.InnerException != null)
+                {
+                    logErro.InnerException = $"{exception.InnerException.GetType().Name}: {exception.InnerException.Message}";
+                }
+
+                if (string.IsNullOrEmpty(arquivo) && !string.IsNullOrEmpty(exception.StackTrace))
+                {
+                    var match = Regex.Match(exception.StackTrace, @"in (.+):line (\d+)");
+                    if (match.Success)
+                    {
+                        logErro.Arquivo = match.Groups[1].Value;
+                        logErro.Linha = int.Parse(match.Groups[2].Value);
+                    }
+                }
+            }
+
+            SaveLogAsync(logErro);
             OnErrorOccurred?.Invoke(message);
         }
         catch (Exception ex)
         {
-            System.Diagnostics.Debug.WriteLine($"Erro ao registrar log Error: {ex.Message}");
+            _logger.LogError(ex, "Erro ao registrar log Error");
         }
     }
 
@@ -122,29 +126,62 @@
     {
         try
         {
-            var sb = new StringBuilder();
-            sb.AppendLine($"[ERROR-JS] ❌ {message}");
-            sb.AppendLine($" 📄 Arquivo: {arquivo ?? "(não identificado)"}");
-            if (!string.IsNullOrEmpty(metodo)) sb.AppendLine($" 🔧 Função: {metodo}");
-            if (linha.HasValue) sb.AppendLine($" 📍 Linha: {linha}" + (coluna.HasValue ? $", Coluna: {coluna}" : ""));
-            if (!string.IsNullOrEmpty(url)) sb.AppendLine($" 🌐 URL: {url}");
-            if (!string.IsNullOrEmpty(userAgent)) sb.AppendLine($" 🖥️ Browser: {userAgent}");
-            sb.AppendLine($" 👤 Usuário: {GetCurrentUser()}");
-            if (!string.IsNullOrEmpty(stack))
-            {
-                sb.AppendLine($" 📚 Stack Trace:");
-                foreach (var line in stack.Split('\n').Take(10))
-                {
-                    sb.AppendLine($" {line.Trim()}");
-                }
-            }
-
-            WriteLog(sb.ToString());
+            var logErro = CreateLogErro("ERROR-JS", "CLIENT", message, arquivo, metodo, linha);
+            logErro.Nivel = "Error";
+            logErro.Coluna = coluna;
+            logErro.StackTrace = stack;
+            logErro.UserAgent = userAgent;
+            logErro.Categoria = "JavaScript";
+
+            if (!string.IsNullOrEmpty(url))
+            {
+                logErro.Url = url;
+            }
+
+            SaveLogAsync(logErro);
             OnErrorOccurred?.Invoke($"[JS] {message}");
         }
         catch (Exception ex)
         {
-            System.Diagnostics.Debug.WriteLine($"Erro ao registrar log ErrorJS: {ex.Message}");
+            _logger.LogError(ex, "Erro ao registrar log ErrorJS");
+        }
+    }
+
+    public void LogConsole(string tipo, string message, string? arquivo = null, string? metodo = null, int? linha = null, int? coluna = null, string? stack = null, string? userAgent = null, string? url = null)
+    {
+        try
+        {
+            var tipoFormatado = $"CONSOLE-{tipo.ToUpper()}";
+            var nivel = tipo.ToUpper() switch
+            {
+                "ERROR" => "Error",
+                "WARN" => "Warning",
+                "DEBUG" => "Debug",
+                _ => "Information"
+            };
+
+            var logErro = CreateLogErro(tipoFormatado, "CLIENT", message, arquivo, metodo, linha);
+            logErro.Nivel = nivel;
+            logErro.Coluna = coluna;
+            logErro.StackTrace = stack;
+            logErro.UserAgent = userAgent;
+            logErro.Categoria = "Console";
+
+            if (!string.IsNullOrEmpty(url))
+            {
+                logErro.Url = url;
+            }
+
+            SaveLogAsync(logErro);
+
+            if (tipo.ToUpper() == "ERROR")
+            {
+                OnErrorOccurred?.Invoke($"[CONSOLE] {message}");
+            }
+        }
+        catch (Exception ex)
+        {
+            _logger.LogError(ex, "Erro ao registrar log do console");
         }
     }
 
@@ -153,15 +190,13 @@
 #if DEBUG
         try
         {
-            var sb = new StringBuilder();
-            sb.Append($"[DEBUG] 🐛 {message}");
-            if (!string.IsNullOrEmpty(arquivo)) sb.Append($" | Arquivo: {arquivo}");
-
-            WriteLog(sb.ToString());
-        }
-        catch (Exception ex)
-        {
-            System.Diagnostics.Debug.WriteLine($"Erro ao registrar log Debug: {ex.Message}");
+            var logErro = CreateLogErro("DEBUG", "SERVER", message, arquivo, null);
+            logErro.Nivel = "Debug";
+            SaveLogAsync(logErro);
+        }
+        catch (Exception ex)
+        {
+            _logger.LogError(ex, "Erro ao registrar log Debug");
         }
 #endif
     }
@@ -170,15 +205,14 @@
     {
         try
         {
-            var sb = new StringBuilder();
-            sb.Append($"[OPERATION] ▶️ Iniciando: {operationName}");
-            if (!string.IsNullOrEmpty(arquivo)) sb.Append($" | Arquivo: {arquivo}");
-
-            WriteLog(sb.ToString());
-        }
-        catch (Exception ex)
-        {
-            System.Diagnostics.Debug.WriteLine($"Erro ao registrar início de operação: {ex.Message}");
+            var logErro = CreateLogErro("OPERATION", "SERVER", $"▶️ Iniciando: {operationName}", arquivo, null);
+            logErro.Nivel = "Information";
+            logErro.Categoria = "Operation";
+            SaveLogAsync(logErro);
+        }
+        catch (Exception ex)
+        {
+            _logger.LogError(ex, "Erro ao registrar início de operação");
         }
     }
 
@@ -186,15 +220,18 @@
     {
         try
         {
-            var message = $"[OPERATION] ✅ Sucesso: {operationName}";
+            var message = $"✅ Sucesso: {operationName}";
             if (!string.IsNullOrEmpty(details))
                 message += $" - {details}";
 
-            WriteLog(message);
-        }
-        catch (Exception ex)
-        {
-            System.Diagnostics.Debug.WriteLine($"Erro ao registrar sucesso de operação: {ex.Message}");
+            var logErro = CreateLogErro("OPERATION", "SERVER", message, null, null);
+            logErro.Nivel = "Information";
+            logErro.Categoria = "Operation";
+            SaveLogAsync(logErro);
+        }
+        catch (Exception ex)
+        {
+            _logger.LogError(ex, "Erro ao registrar sucesso de operação");
         }
     }
 
@@ -202,12 +239,24 @@
     {
         try
         {
-            WriteLogError("OPERATION-FAIL", $"❌ Falha: {operationName}", exception, arquivo);
+            var logErro = CreateLogErro("OPERATION-FAIL", "SERVER", $"❌ Falha: {operationName}", arquivo, null);
+            logErro.Nivel = "Error";
+            logErro.Categoria = "Operation";
+            logErro.ExceptionType = exception.GetType().FullName;
+            logErro.ExceptionMessage = exception.Message;
+            logErro.StackTrace = exception.StackTrace;
+
+            if (exception.InnerException != null)
+            {
+                logErro.InnerException = $"{exception.InnerException.GetType().Name}: {exception.InnerException.Message}";
+            }
+
+            SaveLogAsync(logErro);
             OnErrorOccurred?.Invoke($"Falha: {operationName}");
         }
         catch (Exception ex)
         {
-            System.Diagnostics.Debug.WriteLine($"Erro ao registrar falha de operação: {ex.Message}");
+            _logger.LogError(ex, "Erro ao registrar falha de operação");
         }
     }
 
@@ -216,15 +265,19 @@
         try
         {
             var user = usuario ?? GetCurrentUser();
-            var message = $"[USER] 👤 {user} - {action}";
+            var message = $"👤 {user} - {action}";
             if (!string.IsNullOrEmpty(details))
                 message += $" - {details}";
 
-            WriteLog(message);
-        }
-        catch (Exception ex)
-        {
-            System.Diagnostics.Debug.WriteLine($"Erro ao registrar ação do usuário: {ex.Message}");
+            var logErro = CreateLogErro("USER", "SERVER", message, null, null);
+            logErro.Nivel = "Information";
+            logErro.Categoria = "UserAction";
+            logErro.Usuario = user;
+            SaveLogAsync(logErro);
+        }
+        catch (Exception ex)
+        {
+            _logger.LogError(ex, "Erro ao registrar ação do usuário");
         }
     }
 
@@ -232,19 +285,24 @@
     {
         try
         {
-            var sb = new StringBuilder();
-            sb.AppendLine($"[HTTP-ERROR] 🌐 Status: {statusCode}");
-            sb.AppendLine($" 📍 Path: {path}");
-            sb.AppendLine($" 🔧 Method: {method}");
-            if (!string.IsNullOrEmpty(message)) sb.AppendLine($" 💬 Message: {message}");
-            sb.AppendLine($" 👤 Usuário: {usuario ?? GetCurrentUser()}");
-
-            WriteLog(sb.ToString());
+            var logMessage = $"🌐 Status: {statusCode} | {method} {path}";
+            if (!string.IsNullOrEmpty(message))
+                logMessage += $" | {message}";
+
+            var logErro = CreateLogErro("HTTP-ERROR", "SERVER", logMessage, null, null);
+            logErro.Nivel = statusCode >= 500 ? "Error" : "Warning";
+            logErro.Categoria = "HTTP";
+            logErro.StatusCode = statusCode;
+            logErro.Url = path;
+            logErro.HttpMethod = method;
+            logErro.Usuario = usuario ?? GetCurrentUser();
+
+            SaveLogAsync(logErro);
             OnErrorOccurred?.Invoke($"[HTTP {statusCode}] {path}");
         }
         catch (Exception ex)
         {
-            System.Diagnostics.Debug.WriteLine($"Erro ao registrar HttpError: {ex.Message}");
+            _logger.LogError(ex, "Erro ao registrar HttpError");
         }
     }
 
@@ -252,47 +310,78 @@
     {
         try
         {
-            var logPath = GetLogFilePath();
-            if (File.Exists(logPath))
-            {
-                lock (_lockObject)
-                {
-
-                    return File.ReadAllText(logPath, Encoding.UTF8);
-                }
-            }
-            return "Nenhum log disponível para hoje.";
-        }
-        catch (Exception ex)
-        {
-            System.Diagnostics.Debug.WriteLine($"Erro ao obter logs: {ex.Message}");
-            return $"Erro ao obter logs: {ex.Message}";
-        }
+            using var scope = _serviceProvider.CreateScope();
+            var repository = scope.ServiceProvider.GetService<ILogRepository>();
+
+            if (repository != null)
+            {
+                var logs = repository.GetByDateAsync(DateTime.Today).GetAwaiter().GetResult();
+                return FormatLogsAsText(logs);
+            }
+        }
+        catch (Exception ex)
+        {
+            _logger.LogWarning(ex, "Fallback para leitura de arquivo TXT");
+        }
+
+        return GetLogsFromFile();
     }
 
     public string GetLogsByDate(DateTime date)
     {
         try
         {
-            var logPath = GetLogFilePath(date);
-            if (File.Exists(logPath))
-            {
-                lock (_lockObject)
-                {
-                    return File.ReadAllText(logPath);
-                }
-            }
-            return $"Nenhum log disponível para {date:dd/MM/yyyy}.";
-        }
-        catch (Exception ex)
-        {
-            return $"Erro ao obter logs: {ex.Message}";
-        }
+            using var scope = _serviceProvider.CreateScope();
+            var repository = scope.ServiceProvider.GetService<ILogRepository>();
+
+            if (repository != null)
+            {
+                var logs = repository.GetByDateAsync(date).GetAwaiter().GetResult();
+                return FormatLogsAsText(logs);
+            }
+        }
+        catch (Exception ex)
+        {
+            _logger.LogWarning(ex, "Fallback para leitura de arquivo TXT");
+        }
+
+        return GetLogsFromFile(date);
     }
 
     public List<LogFileInfo> GetLogFiles()
     {
         var files = new List<LogFileInfo>();
+        try
+        {
+
+            using var scope = _serviceProvider.CreateScope();
+            var repository = scope.ServiceProvider.GetService<ILogRepository>();
+
+            if (repository != null)
+            {
+
+                for (int i = 0; i < 30; i++)
+                {
+                    var date = DateTime.Today.AddDays(-i);
+                    var logs = repository.GetByDateAsync(date).GetAwaiter().GetResult();
+
+                    if (logs.Any())
+                    {
+                        files.Add(new LogFileInfo
+                        {
+                            FileName = $"frotix_log_{date:yyyy-MM-dd}.db",
+                            Date = date,
+                            SizeBytes = logs.Count * 500
+                        });
+                    }
+                }
+            }
+        }
+        catch (Exception ex)
+        {
+            _logger.LogWarning(ex, "Fallback para listagem de arquivos TXT");
+        }
+
         try
         {
             if (Directory.Exists(_logDirectory))
@@ -308,25 +397,47 @@
 
                     if (DateTime.TryParse(dateStr, out var date))
                     {
-                        files.Add(new LogFileInfo
+
+                        if (!files.Any(f => f.Date.Date == date.Date))
                         {
-                            FileName = fileInfo.Name,
-                            Date = date,
-                            SizeBytes = fileInfo.Length
-                        });
+                            files.Add(new LogFileInfo
+                            {
+                                FileName = fileInfo.Name,
+                                Date = date,
+                                SizeBytes = fileInfo.Length
+                            });
+                        }
                     }
                 }
             }
         }
-        catch (Exception ex)
-        {
-            System.Diagnostics.Debug.WriteLine($"Erro ao listar arquivos de log: {ex.Message}");
-        }
-        return files;
+        catch { }
+
+        return files.OrderByDescending(f => f.Date).ToList();
     }
 
     public void ClearLogs()
     {
+        try
+        {
+            using var scope = _serviceProvider.CreateScope();
+            var repository = scope.ServiceProvider.GetService<ILogRepository>();
+
+            if (repository != null)
+            {
+
+                var logs = repository.GetByDateAsync(DateTime.Today).GetAwaiter().GetResult();
+                foreach (var log in logs)
+                {
+                    repository.MarkAsResolvedAsync(log.LogErroId, GetCurrentUser(), "Limpo via LogService.ClearLogs").GetAwaiter().GetResult();
+                }
+            }
+        }
+        catch (Exception ex)
+        {
+            _logger.LogWarning(ex, "Fallback para limpeza de arquivo TXT");
+        }
+
         try
         {
             var logPath = GetLogFilePath();
@@ -335,16 +446,30 @@
                 if (File.Exists(logPath))
                     File.Delete(logPath);
             }
-            WriteLog("========== LOGS LIMPOS ==========");
-        }
-        catch (Exception ex)
-        {
-            System.Diagnostics.Debug.WriteLine($"Erro ao limpar logs: {ex.Message}");
-        }
+        }
+        catch { }
+
+        Info("========== LOGS LIMPOS ==========", "LogService.cs", "ClearLogs");
     }
 
     public void ClearLogsBefore(DateTime date)
     {
+        try
+        {
+            using var scope = _serviceProvider.CreateScope();
+            var repository = scope.ServiceProvider.GetService<ILogRepository>();
+
+            if (repository != null)
+            {
+                var deletedCount = repository.DeleteBeforeDateAsync(date).GetAwaiter().GetResult();
+                Info($"Logs anteriores a {date:dd/MM/yyyy} foram limpos ({deletedCount} registros)", "LogService.cs", "ClearLogsBefore");
+            }
+        }
+        catch (Exception ex)
+        {
+            _logger.LogWarning(ex, "Fallback para limpeza de arquivos TXT");
+        }
+
         try
         {
             if (Directory.Exists(_logDirectory))
@@ -361,19 +486,231 @@
                     }
                 }
             }
-            Info($"Logs anteriores a {date:dd/MM/yyyy} foram limpos");
-        }
-        catch (Exception ex)
-        {
-            System.Diagnostics.Debug.WriteLine($"Erro ao limpar logs antigos: {ex.Message}");
-        }
+        }
+        catch { }
     }
 
     public int GetErrorCount()
     {
         try
         {
-            var logs = GetAllLogs();
+            using var scope = _serviceProvider.CreateScope();
+            var repository = scope.ServiceProvider.GetService<ILogRepository>();
+
+            if (repository != null)
+            {
+                var stats = repository.GetDashboardStatsAsync(DateTime.Today, DateTime.Now).GetAwaiter().GetResult();
+                return stats.TotalErros;
+            }
+        }
+        catch (Exception ex)
+        {
+            _logger.LogWarning(ex, "Fallback para contagem de arquivo TXT");
+        }
+
+        return CountErrorsFromFile();
+    }
+
+    public LogStats GetStats()
+    {
+        var stats = new LogStats();
+        try
+        {
+            using var scope = _serviceProvider.CreateScope();
+            var repository = scope.ServiceProvider.GetService<ILogRepository>();
+
+            if (repository != null)
+            {
+                var dbStats = repository.GetDashboardStatsAsync(DateTime.Today, DateTime.Now).GetAwaiter().GetResult();
+
+                stats.TotalLogs = dbStats.TotalLogs;
+                stats.ErrorCount = dbStats.TotalErros;
+                stats.WarningCount = dbStats.TotalWarnings;
+                stats.InfoCount = dbStats.TotalInfo;
+                stats.JSErrorCount = dbStats.TotalJsErrors;
+                stats.HttpErrorCount = dbStats.TotalHttpErrors;
+                stats.ConsoleCount = dbStats.TotalConsole;
+                stats.LastLogDate = dbStats.UltimoErro;
+                stats.FirstLogDate = DateTime.Today;
+
+                return stats;
+            }
+        }
+        catch (Exception ex)
+        {
+            _logger.LogWarning(ex, "Fallback para estatísticas de arquivo TXT");
+        }
+
+        return GetStatsFromFile();
+    }
+
+    private LogErro CreateLogErro(string tipo, string origem, string mensagem, string? arquivo, string? metodo, int? linha = null)
+    {
+        var logErro = new LogErro
+        {
+            DataHora = DateTime.Now,
+            Tipo = tipo,
+            Origem = origem,
+            Mensagem = mensagem,
+            Arquivo = arquivo,
+            Metodo = metodo,
+            Linha = linha,
+            Usuario = GetCurrentUser(),
+            CriadoEm = DateTime.Now
+        };
+
+        try
+        {
+            var context = _httpContextAccessor.HttpContext;
+            if (context != null)
+            {
+                logErro.Url = $"{context.Request.Path}{context.Request.QueryString}";
+                logErro.HttpMethod = context.Request.Method;
+                logErro.UserAgent = context.Request.Headers["User-Agent"].FirstOrDefault();
+                logErro.IpAddress = context.Connection.RemoteIpAddress?.ToString();
+                logErro.SessionId = context.Session?.Id;
+            }
+        }
+        catch { }
+
+        return logErro;
+    }
+
+    private void SaveLogAsync(LogErro logErro)
+    {
+
+        _ = Task.Run(async () =>
+        {
+            try
+            {
+                using var scope = _serviceProvider.CreateScope();
+                var repository = scope.ServiceProvider.GetService<ILogRepository>();
+
+                if (repository != null)
+                {
+                    await repository.AddAsync(logErro);
+                    System.Diagnostics.Debug.WriteLine($"[DB] {logErro.Tipo}: {logErro.Mensagem?.Substring(0, Math.Min(50, logErro.Mensagem.Length))}...");
+                    return;
+                }
+            }
+            catch (Exception ex)
+            {
+                System.Diagnostics.Debug.WriteLine($"[FALLBACK] Erro ao salvar no banco: {ex.Message}");
+
+                if (_failedLogs.Count < MAX_QUEUE_SIZE)
+                {
+                    _failedLogs.Enqueue(logErro);
+                }
+            }
+
+            WriteToFile(logErro.Tipo, logErro.Mensagem, logErro.StackTrace, logErro.Arquivo, logErro.Metodo, logErro.Linha);
+        });
+    }
+
+    private void ProcessFailedLogs(object? state)
+    {
+        if (_failedLogs.IsEmpty) return;
+
+        try
+        {
+            using var scope = _serviceProvider.CreateScope();
+            var repository = scope.ServiceProvider.GetService<ILogRepository>();
+
+            if (repository == null) return;
+
+            var logsToProcess = new List<LogErro>();
+            while (_failedLogs.TryDequeue(out var log) && logsToProcess.Count < 100)
+            {
+                logsToProcess.Add(log);
+            }
+
+            if (logsToProcess.Any())
+            {
+                repository.AddRangeAsync(logsToProcess).GetAwaiter().GetResult();
+                System.Diagnostics.Debug.WriteLine($"[RETRY] Processados {logsToProcess.Count} logs pendentes");
+            }
+        }
+        catch (Exception ex)
+        {
+            System.Diagnostics.Debug.WriteLine($"[RETRY] Falha ao processar logs pendentes: {ex.Message}");
+        }
+    }
+
+    private string GetLogFilePath(DateTime? date = null)
+    {
+        var logDate = date ?? DateTime.Now;
+        return Path.Combine(_logDirectory, $"frotix_log_{logDate:yyyy-MM-dd}.txt");
+    }
+
+    private string GetCurrentUser()
+    {
+        try
+        {
+            return _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "Anônimo";
+        }
+        catch
+        {
+            return "Anônimo";
+        }
+    }
+
+    private void WriteToFile(string tipo, string mensagem, string? stackTrace = null, string? arquivo = null, string? metodo = null, int? linha = null)
+    {
+        try
+        {
+            var sb = new StringBuilder();
+            sb.Append($"[{tipo}] {mensagem}");
+            if (!string.IsNullOrEmpty(arquivo)) sb.Append($" | Arquivo: {arquivo}");
+            if (!string.IsNullOrEmpty(metodo)) sb.Append($" | Método: {metodo}");
+            if (linha.HasValue) sb.Append($" | Linha: {linha}");
+            if (!string.IsNullOrEmpty(stackTrace))
+            {
+                sb.AppendLine();
+                sb.AppendLine($" 📚 StackTrace:");
+                foreach (var line in stackTrace.Split('\n').Take(10))
+                {
+                    sb.AppendLine($" {line.Trim()}");
+                }
+            }
+
+            var logPath = GetLogFilePath();
+            var logMessage = $"[{DateTime.Now:HH:mm:ss.fff}] {sb}";
+
+            lock (_lockObject)
+            {
+                File.AppendAllText(logPath, logMessage + Environment.NewLine, Encoding.UTF8);
+            }
+
+            System.Diagnostics.Debug.WriteLine(logMessage);
+        }
+        catch { }
+    }
+
+    private string GetLogsFromFile(DateTime? date = null)
+    {
+        try
+        {
+            var logPath = GetLogFilePath(date);
+            if (File.Exists(logPath))
+            {
+                lock (_lockObject)
+                {
+                    return File.ReadAllText(logPath, Encoding.UTF8);
+                }
+            }
+            return $"Nenhum log disponível para {(date ?? DateTime.Today):dd/MM/yyyy}.";
+        }
+        catch (Exception ex)
+        {
+            return $"Erro ao obter logs: {ex.Message}";
+        }
+    }
+
+    private int CountErrorsFromFile()
+    {
+        try
+        {
+            var logs = GetLogsFromFile();
             if (string.IsNullOrEmpty(logs))
                 return 0;
 
@@ -385,148 +722,101 @@
         }
     }
 
-    public LogStats GetStats()
+    private LogStats GetStatsFromFile()
     {
         var stats = new LogStats();
         try
         {
-            var logs = GetAllLogs();
+            var logs = GetLogsFromFile();
             if (string.IsNullOrEmpty(logs) || logs.StartsWith("Nenhum log"))
                 return stats;
 
             var lines = logs.Split('\n', StringSplitOptions.RemoveEmptyEntries);
-
             var regexEntrada = new Regex(@"^\[\d{2}:\d{2}:\d{2}\.\d{3}\]\s*\[([A-Z-]+)\]");
-
             var entradasPrincipais = lines.Where(l => regexEntrada.IsMatch(l)).ToList();
 
             stats.TotalLogs = entradasPrincipais.Count;
-
             stats.ErrorCount = entradasPrincipais.Count(l =>
                 regexEntrada.Match(l).Groups[1].Value == "ERROR" ||
                 l.Contains("[OPERATION-FAIL]"));
-
             stats.WarningCount = entradasPrincipais.Count(l =>
                 regexEntrada.Match(l).Groups[1].Value == "WARN");
-
             stats.InfoCount = entradasPrincipais.Count(l =>
             {
                 var match = regexEntrada.Match(l);
                 var tipo = match.Success ? match.Groups[1].Value : "";
                 return tipo == "INFO" || tipo == "USER" || tipo == "OPERATION" || tipo == "DEBUG";
             });
-
             stats.JSErrorCount = entradasPrincipais.Count(l =>
                 regexEntrada.Match(l).Groups[1].Value == "ERROR-JS");
-
             stats.HttpErrorCount = entradasPrincipais.Count(l =>
                 regexEntrada.Match(l).Groups[1].Value == "HTTP-ERROR");
-
-            if (entradasPrincipais.Any())
-            {
-                var dateRegex = new Regex(@"\[(\d{2}:\d{2}:\d{2}\.\d{3})\]");
-                var firstMatch = dateRegex.Match(entradasPrincipais.First());
-                var lastMatch = dateRegex.Match(entradasPrincipais.Last());
-
-                if (firstMatch.Success)
-                {
-                    try
-                    {
-                        var timeParts = firstMatch.Groups[1].Value.Split('.');
-                        stats.FirstLogDate = DateTime.Today.Add(TimeSpan.Parse(timeParts[0]));
-                    }
-                    catch { }
-                }
-                if (lastMatch.Success)
-                {
-                    try
-                    {
-                        var timeParts = lastMatch.Groups[1].Value.Split('.');
-                        stats.LastLogDate = DateTime.Today.Add(TimeSpan.Parse(timeParts[0]));
-                    }
-                    catch { }
-                }
-            }
-        }
-        catch (Exception ex)
-        {
-            System.Diagnostics.Debug.WriteLine($"Erro ao calcular estatísticas: {ex.Message}");
-        }
+            stats.ConsoleCount = entradasPrincipais.Count(l =>
+            {
+                var match = regexEntrada.Match(l);
+                var tipo = match.Success ? match.Groups[1].Value : "";
+                return tipo.StartsWith("CONSOLE-");
+            });
+
+            stats.FirstLogDate = DateTime.Today;
+            stats.LastLogDate = DateTime.Now;
+        }
+        catch { }
         return stats;
     }
 
-    private void WriteLog(string message)
-    {
-        try
-        {
-            var logPath = GetLogFilePath();
-            var logMessage = $"[{DateTime.Now:HH:mm:ss.fff}] {message}";
-
-            lock (_lockObject)
-            {
-
-                File.AppendAllText(logPath, logMessage + Environment.NewLine, Encoding.UTF8);
-            }
-
-            System.Diagnostics.Debug.WriteLine(logMessage);
-        }
-        catch { }
-    }
-
-    private void WriteLogError(string type, string message, Exception? exception = null, string? arquivo = null, string? metodo = null, int? linha = null)
-    {
-        try
-        {
-            var sb = new StringBuilder();
-            sb.AppendLine($"[{type}] ❌ {message}");
-
-            if (!string.IsNullOrEmpty(arquivo)) sb.AppendLine($" 📄 Arquivo: {arquivo}");
-            if (!string.IsNullOrEmpty(metodo)) sb.AppendLine($" 🔧 Método: {metodo}");
-            if (linha.HasValue) sb.AppendLine($" 📍 Linha: {linha}");
-
-            var url = GetCurrentUrl();
-            if (!string.IsNullOrEmpty(url)) sb.AppendLine($" 🌐 URL: {url}");
-
-            sb.AppendLine($" 👤 Usuário: {GetCurrentUser()}");
-
-            if (exception != null)
-            {
-                sb.AppendLine($" ⚡ Exception: {exception.GetType().Name}");
-                sb.AppendLine($" 💬 Message: {exception.Message}");
-
-                if (!string.IsNullOrEmpty(exception.StackTrace))
-                {
-                    var stackMatch = Regex.Match(exception.StackTrace, @"in (.+):line (\d+)");
-                    if (stackMatch.Success)
-                    {
-                        sb.AppendLine($" 📍 Local do Erro: {stackMatch.Groups[1].Value}:linha {stackMatch.Groups[2].Value}");
-                    }
-
-                    sb.AppendLine($" 📚 StackTrace:");
-                    foreach (var line in exception.StackTrace.Split('\n').Take(15))
-                    {
-                        sb.AppendLine($" {line.Trim()}");
-                    }
-                }
-
-                if (exception.InnerException != null)
-                {
-                    sb.AppendLine($" 🔗 InnerException: {exception.InnerException.GetType().Name}");
-                    sb.AppendLine($" Message: {exception.InnerException.Message}");
-                }
-            }
-
-            var logPath = GetLogFilePath();
-            var timestamp = $"[{DateTime.Now:HH:mm:ss.fff}] ";
-
-            lock (_lockObject)
-            {
-
-                File.AppendAllText(logPath, timestamp + sb.ToString() + Environment.NewLine, Encoding.UTF8);
-            }
-
-            System.Diagnostics.Debug.WriteLine(timestamp + sb.ToString());
-        }
-        catch { }
+    private string FormatLogsAsText(List<LogErro> logs)
+    {
+        if (!logs.Any())
+            return $"Nenhum log disponível para {DateTime.Today:dd/MM/yyyy}.";
+
+        var sb = new StringBuilder();
+        foreach (var log in logs.OrderBy(l => l.DataHora))
+        {
+
+            string emoji = log.Tipo switch
+            {
+                "ERROR" => "❌",
+                "ERROR-JS" => "❌",
+                "WARN" => "⚠️",
+                "INFO" => "ℹ️",
+                "DEBUG" => "🐛",
+                var t when t.StartsWith("CONSOLE-") => "🖥️",
+                "HTTP-ERROR" => "🌐",
+                "OPERATION" => "▶️",
+                "OPERATION-FAIL" => "❌",
+                "USER" => "👤",
+                _ => "📝"
+            };
+
+            string origemBadge = log.Origem == "CLIENT" ? "[🌐 CLIENT]" : "[🖥️ SERVER]";
+
+            sb.AppendLine($"[{log.DataHora:HH:mm:ss.fff}] [{log.Tipo}] {emoji} {origemBadge} {log.Mensagem}");
+
+            if (!string.IsNullOrEmpty(log.Arquivo))
+                sb.AppendLine($" 📄 Arquivo: {log.Arquivo}");
+            if (!string.IsNullOrEmpty(log.Metodo))
+                sb.AppendLine($" 🔧 Método: {log.Metodo}");
+            if (log.Linha.HasValue)
+                sb.AppendLine($" 📍 Linha: {log.Linha}" + (log.Coluna.HasValue ? $", Coluna: {log.Coluna}" : ""));
+            if (!string.IsNullOrEmpty(log.Url))
+                sb.AppendLine($" 🌐 URL: {log.Url}");
+            if (!string.IsNullOrEmpty(log.Usuario))
+                sb.AppendLine($" 👤 Usuário: {log.Usuario}");
+            if (!string.IsNullOrEmpty(log.ExceptionType))
+                sb.AppendLine($" ⚡ Exception: {log.ExceptionType}");
+            if (!string.IsNullOrEmpty(log.ExceptionMessage))
+                sb.AppendLine($" 💬 Message: {log.ExceptionMessage}");
+            if (!string.IsNullOrEmpty(log.StackTrace))
+            {
+                sb.AppendLine($" 📚 StackTrace:");
+                foreach (var line in log.StackTrace.Split('\n').Take(10))
+                {
+                    sb.AppendLine($" {line.Trim()}");
+                }
+            }
+        }
+
+        return sb.ToString();
     }
 }
```
