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
using System.Linq;
using FrotiX.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
     * ⚡ FUNÇÃO: LogJavaScript
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Receber logs de erro do JavaScript (client-side).
     *
     * 📥 ENTRADAS     : [LogJavaScriptRequest] request (mensagem, arquivo, linha, stack, etc).
     *
     * 📤 SAÍDAS       : JSON com sucesso ou erro de validação/processamento.
     *
     * 🔗 CHAMADA POR  : window.onerror / handlers de erro no frontend.
     *
     * 🔄 CHAMA        : _logService.ErrorJS().
     ****************************************************************************************/
    [HttpPost]
    [Route("LogJavaScript")]
    public IActionResult LogJavaScript([FromBody] LogJavaScriptRequest request)
    {
        try
        {
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
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao registrar log JavaScript");
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
