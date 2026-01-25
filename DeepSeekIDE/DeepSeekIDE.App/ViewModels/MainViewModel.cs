using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Input;
using DeepSeekIDE.Core.Models;
using DeepSeekIDE.Core.Services;

namespace DeepSeekIDE.App.ViewModels;

/// <summary>
/// ViewModel principal para a MainWindow
/// Esta classe serve como exemplo e ponto de partida para migração completa para MVVM
/// </summary>
public class MainViewModel : ViewModelBase, IDisposable
{
    private readonly DeepSeekService _deepSeekService;
    private readonly FileSystemService _fileSystemService;
    private readonly GitService _gitService;
    private readonly LogService _logger = LogService.Instance;

    private string _currentWorkspacePath = string.Empty;
    private string _currentFilePath = string.Empty;
    private string _currentFileContent = string.Empty;
    private string _chatInput = string.Empty;
    private string _commitMessage = string.Empty;
    private string _searchQuery = string.Empty;
    private bool _isLoading;
    private string _statusMessage = string.Empty;
    private FileSystemItem? _selectedFile;
    private GitRepositoryStatus? _gitStatus;

    public MainViewModel()
    {
        // Inicializa serviços
        _deepSeekService = new DeepSeekService("sk-abe79be96b3347d6b07888636e5253b3");
        _fileSystemService = new FileSystemService();
        _gitService = new GitService();

        // Inicializa coleções
        OpenFiles = new ObservableCollection<OpenFileInfo>();
        ChatMessages = new ObservableCollection<ChatMessageViewModel>();
        SearchResults = new ObservableCollection<SearchResult>();
        FileTree = new ObservableCollection<FileSystemItem>();

        // Inicializa comandos
        OpenFolderCommand = new DelegateCommand(ExecuteOpenFolder);
        SaveFileCommand = new AsyncDelegateCommand(ExecuteSaveFile, () => !string.IsNullOrEmpty(CurrentFilePath));
        SendChatCommand = new AsyncDelegateCommand(ExecuteSendChat, () => !string.IsNullOrEmpty(ChatInput));
        CommitCommand = new DelegateCommand(ExecuteCommit, _ => !string.IsNullOrEmpty(CommitMessage));
        SearchCommand = new AsyncDelegateCommand(ExecuteSearch, () => !string.IsNullOrEmpty(SearchQuery));
        RefreshGitCommand = new DelegateCommand(_ => RefreshGitStatus());
        PushCommand = new AsyncDelegateCommand(ExecutePush);
        PullCommand = new AsyncDelegateCommand(ExecutePull);
        FetchCommand = new AsyncDelegateCommand(ExecuteFetch);

        _logger.Info("MainViewModel inicializado", "MainViewModel");
    }

    #region Properties

    public string CurrentWorkspacePath
    {
        get => _currentWorkspacePath;
        set => SetProperty(ref _currentWorkspacePath, value);
    }

    public string CurrentFilePath
    {
        get => _currentFilePath;
        set => SetProperty(ref _currentFilePath, value);
    }

    public string CurrentFileContent
    {
        get => _currentFileContent;
        set => SetProperty(ref _currentFileContent, value);
    }

    public string ChatInput
    {
        get => _chatInput;
        set => SetProperty(ref _chatInput, value);
    }

    public string CommitMessage
    {
        get => _commitMessage;
        set => SetProperty(ref _commitMessage, value);
    }

    public string SearchQuery
    {
        get => _searchQuery;
        set => SetProperty(ref _searchQuery, value);
    }

    public bool IsLoading
    {
        get => _isLoading;
        set => SetProperty(ref _isLoading, value);
    }

    public string StatusMessage
    {
        get => _statusMessage;
        set => SetProperty(ref _statusMessage, value);
    }

    public FileSystemItem? SelectedFile
    {
        get => _selectedFile;
        set
        {
            if (SetProperty(ref _selectedFile, value) && value != null && !value.IsDirectory)
            {
                _ = OpenFileAsync(value.FullPath);
            }
        }
    }

