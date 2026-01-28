// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ ARQUIVO: EstatisticaAbastecimentoVeiculoMensal.cs                           ║
// ║ PROJETO: FrotiX - Sistema de Gestão de Frotas                               ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO:                                                                   ║
// ║ Estatísticas mensais de abastecimentos por veículo.                         ║
// ║ Granularidade menor para análise de tendências.                             ║
// ║                                                                              ║
// ║ CAMPOS:                                                                      ║
// ║ - Id, Ano, Mes, VeiculoId: Identificação                                    ║
// ║ - TotalAbastecimentos, ValorTotal, LitrosTotal                              ║
// ║ - DataAtualizacao: Timestamp do cache                                       ║
// ║                                                                              ║
// ║ USO: Gráfico de evolução mensal por veículo                                 ║
// ║                                                                              ║
// ║ DOCUMENTADO EM: 2026-01-28 | LOTE: 16                                       ║
// ╚══════════════════════════════════════════════════════════════════════════════╝

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FrotiX.Models.Estatisticas
{
    [Table("EstatisticaAbastecimentoVeiculoMensal")]
    public class EstatisticaAbastecimentoVeiculoMensal
    {
        [Key]
        public Guid Id { get; set; }

        public int Ano { get; set; }

        public int Mes { get; set; }

        public Guid VeiculoId { get; set; }

        public int TotalAbastecimentos { get; set; }

        public decimal? ValorTotal { get; set; }

        public decimal? LitrosTotal { get; set; }

        public DateTime DataAtualizacao { get; set; }
    }
}
