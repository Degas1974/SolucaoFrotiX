/*
 * ╔══════════════════════════════════════════════════════════════════════════╗
 * ║                                                                          ║
 * ║  📚 DOCUMENTAÇÃO DISPONÍVEL                                              ║
 * ║                                                                          ║
 * ║  Este arquivo está documentado em:                                       ║
 * ║  📄 Documentacao/Services/DocGenerator/ClaudeDocProvider.md              ║
 * ║                                                                          ║
 * ║  Última atualização: 15/01/2026                                          ║
 * ║                                                                          ║
 * ╚══════════════════════════════════════════════════════════════════════════╝
 */

using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
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
    /// Provedor de IA usando Anthropic Claude Messages API
    /// Herda de BaseDocProvider para reutilização de prompts e parsing
    /// </summary>
    public class ClaudeDocProvider : BaseDocProvider, IDocAiProvider
    {
        private readonly HttpClient _httpClient;
        private readonly DocGeneratorSettings _settings;

        public string ProviderName => "Claude";
        public bool IsAvailable => !string.IsNullOrWhiteSpace(_settings.Claude.ApiKey);

        public ClaudeDocProvider(
            HttpClient httpClient,
            IOptions<DocGeneratorSettings> settings,
            ILogger<ClaudeDocProvider> logger) : base(logger)
        {
            try
            {
                _httpClient = httpClient;
                _settings = settings.Value;

                if (IsAvailable)
                {
                    // Garante que o BaseUrl termine com barra para não perder o segmento (ex: v1)
                    var baseUrl = _settings.Claude.BaseUrl.TrimEnd('/') + "/";
                    _httpClient.BaseAddress = new Uri(baseUrl);

                    _httpClient.DefaultRequestHeaders.Add("x-api-key", _settings.Claude.ApiKey);
                    _httpClient.DefaultRequestHeaders.Add("anthropic-version", _settings.Claude.ApiVersion);
                    _httpClient.DefaultRequestHeaders.Accept.Add(
                        new MediaTypeWithQualityHeaderValue("application/json"));
                }
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("ClaudeDocProvider.cs", ".ctor", error);
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
                    result.ErrorMessage = "Claude API key não configurada";
                    return result;
                }

                // Usa o método herdado de BaseDocProvider
                var prompt = BuildPrompt(facts, options);
                var requestBody = BuildRequestBody(prompt, options);

                _logger.LogInformation("Enviando requisição para Claude ({Model}) para {File}",
                    _settings.Claude.Model, facts.File.FileName);

                var response = await SendWithRetryAsync(requestBody, ct);

                if (response.Success)
                {
                    result.Markdown = response.Content;
                    result.Plan = ParseLayoutPlan(response.Content);
                    result.IconMap = ExtractIconMap(response.Content);
                    result.TokensUsed = response.TokensUsed;
                    result.ModelUsed = _settings.Claude.Model;
                    result.Success = true;

                    _logger.LogInformation(
                        "Claude retornou resposta para {File} - Tokens: {Tokens}, MarkdownLen: {MdLen}",
                        facts.File.FileName,
                        result.TokensUsed,
                        result.Markdown?.Length ?? 0);
                }
                else
                {
                    _logger.LogWarning("Claude: Falha para {File}: {Error}", facts.File.FileName, response.ErrorMessage);
                    result.Success = false;
                    result.ErrorMessage = response.ErrorMessage;
                }
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("ClaudeDocProvider.cs", "ComposeAsync", error);
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
                model = _settings.Claude.Model,
                max_tokens = _settings.Claude.MaxTokens,
                temperature = _settings.Claude.Temperature,
                system = GetSystemPrompt(),
                messages = new[]
                {
                    new { role = "user", content = prompt }
                }
            };
        }

        private async Task<(bool Success, string Content, int TokensUsed, string ErrorMessage)> SendWithRetryAsync(
            object requestBody,
            CancellationToken ct)
        {
            var retries = 0;
            var delay = _settings.RetryDelayMs;

            while (retries < _settings.MaxRetries)
            {
                try
                {
                    var json = JsonSerializer.Serialize(requestBody);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    using var cts = CancellationTokenSource.CreateLinkedTokenSource(ct);
                    cts.CancelAfter(TimeSpan.FromSeconds(_settings.TimeoutSeconds));

                    var response = await _httpClient.PostAsync("messages", content, cts.Token);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync(ct);
                        var responseJson = JsonDocument.Parse(responseContent);

                        // Claude retorna content como array de blocos
                        var textContent = new StringBuilder();
                        foreach (var block in responseJson.RootElement.GetProperty("content").EnumerateArray())
                        {
                            if (block.GetProperty("type").GetString() == "text")
                            {
                                textContent.Append(block.GetProperty("text").GetString());
                            }
                        }

                        var tokensUsed = 0;
                        if (responseJson.RootElement.TryGetProperty("usage", out var usage))
                        {
                            tokensUsed = usage.GetProperty("input_tokens").GetInt32() +
                                        usage.GetProperty("output_tokens").GetInt32();
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
