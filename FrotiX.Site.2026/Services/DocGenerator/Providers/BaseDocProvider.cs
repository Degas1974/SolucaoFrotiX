/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: BaseDocProvider.cs                                                                      ║
   ║ 📂 CAMINHO: /Services/DocGenerator/Providers                                                        ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO: Classe base abstrata para provedores de IA. Prompts, parsing, catálogo de ícones.      ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 ÍNDICE: IconCatalog (dics), BuildPrompt(), ParseResponse(), GetIconForCategory()                 ║
   ║ 🔗 DEPS: ILogger, Models | 📅 15/01/2026 | 👤 Copilot | 📝 v2.0                                     ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝
*/

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using FrotiX.Services.DocGenerator.Models;
using Microsoft.Extensions.Logging;

namespace FrotiX.Services.DocGenerator.Providers
{
    /// <summary>
    /// Classe base abstrata para provedores de IA do DocGenerator.
    /// Centraliza lógica comum de construção de prompts, parsing de respostas e ícones.
    /// </summary>
    public abstract class BaseDocProvider
    {
        protected readonly ILogger _logger;

        protected BaseDocProvider(ILogger logger)
        {
            _logger = logger;
        }

        #region Catálogo de Ícones FontAwesome Duotone

        /// <summary>
        /// Catálogo completo de ícones FontAwesome Duotone organizados por categoria
        /// </summary>
        public static readonly Dictionary<string, Dictionary<string, string>> IconCatalog = new()
        {
            ["Seções"] = new Dictionary<string, string>
            {
                { "Visão Geral", "fa-duotone fa-telescope" },
                { "Arquitetura", "fa-duotone fa-diagram-project" },
                { "Métodos", "fa-duotone fa-brackets-curly" },
                { "Funções", "fa-duotone fa-function" },
                { "Endpoints", "fa-duotone fa-webhook" },
                { "API", "fa-duotone fa-cloud-arrow-up" },
                { "Interconexões", "fa-duotone fa-link-horizontal" },
                { "Dependências", "fa-duotone fa-puzzle-piece" },
                { "Validações", "fa-duotone fa-shield-check" },
                { "Segurança", "fa-duotone fa-lock-keyhole" },
                { "Troubleshooting", "fa-duotone fa-toolbox" },
                { "Erros", "fa-duotone fa-triangle-exclamation" },
                { "Log", "fa-duotone fa-clipboard-list-check" },
                { "Histórico", "fa-duotone fa-clock-rotate-left" },
                { "Frontend", "fa-duotone fa-browser" },
                { "Backend", "fa-duotone fa-server" },
                { "Banco de Dados", "fa-duotone fa-database" },
                { "Cache", "fa-duotone fa-hard-drive" },
                { "Configuração", "fa-duotone fa-sliders" },
                { "Testes", "fa-duotone fa-vial" },
                { "Performance", "fa-duotone fa-gauge-high" },
                { "Exemplos", "fa-duotone fa-lightbulb" },
                { "Referências", "fa-duotone fa-book-bookmark" }
            },
            ["Tipos de Arquivo"] = new Dictionary<string, string>
            {
                { "Controller", "fa-duotone fa-sitemap" },
                { "Service", "fa-duotone fa-gears" },
                { "Repository", "fa-duotone fa-layer-group" },
                { "Model", "fa-duotone fa-cubes" },
                { "ViewModel", "fa-duotone fa-object-group" },
                { "Helper", "fa-duotone fa-hand-holding-magic" },
                { "Middleware", "fa-duotone fa-filter" },
                { "Hub", "fa-duotone fa-tower-broadcast" },
                { "RazorPage", "fa-duotone fa-file-code" },
                { "JavaScript", "fa-duotone fa-js" },
                { "CSS", "fa-duotone fa-palette" }
            },
            ["Ações"] = new Dictionary<string, string>
            {
                { "Criar", "fa-duotone fa-circle-plus" },
                { "Editar", "fa-duotone fa-pen-to-square" },
                { "Excluir", "fa-duotone fa-trash-can" },
                { "Salvar", "fa-duotone fa-floppy-disk" },
                { "Buscar", "fa-duotone fa-magnifying-glass" },
                { "Filtrar", "fa-duotone fa-filter-list" },
                { "Exportar", "fa-duotone fa-file-export" },
                { "Importar", "fa-duotone fa-file-import" },
                { "Enviar", "fa-duotone fa-paper-plane" },
                { "Receber", "fa-duotone fa-inbox" },
                { "Processar", "fa-duotone fa-microchip" },
                { "Validar", "fa-duotone fa-badge-check" },
                { "Aprovar", "fa-duotone fa-thumbs-up" },
                { "Rejeitar", "fa-duotone fa-thumbs-down" },
                { "Cancelar", "fa-duotone fa-circle-xmark" },
                { "Confirmar", "fa-duotone fa-circle-check" }
            },
            ["Status"] = new Dictionary<string, string>
            {
                { "Sucesso", "fa-duotone fa-circle-check" },
                { "Erro", "fa-duotone fa-circle-xmark" },
                { "Aviso", "fa-duotone fa-triangle-exclamation" },
                { "Info", "fa-duotone fa-circle-info" },
                { "Pendente", "fa-duotone fa-clock" },
                { "Processando", "fa-duotone fa-spinner" },
                { "Ativo", "fa-duotone fa-toggle-on" },
                { "Inativo", "fa-duotone fa-toggle-off" }
            },
            ["Objetos"] = new Dictionary<string, string>
            {
                { "Usuário", "fa-duotone fa-user" },
                { "Usuários", "fa-duotone fa-users" },
                { "Empresa", "fa-duotone fa-building" },
                { "Veículo", "fa-duotone fa-car" },
                { "Motorista", "fa-duotone fa-id-card" },
                { "Documento", "fa-duotone fa-file-lines" },
                { "Relatório", "fa-duotone fa-chart-pie" },
                { "Dashboard", "fa-duotone fa-chart-line" },
                { "Calendário", "fa-duotone fa-calendar-days" },
                { "Notificação", "fa-duotone fa-bell" },
                { "Mensagem", "fa-duotone fa-envelope" },
                { "Anexo", "fa-duotone fa-paperclip" },
                { "Imagem", "fa-duotone fa-image" },
                { "PDF", "fa-duotone fa-file-pdf" },
                { "Excel", "fa-duotone fa-file-excel" }
            }
        };

