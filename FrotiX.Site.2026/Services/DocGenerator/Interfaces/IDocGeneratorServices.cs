/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: IDocGeneratorServices.cs                                                                ║
   ║ 📂 CAMINHO: /Services/DocGenerator/Interfaces                                                       ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO: Interfaces consolidadas do DocGenerator. Discovery, Extraction, Composer, Render etc.  ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 ÍNDICE: IFileDiscoveryService, IDocExtractionService, IDocComposerService, IDocRenderService,    ║
   ║           IDocCacheService, IDocGeneratorOrchestrator                                               ║
   ║ 🔗 DEPS: Models (DiscoveredFile, DocumentFacts) | 📅 13/01/2026 | 👤 Copilot | 📝 v2.0              ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝
*/

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FrotiX.Services.DocGenerator.Models;

namespace FrotiX.Services.DocGenerator.Interfaces
{
    /// <summary>
    /// Serviço de descoberta de arquivos no projeto
    /// </summary>
    public interface IFileDiscoveryService
    {
        /// <summary>
        /// Descobre todos os arquivos do projeto que devem ser documentados
        /// </summary>
        Task<DiscoveryResult> DiscoverAsync(CancellationToken ct = default);

        /// <summary>
        /// Obtém a árvore de pastas para exibição no tree view
        /// </summary>
        FolderNode GetFolderTree();

        /// <summary>
        /// Verifica se um arquivo precisa ser regenerado (baseado no hash)
        /// </summary>
        Task<bool> NeedsRegenerationAsync(string filePath, CancellationToken ct = default);

        /// <summary>
        /// Calcula o hash do conteúdo de um arquivo
        /// </summary>
        string ComputeHash(string filePath);

        /// <summary>
        /// Detecta a categoria de um arquivo
        /// </summary>
        FileCategory DetectCategory(string filePath);
    }

    /// <summary>
    /// Serviço de extração de metadados de arquivos
    /// </summary>
    public interface IDocExtractionService
    {
        /// <summary>
        /// Extrai fatos e metadados de um arquivo
        /// </summary>
        Task<DocumentFacts> ExtractAsync(DiscoveredFile file, CancellationToken ct = default);

        /// <summary>
        /// Extrai fatos de código C# usando Roslyn
        /// </summary>
        Task<CSharpFacts> ExtractCSharpAsync(string filePath, string content, CancellationToken ct = default);

        /// <summary>
        /// Extrai fatos de arquivo Razor
        /// </summary>
        Task<RazorFacts> ExtractRazorAsync(string filePath, string content, CancellationToken ct = default);

        /// <summary>
        /// Extrai fatos de arquivo JavaScript
        /// </summary>
        Task<JavaScriptFacts> ExtractJavaScriptAsync(string filePath, string content, CancellationToken ct = default);
    }

    /// <summary>
    /// Serviço de composição de documentação
    /// </summary>
    public interface IDocComposerService
    {
        /// <summary>
        /// Compõe documentação usando IA ou heurísticas
        /// </summary>
        Task<AiComposeResult> ComposeAsync(
            DocumentFacts facts,
            DocGenerationOptions options,
            CancellationToken ct = default);

        /// <summary>
        /// Compõe documentação usando heurísticas (sem IA)
        /// </summary>
        Task<AiComposeResult> ComposeWithHeuristicsAsync(
            DocumentFacts facts,
            DetailLevel level,
            CancellationToken ct = default);
    }

    /// <summary>
    /// Serviço de renderização de documentação
    /// </summary>
    public interface IDocRenderService
    {
        /// <summary>
        /// Renderiza Markdown e HTML a partir do resultado da composição
        /// </summary>
        Task<RenderResult> RenderAsync(
            DocumentFacts facts,
            AiComposeResult composeResult,
            DocGenerationOptions options,
            CancellationToken ct = default);

        /// <summary>
        /// Gera arquivo Markdown
        /// </summary>
        Task<string> RenderMarkdownAsync(
            DocumentFacts facts,
            string content,
            CancellationToken ct = default);

        /// <summary>
        /// Gera arquivos HTML A4 (pode gerar múltiplos)
        /// </summary>
        Task<List<RenderedPage>> RenderHtmlA4Async(
            DocumentFacts facts,
            string markdownContent,
            LayoutPlan plan,
            CancellationToken ct = default);

        /// <summary>
        /// Divide conteúdo em páginas A4
        /// </summary>
        List<PagePlan> SplitIntoA4Pages(string content, LayoutPlan plan);
    }

    /// <summary>
    /// Provedor de IA para geração de documentação
    /// </summary>
    public interface IDocAiProvider
    {
        /// <summary>
        /// Nome do provedor
        /// </summary>
        string ProviderName { get; }

        /// <summary>
        /// Verifica se o provedor está configurado e disponível
        /// </summary>
        bool IsAvailable { get; }

        /// <summary>
        /// Compõe documentação usando IA
        /// </summary>
        Task<AiComposeResult> ComposeAsync(
            DocumentFacts facts,
            DocGenerationOptions options,
            CancellationToken ct = default);
    }

    /// <summary>
    /// Serviço de cache de documentação
    /// </summary>
    public interface IDocCacheService
    {
        /// <summary>
        /// Obtém entrada de cache para um arquivo
        /// </summary>
        Task<DocCacheEntry?> GetAsync(string filePath, CancellationToken ct = default);

        /// <summary>
        /// Salva entrada no cache
        /// </summary>
        Task SetAsync(string filePath, DocCacheEntry entry, CancellationToken ct = default);

        /// <summary>
        /// Remove entrada do cache
        /// </summary>
        Task RemoveAsync(string filePath, CancellationToken ct = default);

        /// <summary>
        /// Limpa todo o cache
        /// </summary>
        Task ClearAsync(CancellationToken ct = default);

        /// <summary>
        /// Carrega o manifesto de cache do disco
        /// </summary>
        Task LoadAsync(CancellationToken ct = default);

        /// <summary>
        /// Salva o manifesto de cache no disco
        /// </summary>
        Task SaveAsync(CancellationToken ct = default);

        /// <summary>
        /// Verifica se o arquivo precisa ser regenerado
        /// </summary>
        Task<bool> NeedsRegenerationAsync(string filePath, string currentHash, CancellationToken ct = default);
    }

    /// <summary>
    /// Serviço principal de orquestração da geração de documentação
    /// </summary>
    public interface IDocGeneratorOrchestrator
    {
        /// <summary>
        /// Inicia um job de geração de documentação (executa em background, retorna imediatamente)
        /// </summary>
        Task<DocGenerationJob> StartJobInBackgroundAsync(
            DocGenerationOptions options,
            CancellationToken ct = default);

        /// <summary>
        /// Inicia um job de geração de documentação (aguarda conclusão)
        /// </summary>
        Task<DocGenerationJob> StartJobAsync(
            DocGenerationOptions options,
            CancellationToken ct = default);

        /// <summary>
        /// Cancela um job em execução
        /// </summary>
        Task CancelJobAsync(string jobId, CancellationToken ct = default);

        /// <summary>
        /// Obtém status de um job
        /// </summary>
        Task<DocGenerationJob?> GetJobStatusAsync(string jobId, CancellationToken ct = default);

        /// <summary>
        /// Lista jobs recentes
        /// </summary>
        Task<List<DocGenerationJob>> ListRecentJobsAsync(int count = 10, CancellationToken ct = default);

        /// <summary>
        /// Processa um único arquivo
        /// </summary>
        Task<FileProcessingResult> ProcessFileAsync(
            DiscoveredFile file,
            DocGenerationOptions options,
            CancellationToken ct = default);
    }
}
