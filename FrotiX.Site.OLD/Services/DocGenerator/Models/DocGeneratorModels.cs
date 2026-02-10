/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: DocGeneratorModels.cs                                                                   ║
   ║ 📂 CAMINHO: /Services/DocGenerator/Models                                                           ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO: Modelos consolidados do DocGenerator. Enums, DTOs, Settings, Results.                  ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 ÍNDICE: FileCategory, AiProvider, DiscoveredFile, DocumentFacts, DocGeneratorSettings, etc.      ║
   ║ 🔗 DEPS: Nenhuma (POCOs e enums) | 📅 13/01/2026 | 👤 Copilot | 📝 v2.0                             ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝
*/

using System;
using System.Collections.Generic;

namespace FrotiX.Services.DocGenerator.Models
{
    #region Enums

    /// <summary>
    /// Categoria de arquivo no projeto
    /// </summary>
    public enum FileCategory
    {
        Controller,
        RazorPage,
        RazorPageModel,
        Service,
        Repository,
        Model,
        ViewModel,
        Helper,
        Middleware,
        Hub,
        JavaScript,
        Css,
        Data,
        Other
    }

    /// <summary>
    /// Provedor de IA para geração de documentação
    /// </summary>
    public enum AiProvider
    {
        None,
        OpenAI,
        Claude,
        Gemini
    }

    /// <summary>
    /// Nível de detalhamento da documentação
    /// </summary>
    public enum DetailLevel
    {
        Minimal,    // Resumo básico
        Standard,   // Padrão (500+ linhas)
        Detailed,   // Detalhado (1000+ linhas)
        Exhaustive  // Exaustivo (máximo detalhamento)
    }

    /// <summary>
    /// Status de processamento de um arquivo
    /// </summary>
    public enum ProcessingStatus
    {
        Pending,
        Extracting,
        Composing,
        Rendering,
        Completed,
        Failed,
        Skipped
    }

    #endregion

    #region File Discovery Models

    /// <summary>
    /// Representa um arquivo descoberto no projeto
    /// </summary>
    public class DiscoveredFile
    {
        public string FullPath { get; set; } = string.Empty;
        public string RelativePath { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public string Extension { get; set; } = string.Empty;
        public FileCategory Category { get; set; }
        public long FileSize { get; set; }
        public DateTime LastModified { get; set; }
        public string ContentHash { get; set; } = string.Empty;
        public bool HasExistingDoc { get; set; }
        public string ExistingDocPath { get; set; } = string.Empty;
        public bool NeedsRegeneration { get; set; }
    }

    /// <summary>
    /// Representa uma pasta no tree view
    /// </summary>
    public class FolderNode
    {
        public string Name { get; set; } = string.Empty;
        public string Path { get; set; } = string.Empty;
        public FileCategory? Category { get; set; }
        public List<FolderNode> Children { get; set; } = new();
        public List<DiscoveredFile> Files { get; set; } = new();
        public int TotalFiles { get; set; }
        public bool IsExpanded { get; set; }
    }

    /// <summary>
    /// Resultado da descoberta de arquivos
    /// </summary>
    public class DiscoveryResult
    {
        public FolderNode RootNode { get; set; } = new();
        public Dictionary<FileCategory, int> FilesByCategory { get; set; } = new();
        public int TotalFiles { get; set; }
        public int FilesWithDocs { get; set; }
        public int FilesNeedingUpdate { get; set; }
        public DateTime DiscoveredAt { get; set; } = DateTime.Now;
    }

    #endregion

    #region Extraction Models

    /// <summary>
    /// Fatos extraídos de um arquivo C# (Controller, Service, etc.)
    /// </summary>
    public class CSharpFacts
    {
        public string Namespace { get; set; } = string.Empty;
        public string ClassName { get; set; } = string.Empty;
        public string BaseClass { get; set; } = string.Empty;
        public List<string> Interfaces { get; set; } = new();
        public List<string> Attributes { get; set; } = new();
        public List<MethodInfo> Methods { get; set; } = new();
        public List<PropertyInfo> Properties { get; set; } = new();
        public List<DependencyInfo> Dependencies { get; set; } = new();
        public List<string> UsingStatements { get; set; } = new();
        public string Summary { get; set; } = string.Empty;
    }

