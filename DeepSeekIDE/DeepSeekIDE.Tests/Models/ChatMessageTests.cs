using DeepSeekIDE.Core.Models;

namespace DeepSeekIDE.Tests.Models;

public class ChatMessageTests
{
    [Fact]
    public void User_ShouldCreateUserMessage()
    {
        // Arrange
        var content = "Hello, AI!";

        // Act
        var message = ChatMessage.User(content);

        // Assert
        Assert.Equal("user", message.Role);
        Assert.Equal(content, message.Content);
    }

    [Fact]
    public void Assistant_ShouldCreateAssistantMessage()
    {
        // Arrange
        var content = "Hello, human!";

        // Act
        var message = ChatMessage.Assistant(content);

        // Assert
        Assert.Equal("assistant", message.Role);
        Assert.Equal(content, message.Content);
    }

    [Fact]
    public void System_ShouldCreateSystemMessage()
    {
        // Arrange
        var content = "You are a helpful assistant.";

        // Act
        var message = ChatMessage.System(content);

        // Assert
        Assert.Equal("system", message.Role);
        Assert.Equal(content, message.Content);
    }
}
