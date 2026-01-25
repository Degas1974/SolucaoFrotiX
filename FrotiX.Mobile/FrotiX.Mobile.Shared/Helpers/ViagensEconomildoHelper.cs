using System;

namespace FrotiX.Mobile.Shared.Helpers
{
    /// <summary>
    /// Helper para determinar valores corretos de MOB e IdaVolta
    /// baseado na linha e trajeto selecionados
    /// </summary>
    public static class ViagensEconomildoHelper
    {
        /// <summary>
        /// Determina o MOB e IdaVolta corretos baseado na linha e trajeto
        /// </summary>
        /// <param name="linha">Linha selecionada (ex: "Cefor", "PGR", "Rodoviária")</param>
        /// <param name="trajeto">Trajeto/Destino selecionado (ex: "Destino Câmara", "Destino Cefor")</param>
        /// <returns>Tupla com (MOB, IdaVolta)</returns>
        public static (string MOB, string IdaVolta) DeterminarMobEIdaVolta(string linha , string trajeto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(linha) || string.IsNullOrWhiteSpace(trajeto))
                {
                    throw new ArgumentException("Linha e trajeto não podem ser vazios");
                }

                // Normaliza strings para comparação case-insensitive
                linha = linha.Trim();
                trajeto = trajeto.Trim();

                // ========== LINHA: CEFOR ==========
                if (linha.Equals("Economildo Cefor" , StringComparison.OrdinalIgnoreCase))
                {
                    if (trajeto.Contains("Cefor" , StringComparison.OrdinalIgnoreCase))
                    {
                        // Destino Cefor → MOB = Cefor | IdaVolta = IDA
                        return ("Cefor", "IDA");
                    }
                    else if (trajeto.Contains("Chapelaria" , StringComparison.OrdinalIgnoreCase))
                    {
                        // Destino Chapelaria → MOB = Cefor | IdaVolta = VOLTA
                        return ("Cefor", "VOLTA");
                    }
                }

                // ========== LINHA: PGR ==========
                else if (linha.Equals("Economildo PGR" , StringComparison.OrdinalIgnoreCase))
                {
                    if (trajeto.Contains("PGR" , StringComparison.OrdinalIgnoreCase))
                    {
                        // Destino PGR → MOB = PGR | IdaVolta = IDA
                        return ("PGR", "IDA");
                    }
                    else if (trajeto.Contains("Anexo II" , StringComparison.OrdinalIgnoreCase) ||
                             trajeto.Contains("AnexoII" , StringComparison.OrdinalIgnoreCase) ||
                             trajeto.Contains("Anexo 2" , StringComparison.OrdinalIgnoreCase))
                    {
                        // Destino Anexo II → MOB = PGR | IdaVolta = VOLTA
                        return ("PGR", "VOLTA");
                    }
                }

                // ========== LINHA: RODOVIÁRIA ==========
                else if (linha.Equals("Economildo Rodoviária" , StringComparison.OrdinalIgnoreCase) ||
                         linha.Equals("Economildo Rodoviaria" , StringComparison.OrdinalIgnoreCase))
                {
                    if (trajeto.Contains("Rodoviária" , StringComparison.OrdinalIgnoreCase) ||
                        trajeto.Contains("Rodoviaria" , StringComparison.OrdinalIgnoreCase))
                    {
                        // Destino Rodoviária → MOB = RODOVIARIA | IdaVolta = IDA
                        return ("Rodoviária", "IDA");
                    }
                    else if (trajeto.Contains("Câmara" , StringComparison.OrdinalIgnoreCase) ||
                             trajeto.Contains("Camara" , StringComparison.OrdinalIgnoreCase))
                    {
                        // Destino Câmara → MOB = RODOVIARIA | IdaVolta = VOLTA
                        return ("Rodoviária", "VOLTA");
                    }
                }

                // Se não encontrou regra, retorna valores padrão
                System.Diagnostics.Debug.WriteLine($"⚠️ AVISO: Combinação não mapeada - Linha: '{linha}', Trajeto: '{trajeto}'");
                return (linha, "IDA"); // Fallback: mantém linha original e assume IDA
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ ERRO ao determinar MOB/IdaVolta: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Valida se a combinação de linha e trajeto é válida
        /// </summary>
        public static bool ValidarLinhaEtrajeto(string linha , string trajeto)
        {
            try
            {
                var (mob, idaVolta) = DeterminarMobEIdaVolta(linha , trajeto);
                return !string.IsNullOrEmpty(mob) && !string.IsNullOrEmpty(idaVolta);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Obtém descrição da regra aplicada (para debug/log)
        /// </summary>
        public static string ObterDescricaoRegra(string linha , string trajeto)
        {
            var (mob, idaVolta) = DeterminarMobEIdaVolta(linha , trajeto);
            return $"Linha: {linha} | Trajeto: {trajeto} → MOB: {mob} | Ida/Volta: {idaVolta}";
        }
    }
}