    /// <summary>
    /// Informações sobre um método
    /// </summary>
    public class MethodInfo
    {
        public string Name { get; set; } = string.Empty;
        public string ReturnType { get; set; } = string.Empty;
        public List<ParameterInfo> Parameters { get; set; } = new();
        public List<string> Attributes { get; set; } = new();
        public string HttpMethod { get; set; } = string.Empty;
        public string Route { get; set; } = string.Empty;
        public bool IsAsync { get; set; }
        public string Summary { get; set; } = string.Empty;
        public int StartLine { get; set; }
        public int EndLine { get; set; }
        public string BodySnippet { get; set; } = string.Empty;
        public List<string> CalledMethods { get; set; } = new();
    }

    /// <summary>
    /// Informações sobre um parâmetro
    /// </summary>
    public class ParameterInfo
    {
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string DefaultValue { get; set; } = string.Empty;
        public List<string> Attributes { get; set; } = new();
    }

    /// <summary>
    /// Informações sobre uma propriedade
    /// </summary>
    public class PropertyInfo
    {
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public bool HasGetter { get; set; }
        public bool HasSetter { get; set; }
        public List<string> Attributes { get; set; } = new();
        public string Summary { get; set; } = string.Empty;
    }

    /// <summary>
    /// Informações sobre uma dependência injetada
    /// </summary>
    public class DependencyInfo
    {
        public string InterfaceName { get; set; } = string.Empty;
        public string FieldName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }

    /// <summary>
    /// Fatos extraídos de um arquivo Razor (.cshtml)
    /// </summary>
    public class RazorFacts
    {
        public string PageDirective { get; set; } = string.Empty;
        public string ModelType { get; set; } = string.Empty;
        public string Layout { get; set; } = string.Empty;
        public List<string> Sections { get; set; } = new();
        public List<string> Partials { get; set; } = new();
        public List<string> TagHelpers { get; set; } = new();
        public List<string> ScriptReferences { get; set; } = new();
        public List<string> StyleReferences { get; set; } = new();
        public List<string> FormFields { get; set; } = new();
        public List<string> AjaxEndpoints { get; set; } = new();
        public string InlineStyles { get; set; } = string.Empty;
        public string InlineScripts { get; set; } = string.Empty;
    }

    /// <summary>
    /// Fatos extraídos de um arquivo JavaScript
    /// </summary>
    public class JavaScriptFacts
    {
        public List<JsFunctionInfo> Functions { get; set; } = new();
        public List<string> GlobalVariables { get; set; } = new();
        public List<string> EventListeners { get; set; } = new();
        public List<string> AjaxCalls { get; set; } = new();
        public List<string> DomManipulations { get; set; } = new();
        public List<string> Dependencies { get; set; } = new();
        public bool UsesJQuery { get; set; }
        public bool UsesSyncfusion { get; set; }
        public bool UsesDataTables { get; set; }
    }

    /// <summary>
    /// Informações sobre uma função JavaScript
    /// </summary>
    public class JsFunctionInfo
    {
        public string Name { get; set; } = string.Empty;
        public List<string> Parameters { get; set; } = new();
        public bool IsAsync { get; set; }
        public bool IsArrowFunction { get; set; }
        public string BodySnippet { get; set; } = string.Empty;
        public int StartLine { get; set; }
        public int EndLine { get; set; }
    }

    /// <summary>
    /// Container unificado de fatos extraídos
    /// </summary>
    public class DocumentFacts
    {
        public DiscoveredFile File { get; set; } = new();
        public CSharpFacts? CSharpFacts { get; set; }
        public RazorFacts? RazorFacts { get; set; }
        public JavaScriptFacts? JavaScriptFacts { get; set; }
        public string RawContent { get; set; } = string.Empty;
        public int TotalLines { get; set; }
        public DateTime ExtractedAt { get; set; } = DateTime.Now;
        public List<string> Warnings { get; set; } = new();
    }

    #endregion

    #region Composition Models

