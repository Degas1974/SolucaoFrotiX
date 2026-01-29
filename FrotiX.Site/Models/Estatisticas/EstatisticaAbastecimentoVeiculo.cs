/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
    ║ 🚀 ARQUIVO: EstatisticaAbastecimentoVeiculo.cs                                                      ║
    ║ 📂 CAMINHO: /Models/Estatisticas                                                                    ║
    ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
    ║ 🎯 OBJETIVO: Estatísticas de abastecimento por veículo (ano, placa, categoria, totais).            ║
    ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
    ║ 📋 ENTIDADE: EstatisticaAbastecimentoVeiculo (VeiculoId, Placa, TipoVeiculo, Total, ValorTotal)    ║
    ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
    ║ 🔗 DEPS: System.ComponentModel.DataAnnotations                                                      ║
    ║ 📅 Atualizado: 2026 | 👤 FrotiX Team | 📝 Versão: 2.0                                              ║
    ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝ */

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
