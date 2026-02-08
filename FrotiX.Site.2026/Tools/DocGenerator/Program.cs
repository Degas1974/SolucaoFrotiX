/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: Program.cs                                                                              ║
   ║ 📂 CAMINHO: /Tools/DocGenerator                                                                     ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO: CLI para geração automática de documentação .md a partir de Pages/*.cshtml.cs          ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 ÍNDICE: Main (CLI args), --root, --overwrite, --verbose, --minLines, --maxLines                  ║
   ║ 🔗 DEPS: System.CommandLine | 📅 29/01/2026 | 👤 Copilot | 📝 v2.0                                  ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝
*/

using System.CommandLine;
using System.Text;
using System.Text.RegularExpressions;

internal static class Program
{
    private static readonly UTF8Encoding Utf8NoBom = new(encoderShouldEmitUTF8Identifier: false);

    public static async Task<int> Main(string[] args)
    {
        var rootOption = new Option<DirectoryInfo>(
            name: "--root",
            description: "Diretório raiz do projeto (onde ficam Pages/ e Documentacao/).",
            getDefaultValue: () => new DirectoryInfo(Directory.GetCurrentDirectory()));

        var overwriteOption = new Option<bool>(
            name: "--overwrite",
            description: "Se true, sobrescreve arquivos de documentação já existentes (CUIDADO).",
            getDefaultValue: () => false);

        var verboseOption = new Option<bool>(
            name: "--verbose",
            description: "Log detalhado.",
            getDefaultValue: () => false);

        var minLinesOption = new Option<int>(
            name: "--minLines",
            description: "Mínimo de linhas por arquivo de documentação.",
            getDefaultValue: () => 350);

        var maxLinesOption = new Option<int>(
            name: "--maxLines",
            description: "Máximo de linhas por arquivo de documentação.",
            getDefaultValue: () => 650);

        var command = new RootCommand("Gerador de documentação FrotiX (Pages + catálogos de código).")
        {
            rootOption,
            overwriteOption,
            verboseOption,
            minLinesOption,
            maxLinesOption
        };

        command.SetHandler((DirectoryInfo root, bool overwrite, bool verbose, int minLines, int maxLines) =>
        {
            var projectRoot = root.FullName;
            var pagesDir = Path.Combine(projectRoot, "Pages");
            var docsDir = Path.Combine(projectRoot, "Documentacao");
            var codeDocsDir = Path.Combine(docsDir, "Codigo");

            if (!Directory.Exists(pagesDir))
                throw new DirectoryNotFoundException($"Diretório não encontrado: {pagesDir}");

            if (!Directory.Exists(docsDir))
                Directory.CreateDirectory(docsDir);

            if (!Directory.Exists(codeDocsDir))
                Directory.CreateDirectory(codeDocsDir);

            var existingDocs = new HashSet<string>(
                Directory.EnumerateFiles(docsDir, "*.md", SearchOption.AllDirectories)
                    .Select(Path.GetFullPath),
                StringComparer.OrdinalIgnoreCase);

            var result = new GenerationResult();

            var repoIndex = BuildRepositoryIndex(projectRoot, pagesDir, verbose);

            // Pages
            GenerateDocsForPages(pagesDir, docsDir, existingDocs, overwrite, verbose, result, repoIndex, minLines, maxLines);

            // 1 md por .cs (Controllers/Helpers/Middlewares/Services/Models)
            GenerateDocsForCSharpFolder(projectRoot, "Controllers", codeDocsDir, existingDocs, overwrite, verbose, result, repoIndex, minLines, maxLines);
            GenerateDocsForCSharpFolder(projectRoot, "Helpers", codeDocsDir, existingDocs, overwrite, verbose, result, repoIndex, minLines, maxLines);
            GenerateDocsForCSharpFolder(projectRoot, "Middlewares", codeDocsDir, existingDocs, overwrite, verbose, result, repoIndex, minLines, maxLines);
            GenerateDocsForCSharpFolder(projectRoot, "Services", codeDocsDir, existingDocs, overwrite, verbose, result, repoIndex, minLines, maxLines);
            GenerateDocsForCSharpFolder(projectRoot, "Models", codeDocsDir, existingDocs, overwrite, verbose, result, repoIndex, minLines, maxLines);

            // 1 md por .js (cadastros/dashboards/agendamento/alertasfrotix)
            GenerateDocsForJsFolder(projectRoot, Path.Combine("wwwroot", "js", "cadastros"), codeDocsDir, existingDocs, overwrite, verbose, result, repoIndex, minLines, maxLines);
            GenerateDocsForJsFolder(projectRoot, Path.Combine("wwwroot", "js", "dashboards"), codeDocsDir, existingDocs, overwrite, verbose, result, repoIndex, minLines, maxLines);
            GenerateDocsForJsFolder(projectRoot, Path.Combine("wwwroot", "js", "agendamento"), codeDocsDir, existingDocs, overwrite, verbose, result, repoIndex, minLines, maxLines);
            GenerateDocsForJsFolder(projectRoot, Path.Combine("wwwroot", "js", "alertasfrotix"), codeDocsDir, existingDocs, overwrite, verbose, result, repoIndex, minLines, maxLines);

            // Catálogos (mantém os já existentes)
            GenerateCatalogDocs(projectRoot, docsDir, existingDocs, overwrite, verbose, result);

            // Normalização final (350-650 linhas) em TODOS os .md (se overwrite=true)
            NormalizeAllMarkdownDocs(docsDir, overwrite, verbose, minLines, maxLines, result);

            Console.WriteLine("==== DocGenerator ====");
            Console.WriteLine($"Docs criados: {result.Created}");
            Console.WriteLine($"Docs atualizados: {result.Updated}");
            Console.WriteLine($"Docs ignorados (já existiam): {result.Skipped}");
            Console.WriteLine($"Falhas: {result.Failed}");
        }, rootOption, overwriteOption, verboseOption, minLinesOption, maxLinesOption);

        return await command.InvokeAsync(args);
    }

