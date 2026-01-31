/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 📌 ARQUIVO: EstatisticaVeiculoDto.cs                                                                ║
   ║ 📂 CAMINHO: /Models/DTO                                                                             ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🧭 OBJETIVO: Estatísticas de viagens por veículo para validação inteligente.                       ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🗂️  CONTÉM: EstatisticaVeiculoDto, NivelAnomalia                                                    ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🔗 DEPENDÊNCIAS: System                                                                             ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝ */

using System;

namespace FrotiX.Models.DTO
{
    // ==================================================================================================
    // DTO
    // ==================================================================================================
    // Estatísticas de viagens para validação inteligente e calibração de alertas.
    // ==================================================================================================
    public class EstatisticaVeiculoDto
    {
        // ID do veículo.
        public Guid VeiculoId { get; set; }

        // Placa do veículo.
        public string Placa { get; set; }

        // Descrição do veículo (marca/modelo).
        public string Descricao { get; set; }

        // Total de viagens finalizadas.
        public int TotalViagens { get; set; }

        // ========== ESTATÍSTICAS DE QUILOMETRAGEM ==========

        // Média de km por viagem.
        public double KmMedio { get; set; }

        // Mediana de km por viagem.
        public double KmMediano { get; set; }

        // Desvio padrão de km.
        public double KmDesvioPadrao { get; set; }

        // Menor km registrado.
        public int KmMinimo { get; set; }

        // Maior km registrado.
        public int KmMaximo { get; set; }

        // Percentil 95 de km.
        public double KmPercentil95 { get; set; }

        // Percentil 99 de km.
        public double KmPercentil99 { get; set; }

        // ========== ESTATÍSTICAS DE DURAÇÃO ==========

        // Duração média (minutos).
        public double DuracaoMediaMinutos { get; set; }

        // Duração mediana (minutos).
        public double DuracaoMedianaMinutos { get; set; }

        // Desvio padrão da duração (minutos).
        public double DuracaoDesvioPadraoMinutos { get; set; }

        // Menor duração registrada (minutos).
        public int DuracaoMinimaMinutos { get; set; }

        // Maior duração registrada (minutos).
        public int DuracaoMaximaMinutos { get; set; }

        // Percentil 95 de duração (minutos).
        public double DuracaoPercentil95Minutos { get; set; }

        // ========== METADADOS ==========

        // Data da viagem mais antiga considerada.
        public DateTime? DataViagemMaisAntiga { get; set; }

        // Data da viagem mais recente considerada.
        public DateTime? DataViagemMaisRecente { get; set; }

        // Indica se há dados suficientes (>= 10 viagens).
        public bool DadosSuficientes => TotalViagens >= 10;

        // Indica se há dados mínimos (>= 3 viagens).
        public bool DadosMinimos => TotalViagens >= 3;

        // Calcula Z-Score para km rodado.
        public double CalcularZScoreKm(int kmRodado)
        {
            if (KmDesvioPadrao <= 0 || TotalViagens < 3) return 0;
            return (kmRodado - KmMedio) / KmDesvioPadrao;
        }

        // Calcula Z-Score para duração em minutos.
        public double CalcularZScoreDuracao(int duracaoMinutos)
        {
            if (DuracaoDesvioPadraoMinutos <= 0 || TotalViagens < 3) return 0;
            return (duracaoMinutos - DuracaoMediaMinutos) / DuracaoDesvioPadraoMinutos;
        }

        // Classifica km rodado conforme padrão esperado.
        public NivelAnomalia ClassificarKm(int kmRodado)
        {
            if (!DadosMinimos) return NivelAnomalia.SemDados;

            var zScore = Math.Abs(CalcularZScoreKm(kmRodado));

            if (zScore > 3.5) return NivelAnomalia.Severa;
            if (zScore > 2.5) return NivelAnomalia.Moderada;
            if (zScore > 1.5) return NivelAnomalia.Leve;
            return NivelAnomalia.Normal;
        }

        // Classifica duração conforme padrão esperado.
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

    // Níveis de anomalia para classificação de valores.
    public enum NivelAnomalia
    {
        // Não há dados suficientes.
        SemDados = 0,

        // Valor dentro do padrão esperado.
        Normal = 1,

        // Valor ligeiramente acima do esperado.
        Leve = 2,

        // Valor moderadamente acima do esperado.
        Moderada = 3,

        // Valor severamente acima do esperado.
        Severa = 4
    }
}
