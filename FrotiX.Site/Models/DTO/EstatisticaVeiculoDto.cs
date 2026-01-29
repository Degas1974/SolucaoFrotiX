/*
 * ╔══════════════════════════════════════════════════════════════════════════╗
 * ║  📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                   ║
 * ║  Arquivo: Models/DTO/EstatisticaVeiculoDto.cs                            ║
 * ║  Descrição: DTO com estatísticas de viagens por veículo para validação   ║
 * ║  Data: 29/01/2026 | LOTE: 22                                             ║
 * ╚══════════════════════════════════════════════════════════════════════════╝
 */

using System;

namespace FrotiX.Models.DTO
{
    /// <summary>
    /// DTO com estatísticas de viagens de um veículo para validação inteligente
    /// Usado pela IA evolutiva para calibrar alertas baseados no histórico
    /// </summary>
    public class EstatisticaVeiculoDto
    {
        /// <summary>ID do veículo</summary>
        public Guid VeiculoId { get; set; }

        /// <summary>Placa do veículo</summary>
        public string Placa { get; set; }

        /// <summary>Descrição do veículo (marca/modelo)</summary>
        public string Descricao { get; set; }

        /// <summary>Total de viagens finalizadas no histórico</summary>
        public int TotalViagens { get; set; }

        // ========== ESTATÍSTICAS DE QUILOMETRAGEM ==========

        /// <summary>Média de km rodados por viagem</summary>
        public double KmMedio { get; set; }

        /// <summary>Mediana de km rodados por viagem</summary>
        public double KmMediano { get; set; }

        /// <summary>Desvio padrão de km rodados</summary>
        public double KmDesvioPadrao { get; set; }

        /// <summary>Menor km registrado em uma viagem</summary>
        public int KmMinimo { get; set; }

        /// <summary>Maior km registrado em uma viagem</summary>
        public int KmMaximo { get; set; }

        /// <summary>Percentil 95 de km (95% das viagens ficam abaixo deste valor)</summary>
        public double KmPercentil95 { get; set; }

        /// <summary>Percentil 99 de km (99% das viagens ficam abaixo deste valor)</summary>
        public double KmPercentil99 { get; set; }

        // ========== ESTATÍSTICAS DE DURAÇÃO ==========

        /// <summary>Duração média das viagens em minutos</summary>
        public double DuracaoMediaMinutos { get; set; }

        /// <summary>Duração mediana das viagens em minutos</summary>
        public double DuracaoMedianaMinutos { get; set; }

        /// <summary>Desvio padrão da duração em minutos</summary>
        public double DuracaoDesvioPadraoMinutos { get; set; }

        /// <summary>Menor duração registrada (minutos)</summary>
        public int DuracaoMinimaMinutos { get; set; }

        /// <summary>Maior duração registrada (minutos)</summary>
        public int DuracaoMaximaMinutos { get; set; }

        /// <summary>Percentil 95 de duração (minutos)</summary>
        public double DuracaoPercentil95Minutos { get; set; }

        // ========== METADADOS ==========

        /// <summary>Data da viagem mais antiga considerada</summary>
        public DateTime? DataViagemMaisAntiga { get; set; }

        /// <summary>Data da viagem mais recente considerada</summary>
        public DateTime? DataViagemMaisRecente { get; set; }

        /// <summary>Indica se há dados suficientes para análise estatística confiável (>= 10 viagens)</summary>
        public bool DadosSuficientes => TotalViagens >= 10;

        /// <summary>Indica se há dados mínimos para qualquer análise (>= 3 viagens)</summary>
        public bool DadosMinimos => TotalViagens >= 3;

        /// <summary>
        /// Calcula o Z-Score para um valor de km rodado
        /// Z > 2.5 indica anomalia moderada, Z > 3.5 indica anomalia severa
        /// </summary>
        public double CalcularZScoreKm(int kmRodado)
        {
            if (KmDesvioPadrao <= 0 || TotalViagens < 3) return 0;
            return (kmRodado - KmMedio) / KmDesvioPadrao;
        }

        /// <summary>
        /// Calcula o Z-Score para uma duração em minutos
        /// </summary>
        public double CalcularZScoreDuracao(int duracaoMinutos)
        {
            if (DuracaoDesvioPadraoMinutos <= 0 || TotalViagens < 3) return 0;
            return (duracaoMinutos - DuracaoMediaMinutos) / DuracaoDesvioPadraoMinutos;
        }

        /// <summary>
        /// Verifica se um km rodado está dentro do padrão esperado
        /// </summary>
        public NivelAnomalia ClassificarKm(int kmRodado)
        {
            if (!DadosMinimos) return NivelAnomalia.SemDados;

            var zScore = Math.Abs(CalcularZScoreKm(kmRodado));

            if (zScore > 3.5) return NivelAnomalia.Severa;
            if (zScore > 2.5) return NivelAnomalia.Moderada;
            if (zScore > 1.5) return NivelAnomalia.Leve;
            return NivelAnomalia.Normal;
        }

        /// <summary>
        /// Verifica se uma duração está dentro do padrão esperado
        /// </summary>
        public NivelAnomalia ClassificarDuracao(int duracaoMinutos)
        {
            if (!DadosMinimos) return NivelAnomalia.SemDados;

            var zScore = Math.Abs(CalcularZScoreDuracao(duracaoMinutos));

            if (zScore > 3.5) return NivelAnomalia.Severa;
            if (zScore > 2.5) return NivelAnomalia.Moderada;
            if (zScore > 1.5) return NivelAnomalia.Leve;
            return NivelAnomalia.Normal;
        }
    }

    /// <summary>
    /// Níveis de anomalia para classificação de valores
    /// </summary>
    public enum NivelAnomalia
    {
        /// <summary>Não há dados suficientes para análise</summary>
        SemDados = 0,

        /// <summary>Valor dentro do padrão esperado</summary>
        Normal = 1,

        /// <summary>Valor ligeiramente acima do esperado (Z-Score 1.5-2.5)</summary>
        Leve = 2,

        /// <summary>Valor moderadamente acima do esperado (Z-Score 2.5-3.5) - requer confirmação</summary>
        Moderada = 3,

        /// <summary>Valor severamente acima do esperado (Z-Score > 3.5) - requer justificativa</summary>
        Severa = 4
    }
}
