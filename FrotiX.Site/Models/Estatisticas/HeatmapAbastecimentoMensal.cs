// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ ARQUIVO: HeatmapAbastecimentoMensal.cs                                      ║
// ║ PROJETO: FrotiX - Sistema de Gestão de Frotas                               ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO:                                                                   ║
// ║ Dados para heatmap 7x24 de abastecimentos (dia da semana x hora).           ║
// ║ Matriz pré-calculada para visualização de padrões de consumo.               ║
// ║                                                                              ║
// ║ CAMPOS:                                                                      ║
// ║ - Id, Ano, Mes: Identificação do período                                    ║
// ║ - VeiculoId: NULL = todos os veículos                                       ║
// ║ - TipoVeiculo: NULL = todos os tipos                                        ║
// ║ - DiaSemana: 0=Domingo, 1=Segunda, ... 6=Sábado                             ║
// ║ - Hora: 0-23                                                                ║
// ║ - TotalAbastecimentos, ValorTotal                                           ║
// ║                                                                              ║
// ║ USO: Gráfico heatmap no Dashboard de Abastecimentos                         ║
// ║                                                                              ║
// ║ DOCUMENTADO EM: 2026-01-28 | LOTE: 17                                       ║
// ╚══════════════════════════════════════════════════════════════════════════════╝

#nullable enable
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FrotiX.Models.Estatisticas
{
    [Table("HeatmapAbastecimentoMensal")]
    public class HeatmapAbastecimentoMensal
    {
        [Key]
        public Guid Id { get; set; }

        public int Ano { get; set; }

        public int Mes { get; set; }

        /// <summary>
        /// NULL = todos os veículos
        /// </summary>
        public Guid? VeiculoId { get; set; }

        /// <summary>
        /// NULL = todos os tipos
        /// </summary>
        [StringLength(100)]
        public string? TipoVeiculo { get; set; }

        /// <summary>
        /// 0=Domingo, 1=Segunda, ... 6=Sábado
        /// </summary>
        public int DiaSemana { get; set; }

        /// <summary>
        /// 0-23
        /// </summary>
        public int Hora { get; set; }

        public int TotalAbastecimentos { get; set; }

        public decimal? ValorTotal { get; set; }

        public DateTime DataAtualizacao { get; set; }
    }
}
