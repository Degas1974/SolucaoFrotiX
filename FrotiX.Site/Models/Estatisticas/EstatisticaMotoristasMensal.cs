// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ ARQUIVO: EstatisticaMotoristasMensal.cs                                     ║
// ║ PROJETO: FrotiX - Sistema de Gestão de Frotas                               ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO:                                                                   ║
// ║ Estatísticas mensais individuais por motorista.                             ║
// ║ Consolida viagens, multas e abastecimentos por motorista/mês.               ║
// ║                                                                              ║
// ║ VIAGENS: TotalViagens, KmTotal, MinutosTotais                               ║
// ║ MULTAS: TotalMultas, ValorTotalMultas                                       ║
// ║ ABASTECIMENTOS: TotalAbastecimentos, LitrosTotais, ValorTotal               ║
// ║                                                                              ║
// ║ RELACIONAMENTO: Motorista (FK MotoristaId)                                  ║
// ║                                                                              ║
// ║ USO: Ranking de motoristas, análise de performance individual               ║
// ║                                                                              ║
// ║ DOCUMENTADO EM: 2026-01-28 | LOTE: 16                                       ║
// ╚══════════════════════════════════════════════════════════════════════════════╝

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FrotiX.Models.Estatisticas
{
    [Table("EstatisticaMotoristasMensal")]
    public class EstatisticaMotoristasMensal
    {
        [Key]
        public Guid Id { get; set; }

        public Guid MotoristaId { get; set; }

        public int Ano { get; set; }

        public int Mes { get; set; }

        // Viagens
        public int TotalViagens { get; set; }

        public decimal KmTotal { get; set; }

        public int MinutosTotais { get; set; }

        // Multas
        public int TotalMultas { get; set; }

        public decimal ValorTotalMultas { get; set; }

        // Abastecimentos
        public int TotalAbastecimentos { get; set; }

        public decimal LitrosTotais { get; set; }

        public decimal ValorTotalAbastecimentos { get; set; }

        // Controle
        public DateTime DataAtualizacao { get; set; }

        // Navegação
        [ForeignKey("MotoristaId")]
        public virtual FrotiX.Models.Motorista Motorista { get; set; }
    }
}
