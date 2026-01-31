/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
    ║ 🚀 ARQUIVO: HeatmapAbastecimentoMensal.cs                                                           ║
    ║ 📂 CAMINHO: /Models/Estatisticas                                                                    ║
    ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
    ║ 🎯 OBJETIVO: Modelo para heatmap de abastecimentos mensais em dashboards.                           ║
    ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
    ║ 📋 ENTIDADE: HeatmapAbastecimentoMensal (Ano, Mes, VeiculoId, TipoVeiculo, DiaSemana, Hora)        ║
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
    [Table("HeatmapAbastecimentoMensal")]
    public class HeatmapAbastecimentoMensal
    {
        [Key]
        public Guid Id { get; set; }

        public int Ano { get; set; }

        public int Mes { get; set; }

        
        // NULL = todos os veículos
        
        public Guid? VeiculoId { get; set; }

        
        // NULL = todos os tipos
        
        [StringLength(100)]
        public string? TipoVeiculo { get; set; }

        
        // 0=Domingo, 1=Segunda, ... 6=Sábado
        
        public int DiaSemana { get; set; }

        
        // 0-23
        
        public int Hora { get; set; }

        public int TotalAbastecimentos { get; set; }

        public decimal? ValorTotal { get; set; }

        public DateTime DataAtualizacao { get; set; }
    }
}
