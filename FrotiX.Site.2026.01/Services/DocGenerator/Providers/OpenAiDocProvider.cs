/*
 * ╔══════════════════════════════════════════════════════════════════════════╗
 * ║                                                                          ║
 * ║  📚 DOCUMENTAÇÃO DISPONÍVEL                                              ║
 * ║                                                                          ║
 * ║  Este arquivo está documentado em:                                       ║
 * ║  📄 Documentacao/Services/DocGenerator/OpenAiDocProvider.md              ║
 * ║                                                                          ║
 * ║  Última atualização: 15/01/2026                                          ║
 * ║                                                                          ║
 * ╚══════════════════════════════════════════════════════════════════════════╝
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using FrotiX.Services.DocGenerator.Interfaces;
using FrotiX.Services.DocGenerator.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenAI.Chat;

namespace FrotiX.Services.DocGenerator.Providers
{
    /// <summary>
    /// Provedor de IA usando OpenAI SDK oficial (.NET)
    /// Utiliza o ChatClient do pacote NuGet OpenAI
    /// Herda de BaseDocProvider para reutilização de prompts e parsing
    /// </summary>
    public class OpenAiDocProvider : BaseDocProvider, IDocAiProvider
    {
        private readonly DocGeneratorSettings _settings;
        private ChatClient? _chatClient;

        public string ProviderName => "OpenAI";
        public bool IsAvailable => _settings.OpenAi.IsConfigured;

        public OpenAiDocProvider(
            IOptions<DocGeneratorSettings> options,
            ILogger<OpenAiDocProvider> logger) : base(logger)
        {
            try
            {
                _settings = options.Value;

                // Inicializa o ChatClient se a API Key estiver configurada
                if (IsAvailable)
                {
                    _chatClient = new ChatClient(
                        model: _settings.OpenAi.Model,
                        apiKey: _settings.OpenAi.ApiKey
                    );

                    _logger.LogInformation(
                        "OpenAI ChatClient inicializado com modelo {Model}",
                        _settings.OpenAi.Model);
                }
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("OpenAiDocProvider.cs", ".ctor", error);
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
                if (!IsAvailable || _chatClient == null)
                {
                    result.Success = false;
                    result.ErrorMessage = "OpenAI API key não configurada. Configure em appsettings.json ou via variável de ambiente DOCGENERATOR_OPENAI_APIKEY";
                    return result;
                }

                // Usa o método herdado de BaseDocProvider
                var prompt = BuildPrompt(facts, options);

                _logger.LogInformation(
                    "Enviando requisição para OpenAI ({Model}) para {File}",
                    _settings.OpenAi.Model,
                    facts.File.FileName);

                // Monta as mensagens para o chat
                var messages = new List<ChatMessage>
                {
                    new SystemChatMessage(GetSystemPrompt()),
                    new UserChatMessage(prompt)
                };

                // Configurações da requisição
                var chatOptions = new ChatCompletionOptions
                {
                    MaxOutputTokenCount = _settings.OpenAi.MaxTokens,
                    Temperature = (float)_settings.OpenAi.Temperature
                };

                // Faz a chamada assíncrona
                ChatCompletion completion = await _chatClient.CompleteChatAsync(messages, chatOptions, ct);

                if (completion != null && completion.Content.Count > 0)
                {
                    var responseText = completion.Content[0].Text;

                    result.Markdown = responseText;
                    result.Plan = ParseLayoutPlan(responseText);
                    result.IconMap = ExtractIconMap(responseText);
                    result.TokensUsed = completion.Usage?.TotalTokenCount ?? 0;
                    result.ModelUsed = _settings.OpenAi.Model;
                    result.Success = true;

                    _logger.LogInformation(
                        "OpenAI retornou resposta para {File} - Tokens: {Tokens}, MarkdownLen: {MdLen}",
                        facts.File.FileName,
                        result.TokensUsed,
                        result.Markdown?.Length ?? 0);
                }
                else
                {
                    _logger.LogWarning("OpenAI: Resposta vazia ou nula para {File}", facts.File.FileName);
                    result.Success = false;
                    result.ErrorMessage = "OpenAI retornou resposta vazia";
                }
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("OpenAiDocProvider.cs", "ComposeAsync", error);
                result.Success = false;
                result.ErrorMessage = error.Message;
            }

            sw.Stop();
            result.ProcessingTime = sw.Elapsed;
            return result;
        }
    }
}
