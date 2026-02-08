/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: IClaudeAnalysisService.cs                                                               ║
   ║ 📂 CAMINHO: /Services                                                                               ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO: Interface do serviço de análise de erros usando Claude AI.                            ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 MÉTODOS: AnalyzeErrorAsync - Analisa erro e retorna sugestões de correção.                      ║
   ║ 🔗 DEPS: FrotiX.Models.LogErro | 📅 31/01/2026 | 👤 Claude Code | 📝 v1.0                          ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝
*/

using System.Threading.Tasks;
using FrotiX.Models;

namespace FrotiX.Services;

/// <summary>
/// Interface do serviço de análise de erros com Claude AI
/// </summary>
public interface IClaudeAnalysisService
{
    /// <summary>
    /// Analisa um erro e retorna sugestões de correção
    /// </summary>
    /// <param name="logErro">Log de erro a ser analisado</param>
    /// <returns>Resultado da análise com sugestões</returns>
    Task<ClaudeAnalysisResult> AnalyzeErrorAsync(LogErro logErro);

    /// <summary>
    /// Verifica se o serviço está configurado e disponível
    /// </summary>
    bool IsConfigured { get; }
}

/// <summary>
/// Resultado da análise de erro pelo Claude AI
/// </summary>
public class ClaudeAnalysisResult
{
    /// <summary>Indica se a análise foi bem-sucedida</summary>
    public bool Success { get; set; }

    /// <summary>Mensagem de erro (se houver)</summary>
    public string? Error { get; set; }

    /// <summary>Análise completa do Claude</summary>
    public string? Analysis { get; set; }

    /// <summary>Modelo utilizado na análise</summary>
    public string? Model { get; set; }

    /// <summary>Tokens de entrada utilizados</summary>
    public int InputTokens { get; set; }

    /// <summary>Tokens de saída gerados</summary>
    public int OutputTokens { get; set; }

    /// <summary>Data/hora da análise</summary>
    public System.DateTime AnalyzedAt { get; set; } = System.DateTime.Now;
}

/// <summary>
/// Configurações do Claude AI
/// </summary>
public class ClaudeAISettings
{
    /// <summary>API Key do Claude</summary>
    public string ApiKey { get; set; } = "";

    /// <summary>Modelo a ser utilizado (ex: claude-sonnet-4-20250514)</summary>
    public string Model { get; set; } = "claude-sonnet-4-20250514";

    /// <summary>Máximo de tokens na resposta</summary>
    public int MaxTokens { get; set; } = 4096;

    /// <summary>Temperatura (0-1, menor = mais determinístico)</summary>
    public double Temperature { get; set; } = 0.3;

    /// <summary>Prompt do sistema para análise de erros</summary>
    public string SystemPrompt { get; set; } = "Você é um especialista em análise de erros de software.";
}
