namespace DeepSeekIDE.Core.Models;

/// <summary>
/// Representa um item no sistema de arquivos (pasta ou arquivo)
/// </summary>
public class FileSystemItem
{
    public string Name { get; set; } = string.Empty;
    public string FullPath { get; set; } = string.Empty;
    public bool IsDirectory { get; set; }
    public bool IsExpanded { get; set; }
    public long Size { get; set; }
    public DateTime LastModified { get; set; }
    public string Extension => IsDirectory ? "" : Path.GetExtension(Name).ToLowerInvariant();
    public List<FileSystemItem> Children { get; set; } = new();

    /// <summary>
    /// Ícone FontAwesome baseado no tipo de arquivo
    /// </summary>
    public string Icon => GetIcon();

    private string GetIcon()
    {
        if (IsDirectory)
            return "fa-duotone fa-folder";

        return Extension switch
        {
            ".cs" => "fa-duotone fa-file-code",
            ".js" => "fa-duotone fa-js",
            ".ts" => "fa-duotone fa-file-code",
            ".html" => "fa-duotone fa-html5",
            ".css" => "fa-duotone fa-css3",
            ".json" => "fa-duotone fa-brackets-curly",
            ".xml" => "fa-duotone fa-file-code",
            ".md" => "fa-duotone fa-file-lines",
            ".txt" => "fa-duotone fa-file-lines",
            ".sql" => "fa-duotone fa-database",
            ".py" => "fa-duotone fa-snake",
            ".java" => "fa-duotone fa-coffee",
            ".cpp" or ".c" or ".h" => "fa-duotone fa-file-code",
            ".cshtml" => "fa-duotone fa-file-code",
            ".razor" => "fa-duotone fa-file-code",
            ".sln" => "fa-duotone fa-cubes",
            ".csproj" => "fa-duotone fa-cube",
            ".gitignore" => "fa-duotone fa-git",
            ".png" or ".jpg" or ".jpeg" or ".gif" or ".svg" or ".ico" => "fa-duotone fa-image",
            ".pdf" => "fa-duotone fa-file-pdf",
            ".zip" or ".rar" or ".7z" => "fa-duotone fa-file-zipper",
            ".exe" or ".dll" => "fa-duotone fa-gear",
            _ => "fa-duotone fa-file"
        };
    }

    /// <summary>
    /// Linguagem para o Monaco Editor
    /// </summary>
    public string Language => GetLanguage();

    private string GetLanguage()
    {
        return Extension switch
        {
            ".cs" => "csharp",
            ".js" => "javascript",
            ".ts" => "typescript",
            ".html" or ".cshtml" or ".razor" => "html",
            ".css" => "css",
            ".json" => "json",
            ".xml" or ".csproj" or ".sln" => "xml",
            ".md" => "markdown",
            ".sql" => "sql",
            ".py" => "python",
            ".java" => "java",
            ".cpp" or ".c" or ".h" => "cpp",
            ".yaml" or ".yml" => "yaml",
            ".sh" or ".bash" => "shell",
            ".ps1" => "powershell",
            ".bat" or ".cmd" => "bat",
            _ => "plaintext"
        };
    }
}

/// <summary>
/// Resultado de busca em arquivos
/// </summary>
public class SearchResult
{
    public string FilePath { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;
    public int LineNumber { get; set; }
    public string LineContent { get; set; } = string.Empty;
    public int MatchStart { get; set; }
    public int MatchLength { get; set; }
}

/// <summary>
/// Configurações do workspace
/// </summary>
public class WorkspaceSettings
{
    public string RootPath { get; set; } = string.Empty;
    public List<string> RecentFiles { get; set; } = new();
    public List<string> OpenFiles { get; set; } = new();
    public string? ActiveFile { get; set; }
    public Dictionary<string, object> EditorSettings { get; set; } = new();
}
