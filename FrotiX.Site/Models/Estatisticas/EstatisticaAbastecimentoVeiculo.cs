/*
 * ╔══════════════════════════════════════════════════════════════════════════╗
 * ║  📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                   ║
 * ║  Arquivo: Models/Estatisticas/EstatisticaAbastecimentoVeiculo.cs         ║
 * ║  Descrição: Modelo para estatísticas de abastecimento por veículo        ║
 * ║  Data: 29/01/2026 | LOTE: 22                                             ║
 * ╚══════════════════════════════════════════════════════════════════════════╝
 */

#nullable enable
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FrotiX.Models.Estatisticas
{
    [Table("EstatisticaAbastecimentoVeiculo")]
    public class EstatisticaAbastecimentoVeiculo
    {
        [Key]
        public Guid Id { get; set; }

        public int Ano { get; set; }

        public Guid VeiculoId { get; set; }

        [StringLength(20)]
        public string? Placa { get; set; }

        [StringLength(100)]
        public string? TipoVeiculo { get; set; }

        [StringLength(100)]
        public string? Categoria { get; set; }

        public int TotalAbastecimentos { get; set; }

        public decimal? ValorTotal { get; set; }

        public decimal? LitrosTotal { get; set; }

        public DateTime DataAtualizacao { get; set; }
    }
}