    private static void GenerateDocsForPages(
    string pagesDir,
    string docsDir,
    HashSet<string> existingDocs,
    bool overwrite,
    bool verbose,
    GenerationResult result,
    RepoIndex repoIndex,
    int minLines,
    int maxLines)
{
    var cshtmlFiles = Directory.EnumerateFiles(pagesDir, "*.cshtml", SearchOption.AllDirectories)
        .Where(p =>
        {
            var rel = Path.GetRelativePath(pagesDir, p);
            if (rel.Equals("_ViewImports.cshtml", StringComparison.OrdinalIgnoreCase)) return false;
            if (rel.Equals("_ViewStart.cshtml", StringComparison.OrdinalIgnoreCase)) return false;

            // Ignorar Shared e qualquer partial (prefixo "_")
            if (rel.StartsWith("Shared" + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase)) return false;
            if (Path.GetFileName(rel).StartsWith("_", StringComparison.OrdinalIgnoreCase)) return false;

            // Ignorar arquivos de backup
            if (rel.Contains(".bak", StringComparison.OrdinalIgnoreCase)) return false;

            return true;
        })
        .OrderBy(p => p, StringComparer.OrdinalIgnoreCase)
        .ToList();

    foreach (var cshtmlPath in cshtmlFiles)
    {
        try
        {
            var rel = Path.GetRelativePath(pagesDir, cshtmlPath);
            var parts = rel.Split(Path.DirectorySeparatorChar, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length < 2)
                continue; // não é subdiretório (ou estrutura inesperada)

            var modulo = parts[0];
            var pagina = Path.GetFileNameWithoutExtension(cshtmlPath);

            // Nome do arquivo de doc segue o padrão FrotiX observado
            var docFileName = $"Funcionalidade - {SanitizeForFileName(modulo)} - {SanitizeForFileName(pagina)}.md";
            var docPath = Path.Combine(docsDir, docFileName);

            var fullDocPath = Path.GetFullPath(docPath);
            var docExists = File.Exists(fullDocPath);
            if (docExists && !overwrite)
            {
                result.Skipped++;
                continue;
            }

            var cshtmlContent = SafeReadAllText(cshtmlPath);
            var codeBehindPath = cshtmlPath + ".cs"; // padrão Razor Pages: *.cshtml.cs

            var route = ExtractPageRoute(cshtmlContent);
            var model = ExtractRazorModel(cshtmlContent);

            var assets = ExtractAssets(cshtmlContent);
            var lastUpdate = DateTime.Now.ToString("dd/MM/yyyy");

            var title = $"Documentação: {modulo} - {pagina}";
            var md = BuildPageMarkdown(
                title: title,
                lastUpdate: lastUpdate,
                version: "1.0",
                modulo: modulo,
                pagina: pagina,
                route: route,
                cshtmlPath: NormalizeSlashes(Path.Combine("Pages", rel)),
                cshtmlCsPath: File.Exists(codeBehindPath) ? NormalizeSlashes(Path.Combine("Pages", rel + ".cs")) : null,
                modelDirective: model,
                assets: assets,
                repoIndex: repoIndex,
                currentPageRel: NormalizeSlashes(Path.Combine("Pages", rel)));

            md = EnsureLineCount(md, minLines, maxLines, $"Apêndice: Complementos automáticos ({modulo}/{pagina})");

            Directory.CreateDirectory(Path.GetDirectoryName(fullDocPath)!);
            File.WriteAllText(fullDocPath, md, Utf8NoBom);

            if (docExists)
                result.Updated++;
            else
            {
                result.Created++;
                existingDocs.Add(fullDocPath);
            }

            if (verbose)
                Console.WriteLine($"[OK] {docFileName}");
        }
        catch (Exception ex)
        {
            result.Failed++;
            Console.Error.WriteLine($"[ERRO] Falha ao documentar {cshtmlPath}: {ex.Message}");
        }
    }
}

