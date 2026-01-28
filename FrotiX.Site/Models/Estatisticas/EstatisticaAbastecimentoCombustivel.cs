// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ ARQUIVO: EstatisticaAbastecimentoCombustivel.cs                             ║
// ║ PROJETO: FrotiX - Sistema de Gestão de Frotas                               ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO:                                                                   ║
// ║ Estatísticas de abastecimentos agrupadas por tipo de combustível.           ║
// ║ Inclui média de preço por litro para análise de variação.                   ║
// ║                                                                              ║
// ║ CAMPOS:                                                                      ║
// ║ - Id, Ano, Mes: Identificação do período                                    ║
// ║ - TipoCombustivel: Gasolina, Etanol, Diesel, GNV                            ║
// ║ - TotalAbastecimentos, ValorTotal, LitrosTotal                              ║
// ║ - MediaValorLitro: Preço médio por litro no período                         ║
// ║ - DataAtualizacao: Timestamp do cache                                       ║
// ║                                                                              ║
// ║ DOCUMENTADO EM: 2026-01-28 | LOTE: 16                                       ║
// ╚══════════════════════════════════════════════════════════════════════════════╝

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FrotiX.Models.Estatisticas
{
    [Table("EstatisticaAbastecimentoCombustivel")]
    public class EstatisticaAbastecimentoCombustivel
    {
        [Key]
        public Guid Id { get; set; }

        public int Ano { get; set; }

        public int Mes { get; set; }

        [StringLength(100)]
        public string TipoCombustivel { get; set; } = string.Empty;

        public int TotalAbastecimentos { get; set; }

        public decimal? ValorTotal { get; set; }

        public decimal? LitrosTotal { get; set; }

        public decimal? MediaValorLitro { get; set; }

        public DateTime DataAtualizacao { get; set; }
    }
}
