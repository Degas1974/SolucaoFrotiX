/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: DocRenderService.cs                                                                     ║
   ║ 📂 CAMINHO: /Services/DocGenerator                                                                  ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO: Renderização de documentação em Markdown e HTML A4. Gera arquivos .md e páginas HTML.  ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 ÍNDICE: RenderAsync(), RenderMarkdown(), RenderHtmlA4(), SplitForA4Pages(), GenerateIndex()      ║
   ║ 🔗 DEPS: IDocRenderService, DocGeneratorSettings | 📅 13/01/2026 | 👤 Copilot | 📝 v2.0             ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝
*/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using FrotiX.Services.DocGenerator.Interfaces;
using FrotiX.Services.DocGenerator.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FrotiX.Services.DocGenerator
{
    /// <summary>
    /// Serviço de renderização de documentação (MD e HTML A4)
    /// </summary>
    public class DocRenderService : IDocRenderService
    {
        private readonly IWebHostEnvironment _env;
        private readonly DocGeneratorSettings _settings;
        private readonly ILogger<DocRenderService> _logger;

        // Peso estimado por tipo de conteúdo (para split A4)
        private const int WEIGHT_HEADING = 20;
        private const int WEIGHT_PARAGRAPH = 10;
        private const int WEIGHT_CODE_LINE = 8;
        private const int WEIGHT_TABLE_ROW = 12;
        private const int WEIGHT_LIST_ITEM = 6;
        private const int MAX_PAGE_WEIGHT = 800; // Peso máximo por página A4

        public DocRenderService(
            IWebHostEnvironment env,
            IOptions<DocGeneratorSettings> settings,
            ILogger<DocRenderService> logger)
        {
            try
            {
                _env = env;
                _settings = settings.Value;
                _logger = logger;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("DocRenderService.cs", ".ctor", error);
            }
        }

        public async Task<RenderResult> RenderAsync(
            DocumentFacts facts,
            AiComposeResult composeResult,
            DocGenerationOptions options,
            CancellationToken ct = default)
        {
            var result = new RenderResult();
            var sw = Stopwatch.StartNew();

            try
            {
                _logger.LogInformation(
                    "DocRenderService.RenderAsync: Iniciando para {File} - Markdown.Length={MdLen}",
                    facts.File.FileName,
                    composeResult.Markdown?.Length ?? 0);

                if (string.IsNullOrWhiteSpace(composeResult.Markdown))
                {
                    _logger.LogWarning("DocRenderService.RenderAsync: ALERTA! Markdown recebido está vazio para {File}", facts.File.FileName);
                }

                var outputDir = GetOutputDirectory(facts.File);
                Directory.CreateDirectory(outputDir);

                // Gerar Markdown
                if (options.GenerateMarkdown)
                {
                    result.MarkdownPath = await RenderMarkdownAsync(facts, composeResult.Markdown, ct);
                    _logger.LogInformation("Markdown gerado: {Path} (tamanho: {Size} bytes)", result.MarkdownPath, composeResult.Markdown?.Length ?? 0);
                }

                // Gerar HTML A4
                if (options.GenerateHtml)
                {
                    var pages = await RenderHtmlA4Async(facts, composeResult.Markdown, composeResult.Plan, ct);
                    result.HtmlPaths = pages.Select(p => p.FileName).ToList();
                    result.TotalPages = pages.Count;
                    _logger.LogInformation("HTML A4 gerado: {Count} páginas", result.TotalPages);

                    // ⭐ GERAR PDF AUTOMATICAMENTE (Se configurado ou se HTML foi gerado)
                    try
                    {
                        foreach (var htmlPath in result.HtmlPaths)
                        {
                            await ConvertHtmlToPdfAsync(htmlPath, ct);
                        }
                    }
                    catch (Exception pdfError)
                    {
                        _logger.LogError(pdfError, "Erro ao gerar PDF automaticamente para {File}", facts.File.FileName);
                    }
                }

                result.Success = true;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("DocRenderService.cs", "RenderAsync", error);
                result.Success = false;
                result.ErrorMessage = error.Message;
            }

            sw.Stop();
            result.ProcessingTime = sw.Elapsed;
            return result;
        }

        public async Task<string> RenderMarkdownAsync(
            DocumentFacts facts,
            string content,
            CancellationToken ct = default)
        {
            try
            {
                var outputPath = GetMarkdownPath(facts.File);
                var outputDir = Path.GetDirectoryName(outputPath);

                if (!string.IsNullOrEmpty(outputDir))
                    Directory.CreateDirectory(outputDir);

                await File.WriteAllTextAsync(outputPath, content, ct);
                return outputPath;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("DocRenderService.cs", "RenderMarkdownAsync", error);
                throw;
            }
        }

        public async Task<List<RenderedPage>> RenderHtmlA4Async(
            DocumentFacts facts,
            string markdownContent,
            LayoutPlan plan,
            CancellationToken ct = default)
        {
            var pages = new List<RenderedPage>();

            try
            {
                // Dividir conteúdo em páginas
                var pagePlans = SplitIntoA4Pages(markdownContent, plan);

                // Renderizar cada página
                for (int i = 0; i < pagePlans.Count; i++)
                {
                    ct.ThrowIfCancellationRequested();

                    var pagePlan = pagePlans[i];
                    var pageContent = ExtractPageContent(markdownContent, pagePlan, pagePlans);

                    var html = RenderHtmlPage(
                        facts,
                        pageContent,
                        pagePlan,
                        plan,
                        i + 1,
                        pagePlans.Count);

                    var fileName = GetHtmlFileName(facts.File, i + 1);
                    var filePath = Path.Combine(GetOutputDirectory(facts.File), fileName);

                    await File.WriteAllTextAsync(filePath, html, ct);

                    pages.Add(new RenderedPage
                    {
                        PageNumber = i + 1,
                        HtmlContent = html,
                        FileName = filePath,
                        HasPrevious = i > 0,
                        HasNext = i < pagePlans.Count - 1
                    });
                }
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("DocRenderService.cs", "RenderHtmlA4Async", error);
            }

            return pages;
        }

        public List<PagePlan> SplitIntoA4Pages(string content, LayoutPlan plan)
        {
            var pages = new List<PagePlan>();
            var sections = ParseSections(content);

            var currentPage = new PagePlan
            {
                PageNumber = 1,
                HasHero = true,
                HasToc = true,
                Sections = new List<SectionPlan>()
            };

            var currentWeight = 100; // Hero + TOC base weight

            foreach (var section in sections)
            {
                var sectionWeight = CalculateSectionWeight(section.Content);

                // Se a seção não cabe na página atual e não é a primeira seção
                if (currentWeight + sectionWeight > MAX_PAGE_WEIGHT && currentPage.Sections.Any())
                {
                    pages.Add(currentPage);
                    currentPage = new PagePlan
                    {
                        PageNumber = pages.Count + 1,
                        HasHero = false,
                        HasToc = false,
                        Sections = new List<SectionPlan>()
                    };
                    currentWeight = 0;
                }

                // Se a seção é muito grande, dividir
                if (sectionWeight > MAX_PAGE_WEIGHT)
                {
                    var subSections = SplitLargeSection(section);
                    foreach (var subSection in subSections)
                    {
                        var subWeight = CalculateSectionWeight(subSection.Content);

                        if (currentWeight + subWeight > MAX_PAGE_WEIGHT && currentPage.Sections.Any())
                        {
                            pages.Add(currentPage);
                            currentPage = new PagePlan
                            {
                                PageNumber = pages.Count + 1,
                                HasHero = false,
                                HasToc = false,
                                Sections = new List<SectionPlan>()
                            };
                            currentWeight = 0;
                        }

                        currentPage.Sections.Add(subSection);
                        currentWeight += subWeight;
                    }
                }
                else
                {
                    currentPage.Sections.Add(section);
                    currentWeight += sectionWeight;
                }
            }

            // Adicionar última página
            if (currentPage.Sections.Any() || !pages.Any())
            {
                pages.Add(currentPage);
            }

            // Definir títulos das páginas
            for (int i = 0; i < pages.Count; i++)
            {
                pages[i].Title = pages[i].Sections.FirstOrDefault()?.Title
                    ?? $"Página {i + 1}";
            }

            return pages;
        }

        #region Helper Methods

        private async Task ConvertHtmlToPdfAsync(string htmlPath, CancellationToken ct)
        {
            try
            {
                var scriptPath = Path.Combine(_env.ContentRootPath, "ExportarDocumentacaoPDF.ps1");
                if (!File.Exists(scriptPath))
                {
                    _logger.LogWarning("Script de conversão PDF não encontrado: {Path}", scriptPath);
                    return;
                }

                _logger.LogInformation("Iniciando conversão HTML para PDF: {File}", Path.GetFileName(htmlPath));

                var startInfo = new ProcessStartInfo
                {
                    FileName = "powershell.exe",
                    Arguments = $"-NoProfile -ExecutionPolicy Bypass -File \"{scriptPath}\" -FilePath \"{htmlPath}\"",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                };

                using var process = new Process { StartInfo = startInfo };
                process.Start();

                var output = await process.StandardOutput.ReadToEndAsync();
                var error = await process.StandardError.ReadToEndAsync();

                await process.WaitForExitAsync(ct);

                if (process.ExitCode != 0)
                {
                    _logger.LogError("Erro no PowerShell ao converter PDF: {Error}", error);
                }
                else
                {
                    _logger.LogInformation("PDF gerado com sucesso para {File}", Path.GetFileName(htmlPath));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Falha na execução do processo de conversão para PDF");
            }
        }

        private List<SectionPlan> ParseSections(string content)
        {
            var sections = new List<SectionPlan>();
            var lines = content.Split('\n');
            var currentSection = new SectionPlan();
            var sectionContent = new StringBuilder();
            var insideCodeBlock = false;

            foreach (var line in lines)
            {
                // Detectar blocos de código
                if (line.TrimStart().StartsWith("```"))
                {
                    insideCodeBlock = !insideCodeBlock;
                }

                // Detectar headings (## ou ###)
                if (!insideCodeBlock && Regex.IsMatch(line, @"^#{1,3}\s+"))
                {
                    // Salvar seção anterior
                    if (!string.IsNullOrWhiteSpace(sectionContent.ToString()))
                    {
                        currentSection.Content = sectionContent.ToString();
                        sections.Add(currentSection);
                    }

                    // Iniciar nova seção
                    var title = Regex.Replace(line, @"^#+\s*", "").Trim();
                    currentSection = new SectionPlan
                    {
                        Id = Slugify(title),
                        Title = title,
                        Type = line.StartsWith("##") ? "card" : "subcard",
                        KeepTogether = true
                    };
                    sectionContent = new StringBuilder();
                }

                sectionContent.AppendLine(line);
            }

            // Adicionar última seção
            if (!string.IsNullOrWhiteSpace(sectionContent.ToString()))
            {
                currentSection.Content = sectionContent.ToString();
                sections.Add(currentSection);
            }

            return sections;
        }

        private int CalculateSectionWeight(string content)
        {
            if (string.IsNullOrEmpty(content))
                return 0;

            var weight = 0;
            var lines = content.Split('\n');

            foreach (var line in lines)
            {
                var trimmed = line.Trim();

                if (trimmed.StartsWith("#"))
                    weight += WEIGHT_HEADING;
                else if (trimmed.StartsWith("```"))
                    weight += WEIGHT_CODE_LINE;
                else if (trimmed.StartsWith("|"))
                    weight += WEIGHT_TABLE_ROW;
                else if (trimmed.StartsWith("-") || trimmed.StartsWith("*") || Regex.IsMatch(trimmed, @"^\d+\."))
                    weight += WEIGHT_LIST_ITEM;
                else if (!string.IsNullOrWhiteSpace(trimmed))
                    weight += WEIGHT_PARAGRAPH;
            }

            return weight;
        }

        private List<SectionPlan> SplitLargeSection(SectionPlan section)
        {
            var subSections = new List<SectionPlan>();
            var lines = section.Content?.Split('\n') ?? Array.Empty<string>();
            var currentContent = new StringBuilder();
            var currentWeight = 0;
            var partNumber = 1;

            foreach (var line in lines)
            {
                var lineWeight = CalculateSectionWeight(line);

                if (currentWeight + lineWeight > MAX_PAGE_WEIGHT && currentContent.Length > 0)
                {
                    subSections.Add(new SectionPlan
                    {
                        Id = $"{section.Id}-part{partNumber}",
                        Title = $"{section.Title} (cont.)",
                        Type = section.Type,
                        Content = currentContent.ToString(),
                        KeepTogether = false
                    });

                    currentContent = new StringBuilder();
                    currentWeight = 0;
                    partNumber++;
                }

                currentContent.AppendLine(line);
                currentWeight += lineWeight;
            }

            if (currentContent.Length > 0)
            {
                subSections.Add(new SectionPlan
                {
                    Id = $"{section.Id}-part{partNumber}",
                    Title = partNumber > 1 ? $"{section.Title} (cont.)" : section.Title,
                    Type = section.Type,
                    Content = currentContent.ToString(),
                    KeepTogether = false
                });
            }

            return subSections;
        }

        private string ExtractPageContent(string fullContent, PagePlan page, List<PagePlan> allPages)
        {
            var sb = new StringBuilder();

            foreach (var section in page.Sections)
            {
                if (!string.IsNullOrEmpty(section.Content))
                {
                    sb.AppendLine(section.Content);
                }
            }

            return sb.ToString();
        }

        private string RenderHtmlPage(
            DocumentFacts facts,
            string pageContent,
            PagePlan pagePlan,
            LayoutPlan layout,
            int pageNumber,
            int totalPages)
        {
            var sb = new StringBuilder();

            sb.AppendLine("<!doctype html>");
            sb.AppendLine("<html lang=\"pt-BR\">");
            sb.AppendLine("<head>");
            sb.AppendLine("  <meta charset=\"utf-8\" />");
            sb.AppendLine($"  <title>{facts.File.FileName} | FrotiX</title>");
            sb.AppendLine(GetCssStyles(layout));
            sb.AppendLine("</head>");
            sb.AppendLine("<body>");

            // FontAwesome Kit
            sb.AppendLine("  <script src=\"https://kit.fontawesome.com/afeb78ad1f.js\" crossorigin=\"anonymous\"></script>");

            sb.AppendLine("  <div class=\"page\">");

            // Hero (primeira página)
            if (pagePlan.HasHero)
            {
                sb.AppendLine(RenderHero(facts, layout));
            }

            // Conteúdo
            sb.AppendLine("    <main class=\"grid\">");

            // Converter Markdown para HTML e renderizar cards
            var htmlContent = ConvertMarkdownToHtmlCards(pageContent, layout);
            sb.AppendLine(htmlContent);

            sb.AppendLine("    </main>");

            // Navegação entre páginas
            if (totalPages > 1)
            {
                sb.AppendLine(RenderNavigation(facts.File, pageNumber, totalPages));
            }

            // Footer
            sb.AppendLine($"    <footer class=\"page-footer\">Página {pageNumber} de {totalPages} | Gerado em {DateTime.Now:dd/MM/yyyy HH:mm}</footer>");

            sb.AppendLine("  </div>");
            sb.AppendLine("</body>");
            sb.AppendLine("</html>");

            return sb.ToString();
        }

        private string GetCssStyles(LayoutPlan layout)
        {
            return $@"  <style>
    :root {{
      --vinho:#722F37; --vinho-light:#8B3A44;
      --azul:#325d88; --azul-light:#3d6f9e;
      --terracota:#A97B6E; --terracota-light:#C08B7E;
      --verde:#557570; --verde-light:#6A8A85;
      --cinza:#f5f7fb; --card:#fff; --texto:#1f1f1f;
      --shadow:0 20px 45px -18px rgba(0,0,0,.35);
      --radius:14px;
      --header-bg:{layout.PrimaryColor};
      --code-bg:#33465c;
    }}
    @page {{ size: A4; margin: 16mm; }}
    * {{ box-sizing: border-box; }}
    body {{
      margin:0;
      font-family:""Segoe UI"",""Inter"",system-ui,-apple-system,sans-serif;
      background: radial-gradient(circle at 12% 20%, rgba(50,93,136,.08), transparent 26%),
                  radial-gradient(circle at 90% 10%, rgba(114,47,55,.10), transparent 30%),
                  var(--cinza);
      color:var(--texto);
      padding:22px;
      display:flex;
      justify-content:center;
    }}
    .page {{ width:210mm; max-width:100%; }}
    .hero {{
      background:var(--header-bg);
      color:#fff;
      padding:22px 24px;
      border-radius:var(--radius);
      box-shadow:0 0 0 1px #000, 0 0 0 4px #fff, var(--shadow);
      display:flex;
      align-items:center;
      gap:16px;
    }}
    .hero i {{ font-size:50px; }}
    h1 {{ margin:0; font-size:26px; letter-spacing:0.2px; }}
    .subtitle {{ margin:6px 0 0; font-size:13px; opacity:0.92; }}
    .grid {{ display:grid; grid-template-columns:repeat(auto-fit,minmax(300px,1fr)); gap:16px; margin-top:18px; }}
    .card {{
      background:var(--card);
      border-radius:var(--radius);
      box-shadow:var(--shadow);
      padding:16px 18px;
      border:1px solid rgba(0,0,0,0.05);
    }}
    .card-full {{ grid-column: 1 / -1; }}
    .section-title {{
      display:flex;
      align-items:center;
      gap:10px;
      margin:0 0 10px;
      font-size:15px;
      font-weight:800;
      color:var(--vinho);
    }}
    .section-title i {{
      color: var(--header-bg);
      --fa-primary-color: var(--header-bg);
      --fa-secondary-color: #6c757d;
    }}
    ul {{ margin:6px 0 0 18px; padding:0; }}
    li {{ margin-bottom:6px; }}
    code {{
      background:var(--code-bg);
      color:#e9edf5;
      padding:10px 12px;
      border-radius:10px;
      display:block;
      white-space:pre-wrap;
      font-size:12px;
      line-height:1.45;
      box-shadow: inset 0 1px 0 rgba(255,255,255,0.03);
      overflow-x: auto;
    }}
    code.inline {{
      display: inline;
      padding: 2px 6px;
      border-radius: 4px;
      font-size: 0.9em;
    }}
    table {{
      width:100%;
      border-collapse:collapse;
      font-size:12px;
      margin:10px 0;
    }}
    th, td {{
      border:1px solid #ddd;
      padding:8px;
      text-align:left;
    }}
    th {{ background:var(--header-bg); color:#fff; }}
    .nav-bar {{
      display:flex;
      justify-content:space-between;
      align-items:center;
      margin-top:20px;
      padding:10px;
      background:#fff;
      border-radius:var(--radius);
      box-shadow:var(--shadow);
    }}
    .nav-btn {{
      display:inline-flex;
      align-items:center;
      gap:6px;
      padding:8px 16px;
      background:var(--header-bg);
      color:#fff;
      text-decoration:none;
      border-radius:6px;
      font-size:13px;
    }}
    .nav-btn:hover {{ background:var(--vinho); }}
    .nav-btn.disabled {{ background:#ccc; pointer-events:none; }}
    .page-footer {{
      text-align:center;
      margin-top:20px;
      padding:10px;
      font-size:11px;
      color:#666;
    }}
    .quote {{
      border-left: 4px solid var(--vinho);
      padding: 10px 15px;
      margin: 10px 0;
      background: rgba(114, 47, 55, 0.05);
      font-style: italic;
      border-radius: 0 8px 8px 0;
    }}
    hr {{ border: 0; border-top: 1px solid #ddd; margin: 15px 0; }}
    @media print {{
      .nav-bar {{ display:none; }}
      body {{ padding:0; }}
      .page {{ width:100%; }}
    }}
  </style>";
        }

        private string RenderHero(DocumentFacts facts, LayoutPlan layout)
        {
            var icon = GetCategoryIcon(facts.File.Category);

            return $@"    <header class=""hero"">
      <i class=""{icon}""></i>
      <div>
        <h1>{facts.File.FileName}</h1>
        <p class=""subtitle"">{facts.File.Category} | {facts.TotalLines} linhas | Última atualização: {DateTime.Now:dd/MM/yyyy}</p>
      </div>
    </header>";
        }

        private string ConvertMarkdownToHtmlCards(string markdown, LayoutPlan layout)
        {
            var sb = new StringBuilder();
            var lines = markdown.Split('\n');
            var inCodeBlock = false;
            var inCard = false;
            var inList = false;
            var inTable = false;
            var codeContent = new StringBuilder();
            var cardContent = new StringBuilder();

            foreach (var line in lines)
            {
                var trimmed = line.Trim();

                // Pular linhas vazias se não estivermos em card nem em código
                if (string.IsNullOrWhiteSpace(trimmed) && !inCodeBlock && !inCard)
                    continue;

                // Forçar abertura de card se tivermos conteúdo visível mas nenhum card aberto
                if (!inCard && !inCodeBlock && !string.IsNullOrWhiteSpace(trimmed) && !trimmed.StartsWith("#"))
                {
                    cardContent.AppendLine($@"<section class=""card card-full"">");
                    cardContent.AppendLine($@"  <div class=""section-title""><i class=""fa-duotone fa-file-lines""></i>Introdução / Continuação</div>");
                    inCard = true;
                }

                // Fechar lista se a linha não for item de lista
                if (inList && !trimmed.StartsWith("- ") && !trimmed.StartsWith("* ") && !string.IsNullOrWhiteSpace(trimmed))
                {
                    cardContent.AppendLine("</ul>");
                    inList = false;
                }

                // Fechar tabela se a linha não for de tabela
                if (inTable && !trimmed.StartsWith("|") && !string.IsNullOrWhiteSpace(trimmed))
                {
                    cardContent.AppendLine("</tbody></table>");
                    inTable = false;
                }

                // Toggle code block
                if (trimmed.StartsWith("```"))
                {
                    if (inCodeBlock)
                    {
                        cardContent.AppendLine($@"<pre class=""code-block""><code>{System.Web.HttpUtility.HtmlEncode(codeContent.ToString().Trim())}</code></pre>");
                        codeContent.Clear();
                    }
                    else if (inCard)
                    {
                        // Se não tem card aberto, o "Forçar abertura" acima já cuidou disso
                    }
                    inCodeBlock = !inCodeBlock;
                    continue;
                }

                if (inCodeBlock)
                {
                    codeContent.AppendLine(line);
                    continue;
                }

                // Heading # ou ## - novo card
                if (trimmed.StartsWith("## ") || trimmed.StartsWith("# "))
                {
                    if (inCard)
                    {
                        if (inList) { cardContent.AppendLine("</ul>"); inList = false; }
                        if (inTable) { cardContent.AppendLine("</tbody></table>"); inTable = false; }
                        cardContent.AppendLine("</section>");
                        sb.AppendLine(cardContent.ToString());
                        cardContent.Clear();
                    }

                    var title = trimmed.StartsWith("## ") ? trimmed.Substring(3).Trim() : trimmed.Substring(2).Trim();
                    var icon = GetSectionIcon(title, layout.IconMap);

                    cardContent.AppendLine($@"<section class=""card card-full"">");
                    cardContent.AppendLine($@"  <div class=""section-title""><i class=""{icon}""></i>{title}</div>");
                    inCard = true;
                    continue;
                }

                // Citações (Quotes)
                if (trimmed.StartsWith(">"))
                {
                    var quote = ConvertInlineMarkdown(trimmed.Substring(1).Trim());
                    cardContent.AppendLine($@"<div class=""quote"">{quote}</div>");
                    continue;
                }

                // Heading ### - subcabeçalho
                if (trimmed.StartsWith("### "))
                {
                    var title = trimmed.Substring(4).Trim();
                    cardContent.AppendLine($"<h4 style=\"margin-top:15px;color:var(--azul);\">{title}</h4>");
                    continue;
                }

                // Listas
                if (trimmed.StartsWith("- ") || trimmed.StartsWith("* "))
                {
                    if (!inList)
                    {
                        cardContent.AppendLine("<ul class=\"ftx-list\">");
                        inList = true;
                    }
                    var item = ConvertInlineMarkdown(trimmed.Substring(2));
                    cardContent.AppendLine($"<li>{item}</li>");
                    continue;
                }

                // Tabelas
                if (trimmed.StartsWith("|"))
                {
                    if (inList) { cardContent.AppendLine("</ul>"); inList = false; }
                    if (!inTable)
                    {
                        cardContent.AppendLine("<table class=\"ftx-table\"><tbody>");
                        inTable = true;
                    }
                    cardContent.AppendLine(ConvertTableLine(trimmed));
                    continue;
                }

                // Divider
                if (trimmed.StartsWith("---"))
                {
                    if (inList) { cardContent.AppendLine("</ul>"); inList = false; }
                    if (inTable) { cardContent.AppendLine("</tbody></table>"); inTable = false; }
                    cardContent.AppendLine("<hr/>");
                    continue;
                }

                // Parágrafos
                if (!string.IsNullOrWhiteSpace(trimmed) && !trimmed.StartsWith("#"))
                {
                    if (inList) { cardContent.AppendLine("</ul>"); inList = false; }
                    if (inTable) { cardContent.AppendLine("</tbody></table>"); inTable = false; }
                    
                    var para = ConvertInlineMarkdown(trimmed);
                    cardContent.AppendLine($"<p>{para}</p>");
                }
            }

            // Fechar tags pendentes
            if (inList) cardContent.AppendLine("</ul>");
            if (inTable) cardContent.AppendLine("</tbody></table>");

            // Fechar último card
            if (inCard)
            {
                cardContent.AppendLine("</section>");
                sb.AppendLine(cardContent.ToString());
            }

            return sb.ToString();
        }
        private string ConvertInlineMarkdown(string text)
        {
            // Bold: **text**
            text = Regex.Replace(text, @"\*\*(.+?)\*\*", "<strong>$1</strong>");

            // Italic: *text* ou _text_
            text = Regex.Replace(text, @"\*(.+?)\*", "<em>$1</em>");
            text = Regex.Replace(text, @"_(.+?)_", "<em>$1</em>");

            // Inline code: `text`
            text = Regex.Replace(text, @"`([^`]+)`", "<code class=\"inline\">$1</code>");

            // Links: [text](url)
            text = Regex.Replace(text, @"\[([^\]]+)\]\(([^\)]+)\)", "<a href=\"$2\">$1</a>");

            // Checkmarks
            text = text.Replace("✅", "<span style=\"color:green\">✅</span>");
            text = text.Replace("❌", "<span style=\"color:red\">❌</span>");

            return text;
        }

        private string ConvertTableLine(string line)
        {
            if (line.Contains("---"))
                return ""; // Separador

            var cells = line.Split('|')
                .Where(c => !string.IsNullOrWhiteSpace(c))
                .Select(c => c.Trim())
                .ToList();

            if (!cells.Any())
                return "";

            var isHeader = cells.All(c => c.StartsWith("**") || Regex.IsMatch(c, @"^[A-Z]"));
            var tag = isHeader ? "th" : "td";

            var sb = new StringBuilder("<tr>");
            foreach (var cell in cells)
            {
                var content = ConvertInlineMarkdown(cell);
                sb.Append($"<{tag}>{content}</{tag}>");
            }
            sb.Append("</tr>");

            return sb.ToString();
        }

        private string RenderNavigation(DiscoveredFile file, int currentPage, int totalPages)
        {
            var prevFile = currentPage > 1 ? GetHtmlFileName(file, currentPage - 1) : "";
            var nextFile = currentPage < totalPages ? GetHtmlFileName(file, currentPage + 1) : "";

            return $@"    <nav class=""nav-bar"">
      <a href=""{prevFile}"" class=""nav-btn {(currentPage == 1 ? "disabled" : "")}"">
        <i class=""fa-duotone fa-chevron-left""></i> Anterior
      </a>
      <span>Página {currentPage} de {totalPages}</span>
      <a href=""{nextFile}"" class=""nav-btn {(currentPage == totalPages ? "disabled" : "")}"">
        Próxima <i class=""fa-duotone fa-chevron-right""></i>
      </a>
    </nav>";
        }

        private string GetCategoryIcon(FileCategory category)
        {
            return category switch
            {
                FileCategory.Controller => "fa-duotone fa-gears",
                FileCategory.RazorPage => "fa-duotone fa-file-code",
                FileCategory.RazorPageModel => "fa-duotone fa-file-lines",
                FileCategory.Service => "fa-duotone fa-cog",
                FileCategory.Repository => "fa-duotone fa-database",
                FileCategory.Model => "fa-duotone fa-cube",
                FileCategory.ViewModel => "fa-duotone fa-cubes",
                FileCategory.Helper => "fa-duotone fa-toolbox",
                FileCategory.Middleware => "fa-duotone fa-layer-group",
                FileCategory.Hub => "fa-duotone fa-broadcast-tower",
                FileCategory.JavaScript => "fa-duotone fa-js",
                FileCategory.Css => "fa-duotone fa-css3",
                FileCategory.Data => "fa-duotone fa-database",
                _ => "fa-duotone fa-file"
            };
        }

        private string GetSectionIcon(string title, Dictionary<string, string> iconMap)
        {
            // Procurar no mapa
            foreach (var kvp in iconMap)
            {
                if (title.Contains(kvp.Key, StringComparison.OrdinalIgnoreCase))
                    return kvp.Value;
            }

            // Ícones padrão por título
            if (title.Contains("Visão", StringComparison.OrdinalIgnoreCase))
                return "fa-duotone fa-eye";
            if (title.Contains("Arquitetura", StringComparison.OrdinalIgnoreCase))
                return "fa-duotone fa-sitemap";
            if (title.Contains("Método", StringComparison.OrdinalIgnoreCase))
                return "fa-duotone fa-code";
            if (title.Contains("Endpoint", StringComparison.OrdinalIgnoreCase))
                return "fa-duotone fa-route";
            if (title.Contains("Dependência", StringComparison.OrdinalIgnoreCase))
                return "fa-duotone fa-plug";
            if (title.Contains("Propriedade", StringComparison.OrdinalIgnoreCase))
                return "fa-duotone fa-list";
            if (title.Contains("Validaç", StringComparison.OrdinalIgnoreCase))
                return "fa-duotone fa-check-circle";
            if (title.Contains("Troubleshooting", StringComparison.OrdinalIgnoreCase))
                return "fa-duotone fa-wrench";
            if (title.Contains("Log", StringComparison.OrdinalIgnoreCase))
                return "fa-duotone fa-list-check";
            if (title.Contains("Interconex", StringComparison.OrdinalIgnoreCase))
                return "fa-duotone fa-link";

            return "fa-duotone fa-info-circle";
        }

        private string GetOutputDirectory(DiscoveredFile file)
        {
            var docRoot = Path.Combine(_env.ContentRootPath, _settings.OutputPath);
            var relativeDir = Path.GetDirectoryName(file.RelativePath) ?? "";
            return Path.Combine(docRoot, relativeDir);
        }

        private string GetMarkdownPath(DiscoveredFile file)
        {
            var docRoot = Path.Combine(_env.ContentRootPath, _settings.OutputPath);
            var relativePath = Path.ChangeExtension(file.RelativePath, ".md");
            return Path.Combine(docRoot, relativePath);
        }

        private string GetHtmlFileName(DiscoveredFile file, int pageNumber)
        {
            var baseName = Path.GetFileNameWithoutExtension(file.FileName);
            return $"({GetCategoryFolder(file.Category)}) {baseName}A4{pageNumber:D2}.html";
        }

        private string GetCategoryFolder(FileCategory category)
        {
            return category switch
            {
                FileCategory.Controller => "Controllers",
                FileCategory.RazorPage => "Pages",
                FileCategory.RazorPageModel => "Pages",
                FileCategory.Service => "Services",
                FileCategory.Repository => "Repository",
                FileCategory.Model => "Models",
                FileCategory.ViewModel => "ViewModels",
                FileCategory.Helper => "Helpers",
                FileCategory.Middleware => "Middlewares",
                FileCategory.Hub => "Hubs",
                FileCategory.JavaScript => "JavaScript",
                FileCategory.Css => "CSS",
                FileCategory.Data => "Data",
                _ => "Other"
            };
        }

        private string Slugify(string text)
        {
            return Regex.Replace(text.ToLowerInvariant(), @"[^a-z0-9]+", "-").Trim('-');
        }

        #endregion
    }
}
