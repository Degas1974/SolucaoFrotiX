using System.Net.Http.Headers;
using System.Text;
using DeepSeekIDE.Core.Models;
using Newtonsoft.Json;

namespace DeepSeekIDE.Core.Services;

/// <summary>
/// Serviço para comunicação com a API DeepSeek
/// </summary>
public class DeepSeekService : IDisposable
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private const string BaseUrl = "https://api.deepseek.com/v1";

    public event EventHandler<string>? OnStreamChunk;
    public event EventHandler<string>? OnStreamComplete;
    public event EventHandler<Exception>? OnError;

    public DeepSeekService(string apiKey)
    {
        _apiKey = apiKey;
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(BaseUrl),
            Timeout = TimeSpan.FromMinutes(5)
        };
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    /// <summary>
    /// Envia uma mensagem para o chat e retorna a resposta completa
    /// </summary>
    public async Task<string> SendMessageAsync(List<ChatMessage> messages, string model = "deepseek-chat", CancellationToken cancellationToken = default)
    {
        var request = new DeepSeekRequest
        {
            Model = model,
            Messages = messages,
            Stream = false
        };

        var json = JsonConvert.SerializeObject(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync("/chat/completions", content, cancellationToken);
        response.EnsureSuccessStatusCode();

        var responseJson = await response.Content.ReadAsStringAsync(cancellationToken);
        var result = JsonConvert.DeserializeObject<DeepSeekResponse>(responseJson);

        return result?.Choices?.FirstOrDefault()?.Message?.Content ?? string.Empty;
    }

    /// <summary>
    /// Envia uma mensagem com streaming (respostas em tempo real)
    /// </summary>
    public async Task SendMessageStreamAsync(List<ChatMessage> messages, string model = "deepseek-chat", CancellationToken cancellationToken = default)
    {
        var request = new DeepSeekRequest
        {
            Model = model,
            Messages = messages,
            Stream = true
        };

        var json = JsonConvert.SerializeObject(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        using var requestMessage = new HttpRequestMessage(HttpMethod.Post, "/chat/completions")
        {
            Content = content
        };

        using var response = await _httpClient.SendAsync(requestMessage, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
        response.EnsureSuccessStatusCode();

        var fullResponse = new StringBuilder();

        using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
        using var reader = new StreamReader(stream);

        string? line;
        while ((line = await reader.ReadLineAsync(cancellationToken)) != null && !cancellationToken.IsCancellationRequested)
        {
            if (string.IsNullOrWhiteSpace(line))
                continue;

            if (line.StartsWith("data: "))
            {
                var data = line[6..]; // Remove "data: " prefix

                if (data == "[DONE]")
                {
                    OnStreamComplete?.Invoke(this, fullResponse.ToString());
                    break;
                }

                try
                {
                    var chunk = JsonConvert.DeserializeObject<DeepSeekResponse>(data);
                    var chunkContent = chunk?.Choices?.FirstOrDefault()?.Delta?.Content;

                    if (!string.IsNullOrEmpty(chunkContent))
                    {
                        fullResponse.Append(chunkContent);
                        OnStreamChunk?.Invoke(this, chunkContent);
                    }
                }
                catch (JsonException ex)
                {
                    OnError?.Invoke(this, new InvalidOperationException($"Erro ao processar chunk JSON: {data}", ex));
                }
            }
        }
    }

    /// <summary>
    /// Gera sugestões de código (autocomplete)
    /// </summary>
    public async Task<string> GetCodeCompletionAsync(string code, string language, string instruction = "", CancellationToken cancellationToken = default)
    {
        var systemPrompt = $@"Você é um assistente de programação especializado em {language}.
Quando o usuário fornecer código, complete-o ou sugira melhorias.
Responda APENAS com o código, sem explicações, a menos que o usuário peça.
Use boas práticas e padrões de código limpo.";

        var userMessage = string.IsNullOrEmpty(instruction)
            ? $"Complete o seguinte código {language}:\n\n```{language}\n{code}\n```"
            : $"{instruction}\n\n```{language}\n{code}\n```";

        var messages = new List<ChatMessage>
        {
            ChatMessage.System(systemPrompt),
            ChatMessage.User(userMessage)
        };

        return await SendMessageAsync(messages, "deepseek-chat", cancellationToken);
    }

    /// <summary>
    /// Analisa código e sugere melhorias
    /// </summary>
    public async Task<string> AnalyzeCodeAsync(string code, string language, CancellationToken cancellationToken = default)
    {
        var systemPrompt = @"Você é um revisor de código experiente. Analise o código fornecido e:
1. Identifique possíveis bugs ou problemas
2. Sugira melhorias de performance
3. Verifique boas práticas e padrões
4. Proponha refatorações quando apropriado

Seja objetivo e forneça exemplos de código quando sugerir mudanças.";

        var messages = new List<ChatMessage>
        {
            ChatMessage.System(systemPrompt),
            ChatMessage.User($"Analise este código {language}:\n\n```{language}\n{code}\n```")
        };

        return await SendMessageAsync(messages, "deepseek-chat", cancellationToken);
    }

    /// <summary>
    /// Explica código para o usuário
    /// </summary>
    public async Task<string> ExplainCodeAsync(string code, string language, CancellationToken cancellationToken = default)
    {
        var systemPrompt = @"Você é um professor de programação paciente e didático.
Explique o código fornecido de forma clara, incluindo:
1. O que cada parte do código faz
2. Por que certas decisões foram tomadas
3. Conceitos importantes usados
4. Possíveis casos de uso

Use linguagem simples e exemplos quando necessário.";

        var messages = new List<ChatMessage>
        {
            ChatMessage.System(systemPrompt),
            ChatMessage.User($"Explique este código {language}:\n\n```{language}\n{code}\n```")
        };

        return await SendMessageAsync(messages, "deepseek-chat", cancellationToken);
    }

    /// <summary>
    /// Gera testes unitários para o código
    /// </summary>
    public async Task<string> GenerateTestsAsync(string code, string language, string testFramework = "", CancellationToken cancellationToken = default)
    {
        var frameworkHint = string.IsNullOrEmpty(testFramework) ? "" : $" usando {testFramework}";
        var systemPrompt = $@"Você é um especialista em testes de software.
Gere testes unitários{frameworkHint} para o código fornecido, incluindo:
1. Testes para o caminho feliz
2. Testes de borda e casos limite
3. Testes de erro/exceção
4. Comentários explicando cada teste

Retorne apenas o código dos testes.";

        var messages = new List<ChatMessage>
        {
            ChatMessage.System(systemPrompt),
            ChatMessage.User($"Gere testes para este código {language}:\n\n```{language}\n{code}\n```")
        };

        return await SendMessageAsync(messages, "deepseek-chat", cancellationToken);
    }

    /// <summary>
    /// Verifica se a API está funcionando
    /// </summary>
    public async Task<bool> TestConnectionAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var messages = new List<ChatMessage> { ChatMessage.User("ping") };
            var response = await SendMessageAsync(messages, "deepseek-chat", cancellationToken);
            return !string.IsNullOrEmpty(response);
        }
        catch
        {
            return false;
        }
    }

    public void Dispose()
    {
        _httpClient.Dispose();
        GC.SuppressFinalize(this);
    }
}
