/* ****************************************************************************************
 * ⚡ ARQUIVO: LogErrosController.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Centralizar o recebimento, consulta e limpeza de logs de erro
 *                   (frontend JavaScript e backend C#).
 *
 * 📥 ENTRADAS     : LogJavaScriptRequest, filtros de data e parâmetros de limpeza.
 *
 * 📤 SAÍDAS       : JSON com confirmações, listas e estatísticas; arquivos para download.
 *
 * 🔗 CHAMADA POR  : window.onerror (JS), páginas administrativas de logs.
 *
 * 🔄 CHAMA        : ILogService, ILogger, JsonSerializer.
 *
 * 📦 DEPENDÊNCIAS : ASP.NET Core, ILogService, Microsoft.Extensions.Logging.
 *
 * 📝 OBSERVAÇÕES  : Endpoints permitem acesso anônimo para registrar erros sem autenticação.
 **************************************************************************************** */

/****************************************************************************************
 * ⚡ CONTROLLER: LogErrosController (Partial Class)
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Expor endpoints para registrar, consultar, estatizar e limpar logs.
 *
 * 📥 ENTRADAS     : Logs JS, parâmetros de data e dias de retenção.
 *
 * 📤 SAÍDAS       : JSON com confirmação/erros e conteúdo de logs.
 *
 * 🔗 CHAMADA POR  : Frontend (JS), Pages/LogErros.
 *
 * 🔄 CHAMA        : ILogService (persistência), ILogger (erros), JsonSerializer.
 *
 * 📦 DEPENDÊNCIAS : ASP.NET Core, ILogService, Microsoft.Extensions.Logging.
 *
 * 🔓 SEGURANÇA    : [AllowAnonymous] habilita captura de erros sem autenticação.
 ****************************************************************************************/
using System;
using System.IO;
using System.Linq;
using System.Threading;
using FrotiX.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FrotiX.Controllers;

[Route("api/[controller]")]
[ApiController]
[AllowAnonymous]
public partial class LogErrosController : ControllerBase
{
    private readonly ILogService _logService;
    private readonly ILogger<LogErrosController> _logger;

    /****************************************************************************************
     * ⚡ FUNÇÃO: LogErrosController (Construtor)
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Injetar serviços de log e logger.
     *
     * 📥 ENTRADAS     : [ILogService] logService, [ILogger<LogErrosController>] logger.
     *
     * 📤 SAÍDAS       : Instância configurada.
     *
     * 🔗 CHAMADA POR  : ASP.NET Core DI.
     ****************************************************************************************/
    public LogErrosController(ILogService logService, ILogger<LogErrosController> logger)
    {
        _logService = logService;
        _logger = logger;
    }

    /****************************************************************************************
     * ⚡ FUNÇÃO: IsClientDisconnectException
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Verificar se uma exceção indica desconexão do cliente.
     *
     * 📥 ENTRADAS     : [Exception] ex - Exceção a ser verificada.
     *
     * 📤 SAÍDAS       : [bool] true se é uma desconexão de cliente.
     *
     * 📝 OBSERVAÇÕES  : Detecta ConnectionResetException, IOException com mensagens
     *                   específicas, e outras exceções relacionadas à desconexão.
     ****************************************************************************************/
    private static bool IsClientDisconnectException(Exception ex)
    {
        var current = ex;
        while (current != null)
        {
            // ConnectionResetException do ASP.NET Core
            if (current is ConnectionResetException)
                return true;

            // ConnectionAbortedException do ASP.NET Core
            if (current.GetType().Name == "ConnectionAbortedException")
                return true;

            // IOException com mensagens específicas de desconexão
            if (current is IOException ioEx)
            {
                var message = ioEx.Message?.ToLowerInvariant() ?? "";
                if (message.Contains("connection reset") ||
                    message.Contains("broken pipe") ||
                    message.Contains("an existing connection was forcibly closed") ||
                    message.Contains("the client has disconnected") ||
                    message.Contains("connection was aborted"))
                {
                    return true;
                }
            }

            // SocketException com códigos de desconexão
            if (current is System.Net.Sockets.SocketException socketEx)
            {
                if (socketEx.SocketErrorCode == System.Net.Sockets.SocketError.ConnectionReset ||
                    socketEx.SocketErrorCode == System.Net.Sockets.SocketError.ConnectionAborted ||
                    socketEx.SocketErrorCode == System.Net.Sockets.SocketError.Shutdown)
                {
                    return true;
                }
            }

            // Mensagens genéricas de desconexão
            var msg = current.Message?.ToLowerInvariant() ?? "";
            if (msg.Contains("the client has disconnected") ||
                msg.Contains("connection reset by peer") ||
                msg.Contains("an established connection was aborted"))
            {
                return true;
            }

            current = current.InnerException;
        }

        return false;
    }

