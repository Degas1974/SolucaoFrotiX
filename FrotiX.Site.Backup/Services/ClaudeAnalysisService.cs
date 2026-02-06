using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrotiX.Site.Services
{
    /// <summary>
    /// Serviço para análise de padrões de erro (placeholder para integração futura com IA)
    /// </summary>
    public class ClaudeAnalysisService : IClaudeAnalysisService
    {
        public async Task<string> AnalisarPadraoAsync(List<string> erros)
        {
            // Implementação futura: enviar para Claude API
            // Por enquanto, análise simples local

            if (erros == null || !erros.Any())
            {
                return "Nenhum erro para analisar";
            }

            var analise = new List<string>();

            // Contar erros de CORS
            var errosCors = erros.Count(e => e.Contains("CORS") || e.Contains("cross-origin"));
            if (errosCors > 0)
            {
                analise.Add($"⚠️ {errosCors} erro(s) relacionados a CORS detectados");
            }

            // Contar Promise rejeitadas
            var promisesRejeitadas = erros.Count(e => e.Contains("Promise rejeitada"));
            if (promisesRejeitadas > 0)
            {
                analise.Add($"⚠️ {promisesRejeitadas} Promise(s) rejeitada(s) sem tratamento");
            }

            // Contar erros HTTP
            var errosHttp = erros.Count(e => e.Contains("HTTP"));
            if (errosHttp > 0)
            {
                analise.Add($"⚠️ {errosHttp} erro(s) de requisição HTTP");
            }

            return analise.Any() 
                ? string.Join("\n", analise) 
                : "Nenhum padrão específico detectado";
        }

        public async Task<Dictionary<string, int>> ClassificarErrosAsync(List<string> erros)
        {
            var classificacao = new Dictionary<string, int>
            {
                ["CORS"] = 0,
                ["Promise"] = 0,
                ["HTTP"] = 0,
                ["Runtime"] = 0,
                ["Outros"] = 0
            };

            foreach (var erro in erros)
            {
                if (erro.Contains("CORS") || erro.Contains("cross-origin"))
                {
                    classificacao["CORS"]++;
                }
                else if (erro.Contains("Promise"))
                {
                    classificacao["Promise"]++;
                }
                else if (erro.Contains("HTTP"))
                {
                    classificacao["HTTP"]++;
                }
                else if (erro.Contains("ReferenceError") || erro.Contains("TypeError"))
                {
                    classificacao["Runtime"]++;
                }
                else
                {
                    classificacao["Outros"]++;
                }
            }

            return await Task.FromResult(classificacao);
        }
    }
}
