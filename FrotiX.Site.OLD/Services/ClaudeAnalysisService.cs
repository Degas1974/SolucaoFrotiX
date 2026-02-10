/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: ClaudeAnalysisService.cs                                                                ║
   ║ 📂 CAMINHO: /Services                                                                               ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO: Serviço de análise de erros usando Claude AI (Anthropic API).                         ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 FUNCIONALIDADES:                                                                                 ║
   ║ • Envia detalhes do erro para Claude AI via API                                                    ║
   ║ • Recebe diagnóstico, sugestões de correção e prevenção                                            ║
   ║ • Formata resposta em Markdown para exibição                                                       ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🔗 DEPS: HttpClient, IConfiguration | 📅 31/01/2026 | 👤 Claude Code | 📝 v1.0                     ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝
*/

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

/// <summary>
/// Implementação do serviço de análise de erros com Claude AI
/// </summary>
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

        // Carrega configurações
        _settings = new ClaudeAISettings();
        configuration.GetSection("ClaudeAI").Bind(_settings);

        // Configura headers padrão
        if (!string.IsNullOrEmpty(_settings.ApiKey))
        {
            _httpClient.DefaultRequestHeaders.Add("x-api-key", _settings.ApiKey);
            _httpClient.DefaultRequestHeaders.Add("anthropic-version", ANTHROPIC_VERSION);
        }
    }

    /// <summary>
    /// Verifica se o serviço está configurado
    /// </summary>
    public bool IsConfigured => !string.IsNullOrEmpty(_settings.ApiKey) &&
                                 _settings.ApiKey != "COLE_SUA_API_KEY_AQUI";

    /// <summary>
    /// Analisa um erro e retorna sugestões de correção
    /// </summary>
    /***********************************************************************************
     * ⚡ FUNÇÃO: AnalyzeErrorAsync
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Enviar erro para Claude AI e retornar diagnóstico com sugestões
     *                   de correção e prevenção em Markdown
     *
     * 📥 ENTRADAS     : logErro [LogErro] - Detalhes do erro do sistema
     *
     * 📤 SAÍDAS       : Task<ClaudeAnalysisResult> - Resultado com análise ou erro
     *
     * ⬅️ CHAMADO POR  : LogErrosAlertService, Controllers de erro
     *
     * ➡️ CHAMA        : IsConfigured [check]
     *                   BuildErrorContext() [linha 83]
     *                   _httpClient.PostAsync() [API Anthropic]
     *                   ParseErrorMessage() [linha 122]
     *
     * 📝 OBSERVAÇÕES  : Enriquece com tokens de uso. Trata 3 tipos de erro: config,
     *                   API falha e processamento. Try-catch abrangente.
     ***********************************************************************************/
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
            // Monta o prompt com os dados do erro
            var errorContext = BuildErrorContext(logErro);

            // Prepara a requisição
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

            // Parse da resposta
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

            // Extrai o texto da resposta
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

    /// <summary>
    /// Monta o contexto do erro para enviar ao Claude
    /// </summary>
    /***********************************************************************************
     * ⚡ FUNÇÃO: BuildErrorContext
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Formatar dados do erro em Markdown estruturado para envio a Claude.
     *                   Inclui stack trace, inner exception, contexto HTTP
     *
     * 📥 ENTRADAS     : logErro [LogErro] - Objeto do banco com erro completo
     *
     * 📤 SAÍDAS       : string - Markdown formatado com seções estruturadas
     *
     * ⬅️ CHAMADO POR  : AnalyzeErrorAsync() [linha 102]
     *
     * ➡️ CHAMA        : TruncateIfNeeded() [linhas 236, 245, 254]
     *                   StringBuilder.AppendLine() [múltiplas vezes]
     *
     * 📝 OBSERVAÇÕES  : Constrói prompt completo em Markdown. Trunca stack trace (3000),
     *                   inner exception (1500), dados adicionais (1500) para evitar
     *                   token limit da API Claude. Inclui instruções finais para análise.
     ***********************************************************************************/
    private string BuildErrorContext(LogErro logErro)
    {
        var sb = new StringBuilder();

        // [DADOS] Início do documento em Markdown
        sb.AppendLine("# Erro para Análise");
        sb.AppendLine();

        // [UI] Seção informações básicas: tipo, origem, nível, data-hora
        sb.AppendLine("## Informações Básicas");
        sb.AppendLine($"- **Tipo:** {logErro.Tipo}");
        sb.AppendLine($"- **Origem:** {logErro.Origem}");
        sb.AppendLine($"- **Nível:** {logErro.Nivel ?? "N/A"}");
        sb.AppendLine($"- **Data/Hora:** {logErro.DataHora:dd/MM/yyyy HH:mm:ss}");
        sb.AppendLine();

        // [DADOS] Localização do erro (arquivo, método, linha, coluna, URL)
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

        // [DEBUG] Mensagem de erro principal
        sb.AppendLine("## Mensagem de Erro");
        sb.AppendLine("```");
        sb.AppendLine(logErro.Mensagem);
        sb.AppendLine("```");
        sb.AppendLine();

        // [DEBUG] Tipo e mensagem da exceção C#
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

        // [DEBUG] Stack trace truncado para evitar limite de tokens
        if (!string.IsNullOrEmpty(logErro.StackTrace))
        {
            sb.AppendLine("## Stack Trace");
            sb.AppendLine("```");
            sb.AppendLine(TruncateIfNeeded(logErro.StackTrace, 3000));
            sb.AppendLine("```");
            sb.AppendLine();
        }

        // [DEBUG] Inner exception também truncada
        if (!string.IsNullOrEmpty(logErro.InnerException))
        {
            sb.AppendLine("## Inner Exception");
            sb.AppendLine("```");
            sb.AppendLine(TruncateIfNeeded(logErro.InnerException, 1500));
            sb.AppendLine("```");
            sb.AppendLine();
        }

        // [DATA] Dados adicionais em JSON, truncado para evitar limite
        if (!string.IsNullOrEmpty(logErro.DadosAdicionais))
        {
            sb.AppendLine("## Dados Adicionais (JSON)");
            sb.AppendLine("```json");
            sb.AppendLine(TruncateIfNeeded(logErro.DadosAdicionais, 1500));
            sb.AppendLine("```");
            sb.AppendLine();
        }

        // [DADOS] Contexto HTTP e informações do usuário
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

    /// <summary>
    /// Trunca texto se exceder o limite
    /// </summary>
    /***********************************************************************************
     * ⚡ FUNÇÃO: TruncateIfNeeded
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Limitar tamanho de strings para evitar exceder token limit
     *                   da API Claude (stack traces, inner exceptions grandes)
     *
     * 📥 ENTRADAS     : text [string] - Texto a desempenhar truncagem
     *                   maxLength [int] - Limite máximo de caracteres permitido
     *
     * 📤 SAÍDAS       : string - Texto original ou truncado + nota "[truncado]"
     *
     * ⬅️ CHAMADO POR  : BuildErrorContext() [linhas 282, 292, 302]
     *
     * ➡️ CHAMA        : string.IsNullOrEmpty(), string.Substring()
     *
     * 📝 OBSERVAÇÕES  : Simples e direto. String null/vazia retorna como-é.
     *                   Truncado recebe aviso "[truncado]" em nova linha.
     ***********************************************************************************/
    private string TruncateIfNeeded(string text, int maxLength)
    {
        if (string.IsNullOrEmpty(text) || text.Length <= maxLength)
            return text;

        return text.Substring(0, maxLength) + "\n... [truncado]";
    }

    /// <summary>
    /// Extrai mensagem de erro da resposta da API
    /// </summary>
    /***********************************************************************************
     * ⚡ FUNÇÃO: ParseErrorMessage
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Extrair mensagem de erro da resposta JSON da API Anthropic
     *                   para exibição legível ao usuário
     *
     * 📥 ENTRADAS     : responseBody [string] - JSON da resposta (possivelmente com erro)
     *
     * 📤 SAÍDAS       : string - Mensagem legível ou fallback para todo responseBody
     *
     * ⬅️ CHAMADO POR  : AnalyzeErrorAsync() [linha 122]
     *
     * ➡️ CHAMA        : JsonDocument.Parse() [standard .NET JSON]
     *
     * 📝 OBSERVAÇÕES  : Try-catch interno ignora erros de parsing JSON.
     *                   Fallback é retornar todo responseBody se não conseguir extrair.
     ***********************************************************************************/
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

// ====== DTOs para API Claude ======

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