    /****************************************************************************************
     * ⚡ FUNÇÃO: LogJavaScript
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Receber logs de erro do JavaScript (client-side).
     *
     * 📥 ENTRADAS     : [LogJavaScriptRequest] request (mensagem, arquivo, linha, stack, etc).
     *                   [CancellationToken] cancellationToken para detectar desconexão do cliente.
     *
     * 📤 SAÍDAS       : JSON com sucesso ou erro de validação/processamento.
     *                   Status 499 se o cliente desconectou antes da resposta.
     *
     * 🔗 CHAMADA POR  : window.onerror / handlers de erro no frontend.
     *
     * 🔄 CHAMA        : _logService.ErrorJS().
     ****************************************************************************************/
    [HttpPost]
    [Route("LogJavaScript")]
    public IActionResult LogJavaScript([FromBody] LogJavaScriptRequest request, CancellationToken cancellationToken)
    {
        try
        {
            // [CHECK] Verifica se o cliente ainda está conectado antes de processar
            if (cancellationToken.IsCancellationRequested)
            {
                _logger.LogDebug("LogJavaScript: Cliente desconectou antes do processamento");
                return StatusCode(499); // Client Closed Request
            }

            if (request == null || string.IsNullOrEmpty(request.Mensagem))
            {
                return BadRequest(new { success = false, error = "Dados de log inválidos" });
            }

            _logService.ErrorJS(
                request.Mensagem,
                request.Arquivo,
                request.Metodo,
                request.Linha,
                request.Coluna,
                request.Stack,
                request.UserAgent,
                request.Url
            );

            return Ok(new { success = true });
        }
        catch (Exception ex) when (IsClientDisconnectException(ex) || cancellationToken.IsCancellationRequested)
        {
            // [INFO] Cliente desconectou - isso é normal para logs assíncronos
            _logger.LogDebug("LogJavaScript: Cliente desconectou durante processamento");
            return StatusCode(499); // Client Closed Request
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao registrar log JavaScript");
            return StatusCode(500, new { success = false, error = "Erro interno ao processar log" });
        }
    }

    /****************************************************************************************
     * ⚡ FUNÇÃO: Client
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Receber logs de erros HTTP do cliente (fetch/AJAX failures).
     *
     * 📥 ENTRADAS     : [LogClientRequest] request (tipo, mensagem, statusCode, requestId, etc).
     *                   [CancellationToken] cancellationToken para detectar desconexão do cliente.
     *
     * 📤 SAÍDAS       : JSON com sucesso ou erro de validação/processamento.
     *                   Status 499 se o cliente desconectou antes da resposta.
     *
     * 🔗 CHAMADA POR  : api-client.js, global-error-handler.js (via sendBeacon ou fetch).
     *
     * 🔄 CHAMA        : _logService.LogHttpError().
     ****************************************************************************************/
    [HttpPost]
    [Route("Client")]
    public IActionResult Client([FromBody] LogClientRequest request, CancellationToken cancellationToken)
    {
        try
        {
            // [CHECK] Verifica se o cliente ainda está conectado antes de processar
            if (cancellationToken.IsCancellationRequested)
            {
                _logger.LogDebug("Client: Cliente desconectou antes do processamento");
                return StatusCode(499); // Client Closed Request
            }

            if (request == null || string.IsNullOrEmpty(request.Mensagem))
            {
                return BadRequest(new { success = false, error = "Dados de log inválidos" });
            }

            // Determina o método do log baseado no tipo
            var tipo = request.Tipo?.ToUpperInvariant() ?? "HTTP-ERROR";

            switch (tipo)
            {
                case "ERROR":
                case "HTTP-ERROR":
                    _logService.HttpError(
                        request.StatusCode ?? 0,
                        request.Url ?? "Unknown",
                        request.Metodo ?? "AJAX",
                        request.Mensagem,
                        User?.Identity?.Name
                    );
                    break;

                case "GLOBAL-ERROR":
                case "UNHANDLED-PROMISE":
                    _logService.ErrorJS(
                        request.Mensagem,
                        request.Arquivo,
                        request.Metodo,
                        request.Linha,
                        request.Coluna,
                        request.Stack,
                        request.UserAgent,
                        request.Url
                    );
                    break;

                default:
                    _logService.LogConsole(
                        tipo,
                        request.Mensagem,
                        request.Arquivo,
                        request.Metodo,
                        request.Linha,
                        request.Coluna,
                        request.Stack,
                        request.UserAgent,
                        request.Url
                    );
                    break;
            }

            return Ok(new { success = true, requestId = request.RequestId });
        }
        catch (Exception ex) when (IsClientDisconnectException(ex) || cancellationToken.IsCancellationRequested)
        {
            // [INFO] Cliente desconectou - isso é normal para logs assíncronos
            _logger.LogDebug("Client: Cliente desconectou durante processamento");
            return StatusCode(499); // Client Closed Request
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao registrar log do cliente");
            return StatusCode(500, new { success = false, error = "Erro interno ao processar log" });
        }
    }

