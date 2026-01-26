using DeepSeekIDE.Core.Models;
using LibGit2Sharp;
using LibGit2Sharp.Handlers;

namespace DeepSeekIDE.Core.Services;

/// <summary>
/// Serviço para operações Git usando LibGit2Sharp
/// </summary>
public class GitService : IDisposable
{
    private Repository? _repository;
    private string _repositoryPath = string.Empty;
    private GitCredentials? _credentials;

    public event EventHandler<string>? OnProgressUpdate;
    public bool IsRepositoryOpen => _repository != null;

    /// <summary>
    /// Abre um repositório Git existente
    /// </summary>
    public bool OpenRepository(string path)
    {
        try
        {
            CloseRepository();

            // Encontra a raiz do repositório
            var repoPath = Repository.Discover(path);
            if (string.IsNullOrEmpty(repoPath))
                return false;

            _repository = new Repository(repoPath);
            _repositoryPath = Path.GetDirectoryName(repoPath.TrimEnd(Path.DirectorySeparatorChar))!;
            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Inicializa um novo repositório Git
    /// </summary>
    public bool InitRepository(string path)
    {
        try
        {
            Repository.Init(path);
            return OpenRepository(path);
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Clona um repositório remoto
    /// </summary>
    public async Task<bool> CloneRepositoryAsync(string url, string localPath, GitCredentials? credentials = null)
    {
        return await Task.Run(() =>
        {
            try
            {
                var options = new CloneOptions();

                if (credentials != null)
                {
                    options.FetchOptions.CredentialsProvider = GetCredentialsHandler(credentials);
                }

                OnProgressUpdate?.Invoke(this, "Clonando repositório...");
                Repository.Clone(url, localPath, options);
                OnProgressUpdate?.Invoke(this, "Clone concluído!");
                return OpenRepository(localPath);
            }
            catch
            {
                return false;
            }
        });
    }

    /// <summary>
    /// Define as credenciais para operações remotas
    /// </summary>
    public void SetCredentials(GitCredentials credentials)
    {
        _credentials = credentials;
    }

    /// <summary>
    /// Obtém o status do repositório
    /// </summary>
    public GitRepositoryStatus? GetStatus()
    {
        if (_repository == null) return null;

        var status = new GitRepositoryStatus
        {
            RepositoryPath = _repositoryPath,
            CurrentBranch = _repository.Head.FriendlyName,
            IsHead = !_repository.Head.IsTracking,
            HasUncommittedChanges = _repository.RetrieveStatus().IsDirty
        };

        // Tracking info
        if (_repository.Head.IsTracking && _repository.Head.TrackingDetails != null)
        {
            status.AheadBy = _repository.Head.TrackingDetails.AheadBy ?? 0;
            status.BehindBy = _repository.Head.TrackingDetails.BehindBy ?? 0;
        }

        // File status
        foreach (var entry in _repository.RetrieveStatus())
        {
            var fileStatus = new GitFileStatus
            {
                FilePath = entry.FilePath,
                FileName = Path.GetFileName(entry.FilePath),
                State = MapFileState(entry.State),
                IsStaged = entry.State.HasFlag(FileStatus.ModifiedInIndex) ||
                           entry.State.HasFlag(FileStatus.NewInIndex) ||
                           entry.State.HasFlag(FileStatus.DeletedFromIndex) ||
                           entry.State.HasFlag(FileStatus.RenamedInIndex)
            };

            if (fileStatus.State != GitFileState.Unmodified && fileStatus.State != GitFileState.Ignored)
            {
                status.Files.Add(fileStatus);
            }
        }

        // Branches
        status.LocalBranches = _repository.Branches.Where(b => !b.IsRemote).Select(b => b.FriendlyName).ToList();
        status.RemoteBranches = _repository.Branches.Where(b => b.IsRemote).Select(b => b.FriendlyName).ToList();

        // Remote URL
        var origin = _repository.Network.Remotes.FirstOrDefault(r => r.Name == "origin");
        status.RemoteUrl = origin?.Url;

        return status;
    }

    /// <summary>
    /// Adiciona arquivos ao staging area
    /// </summary>
    public void Stage(params string[] paths)
    {
        if (_repository == null) return;

        foreach (var path in paths)
        {
            Commands.Stage(_repository, path);
        }
    }

    /// <summary>
    /// Adiciona todos os arquivos ao staging area
    /// </summary>
    public void StageAll()
    {
        if (_repository == null) return;
        Commands.Stage(_repository, "*");
    }

    /// <summary>
    /// Remove arquivos do staging area
    /// </summary>
    public void Unstage(params string[] paths)
    {
        if (_repository == null) return;

        foreach (var path in paths)
        {
            Commands.Unstage(_repository, path);
        }
    }

    /// <summary>
    /// Cria um commit
    /// </summary>
    public GitCommitInfo? Commit(string message, string authorName, string authorEmail)
    {
        if (_repository == null) return null;

        var signature = new Signature(authorName, authorEmail, DateTimeOffset.Now);
        var commit = _repository.Commit(message, signature, signature);

        return new GitCommitInfo
        {
            Sha = commit.Sha,
            Message = commit.Message,
            Author = commit.Author.Name,
            AuthorEmail = commit.Author.Email,
            Date = commit.Author.When.DateTime,
            ParentShas = commit.Parents.Select(p => p.Sha).ToList()
        };
    }

    /// <summary>
    /// Faz push para o repositório remoto
    /// </summary>
    public async Task<bool> PushAsync(string? remoteName = null, string? branchName = null)
    {
        if (_repository == null) return false;

        return await Task.Run(() =>
        {
            try
            {
                var remote = _repository.Network.Remotes[remoteName ?? "origin"];
                var branch = branchName ?? _repository.Head.FriendlyName;

                var options = new PushOptions
                {
                    OnPushStatusError = (error) =>
                    {
                        OnProgressUpdate?.Invoke(this, $"Erro: {error.Message}");
                    }
                };

                if (_credentials != null)
                {
                    options.CredentialsProvider = GetCredentialsHandler(_credentials);
                }

                _repository.Network.Push(remote, $"refs/heads/{branch}:refs/heads/{branch}", options);
                return true;
            }
            catch
            {
                return false;
            }
        });
    }

    /// <summary>
    /// Faz pull do repositório remoto
    /// </summary>
    public async Task<bool> PullAsync(string authorName, string authorEmail)
    {
        if (_repository == null) return false;

        return await Task.Run(() =>
        {
            try
            {
                var options = new PullOptions
                {
                    FetchOptions = new FetchOptions()
                };

                if (_credentials != null)
                {
                    options.FetchOptions.CredentialsProvider = GetCredentialsHandler(_credentials);
                }

                var signature = new Signature(authorName, authorEmail, DateTimeOffset.Now);
                Commands.Pull(_repository, signature, options);
                return true;
            }
            catch
            {
                return false;
            }
        });
    }

    /// <summary>
    /// Faz fetch do repositório remoto
    /// </summary>
    public async Task<bool> FetchAsync(string? remoteName = null)
    {
        if (_repository == null) return false;

        return await Task.Run(() =>
        {
            try
            {
                var remote = _repository.Network.Remotes[remoteName ?? "origin"];
                var refSpecs = remote.FetchRefSpecs.Select(x => x.Specification);

                var options = new FetchOptions();
                if (_credentials != null)
                {
                    options.CredentialsProvider = GetCredentialsHandler(_credentials);
                }

                Commands.Fetch(_repository, remote.Name, refSpecs, options, string.Empty);
                return true;
            }
            catch
            {
                return false;
            }
        });
    }

    /// <summary>
    /// Cria uma nova branch
    /// </summary>
    public bool CreateBranch(string branchName)
    {
        if (_repository == null) return false;

        try
        {
            _repository.CreateBranch(branchName);
            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Troca para uma branch
    /// </summary>
    public bool Checkout(string branchName)
    {
        if (_repository == null) return false;

        try
        {
            var branch = _repository.Branches[branchName];
            if (branch == null) return false;

            Commands.Checkout(_repository, branch);
            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Obtém o histórico de commits
    /// </summary>
    public List<GitCommitInfo> GetCommitHistory(int limit = 50)
    {
        if (_repository == null) return new List<GitCommitInfo>();

        return _repository.Commits
            .Take(limit)
            .Select(c => new GitCommitInfo
            {
                Sha = c.Sha,
                Message = c.Message,
                Author = c.Author.Name,
                AuthorEmail = c.Author.Email,
                Date = c.Author.When.DateTime,
                ParentShas = c.Parents.Select(p => p.Sha).ToList()
            })
            .ToList();
    }

    /// <summary>
    /// Obtém o diff de um arquivo
    /// </summary>
    public string GetFileDiff(string filePath)
    {
        if (_repository == null) return string.Empty;

        try
        {
            var patch = _repository.Diff.Compare<Patch>(new[] { filePath });
            return patch.Content;
        }
        catch
        {
            return string.Empty;
        }
    }

    /// <summary>
    /// Descarta alterações de um arquivo
    /// </summary>
    public bool DiscardChanges(string filePath)
    {
        if (_repository == null) return false;

        try
        {
            var options = new CheckoutOptions { CheckoutModifiers = CheckoutModifiers.Force };
            _repository.CheckoutPaths(_repository.Head.FriendlyName, new[] { filePath }, options);
            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Verifica se um diretório é um repositório Git
    /// </summary>
    public static bool IsGitRepository(string path)
    {
        return !string.IsNullOrEmpty(Repository.Discover(path));
    }

    public void CloseRepository()
    {
        _repository?.Dispose();
        _repository = null;
        _repositoryPath = string.Empty;
    }

    private CredentialsHandler GetCredentialsHandler(GitCredentials credentials)
    {
        return (url, user, cred) =>
        {
            if (!string.IsNullOrEmpty(credentials.PersonalAccessToken))
            {
                return new UsernamePasswordCredentials
                {
                    Username = credentials.Username,
                    Password = credentials.PersonalAccessToken
                };
            }

            return new UsernamePasswordCredentials
            {
                Username = credentials.Username,
                Password = credentials.Password
            };
        };
    }

    private static GitFileState MapFileState(FileStatus status)
    {
        if (status.HasFlag(FileStatus.NewInIndex) || status.HasFlag(FileStatus.NewInWorkdir))
            return GitFileState.Added;
        if (status.HasFlag(FileStatus.ModifiedInIndex) || status.HasFlag(FileStatus.ModifiedInWorkdir))
            return GitFileState.Modified;
        if (status.HasFlag(FileStatus.DeletedFromIndex) || status.HasFlag(FileStatus.DeletedFromWorkdir))
            return GitFileState.Deleted;
        if (status.HasFlag(FileStatus.RenamedInIndex) || status.HasFlag(FileStatus.RenamedInWorkdir))
            return GitFileState.Renamed;
        if (status.HasFlag(FileStatus.Conflicted))
            return GitFileState.Conflicted;
        if (status.HasFlag(FileStatus.Ignored))
            return GitFileState.Ignored;

        return GitFileState.Unmodified;
    }

    public void Dispose()
    {
        CloseRepository();
        GC.SuppressFinalize(this);
    }
}