    /// <summary>
    /// Plano de layout para geração de HTML A4
    /// </summary>
    public class LayoutPlan
    {
        public string Theme { get; set; } = "default";
        public string PrimaryColor { get; set; } = "#b66a3d";
        public string SecondaryColor { get; set; } = "#722F37";
        public List<PagePlan> Pages { get; set; } = new();
        public Dictionary<string, string> IconMap { get; set; } = new();
    }

    /// <summary>
    /// Plano para uma página A4
    /// </summary>
    public class PagePlan
    {
        public int PageNumber { get; set; }
        public string Title { get; set; } = string.Empty;
        public bool HasHero { get; set; }
        public bool HasToc { get; set; }
        public List<SectionPlan> Sections { get; set; } = new();
        public int EstimatedWeight { get; set; }
    }

    /// <summary>
    /// Plano para uma seção dentro de uma página
    /// </summary>
    public class SectionPlan
    {
        public string Id { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public string Type { get; set; } = "card"; // card, hero, toc, timeline, code
        public int EstimatedWeight { get; set; }
        public bool KeepTogether { get; set; } = true;
        public string Content { get; set; } = string.Empty;
    }

    /// <summary>
    /// Resultado da composição por IA
    /// </summary>
    public class AiComposeResult
    {
        public string Markdown { get; set; } = string.Empty;
        public LayoutPlan Plan { get; set; } = new();
        public Dictionary<string, string> IconMap { get; set; } = new();
        public int TokensUsed { get; set; }
        public TimeSpan ProcessingTime { get; set; }
        public string ModelUsed { get; set; } = string.Empty;
        public bool Success { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
    }

    #endregion

    #region Rendering Models

    /// <summary>
    /// Resultado da renderização
    /// </summary>
    public class RenderResult
    {
        public string MarkdownPath { get; set; } = string.Empty;
        public List<string> HtmlPaths { get; set; } = new();
        public int TotalPages { get; set; }
        public bool Success { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
        public TimeSpan ProcessingTime { get; set; }
    }

    /// <summary>
    /// Conteúdo de uma página HTML A4 renderizada
    /// </summary>
    public class RenderedPage
    {
        public int PageNumber { get; set; }
        public string HtmlContent { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public bool HasPrevious { get; set; }
        public bool HasNext { get; set; }
    }

    #endregion

    #region Job Models

    /// <summary>
    /// Opções para geração de documentação
    /// </summary>
    public class DocGenerationOptions
    {
        public bool OnlyModified { get; set; }
        public bool ForceRegenerate { get; set; }
        public bool UseAi { get; set; }
        public AiProvider AiProvider { get; set; } = AiProvider.None;
        public string AiModel { get; set; } = string.Empty;
        public DetailLevel DetailLevel { get; set; } = DetailLevel.Standard;
        public List<string> SelectedPaths { get; set; } = new();
        public bool GenerateHtml { get; set; } = true;
        public bool GenerateMarkdown { get; set; } = true;
    }

    /// <summary>
    /// Status de um job de geração
    /// </summary>
    public class DocGenerationJob
    {
        public string JobId { get; set; } = Guid.NewGuid().ToString();
        public DateTime StartedAt { get; set; } = DateTime.Now;
        public DateTime? CompletedAt { get; set; }
        public DocGenerationOptions Options { get; set; } = new();
        public int TotalFiles { get; set; }
        public int ProcessedFiles { get; set; }
        public int SuccessCount { get; set; }
        public int FailedCount { get; set; }
        public int SkippedCount { get; set; }
        public List<FileProcessingResult> Results { get; set; } = new();
        public bool IsRunning { get; set; }
        public bool IsCancelled { get; set; }
        public string CurrentFile { get; set; } = string.Empty;
        public ProcessingStatus CurrentStatus { get; set; }
    }

    /// <summary>
    /// Resultado do processamento de um arquivo
    /// </summary>
    public class FileProcessingResult
    {
        public string FilePath { get; set; } = string.Empty;
        public ProcessingStatus Status { get; set; }
        public string MarkdownPath { get; set; } = string.Empty;
        public List<string> HtmlPaths { get; set; } = new();
        public TimeSpan ProcessingTime { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
        public bool UsedAi { get; set; }
        public int TokensUsed { get; set; }
    }