    public GitRepositoryStatus? GitStatus
    {
        get => _gitStatus;
        set => SetProperty(ref _gitStatus, value);
    }

    public ObservableCollection<OpenFileInfo> OpenFiles { get; }
    public ObservableCollection<ChatMessageViewModel> ChatMessages { get; }
    public ObservableCollection<SearchResult> SearchResults { get; }
    public ObservableCollection<FileSystemItem> FileTree { get; }

    #endregion

    #region Commands

    public ICommand OpenFolderCommand { get; }
    public ICommand SaveFileCommand { get; }
    public ICommand SendChatCommand { get; }
    public ICommand CommitCommand { get; }
    public ICommand SearchCommand { get; }
    public ICommand RefreshGitCommand { get; }
    public ICommand PushCommand { get; }
    public ICommand PullCommand { get; }
    public ICommand FetchCommand { get; }

    #endregion

    #region Command Implementations

    private void ExecuteOpenFolder(object? parameter)
    {
        var dialog = new System.Windows.Forms.FolderBrowserDialog
        {
            Description = "Selecione a pasta do projeto"
        };

        if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
        {
            OpenWorkspace(dialog.SelectedPath);
        }
    }

    private void OpenWorkspace(string path)
    {
        CurrentWorkspacePath = path;

        var rootItem = _fileSystemService.GetDirectoryStructure(path, maxDepth: 4);
        FileTree.Clear();
        FileTree.Add(rootItem);

        if (GitService.IsGitRepository(path))
        {
            _gitService.OpenRepository(path);
            RefreshGitStatus();
        }

        _logger.Info($"Workspace aberto: {path}", "MainViewModel");
        StatusMessage = $"Pasta aberta: {path}";
    }

