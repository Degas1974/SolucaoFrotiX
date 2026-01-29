/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: LogErrosController.cs                                                                   ║
   ║ 📂 CAMINHO: /Controllers                                                                            ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO: Receber/gerenciar logs de erro (JS client + C# server). Monitoramento centralizado.    ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 ÍNDICE: LogJavaScript(), GetAll() - [AllowAnonymous] para capturar erros sem autenticação        ║
   ║ 🔗 DEPS: ILogService, ILogger | 📅 28/01/2026 | 👤 Copilot | 📝 v2.0                                ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝
*/

/****************************************************************************************
 * ⚡ CONTROLLER: LogErrosController (Partial Class)
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Receber e gerenciar logs de erro (JavaScript client-side e C# server-side)
 *                   Centraliza tratamento de erros para monitoramento e debug
 * 📥 ENTRADAS     : LogJavaScriptRequest (erros do frontend), Filtros de visualização
 * 📤 SAÍDAS       : JSON com confirmação de log salvo, listas de erros
 * 🔗 CHAMADA POR  : JavaScript (window.onerror), Alerta.TratamentoErroComLinha, Pages/LogErros
 * 🔄 CHAMA        : ILogService (salva logs no banco/arquivo), ILogger
 * 📦 DEPENDÊNCIAS : ASP.NET Core, ILogService, Microsoft.Extensions.Logging
 *
 * 🔓 SEGURANÇA:
 *    - [AllowAnonymous]: Permite logs mesmo sem autenticação (para capturar todos os erros)
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
     * 🎯 OBJETIVO     : Injetar serviços de log
     ****************************************************************************************/
    public LogErrosController(ILogService logService, ILogger<LogErrosController> logger)
    {
        _logService = logService;
        _logger = logger;
    }

    /// <summary>
    /// Recebe logs de erro do JavaScript (client-side)
    /// </summary>
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

    /// <summary>
    /// Obtém todos os logs do dia atual
    /// </summary>
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

    /// <summary>
    /// Obtém logs de uma data específica
    /// </summary>
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

    /// <summary>
    /// Lista arquivos de log disponíveis
    /// </summary>
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

    /// <summary>
    /// Obtém estatísticas dos logs
    /// </summary>
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

    /// <summary>
    /// Limpa logs do dia atual
    /// </summary>
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

    /// <summary>
    /// Limpa logs anteriores a uma data
    /// </summary>
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

    /// <summary>
    /// Download de arquivo de log específico
    /// </summary>
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

/// <summary>
/// Request model para logs JavaScript
/// </summary>
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
