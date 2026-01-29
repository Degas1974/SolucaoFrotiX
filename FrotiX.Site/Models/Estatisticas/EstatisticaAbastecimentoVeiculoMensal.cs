/*
 * ╔══════════════════════════════════════════════════════════════════════════╗
 * ║  📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                   ║
 * ║  Arquivo: Models/Estatisticas/EstatisticaAbastecimentoVeiculoMensal.cs   ║
 * ║  Descrição: Modelo para estatísticas mensais de abastecimento por veículo║
 * ║  Data: 29/01/2026 | LOTE: 22                                             ║
 * ╚══════════════════════════════════════════════════════════════════════════╝
 */

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
