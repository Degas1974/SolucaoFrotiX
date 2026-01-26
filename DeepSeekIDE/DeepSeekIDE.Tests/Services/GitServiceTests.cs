using DeepSeekIDE.Core.Services;

namespace DeepSeekIDE.Tests.Services;

public class GitServiceTests : IDisposable
{
    private readonly string _testDirectory;

    public GitServiceTests()
    {
        _testDirectory = Path.Combine(Path.GetTempPath(), $"DeepSeekIDE_GitTests_{Guid.NewGuid()}");
        Directory.CreateDirectory(_testDirectory);
    }

    public void Dispose()
    {
        // Tentar limpar o diretório de teste
        try
        {
            if (Directory.Exists(_testDirectory))
            {
                // Remover atributos somente-leitura dos arquivos .git
                foreach (var file in Directory.GetFiles(_testDirectory, "*", SearchOption.AllDirectories))
                {
                    File.SetAttributes(file, FileAttributes.Normal);
                }
                Directory.Delete(_testDirectory, recursive: true);
            }
        }
        catch
        {
            // Ignora erros de limpeza
        }
        GC.SuppressFinalize(this);
    }

    [Fact]
    public void IsGitRepository_ShouldReturnFalseForNonGitDirectory()
    {
        // Act
        var result = GitService.IsGitRepository(_testDirectory);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void InitRepository_ShouldCreateGitRepository()
    {
        // Arrange
        using var gitService = new GitService();

        // Act
        var result = gitService.InitRepository(_testDirectory);

        // Assert
        Assert.True(result);
        Assert.True(GitService.IsGitRepository(_testDirectory));
    }

    [Fact]
    public void OpenRepository_ShouldReturnFalseForNonGitDirectory()
    {
        // Arrange
        using var gitService = new GitService();

        // Act
        var result = gitService.OpenRepository(_testDirectory);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void OpenRepository_ShouldReturnTrueForGitDirectory()
    {
        // Arrange
        using var gitService = new GitService();
        gitService.InitRepository(_testDirectory);

        using var gitService2 = new GitService();

        // Act
        var result = gitService2.OpenRepository(_testDirectory);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void GetStatus_ShouldReturnStatusForInitializedRepo()
    {
        // Arrange
        using var gitService = new GitService();
        gitService.InitRepository(_testDirectory);
        gitService.OpenRepository(_testDirectory);

        // Act
        var status = gitService.GetStatus();

        // Assert
        Assert.NotNull(status);
        Assert.Equal("master", status.CurrentBranch);
        Assert.Empty(status.Files);
    }

    [Fact]
    public void GetStatus_ShouldDetectNewFile()
    {
        // Arrange
        using var gitService = new GitService();
        gitService.InitRepository(_testDirectory);
        gitService.OpenRepository(_testDirectory);

        // Criar arquivo novo
        File.WriteAllText(Path.Combine(_testDirectory, "new_file.txt"), "content");

        // Act
        var status = gitService.GetStatus();

        // Assert
        Assert.NotNull(status);
        Assert.Single(status.Files);
        Assert.Equal("new_file.txt", status.Files[0].FilePath);
    }

    [Fact]
    public void StageAll_ShouldStageAllFiles()
    {
        // Arrange
        using var gitService = new GitService();
        gitService.InitRepository(_testDirectory);
        gitService.OpenRepository(_testDirectory);
        File.WriteAllText(Path.Combine(_testDirectory, "file1.txt"), "content1");
        File.WriteAllText(Path.Combine(_testDirectory, "file2.txt"), "content2");

        // Act
        gitService.StageAll();

        // Assert
        var status = gitService.GetStatus();
        Assert.NotNull(status);
        var stagedCount = status.Files.Count(f => f.IsStaged);
        Assert.Equal(2, stagedCount);
    }

    [Fact]
    public void Commit_ShouldCreateCommit()
    {
        // Arrange
        using var gitService = new GitService();
        gitService.InitRepository(_testDirectory);
        gitService.OpenRepository(_testDirectory);
        File.WriteAllText(Path.Combine(_testDirectory, "file.txt"), "content");
        gitService.StageAll();

        // Act
        var commitInfo = gitService.Commit("Test commit", "Test User", "test@example.com");

        // Assert
        Assert.NotNull(commitInfo);
        Assert.Contains("Test commit", commitInfo.Message);
        var history = gitService.GetCommitHistory(1);
        Assert.Single(history);
    }

    [Fact]
    public void CreateBranch_ShouldCreateNewBranch()
    {
        // Arrange
        using var gitService = new GitService();
        gitService.InitRepository(_testDirectory);
        gitService.OpenRepository(_testDirectory);
        File.WriteAllText(Path.Combine(_testDirectory, "file.txt"), "content");
        gitService.StageAll();
        gitService.Commit("Initial commit", "Test User", "test@example.com");

        // Act
        var result = gitService.CreateBranch("feature-branch");

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Checkout_ShouldSwitchBranch()
    {
        // Arrange
        using var gitService = new GitService();
        gitService.InitRepository(_testDirectory);
        gitService.OpenRepository(_testDirectory);
        File.WriteAllText(Path.Combine(_testDirectory, "file.txt"), "content");
        gitService.StageAll();
        gitService.Commit("Initial commit", "Test User", "test@example.com");
        gitService.CreateBranch("feature-branch");

        // Act
        var result = gitService.Checkout("feature-branch");

        // Assert
        Assert.True(result);
        var status = gitService.GetStatus();
        Assert.Equal("feature-branch", status!.CurrentBranch);
    }
}
