/*
 * ╔══════════════════════════════════════════════════════════════════════════╗
 * ║                                                                          ║
 * ║  📚 DOCUMENTAÇÃO DISPONÍVEL                                              ║
 * ║                                                                          ║
 * ║  Este arquivo está documentado em:                                       ║
 * ║  📄 Documentacao/Services/DocGenerator/DocExtractionService.md           ║
 * ║                                                                          ║
 * ║  Última atualização: 13/01/2026                                          ║
 * ║                                                                          ║
 * ╚══════════════════════════════════════════════════════════════════════════╝
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using FrotiX.Services.DocGenerator.Interfaces;
using FrotiX.Services.DocGenerator.Models;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Logging;

namespace FrotiX.Services.DocGenerator
{
    /// <summary>
    /// Serviço de extração de metadados de arquivos usando Roslyn e Regex
    /// </summary>
    public class DocExtractionService : IDocExtractionService
    {
        private readonly ILogger<DocExtractionService> _logger;

        public DocExtractionService(ILogger<DocExtractionService> logger)
        {
            try
            {
                _logger = logger;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("DocExtractionService.cs", ".ctor", error);
            }
        }

        public async Task<DocumentFacts> ExtractAsync(DiscoveredFile file, CancellationToken ct = default)
        {
            var facts = new DocumentFacts
            {
                File = file
            };

            try
            {
                if (!File.Exists(file.FullPath))
                {
                    facts.Warnings.Add($"Arquivo não encontrado: {file.FullPath}");
                    return facts;
                }

                var content = await File.ReadAllTextAsync(file.FullPath, ct);
                facts.RawContent = content;
                facts.TotalLines = content.Split('\n').Length;

                // Extrair fatos baseado na categoria
                switch (file.Category)
                {
                    case FileCategory.Controller:
                    case FileCategory.Service:
                    case FileCategory.Repository:
                    case FileCategory.Helper:
                    case FileCategory.Middleware:
                    case FileCategory.Hub:
                    case FileCategory.Model:
                    case FileCategory.ViewModel:
                    case FileCategory.Data:
                    case FileCategory.RazorPageModel:
                        facts.CSharpFacts = await ExtractCSharpAsync(file.FullPath, content, ct);
                        break;

                    case FileCategory.RazorPage:
                        facts.RazorFacts = await ExtractRazorAsync(file.FullPath, content, ct);
                        break;

                    case FileCategory.JavaScript:
                        facts.JavaScriptFacts = await ExtractJavaScriptAsync(file.FullPath, content, ct);
                        break;

                    case FileCategory.Css:
                        // CSS não precisa de extração especial, usa conteúdo bruto
                        break;
                }

                _logger.LogDebug("Extração concluída para {File}: {Lines} linhas",
                    file.FileName, facts.TotalLines);
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("DocExtractionService.cs", "ExtractAsync", error);
                facts.Warnings.Add($"Erro na extração: {error.Message}");
            }

            return facts;
        }

        public async Task<CSharpFacts> ExtractCSharpAsync(string filePath, string content, CancellationToken ct = default)
        {
            var facts = new CSharpFacts();

            try
            {
                var tree = CSharpSyntaxTree.ParseText(content, cancellationToken: ct);
                var root = await tree.GetRootAsync(ct);

                // Namespace
                var namespaceDecl = root.DescendantNodes().OfType<BaseNamespaceDeclarationSyntax>().FirstOrDefault();
                facts.Namespace = namespaceDecl?.Name.ToString() ?? string.Empty;

                // Usings
                facts.UsingStatements = root.DescendantNodes()
                    .OfType<UsingDirectiveSyntax>()
                    .Select(u => u.Name?.ToString() ?? string.Empty)
                    .Where(s => !string.IsNullOrEmpty(s))
                    .ToList();

                // Classes
                var classDecl = root.DescendantNodes().OfType<ClassDeclarationSyntax>().FirstOrDefault();
                if (classDecl != null)
                {
                    facts.ClassName = classDecl.Identifier.Text;

                    // Classe base
                    if (classDecl.BaseList != null)
                    {
                        var baseTypes = classDecl.BaseList.Types.ToList();
                        if (baseTypes.Count > 0)
                        {
                            var firstBase = baseTypes[0].Type.ToString();
                            // Se não começa com I, é classe base
                            if (!firstBase.StartsWith("I") || firstBase.Length < 2 || !char.IsUpper(firstBase[1]))
                            {
                                facts.BaseClass = firstBase;
                            }
                        }

                        // Interfaces
                        facts.Interfaces = baseTypes
                            .Select(t => t.Type.ToString())
                            .Where(t => t.StartsWith("I") && t.Length > 1 && char.IsUpper(t[1]))
                            .ToList();
                    }

                    // Atributos da classe
                    facts.Attributes = classDecl.AttributeLists
                        .SelectMany(al => al.Attributes)
                        .Select(a => a.ToString())
                        .ToList();

                    // XML Summary da classe
                    var classTrivia = classDecl.GetLeadingTrivia();
                    facts.Summary = ExtractXmlSummary(classTrivia);

                    // Extrair dependências do construtor
                    ExtractDependencies(classDecl, facts);

                    // Extrair métodos
                    ExtractMethods(classDecl, facts, content);

                    // Extrair propriedades
                    ExtractProperties(classDecl, facts);
                }
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("DocExtractionService.cs", "ExtractCSharpAsync", error);
            }

            return facts;
        }

        public async Task<RazorFacts> ExtractRazorAsync(string filePath, string content, CancellationToken ct = default)
        {
            var facts = new RazorFacts();

            try
            {
                await Task.Yield(); // Para manter async

                // @page directive
                var pageMatch = Regex.Match(content, @"@page\s+""?([^""\s]*)""?");
                facts.PageDirective = pageMatch.Success ? pageMatch.Groups[1].Value : string.Empty;

                // @model
                var modelMatch = Regex.Match(content, @"@model\s+(\S+)");
                facts.ModelType = modelMatch.Success ? modelMatch.Groups[1].Value : string.Empty;

                // @layout
                var layoutMatch = Regex.Match(content, @"@\{\s*Layout\s*=\s*""([^""]+)""");
                facts.Layout = layoutMatch.Success ? layoutMatch.Groups[1].Value : string.Empty;

                // @section
                var sectionMatches = Regex.Matches(content, @"@section\s+(\w+)");
                facts.Sections = sectionMatches.Cast<Match>().Select(m => m.Groups[1].Value).ToList();

                // Partials (@await Html.PartialAsync, <partial name=)
                var partialMatches = Regex.Matches(content, @"(?:Html\.Partial(?:Async)?\s*\(\s*""([^""]+)""|<partial\s+name=""([^""]+)"")");
                facts.Partials = partialMatches.Cast<Match>()
                    .SelectMany(m => new[] { m.Groups[1].Value, m.Groups[2].Value })
                    .Where(s => !string.IsNullOrEmpty(s))
                    .Distinct()
                    .ToList();

                // Tag Helpers
                var tagHelperMatches = Regex.Matches(content, @"<([\w-]+)\s+(?:asp-[\w-]+)");
                facts.TagHelpers = tagHelperMatches.Cast<Match>()
                    .Select(m => m.Groups[1].Value)
                    .Distinct()
                    .ToList();

                // Script references
                var scriptMatches = Regex.Matches(content, @"<script[^>]+src=""([^""]+)""");
                facts.ScriptReferences = scriptMatches.Cast<Match>()
                    .Select(m => m.Groups[1].Value)
                    .ToList();

                // Style references
                var styleMatches = Regex.Matches(content, @"<link[^>]+href=""([^""]+\.css)""");
                facts.StyleReferences = styleMatches.Cast<Match>()
                    .Select(m => m.Groups[1].Value)
                    .ToList();

                // Form fields (input, select, textarea)
                var inputMatches = Regex.Matches(content, @"<(input|select|textarea)[^>]+(?:asp-for|name)=""([^""]+)""");
                facts.FormFields = inputMatches.Cast<Match>()
                    .Select(m => $"{m.Groups[1].Value}:{m.Groups[2].Value}")
                    .ToList();

                // AJAX endpoints (fetch, $.ajax, axios)
                var ajaxMatches = Regex.Matches(content, @"(?:fetch|axios\.[a-z]+|\$\.(?:ajax|get|post))\s*\(\s*['""]([^'""]+)['""]");
                facts.AjaxEndpoints = ajaxMatches.Cast<Match>()
                    .Select(m => m.Groups[1].Value)
                    .Distinct()
                    .ToList();

                // Inline styles
                var styleBlockMatch = Regex.Match(content, @"<style[^>]*>([\s\S]*?)</style>", RegexOptions.IgnoreCase);
                facts.InlineStyles = styleBlockMatch.Success ? styleBlockMatch.Groups[1].Value.Trim() : string.Empty;

                // Inline scripts
                var scriptBlockMatches = Regex.Matches(content, @"<script[^>]*>(?!.*src)([\s\S]*?)</script>", RegexOptions.IgnoreCase);
                facts.InlineScripts = string.Join("\n\n", scriptBlockMatches.Cast<Match>().Select(m => m.Groups[1].Value.Trim()));
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("DocExtractionService.cs", "ExtractRazorAsync", error);
            }

            return facts;
        }

        public async Task<JavaScriptFacts> ExtractJavaScriptAsync(string filePath, string content, CancellationToken ct = default)
        {
            var facts = new JavaScriptFacts();

            try
            {
                await Task.Yield(); // Para manter async

                // Detectar bibliotecas
                facts.UsesJQuery = content.Contains("$") || content.Contains("jQuery");
                facts.UsesSyncfusion = content.Contains("ej.") || content.Contains("ej2");
                facts.UsesDataTables = content.Contains("DataTable") || content.Contains("dataTable");

                // Funções normais e arrow functions
                ExtractJsFunctions(content, facts);

                // Variáveis globais (var/let/const no topo)
                var globalVarMatches = Regex.Matches(content, @"^(?:var|let|const)\s+(\w+)\s*=", RegexOptions.Multiline);
                facts.GlobalVariables = globalVarMatches.Cast<Match>()
                    .Select(m => m.Groups[1].Value)
                    .Take(50)
                    .ToList();

                // Event Listeners
                var eventMatches = Regex.Matches(content, @"(?:addEventListener|\.on)\s*\(\s*['""](\w+)['""]");
                facts.EventListeners = eventMatches.Cast<Match>()
                    .Select(m => m.Groups[1].Value)
                    .Distinct()
                    .ToList();

                // Chamadas AJAX (fetch, $.ajax, $.get, $.post, axios)
                var ajaxPatterns = new[]
                {
                    @"fetch\s*\(\s*['""`]([^'""` ]+)['""`]",
                    @"\$\.(?:ajax|get|post|getJSON)\s*\(\s*\{?\s*(?:url\s*:\s*)?['""`]?([^'""`,\s\}]+)",
                    @"axios\.[a-z]+\s*\(\s*['""`]([^'""` ]+)['""`]"
                };

                foreach (var pattern in ajaxPatterns)
                {
                    var matches = Regex.Matches(content, pattern, RegexOptions.IgnoreCase);
                    foreach (Match match in matches)
                    {
                        var url = match.Groups[1].Value;
                        if (!string.IsNullOrWhiteSpace(url) && !facts.AjaxCalls.Contains(url))
                        {
                            facts.AjaxCalls.Add(url);
                        }
                    }
                }

                // Manipulações DOM
                var domPatterns = new[] { "getElementById", "querySelector", "getElementsByClassName", "getElementsByTagName", "innerHTML", "textContent" };
                facts.DomManipulations = domPatterns.Where(p => content.Contains(p)).ToList();

                // Dependências (import, require)
                var importMatches = Regex.Matches(content, @"(?:import\s+.*?\s+from\s+|require\s*\(\s*)['""]([^'""]+)['""]");
                facts.Dependencies = importMatches.Cast<Match>()
                    .Select(m => m.Groups[1].Value)
                    .Distinct()
                    .ToList();
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("DocExtractionService.cs", "ExtractJavaScriptAsync", error);
            }

            return facts;
        }

        #region Helper Methods

        private void ExtractDependencies(ClassDeclarationSyntax classDecl, CSharpFacts facts)
        {
            var constructors = classDecl.Members.OfType<ConstructorDeclarationSyntax>();

            foreach (var ctor in constructors)
            {
                foreach (var param in ctor.ParameterList.Parameters)
                {
                    var typeName = param.Type?.ToString() ?? string.Empty;
                    var paramName = param.Identifier.Text;

                    // Procurar assignment no corpo do construtor
                    var fieldName = $"_{char.ToLower(paramName[0])}{paramName.Substring(1)}";

                    facts.Dependencies.Add(new DependencyInfo
                    {
                        InterfaceName = typeName,
                        FieldName = fieldName,
                        Description = $"Injetado via construtor"
                    });
                }
            }
        }

        private void ExtractMethods(ClassDeclarationSyntax classDecl, CSharpFacts facts, string content)
        {
            var methods = classDecl.Members.OfType<MethodDeclarationSyntax>();
            var lines = content.Split('\n');

            foreach (var method in methods)
            {
                var methodInfo = new Models.MethodInfo
                {
                    Name = method.Identifier.Text,
                    ReturnType = method.ReturnType.ToString(),
                    IsAsync = method.Modifiers.Any(m => m.Text == "async")
                };

                // Parâmetros
                foreach (var param in method.ParameterList.Parameters)
                {
                    methodInfo.Parameters.Add(new Models.ParameterInfo
                    {
                        Name = param.Identifier.Text,
                        Type = param.Type?.ToString() ?? "object",
                        DefaultValue = param.Default?.Value.ToString() ?? string.Empty,
                        Attributes = param.AttributeLists
                            .SelectMany(al => al.Attributes)
                            .Select(a => a.ToString())
                            .ToList()
                    });
                }

                // Atributos do método
                methodInfo.Attributes = method.AttributeLists
                    .SelectMany(al => al.Attributes)
                    .Select(a => a.ToString())
                    .ToList();

                // HTTP Method e Route (para Controllers)
                foreach (var attr in methodInfo.Attributes)
                {
                    if (attr.StartsWith("HttpGet"))
                    {
                        methodInfo.HttpMethod = "GET";
                        var routeMatch = Regex.Match(attr, @"""([^""]+)""");
                        methodInfo.Route = routeMatch.Success ? routeMatch.Groups[1].Value : string.Empty;
                    }
                    else if (attr.StartsWith("HttpPost"))
                    {
                        methodInfo.HttpMethod = "POST";
                        var routeMatch = Regex.Match(attr, @"""([^""]+)""");
                        methodInfo.Route = routeMatch.Success ? routeMatch.Groups[1].Value : string.Empty;
                    }
                    else if (attr.StartsWith("HttpPut"))
                    {
                        methodInfo.HttpMethod = "PUT";
                        var routeMatch = Regex.Match(attr, @"""([^""]+)""");
                        methodInfo.Route = routeMatch.Success ? routeMatch.Groups[1].Value : string.Empty;
                    }
                    else if (attr.StartsWith("HttpDelete"))
                    {
                        methodInfo.HttpMethod = "DELETE";
                        var routeMatch = Regex.Match(attr, @"""([^""]+)""");
                        methodInfo.Route = routeMatch.Success ? routeMatch.Groups[1].Value : string.Empty;
                    }
                }

                // Summary
                var methodTrivia = method.GetLeadingTrivia();
                methodInfo.Summary = ExtractXmlSummary(methodTrivia);

                // Linhas
                methodInfo.StartLine = method.GetLocation().GetLineSpan().StartLinePosition.Line + 1;
                methodInfo.EndLine = method.GetLocation().GetLineSpan().EndLinePosition.Line + 1;

                // Corpo do método (snippet)
                if (method.Body != null)
                {
                    var bodyText = method.Body.ToString();
                    methodInfo.BodySnippet = bodyText.Length > 1000
                        ? bodyText.Substring(0, 1000) + "..."
                        : bodyText;

                    // Métodos chamados
                    var invocations = method.Body.DescendantNodes().OfType<InvocationExpressionSyntax>();
                    methodInfo.CalledMethods = invocations
                        .Select(i => i.Expression.ToString())
                        .Where(s => !s.Contains("(")) // Remove chamadas aninhadas
                        .Distinct()
                        .Take(20)
                        .ToList();
                }

                facts.Methods.Add(methodInfo);
            }
        }

        private void ExtractProperties(ClassDeclarationSyntax classDecl, CSharpFacts facts)
        {
            var properties = classDecl.Members.OfType<PropertyDeclarationSyntax>();

            foreach (var prop in properties)
            {
                var propInfo = new Models.PropertyInfo
                {
                    Name = prop.Identifier.Text,
                    Type = prop.Type.ToString(),
                    HasGetter = prop.AccessorList?.Accessors.Any(a => a.Kind() == SyntaxKind.GetAccessorDeclaration) ?? false,
                    HasSetter = prop.AccessorList?.Accessors.Any(a => a.Kind() == SyntaxKind.SetAccessorDeclaration) ?? false
                };

                propInfo.Attributes = prop.AttributeLists
                    .SelectMany(al => al.Attributes)
                    .Select(a => a.ToString())
                    .ToList();

                var propTrivia = prop.GetLeadingTrivia();
                propInfo.Summary = ExtractXmlSummary(propTrivia);

                facts.Properties.Add(propInfo);
            }
        }

        private string ExtractXmlSummary(SyntaxTriviaList trivia)
        {
            foreach (var t in trivia)
            {
                if (t.IsKind(SyntaxKind.SingleLineDocumentationCommentTrivia) ||
                    t.IsKind(SyntaxKind.MultiLineDocumentationCommentTrivia))
                {
                    var xml = t.ToString();
                    var summaryMatch = Regex.Match(xml, @"<summary>\s*(.*?)\s*</summary>", RegexOptions.Singleline);
                    if (summaryMatch.Success)
                    {
                        return Regex.Replace(summaryMatch.Groups[1].Value, @"///\s*", "").Trim();
                    }
                }
            }
            return string.Empty;
        }

        private void ExtractJsFunctions(string content, JavaScriptFacts facts)
        {
            var lines = content.Split('\n');

            // Funções tradicionais: function name(params)
            var funcPattern = @"(?:async\s+)?function\s+(\w+)\s*\(([^)]*)\)";
            var funcMatches = Regex.Matches(content, funcPattern);

            foreach (Match match in funcMatches)
            {
                var func = new JsFunctionInfo
                {
                    Name = match.Groups[1].Value,
                    Parameters = match.Groups[2].Value
                        .Split(',')
                        .Select(p => p.Trim())
                        .Where(p => !string.IsNullOrEmpty(p))
                        .ToList(),
                    IsAsync = match.Value.StartsWith("async"),
                    IsArrowFunction = false
                };

                // Encontrar linha
                var index = match.Index;
                func.StartLine = content.Substring(0, index).Count(c => c == '\n') + 1;

                facts.Functions.Add(func);
            }

            // Arrow functions: const name = (params) => ou const name = async (params) =>
            var arrowPattern = @"(?:const|let|var)\s+(\w+)\s*=\s*(?:async\s+)?\(?([^)=]*)\)?\s*=>";
            var arrowMatches = Regex.Matches(content, arrowPattern);

            foreach (Match match in arrowMatches)
            {
                var func = new JsFunctionInfo
                {
                    Name = match.Groups[1].Value,
                    Parameters = match.Groups[2].Value
                        .Split(',')
                        .Select(p => p.Trim())
                        .Where(p => !string.IsNullOrEmpty(p))
                        .ToList(),
                    IsAsync = match.Value.Contains("async"),
                    IsArrowFunction = true
                };

                var index = match.Index;
                func.StartLine = content.Substring(0, index).Count(c => c == '\n') + 1;

                facts.Functions.Add(func);
            }

            // Métodos de objeto: name: function(params) ou name(params) { dentro de objetos
            var methodPattern = @"(\w+)\s*:\s*(?:async\s+)?function\s*\(([^)]*)\)";
            var methodMatches = Regex.Matches(content, methodPattern);

            foreach (Match match in methodMatches)
            {
                var funcName = match.Groups[1].Value;
                if (!facts.Functions.Any(f => f.Name == funcName))
                {
                    facts.Functions.Add(new JsFunctionInfo
                    {
                        Name = funcName,
                        Parameters = match.Groups[2].Value
                            .Split(',')
                            .Select(p => p.Trim())
                            .Where(p => !string.IsNullOrEmpty(p))
                            .ToList(),
                        IsAsync = match.Value.Contains("async"),
                        IsArrowFunction = false,
                        StartLine = content.Substring(0, match.Index).Count(c => c == '\n') + 1
                    });
                }
            }
        }

        #endregion
    }
}
