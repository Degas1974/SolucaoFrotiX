# Services/ClaudeAnalysisService.cs

**ARQUIVO NOVO** | 270 linhas de codigo

> Copiar integralmente para o Janeiro.

---

```csharp
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using FrotiX.Models;

namespace FrotiX.Services;

public class ClaudeAnalysisService : IClaudeAnalysisService
{
    private readonly HttpClient _httpClient;
    private readonly ClaudeAISettings _settings;
    private readonly ILogger<ClaudeAnalysisService> _logger;
    private const string ANTHROPIC_API_URL = "https://api.anthropic.com/v1/messages";
    private const string ANTHROPIC_VERSION = "2023-06-01";

    public ClaudeAnalysisService(
        IHttpClientFactory httpClientFactory,
        IConfiguration configuration,
        ILogger<ClaudeAnalysisService> logger)
    {
        _httpClient = httpClientFactory.CreateClient("ClaudeAI");
        _logger = logger;

        _settings = new ClaudeAISettings();
        configuration.GetSection("ClaudeAI").Bind(_settings);

        if (!string.IsNullOrEmpty(_settings.ApiKey))
        {
            _httpClient.DefaultRequestHeaders.Add("x-api-key", _settings.ApiKey);
            _httpClient.DefaultRequestHeaders.Add("anthropic-version", ANTHROPIC_VERSION);
        }
    }

    public bool IsConfigured => !string.IsNullOrEmpty(_settings.ApiKey) &&
                                 _settings.ApiKey != "COLE_SUA_API_KEY_AQUI";

    public async Task<ClaudeAnalysisResult> AnalyzeErrorAsync(LogErro logErro)
    {
        if (!IsConfigured)
        {
            return new ClaudeAnalysisResult
            {
                Success = false,
                Error = "Serviço Claude AI não configurado. Configure a API Key em appsettings.json."
            };
        }

        try
        {

            var errorContext = BuildErrorContext(logErro);

            var request = new ClaudeRequest
            {
                Model = _settings.Model,
                MaxTokens = _settings.MaxTokens,
                System = _settings.SystemPrompt,
                Messages = new[]
                {
                    new ClaudeMessage
                    {
                        Role = "user",
                        Content = errorContext
                    }
                }
            };

            var json = JsonSerializer.Serialize(request, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            });

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            _logger.LogInformation("Enviando erro #{LogErroId} para análise do Claude AI", logErro.LogErroId);

            var response = await _httpClient.PostAsync(ANTHROPIC_API_URL, content);
            var responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Erro na API Claude: {StatusCode} - {Response}",
                    response.StatusCode, responseBody);

                return new ClaudeAnalysisResult
                {
                    Success = false,
                    Error = $"Erro na API Claude: {response.StatusCode} - {ParseErrorMessage(responseBody)}"
                };
            }

            var claudeResponse = JsonSerializer.Deserialize<ClaudeResponse>(responseBody, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
            });

            if (claudeResponse == null)
            {
                return new ClaudeAnalysisResult
                {
                    Success = false,
                    Error = "Resposta inválida da API Claude"
                };
            }

            var analysisText = "";
            if (claudeResponse.Content != null)
            {
                foreach (var block in claudeResponse.Content)
                {
                    if (block.Type == "text")
                    {
                        analysisText += block.Text;
                    }
                }
            }

            _logger.LogInformation("Análise concluída para erro #{LogErroId}. Tokens: {Input}/{Output}",
                logErro.LogErroId, claudeResponse.Usage?.InputTokens ?? 0, claudeResponse.Usage?.OutputTokens ?? 0);

            return new ClaudeAnalysisResult
            {
                Success = true,
                Analysis = analysisText,
                Model = claudeResponse.Model,
                InputTokens = claudeResponse.Usage?.InputTokens ?? 0,
                OutputTokens = claudeResponse.Usage?.OutputTokens ?? 0
            };
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Erro de conexão com API Claude");
            return new ClaudeAnalysisResult
            {
                Success = false,
                Error = $"Erro de conexão: {ex.Message}"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro inesperado ao analisar erro com Claude");
            return new ClaudeAnalysisResult
            {
                Success = false,
                Error = $"Erro inesperado: {ex.Message}"
            };
        }
    }

    private string BuildErrorContext(LogErro logErro)
    {
        var sb = new StringBuilder();

        sb.AppendLine("# Erro para Análise");
        sb.AppendLine();

        sb.AppendLine("## Informações Básicas");
        sb.AppendLine($"- **Tipo:** {logErro.Tipo}");
        sb.AppendLine($"- **Origem:** {logErro.Origem}");
        sb.AppendLine($"- **Nível:** {logErro.Nivel ?? "N/A"}");
        sb.AppendLine($"- **Data/Hora:** {logErro.DataHora:dd/MM/yyyy HH:mm:ss}");
        sb.AppendLine();

        sb.AppendLine("## Localização");
        if (!string.IsNullOrEmpty(logErro.Arquivo))
            sb.AppendLine($"- **Arquivo:** {logErro.Arquivo}");
        if (!string.IsNullOrEmpty(logErro.Metodo))
            sb.AppendLine($"- **Método/Função:** {logErro.Metodo}");
        if (logErro.Linha.HasValue)
            sb.AppendLine($"- **Linha:** {logErro.Linha}");
        if (logErro.Coluna.HasValue)
            sb.AppendLine($"- **Coluna:** {logErro.Coluna}");
        if (!string.IsNullOrEmpty(logErro.Url))
            sb.AppendLine($"- **URL:** {logErro.Url}");
        sb.AppendLine();

        sb.AppendLine("## Mensagem de Erro");
        sb.AppendLine("```");
        sb.AppendLine(logErro.Mensagem);
        sb.AppendLine("```");
        sb.AppendLine();

        if (!string.IsNullOrEmpty(logErro.ExceptionType))
        {
            sb.AppendLine("## Exceção");
            sb.AppendLine($"- **Tipo:** {logErro.ExceptionType}");
            if (!string.IsNullOrEmpty(logErro.ExceptionMessage))
            {
                sb.AppendLine($"- **Mensagem:** {logErro.ExceptionMessage}");
            }
            sb.AppendLine();
        }

        if (!string.IsNullOrEmpty(logErro.StackTrace))
        {
            sb.AppendLine("## Stack Trace");
            sb.AppendLine("```");
            sb.AppendLine(TruncateIfNeeded(logErro.StackTrace, 3000));
            sb.AppendLine("```");
            sb.AppendLine();
        }

        if (!string.IsNullOrEmpty(logErro.InnerException))
        {
            sb.AppendLine("## Inner Exception");
            sb.AppendLine("```");
            sb.AppendLine(TruncateIfNeeded(logErro.InnerException, 1500));
            sb.AppendLine("```");
            sb.AppendLine();
        }

        if (!string.IsNullOrEmpty(logErro.DadosAdicionais))
        {
            sb.AppendLine("## Dados Adicionais (JSON)");
            sb.AppendLine("```json");
            sb.AppendLine(TruncateIfNeeded(logErro.DadosAdicionais, 1500));
            sb.AppendLine("```");
            sb.AppendLine();
        }

        sb.AppendLine("## Contexto");
        sb.AppendLine($"- **Usuário:** {logErro.Usuario ?? "Anônimo"}");
        if (!string.IsNullOrEmpty(logErro.UserAgent))
            sb.AppendLine($"- **User Agent:** {logErro.UserAgent}");
        if (!string.IsNullOrEmpty(logErro.HttpMethod))
            sb.AppendLine($"- **Método HTTP:** {logErro.HttpMethod}");
        if (logErro.StatusCode.HasValue)
            sb.AppendLine($"- **Status Code:** {logErro.StatusCode}");
        sb.AppendLine();

        sb.AppendLine("---");
        sb.AppendLine();
        sb.AppendLine("Por favor, analise este erro e forneça:");
        sb.AppendLine("1. **Diagnóstico:** Qual é a causa raiz provável deste erro?");
        sb.AppendLine("2. **Correção:** Quais são os passos específicos para corrigir? Inclua código quando aplicável.");
        sb.AppendLine("3. **Prevenção:** Como evitar erros similares no futuro?");

        return sb.ToString();
    }

    private string TruncateIfNeeded(string text, int maxLength)
    {
        if (string.IsNullOrEmpty(text) || text.Length <= maxLength)
            return text;

        return text.Substring(0, maxLength) + "\n... [truncado]";
    }

    private string ParseErrorMessage(string responseBody)
    {
        try
        {
            using var doc = JsonDocument.Parse(responseBody);
            if (doc.RootElement.TryGetProperty("error", out var errorElement))
            {
                if (errorElement.TryGetProperty("message", out var messageElement))
                {
                    return messageElement.GetString() ?? "Erro desconhecido";
                }
            }
            return responseBody;
        }
        catch
        {
            return responseBody;
        }
    }
}

internal class ClaudeRequest
{
    public string Model { get; set; } = "";
    public int MaxTokens { get; set; }
    public string? System { get; set; }
    public ClaudeMessage[] Messages { get; set; } = Array.Empty<ClaudeMessage>();
}

internal class ClaudeMessage
{
    public string Role { get; set; } = "";
    public string Content { get; set; } = "";
}

internal class ClaudeResponse
{
    public string? Id { get; set; }
    public string? Type { get; set; }
    public string? Role { get; set; }
    public string? Model { get; set; }
    public ClaudeContentBlock[]? Content { get; set; }
    public ClaudeUsage? Usage { get; set; }
}

internal class ClaudeContentBlock
{
    public string Type { get; set; } = "";
    public string? Text { get; set; }
}

internal class ClaudeUsage
{
    public int InputTokens { get; set; }
    public int OutputTokens { get; set; }
}
```
