namespace DeepSeekIDE.Api.Models;

/// <summary>
/// Request para enviar mensagem ao chat
/// </summary>
public class ChatRequest
{
    public List<ChatMessageDto> Messages { get; set; } = [];
    public string Model { get; set; } = "deepseek-chat";
    public bool Stream { get; set; } = false;
}

/// <summary>
/// DTO para mensagem do chat
/// </summary>
public class ChatMessageDto
{
    public string Role { get; set; } = "user";
    public string Content { get; set; } = string.Empty;
}

/// <summary>
/// Response do chat
/// </summary>
public class ChatResponse
{
    public bool Success { get; set; }
    public string Content { get; set; } = string.Empty;
    public string? Error { get; set; }
}

/// <summary>
/// Request para análise de código
/// </summary>
public class CodeAnalysisRequest
{
    public string Code { get; set; } = string.Empty;
    public string Language { get; set; } = string.Empty;
    public CodeAnalysisType AnalysisType { get; set; } = CodeAnalysisType.Analyze;
    public string? TestFramework { get; set; }
    public string? Instruction { get; set; }
}

/// <summary>
/// Tipo de análise de código
/// </summary>
public enum CodeAnalysisType
{
    Analyze,
    Explain,
    GenerateTests,
    Complete
}

/// <summary>
/// Response da análise de código
/// </summary>
public class CodeAnalysisResponse
{
    public bool Success { get; set; }
    public string Result { get; set; } = string.Empty;
    public string? Error { get; set; }
}

/// <summary>
/// Request para operações de arquivo
/// </summary>
public class FileOperationRequest
{
    public string Path { get; set; } = string.Empty;
    public string? Content { get; set; }
    public string? NewPath { get; set; }
}

/// <summary>
/// Response de operação de arquivo
/// </summary>
public class FileOperationResponse
{
    public bool Success { get; set; }
    public string? Content { get; set; }
    public string? Error { get; set; }
}

/// <summary>
/// Request para busca em arquivos
/// </summary>
public class SearchRequest
{
    public string RootPath { get; set; } = string.Empty;
    public string Pattern { get; set; } = string.Empty;
    public bool UseRegex { get; set; } = false;
    public List<string>? FileExtensions { get; set; }
}

/// <summary>
/// Response de status da API
/// </summary>
public class ApiStatusResponse
{
    public bool IsHealthy { get; set; }
    public string Version { get; set; } = string.Empty;
    public bool DeepSeekApiAvailable { get; set; }
    public DateTime ServerTime { get; set; }
}
