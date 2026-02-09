using DeepSeekIDE.Core.Models;

namespace DeepSeekIDE.Tests.Models;

public class FileSystemItemTests
{
    [Theory]
    [InlineData(".cs", "csharp")]
    [InlineData(".js", "javascript")]
    [InlineData(".ts", "typescript")]
    [InlineData(".py", "python")]
    [InlineData(".json", "json")]
    [InlineData(".xml", "xml")]
    [InlineData(".html", "html")]
    [InlineData(".css", "css")]
    [InlineData(".md", "markdown")]
    [InlineData(".sql", "sql")]
    [InlineData(".unknown", "plaintext")]
    public void Language_ShouldReturnCorrectLanguage(string extension, string expectedLanguage)
    {
        // Arrange
        var item = new FileSystemItem
        {
            Name = $"test{extension}",
            IsDirectory = false
        };

        // Act
        var language = item.Language;

        // Assert
        Assert.Equal(expectedLanguage, language);
    }

    [Theory]
    [InlineData(".cs", "fa-duotone fa-file-code")]
    [InlineData(".js", "fa-duotone fa-js")]
    [InlineData(".py", "fa-duotone fa-snake")]
    [InlineData(".json", "fa-duotone fa-brackets-curly")]
    [InlineData(".md", "fa-duotone fa-file-lines")]
    [InlineData(".unknown", "fa-duotone fa-file")]
    public void Icon_ShouldReturnCorrectIcon(string extension, string expectedIcon)
    {
        // Arrange
        var item = new FileSystemItem
        {
            Name = $"test{extension}",
            IsDirectory = false
        };

        // Act
        var icon = item.Icon;

        // Assert
        Assert.Equal(expectedIcon, icon);
    }

    [Fact]
    public void Icon_ShouldReturnFolderIconForDirectory()
    {
        // Arrange
        var item = new FileSystemItem
        {
            Name = "folder",
            IsDirectory = true
        };

        // Act
        var icon = item.Icon;

        // Assert
        Assert.Equal("fa-duotone fa-folder", icon);
    }

    [Fact]
    public void FileSystemItem_ShouldInitializeWithEmptyChildren()
    {
        // Act
        var item = new FileSystemItem();

        // Assert
        Assert.NotNull(item.Children);
        Assert.Empty(item.Children);
    }

    [Fact]
    public void FileSystemItem_ShouldSetPropertiesCorrectly()
    {
        // Arrange & Act
        var item = new FileSystemItem
        {
            Name = "test.cs",
            FullPath = @"C:\project\test.cs",
            IsDirectory = false,
            Size = 1024,
            LastModified = DateTime.Now
        };

        // Assert
        Assert.Equal("test.cs", item.Name);
        Assert.Equal(@"C:\project\test.cs", item.FullPath);
        Assert.False(item.IsDirectory);
        Assert.Equal(1024, item.Size);
    }

    [Fact]
    public void Extension_ShouldReturnCorrectExtension()
    {
        // Arrange
        var item = new FileSystemItem
        {
            Name = "test.cs",
            IsDirectory = false
        };

        // Act
        var extension = item.Extension;

        // Assert
        Assert.Equal(".cs", extension);
    }

    [Fact]
    public void Extension_ShouldReturnEmptyForDirectory()
    {
        // Arrange
        var item = new FileSystemItem
        {
            Name = "folder",
            IsDirectory = true
        };

        // Act
        var extension = item.Extension;

        // Assert
        Assert.Equal("", extension);
    }
}
