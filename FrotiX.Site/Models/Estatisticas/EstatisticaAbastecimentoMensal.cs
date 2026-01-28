// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ ARQUIVO: EstatisticaAbastecimentoMensal.cs                                  ║
// ║ PROJETO: FrotiX - Sistema de Gestão de Frotas                               ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO:                                                                   ║
// ║ Totais consolidados de abastecimentos por mês.                              ║
// ║ Base para gráficos de linha de evolução mensal.                             ║
// ║                                                                              ║
// ║ CAMPOS:                                                                      ║
// ║ - Id, Ano, Mes: Identificação do período                                    ║
// ║ - TotalAbastecimentos: Quantidade de abastecimentos                         ║
// ║ - ValorTotal: Valor total gasto em R$                                       ║
// ║ - LitrosTotal: Total de litros abastecidos                                  ║
// ║ - DataAtualizacao: Timestamp do cache                                       ║
// ║                                                                              ║
// ║ DOCUMENTADO EM: 2026-01-28 | LOTE: 16                                       ║
// ╚══════════════════════════════════════════════════════════════════════════════╝

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FrotiX.Models.Estatisticas
{
    [Table("EstatisticaAbastecimentoMensal")]
    public class EstatisticaAbastecimentoMensal
    {
        [Key]
        public Guid Id { get; set; }

        public int Ano { get; set; }

        public int Mes { get; set; }

        public int TotalAbastecimentos { get; set; }

        public decimal? ValorTotal { get; set; }

        public decimal? LitrosTotal { get; set; }

        public DateTime DataAtualizacao { get; set; }
    }
}