    /// <summary>
    /// Progresso enviado via SignalR
    /// </summary>
    public class DocGenerationProgress
    {
        public string JobId { get; set; } = string.Empty;
        public int TotalFiles { get; set; }
        public int ProcessedFiles { get; set; }
        public int Percentage => TotalFiles > 0 ? (ProcessedFiles * 100) / TotalFiles : 0;
        public string CurrentFile { get; set; } = string.Empty;
        public ProcessingStatus CurrentStatus { get; set; }
        public string Message { get; set; } = string.Empty;
        public bool IsComplete { get; set; }
        public bool HasError { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;

        // Campos de contagem para acompanhamento real
        public int SuccessCount { get; set; }
        public int FailedCount { get; set; }
        public int SkippedCount { get; set; }
    }

    #endregion

    #region Cache Models

    /// <summary>
    /// Entrada de cache para evitar regeneração desnecessária
    /// </summary>
    public class DocCacheEntry
    {
        public string FilePath { get; set; } = string.Empty;
        public string ContentHash { get; set; } = string.Empty;
        public string TemplateVersion { get; set; } = string.Empty;
        public string PromptVersion { get; set; } = string.Empty;
        public DateTime GeneratedAt { get; set; }
        public string MarkdownPath { get; set; } = string.Empty;
        public List<string> HtmlPaths { get; set; } = new();
    }

    /// <summary>
    /// Manifesto de cache completo
    /// </summary>
    public class DocCacheManifest
    {
        public string Version { get; set; } = "1.0";
        public DateTime LastUpdated { get; set; } = DateTime.Now;
        public Dictionary<string, DocCacheEntry> Entries { get; set; } = new();
    }

    #endregion

    #region Configuration Models

    /// <summary>
    /// Configurações do DocGenerator (appsettings)
    /// </summary>
    public class DocGeneratorSettings
    {
        public const string SectionName = "DocGenerator";

        public string OutputPath { get; set; } = "Documentacao";
        public string CacheFile { get; set; } = ".doc-cache.json";
        public string TemplateVersion { get; set; } = "2.0";
        public string PromptVersion { get; set; } = "1.0";

        public OpenAiSettings OpenAi { get; set; } = new();
        public ClaudeSettings Claude { get; set; } = new();
        public GeminiSettings Gemini { get; set; } = new();

        public List<string> IncludePatterns { get; set; } = new()
        {
            "Controllers/**/*.cs",
            "Pages/**/*.cshtml",
            "Pages/**/*.cshtml.cs",
            "Services/**/*.cs",
            "Repository/**/*.cs",
            "Helpers/**/*.cs",
            "Middlewares/**/*.cs",
            "Hubs/**/*.cs",
            "Models/**/*.cs",
            "ViewModels/**/*.cs",
            "Data/**/*.cs",
            "wwwroot/js/**/*.js",
            "wwwroot/css/**/*.css"
        };

        public List<string> ExcludePatterns { get; set; } = new()
        {
            "bin/**",
            "obj/**",
            ".git/**",
            "node_modules/**",
            "Documentacao/**",
            "Migrations/**",
            "**/*.g.cs",
            "**/*.designer.cs"
        };

        public int MaxTokensPerRequest { get; set; } = 8000;
        public int MaxRetries { get; set; } = 3;
        public int RetryDelayMs { get; set; } = 1000;
        public int TimeoutSeconds { get; set; } = 120;
    }

    /// <summary>
    /// Configurações OpenAI
    /// </summary>
    public class OpenAiSettings
    {
        private string _apiKey = string.Empty;

        /// <summary>
        /// API Key - lê primeiro do appsettings.json, depois da environment variable
        /// DOCGENERATOR_OPENAI_APIKEY como fallback
        /// </summary>
        public string ApiKey
        {
            get
            {
                // Prioridade: appsettings.json > Environment Variable
                if (!string.IsNullOrWhiteSpace(_apiKey))
                    return _apiKey;
                return Environment.GetEnvironmentVariable("DOCGENERATOR_OPENAI_APIKEY") ?? string.Empty;
            }
            set => _apiKey = value;
        }

