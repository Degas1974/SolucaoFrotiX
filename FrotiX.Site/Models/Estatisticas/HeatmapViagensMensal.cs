/*
 * ╔══════════════════════════════════════════════════════════════════════════╗
 * ║  📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                   ║
 * ║  Arquivo: Models/Estatisticas/HeatmapViagensMensal.cs                    ║
 * ║  Descrição: Modelo para heatmap de viagens mensais (dashboards)          ║
 * ║  Data: 29/01/2026 | LOTE: 22                                             ║
 * ╚══════════════════════════════════════════════════════════════════════════╝
 */

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FrotiX.Models.Estatisticas
{
    [Table("HeatmapViagensMensal")]
    public class HeatmapViagensMensal
    {
        [Key]
        public Guid Id { get; set; }

        public int Ano { get; set; }

        public int Mes { get; set; }

        public Guid? MotoristaId { get; set; } // NULL = todos os motoristas

        public int DiaSemana { get; set; } // 0=Domingo, 1=Segunda, ... 6=Sábado

        public int Hora { get; set; } // 0-23

        public int TotalViagens { get; set; }

        // Controle
        public DateTime DataAtualizacao { get; set; }

        // Navegação
        [ForeignKey("MotoristaId")]
        public virtual FrotiX.Models.Motorista Motorista { get; set; }
    }
}
