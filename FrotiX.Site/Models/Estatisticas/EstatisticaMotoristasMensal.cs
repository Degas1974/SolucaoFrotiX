/*
 * ╔══════════════════════════════════════════════════════════════════════════╗
 * ║  📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                   ║
 * ║  Arquivo: Models/Estatisticas/EstatisticaMotoristasMensal.cs             ║
 * ║  Descrição: Modelo para estatísticas mensais de motoristas               ║
 * ║  Data: 29/01/2026 | LOTE: 22                                             ║
 * ╚══════════════════════════════════════════════════════════════════════════╝
 */

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
