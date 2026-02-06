using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FrotiX.Models;

namespace FrotiX.Services
{
    /// <summary>
    /// Serviço para análise de padrões de erro (placeholder para integração futura com IA)
    /// </summary>
    public class ClaudeAnalysisService : IClaudeAnalysisService
    {
        /// <summary>
        /// Indica se o serviço está configurado (sempre false nesta implementação mock)
        /// </summary>
        public bool IsConfigured => false;

        /// <summary>
        /// Analisa um erro e retorna sugestões de correção (implementação mock)
        /// </summary>
        public async Task<ClaudeAnalysisResult> AnalyzeErrorAsync(LogErro logErro)
        {
            // Implementação mock - aguardando integração real com Claude API
            await Task.CompletedTask;

            if (logErro == null)
            {
                return new ClaudeAnalysisResult
                {
                    Success = false,
                    Error = "LogErro não pode ser null"
                };
            }

            // Análise básica local (sem API)
            var analise = new List<string>();

            if (logErro.Mensagem?.Contains("CORS") == true || logErro.Mensagem?.Contains("cross-origin") == true)
            {
                analise.Add("⚠️ Erro relacionado a CORS detectado");
                analise.Add("💡 Sugestão: Verifique as configurações de CORS no servidor");
            }

            if (logErro.Mensagem?.Contains("Promise") == true)
            {
                analise.Add("⚠️ Promise rejeitada sem tratamento detectada");
                analise.Add("💡 Sugestão: Adicione .catch() ou try/catch ao redor de await");
            }

            if (logErro.Mensagem?.Contains("HTTP") == true)
            {
                analise.Add("⚠️ Erro de requisição HTTP detectado");
                analise.Add("💡 Sugestão: Verifique a conectividade e endpoints da API");
            }

            var analysisText = analise.Any() 
                ? string.Join("\n", analise) 
                : "Nenhum padrão específico detectado. Análise completa requer integração com Claude API.";

            return new ClaudeAnalysisResult
            {
                Success = true,
                Analysis = analysisText,
                Model = "mock-local-analysis",
                InputTokens = 0,
                OutputTokens = 0,
                AnalyzedAt = DateTime.Now
            };
        }
    }
}