    private static void GenerateCatalogDocs(
    string projectRoot,
    string docsDir,
    HashSet<string> existingDocs,
    bool overwrite,
    bool verbose,
    GenerationResult result)
{
    var targets = new (string Folder, string DocName, string Title)[]
    {
        ("Controllers", "Codigo - Controllers.md", "Catálogo de Código: Controllers"),
        ("Helpers", "Codigo - Helpers.md", "Catálogo de Código: Helpers"),
        ("Middlewares", "Codigo - Middlewares.md", "Catálogo de Código: Middlewares"),
        ("Services", "Codigo - Services.md", "Catálogo de Código: Services"),
        ("Models", "Codigo - Models.md", "Catálogo de Código: Models"),
    };

    foreach (var (folder, docName, title) in targets)
    {
        var folderPath = Path.Combine(projectRoot, folder);
        if (!Directory.Exists(folderPath))
            continue;

        var docPath = Path.Combine(docsDir, docName);
        var fullDocPath = Path.GetFullPath(docPath);
        var docExists = File.Exists(fullDocPath);

        if (docExists && !overwrite)
        {
            result.Skipped++;
            continue;
        }

        var csFiles = Directory.EnumerateFiles(folderPath, "*.cs", SearchOption.AllDirectories)
            .Where(p => !p.Contains(".bak", StringComparison.OrdinalIgnoreCase))
            .ToList();

        var lastUpdate = DateTime.Now.ToString("dd/MM/yyyy");
        var md = BuildCatalogMarkdown(title, lastUpdate, folder, csFiles, projectRoot);

        File.WriteAllText(fullDocPath, md, Utf8NoBom);

        if (docExists) result.Updated++; else { result.Created++; existingDocs.Add(fullDocPath); }
        if (verbose) Console.WriteLine($"[OK] {docName}");
    }
}

