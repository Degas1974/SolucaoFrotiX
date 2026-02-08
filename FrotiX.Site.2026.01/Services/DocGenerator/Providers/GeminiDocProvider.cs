/*
 * ╔══════════════════════════════════════════════════════════════════════════╗
 * ║                                                                          ║
 * ║  📚 DOCUMENTAÇÃO DISPONÍVEL                                              ║
 * ║                                                                          ║
 * ║  Este arquivo está documentado em:                                       ║
 * ║  📄 Documentacao/Services/DocGenerator/GeminiDocProvider.md              ║
 * ║                                                                          ║
 * ║  Última atualização: 15/01/2026                                          ║
 * ║                                                                          ║
 * ╚══════════════════════════════════════════════════════════════════════════╝
 */

using System;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using FrotiX.Services.DocGenerator.Interfaces;
using FrotiX.Services.DocGenerator.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FrotiX.Services.DocGenerator.Providers
{
    /// <summary>
    /// Provedor de IA usando Google Gemini generateContent API
    /// Herda de BaseDocProvider para reutilização de prompts e parsing
    /// </summary>
    public class GeminiDocProvider : BaseDocProvider, IDocAiProvider
    {
        private readonly HttpClient _httpClient;
        private readonly DocGeneratorSettings _settings;

        public string ProviderName => "Gemini";
        public bool IsAvailable => !string.IsNullOrWhiteSpace(_settings.Gemini.ApiKey);

        public GeminiDocProvider(
            HttpClient httpClient,
            IOptions<DocGeneratorSettings> settings,
            ILogger<GeminiDocProvider> logger) : base(logger)
        {
            try
            {
                _httpClient = httpClient;
                _settings = settings.Value;

                if (IsAvailable)
                {
                    // Garante que o BaseUrl termine com barra para não perder o segmento (ex: v1beta)
                    var baseUrl = _settings.Gemini.BaseUrl.TrimEnd('/') + "/";
                    _httpClient.BaseAddress = new Uri(baseUrl);
                }
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("GeminiDocProvider.cs", ".ctor", error);
            }
        }

        public async Task<AiComposeResult> ComposeAsync(
            DocumentFacts facts,
            DocGenerationOptions options,
            CancellationToken ct = default)
        {
            var result = new AiComposeResult();
            var sw = Stopwatch.StartNew();

            try
            {
                if (!IsAvailable)
                {
                    result.Success = false;
                    result.ErrorMessage = "Gemini API key não configurada";
                    return result;
                }

                // Usa o método herdado de BaseDocProvider
                var prompt = BuildPrompt(facts, options);
                var requestBody = BuildRequestBody(prompt, options);

                _logger.LogInformation("Enviando requisição para Gemini ({Model}) para {File}",
                    _settings.Gemini.Model, facts.File.FileName);

                var response = await SendWithRetryAsync(requestBody, ct);

                if (response.Success)
                {
                    result.Markdown = response.Content;
                    result.Plan = ParseLayoutPlan(response.Content);
                    result.IconMap = ExtractIconMap(response.Content);
                    result.TokensUsed = response.TokensUsed;
                    result.ModelUsed = _settings.Gemini.Model;
                    result.Success = true;

                    _logger.LogInformation(
                        "Gemini retornou resposta para {File} - Tokens: {Tokens}, MarkdownLen: {MdLen}",
                        facts.File.FileName,
                        result.TokensUsed,
                        result.Markdown?.Length ?? 0);
                }
                else
                {
                    _logger.LogWarning("Gemini: Falha para {File}: {Error}", facts.File.FileName, response.ErrorMessage);
                    result.Success = false;
                    result.ErrorMessage = response.ErrorMessage;
                }
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("GeminiDocProvider.cs", "ComposeAsync", error);
                result.Success = false;
                result.ErrorMessage = error.Message;
            }

            sw.Stop();
            result.ProcessingTime = sw.Elapsed;
            return result;
        }

        private object BuildRequestBody(string prompt, DocGenerationOptions options)
        {
            return new
            {
                contents = new[]
                {
                    new
                    {
                        role = "user",
                        parts = new[]
                        {
                            new { text = prompt }
                        }
                    }
                },
                generationConfig = new
                {
                    temperature = _settings.Gemini.Temperature,
                    maxOutputTokens = _settings.Gemini.MaxTokens,
                    topP = 0.95,
                    topK = 64
                },
                systemInstruction = new
                {
                    parts = new[]
                    {
                        new { text = GetSystemPrompt() }
                    }
                }
            };
        }

        private async Task<(bool Success, string Content, int TokensUsed, string ErrorMessage)> SendWithRetryAsync(
            object requestBody,
            CancellationToken ct)
        {
            var retries = 0;
            var delay = _settings.RetryDelayMs;

            // URL com API key como query parameter (padrão Gemini)
            var url = $"models/{_settings.Gemini.Model}:generateContent?key={_settings.Gemini.ApiKey}";

            while (retries < _settings.MaxRetries)
            {
                try
                {
                    var json = JsonSerializer.Serialize(requestBody);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    using var cts = CancellationTokenSource.CreateLinkedTokenSource(ct);
                    cts.CancelAfter(TimeSpan.FromSeconds(_settings.TimeoutSeconds));

                    var response = await _httpClient.PostAsync(url, content, cts.Token);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync(ct);
                        var responseJson = JsonDocument.Parse(responseContent);

                        // Gemini retorna candidates[0].content.parts[0].text
                        var textContent = new StringBuilder();
                        if (responseJson.RootElement.TryGetProperty("candidates", out var candidates))
                        {
                            if (candidates.GetArrayLength() > 0)
                            {
                                var firstCandidate = candidates[0];
                                if (firstCandidate.TryGetProperty("content", out var candidateContent))
                                {
                                    if (candidateContent.TryGetProperty("parts", out var parts))
                                    {
                                        foreach (var part in parts.EnumerateArray())
                                        {
                                            if (part.TryGetProperty("text", out var text))
                                            {
                                                textContent.Append(text.GetString());
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            // Verificar se há erro na resposta
                            if (responseJson.RootElement.TryGetProperty("error", out var errorProp))
                            {
                                var errorMsg = errorProp.GetRawText();
                                _logger.LogError("Gemini API: Erro na resposta: {Error}", errorMsg);
                                return (false, string.Empty, 0, errorMsg);
                            }
                        }

                        var tokensUsed = 0;
                        if (responseJson.RootElement.TryGetProperty("usageMetadata", out var usage))
                        {
                            if (usage.TryGetProperty("totalTokenCount", out var total))
                            {
                                tokensUsed = total.GetInt32();
                            }
                        }

                        return (true, textContent.ToString(), tokensUsed, string.Empty);
                    }

                    if ((int)response.StatusCode == 429 || (int)response.StatusCode >= 500)
                    {
                        retries++;
                        _logger.LogWarning("Retry {Retry}/{Max} após status {Status}",
                            retries, _settings.MaxRetries, response.StatusCode);
                        await Task.Delay(delay, ct);
                        delay *= 2;
                        continue;
                    }

                    var errorContent = await response.Content.ReadAsStringAsync(ct);
                    return (false, string.Empty, 0, $"HTTP {response.StatusCode}: {errorContent}");
                }
                catch (TaskCanceledException) when (!ct.IsCancellationRequested)
                {
                    retries++;
                    _logger.LogWarning("Timeout, retry {Retry}/{Max}", retries, _settings.MaxRetries);
                    await Task.Delay(delay, ct);
                    delay *= 2;
                }
                catch (Exception ex)
                {
                    return (false, string.Empty, 0, ex.Message);
                }
            }

            return (false, string.Empty, 0, $"Falha após {_settings.MaxRetries} tentativas");
        }
    }
}