        /// <summary>
        /// Obtém o mapa de ícones padrão para seções de documentação
        /// </summary>
        public static Dictionary<string, string> GetDefaultIconMap()
        {
            return new Dictionary<string, string>
            {
                { "Visão Geral", "fa-duotone fa-telescope" },
                { "Arquitetura", "fa-duotone fa-diagram-project" },
                { "Métodos", "fa-duotone fa-brackets-curly" },
                { "Funções", "fa-duotone fa-function" },
                { "Endpoints", "fa-duotone fa-webhook" },
                { "Interconexões", "fa-duotone fa-link-horizontal" },
                { "Dependências", "fa-duotone fa-puzzle-piece" },
                { "Validações", "fa-duotone fa-shield-check" },
                { "Troubleshooting", "fa-duotone fa-toolbox" },
                { "Log", "fa-duotone fa-clipboard-list-check" },
                { "Frontend", "fa-duotone fa-browser" },
                { "Backend", "fa-duotone fa-server" },
                { "Exemplos", "fa-duotone fa-lightbulb" },
                { "Referências", "fa-duotone fa-book-bookmark" }
            };
        }

        #endregion

        #region System Prompt - Orientações Gerais

        /// <summary>
        /// Prompt de sistema otimizado para documentação rica e visual
        /// </summary>
        protected virtual string GetSystemPrompt()
        {
            return @"Você é um DOCUMENTADOR TÉCNICO SÊNIOR especializado em ASP.NET Core, trabalhando no projeto FrotiX (sistema de gestão de frotas).

## SUA MISSÃO
Criar documentação EXCEPCIONAL que seja:
- **VISUAL**: Use ícones FontAwesome Duotone em TODOS os títulos e subtítulos
- **EXPLICATIVA**: Foque em EXPLICAR o propósito e funcionamento, não apenas listar código
- **PROFISSIONAL**: Documentação de nível enterprise, digna de um projeto corporativo
- **ACESSÍVEL**: Linguagem clara para desenvolvedores de todos os níveis

## REGRAS DE FORMATAÇÃO OBRIGATÓRIAS

### 1. ÍCONES FONTAWESOME DUOTONE
SEMPRE use ícones nos headings no formato: `## <i class=""fa-duotone fa-nome-icone""></i> Título`

Exemplos de ícones disponíveis:
- Visão Geral: fa-duotone fa-telescope
- Arquitetura: fa-duotone fa-diagram-project
- Métodos: fa-duotone fa-brackets-curly
- Funções: fa-duotone fa-function
- Endpoints/API: fa-duotone fa-webhook
- Interconexões: fa-duotone fa-link-horizontal
- Dependências: fa-duotone fa-puzzle-piece
- Validações: fa-duotone fa-shield-check
- Segurança: fa-duotone fa-lock-keyhole
- Troubleshooting: fa-duotone fa-toolbox
- Exemplos: fa-duotone fa-lightbulb
- Performance: fa-duotone fa-gauge-high
- Banco de Dados: fa-duotone fa-database
- Cache: fa-duotone fa-hard-drive
- Testes: fa-duotone fa-vial

### 2. ESTRUTURA DE DOCUMENTAÇÃO
Organize em seções claras:
1. **Visão Geral** - O QUE é e POR QUE existe
2. **Arquitetura** - COMO se encaixa no sistema
3. **Funcionalidades** - O QUE faz (com explicações detalhadas)
4. **Interconexões** - COM QUEM se comunica
5. **Exemplos de Uso** - COMO usar na prática
6. **Troubleshooting** - Problemas comuns e soluções

### 3. ESTILO DE ESCRITA
- **EVITE** blocos de código muito longos (máximo 15-20 linhas)
- **PREFIRA** explicações textuais com pequenos snippets ilustrativos
- **USE** tabelas para organizar informações
- **USE** listas com ícones para enumerar itens
- **USE** callouts/alertas para destacar informações importantes
- **ESCREVA** parágrafos explicativos entre cada seção de código

### 4. FORMATAÇÃO VISUAL
Use callouts estilizados:
```
> ℹ️ **INFORMAÇÃO**: Texto informativo aqui
> ⚠️ **ATENÇÃO**: Alerta importante aqui
> ✅ **DICA**: Sugestão útil aqui
> ❌ **EVITE**: O que não fazer
```

### 5. LÍNGUA
- Use português brasileiro
- Seja formal mas acessível
- Explique termos técnicos quando necessário

### 6. EXTENSÃO
- Mínimo 500 linhas para arquivos simples
- Mínimo 1000 linhas para arquivos complexos
- Foque em QUALIDADE e PROFUNDIDADE, não apenas quantidade";
        }

