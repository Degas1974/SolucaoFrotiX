/*
 * ╔══════════════════════════════════════════════════════════════════════════╗
 * ║                                                                          ║
 * ║  📚 DOCUMENTAÇÃO DISPONÍVEL                                              ║
 * ║                                                                          ║
 * ║  Este arquivo está documentado em:                                       ║
 * ║  📄 Documentacao/Services/DocGenerator/DocComposerService.md             ║
 * ║                                                                          ║
 * ║  Última atualização: 13/01/2026                                          ║
 * ║                                                                          ║
 * ╚══════════════════════════════════════════════════════════════════════════╝
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FrotiX.Services.DocGenerator.Interfaces;
using FrotiX.Services.DocGenerator.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FrotiX.Services.DocGenerator
{
    /// <summary>
    /// Serviço de composição de documentação (IA ou Heurísticas)
    /// </summary>
    public class DocComposerService : IDocComposerService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<DocComposerService> _logger;

        public DocComposerService(
            IServiceProvider serviceProvider,
            ILogger<DocComposerService> logger)
        {
            try
            {
                _serviceProvider = serviceProvider;
                _logger = logger;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("DocComposerService.cs", ".ctor", error);
            }
        }

        public async Task<AiComposeResult> ComposeAsync(
            DocumentFacts facts,
            DocGenerationOptions options,
            CancellationToken ct = default)
        {
            try
            {
                if (!options.UseAi || options.AiProvider == AiProvider.None)
                {
                    _logger.LogInformation("Usando heurísticas para {File}", facts.File.FileName);
                    return await ComposeWithHeuristicsAsync(facts, options.DetailLevel, ct);
                }

                // Obter o provedor de IA correto
                var provider = GetAiProvider(options.AiProvider);

                if (provider == null || !provider.IsAvailable)
                {
                    _logger.LogWarning("Provedor {Provider} não disponível, usando heurísticas",
                        options.AiProvider);
                    return await ComposeWithHeuristicsAsync(facts, options.DetailLevel, ct);
                }

                _logger.LogInformation("Usando {Provider} para {File}",
                    provider.ProviderName, facts.File.FileName);

                var aiResult = await provider.ComposeAsync(facts, options, ct);

                _logger.LogInformation(
                    "DocComposerService: Resultado do provider {Provider} - Success={Success}, Markdown.Length={MdLen}, TokensUsed={Tokens}, Error={Error}",
                    provider.ProviderName,
                    aiResult.Success,
                    aiResult.Markdown?.Length ?? 0,
                    aiResult.TokensUsed,
                    aiResult.ErrorMessage ?? "(nenhum)");

                return aiResult;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("DocComposerService.cs", "ComposeAsync", error);
                return new AiComposeResult
                {
                    Success = false,
                    ErrorMessage = error.Message
                };
            }
        }

        public async Task<AiComposeResult> ComposeWithHeuristicsAsync(
            DocumentFacts facts,
            DetailLevel level,
            CancellationToken ct = default)
        {
            var result = new AiComposeResult();
            var sw = Stopwatch.StartNew();

            try
            {
                await Task.Yield(); // Para manter async

                var sb = new StringBuilder();
                var iconMap = new Dictionary<string, string>();

                // Header do documento
                sb.AppendLine($"# Documentação: {facts.File.FileName}");
                sb.AppendLine();
                sb.AppendLine($"> **Última Atualização**: {DateTime.Now:dd/MM/yyyy}");
                sb.AppendLine($"> **Versão Atual**: 1.0");
                sb.AppendLine();
                sb.AppendLine("---");
                sb.AppendLine();

                // PARTE 1: DOCUMENTAÇÃO
                sb.AppendLine("# PARTE 1: DOCUMENTAÇÃO DA FUNCIONALIDADE");
                sb.AppendLine();

                // Índice
                sb.AppendLine("## Índice");
                sb.AppendLine("1. [Visão Geral](#visão-geral)");
                sb.AppendLine("2. [Arquitetura](#arquitetura)");

                if (facts.CSharpFacts != null)
                {
                    sb.AppendLine("3. [Métodos](#métodos)");
                    sb.AppendLine("4. [Dependências](#dependências)");
                    sb.AppendLine("5. [Propriedades](#propriedades)");
                }

                if (facts.RazorFacts != null)
                {
                    sb.AppendLine("3. [Estrutura da Página](#estrutura-da-página)");
                    sb.AppendLine("4. [Scripts e Estilos](#scripts-e-estilos)");
                }

                if (facts.JavaScriptFacts != null)
                {
                    sb.AppendLine("3. [Funções](#funções)");
                    sb.AppendLine("4. [Chamadas AJAX](#chamadas-ajax)");
                }

                sb.AppendLine("6. [Interconexões](#interconexões)");
                sb.AppendLine("7. [Troubleshooting](#troubleshooting)");
                sb.AppendLine();
                sb.AppendLine("---");
                sb.AppendLine();

                // Visão Geral
                iconMap["Visão Geral"] = "fa-duotone fa-eye";
                sb.AppendLine("## Visão Geral");
                sb.AppendLine();
                GenerateOverview(sb, facts);
                sb.AppendLine();

                // Arquitetura
                iconMap["Arquitetura"] = "fa-duotone fa-sitemap";
                sb.AppendLine("## Arquitetura");
                sb.AppendLine();
                GenerateArchitecture(sb, facts);
                sb.AppendLine();

                // Seções específicas por tipo
                if (facts.CSharpFacts != null)
                {
                    iconMap["Métodos"] = "fa-duotone fa-code";
                    iconMap["Dependências"] = "fa-duotone fa-plug";
                    iconMap["Propriedades"] = "fa-duotone fa-list";

                    GenerateCSharpSections(sb, facts.CSharpFacts, level);
                }

                if (facts.RazorFacts != null)
                {
                    iconMap["Estrutura da Página"] = "fa-duotone fa-file-code";
                    iconMap["Scripts e Estilos"] = "fa-duotone fa-palette";

                    GenerateRazorSections(sb, facts.RazorFacts, level);
                }

                if (facts.JavaScriptFacts != null)
                {
                    iconMap["Funções"] = "fa-duotone fa-function";
                    iconMap["Chamadas AJAX"] = "fa-duotone fa-exchange";

                    GenerateJavaScriptSections(sb, facts.JavaScriptFacts, level);
                }

                // Interconexões
                iconMap["Interconexões"] = "fa-duotone fa-link";
                sb.AppendLine("## Interconexões");
                sb.AppendLine();
                GenerateInterconnections(sb, facts);
                sb.AppendLine();

                // Troubleshooting
                iconMap["Troubleshooting"] = "fa-duotone fa-wrench";
                sb.AppendLine("## Troubleshooting");
                sb.AppendLine();
                GenerateTroubleshooting(sb, facts);
                sb.AppendLine();

                // PARTE 2: LOG
                sb.AppendLine("---");
                sb.AppendLine();
                sb.AppendLine("# PARTE 2: LOG DE MODIFICAÇÕES/CORREÇÕES");
                sb.AppendLine();
                sb.AppendLine($"## [{DateTime.Now:dd/MM/yyyy HH:mm}] - Documentação inicial gerada automaticamente");
                sb.AppendLine();
                sb.AppendLine("**Descrição**: Documentação gerada automaticamente pelo DocGenerator FrotiX.");
                sb.AppendLine();
                sb.AppendLine("**Arquivos Afetados**:");
                sb.AppendLine($"- `{facts.File.RelativePath}`");
                sb.AppendLine();
                sb.AppendLine("**Status**: ✅ **Concluído**");
                sb.AppendLine();

                result.Markdown = sb.ToString();
                result.IconMap = iconMap;
                result.Plan = CreateLayoutPlan(facts, iconMap);
                result.ModelUsed = "Heurísticas";
                result.Success = true;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("DocComposerService.cs", "ComposeWithHeuristicsAsync", error);
                result.Success = false;
                result.ErrorMessage = error.Message;
            }

            sw.Stop();
            result.ProcessingTime = sw.Elapsed;
            return result;
        }

        #region Generation Helpers

        private void GenerateOverview(StringBuilder sb, DocumentFacts facts)
        {
            sb.AppendLine($"**Arquivo**: `{facts.File.RelativePath}`");
            sb.AppendLine();
            sb.AppendLine($"**Categoria**: {facts.File.Category}");
            sb.AppendLine();
            sb.AppendLine($"**Total de Linhas**: {facts.TotalLines}");
            sb.AppendLine();

            if (facts.CSharpFacts != null)
            {
                sb.AppendLine($"**Classe**: `{facts.CSharpFacts.ClassName}`");
                sb.AppendLine();
                sb.AppendLine($"**Namespace**: `{facts.CSharpFacts.Namespace}`");
                sb.AppendLine();

                if (!string.IsNullOrEmpty(facts.CSharpFacts.Summary))
                {
                    sb.AppendLine($"**Descrição**: {facts.CSharpFacts.Summary}");
                    sb.AppendLine();
                }

                sb.AppendLine("### Características Principais");
                sb.AppendLine();
                sb.AppendLine($"- ✅ **{facts.CSharpFacts.Methods.Count}** métodos públicos");
                sb.AppendLine($"- ✅ **{facts.CSharpFacts.Properties.Count}** propriedades");
                sb.AppendLine($"- ✅ **{facts.CSharpFacts.Dependencies.Count}** dependências injetadas");

                if (facts.CSharpFacts.Interfaces.Any())
                    sb.AppendLine($"- ✅ Implementa: {string.Join(", ", facts.CSharpFacts.Interfaces)}");

                if (!string.IsNullOrEmpty(facts.CSharpFacts.BaseClass))
                    sb.AppendLine($"- ✅ Herda de: `{facts.CSharpFacts.BaseClass}`");
            }

            if (facts.RazorFacts != null)
            {
                sb.AppendLine("### Características Principais");
                sb.AppendLine();
                if (!string.IsNullOrEmpty(facts.RazorFacts.ModelType))
                    sb.AppendLine($"- ✅ **Model**: `{facts.RazorFacts.ModelType}`");
                if (!string.IsNullOrEmpty(facts.RazorFacts.PageDirective))
                    sb.AppendLine($"- ✅ **Rota**: `{facts.RazorFacts.PageDirective}`");
                sb.AppendLine($"- ✅ **{facts.RazorFacts.Sections.Count}** sections");
                sb.AppendLine($"- ✅ **{facts.RazorFacts.ScriptReferences.Count}** scripts referenciados");
                sb.AppendLine($"- ✅ **{facts.RazorFacts.FormFields.Count}** campos de formulário");
            }

            if (facts.JavaScriptFacts != null)
            {
                sb.AppendLine("### Características Principais");
                sb.AppendLine();
                sb.AppendLine($"- ✅ **{facts.JavaScriptFacts.Functions.Count}** funções");
                sb.AppendLine($"- ✅ **{facts.JavaScriptFacts.AjaxCalls.Count}** chamadas AJAX");
                if (facts.JavaScriptFacts.UsesJQuery)
                    sb.AppendLine("- ✅ Usa jQuery");
                if (facts.JavaScriptFacts.UsesSyncfusion)
                    sb.AppendLine("- ✅ Usa Syncfusion");
                if (facts.JavaScriptFacts.UsesDataTables)
                    sb.AppendLine("- ✅ Usa DataTables");
            }
        }

        private void GenerateArchitecture(StringBuilder sb, DocumentFacts facts)
        {
            sb.AppendLine("### Estrutura de Arquivos");
            sb.AppendLine();
            sb.AppendLine("```");
            sb.AppendLine("FrotiX.Site/");

            var parts = facts.File.RelativePath.Split(new[] { '/', '\\' }, StringSplitOptions.RemoveEmptyEntries);
            var indent = "├── ";
            for (int i = 0; i < parts.Length; i++)
            {
                if (i == parts.Length - 1)
                    indent = "└── ";
                sb.AppendLine($"{"│   ".PadLeft(i * 4 + 4)}{indent}{parts[i]}");
            }

            sb.AppendLine("```");
            sb.AppendLine();

            sb.AppendLine("### Tecnologias Utilizadas");
            sb.AppendLine();
            sb.AppendLine("| Tecnologia | Uso |");
            sb.AppendLine("|------------|-----|");

            if (facts.CSharpFacts != null)
            {
                sb.AppendLine("| ASP.NET Core | Backend |");
                sb.AppendLine("| C# 12 | Linguagem |");

                if (facts.CSharpFacts.UsingStatements.Any(u => u.Contains("EntityFramework")))
                    sb.AppendLine("| Entity Framework Core | ORM |");

                if (facts.CSharpFacts.Dependencies.Any(d => d.InterfaceName.Contains("IUnitOfWork")))
                    sb.AppendLine("| Unit of Work | Padrão Repository |");
            }

            if (facts.RazorFacts != null)
            {
                sb.AppendLine("| Razor Pages | View Engine |");
            }

            if (facts.JavaScriptFacts != null)
            {
                if (facts.JavaScriptFacts.UsesJQuery)
                    sb.AppendLine("| jQuery | Manipulação DOM |");
                if (facts.JavaScriptFacts.UsesSyncfusion)
                    sb.AppendLine("| Syncfusion EJ2 | Componentes UI |");
                if (facts.JavaScriptFacts.UsesDataTables)
                    sb.AppendLine("| DataTables | Grid de Dados |");
            }
        }

        private void GenerateCSharpSections(StringBuilder sb, CSharpFacts facts, DetailLevel level)
        {
            // Métodos
            sb.AppendLine("## Métodos");
            sb.AppendLine();

            foreach (var method in facts.Methods)
            {
                sb.AppendLine($"### `{method.Name}`");
                sb.AppendLine();

                if (!string.IsNullOrEmpty(method.Summary))
                {
                    sb.AppendLine($"**Descrição**: {method.Summary}");
                    sb.AppendLine();
                }

                sb.AppendLine($"**Retorno**: `{method.ReturnType}`");
                sb.AppendLine();

                if (!string.IsNullOrEmpty(method.HttpMethod))
                {
                    sb.AppendLine($"**HTTP**: `{method.HttpMethod}` `{method.Route}`");
                    sb.AppendLine();
                }

                if (method.Parameters.Any())
                {
                    sb.AppendLine("**Parâmetros**:");
                    sb.AppendLine();
                    foreach (var param in method.Parameters)
                    {
                        sb.AppendLine($"- `{param.Type} {param.Name}` {(param.Attributes.Any() ? $"[{string.Join(", ", param.Attributes)}]" : "")}");
                    }
                    sb.AppendLine();
                }

                if (level >= DetailLevel.Standard && !string.IsNullOrEmpty(method.BodySnippet))
                {
                    sb.AppendLine("**Código**:");
                    sb.AppendLine();
                    sb.AppendLine("```csharp");
                    sb.AppendLine(method.BodySnippet);
                    sb.AppendLine("```");
                    sb.AppendLine();
                }

                if (method.CalledMethods.Any())
                {
                    sb.AppendLine("**Métodos Chamados**:");
                    sb.AppendLine();
                    foreach (var called in method.CalledMethods.Take(10))
                    {
                        sb.AppendLine($"- `{called}`");
                    }
                    sb.AppendLine();
                }

                sb.AppendLine("---");
                sb.AppendLine();
            }

            // Dependências
            if (facts.Dependencies.Any())
            {
                sb.AppendLine("## Dependências");
                sb.AppendLine();
                sb.AppendLine("| Interface | Campo | Descrição |");
                sb.AppendLine("|-----------|-------|-----------|");

                foreach (var dep in facts.Dependencies)
                {
                    sb.AppendLine($"| `{dep.InterfaceName}` | `{dep.FieldName}` | {dep.Description} |");
                }
                sb.AppendLine();
            }

            // Propriedades
            if (facts.Properties.Any())
            {
                sb.AppendLine("## Propriedades");
                sb.AppendLine();
                sb.AppendLine("| Nome | Tipo | Get | Set | Atributos |");
                sb.AppendLine("|------|------|-----|-----|-----------|");

                foreach (var prop in facts.Properties)
                {
                    sb.AppendLine($"| `{prop.Name}` | `{prop.Type}` | {(prop.HasGetter ? "✅" : "❌")} | {(prop.HasSetter ? "✅" : "❌")} | {string.Join(", ", prop.Attributes)} |");
                }
                sb.AppendLine();
            }
        }

        private void GenerateRazorSections(StringBuilder sb, RazorFacts facts, DetailLevel level)
        {
            sb.AppendLine("## Estrutura da Página");
            sb.AppendLine();

            if (!string.IsNullOrEmpty(facts.ModelType))
                sb.AppendLine($"**Model**: `{facts.ModelType}`");

            if (!string.IsNullOrEmpty(facts.Layout))
                sb.AppendLine($"**Layout**: `{facts.Layout}`");

            sb.AppendLine();

            if (facts.Sections.Any())
            {
                sb.AppendLine("### Sections");
                sb.AppendLine();
                foreach (var section in facts.Sections)
                {
                    sb.AppendLine($"- `@section {section}`");
                }
                sb.AppendLine();
            }

            if (facts.Partials.Any())
            {
                sb.AppendLine("### Partials");
                sb.AppendLine();
                foreach (var partial in facts.Partials)
                {
                    sb.AppendLine($"- `{partial}`");
                }
                sb.AppendLine();
            }

            if (facts.FormFields.Any())
            {
                sb.AppendLine("### Campos de Formulário");
                sb.AppendLine();
                sb.AppendLine("| Tipo | Campo |");
                sb.AppendLine("|------|-------|");
                foreach (var field in facts.FormFields)
                {
                    var parts = field.Split(':');
                    sb.AppendLine($"| `{parts[0]}` | `{(parts.Length > 1 ? parts[1] : "")}` |");
                }
                sb.AppendLine();
            }

            sb.AppendLine("## Scripts e Estilos");
            sb.AppendLine();

            if (facts.ScriptReferences.Any())
            {
                sb.AppendLine("### Scripts Referenciados");
                sb.AppendLine();
                foreach (var script in facts.ScriptReferences)
                {
                    sb.AppendLine($"- `{script}`");
                }
                sb.AppendLine();
            }

            if (facts.StyleReferences.Any())
            {
                sb.AppendLine("### Estilos Referenciados");
                sb.AppendLine();
                foreach (var style in facts.StyleReferences)
                {
                    sb.AppendLine($"- `{style}`");
                }
                sb.AppendLine();
            }

            if (facts.AjaxEndpoints.Any())
            {
                sb.AppendLine("### Endpoints AJAX");
                sb.AppendLine();
                foreach (var endpoint in facts.AjaxEndpoints)
                {
                    sb.AppendLine($"- `{endpoint}`");
                }
                sb.AppendLine();
            }
        }

        private void GenerateJavaScriptSections(StringBuilder sb, JavaScriptFacts facts, DetailLevel level)
        {
            sb.AppendLine("## Funções");
            sb.AppendLine();

            foreach (var func in facts.Functions)
            {
                var asyncPrefix = func.IsAsync ? "async " : "";
                var arrowSuffix = func.IsArrowFunction ? " (arrow)" : "";

                sb.AppendLine($"### `{asyncPrefix}{func.Name}({string.Join(", ", func.Parameters)})`{arrowSuffix}");
                sb.AppendLine();
                sb.AppendLine($"**Linha**: {func.StartLine}");
                sb.AppendLine();

                if (level >= DetailLevel.Standard && !string.IsNullOrEmpty(func.BodySnippet))
                {
                    sb.AppendLine("**Código**:");
                    sb.AppendLine();
                    sb.AppendLine("```javascript");
                    sb.AppendLine(func.BodySnippet);
                    sb.AppendLine("```");
                    sb.AppendLine();
                }

                sb.AppendLine("---");
                sb.AppendLine();
            }

            if (facts.AjaxCalls.Any())
            {
                sb.AppendLine("## Chamadas AJAX");
                sb.AppendLine();
                foreach (var ajax in facts.AjaxCalls)
                {
                    sb.AppendLine($"- `{ajax}`");
                }
                sb.AppendLine();
            }

            if (facts.EventListeners.Any())
            {
                sb.AppendLine("## Event Listeners");
                sb.AppendLine();
                foreach (var evt in facts.EventListeners)
                {
                    sb.AppendLine($"- `{evt}`");
                }
                sb.AppendLine();
            }
        }

        private void GenerateInterconnections(StringBuilder sb, DocumentFacts facts)
        {
            sb.AppendLine("### Quem Chama Este Arquivo");
            sb.AppendLine();
            sb.AppendLine("*(Análise manual necessária)*");
            sb.AppendLine();

            sb.AppendLine("### O Que Este Arquivo Chama");
            sb.AppendLine();

            if (facts.CSharpFacts != null)
            {
                foreach (var dep in facts.CSharpFacts.Dependencies)
                {
                    sb.AppendLine($"- `{dep.InterfaceName}` → Injetado via construtor");
                }
            }

            if (facts.RazorFacts != null && facts.RazorFacts.AjaxEndpoints.Any())
            {
                foreach (var endpoint in facts.RazorFacts.AjaxEndpoints)
                {
                    sb.AppendLine($"- API: `{endpoint}`");
                }
            }

            if (facts.JavaScriptFacts != null && facts.JavaScriptFacts.AjaxCalls.Any())
            {
                foreach (var ajax in facts.JavaScriptFacts.AjaxCalls)
                {
                    sb.AppendLine($"- AJAX: `{ajax}`");
                }
            }

            sb.AppendLine();
        }

        private void GenerateTroubleshooting(StringBuilder sb, DocumentFacts facts)
        {
            sb.AppendLine("### Problemas Comuns");
            sb.AppendLine();

            sb.AppendLine("#### Erro: Exceção não tratada");
            sb.AppendLine();
            sb.AppendLine("**Sintoma**: Erro 500 ao acessar a funcionalidade");
            sb.AppendLine();
            sb.AppendLine("**Solução**: Verificar logs em `/Documentacao/Logs/` e consultar `Alerta.TratamentoErroComLinha()`");
            sb.AppendLine();

            if (facts.CSharpFacts != null && facts.CSharpFacts.Dependencies.Any(d => d.InterfaceName.Contains("IUnitOfWork")))
            {
                sb.AppendLine("#### Erro: Dados não persistidos");
                sb.AppendLine();
                sb.AppendLine("**Sintoma**: Alterações não salvas no banco");
                sb.AppendLine();
                sb.AppendLine("**Solução**: Verificar se `_unitOfWork.SaveChanges()` está sendo chamado");
                sb.AppendLine();
            }
        }

        private LayoutPlan CreateLayoutPlan(DocumentFacts facts, Dictionary<string, string> iconMap)
        {
            var plan = new LayoutPlan
            {
                Theme = "default",
                PrimaryColor = "#b66a3d",
                SecondaryColor = "#722F37",
                IconMap = iconMap
            };

            // Estimar número de páginas baseado no tamanho do conteúdo
            var estimatedLines = facts.TotalLines;
            var linesPerPage = 60; // Aproximado para A4
            var pageCount = Math.Max(1, (int)Math.Ceiling(estimatedLines / (double)linesPerPage));

            for (int i = 1; i <= pageCount; i++)
            {
                plan.Pages.Add(new PagePlan
                {
                    PageNumber = i,
                    Title = i == 1 ? facts.File.FileName : $"{facts.File.FileName} (pág. {i})",
                    HasHero = i == 1,
                    HasToc = i == 1,
                    Sections = new List<SectionPlan>()
                });
            }

            return plan;
        }

        #endregion

        private IDocAiProvider? GetAiProvider(AiProvider provider)
        {
            return provider switch
            {
                AiProvider.OpenAI => _serviceProvider.GetService<Providers.OpenAiDocProvider>(),
                AiProvider.Claude => _serviceProvider.GetService<Providers.ClaudeDocProvider>(),
                AiProvider.Gemini => _serviceProvider.GetService<Providers.GeminiDocProvider>(),
                _ => null
            };
        }
    }
}