    private static string BuildPageMarkdown(
    string title,
    string lastUpdate,
    string version,
    string modulo,
    string pagina,
    string? route,
    string cshtmlPath,
    string? cshtmlCsPath,
    string? modelDirective,
    AssetInfo assets)
{
    var sb = new StringBuilder();

    sb.AppendLine($"# {title}");
    sb.AppendLine();
    sb.AppendLine($"> **Última Atualização**: {lastUpdate}");
    sb.AppendLine($"> **Versão Atual**: {version}");
    sb.AppendLine();
    sb.AppendLine("---");
    sb.AppendLine();
    sb.AppendLine("# PARTE 1: DOCUMENTAÇÃO DA FUNCIONALIDADE");
    sb.AppendLine();
    sb.AppendLine("## Índice");
    sb.AppendLine("1. [Visão Geral](#visão-geral)");
    sb.AppendLine("2. [Arquitetura](#arquitetura)");
    sb.AppendLine("3. [Frontend](#frontend)");
    sb.AppendLine("4. [Endpoints API](#endpoints-api)");
    sb.AppendLine("5. [Validações](#validações)");
    sb.AppendLine("6. [Troubleshooting](#troubleshooting)");
    sb.AppendLine();
    sb.AppendLine("---");
    sb.AppendLine();
    sb.AppendLine("## Visão Geral");
    sb.AppendLine();
    sb.AppendLine("> **TODO**: Descrever o objetivo da página e as principais ações do usuário.");
    sb.AppendLine();
    sb.AppendLine("### Características Principais");
    sb.AppendLine("- ✅ **TODO**");
    sb.AppendLine();
    sb.AppendLine("---");
    sb.AppendLine();
    sb.AppendLine("## Arquitetura");
    sb.AppendLine();
    sb.AppendLine("### Estrutura de Arquivos");
    sb.AppendLine();
    sb.AppendLine("```");
    sb.AppendLine("FrotiX.Site/");
    sb.AppendLine($"├── {cshtmlPath}");
    if (!string.IsNullOrWhiteSpace(cshtmlCsPath))
        sb.AppendLine($"├── {cshtmlCsPath}");
    sb.AppendLine("```");
    sb.AppendLine();
    sb.AppendLine("### Informações de Roteamento");
    sb.AppendLine();
    sb.AppendLine($"- **Módulo**: `{modulo}`");
    sb.AppendLine($"- **Página**: `{pagina}`");
    sb.AppendLine($"- **Rota (Razor Pages)**: {(string.IsNullOrWhiteSpace(route) ? "_não identificada no arquivo_" : $"`{route}`")}");
    if (!string.IsNullOrWhiteSpace(modelDirective))
        sb.AppendLine($"- **@model**: `{modelDirective}`");
    sb.AppendLine();
    sb.AppendLine("---");
    sb.AppendLine();
    sb.AppendLine("## Frontend");
    sb.AppendLine();
    sb.AppendLine("### Assets referenciados na página");
    sb.AppendLine();
    sb.AppendLine($"- **CSS** ({assets.Css.Count}):");
    foreach (var css in assets.Css)
        sb.AppendLine($"  - `{css}`");
    sb.AppendLine($"- **JS** ({assets.Js.Count}):");
    foreach (var js in assets.Js)
        sb.AppendLine($"  - `{js}`");
    if (assets.Notes.Count > 0)
    {
        sb.AppendLine();
        sb.AppendLine("### Observações detectadas");
        foreach (var n in assets.Notes)
            sb.AppendLine($"- {n}");
    }
    sb.AppendLine();
    sb.AppendLine("---");
    sb.AppendLine();
    sb.AppendLine("## Endpoints API");
    sb.AppendLine();
    sb.AppendLine("> **TODO**: Listar endpoints consumidos pela página e incluir trechos reais de código do Controller/Handler quando aplicável.");
    sb.AppendLine();
    sb.AppendLine("---");
    sb.AppendLine();
    sb.AppendLine("## Validações");
    sb.AppendLine();
    sb.AppendLine("> **TODO**: Listar validações do frontend e backend (com trechos reais do código).");
    sb.AppendLine();
    sb.AppendLine("---");
    sb.AppendLine();
    sb.AppendLine("## Troubleshooting");
    sb.AppendLine();
    sb.AppendLine("> **TODO**: Problemas comuns, sintomas, causa e solução.");
    sb.AppendLine();
    sb.AppendLine("---");
    sb.AppendLine();
    sb.AppendLine("# PARTE 2: LOG DE MODIFICAÇÕES/CORREÇÕES");
    sb.AppendLine();
    sb.AppendLine("> **FORMATO**: Entradas em ordem **decrescente** (mais recente primeiro)");
    sb.AppendLine();
    sb.AppendLine("---");
    sb.AppendLine();
    sb.AppendLine($"## [{DateTime.Now:dd/MM/yyyy HH:mm}] - Criação automática da documentação (stub)");
    sb.AppendLine();
    sb.AppendLine("**Descrição**:");
    sb.AppendLine("- Criado esqueleto de documentação automaticamente a partir da estrutura de arquivos e referências encontradas na página.");
    sb.AppendLine("- **TODO**: Completar PARTE 1 com detalhes e trechos de código reais.");
    sb.AppendLine();
    sb.AppendLine("**Status**: ✅ **Gerado (pendente detalhamento)**");

    return sb.ToString();
}

