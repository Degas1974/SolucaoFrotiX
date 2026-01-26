using Newtonsoft.Json;

namespace DeepSeekIDE.Core.Models;

/// <summary>
/// Representa uma mensagem no chat com a IA
/// </summary>
public class ChatMessage
{
    [JsonProperty("role")]
    public string Role { get; set; } = "user";

    [JsonProperty("content")]
    public string Content { get; set; } = string.Empty;

    public DateTime Timestamp { get; set; } = DateTime.Now;

    public static ChatMessage User(string content) => new() { Role = "user", Content = content };
    public static ChatMessage Assistant(string content) => new() { Role = "assistant", Content = content };
    public static ChatMessage System(string content) => new() { Role = "system", Content = content };
}

/// <summary>
/// Request para a API DeepSeek
/// </summary>
public class DeepSeekRequest
{
    [JsonProperty("model")]
    public string Model { get; set; } = "deepseek-chat";

    [JsonProperty("messages")]
    public List<ChatMessage> Messages { get; set; } = new();

    [JsonProperty("stream")]
    public bool Stream { get; set; } = true;

    [JsonProperty("temperature")]
    public double Temperature { get; set; } = 0.7;

    [JsonProperty("max_tokens")]
    public int MaxTokens { get; set; } = 4096;
}

/// <summary>
/// Response da API DeepSeek
/// </summary>
public class DeepSeekResponse
{
    [JsonProperty("id")]
    public string Id { get; set; } = string.Empty;

    [JsonProperty("object")]
    public string Object { get; set; } = string.Empty;

    [JsonProperty("created")]
    public long Created { get; set; }

    [JsonProperty("model")]
    public string Model { get; set; } = string.Empty;

    [JsonProperty("choices")]
    public List<Choice> Choices { get; set; } = new();

    [JsonProperty("usage")]
    public Usage? Usage { get; set; }
}

public class Choice
{
    [JsonProperty("index")]
    public int Index { get; set; }

    [JsonProperty("message")]
    public ChatMessage? Message { get; set; }

    [JsonProperty("delta")]
    public Delta? Delta { get; set; }

    [JsonProperty("finish_reason")]
    public string? FinishReason { get; set; }
}

public class Delta
{
    [JsonProperty("role")]
    public string? Role { get; set; }

    [JsonProperty("content")]
    public string? Content { get; set; }
}

public class Usage
{
    [JsonProperty("prompt_tokens")]
    public int PromptTokens { get; set; }

    [JsonProperty("completion_tokens")]
    public int CompletionTokens { get; set; }

    [JsonProperty("total_tokens")]
    public int TotalTokens { get; set; }
}
