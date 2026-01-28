// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ ARQUIVO: HeatmapViagensMensal.cs                                            ║
// ║ PROJETO: FrotiX - Sistema de Gestão de Frotas                               ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO:                                                                   ║
// ║ Dados para heatmap 7x24 de viagens (dia da semana x hora).                  ║
// ║ Matriz pré-calculada para visualização de padrões de demanda.               ║
// ║                                                                              ║
// ║ CAMPOS:                                                                      ║
// ║ - Id, Ano, Mes: Identificação do período                                    ║
// ║ - MotoristaId: NULL = todos os motoristas                                   ║
// ║ - DiaSemana: 0=Domingo, 1=Segunda, ... 6=Sábado                             ║
// ║ - Hora: 0-23                                                                ║
// ║ - TotalViagens                                                              ║
// ║                                                                              ║
// ║ USO: Gráfico heatmap Economildo, análise de demanda                         ║
// ║                                                                              ║
// ║ DOCUMENTADO EM: 2026-01-28 | LOTE: 17                                       ║
// ╚══════════════════════════════════════════════════════════════════════════════╝

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
