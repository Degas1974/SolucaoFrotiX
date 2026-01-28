// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ ARQUIVO: EstatisticaAbastecimentoTipoVeiculo.cs                             ║
// ║ PROJETO: FrotiX - Sistema de Gestão de Frotas                               ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO:                                                                   ║
// ║ Estatísticas de abastecimentos agrupadas por tipo de veículo.               ║
// ║ Permite análise comparativa entre tipos (Van, Sedan, SUV, etc).             ║
// ║                                                                              ║
// ║ CAMPOS:                                                                      ║
// ║ - Id, Ano, Mes: Identificação do período                                    ║
// ║ - TipoVeiculo: Tipo do veículo                                              ║
// ║ - TotalAbastecimentos, ValorTotal, LitrosTotal                              ║
// ║ - DataAtualizacao: Timestamp do cache                                       ║
// ║                                                                              ║
// ║ DOCUMENTADO EM: 2026-01-28 | LOTE: 16                                       ║
// ╚══════════════════════════════════════════════════════════════════════════════╝

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FrotiX.Models.Estatisticas
{
    [Table("EstatisticaAbastecimentoTipoVeiculo")]
    public class EstatisticaAbastecimentoTipoVeiculo
    {
        [Key]
        public Guid Id { get; set; }

        public int Ano { get; set; }

        public int Mes { get; set; }

        [StringLength(100)]
        public string TipoVeiculo { get; set; } = string.Empty;

        public int TotalAbastecimentos { get; set; }

        public decimal? ValorTotal { get; set; }

        public decimal? LitrosTotal { get; set; }

        public DateTime DataAtualizacao { get; set; }
    }
}
