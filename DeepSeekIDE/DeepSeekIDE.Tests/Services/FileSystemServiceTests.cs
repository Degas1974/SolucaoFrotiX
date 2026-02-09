using DeepSeekIDE.Core.Services;

namespace DeepSeekIDE.Tests.Services;

public class FileSystemServiceTests : IDisposable
{
    private readonly FileSystemService _fileSystemService;
    private readonly string _testDirectory;

    public FileSystemServiceTests()
    {
        _fileSystemService = new FileSystemService();
        _testDirectory = Path.Combine(Path.GetTempPath(), $"DeepSeekIDE_Tests_{Guid.NewGuid()}");
        Directory.CreateDirectory(_testDirectory);
    }

    public void Dispose()
    {
        if (Directory.Exists(_testDirectory))
        {
            Directory.Delete(_testDirectory, recursive: true);
        }
        GC.SuppressFinalize(this);
    }

    [Fact]
    public async Task CreateFileAsync_ShouldCreateNewFile()
    {
        // Arrange
        var fileName = "test.txt";
        var content = "Hello, World!";

        // Act
        var path = await _fileSystemService.CreateFileAsync(_testDirectory, fileName, content);

        // Assert
        Assert.True(File.Exists(path));
        var actualContent = await File.ReadAllTextAsync(path);
        Assert.Equal(content, actualContent);
    }

    [Fact]
    public async Task ReadFileAsync_ShouldReturnFileContent()
    {
        // Arrange
        var filePath = Path.Combine(_testDirectory, "read_test.txt");
        var expectedContent = "Test content";
        await File.WriteAllTextAsync(filePath, expectedContent);

        // Act
        var content = await _fileSystemService.ReadFileAsync(filePath);

        // Assert
        Assert.Equal(expectedContent, content);
    }

    [Fact]
    public async Task ReadFileAsync_ShouldThrowWhenFileNotExists()
    {
        // Arrange
        var nonExistentPath = Path.Combine(_testDirectory, "non_existent.txt");

        // Act & Assert
        await Assert.ThrowsAsync<FileNotFoundException>(() =>
            _fileSystemService.ReadFileAsync(nonExistentPath));
    }

    [Fact]
    public async Task SaveFileAsync_ShouldSaveContent()
    {
        // Arrange
        var filePath = Path.Combine(_testDirectory, "save_test.txt");
        var content = "Saved content";

        // Act
        await _fileSystemService.SaveFileAsync(filePath, content);

        // Assert
        Assert.True(File.Exists(filePath));
        var actualContent = await File.ReadAllTextAsync(filePath);
        Assert.Equal(content, actualContent);
    }

    [Fact]
    public void CreateDirectory_ShouldCreateNewDirectory()
    {
        // Arrange
        var folderName = "new_folder";

        // Act
        var path = _fileSystemService.CreateDirectory(_testDirectory, folderName);

        // Assert
        Assert.True(Directory.Exists(path));
    }

    [Fact]
    public async Task Delete_ShouldDeleteFile()
    {
        // Arrange
        var filePath = Path.Combine(_testDirectory, "to_delete.txt");
        await File.WriteAllTextAsync(filePath, "content");

        // Act
        _fileSystemService.Delete(filePath);

        // Assert
        Assert.False(File.Exists(filePath));
    }

    [Fact]
    public void Delete_ShouldDeleteDirectory()
    {
        // Arrange
        var dirPath = Path.Combine(_testDirectory, "to_delete_folder");
        Directory.CreateDirectory(dirPath);

        // Act
        _fileSystemService.Delete(dirPath);

        // Assert
        Assert.False(Directory.Exists(dirPath));
    }

    [Fact]
    public async Task Rename_ShouldRenameFile()
    {
        // Arrange
        var originalPath = Path.Combine(_testDirectory, "original.txt");
        var newName = "renamed.txt";
        await File.WriteAllTextAsync(originalPath, "content");

        // Act
        var newPath = _fileSystemService.Rename(originalPath, newName);

        // Assert
        Assert.False(File.Exists(originalPath));
        Assert.True(File.Exists(newPath));
    }

    [Fact]
    public void GetDirectoryStructure_ShouldReturnCorrectStructure()
    {
        // Arrange
        var subDir = Path.Combine(_testDirectory, "subdir");
        Directory.CreateDirectory(subDir);
        File.WriteAllText(Path.Combine(_testDirectory, "file1.txt"), "content1");
        File.WriteAllText(Path.Combine(subDir, "file2.txt"), "content2");

        // Act
        var result = _fileSystemService.GetDirectoryStructure(_testDirectory);

        // Assert
        Assert.True(result.IsDirectory);
        Assert.Equal(2, result.Children.Count); // subdir + file1.txt
    }

    [Fact]
    public async Task SearchInFilesAsync_ShouldFindMatchingText()
    {
        // Arrange
        var filePath = Path.Combine(_testDirectory, "searchable.txt");
        await File.WriteAllTextAsync(filePath, "Line 1\nFind this text\nLine 3");

        // Act
        var results = await _fileSystemService.SearchInFilesAsync(
            _testDirectory,
            "Find this",
            caseSensitive: false,
            useRegex: false,
            filePattern: "*.txt");

        // Assert
        Assert.Single(results);
        Assert.Equal(2, results[0].LineNumber);
        Assert.Contains("Find this text", results[0].LineContent);
    }

    [Fact]
    public void IsValidPath_ShouldReturnTrueForExistingDirectory()
    {
        // Act & Assert
        Assert.True(_fileSystemService.IsValidPath(_testDirectory));
    }

    [Fact]
    public void IsValidPath_ShouldReturnFalseForNonExistentPath()
    {
        // Act & Assert
        Assert.False(_fileSystemService.IsValidPath(Path.Combine(_testDirectory, "non_existent")));
    }

    [Fact]
    public void GetFileInfo_ShouldReturnCorrectInfo()
    {
        // Arrange
        var filePath = Path.Combine(_testDirectory, "info_test.txt");
        File.WriteAllText(filePath, "test content");

        // Act
        var info = _fileSystemService.GetFileInfo(filePath);

        // Assert
        Assert.Equal("info_test.txt", info.Name);
        Assert.Equal(filePath, info.FullPath);
        Assert.False(info.IsDirectory);
    }
}