        #endregion

        #region Construção de Prompt

        /// <summary>
        /// Constrói o prompt principal para geração de documentação
        /// </summary>
        protected virtual string BuildPrompt(DocumentFacts facts, DocGenerationOptions options)
        {
            var sb = new StringBuilder();

            sb.AppendLine("# TAREFA: Gerar Documentação Técnica Completa");
            sb.AppendLine();
            sb.AppendLine("## Arquivo a Documentar:");
            sb.AppendLine($"- **Nome:** `{facts.File.FileName}`");
            sb.AppendLine($"- **Caminho:** `{facts.File.RelativePath}`");
            sb.AppendLine($"- **Categoria:** {facts.File.Category}");
            sb.AppendLine($"- **Total de Linhas:** {facts.TotalLines}");
            sb.AppendLine();

            // Nível de detalhamento
            var detailInstructions = options.DetailLevel switch
            {
                DetailLevel.Minimal => "Documentação resumida com foco nos pontos principais (mínimo 300 linhas).",
                DetailLevel.Standard => "Documentação padrão com todas as seções (mínimo 500 linhas).",
                DetailLevel.Detailed => "Documentação detalhada com exemplos e casos de uso (mínimo 800 linhas).",
                DetailLevel.Exhaustive => "Documentação exaustiva cobrindo todos os aspectos possíveis (mínimo 1200 linhas).",
                _ => "Documentação padrão (mínimo 500 linhas)."
            };
            sb.AppendLine($"**Nível de Detalhamento:** {detailInstructions}");
            sb.AppendLine();

            // Adiciona fatos específicos por tipo
            if (facts.CSharpFacts != null)
            {
                AppendCSharpFacts(sb, facts.CSharpFacts);
            }

            if (facts.RazorFacts != null)
            {
                AppendRazorFacts(sb, facts.RazorFacts);
            }

            if (facts.JavaScriptFacts != null)
            {
                AppendJavaScriptFacts(sb, facts.JavaScriptFacts);
            }

            // Conteúdo do arquivo (limitado)
            AppendFileContent(sb, facts);

            // Instruções de saída
            AppendOutputInstructions(sb);

            return sb.ToString();
        }

