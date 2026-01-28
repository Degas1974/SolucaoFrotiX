// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ ARQUIVO: EvolucaoViagensDiaria.cs                                           ║
// ║ PROJETO: FrotiX - Sistema de Gestão de Frotas                               ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO:                                                                   ║
// ║ Evolução diária de viagens para gráficos de linha.                          ║
// ║ MotoristaId NULL representa totais de todos os motoristas.                  ║
// ║                                                                              ║
// ║ CAMPOS:                                                                      ║
// ║ - Id, Data (date): Identificação do dia                                     ║
// ║ - MotoristaId: NULL = todos, ou ID específico                               ║
// ║ - TotalViagens, KmTotal, MinutosTotais                                      ║
// ║ - DataAtualizacao: Timestamp do cache                                       ║
// ║                                                                              ║
// ║ USO: Gráficos de evolução diária, análise de sazonalidade                   ║
// ║                                                                              ║
// ║ DOCUMENTADO EM: 2026-01-28 | LOTE: 16                                       ║
// ╚══════════════════════════════════════════════════════════════════════════════╝

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FrotiX.Models.Estatisticas
{
    [Table("EvolucaoViagensDiaria")]
    public class EvolucaoViagensDiaria
    {
        [Key]
        public Guid Id { get; set; }

        [Column(TypeName = "date")]
        public DateTime Data { get; set; }

        public Guid? MotoristaId { get; set; } // NULL = todos os motoristas

        public int TotalViagens { get; set; }

        public decimal KmTotal { get; set; }

        public int MinutosTotais { get; set; }

        // Controle
        public DateTime DataAtualizacao { get; set; }

        // Navegação
        [ForeignKey("MotoristaId")]
        public virtual FrotiX.Models.Motorista Motorista { get; set; }
    }
}
