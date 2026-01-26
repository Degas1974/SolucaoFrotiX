namespace DeepSeekIDE.Core.Models;

/// <summary>
/// Representa o status de um repositório Git
/// </summary>
public class GitRepositoryStatus
{
    public string RepositoryPath { get; set; } = string.Empty;
    public string CurrentBranch { get; set; } = string.Empty;
    public bool IsHead { get; set; }
    public bool HasUncommittedChanges { get; set; }
    public int AheadBy { get; set; }
    public int BehindBy { get; set; }
    public List<GitFileStatus> Files { get; set; } = new();
    public List<string> LocalBranches { get; set; } = new();
    public List<string> RemoteBranches { get; set; } = new();
    public string? RemoteUrl { get; set; }
}

/// <summary>
/// Status de um arquivo no Git
/// </summary>
public class GitFileStatus
{
    public string FilePath { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;
    public GitFileState State { get; set; }
    public bool IsStaged { get; set; }

    /// <summary>
    /// Ícone FontAwesome baseado no estado
    /// </summary>
    public string Icon => State switch
    {
        GitFileState.Added => "fa-duotone fa-plus",
        GitFileState.Modified => "fa-duotone fa-pen",
        GitFileState.Deleted => "fa-duotone fa-trash",
        GitFileState.Renamed => "fa-duotone fa-arrow-right",
        GitFileState.Untracked => "fa-duotone fa-question",
        GitFileState.Conflicted => "fa-duotone fa-exclamation-triangle",
        _ => "fa-duotone fa-file"
    };

    /// <summary>
    /// Cor do estado
    /// </summary>
    public string Color => State switch
    {
        GitFileState.Added => "#38A169",
        GitFileState.Modified => "#D69E2E",
        GitFileState.Deleted => "#E53E3E",
        GitFileState.Renamed => "#3182CE",
        GitFileState.Untracked => "#718096",
        GitFileState.Conflicted => "#E53E3E",
        _ => "#A0AEC0"
    };
}

public enum GitFileState
{
    Unmodified,
    Added,
    Modified,
    Deleted,
    Renamed,
    Copied,
    Untracked,
    Ignored,
    Conflicted
}

/// <summary>
/// Representa um commit Git
/// </summary>
public class GitCommitInfo
{
    public string Sha { get; set; } = string.Empty;
    public string ShortSha => Sha.Length >= 7 ? Sha[..7] : Sha;
    public string Message { get; set; } = string.Empty;
    public string MessageShort => Message.Split('\n').FirstOrDefault() ?? Message;
    public string Author { get; set; } = string.Empty;
    public string AuthorEmail { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public List<string> ParentShas { get; set; } = new();
}

/// <summary>
/// Diferença entre versões de um arquivo
/// </summary>
public class GitDiff
{
    public string FilePath { get; set; } = string.Empty;
    public string OldContent { get; set; } = string.Empty;
    public string NewContent { get; set; } = string.Empty;
    public List<GitDiffHunk> Hunks { get; set; } = new();
}

public class GitDiffHunk
{
    public int OldStart { get; set; }
    public int OldLines { get; set; }
    public int NewStart { get; set; }
    public int NewLines { get; set; }
    public List<GitDiffLine> Lines { get; set; } = new();
}

public class GitDiffLine
{
    public int? OldLineNumber { get; set; }
    public int? NewLineNumber { get; set; }
    public string Content { get; set; } = string.Empty;
    public GitDiffLineType Type { get; set; }
}

public enum GitDiffLineType
{
    Context,
    Added,
    Deleted
}

/// <summary>
/// Credenciais para autenticação Git
/// </summary>
public class GitCredentials
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string? PersonalAccessToken { get; set; }
}
