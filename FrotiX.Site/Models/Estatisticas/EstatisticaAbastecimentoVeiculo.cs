// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ ARQUIVO: EstatisticaAbastecimentoVeiculo.cs                                 ║
// ║ PROJETO: FrotiX - Sistema de Gestão de Frotas                               ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO:                                                                   ║
// ║ Estatísticas anuais de abastecimentos por veículo.                          ║
// ║ Inclui dados desnormalizados para performance em consultas.                 ║
// ║                                                                              ║
// ║ CAMPOS:                                                                      ║
// ║ - Id, Ano, VeiculoId: Identificação                                         ║
// ║ - Placa, TipoVeiculo, Categoria: Dados desnormalizados                      ║
// ║ - TotalAbastecimentos, ValorTotal, LitrosTotal                              ║
// ║ - DataAtualizacao: Timestamp do cache                                       ║
// ║                                                                              ║
// ║ USO: Ranking de veículos por consumo, análises de custo por placa           ║
// ║                                                                              ║
// ║ DOCUMENTADO EM: 2026-01-28 | LOTE: 16                                       ║
// ╚══════════════════════════════════════════════════════════════════════════════╝

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
