using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using FrotiX.Services.DocGenerator.Interfaces;
using FrotiX.Services.DocGenerator.Models;
using FrotiX.Services.DocGenerator.Providers;
using FrotiX.Services.DocGenerator.Services; // Para IFileTrackingService
using System;

namespace FrotiX.Services.DocGenerator
{
    /// <summary>
    /// Extensões para registrar serviços do DocGenerator no DI container
    /// </summary>
    public static class DocGeneratorServiceCollectionExtensions
    {
        /// <summary>
        /// Adiciona todos os serviços necessários para o DocGenerator
        /// </summary>
        public static IServiceCollection AddDocGenerator(this IServiceCollection services, IConfiguration configuration)
        {
            try
            {
                // Registrar configurações
                services.Configure<DocGeneratorSettings>(configuration.GetSection(DocGeneratorSettings.SectionName));

                // Registrar serviços principais
                services.AddScoped<IFileDiscoveryService, FileDiscoveryService>();
                services.AddScoped<IDocExtractionService, DocExtractionService>();
                services.AddScoped<IDocComposerService, DocComposerService>();
                services.AddScoped<IDocRenderService, DocRenderService>();
                services.AddScoped<IDocCacheService, DocCacheService>();
                services.AddScoped<IDocGeneratorOrchestrator, DocGeneratorOrchestrator>();

                // FileTrackingService: Singleton para funcionar como HostedService
                // e permitir injeção em outros serviços (evita instâncias duplicadas)
                services.AddSingleton<FileTrackingService>();
                services.AddSingleton<IFileTrackingService>(sp => sp.GetRequiredService<FileTrackingService>());
                services.AddHostedService(sp => sp.GetRequiredService<FileTrackingService>());

                // Registrar provedores de IA
                services.AddScoped<IDocAiProvider, OpenAiDocProvider>();
                services.AddScoped<IDocAiProvider, ClaudeDocProvider>();
                services.AddScoped<IDocAiProvider, GeminiDocProvider>();

                // Registrar HttpClient para provedores de IA
                services.AddHttpClient<OpenAiDocProvider>();
                services.AddHttpClient<ClaudeDocProvider>();
                services.AddHttpClient<GeminiDocProvider>();

                return services;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("DocGeneratorServiceCollectionExtensions.cs", "AddDocGenerator", error);
                throw;
            }
        }
    }
}