        /// <summary>
        /// Modelo OpenAI a usar. Opções recomendadas (2026):
        /// - gpt-4.1 (mais recente, melhor para código)
        /// - gpt-4.1-mini (mais rápido e econômico)
        /// - gpt-4o (anterior, ainda suportado)
        /// </summary>
        public string Model { get; set; } = "gpt-4.1";
        public string BaseUrl { get; set; } = "https://api.openai.com/v1";
        public double Temperature { get; set; } = 0.3;
        public int MaxTokens { get; set; } = 16384;

        /// <summary>
        /// Service Tier para OpenAI: "flex" (menor latência), "auto" ou "default"
        /// Recomendado: "flex" para documentação batch
        /// </summary>
        public string ServiceTier { get; set; } = "flex";

        /// <summary>
        /// Verifica se as credenciais estão configuradas
        /// </summary>
        public bool IsConfigured => !string.IsNullOrWhiteSpace(ApiKey);
    }

    /// <summary>
    /// Configurações Claude (Anthropic)
    /// </summary>
    public class ClaudeSettings
    {
        private string _apiKey = string.Empty;

        /// <summary>
        /// API Key - lê primeiro do appsettings.json, depois da environment variable
        /// DOCGENERATOR_CLAUDE_APIKEY como fallback
        /// </summary>
        public string ApiKey
        {
            get
            {
                // Prioridade: appsettings.json > Environment Variable
                if (!string.IsNullOrWhiteSpace(_apiKey))
                    return _apiKey;
                return Environment.GetEnvironmentVariable("DOCGENERATOR_CLAUDE_APIKEY") ?? string.Empty;
            }
            set => _apiKey = value;
        }

        /// <summary>
        /// Modelo Claude a usar. Opções recomendadas (2026):
        /// - claude-sonnet-4-5-20251101 (mais recente, excelente custo-benefício)
        /// - claude-opus-4-5-20251101 (máxima qualidade, mais caro)
        /// - claude-3-5-sonnet-20241022 (anterior, ainda suportado)
        /// </summary>
        public string Model { get; set; } = "claude-sonnet-4-5-20251101";
        public string BaseUrl { get; set; } = "https://api.anthropic.com/v1";
        public string ApiVersion { get; set; } = "2023-06-01";
        public double Temperature { get; set; } = 0.3;
        public int MaxTokens { get; set; } = 16384;

        /// <summary>
        /// Verifica se as credenciais estão configuradas
        /// </summary>
        public bool IsConfigured => !string.IsNullOrWhiteSpace(ApiKey);
    }

    /// <summary>
    /// Configurações Gemini (Google)
    /// </summary>
    public class GeminiSettings
    {
        private string _apiKey = string.Empty;

        /// <summary>
        /// API Key - lê primeiro do appsettings.json, depois da environment variable
        /// DOCGENERATOR_GEMINI_APIKEY como fallback
        /// </summary>
        public string ApiKey
        {
            get
            {
                // Prioridade: appsettings.json > Environment Variable
                if (!string.IsNullOrWhiteSpace(_apiKey))
                    return _apiKey;
                return Environment.GetEnvironmentVariable("DOCGENERATOR_GEMINI_APIKEY") ?? string.Empty;
            }
            set => _apiKey = value;
        }

        /// <summary>
        /// Modelo Gemini a usar. Opções recomendadas (2026):
        /// - gemini-2.5-pro (mais recente, máxima qualidade)
        /// - gemini-2.5-flash (rápido e econômico)
        /// - gemini-1.5-flash (anterior, ainda suportado)
        /// </summary>
        public string Model { get; set; } = "gemini-2.5-flash";
        public string BaseUrl { get; set; } = "https://generativelanguage.googleapis.com/v1beta";
        public double Temperature { get; set; } = 0.3;
        public int MaxTokens { get; set; } = 16384;

        /// <summary>
        /// Verifica se as credenciais estão configuradas
        /// </summary>
        public bool IsConfigured => !string.IsNullOrWhiteSpace(ApiKey);
    }

    #endregion
}
