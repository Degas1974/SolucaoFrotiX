// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ ARQUIVO: RankingMotoristasMensal.cs                                         ║
// ║ PROJETO: FrotiX - Sistema de Gestão de Frotas                               ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO:                                                                   ║
// ║ Rankings mensais de motoristas por diferentes métricas.                     ║
// ║ Pré-calculado para dashboards de performance.                               ║
// ║                                                                              ║
// ║ TIPOS DE RANKING:                                                            ║
// ║ - VIAGENS: Quantidade de viagens realizadas                                 ║
// ║ - KM: Quilometragem total percorrida                                        ║
// ║ - HORAS: Total de horas trabalhadas                                         ║
// ║ - ABASTECIMENTOS: Quantidade de abastecimentos                              ║
// ║ - MULTAS: Quantidade de multas (ranking inverso)                            ║
// ║ - PERFORMANCE: Score composto (viagens, km, horas, menos multas)            ║
// ║                                                                              ║
// ║ CAMPOS:                                                                      ║
// ║ - Posicao: 1 = primeiro lugar                                               ║
// ║ - ValorPrincipal, ValorSecundario, ValorTerciario, ValorQuaternario         ║
// ║                                                                              ║
// ║ DOCUMENTADO EM: 2026-01-28 | LOTE: 17                                       ║
// ╚══════════════════════════════════════════════════════════════════════════════╝

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FrotiX.Models.Estatisticas
{
    [Table("RankingMotoristasMensal")]
    public class RankingMotoristasMensal
    {
        [Key]
        public Guid Id { get; set; }

        public int Ano { get; set; }

        public int Mes { get; set; }

        [StringLength(50)]
        public string TipoRanking { get; set; } // 'VIAGENS', 'KM', 'HORAS', 'ABASTECIMENTOS', 'MULTAS', 'PERFORMANCE'

        public int Posicao { get; set; }

        public Guid MotoristaId { get; set; }

        [StringLength(200)]
        public string NomeMotorista { get; set; }

        [StringLength(50)]
        public string TipoMotorista { get; set; } // Efetivo/Ferista/Cobertura

        // Valores conforme o tipo de ranking
        public decimal ValorPrincipal { get; set; } // Viagens/KM/Horas/etc

        public decimal ValorSecundario { get; set; } // KM (para performance), Valor (para multas)

        public decimal ValorTerciario { get; set; } // Horas (para performance)

        public int ValorQuaternario { get; set; } // Multas (para performance)

        // Controle
        public DateTime DataAtualizacao { get; set; }

        // Navegação
        [ForeignKey("MotoristaId")]
        public virtual FrotiX.Models.Motorista Motorista { get; set; }
    }
}