    private static string BuildCatalogMarkdown(
    string title,
    string lastUpdate,
    string folderName,
    List<string> csFiles,
    string projectRoot)
{
    var sb = new StringBuilder();
    sb.AppendLine($"# {title}");
    sb.AppendLine();
    sb.AppendLine($"> **Última Atualização**: {lastUpdate}");
    sb.AppendLine($"> **Versão Atual**: 0.1");
    sb.AppendLine();
    sb.AppendLine("---");
    sb.AppendLine();
    sb.AppendLine("## Visão Geral");
    sb.AppendLine();
    sb.AppendLine($"Este documento cataloga os arquivos dentro de `{folderName}/` e aponta classes/métodos principais quando possível.");
    sb.AppendLine();
    sb.AppendLine("## Arquivos");
    sb.AppendLine();
    sb.AppendLine($"Total: **{csFiles.Count}** arquivo(s).");
    sb.AppendLine();

    foreach (var file in csFiles.OrderBy(p => p, StringComparer.OrdinalIgnoreCase))
    {
        var rel = NormalizeSlashes(Path.GetRelativePath(projectRoot, file));
        var content = SafeReadAllText(file, maxChars: 150_000);

        var classes = ExtractClassNames(content).Take(5).ToList();
        var routes = ExtractAttributeValues(content, "Route").Take(5).ToList();
        var httpAttrs = ExtractHttpVerbAttributes(content).Take(8).ToList();

        sb.AppendLine($"### `{rel}`");
        sb.AppendLine();
        if (classes.Count > 0)
            sb.AppendLine($"- **Classes**: {string.Join(", ", classes.Select(c => $"`{c}`"))}");
        if (routes.Count > 0)
            sb.AppendLine($"- **[Route]**: {string.Join(", ", routes.Select(r => $"`{r}`"))}");
        if (httpAttrs.Count > 0)
            sb.AppendLine($"- **HTTP Attributes**: {string.Join(", ", httpAttrs.Select(a => $"`{a}`"))}");
        sb.AppendLine();
    }

    sb.AppendLine("---");
    sb.AppendLine();
    sb.AppendLine("# PARTE 2: LOG DE MODIFICAÇÕES/CORREÇÕES");
    sb.AppendLine();
    sb.AppendLine($"> **FORMATO**: Entradas em ordem **decrescente** (mais recente primeiro)");
    sb.AppendLine();
    sb.AppendLine("---");
    sb.AppendLine();
    sb.AppendLine($"## [{DateTime.Now:dd/MM/yyyy HH:mm}] - Criação automática do catálogo (stub)");
    sb.AppendLine();
    sb.AppendLine("**Descrição**:");
    sb.AppendLine("- Gerado automaticamente listando arquivos e metadados básicos (classes/rotas/atributos HTTP quando detectados).");
    sb.AppendLine("- **TODO**: Detalhar arquivos críticos conforme necessidade (com trechos reais de código).");
    sb.AppendLine();
    sb.AppendLine("**Status**: ✅ **Gerado (pendente detalhamento)**");

    return sb.ToString();
}

    private static string SafeReadAllText(string path, int maxChars = 500_000)
{
    // Protege contra arquivos muito grandes e encoding exótico.
    try
    {
        using var stream = File.OpenRead(path);
        using var reader = new StreamReader(stream, detectEncodingFromByteOrderMarks: true);
        var buffer = new char[8192];
        var sb = new StringBuilder();
        int read;
        while ((read = reader.Read(buffer, 0, buffer.Length)) > 0)
        {
            sb.Append(buffer, 0, read);
            if (sb.Length >= maxChars)
                break;
        }
        return sb.ToString();
    }
    catch
    {
        return string.Empty;
    }
}