    private async Task OpenFileAsync(string filePath)
    {
        try
        {
            IsLoading = true;
            var content = await _fileSystemService.ReadFileAsync(filePath);
            CurrentFilePath = filePath;
            CurrentFileContent = content;

            var existing = OpenFiles.FirstOrDefault(f => f.FilePath == filePath);
            if (existing == null)
            {
                OpenFiles.Add(new OpenFileInfo
                {
                    FilePath = filePath,
                    FileName = Path.GetFileName(filePath),
                    Content = content
                });
            }

            _logger.Info($"Arquivo aberto: {filePath}", "MainViewModel");
        }
        catch (Exception ex)
        {
            _logger.Error($"Erro ao abrir arquivo: {filePath}", "MainViewModel", ex);
            StatusMessage = $"Erro ao abrir arquivo: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }

    private async Task ExecuteSaveFile()
    {
        if (string.IsNullOrEmpty(CurrentFilePath)) return;

        try
        {
            await _fileSystemService.SaveFileAsync(CurrentFilePath, CurrentFileContent);
            _logger.Info($"Arquivo salvo: {CurrentFilePath}", "MainViewModel");
            StatusMessage = "Arquivo salvo com sucesso";
        }
        catch (Exception ex)
        {
            _logger.Error($"Erro ao salvar arquivo: {CurrentFilePath}", "MainViewModel", ex);
            StatusMessage = $"Erro ao salvar: {ex.Message}";
        }
    }

    private async Task ExecuteSendChat()
    {
        if (string.IsNullOrEmpty(ChatInput)) return;

        var userMessage = ChatInput;
        ChatInput = string.Empty;

        ChatMessages.Add(new ChatMessageViewModel
        {
            Content = userMessage,
            IsUser = true,
            Timestamp = DateTime.Now
        });

        try
        {
            var messages = new List<ChatMessage>
            {
                ChatMessage.System("Você é um assistente de programação útil."),
                ChatMessage.User(userMessage)
            };

            var response = await _deepSeekService.SendMessageAsync(messages);

            ChatMessages.Add(new ChatMessageViewModel
            {
                Content = response,
                IsUser = false,
                Timestamp = DateTime.Now
            });
        }
        catch (Exception ex)
        {
            _logger.Error("Erro ao enviar mensagem", "MainViewModel", ex);
            ChatMessages.Add(new ChatMessageViewModel
            {
                Content = $"Erro: {ex.Message}",
                IsUser = false,
                IsError = true,
                Timestamp = DateTime.Now
            });
        }
    }

    private void ExecuteCommit(object? parameter)
    {
        if (string.IsNullOrEmpty(CommitMessage)) return;

        try
        {
            _gitService.StageAll();
            var commit = _gitService.Commit(CommitMessage, "DeepSeek IDE User", "user@deepseekide.local");

            if (commit != null)
            {
                _logger.Info($"Commit criado: {commit.ShortSha}", "MainViewModel");
                StatusMessage = $"Commit criado: {commit.ShortSha}";
                CommitMessage = string.Empty;
                RefreshGitStatus();
            }
        }
        catch (Exception ex)
        {
            _logger.Error("Erro no commit", "MainViewModel", ex);
            StatusMessage = $"Erro no commit: {ex.Message}";
        }
    }

    private async Task ExecuteSearch()
    {
        if (string.IsNullOrEmpty(SearchQuery) || string.IsNullOrEmpty(CurrentWorkspacePath)) return;

        try
        {
            IsLoading = true;
            SearchResults.Clear();

            var results = await _fileSystemService.SearchInFilesAsync(CurrentWorkspacePath, SearchQuery);

            foreach (var result in results.Take(100))
            {
                SearchResults.Add(result);
            }

            StatusMessage = $"Encontrados {results.Count} resultados";
        }
        catch (Exception ex)
        {
            _logger.Error("Erro na busca", "MainViewModel", ex);
            StatusMessage = $"Erro na busca: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }

    private void RefreshGitStatus()
    {
        GitStatus = _gitService.GetStatus();
    }

    private async Task ExecutePush()
    {
        try
        {
            StatusMessage = "Enviando...";
            var result = await _gitService.PushAsync();
            StatusMessage = result ? "Push realizado com sucesso" : "Falha no push";
            RefreshGitStatus();
        }
        catch (Exception ex)
        {
            _logger.Error("Erro no push", "MainViewModel", ex);
            StatusMessage = $"Erro no push: {ex.Message}";
        }
    }

    private async Task ExecutePull()
    {
        try
        {
            StatusMessage = "Baixando...";
            var result = await _gitService.PullAsync("DeepSeek IDE User", "user@deepseekide.local");
            StatusMessage = result ? "Pull realizado com sucesso" : "Falha no pull";
            RefreshGitStatus();
        }
        catch (Exception ex)
        {
            _logger.Error("Erro no pull", "MainViewModel", ex);
            StatusMessage = $"Erro no pull: {ex.Message}";
        }
    }

    private async Task ExecuteFetch()
    {
        try
        {
            StatusMessage = "Buscando...";
            var result = await _gitService.FetchAsync();
            StatusMessage = result ? "Fetch realizado com sucesso" : "Falha no fetch";
            RefreshGitStatus();
        }
        catch (Exception ex)
        {
            _logger.Error("Erro no fetch", "MainViewModel", ex);
            StatusMessage = $"Erro no fetch: {ex.Message}";
        }
    }

    #endregion

    public void Dispose()
    {
        _deepSeekService.Dispose();
        _gitService.Dispose();
        GC.SuppressFinalize(this);
    }
}

/// <summary>
/// Informações de arquivo aberto
/// </summary>
public class OpenFileInfo
{
    public string FilePath { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public bool IsModified { get; set; }
}

/// <summary>
/// ViewModel para mensagens do chat
/// </summary>
public class ChatMessageViewModel
{
    public string Content { get; set; } = string.Empty;
    public bool IsUser { get; set; }
    public bool IsError { get; set; }
    public DateTime Timestamp { get; set; }
}
