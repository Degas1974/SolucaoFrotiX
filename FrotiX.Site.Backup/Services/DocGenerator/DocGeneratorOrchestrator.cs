/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: DocGeneratorOrchestrator.cs                                                             ║
   ║ 📂 CAMINHO: /Services/DocGenerator                                                                  ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO: Orquestrador principal de geração de documentação. Coordena discovery, cache, render.  ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 ÍNDICE: GenerateAsync(), ProcessFileBatchAsync(), NotifyProgress(), GetEntireProgressAsync()     ║
   ║ 🔗 DEPS: IDocGeneratorOrchestrator, SignalR DocGenerationHub | 📅 13/01/2026 | 👤 Copilot | 📝 v2.0 ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝
*/

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FrotiX.Hubs;
using FrotiX.Services.DocGenerator.Interfaces;
using FrotiX.Services.DocGenerator.Models;
using FrotiX.Services.DocGenerator.Services; // Adicionado para IFileTrackingService
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FrotiX.Services.DocGenerator
{
    /// <summary>
    /// Orquestrador principal de geração de documentação
    /// </summary>
    public class DocGeneratorOrchestrator : IDocGeneratorOrchestrator
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IFileDiscoveryService _discoveryService;
        private readonly IFileTrackingService _fileTrackingService; // Novo serviço
        private readonly IHubContext<DocGenerationHub> _hubContext;
        private readonly ILogger<DocGeneratorOrchestrator> _logger;

        private readonly ConcurrentDictionary<string, DocGenerationJob> _jobs = new();
        private readonly ConcurrentDictionary<string, CancellationTokenSource> _cancellationTokens = new();

        public DocGeneratorOrchestrator(
            IServiceScopeFactory scopeFactory,
            IFileDiscoveryService discoveryService,
            IFileTrackingService fileTrackingService, // Novo parâmetro
            IHubContext<DocGenerationHub> hubContext,
            ILogger<DocGeneratorOrchestrator> logger)
        {
            try
            {
                _scopeFactory = scopeFactory;
                _discoveryService = discoveryService;
                _fileTrackingService = fileTrackingService; // Atribuir
                _hubContext = hubContext;
                _logger = logger;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("DocGeneratorOrchestrator.cs", ".ctor", error);
            }
        }

        /// <summary>
        /// Inicia um job em background - retorna imediatamente após configuração inicial
        /// O processamento ocorre em background e o progresso é enviado via SignalR
        /// </summary>
        public async Task<DocGenerationJob> StartJobInBackgroundAsync(
            DocGenerationOptions options,
            CancellationToken ct = default)
        {
            var job = new DocGenerationJob
            {
                Options = options,
                IsRunning = true
            };

            try
            {
                _jobs[job.JobId] = job;

                // Não linkar com o CancellationToken da requisição HTTP (ct)
                // Caso contrário, o job é cancelado assim que a requisição termina
                var cts = new CancellationTokenSource();
                _cancellationTokens[job.JobId] = cts;

                _logger.LogInformation("Iniciando job de geração (background): {JobId}", job.JobId);

                // Descobrir arquivos - isso é rápido
                var discovery = await _discoveryService.DiscoverAsync(cts.Token);

                // Filtrar arquivos selecionados
                var filesToProcess = GetFilesToProcess(discovery, options);
                job.TotalFiles = filesToProcess.Count;

                _logger.LogInformation(
                    "Job {JobId}: {Total} arquivos para processar. SelectedPaths: {Paths}, OnlyModified: {OnlyMod}, ForceRegen: {Force}",
                    job.JobId,
                    filesToProcess.Count,
                    options.SelectedPaths.Count > 0 ? string.Join(", ", options.SelectedPaths) : "(nenhum)",
                    options.OnlyModified,
                    options.ForceRegenerate);

                if (filesToProcess.Count == 0)
                {
                    await SendProgressAsync(job, "Nenhum arquivo para processar. Tente marcar 'Forçar regeneração'.", true);
                    job.IsRunning = false;
                    return job;
                }

                // Iniciar processamento em background (fire-and-forget com tratamento de erro)
                // IMPORTANTE: Criar cópia da lista para evitar problemas de closure
                var filesToProcessCopy = filesToProcess.ToList();
                _logger.LogInformation("StartJobInBackgroundAsync: Criada cópia da lista com {Count} arquivos", filesToProcessCopy.Count);

                _ = Task.Run(async () =>
                {
                    try
                    {
                        await ProcessJobAsync(job, filesToProcessCopy, options, cts.Token);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Erro no processamento background do job {JobId}", job.JobId);
                        job.IsRunning = false;
                        await SendProgressAsync(job, $"Erro: {ex.Message}", true, true, ex.Message);
                    }
                    finally
                    {
                        _cancellationTokens.TryRemove(job.JobId, out _);
                    }
                });

                // Retorna imediatamente com o job configurado
                return job;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("DocGeneratorOrchestrator.cs", "StartJobInBackgroundAsync", error);
                job.IsRunning = false;
                throw;
            }
        }

        /// <summary>
        /// Processa o job de forma assíncrona (chamado em background)
        /// Cria um escopo de DI dedicado para não depender do escopo HTTP
        /// </summary>
        private async Task ProcessJobAsync(
            DocGenerationJob job,
            List<DiscoveredFile> filesToProcess,
            DocGenerationOptions options,
            CancellationToken ct)
        {
            // Criar um escopo de serviços dedicado para o processamento em background
            // Isso é necessário porque o escopo HTTP é descartado quando a requisição termina
            using var scope = _scopeFactory.CreateScope();
            var extractionService = scope.ServiceProvider.GetRequiredService<IDocExtractionService>();
            var composerService = scope.ServiceProvider.GetRequiredService<IDocComposerService>();
            var renderService = scope.ServiceProvider.GetRequiredService<IDocRenderService>();
            var cacheService = scope.ServiceProvider.GetRequiredService<IDocCacheService>();

            try
            {
                _logger.LogInformation("ProcessJobAsync: Iniciando com {Count} arquivos. JobId={JobId}", filesToProcess.Count, job.JobId);
                await SendProgressAsync(job, $"Iniciando processamento de {filesToProcess.Count} arquivos...");

                if (filesToProcess.Count == 0)
                {
                    _logger.LogWarning("ProcessJobAsync: Lista de arquivos está vazia!");
                    await SendProgressAsync(job, "Nenhum arquivo na lista para processar.", true);
                    job.IsRunning = false;
                    return;
                }

                _logger.LogInformation("ProcessJobAsync: Começando iteração do loop foreach");
                foreach (var file in filesToProcess)
                {
                    _logger.LogInformation("ProcessJobAsync: [LOOP START] Arquivo: {File} (JobId={JobId})", file.FileName, job.JobId);

                    if (ct.IsCancellationRequested)
                    {
                        _logger.LogWarning("ProcessJobAsync: Cancelamento solicitado (via CancellationToken) para {File}", file.FileName);
                        job.IsCancelled = true;
                        break;
                    }

                    job.CurrentFile = file.RelativePath;
                    job.CurrentStatus = ProcessingStatus.Extracting;

                    _logger.LogInformation("ProcessJobAsync: Processando arquivo: {File}", file.FileName);
                    await SendProgressAsync(job, $"Processando: {file.FileName}");

                    var result = await ProcessFileCoreAsync(file, options, extractionService, composerService, renderService, cacheService, ct);
                    job.Results.Add(result);
                    job.ProcessedFiles++;

                    if (result.Status == ProcessingStatus.Completed)
                        job.SuccessCount++;
                    else if (result.Status == ProcessingStatus.Failed)
                        job.FailedCount++;
                    else if (result.Status == ProcessingStatus.Skipped)
                        job.SkippedCount++;

                    await SendProgressAsync(job, $"Concluído: {file.FileName} ({result.Status})");

                    // Aguardar entre arquivos se estiver usando IA para respeitar rate limits
                    // Claude Sonnet tem limite de 8k output tokens/minuto
                    if (options.UseAi && options.AiProvider != AiProvider.None && job.ProcessedFiles < filesToProcess.Count)
                    {
                        var delaySeconds = options.AiProvider switch
                        {
                            AiProvider.Claude => 15,  // Claude tem rate limit mais restrito
                            AiProvider.OpenAI => 5,   // OpenAI geralmente tem rate limits mais altos
                            AiProvider.Gemini => 5,   // Gemini similar ao OpenAI
                            _ => 2
                        };

                        await SendProgressAsync(job, $"Aguardando {delaySeconds}s para respeitar rate limits...");
                        await Task.Delay(TimeSpan.FromSeconds(delaySeconds), ct);
                    }
                }

                job.CompletedAt = DateTime.Now;
                job.IsRunning = false;

                await SendProgressAsync(job, $"Job finalizado! Sucesso: {job.SuccessCount}, Falha: {job.FailedCount}, Ignorados: {job.SkippedCount}", true);

                _logger.LogInformation(
                    "Job {JobId} finalizado: {Success} sucesso, {Failed} falha, {Skipped} ignorados",
                    job.JobId, job.SuccessCount, job.FailedCount, job.SkippedCount);
            }
            catch (OperationCanceledException)
            {
                job.IsCancelled = true;
                job.IsRunning = false;
                _logger.LogWarning("Job {JobId} cancelado", job.JobId);
                await SendProgressAsync(job, "Job cancelado pelo usuário.", true);
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("DocGeneratorOrchestrator.cs", "ProcessJobAsync", error);
                job.IsRunning = false;
                await SendProgressAsync(job, $"Erro: {error.Message}", true, true, error.Message);
            }
        }

        public async Task<DocGenerationJob> StartJobAsync(
            DocGenerationOptions options,
            CancellationToken ct = default)
        {
            var job = new DocGenerationJob
            {
                Options = options,
                IsRunning = true
            };

            try
            {
                _jobs[job.JobId] = job;

                var cts = CancellationTokenSource.CreateLinkedTokenSource(ct);
                _cancellationTokens[job.JobId] = cts;

                _logger.LogInformation("Iniciando job de geração: {JobId}", job.JobId);

                // Descobrir arquivos
                var discovery = await _discoveryService.DiscoverAsync(cts.Token);

                // Filtrar arquivos selecionados
                var filesToProcess = GetFilesToProcess(discovery, options);
                job.TotalFiles = filesToProcess.Count;

                _logger.LogInformation(
                    "Job {JobId}: {Total} arquivos para processar. SelectedPaths: {Paths}, OnlyModified: {OnlyMod}, ForceRegen: {Force}",
                    job.JobId,
                    filesToProcess.Count,
                    options.SelectedPaths.Count > 0 ? string.Join(", ", options.SelectedPaths) : "(nenhum)",
                    options.OnlyModified,
                    options.ForceRegenerate);

                if (filesToProcess.Count == 0)
                {
                    await SendProgressAsync(job, "Nenhum arquivo para processar. Tente marcar 'Forçar regeneração'.", true);
                    job.IsRunning = false;
                    return job;
                }

                await SendProgressAsync(job, $"Iniciando processamento de {filesToProcess.Count} arquivos...");

                // Processar arquivos
                foreach (var file in filesToProcess)
                {
                    if (cts.Token.IsCancellationRequested)
                    {
                        job.IsCancelled = true;
                        break;
                    }

                    job.CurrentFile = file.RelativePath;
                    job.CurrentStatus = ProcessingStatus.Extracting;

                    await SendProgressAsync(job, $"Processando: {file.FileName}");

                    var result = await ProcessFileAsync(file, options, cts.Token);
                    job.Results.Add(result);
                    job.ProcessedFiles++;

                    if (result.Status == ProcessingStatus.Completed)
                        job.SuccessCount++;
                    else if (result.Status == ProcessingStatus.Failed)
                        job.FailedCount++;
                    else if (result.Status == ProcessingStatus.Skipped)
                        job.SkippedCount++;

                    await SendProgressAsync(job, $"Concluído: {file.FileName}");
                }

                job.CompletedAt = DateTime.Now;
                job.IsRunning = false;

                await SendProgressAsync(job, "Job finalizado!", true);

                _logger.LogInformation(
                    "Job {JobId} finalizado: {Success} sucesso, {Failed} falha, {Skipped} ignorados",
                    job.JobId, job.SuccessCount, job.FailedCount, job.SkippedCount);
            }
            catch (OperationCanceledException)
            {
                job.IsCancelled = true;
                job.IsRunning = false;
                _logger.LogWarning("Job {JobId} cancelado", job.JobId);
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("DocGeneratorOrchestrator.cs", "StartJobAsync", error);
                job.IsRunning = false;
                await SendProgressAsync(job, $"Erro: {error.Message}", true, true, error.Message);
            }
            finally
            {
                _cancellationTokens.TryRemove(job.JobId, out _);
            }

            return job;
        }

        public async Task CancelJobAsync(string jobId, CancellationToken ct = default)
        {
            try
            {
                _logger.LogInformation("Solicitação de cancelamento recebida para job {JobId}", jobId);

                if (_cancellationTokens.TryGetValue(jobId, out var cts))
                {
                    cts.Cancel();
                    _logger.LogInformation("CancellationToken cancelado para job {JobId}", jobId);
                }

                if (_jobs.TryGetValue(jobId, out var job))
                {
                    job.IsCancelled = true;
                    job.IsRunning = false;
                    job.CompletedAt = DateTime.Now;

                    // Enviar mensagem de progresso informando o cancelamento
                    await SendProgressAsync(job, "Job cancelado pelo usuário.", true);
                    _logger.LogInformation("Job {JobId} marcado como cancelado e mensagem enviada via SignalR", jobId);
                }
                else
                {
                    _logger.LogWarning("Job {JobId} não encontrado para cancelamento", jobId);
                }
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("DocGeneratorOrchestrator.cs", "CancelJobAsync", error);
            }
        }

        public async Task<DocGenerationJob?> GetJobStatusAsync(string jobId, CancellationToken ct = default)
        {
            await Task.CompletedTask;
            return _jobs.TryGetValue(jobId, out var job) ? job : null;
        }

        public async Task<List<DocGenerationJob>> ListRecentJobsAsync(int count = 10, CancellationToken ct = default)
        {
            await Task.CompletedTask;
            return _jobs.Values
                .OrderByDescending(j => j.StartedAt)
                .Take(count)
                .ToList();
        }

        public async Task<FileProcessingResult> ProcessFileAsync(
            DiscoveredFile file,
            DocGenerationOptions options,
            CancellationToken ct = default)
        {
            // Criar escopo para garantir que os serviços estejam disponíveis
            using var scope = _scopeFactory.CreateScope();
            var extractionService = scope.ServiceProvider.GetRequiredService<IDocExtractionService>();
            var composerService = scope.ServiceProvider.GetRequiredService<IDocComposerService>();
            var renderService = scope.ServiceProvider.GetRequiredService<IDocRenderService>();
            var cacheService = scope.ServiceProvider.GetRequiredService<IDocCacheService>();

            return await ProcessFileCoreAsync(file, options, extractionService, composerService, renderService, cacheService, ct);
        }

        /// <summary>
        /// Método core de processamento que recebe os serviços como parâmetros
        /// Usado internamente para evitar problemas de escopo de DI
        /// </summary>
        private async Task<FileProcessingResult> ProcessFileCoreAsync(
            DiscoveredFile file,
            DocGenerationOptions options,
            IDocExtractionService extractionService,
            IDocComposerService composerService,
            IDocRenderService renderService,
            IDocCacheService cacheService,
            CancellationToken ct = default)
        {
            var result = new FileProcessingResult
            {
                FilePath = file.RelativePath
            };
            var sw = Stopwatch.StartNew();

            try
            {
                // Verificar se precisa regenerar
                if (!options.ForceRegenerate && !file.NeedsRegeneration)
                {
                    result.Status = ProcessingStatus.Skipped;
                    sw.Stop();
                    result.ProcessingTime = sw.Elapsed;
                    return result;
                }

                // Extrair fatos
                result.Status = ProcessingStatus.Extracting;
                var facts = await extractionService.ExtractAsync(file, ct);

                if (facts.Warnings.Any())
                {
                    _logger.LogWarning("Warnings na extração de {File}: {Warnings}",
                        file.FileName, string.Join(", ", facts.Warnings));
                }

                // Compor documentação
                result.Status = ProcessingStatus.Composing;
                _logger.LogInformation("ProcessFileCoreAsync: Chamando composerService.ComposeAsync para {File}", file.FileName);

                var composeResult = await composerService.ComposeAsync(facts, options, ct);

                _logger.LogInformation(
                    "ProcessFileCoreAsync: Resultado da composição - Success={Success}, Markdown.Length={MdLen}, TokensUsed={Tokens}, ErrorMessage={Error}",
                    composeResult.Success,
                    composeResult.Markdown?.Length ?? 0,
                    composeResult.TokensUsed,
                    composeResult.ErrorMessage ?? "(nenhum)");

                if (!composeResult.Success)
                {
                    result.Status = ProcessingStatus.Failed;
                    result.ErrorMessage = composeResult.ErrorMessage;
                    _logger.LogWarning("ProcessFileCoreAsync: Composição falhou para {File}: {Error}", file.FileName, composeResult.ErrorMessage);
                    sw.Stop();
                    result.ProcessingTime = sw.Elapsed;
                    return result;
                }

                // Verificar se o Markdown está vazio (possível problema no provider)
                if (string.IsNullOrWhiteSpace(composeResult.Markdown))
                {
                    _logger.LogError("ProcessFileCoreAsync: ALERTA! Markdown está vazio apesar de Success=true para {File}", file.FileName);
                    result.Status = ProcessingStatus.Failed;
                    result.ErrorMessage = "Provider retornou conteúdo vazio";
                    sw.Stop();
                    result.ProcessingTime = sw.Elapsed;
                    return result;
                }

                result.UsedAi = options.UseAi && options.AiProvider != AiProvider.None;
                result.TokensUsed = composeResult.TokensUsed;

                // Renderizar
                result.Status = ProcessingStatus.Rendering;
                var renderResult = await renderService.RenderAsync(facts, composeResult, options, ct);

                if (!renderResult.Success)
                {
                    result.Status = ProcessingStatus.Failed;
                    result.ErrorMessage = renderResult.ErrorMessage;
                    sw.Stop();
                    result.ProcessingTime = sw.Elapsed;
                    return result;
                }

                result.MarkdownPath = renderResult.MarkdownPath;
                result.HtmlPaths = renderResult.HtmlPaths;

                // Atualizar cache
                await cacheService.SetAsync(file.RelativePath, new DocCacheEntry
                {
                    FilePath = file.RelativePath,
                    ContentHash = file.ContentHash,
                    TemplateVersion = "2.0",
                    PromptVersion = "1.0",
                    GeneratedAt = DateTime.Now,
                    MarkdownPath = result.MarkdownPath,
                    HtmlPaths = result.HtmlPaths
                }, ct);

                // Marcar arquivo como documentado no sistema de tracking
                try
                {
                    // Obter escopo para acessar IFileTrackingService
                    using var trackingScope = _scopeFactory.CreateScope();
                    var fileTrackingService = trackingScope.ServiceProvider.GetRequiredService<IFileTrackingService>();
                    await fileTrackingService.MarkAsDocumentedAsync(file.FullPath, version: 1, ct);
                    _logger.LogInformation("Arquivo marcado como documentado no tracking: {File}", file.FileName);
                }
                catch (Exception trackingError)
                {
                    _logger.LogWarning(trackingError, "Erro ao marcar arquivo como documentado no tracking: {File}", file.FileName);
                    // Não falha o processamento se o tracking falhar
                }

                result.Status = ProcessingStatus.Completed;
            }
            catch (OperationCanceledException)
            {
                result.Status = ProcessingStatus.Failed;
                result.ErrorMessage = "Operação cancelada";
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("DocGeneratorOrchestrator.cs", "ProcessFileCoreAsync", error);
                result.Status = ProcessingStatus.Failed;
                result.ErrorMessage = error.Message;
            }

            sw.Stop();
            result.ProcessingTime = sw.Elapsed;
            return result;
        }

        #region Private Methods

        private List<DiscoveredFile> GetFilesToProcess(DiscoveryResult discovery, DocGenerationOptions options)
        {
            var allFiles = new List<DiscoveredFile>();

            // Coletar todos os arquivos recursivamente
            CollectFiles(discovery.RootNode, allFiles);

            _logger.LogInformation("GetFilesToProcess: Total de arquivos descobertos: {Count}", allFiles.Count);

            // Filtrar por seleção
            if (options.SelectedPaths.Any())
            {
                // Normalizar paths para comparação (usar barras normais)
                var normalizedSelectedPaths = options.SelectedPaths
                    .Select(p => p.Replace("\\", "/").ToLowerInvariant())
                    .ToList();

                _logger.LogInformation("GetFilesToProcess: Paths selecionados (normalizados): {Paths}",
                    string.Join(", ", normalizedSelectedPaths));

                // Log primeiros arquivos para debug
                foreach (var f in allFiles.Take(5))
                {
                    _logger.LogInformation("GetFilesToProcess: Exemplo arquivo - RelativePath={Rel}, FullPath={Full}",
                        f.RelativePath, f.FullPath);
                }

                allFiles = allFiles
                    .Where(f =>
                    {
                        var normalizedRelPath = f.RelativePath.Replace("\\", "/").ToLowerInvariant();
                        var normalizedFullPath = f.FullPath.Replace("\\", "/").ToLowerInvariant();

                        // Verificar match exato ou se começa com o path selecionado (para pastas)
                        var match = normalizedSelectedPaths.Any(p =>
                            normalizedRelPath.Equals(p, StringComparison.OrdinalIgnoreCase) ||
                            normalizedRelPath.StartsWith(p + "/", StringComparison.OrdinalIgnoreCase) ||
                            normalizedFullPath.EndsWith("/" + p, StringComparison.OrdinalIgnoreCase) ||
                            normalizedFullPath.Contains("/" + p));

                        if (match)
                        {
                            _logger.LogInformation("GetFilesToProcess: MATCH encontrado - {File}", f.RelativePath);
                        }

                        return match;
                    })
                    .ToList();

                _logger.LogInformation("GetFilesToProcess: Após filtro de seleção: {Count} arquivos", allFiles.Count);
            }

            // Filtrar por modificação
            if (options.OnlyModified && !options.ForceRegenerate)
            {
                allFiles = allFiles.Where(f => f.NeedsRegeneration).ToList();
                _logger.LogInformation("GetFilesToProcess: Após filtro OnlyModified: {Count} arquivos", allFiles.Count);
            }

            return allFiles;
        }

        private void CollectFiles(FolderNode node, List<DiscoveredFile> files)
        {
            files.AddRange(node.Files);

            foreach (var child in node.Children)
            {
                CollectFiles(child, files);
            }
        }

        private async Task SendProgressAsync(
            DocGenerationJob job,
            string message,
            bool isComplete = false,
            bool hasError = false,
            string errorMessage = "")
        {
            try
            {
                var progress = new DocGenerationProgress
                {
                    JobId = job.JobId,
                    TotalFiles = job.TotalFiles,
                    ProcessedFiles = job.ProcessedFiles,
                    CurrentFile = job.CurrentFile,
                    CurrentStatus = job.CurrentStatus,
                    Message = message,
                    IsComplete = isComplete,
                    HasError = hasError,
                    ErrorMessage = errorMessage,
                    SuccessCount = job.SuccessCount,
                    FailedCount = job.FailedCount,
                    SkippedCount = job.SkippedCount
                };

                // Enviar progresso apenas para o grupo deste job
                  await _hubContext.Clients.Group($"job_{job.JobId}").SendAsync("DocGenerationProgress", progress);
                  
                  // Também enviar para "All" para debugging (opcional - remover em produção)
                  // await _hubContext.Clients.All.SendAsync("DocGenerationProgress", progress);
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("DocGeneratorOrchestrator.cs", "SendProgressAsync", error);
            }
        }

        #endregion

        #region Limpeza de Jobs Antigos

        /// <summary>
        /// Remove jobs finalizados há mais de X horas para evitar memory leak
        /// Deve ser chamado periodicamente ou após cada job finalizado
        /// </summary>
        /// <param name="maxAgeHours">Máximo de horas que um job finalizado pode permanecer em memória (padrão: 24h)</param>
        /// <returns>Número de jobs removidos</returns>
        public int CleanupOldJobs(int maxAgeHours = 24)
        {
            try
            {
                var cutoff = DateTime.Now.AddHours(-maxAgeHours);
                var jobsToRemove = new List<string>();

                foreach (var kvp in _jobs)
                {
                    var job = kvp.Value;

                    // Remover apenas jobs finalizados (não em execução) que são antigos
                    if (!job.IsRunning && job.CompletedAt.HasValue && job.CompletedAt.Value < cutoff)
                    {
                        jobsToRemove.Add(kvp.Key);
                    }
                }

                foreach (var jobId in jobsToRemove)
                {
                    _jobs.TryRemove(jobId, out _);
                    _cancellationTokens.TryRemove(jobId, out var cts);
                    cts?.Dispose();
                }

                if (jobsToRemove.Count > 0)
                {
                    _logger.LogInformation("Limpeza de jobs: {Count} jobs antigos removidos", jobsToRemove.Count);
                }

                return jobsToRemove.Count;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("DocGeneratorOrchestrator.cs", "CleanupOldJobs", error);
                return 0;
            }
        }

        /// <summary>
        /// Retorna estatísticas dos jobs em memória
        /// </summary>
        public (int Total, int Running, int Completed, int Failed) GetJobStats()
        {
            try
            {
                var total = _jobs.Count;
                var running = 0;
                var completed = 0;
                var failed = 0;

                foreach (var job in _jobs.Values)
                {
                    if (job.IsRunning) running++;
                    else if (job.FailedCount > 0 && job.SuccessCount == 0) failed++;
                    else completed++;
                }

                return (total, running, completed, failed);
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("DocGeneratorOrchestrator.cs", "GetJobStats", error);
                return (0, 0, 0, 0);
            }
        }

        #endregion
    }
}
