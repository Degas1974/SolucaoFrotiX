using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FrotiX.Services.DocGenerator.Interfaces;
using FrotiX.Services.DocGenerator.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using FrotiX.Helpers;
using FrotiX.Services;

namespace FrotiX.Controllers.Api
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
    *  #   MODULO:  DOCGENERATOR (MOTOR DE DOCUMENTAÇÃO IA)                                            #
    *  #   DATA:    2026 (Modernização FrotiX 2026)                                                   #
    *  #                                                                                               #
    *  #################################################################################################
    */

    /// ╔══════════════════════════════════════════════════════════════════════════════╗
    /// ║ 📌 NOME: DocGeneratorController                                           ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 📝 DESCRIÇÃO:                                                                ║
    /// ║    Orquestrador de geração de documentação técnica.                           ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 🔗 ESCOPO: API REST                                                          ║
    /// ║    • Rota base: /api/DocGenerator                                          ║
    /// ╚══════════════════════════════════════════════════════════════════════════════╝
    [Route("api/[controller]")]
    [ApiController]
    [IgnoreAntiforgeryToken]
    public class DocGeneratorController : Controller
    {
        private readonly IFileDiscoveryService _discoveryService;
        private readonly IDocGeneratorOrchestrator _orchestrator;
        private readonly IDocCacheService _cacheService;
        private readonly ILogger<DocGeneratorController> _logger;
        private readonly ILogService _log;

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: DocGeneratorController (Construtor)                             ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Injeta serviços de descoberta, orquestração e cache.                      ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • discoveryService, orchestrator, cacheService                           ║
        /// ║    • logger, log                                                            ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        public DocGeneratorController(
            IFileDiscoveryService discoveryService,
            IDocGeneratorOrchestrator orchestrator,
            IDocCacheService cacheService,
            ILogger<DocGeneratorController> logger,
            ILogService log
        )
        {
            try
            {
                _discoveryService = discoveryService;
                _orchestrator = orchestrator;
                _cacheService = cacheService;
                _logger = logger;
                _log = log;
            }
            catch (Exception ex)
            {
                Alerta.TratamentoErroComLinha("DocGeneratorController.cs", "Constructor", ex);
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: Discover (GET)                                                ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Escaneia workspace e retorna métricas de documentação.                    ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • ct (CancellationToken): cancelamento.                                  ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com métricas e árvore.                              ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpGet("discover")]
        public async Task<IActionResult> Discover(CancellationToken ct)
        {
            try
            {
                // [PROCESSAMENTO] Descoberta de arquivos.
                var result = await _discoveryService.DiscoverAsync(ct);
                return Json(new
                {
                    success = true,
                    data = new
                    {
                        totalFiles = result.TotalFiles,
                        filesWithDocs = result.FilesWithDocs,
                        filesNeedingUpdate = result.FilesNeedingUpdate,
                        filesByCategory = result.FilesByCategory,
                        tree = result.RootNode
                    }
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("DocGeneratorController.cs", "Discover", error);
                return Json(new { success = false, message = error.Message });
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: GetTree (GET)                                                 ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Retorna árvore de diretórios do workspace.                               ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com estrutura de diretórios.                        ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpGet("tree")]
        public IActionResult GetTree()
        {
            try
            {
                var tree = _discoveryService.GetFolderTree();
                return Json(new { success = true, data = tree });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("DocGeneratorController.cs", "GetTree", error);
                return Json(new { success = false, message = error.Message });
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: Generate (POST)                                              ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Inicia job de geração de documentação em background.                      ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • request (DocGenerationRequest)                                         ║
        /// ║    • ct (CancellationToken)                                                 ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com ID do job.                                      ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        /// ║ 📞 FUNÇÕES QUE CHAMA:                                                        ║
        /// ║    • _orchestrator.StartJobInBackgroundAsync() → inicia job.                  ║
        /// ║    • ParseAiProvider() / ParseDetailLevel() → parsing de opções.             ║
        /// ║    • Alerta.TratamentoErroComLinha() → tratamento de erros.                  ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📲 CHAMADA POR:                                                              ║
        /// ║    • POST /api/DocGenerator/generate                                         ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 🔗 ESCOPO: EXTERNA - DocGenerator                                             ║
        /// ║    • Arquivos relacionados: wwwroot/js/docgenerator.js                        ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        /// </summary>
        [HttpPost("generate")]
        public async Task<IActionResult> Generate([FromBody] DocGenerationRequest request, CancellationToken ct)
        {
            try
            {
                // [DEBUG] Log de parâmetros de geração
                _logger.LogInformation(
                    "Generate request: ForceRegen={Force}, UseAi={UseAi}, Provider={Provider}, Paths={Paths}",
                    request.ForceRegenerate,
                    request.UseAi,
                    request.AiProvider,
                    request.SelectedPaths?.Count ?? 0);

                // [DADOS] Montagem das opções de geração
                var options = new DocGenerationOptions
                {
                    OnlyModified = request.OnlyModified,
                    ForceRegenerate = request.ForceRegenerate,
                    UseAi = request.UseAi,
                    AiProvider = ParseAiProvider(request.AiProvider),
                    AiModel = request.AiModel ?? "",
                    DetailLevel = ParseDetailLevel(request.DetailLevel),
                    SelectedPaths = request.SelectedPaths ?? new List<string>(),
                    GenerateHtml = request.GenerateHtml,
                    GenerateMarkdown = request.GenerateMarkdown
                };

                // Iniciar o job em background para não bloquear a requisição HTTP
                // O job será processado assincronamente e o progresso será enviado via SignalR
                // [LOGICA] Orquestração do Job
                var job = await _orchestrator.StartJobInBackgroundAsync(options, ct);

                return Json(new
                {
                    success = true,
                    data = new
                    {
                        jobId = job.JobId,
                        startedAt = job.StartedAt,
                        totalFiles = job.TotalFiles
                    }
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("DocGeneratorController.cs", "Generate", error);
                return Json(new { success = false, message = error.Message });
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: GetJobStatus (GET)                                            ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Retorna status detalhado do job.                                         ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • jobId (string), ct (CancellationToken)                                 ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com status e métricas.                              ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpGet("job/{jobId}")]
        public async Task<IActionResult> GetJobStatus(string jobId, CancellationToken ct)
        {
            try
            {
            // [DADOS] Recupera status do job
                var job = await _orchestrator.GetJobStatusAsync(jobId, ct);

                if (job == null)
                    return Json(new { success = false, message = "Job não encontrado" });

                return Json(new
                {
                    success = true,
                    data = new
                    {
                        jobId = job.JobId,
                        startedAt = job.StartedAt,
                        completedAt = job.CompletedAt,
                        isRunning = job.IsRunning,
                        isCancelled = job.IsCancelled,
                        totalFiles = job.TotalFiles,
                        processedFiles = job.ProcessedFiles,
                        successCount = job.SuccessCount,
                        failedCount = job.FailedCount,
                        skippedCount = job.SkippedCount,
                        currentFile = job.CurrentFile,
                        currentStatus = job.CurrentStatus.ToString(),
                        results = job.Results
                    }
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("DocGeneratorController.cs", "GetJobStatus", error);
                return Json(new { success = false, message = error.Message });
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: CancelJob (POST)                                              ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Cancela um job em execução.                                             ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • jobId (string), ct (CancellationToken)                                 ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com status.                                       ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpPost("job/{jobId}/cancel")]
        public async Task<IActionResult> CancelJob(string jobId, CancellationToken ct)
        {
            try
            {
                await _orchestrator.CancelJobAsync(jobId, ct);
                return Json(new { success = true, message = "Job cancelado" });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("DocGeneratorController.cs", "CancelJob", error);
                return Json(new { success = false, message = error.Message });
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: ListJobs (GET)                                               ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Lista jobs recentes.                                                     ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • count (int), ct (CancellationToken)                                    ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com lista de jobs.                                 ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpGet("jobs")]
        public async Task<IActionResult> ListJobs([FromQuery] int count = 10, CancellationToken ct = default)
        {
            try
            {
                var jobs = await _orchestrator.ListRecentJobsAsync(count, ct);

                return Json(new
                {
                    success = true,
                    data = jobs.ConvertAll(j => new
                    {
                        jobId = j.JobId,
                        startedAt = j.StartedAt,
                        completedAt = j.CompletedAt,
                        isRunning = j.IsRunning,
                        totalFiles = j.TotalFiles,
                        successCount = j.SuccessCount,
                        failedCount = j.FailedCount,
                        skippedCount = j.SkippedCount
                    })
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("DocGeneratorController.cs", "ListJobs", error);
                return Json(new { success = false, message = error.Message });
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: ClearCache (POST)                                            ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Limpa cache de documentação e metadados.                                 ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • ct (CancellationToken)                                                ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com status.                                       ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpPost("cache/clear")]
        public async Task<IActionResult> ClearCache(CancellationToken ct)
        {
            try
            {
                await _cacheService.ClearAsync(ct);
                return Json(new { success = true, message = "Cache limpo" });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("DocGeneratorController.cs", "ClearCache", error);
                return Json(new { success = false, message = error.Message });
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: GetProviders (GET)                                           ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Lista provedores de IA disponíveis/configurados.                         ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • options (IOptions<DocGeneratorSettings>)                               ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com lista de providers.                            ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpGet("providers")]
        public IActionResult GetProviders([FromServices] IOptions<DocGeneratorSettings> options)
        {
            try
            {
                var settings = options.Value;

                // Verifica se cada provider tem API Key configurada
                return Json(new
                {
                    success = true,
                    data = new[]
                    {
                        new {
                            id = "none",
                            name = "Sem IA (Heurísticas)",
                            available = true,
                            configured = true,
                            configHint = ""
                        },
                        new {
                            id = "openai",
                            name = "OpenAI (GPT-5.2)",
                            available = true,
                            configured = settings.OpenAi.IsConfigured,
                            configHint = settings.OpenAi.IsConfigured ? "" : "Configure DOCGENERATOR_OPENAI_APIKEY"
                        },
                        new {
                            id = "claude",
                            name = "Claude 4.5 (Anthropic)",
                            available = true,
                            configured = settings.Claude.IsConfigured,
                            configHint = settings.Claude.IsConfigured ? "" : "Configure DOCGENERATOR_CLAUDE_APIKEY"
                        },
                        new {
                            id = "gemini",
                            name = "Gemini 3 Pro (Google)",
                            available = true,
                            configured = settings.Gemini.IsConfigured,
                            configHint = settings.Gemini.IsConfigured ? "" : "Configure DOCGENERATOR_GEMINI_APIKEY"
                        }
                    }
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("DocGeneratorController.cs", "GetProviders", error);
                return Json(new { success = false, message = error.Message });
            }
        }
        /// <summary>
        /// Executa testes unitários do DocGenerator
        /// </summary>
      /*  [HttpGet("tests/run")]
        public IActionResult RunTests()
        {
            try
            {
                var report = FrotiX.Tests.DocGenerator.DocGeneratorTests.RunAllTests();
                return Json(new
                {
                    success = true,
                    data = new
                    {
                        report = report,
                        executedAt = DateTime.Now
                    }
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("DocGeneratorController.cs", "RunTests", error);
                return Json(new { success = false, message = error.Message });
            }
        } */


        #region Helper Methods

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: ParseAiProvider (Privado)                                     ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Converte string para enum AiProvider.                                   ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        private AiProvider ParseAiProvider(string? provider)
        {
            return provider?.ToLowerInvariant() switch
            {
                "openai" => AiProvider.OpenAI,
                "claude" => AiProvider.Claude,
                "gemini" => AiProvider.Gemini,
                _ => AiProvider.None
            };
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: ParseDetailLevel (Privado)                                   ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Converte string para enum DetailLevel.                                  ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        private DetailLevel ParseDetailLevel(string? level)
        {
            return level?.ToLowerInvariant() switch
            {
                "minimal" => DetailLevel.Minimal,
                "detailed" => DetailLevel.Detailed,
                "exhaustive" => DetailLevel.Exhaustive,
                _ => DetailLevel.Standard
            };
        }

        #endregion
    }

    /// ╔══════════════════════════════════════════════════════════════════════════════╗
    /// ║ 📌 NOME: DocGenerationRequest                                             ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 📝 DESCRIÇÃO:                                                                ║
    /// ║    DTO para requisição de geração de documentação.                           ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 📦 PROPRIEDADES:                                                             ║
    /// ║    • OnlyModified, ForceRegenerate, UseAi                                    ║
    /// ║    • AiProvider, AiModel, DetailLevel                                        ║
    /// ║    • SelectedPaths, GenerateHtml, GenerateMarkdown                           ║
    /// ╚══════════════════════════════════════════════════════════════════════════════╝
    public class DocGenerationRequest
    {
        public bool OnlyModified { get; set; }
        public bool ForceRegenerate { get; set; }
        public bool UseAi { get; set; }
        public string? AiProvider { get; set; }
        public string? AiModel { get; set; }
        public string? DetailLevel { get; set; }
        public List<string>? SelectedPaths { get; set; }
        public bool GenerateHtml { get; set; } = true;
        public bool GenerateMarkdown { get; set; } = true;
    }
}
