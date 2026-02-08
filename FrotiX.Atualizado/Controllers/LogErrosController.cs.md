# Controllers/LogErrosController.cs

**Mudanca:** GRANDE | **+473** linhas | **-221** linhas

---

```diff
--- JANEIRO: Controllers/LogErrosController.cs
+++ ATUAL: Controllers/LogErrosController.cs
@@ -1,261 +1,542 @@
 using System;
+using System.IO;
 using System.Linq;
+using System.Threading;
 using FrotiX.Services;
 using Microsoft.AspNetCore.Authorization;
+using Microsoft.AspNetCore.Connections;
 using Microsoft.AspNetCore.Mvc;
+using Microsoft.Extensions.DependencyInjection;
 using Microsoft.Extensions.Logging;
 
-namespace FrotiX.Controllers
+namespace FrotiX.Controllers;
+
+[Route("api/[controller]")]
+[ApiController]
+[AllowAnonymous]
+public partial class LogErrosController : ControllerBase
 {
-
-    [Route("api/[controller]")]
-    [ApiController]
-    [AllowAnonymous]
-    public partial class LogErrosController : ControllerBase
-    {
-        private readonly ILogService _log;
-        private readonly ILogger<LogErrosController> _logger;
-
-        public LogErrosController(ILogService log, ILogger<LogErrosController> logger)
-        {
-            try
-            {
-                _log = log;
-                _logger = logger;
-            }
-            catch (Exception ex)
-            {
-                Alerta.TratamentoErroComLinha("LogErrosController.cs", "LogErrosController", ex);
-                Console.WriteLine($"FATAL: Erro construtor LogErrosController: {ex.Message}");
-            }
-        }
-
-        [HttpPost]
-        [Route("LogJavaScript")]
-        public IActionResult LogJavaScript([FromBody] LogJavaScriptRequest request)
-        {
-            try
-            {
-
-                if (request == null || string.IsNullOrEmpty(request.Mensagem))
-                {
-                    return BadRequest(new { success = false, error = "Dados de log inválidos" });
+    private readonly ILogService _logService;
+    private readonly ILogger<LogErrosController> _logger;
+
+    public LogErrosController(ILogService logService, ILogger<LogErrosController> logger)
+    {
+        _logService = logService;
+        _logger = logger;
+    }
+
+    private static bool IsClientDisconnectException(Exception ex)
+    {
+        var current = ex;
+        while (current != null)
+        {
+
+            if (current is ConnectionResetException)
+                return true;
+
+            if (current.GetType().Name == "ConnectionAbortedException")
+                return true;
+
+            if (current is IOException ioEx)
+            {
+                var message = ioEx.Message?.ToLowerInvariant() ?? "";
+                if (message.Contains("connection reset") ||
+                    message.Contains("broken pipe") ||
+                    message.Contains("an existing connection was forcibly closed") ||
+                    message.Contains("the client has disconnected") ||
+                    message.Contains("connection was aborted"))
+                {
+                    return true;
                 }
-
-                _log.ErrorJS(
-                    request.Mensagem,
-                    request.Arquivo,
-                    request.Metodo,
-                    request.Linha,
-                    request.Coluna,
-                    request.Stack,
-                    request.UserAgent,
-                    request.Url
-                );
-
-                return Ok(new { success = true });
-            }
-            catch (Exception ex)
-            {
-                _log.Error(ex.Message, ex, "LogErrosController.cs", "LogJavaScript");
-                Alerta.TratamentoErroComLinha("LogErrosController.cs", "LogJavaScript", ex);
-                _logger.LogError(ex, "Erro ao registrar log JavaScript");
-                return StatusCode(500, new { success = false, error = "Erro interno ao processar log" });
-            }
-        }
-
-        [HttpGet]
-        [Route("ObterLogs")]
-        public IActionResult ObterLogs()
-        {
-            try
-            {
-
-                var logs = _log.GetAllLogs() ?? "";
-                var stats = _log.GetStats();
-
-                var json = System.Text.Json.JsonSerializer.Serialize(new
-                {
-                    success = true,
-                    logs = logs,
-                    stats = new
-                    {
-                        stats.TotalLogs,
-                        stats.ErrorCount,
-                        stats.WarningCount,
-                        stats.InfoCount,
-                        stats.JSErrorCount,
-                        stats.HttpErrorCount
-                    }
-                }, new System.Text.Json.JsonSerializerOptions
-                {
-                    PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase,
-                    WriteIndented = false
+            }
+
+            if (current is System.Net.Sockets.SocketException socketEx)
+            {
+                if (socketEx.SocketErrorCode == System.Net.Sockets.SocketError.ConnectionReset ||
+                    socketEx.SocketErrorCode == System.Net.Sockets.SocketError.ConnectionAborted ||
+                    socketEx.SocketErrorCode == System.Net.Sockets.SocketError.Shutdown)
+                {
+                    return true;
+                }
+            }
+
+            var msg = current.Message?.ToLowerInvariant() ?? "";
+            if (msg.Contains("the client has disconnected") ||
+                msg.Contains("connection reset by peer") ||
+                msg.Contains("an established connection was aborted"))
+            {
+                return true;
+            }
+
+            current = current.InnerException;
+        }
+
+        return false;
+    }
+
+    [HttpPost]
+    [Route("LogJavaScript")]
+    public IActionResult LogJavaScript([FromBody] LogJavaScriptRequest request, CancellationToken cancellationToken)
+    {
+        try
+        {
+
+            if (cancellationToken.IsCancellationRequested)
+            {
+                _logger.LogDebug("LogJavaScript: Cliente desconectou antes do processamento");
+                return StatusCode(499);
+            }
+
+            if (request == null || string.IsNullOrEmpty(request.Mensagem))
+            {
+                return BadRequest(new { success = false, error = "Dados de log inválidos" });
+            }
+
+            _logService.ErrorJS(
+                request.Mensagem,
+                request.Arquivo,
+                request.Metodo,
+                request.Linha,
+                request.Coluna,
+                request.Stack,
+                request.UserAgent,
+                request.Url
+            );
+
+            return Ok(new { success = true });
+        }
+        catch (Exception ex) when (IsClientDisconnectException(ex) || cancellationToken.IsCancellationRequested)
+        {
+
+            _logger.LogDebug("LogJavaScript: Cliente desconectou durante processamento");
+            return StatusCode(499);
+        }
+        catch (Exception ex)
+        {
+            _logger.LogError(ex, "Erro ao registrar log JavaScript");
+            return StatusCode(500, new { success = false, error = "Erro interno ao processar log" });
+        }
+    }
+
+    [HttpPost]
+    [Route("Client")]
+    public IActionResult Client([FromBody] LogClientRequest request, CancellationToken cancellationToken)
+    {
+        try
+        {
+
+            if (cancellationToken.IsCancellationRequested)
+            {
+                _logger.LogDebug("Client: Cliente desconectou antes do processamento");
+                return StatusCode(499);
+            }
+
+            if (request == null || string.IsNullOrEmpty(request.Mensagem))
+            {
+                return BadRequest(new { success = false, error = "Dados de log inválidos" });
+            }
+
+            var tipo = request.Tipo?.ToUpperInvariant() ?? "HTTP-ERROR";
+
+            switch (tipo)
+            {
+                case "ERROR":
+                case "HTTP-ERROR":
+                    _logService.HttpError(
+                        request.StatusCode ?? 0,
+                        request.Url ?? "Unknown",
+                        request.Metodo ?? "AJAX",
+                        request.Mensagem,
+                        User?.Identity?.Name
+                    );
+                    break;
+
+                case "GLOBAL-ERROR":
+                case "UNHANDLED-PROMISE":
+                    _logService.ErrorJS(
+                        request.Mensagem,
+                        request.Arquivo,
+                        request.Metodo,
+                        request.Linha,
+                        request.Coluna,
+                        request.Stack,
+                        request.UserAgent,
+                        request.Url
+                    );
+                    break;
+
+                default:
+                    _logService.LogConsole(
+                        tipo,
+                        request.Mensagem,
+                        request.Arquivo,
+                        request.Metodo,
+                        request.Linha,
+                        request.Coluna,
+                        request.Stack,
+                        request.UserAgent,
+                        request.Url
+                    );
+                    break;
+            }
+
+            return Ok(new { success = true, requestId = request.RequestId });
+        }
+        catch (Exception ex) when (IsClientDisconnectException(ex) || cancellationToken.IsCancellationRequested)
+        {
+
+            _logger.LogDebug("Client: Cliente desconectou durante processamento");
+            return StatusCode(499);
+        }
+        catch (Exception ex)
+        {
+            _logger.LogError(ex, "Erro ao registrar log do cliente");
+            return StatusCode(500, new { success = false, error = "Erro interno ao processar log" });
+        }
+    }
+
+    [HttpPost]
+    [Route("LogConsole")]
+    public IActionResult LogConsole([FromBody] LogConsoleRequest request, CancellationToken cancellationToken)
+    {
+        try
+        {
+
+            if (cancellationToken.IsCancellationRequested)
+            {
+                _logger.LogDebug("LogConsole: Cliente desconectou antes do processamento");
+                return StatusCode(499);
+            }
+
+            if (request == null || string.IsNullOrEmpty(request.Mensagem))
+            {
+                return BadRequest(new { success = false, error = "Dados de log inválidos" });
+            }
+
+            _logService.LogConsole(
+                request.Tipo ?? "INFO",
+                request.Mensagem,
+                request.Arquivo,
+                request.Metodo,
+                request.Linha,
+                request.Coluna,
+                request.Stack,
+                request.UserAgent,
+                request.Url
+            );
+
+            return Ok(new { success = true });
+        }
+        catch (Exception ex) when (IsClientDisconnectException(ex) || cancellationToken.IsCancellationRequested)
+        {
+
+            _logger.LogDebug("LogConsole: Cliente desconectou durante processamento");
+            return StatusCode(499);
+        }
+        catch (Exception ex)
+        {
+            _logger.LogError(ex, "Erro ao registrar log do console");
+            return StatusCode(500, new { success = false, error = "Erro interno ao processar log" });
+        }
+    }
+
+    [HttpGet]
+    [Route("ObterLogs")]
+    public IActionResult ObterLogs()
+    {
+        try
+        {
+            var logs = _logService.GetAllLogs() ?? "";
+            var stats = _logService.GetStats();
+
+            var json = System.Text.Json.JsonSerializer.Serialize(new
+            {
+                success = true,
+                logs = logs,
+                stats = new
+                {
+                    stats.TotalLogs,
+                    stats.ErrorCount,
+                    stats.WarningCount,
+                    stats.InfoCount,
+                    stats.JSErrorCount,
+                    stats.HttpErrorCount
+                }
+            }, new System.Text.Json.JsonSerializerOptions
+            {
+                PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase,
+                WriteIndented = false
+            });
+
+            return Content(json, "application/json", System.Text.Encoding.UTF8);
+        }
+        catch (Exception ex)
+        {
+            _logger.LogError(ex, "Erro ao obter logs");
+            var errorJson = System.Text.Json.JsonSerializer.Serialize(new { success = false, error = ex.Message, logs = "" });
+            return Content(errorJson, "application/json", System.Text.Encoding.UTF8);
+        }
+    }
+
+    [HttpGet]
+    [Route("ObterLogsPorData")]
+    public IActionResult ObterLogsPorData([FromQuery] DateTime data)
+    {
+        try
+        {
+            var logs = _logService.GetLogsByDate(data);
+            return Ok(new { success = true, logs, data = data.ToString("dd/MM/yyyy") });
+        }
+        catch (Exception ex)
+        {
+            _logger.LogError(ex, "Erro ao obter logs por data");
+            return StatusCode(500, new { success = false, error = ex.Message });
+        }
+    }
+
+    [HttpGet]
+    [Route("ListarArquivos")]
+    public IActionResult ListarArquivos()
+    {
+        try
+        {
+            var files = _logService.GetLogFiles();
+            return Ok(new
+            {
+                success = true,
+                arquivos = files.Select(f => new
+                {
+                    f.FileName,
+                    Data = f.Date.ToString("dd/MM/yyyy"),
+                    f.SizeFormatted
+                })
+            });
+        }
+        catch (Exception ex)
+        {
+            _logger.LogError(ex, "Erro ao listar arquivos de log");
+            return StatusCode(500, new { success = false, error = ex.Message });
+        }
+    }
+
+    [HttpGet]
+    [Route("ObterEstatisticas")]
+    public IActionResult ObterEstatisticas()
+    {
+        try
+        {
+            var stats = _logService.GetStats();
+            var errorCount = _logService.GetErrorCount();
+
+            return Ok(new
+            {
+                success = true,
+                stats = new
+                {
+                    stats.TotalLogs,
+                    stats.ErrorCount,
+                    stats.WarningCount,
+                    stats.InfoCount,
+                    stats.JSErrorCount,
+                    stats.HttpErrorCount,
+                    TotalErros = errorCount
+                }
+            });
+        }
+        catch (Exception ex)
+        {
+            _logger.LogError(ex, "Erro ao obter estatísticas");
+            return StatusCode(500, new { success = false, error = ex.Message });
+        }
+    }
+
+    [HttpPost]
+    [Route("LimparLogs")]
+    public IActionResult LimparLogs()
+    {
+        try
+        {
+            _logService.ClearLogs();
+            _logService.UserAction("Limpou todos os logs do dia");
+
+            return Ok(new { success = true, message = "Logs limpos com sucesso" });
+        }
+        catch (Exception ex)
+        {
+            _logger.LogError(ex, "Erro ao limpar logs");
+            return StatusCode(500, new { success = false, error = ex.Message });
+        }
+    }
+
+    [HttpPost]
+    [Route("LimparLogsAntigos")]
+    public IActionResult LimparLogsAntigos([FromQuery] int diasManter = 30)
+    {
+        try
+        {
+            var dataLimite = DateTime.Now.AddDays(-diasManter);
+            _logService.ClearLogsBefore(dataLimite);
+            _logService.UserAction($"Limpou logs anteriores a {dataLimite:dd/MM/yyyy}");
+
+            return Ok(new { success = true, message = $"Logs anteriores a {dataLimite:dd/MM/yyyy} foram limpos" });
+        }
+        catch (Exception ex)
+        {
+            _logger.LogError(ex, "Erro ao limpar logs antigos");
+            return StatusCode(500, new { success = false, error = ex.Message });
+        }
+    }
+
+    [HttpGet]
+    [Route("DownloadLog")]
+    public IActionResult DownloadLog([FromQuery] DateTime data)
+    {
+        try
+        {
+            var logs = _logService.GetLogsByDate(data);
+            var fileName = $"frotix_log_{data:yyyy-MM-dd}.txt";
+
+            var bytes = System.Text.Encoding.UTF8.GetBytes(logs);
+            return File(bytes, "text/plain", fileName);
+        }
+        catch (Exception ex)
+        {
+            _logger.LogError(ex, "Erro ao fazer download do log");
+            return StatusCode(500, new { success = false, error = ex.Message });
+        }
+    }
+
+    [HttpPost]
+    [Route("AnalisarErroTexto")]
+    public async System.Threading.Tasks.Task<IActionResult> AnalisarErroTexto([FromBody] AnalyzeTextRequest request)
+    {
+        try
+        {
+            if (request == null || string.IsNullOrEmpty(request.Mensagem))
+            {
+                return Ok(new { success = false, error = "Dados do erro não fornecidos" });
+            }
+
+            var claudeService = HttpContext.RequestServices.GetService<IClaudeAnalysisService>();
+
+            if (claudeService == null)
+            {
+                return Ok(new { success = false, error = "Serviço de análise não disponível" });
+            }
+
+            if (!claudeService.IsConfigured)
+            {
+                return Ok(new { success = false, error = "API Key do Claude não configurada. Configure em appsettings.json na seção 'ClaudeAI'." });
+            }
+
+            var logErro = new FrotiX.Models.LogErro
+            {
+                Tipo = request.Tipo ?? "ERROR",
+                Origem = request.Origem ?? "SERVER",
+                Mensagem = request.Mensagem,
+                Arquivo = request.Arquivo,
+                Metodo = request.Metodo,
+                Linha = request.Linha,
+                Coluna = request.Coluna,
+                StackTrace = request.Stack,
+                Url = request.Url,
+                UserAgent = request.UserAgent,
+                DataHora = request.DataHora ?? DateTime.Now,
+                Usuario = User?.Identity?.Name ?? "Anônimo"
+            };
+
+            _logger.LogInformation("Iniciando análise de erro via texto com Claude AI");
+
+            var result = await claudeService.AnalyzeErrorAsync(logErro);
+
+            if (!result.Success)
+            {
+                return Ok(new
+                {
+                    success = false,
+                    error = result.Error
                 });
-
-                return Content(json, "application/json", System.Text.Encoding.UTF8);
-            }
-            catch (Exception ex)
-            {
-                Alerta.TratamentoErroComLinha("LogErrosController.cs", "ObterLogs", ex);
-                _logger.LogError(ex, "Erro ao obter logs");
-                var errorJson = System.Text.Json.JsonSerializer.Serialize(new { success = false, error = ex.Message, logs = "" });
-                return Content(errorJson, "application/json", System.Text.Encoding.UTF8);
-            }
-        }
-
-        [HttpGet]
-        [Route("ObterLogsPorData")]
-        public IActionResult ObterLogsPorData([FromQuery] DateTime data)
-        {
-            try
-            {
-
-                var logs = _log.GetLogsByDate(data);
-                return Ok(new { success = true, logs, data = data.ToString("dd/MM/yyyy") });
-            }
-            catch (Exception ex)
-            {
-                Alerta.TratamentoErroComLinha("LogErrosController.cs", "ObterLogsPorData", ex);
-                _logger.LogError(ex, "Erro ao obter logs por data");
-                return StatusCode(500, new { success = false, error = ex.Message });
-            }
-        }
-
-        [HttpGet]
-        [Route("ListarArquivos")]
-        public IActionResult ListarArquivos()
-        {
-            try
-            {
-
-                var files = _log.GetLogFiles();
-                return Ok(new
-                {
-                    success = true,
-                    arquivos = files.Select(f => new
-                    {
-                        f.FileName,
-                        Data = f.Date.ToString("dd/MM/yyyy"),
-                        f.SizeFormatted
-                    })
-                });
-            }
-            catch (Exception ex)
-            {
-                Alerta.TratamentoErroComLinha("LogErrosController.cs", "ListarArquivos", ex);
-                _logger.LogError(ex, "Erro ao listar arquivos de log");
-                return StatusCode(500, new { success = false, error = ex.Message });
-            }
-        }
-
-        [HttpGet]
-        [Route("ObterEstatisticas")]
-        public IActionResult ObterEstatisticas()
-        {
-            try
-            {
-
-                var stats = _log.GetStats();
-                var errorCount = _log.GetErrorCount();
-
-                return Ok(new
-                {
-                    success = true,
-                    stats = new
-                    {
-                        stats.TotalLogs,
-                        stats.ErrorCount,
-                        stats.WarningCount,
-                        stats.InfoCount,
-                        stats.JSErrorCount,
-                        stats.HttpErrorCount,
-                        TotalErros = errorCount
-                    }
-                });
-            }
-            catch (Exception ex)
-            {
-                Alerta.TratamentoErroComLinha("LogErrosController.cs", "ObterEstatisticas", ex);
-                _logger.LogError(ex, "Erro ao obter estatísticas");
-                return StatusCode(500, new { success = false, error = ex.Message });
-            }
-        }
-
-        [HttpPost]
-        [Route("LimparLogs")]
-        public IActionResult LimparLogs()
-        {
-            try
-            {
-
-                _log.ClearLogs();
-                _log.UserAction("Limpou todos os logs do dia");
-
-                return Ok(new { success = true, message = "Logs limpos com sucesso" });
-            }
-            catch (Exception ex)
-            {
-                Alerta.TratamentoErroComLinha("LogErrosController.cs", "LimparLogs", ex);
-                _logger.LogError(ex, "Erro ao limpar logs");
-                return StatusCode(500, new { success = false, error = ex.Message });
-            }
-        }
-
-        [HttpPost]
-        [Route("LimparLogsAntigos")]
-        public IActionResult LimparLogsAntigos([FromQuery] int diasManter = 30)
-        {
-            try
-            {
-
-                var dataLimite = DateTime.Now.AddDays(-diasManter);
-
-                _log.ClearLogsBefore(dataLimite);
-                _log.UserAction($"Limpou logs anteriores a {dataLimite:dd/MM/yyyy}");
-
-                return Ok(new { success = true, message = $"Logs anteriores a {dataLimite:dd/MM/yyyy} foram limpos" });
-            }
-            catch (Exception ex)
-            {
-                Alerta.TratamentoErroComLinha("LogErrosController.cs", "LimparLogsAntigos", ex);
-                _logger.LogError(ex, "Erro ao limpar logs antigos");
-                return StatusCode(500, new { success = false, error = ex.Message });
-            }
-        }
-
-        [HttpGet]
-        [Route("DownloadLog")]
-        public IActionResult DownloadLog([FromQuery] DateTime data)
-        {
-            try
-            {
-
-                var logs = _log.GetLogsByDate(data);
-                var fileName = $"frotix_log_{data:yyyy-MM-dd}.txt";
-
-                var bytes = System.Text.Encoding.UTF8.GetBytes(logs);
-                return File(bytes, "text/plain", fileName);
-            }
-            catch (Exception ex)
-            {
-                Alerta.TratamentoErroComLinha("LogErrosController.cs", "DownloadLog", ex);
-                _logger.LogError(ex, "Erro ao fazer download do log");
-                return StatusCode(500, new { success = false, error = ex.Message });
-            }
-        }
-    }
-
-    public class LogJavaScriptRequest
-    {
-        public string Mensagem { get; set; } = "";
-        public string? Arquivo { get; set; }
-        public string? Metodo { get; set; }
-        public int? Linha { get; set; }
-        public int? Coluna { get; set; }
-        public string? Stack { get; set; }
-        public string? UserAgent { get; set; }
-        public string? Url { get; set; }
-        public string? Timestamp { get; set; }
+            }
+
+            return Ok(new
+            {
+                success = true,
+                analysis = result.Analysis,
+                model = result.Model,
+                tokens = new
+                {
+                    input = result.InputTokens,
+                    output = result.OutputTokens,
+                    total = result.InputTokens + result.OutputTokens
+                },
+                analyzedAt = result.AnalyzedAt
+            });
+        }
+        catch (Exception ex)
+        {
+            _logger.LogError(ex, "Erro ao analisar erro via texto com Claude");
+            return Ok(new { success = false, error = ex.Message });
+        }
     }
 }
+
+public class LogJavaScriptRequest
+{
+    public string Mensagem { get; set; } = "";
+    public string? Arquivo { get; set; }
+    public string? Metodo { get; set; }
+    public int? Linha { get; set; }
+    public int? Coluna { get; set; }
+    public string? Stack { get; set; }
+    public string? UserAgent { get; set; }
+    public string? Url { get; set; }
+    public string? Timestamp { get; set; }
+}
+
+public class LogConsoleRequest
+{
+    public string? Tipo { get; set; }
+    public string Mensagem { get; set; } = "";
+    public string? Arquivo { get; set; }
+    public string? Metodo { get; set; }
+    public int? Linha { get; set; }
+    public int? Coluna { get; set; }
+    public string? Stack { get; set; }
+    public string? UserAgent { get; set; }
+    public string? Url { get; set; }
+    public string? Timestamp { get; set; }
+}
+
+public class AnalyzeTextRequest
+{
+    public string? Tipo { get; set; }
+    public string? Origem { get; set; }
+    public string Mensagem { get; set; } = "";
+    public string? Arquivo { get; set; }
+    public string? Metodo { get; set; }
+    public int? Linha { get; set; }
+    public int? Coluna { get; set; }
+    public string? Stack { get; set; }
+    public string? UserAgent { get; set; }
+    public string? Url { get; set; }
+    public DateTime? DataHora { get; set; }
+}
+
+public class LogClientRequest
+{
+    public string? Tipo { get; set; }
+    public string Mensagem { get; set; } = "";
+    public int? StatusCode { get; set; }
+    public string? RequestId { get; set; }
+    public string? TextStatus { get; set; }
+    public string? ErrorThrown { get; set; }
+    public string? Arquivo { get; set; }
+    public string? Metodo { get; set; }
+    public int? Linha { get; set; }
+    public int? Coluna { get; set; }
+    public string? Stack { get; set; }
+    public string? UserAgent { get; set; }
+    public string? Url { get; set; }
+    public string? Timestamp { get; set; }
+    public string? Detalhes { get; set; }
+}
```