        protected virtual void AppendCSharpFacts(StringBuilder sb, CSharpFacts csharp)
        {
            sb.AppendLine("---");
            sb.AppendLine("## Metadados Extraídos (C#)");
            sb.AppendLine();
            sb.AppendLine("| Propriedade | Valor |");
            sb.AppendLine("|-------------|-------|");
            sb.AppendLine($"| Namespace | `{csharp.Namespace}` |");
            sb.AppendLine($"| Classe | `{csharp.ClassName}` |");
            sb.AppendLine($"| Classe Base | `{csharp.BaseClass ?? "Nenhuma"}` |");
            sb.AppendLine($"| Interfaces | {(csharp.Interfaces.Count > 0 ? string.Join(", ", csharp.Interfaces) : "Nenhuma")} |");
            sb.AppendLine();

            if (csharp.Methods.Count > 0)
            {
                sb.AppendLine("### Métodos Identificados");
                sb.AppendLine();
                sb.AppendLine("| Método | Retorno | HTTP | Rota |");
                sb.AppendLine("|--------|---------|------|------|");
                foreach (var method in csharp.Methods)
                {
                    var httpInfo = !string.IsNullOrEmpty(method.HttpMethod) ? method.HttpMethod : "-";
                    var routeInfo = !string.IsNullOrEmpty(method.Route) ? method.Route : "-";
                    sb.AppendLine($"| `{method.Name}` | `{method.ReturnType}` | {httpInfo} | {routeInfo} |");
                }
                sb.AppendLine();
            }

            if (csharp.Dependencies.Count > 0)
            {
                sb.AppendLine("### Dependências Injetadas");
                sb.AppendLine();
                foreach (var dep in csharp.Dependencies)
                {
                    sb.AppendLine($"- `{dep.InterfaceName}` → campo `{dep.FieldName}`");
                }
                sb.AppendLine();
            }
        }

        protected virtual void AppendRazorFacts(StringBuilder sb, RazorFacts razor)
        {
            sb.AppendLine("---");
            sb.AppendLine("## Metadados Extraídos (Razor Page)");
            sb.AppendLine();
            sb.AppendLine("| Propriedade | Valor |");
            sb.AppendLine("|-------------|-------|");
            sb.AppendLine($"| Model | `{razor.ModelType ?? "Não especificado"}` |");
            sb.AppendLine($"| Layout | `{razor.Layout ?? "Padrão"}` |");
            sb.AppendLine($"| Sections | {(razor.Sections.Count > 0 ? string.Join(", ", razor.Sections) : "Nenhuma")} |");
            sb.AppendLine($"| Partials | {(razor.Partials.Count > 0 ? string.Join(", ", razor.Partials) : "Nenhum")} |");
            sb.AppendLine();
        }

