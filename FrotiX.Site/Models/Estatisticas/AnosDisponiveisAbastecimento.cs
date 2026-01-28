// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ ARQUIVO: AnosDisponiveisAbastecimento.cs                                    ║
// ║ PROJETO: FrotiX - Sistema de Gestão de Frotas                               ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO:                                                                   ║
// ║ Entidade para cache de anos com abastecimentos disponíveis.                 ║
// ║ Pré-calculado para dropdowns de filtro por ano em dashboards.               ║
// ║                                                                              ║
// ║ CAMPOS:                                                                      ║
// ║ - Ano (PK): Ano disponível (ex: 2025, 2026)                                 ║
// ║ - TotalAbastecimentos: Quantidade de abastecimentos no ano                  ║
// ║ - DataAtualizacao: Timestamp da última atualização do cache                 ║
// ║                                                                              ║
// ║ USO: Filtros de ano no Dashboard de Abastecimentos                          ║
// ║                                                                              ║
// ║ DOCUMENTADO EM: 2026-01-28 | LOTE: 16                                       ║
// ╚══════════════════════════════════════════════════════════════════════════════╝

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FrotiX.Models.Estatisticas
{
    [Table("AnosDisponiveisAbastecimento")]
    public class AnosDisponiveisAbastecimento
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Ano { get; set; }

        public int TotalAbastecimentos { get; set; }

        public DateTime DataAtualizacao { get; set; }
    }
}
