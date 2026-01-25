using System;
using System.Linq;
using FrotiX.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FrotiX.Controllers
{
    /*
    *  #################################################################################################
    *  #                                                                                               #
    *  #   ███████╗██████╗  ██████╗ ████████╗██╗██╗  ██╗    ██████╗  ██████╗ ██████╗  ██████╗          #
    *  #   ██╔════╝██╔══██╗██╔═══██╗╚══██╔══╝██║╚██╗██╔╝    ╚════██╗██╔═████╗╚════██╗██╔════╝          #
    *  #   █████╗  ██████╔╝██║   ██║   ██║   ██║ ╚███╔╝      █████╔╝██║██╔██║ █████╔╝███████╗          #
    *  #   ██╔══╝  ██╔══██╗██║   ██║   ██║   ██║ ██╔██╗     ██╔═══╝ ████╔╝██║██╔═══╝ ██╔═══██╗          #
    *  #   ██║     ██║  ██║╚██████╔╝   ██║   ██║██╔╝ ██╗    ███████╗╚██████╔╝███████╗╚██████╔╝          #
    *  #   ╚═╝     ╚═╝  ╚═╝ ╚═════╝    ╚═╝   ╚═╝╚═╝  ╚═╝    ╚══════╝ ╚═════╝ ╚══════╝ ╚═════╝           #
    *  #                                                                                               #
    *  #   PROJETO: FROTIX - SOLUÇÃO INTEGRADA DE GESTÃO DE FROTAS                                     #
    *  #   MODULO:  SISTEMA CENTRAL DE TELEMETRIA E LOGS (FROTIX AUDIT)                                #
    *  #   DATA:    2026 (Modernização FrotiX 2026)                                                   #
    *  #                                                                                               #
    *  #################################################################################################
    */

    /// ╔══════════════════════════════════════════════════════════════════════════════╗
    /// ║ 📌 NOME: LogErrosController                                                  ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 📝 DESCRIÇÃO:                                                                ║
    /// ║    API centralizada para recepção, auditoria e consulta de logs técnicos.    ║
    /// ║    Integra o FrotiX Audit para erros de backend e JavaScript.                ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 🔗 ESCOPO: API REST                                                          ║
    /// ║    • Rota base: /api/LogErros                                               ║
    /// ╚══════════════════════════════════════════════════════════════════════════════╝
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public partial class LogErrosController : ControllerBase
    {
        private readonly ILogService _log;
        private readonly ILogger<LogErrosController> _logger;

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: LogErrosController (Construtor)                                    ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Inicializa o hub de auditoria e telemetria de erros.                      ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • log (ILogService): Serviço de log centralizado.                         ║
        /// ║    • logger (ILogger<LogErrosController>): Logger do framework.              ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        public LogErrosController(ILogService log, ILogger<LogErrosController> logger)
        {
            try
            {
                _log = log;
                _logger = logger;
            }
            catch (Exception ex)
            {
                Alerta.TratamentoErroComLinha("LogErrosController.cs", "LogErrosController", ex);
                Console.WriteLine($"FATAL: Erro construtor LogErrosController: {ex.Message}");
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: LogJavaScript (POST)                                             ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Recebe exceções do navegador (window.onerror ou try-catch).               ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • request (LogJavaScriptRequest): Detalhes do erro JS.                    ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: Status da gravação.                                      ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpPost]
        [Route("LogJavaScript")]
        public IActionResult LogJavaScript([FromBody] LogJavaScriptRequest request)
        {
            try
            {
                // [VALIDACAO] Verifica payload mínimo.
                if (request == null || string.IsNullOrEmpty(request.Mensagem))
                {
                    return BadRequest(new { success = false, error = "Dados de log inválidos" });
                }

                // [LOG] Registra erro JavaScript no serviço centralizado.
                _log.ErrorJS(
                    request.Mensagem,
                    request.Arquivo,
                    request.Metodo,
                    request.Linha,
                    request.Coluna,
                    request.Stack,
                    request.UserAgent,
                    request.Url
                );

                // [RETORNO] Confirma gravação.
                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message, ex, "LogErrosController.cs", "LogJavaScript");
                Alerta.TratamentoErroComLinha("LogErrosController.cs", "LogJavaScript", ex);
                _logger.LogError(ex, "Erro ao registrar log JavaScript");
                return StatusCode(500, new { success = false, error = "Erro interno ao processar log" });
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: ObterLogs (GET)                                                  ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Obtém todos os logs registrados no dia atual.                            ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com logs e estatísticas.                            ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpGet]
        [Route("ObterLogs")]
        public IActionResult ObterLogs()
        {
            try
            {
                // [DADOS] Carrega logs e estatísticas do dia.
                var logs = _log.GetAllLogs() ?? "";
                var stats = _log.GetStats();

                // [SERIALIZACAO] Serializa manualmente para evitar interceptadores.
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

                // [RETORNO] Conteúdo JSON no formato esperado.
                return Content(json, "application/json", System.Text.Encoding.UTF8);
            }
            catch (Exception ex)
            {
                Alerta.TratamentoErroComLinha("LogErrosController.cs", "ObterLogs", ex);
                _logger.LogError(ex, "Erro ao obter logs");
                var errorJson = System.Text.Json.JsonSerializer.Serialize(new { success = false, error = ex.Message, logs = "" });
                return Content(errorJson, "application/json", System.Text.Encoding.UTF8);
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: ObterLogsPorData (GET)                                           ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Obtém logs de uma data específica.                                       ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • data (DateTime): Data alvo.                                             ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com logs da data.                                   ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpGet]
        [Route("ObterLogsPorData")]
        public IActionResult ObterLogsPorData([FromQuery] DateTime data)
        {
            try
            {
                // [DADOS] Carrega logs da data informada.
                var logs = _log.GetLogsByDate(data);
                return Ok(new { success = true, logs, data = data.ToString("dd/MM/yyyy") });
            }
            catch (Exception ex)
            {
                Alerta.TratamentoErroComLinha("LogErrosController.cs", "ObterLogsPorData", ex);
                _logger.LogError(ex, "Erro ao obter logs por data");
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: ListarArquivos (GET)                                            ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Lista arquivos de log disponíveis.                                      ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com metadados dos arquivos.                        ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpGet]
        [Route("ListarArquivos")]
        public IActionResult ListarArquivos()
        {
            try
            {
                // [DADOS] Recupera lista de arquivos de log.
                var files = _log.GetLogFiles();
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
                Alerta.TratamentoErroComLinha("LogErrosController.cs", "ListarArquivos", ex);
                _logger.LogError(ex, "Erro ao listar arquivos de log");
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: ObterEstatisticas (GET)                                        ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Obtém estatísticas dos logs.                                             ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com estatísticas consolidadas.                     ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpGet]
        [Route("ObterEstatisticas")]
        public IActionResult ObterEstatisticas()
        {
            try
            {
                // [DADOS] Coleta estatísticas e contagem de erros.
                var stats = _log.GetStats();
                var errorCount = _log.GetErrorCount();

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
                Alerta.TratamentoErroComLinha("LogErrosController.cs", "ObterEstatisticas", ex);
                _logger.LogError(ex, "Erro ao obter estatísticas");
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: LimparLogs (POST)                                              ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Limpa logs do dia atual.                                                ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: Status da limpeza.                                      ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpPost]
        [Route("LimparLogs")]
        public IActionResult LimparLogs()
        {
            try
            {
                // [ACAO] Limpa logs e registra ação do usuário.
                _log.ClearLogs();
                _log.UserAction("Limpou todos os logs do dia");

                return Ok(new { success = true, message = "Logs limpos com sucesso" });
            }
            catch (Exception ex)
            {
                Alerta.TratamentoErroComLinha("LogErrosController.cs", "LimparLogs", ex);
                _logger.LogError(ex, "Erro ao limpar logs");
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: LimparLogsAntigos (POST)                                       ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Limpa logs anteriores a uma data baseada em dias de retenção.            ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • diasManter (int): Dias a manter.                                        ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: Status da limpeza.                                      ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpPost]
        [Route("LimparLogsAntigos")]
        public IActionResult LimparLogsAntigos([FromQuery] int diasManter = 30)
        {
            try
            {
                // [CALCULO] Define data limite com base na retenção.
                var dataLimite = DateTime.Now.AddDays(-diasManter);
                // [ACAO] Limpa logs anteriores e registra ação.
                _log.ClearLogsBefore(dataLimite);
                _log.UserAction($"Limpou logs anteriores a {dataLimite:dd/MM/yyyy}");

                return Ok(new { success = true, message = $"Logs anteriores a {dataLimite:dd/MM/yyyy} foram limpos" });
            }
            catch (Exception ex)
            {
                Alerta.TratamentoErroComLinha("LogErrosController.cs", "LimparLogsAntigos", ex);
                _logger.LogError(ex, "Erro ao limpar logs antigos");
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: DownloadLog (GET)                                              ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Download de arquivo de log específico.                                   ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • data (DateTime): Data do log.                                           ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: Arquivo .txt com o log.                                  ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpGet]
        [Route("DownloadLog")]
        public IActionResult DownloadLog([FromQuery] DateTime data)
        {
            try
            {
                // [DADOS] Carrega logs da data.
                var logs = _log.GetLogsByDate(data);
                var fileName = $"frotix_log_{data:yyyy-MM-dd}.txt";

                // [ARQUIVO] Serializa para bytes e retorna download.
                var bytes = System.Text.Encoding.UTF8.GetBytes(logs);
                return File(bytes, "text/plain", fileName);
            }
            catch (Exception ex)
            {
                Alerta.TratamentoErroComLinha("LogErrosController.cs", "DownloadLog", ex);
                _logger.LogError(ex, "Erro ao fazer download do log");
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }
    }

    /// ╔══════════════════════════════════════════════════════════════════════════════╗
    /// ║ 📌 NOME: LogJavaScriptRequest (Model)                                        ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 📝 DESCRIÇÃO:                                                                ║
    /// ║    Modelo de request para logs JavaScript.                                   ║
    /// ╚══════════════════════════════════════════════════════════════════════════════╝
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
}