    /****************************************************************************************
     * ⚡ FUNÇÃO: LogConsole
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Receber logs do console do navegador (console.log/warn/error/info).
     *
     * 📥 ENTRADAS     : [LogConsoleRequest] request (tipo, mensagem, arquivo, linha, stack, etc).
     *                   [CancellationToken] cancellationToken para detectar desconexão do cliente.
     *
     * 📤 SAÍDAS       : JSON com sucesso ou erro de validação/processamento.
     *                   Status 499 se o cliente desconectou antes da resposta.
     *
     * 🔗 CHAMADA POR  : console-interceptor.js (interceptador de console).
     *
     * 🔄 CHAMA        : _logService.LogConsole().
     ****************************************************************************************/
    [HttpPost]
    [Route("LogConsole")]
    public IActionResult LogConsole([FromBody] LogConsoleRequest request, CancellationToken cancellationToken)
    {
        try
        {
            // [CHECK] Verifica se o cliente ainda está conectado antes de processar
            if (cancellationToken.IsCancellationRequested)
            {
                _logger.LogDebug("LogConsole: Cliente desconectou antes do processamento");
                return StatusCode(499); // Client Closed Request
            }

            if (request == null || string.IsNullOrEmpty(request.Mensagem))
            {
                return BadRequest(new { success = false, error = "Dados de log inválidos" });
            }

            _logService.LogConsole(
                request.Tipo ?? "INFO",
                request.Mensagem,
                request.Arquivo,
                request.Metodo,
                request.Linha,
                request.Coluna,
                request.Stack,
                request.UserAgent,
                request.Url
            );

            return Ok(new { success = true });
        }
        catch (Exception ex) when (IsClientDisconnectException(ex) || cancellationToken.IsCancellationRequested)
        {
            // [INFO] Cliente desconectou - isso é normal para logs assíncronos
            _logger.LogDebug("LogConsole: Cliente desconectou durante processamento");
            return StatusCode(499); // Client Closed Request
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao registrar log do console");
            return StatusCode(500, new { success = false, error = "Erro interno ao processar log" });
        }
    }

    /****************************************************************************************
     * ⚡ FUNÇÃO: ObterLogs
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Obter todos os logs do dia atual com estatísticas.
     *
     * 📥 ENTRADAS     : Nenhuma.
     *
     * 📤 SAÍDAS       : JSON com logs e contadores (Total, Error, Warning, Info, JS, HTTP).
     *
     * 🔗 CHAMADA POR  : Tela de monitoramento de logs.
     *
     * 🔄 CHAMA        : _logService.GetAllLogs(), _logService.GetStats().
     *
     * 📝 OBSERVAÇÕES  : Serialização manual para evitar conflitos com interceptadores.
     ****************************************************************************************/
    [HttpGet]
    [Route("ObterLogs")]
    public IActionResult ObterLogs()
    {
        try
        {
            var logs = _logService.GetAllLogs() ?? "";
            var stats = _logService.GetStats();

            // Serializa manualmente para evitar problemas com interceptadores
            var json = System.Text.Json.JsonSerializer.Serialize(new
            {
                success = true,
                logs = logs,
                stats = new
                {
                    stats.TotalLogs,
                    stats.ErrorCount,
                    stats.WarningCount,
                    stats.InfoCount,
                    stats.JSErrorCount,
                    stats.HttpErrorCount
                }
            }, new System.Text.Json.JsonSerializerOptions
            {
                PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase,
                WriteIndented = false
            });

            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter logs");
            var errorJson = System.Text.Json.JsonSerializer.Serialize(new { success = false, error = ex.Message, logs = "" });
            return Content(errorJson, "application/json", System.Text.Encoding.UTF8);
        }
    }