        protected virtual void AppendJavaScriptFacts(StringBuilder sb, JavaScriptFacts js)
        {
            sb.AppendLine("---");
            sb.AppendLine("## Metadados Extraídos (JavaScript)");
            sb.AppendLine();
            sb.AppendLine("| Biblioteca | Detectado |");
            sb.AppendLine("|------------|-----------|");
            sb.AppendLine($"| jQuery | {(js.UsesJQuery ? "✅ Sim" : "❌ Não")} |");
            sb.AppendLine($"| Syncfusion | {(js.UsesSyncfusion ? "✅ Sim" : "❌ Não")} |");
            sb.AppendLine($"| DataTables | {(js.UsesDataTables ? "✅ Sim" : "❌ Não")} |");
            sb.AppendLine();

            if (js.Functions.Count > 0)
            {
                sb.AppendLine("### Funções Identificadas");
                sb.AppendLine();
                foreach (var func in js.Functions)
                {
                    var asyncTag = func.IsAsync ? " `async`" : "";
                    var funcParams = func.Parameters.Count > 0 ? string.Join(", ", func.Parameters) : "";
                    sb.AppendLine($"- `{func.Name}({funcParams})`{asyncTag}");
                }
                sb.AppendLine();
            }

            if (js.AjaxCalls.Count > 0)
            {
                sb.AppendLine("### Chamadas AJAX");
                sb.AppendLine();
                foreach (var ajax in js.AjaxCalls)
                {
                    sb.AppendLine($"- {ajax}");
                }
                sb.AppendLine();
            }
        }

        protected virtual void AppendFileContent(StringBuilder sb, DocumentFacts facts, int maxChars = 12000)
        {
            sb.AppendLine("---");
            sb.AppendLine("## Conteúdo do Arquivo (Referência)");
            sb.AppendLine();
            sb.AppendLine("> Use este conteúdo como REFERÊNCIA para criar a documentação.");
            sb.AppendLine("> NÃO copie grandes blocos - EXPLIQUE o funcionamento com suas palavras.");
            sb.AppendLine();

            var contentPreview = facts.RawContent.Length > maxChars
                ? facts.RawContent.Substring(0, maxChars) + "\n\n... [conteúdo truncado]"
                : facts.RawContent;

            var extension = facts.File.Extension.TrimStart('.');
            var language = extension switch
            {
                "cs" => "csharp",
                "js" => "javascript",
                "cshtml" => "html",
                "css" => "css",
                _ => extension
            };

            sb.AppendLine($"```{language}");
            sb.AppendLine(contentPreview);
            sb.AppendLine("```");
            sb.AppendLine();
        }

        protected virtual void AppendOutputInstructions(StringBuilder sb)
        {
            sb.AppendLine("---");
            sb.AppendLine("## INSTRUÇÕES DE SAÍDA");
            sb.AppendLine();
            sb.AppendLine("1. Gere a documentação em Markdown seguindo TODAS as regras do system prompt");
            sb.AppendLine("2. Use ícones FontAwesome Duotone em TODOS os títulos");
            sb.AppendLine("3. Foque em EXPLICAR, não apenas listar código");
            sb.AppendLine("4. Ao final, inclua um bloco JSON com o LayoutPlan:");
            sb.AppendLine();
            sb.AppendLine("```json");
            sb.AppendLine("{");
            sb.AppendLine("  \"layoutPlan\": {");
            sb.AppendLine("    \"theme\": \"frotix\",");
            sb.AppendLine("    \"primaryColor\": \"#b66a3d\",");
            sb.AppendLine("    \"secondaryColor\": \"#722F37\",");
            sb.AppendLine("    \"pages\": [{");
            sb.AppendLine("      \"pageNumber\": 1,");
            sb.AppendLine("      \"title\": \"[Título do Documento]\",");
            sb.AppendLine("      \"hasHero\": true,");
            sb.AppendLine("      \"sections\": [");
            sb.AppendLine("        { \"id\": \"visao-geral\", \"title\": \"Visão Geral\", \"icon\": \"fa-duotone fa-telescope\" },");
            sb.AppendLine("        { \"id\": \"arquitetura\", \"title\": \"Arquitetura\", \"icon\": \"fa-duotone fa-diagram-project\" }");
            sb.AppendLine("      ]");
            sb.AppendLine("    }],");
            sb.AppendLine("    \"iconMap\": {");
            sb.AppendLine("      \"Visão Geral\": \"fa-duotone fa-telescope\",");
            sb.AppendLine("      \"Arquitetura\": \"fa-duotone fa-diagram-project\"");
            sb.AppendLine("    }");
            sb.AppendLine("  }");
            sb.AppendLine("}");
            sb.AppendLine("```");
        }