    private static string? ExtractPageRoute(string cshtml)
{
    foreach (var line in cshtml.Split('\n'))
    {
        var trimmed = line.Trim();
        if (!trimmed.StartsWith("@page", StringComparison.OrdinalIgnoreCase)) continue;

        // @page  or @page "/rota"
        var match = Regex.Match(trimmed, @"^@page\s*(?:""(?<r>[^""]+)""|'(?<r>[^']+)')?", RegexOptions.IgnoreCase);
        if (match.Success && match.Groups["r"].Success)
            return match.Groups["r"].Value;
        return "/<convenção Razor Pages>";
    }
    return null;
}

    private static string? ExtractRazorModel(string cshtml)
{
    foreach (var line in cshtml.Split('\n'))
    {
        var trimmed = line.Trim();
        if (!trimmed.StartsWith("@model", StringComparison.OrdinalIgnoreCase)) continue;
        return trimmed.Replace("@model", "", StringComparison.OrdinalIgnoreCase).Trim();
    }
    return null;
}

    private static AssetInfo ExtractAssets(string cshtml)
{
    var css = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
    var js = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
    var notes = new List<string>();

    foreach (Match m in Regex.Matches(cshtml, @"\b(?:src|href)\s*=\s*(?:""(?<u>[^""]+)""|'(?<u>[^']+)')", RegexOptions.IgnoreCase))
    {
        var url = m.Groups["u"].Value.Trim();
        if (url.EndsWith(".css", StringComparison.OrdinalIgnoreCase))
            css.Add(url);
        else if (url.EndsWith(".js", StringComparison.OrdinalIgnoreCase))
            js.Add(url);
    }

    if (cshtml.Contains("@section ScriptsBlock", StringComparison.OrdinalIgnoreCase))
        notes.Add("Contém `@section ScriptsBlock`.");
    if (cshtml.Contains("@section HeadBlock", StringComparison.OrdinalIgnoreCase))
        notes.Add("Contém `@section HeadBlock`.");
    if (cshtml.Contains("DataTable", StringComparison.OrdinalIgnoreCase) || cshtml.Contains("DataTables", StringComparison.OrdinalIgnoreCase))
        notes.Add("Possível uso de DataTables (detectado por string).");
    if (cshtml.Contains("ejs-", StringComparison.OrdinalIgnoreCase))
        notes.Add("Possível uso de componentes Syncfusion EJ2 (detectado por tags `ejs-*`).");

    return new AssetInfo(css.OrderBy(x => x, StringComparer.OrdinalIgnoreCase).ToList(),
        js.OrderBy(x => x, StringComparer.OrdinalIgnoreCase).ToList(),
        notes);
}

    private static IEnumerable<string> ExtractClassNames(string content)
{
    foreach (Match m in Regex.Matches(content, @"\bclass\s+(?<n>[A-Za-z_][A-Za-z0-9_]*)\b"))
        yield return m.Groups["n"].Value;
}

    private static IEnumerable<string> ExtractAttributeValues(string content, string attributeName)
{
    // Ex: [Route("api/foo")]
    foreach (Match m in Regex.Matches(content, @$"\[\s*{Regex.Escape(attributeName)}\s*\(\s*(?:""(?<v>[^""]+)""|'(?<v>[^']+)')", RegexOptions.IgnoreCase))
        yield return m.Groups["v"].Value;
}

    private static IEnumerable<string> ExtractHttpVerbAttributes(string content)
{
    var verbs = new[] { "HttpGet", "HttpPost", "HttpPut", "HttpDelete", "HttpPatch" };
    foreach (var v in verbs)
    {
        foreach (Match m in Regex.Matches(content, @$"\[\s*{v}\b[^\]]*\]", RegexOptions.IgnoreCase))
            yield return m.Value.Replace("\r", "").Replace("\n", "").Trim();
    }
}

    private static string NormalizeSlashes(string path) => path.Replace('\\', '/');

    private static string SanitizeForFileName(string s)
{
    var invalid = Path.GetInvalidFileNameChars();
    var cleaned = new string(s.Select(ch => invalid.Contains(ch) ? '-' : ch).ToArray());
    cleaned = cleaned.Replace("  ", " ").Trim();
    return cleaned;
}

    private readonly record struct AssetInfo(List<string> Css, List<string> Js, List<string> Notes);

    private sealed class GenerationResult
    {
        public int Created { get; set; }
        public int Updated { get; set; }
        public int Skipped { get; set; }
        public int Failed { get; set; }
    }
}