    /****************************************************************************************
     * ⚡ FUNÇÃO: ObterLogsPorData
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Obter logs de uma data específica.
     *
     * 📥 ENTRADAS     : data (DateTime) via query string.
     *
     * 📤 SAÍDAS       : JSON com logs e data formatada.
     *
     * 🔗 CHAMADA POR  : Filtros de data na tela de logs.
     *
     * 🔄 CHAMA        : _logService.GetLogsByDate().
     ****************************************************************************************/
    [HttpGet]
    [Route("ObterLogsPorData")]
    public IActionResult ObterLogsPorData([FromQuery] DateTime data)
    {
        try
        {
            var logs = _logService.GetLogsByDate(data);
            return Ok(new { success = true, logs, data = data.ToString("dd/MM/yyyy") });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter logs por data");
            return StatusCode(500, new { success = false, error = ex.Message });
        }
    }

    /****************************************************************************************
     * ⚡ FUNÇÃO: ListarArquivos
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Listar arquivos de log disponíveis no armazenamento.
     *
     * 📥 ENTRADAS     : Nenhuma.
     *
     * 📤 SAÍDAS       : JSON com lista de arquivos (nome, data, tamanho).
     *
     * 🔗 CHAMADA POR  : Tela de logs (download/consulta).
     *
     * 🔄 CHAMA        : _logService.GetLogFiles().
     ****************************************************************************************/
    [HttpGet]
    [Route("ListarArquivos")]
    public IActionResult ListarArquivos()
    {
        try
        {
            var files = _logService.GetLogFiles();
            return Ok(new
            {
                success = true,
                arquivos = files.Select(f => new
                {
                    f.FileName,
                    Data = f.Date.ToString("dd/MM/yyyy"),
                    f.SizeFormatted
                })
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar arquivos de log");
            return StatusCode(500, new { success = false, error = ex.Message });
        }
    }

    /****************************************************************************************
     * ⚡ FUNÇÃO: ObterEstatisticas
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Retornar estatísticas agregadas dos logs.
     *
     * 📥 ENTRADAS     : Nenhuma.
     *
     * 📤 SAÍDAS       : JSON com contadores totais e de erros.
     *
     * 🔗 CHAMADA POR  : Dashboard/indicadores de logs.
     *
     * 🔄 CHAMA        : _logService.GetStats(), _logService.GetErrorCount().
     ****************************************************************************************/
    [HttpGet]
    [Route("ObterEstatisticas")]
    public IActionResult ObterEstatisticas()
    {
        try
        {
            var stats = _logService.GetStats();
            var errorCount = _logService.GetErrorCount();

            return Ok(new
            {
                success = true,
                stats = new
                {
                    stats.TotalLogs,
                    stats.ErrorCount,
                    stats.WarningCount,
                    stats.InfoCount,
                    stats.JSErrorCount,
                    stats.HttpErrorCount,
                    TotalErros = errorCount
                }
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter estatísticas");
            return StatusCode(500, new { success = false, error = ex.Message });
        }
    }

    /****************************************************************************************
     * ⚡ FUNÇÃO: LimparLogs
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Limpar logs do dia atual.
     *
     * 📥 ENTRADAS     : Nenhuma.
     *
     * 📤 SAÍDAS       : JSON com mensagem de sucesso/erro.
     *
     * 🔗 CHAMADA POR  : Ação administrativa de limpeza.
     *
     * 🔄 CHAMA        : _logService.ClearLogs(), _logService.UserAction().
     ****************************************************************************************/
    [HttpPost]
    [Route("LimparLogs")]
    public IActionResult LimparLogs()
    {
        try
        {
            _logService.ClearLogs();
            _logService.UserAction("Limpou todos os logs do dia");

            return Ok(new { success = true, message = "Logs limpos com sucesso" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao limpar logs");
            return StatusCode(500, new { success = false, error = ex.Message });
        }
    }

    /****************************************************************************************
     * ⚡ FUNÇÃO: LimparLogsAntigos
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Limpar logs anteriores a uma data limite (retenção).
     *
     * 📥 ENTRADAS     : diasManter (int) - quantidade de dias a manter.
     *
     * 📤 SAÍDAS       : JSON com mensagem de sucesso/erro.
     *
     * 🔗 CHAMADA POR  : Ação administrativa de limpeza por retenção.
     *
     * 🔄 CHAMA        : _logService.ClearLogsBefore(), _logService.UserAction().
     ****************************************************************************************/
    [HttpPost]
    [Route("LimparLogsAntigos")]
    public IActionResult LimparLogsAntigos([FromQuery] int diasManter = 30)
    {
        try
        {
            var dataLimite = DateTime.Now.AddDays(-diasManter);
            _logService.ClearLogsBefore(dataLimite);
            _logService.UserAction($"Limpou logs anteriores a {dataLimite:dd/MM/yyyy}");

            return Ok(new { success = true, message = $"Logs anteriores a {dataLimite:dd/MM/yyyy} foram limpos" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao limpar logs antigos");
            return StatusCode(500, new { success = false, error = ex.Message });
        }
    }

    /****************************************************************************************
     * ⚡ FUNÇÃO: DownloadLog
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Fazer download do arquivo de log de uma data específica.
     *
     * 📥 ENTRADAS     : data (DateTime) via query string.
     *
     * 📤 SAÍDAS       : Arquivo texto com os logs da data informada.
     *
     * 🔗 CHAMADA POR  : Ação de download na tela de logs.
     *
     * 🔄 CHAMA        : _logService.GetLogsByDate().
     ****************************************************************************************/
    [HttpGet]
    [Route("DownloadLog")]
    public IActionResult DownloadLog([FromQuery] DateTime data)
    {
        try
        {
            var logs = _logService.GetLogsByDate(data);
            var fileName = $"frotix_log_{data:yyyy-MM-dd}.txt";

            var bytes = System.Text.Encoding.UTF8.GetBytes(logs);
            return File(bytes, "text/plain", fileName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao fazer download do log");
            return StatusCode(500, new { success = false, error = ex.Message });
        }
    }

    /****************************************************************************************
     * ⚡ FUNÇÃO: AnalisarErroTexto
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Analisar um erro enviado como texto com Claude AI.
     *
     * 📥 ENTRADAS     : [AnalyzeTextRequest] request com detalhes do erro.
     *
     * 📤 SAÍDAS       : JSON com análise do Claude (diagnóstico, correção, prevenção).
     *
     * 🔗 CHAMADA POR  : Página de LogErros, botão "Analisar com IA".
     *
     * 🔄 CHAMA        : IClaudeAnalysisService.AnalyzeErrorAsync().
     ****************************************************************************************/
    [HttpPost]
    [Route("AnalisarErroTexto")]
    public async System.Threading.Tasks.Task<IActionResult> AnalisarErroTexto([FromBody] AnalyzeTextRequest request)
    {
        try
        {
            if (request == null || string.IsNullOrEmpty(request.Mensagem))
            {
                return Ok(new { success = false, error = "Dados do erro não fornecidos" });
            }

            var claudeService = HttpContext.RequestServices.GetService<IClaudeAnalysisService>();

            if (claudeService == null)
            {
                return Ok(new { success = false, error = "Serviço de análise não disponível" });
            }

            if (!claudeService.IsConfigured)
            {
                return Ok(new { success = false, error = "API Key do Claude não configurada. Configure em appsettings.json na seção 'ClaudeAI'." });
            }

            // Cria um LogErro temporário para análise
            var logErro = new FrotiX.Models.LogErro
            {
                Tipo = request.Tipo ?? "ERROR",
                Origem = request.Origem ?? "SERVER",
                Mensagem = request.Mensagem,
                Arquivo = request.Arquivo,
                Metodo = request.Metodo,
                Linha = request.Linha,
                Coluna = request.Coluna,
                StackTrace = request.Stack,
                Url = request.Url,
                UserAgent = request.UserAgent,
                DataHora = request.DataHora ?? DateTime.Now,
                Usuario = User?.Identity?.Name ?? "Anônimo"
            };

            _logger.LogInformation("Iniciando análise de erro via texto com Claude AI");

            var result = await claudeService.AnalyzeErrorAsync(logErro);

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
            _logger.LogError(ex, "Erro ao analisar erro via texto com Claude");
            return Ok(new { success = false, error = ex.Message });
        }
    }
}

/* ****************************************************************************************
 * ⚡ CLASSE: LogJavaScriptRequest
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Transportar dados de erro capturados no JavaScript.
 *
 * 📥 ENTRADAS     : Mensagem, arquivo, método, linha, coluna, stack, user agent e URL.
 *
 * 📤 SAÍDAS       : Objeto usado no endpoint LogJavaScript.
 **************************************************************************************** */
public class LogJavaScriptRequest
{
    public string Mensagem { get; set; } = "";
    public string? Arquivo { get; set; }
    public string? Metodo { get; set; }
    public int? Linha { get; set; }
    public int? Coluna { get; set; }
    public string? Stack { get; set; }
    public string? UserAgent { get; set; }
    public string? Url { get; set; }
    public string? Timestamp { get; set; }
}

/* ****************************************************************************************
 * ⚡ CLASSE: LogConsoleRequest
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Transportar dados de logs do console capturados no navegador.
 *
 * 📥 ENTRADAS     : Tipo (INFO/WARN/ERROR/DEBUG), mensagem, arquivo, método, linha,
 *                   coluna, stack, user agent e URL.
 *
 * 📤 SAÍDAS       : Objeto usado no endpoint LogConsole.
 **************************************************************************************** */
public class LogConsoleRequest
{
    public string? Tipo { get; set; } // INFO, WARN, ERROR, DEBUG
    public string Mensagem { get; set; } = "";
    public string? Arquivo { get; set; }
    public string? Metodo { get; set; }
    public int? Linha { get; set; }
    public int? Coluna { get; set; }
    public string? Stack { get; set; }
    public string? UserAgent { get; set; }
    public string? Url { get; set; }
    public string? Timestamp { get; set; }
}

/* ****************************************************************************************
 * ⚡ CLASSE: AnalyzeTextRequest
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Transportar dados de erro para análise via Claude AI.
 *
 * 📥 ENTRADAS     : Tipo, origem, mensagem, arquivo, método, linha, coluna, stack,
 *                   URL, user agent e data/hora do erro.
 *
 * 📤 SAÍDAS       : Objeto usado no endpoint AnalisarErroTexto.
 **************************************************************************************** */
public class AnalyzeTextRequest
{
    public string? Tipo { get; set; }
    public string? Origem { get; set; }
    public string Mensagem { get; set; } = "";
    public string? Arquivo { get; set; }
    public string? Metodo { get; set; }
    public int? Linha { get; set; }
    public int? Coluna { get; set; }
    public string? Stack { get; set; }
    public string? UserAgent { get; set; }
    public string? Url { get; set; }
    public DateTime? DataHora { get; set; }
}

/* ****************************************************************************************
 * ⚡ CLASSE: LogClientRequest
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Transportar dados de erros HTTP e erros globais do cliente.
 *
 * 📥 ENTRADAS     : Tipo, mensagem, statusCode, requestId, arquivo, método, linha,
 *                   coluna, stack, URL, user agent e detalhes adicionais.
 *
 * 📤 SAÍDAS       : Objeto usado no endpoint Client.
 **************************************************************************************** */
public class LogClientRequest
{
    public string? Tipo { get; set; }           // HTTP-ERROR, GLOBAL-ERROR, UNHANDLED-PROMISE
    public string Mensagem { get; set; } = "";
    public int? StatusCode { get; set; }        // Status HTTP (0 para erros de rede)
    public string? RequestId { get; set; }      // ID da requisição para rastreamento
    public string? TextStatus { get; set; }     // Status text do AJAX (timeout, error, etc)
    public string? ErrorThrown { get; set; }    // Erro lançado (para jQuery AJAX)
    public string? Arquivo { get; set; }
    public string? Metodo { get; set; }
    public int? Linha { get; set; }
    public int? Coluna { get; set; }
    public string? Stack { get; set; }
    public string? UserAgent { get; set; }
    public string? Url { get; set; }
    public string? Timestamp { get; set; }
    public string? Detalhes { get; set; }       // JSON com detalhes extras
}