        #endregion

        #region Parsing de Respostas

        /// <summary>
        /// Extrai o LayoutPlan do conteúdo markdown gerado
        /// </summary>
        protected LayoutPlan ParseLayoutPlan(string content)
        {
            try
            {
                var jsonStart = content.LastIndexOf("```json");
                var jsonEnd = content.LastIndexOf("```");

                if (jsonStart >= 0 && jsonEnd > jsonStart)
                {
                    var jsonContent = content.Substring(jsonStart + 7, jsonEnd - jsonStart - 7).Trim();
                    var doc = JsonDocument.Parse(jsonContent);

                    if (doc.RootElement.TryGetProperty("layoutPlan", out var planElement))
                    {
                        var options = new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        };
                        return JsonSerializer.Deserialize<LayoutPlan>(planElement.GetRawText(), options)
                            ?? CreateDefaultLayoutPlan();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning("Falha ao parsear LayoutPlan: {Message}", ex.Message);
            }

            return CreateDefaultLayoutPlan();
        }

        /// <summary>
        /// Extrai o mapa de ícones do conteúdo markdown gerado
        /// </summary>
        protected Dictionary<string, string> ExtractIconMap(string content)
        {
            var iconMap = GetDefaultIconMap();

            try
            {
                var jsonStart = content.LastIndexOf("```json");
                var jsonEnd = content.LastIndexOf("```");

                if (jsonStart >= 0 && jsonEnd > jsonStart)
                {
                    var jsonContent = content.Substring(jsonStart + 7, jsonEnd - jsonStart - 7).Trim();
                    var doc = JsonDocument.Parse(jsonContent);

                    if (doc.RootElement.TryGetProperty("layoutPlan", out var planElement))
                    {
                        if (planElement.TryGetProperty("iconMap", out var mapElement))
                        {
                            foreach (var prop in mapElement.EnumerateObject())
                            {
                                iconMap[prop.Name] = prop.Value.GetString() ?? "fa-duotone fa-file";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning("Falha ao extrair iconMap: {Message}", ex.Message);
            }

            return iconMap;
        }

        /// <summary>
        /// Cria um LayoutPlan padrão quando não é possível extrair da resposta
        /// </summary>
        protected LayoutPlan CreateDefaultLayoutPlan()
        {
            return new LayoutPlan
            {
                Theme = "frotix",
                PrimaryColor = "#b66a3d",
                SecondaryColor = "#722F37",
                Pages = new List<PagePlan>
                {
                    new PagePlan
                    {
                        PageNumber = 1,
                        Title = "Documentação",
                        HasHero = true,
                        HasToc = true,
                        Sections = new List<SectionPlan>
                        {
                            new SectionPlan { Id = "visao-geral", Title = "Visão Geral", Icon = "fa-duotone fa-telescope", Type = "card" },
                            new SectionPlan { Id = "arquitetura", Title = "Arquitetura", Icon = "fa-duotone fa-diagram-project", Type = "card" },
                            new SectionPlan { Id = "funcionalidades", Title = "Funcionalidades", Icon = "fa-duotone fa-brackets-curly", Type = "card" },
                            new SectionPlan { Id = "interconexoes", Title = "Interconexões", Icon = "fa-duotone fa-link-horizontal", Type = "card" },
                            new SectionPlan { Id = "exemplos", Title = "Exemplos", Icon = "fa-duotone fa-lightbulb", Type = "card" },
                            new SectionPlan { Id = "troubleshooting", Title = "Troubleshooting", Icon = "fa-duotone fa-toolbox", Type = "card" }
                        }
                    }
                },
                IconMap = GetDefaultIconMap()
            };
        }

        #endregion
    }
}
