/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: VeiculoPadraoViagem.cs                                                                  ║
   ║ 📂 CAMINHO: /Models                                                                                 ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO: Padrões de comportamento de veículos para validação de dados e detecção de outliers.  ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 ENTIDADE: VeiculoPadraoViagem (VeiculoId, TipoUso, TotalViagens, MediaDuracaoMinutos, MediaKm)   ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🔗 DEPS: System.ComponentModel.DataAnnotations                                                      ║
   ║ 📅 Atualizado: 2026 | 👤 FrotiX Team | 📝 Versão: 2.0                                              ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝ */

#nullable enable
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FrotiX.Models
{
    /// <summary>
    /// Armazena padrões de comportamento de cada veículo,
    /// calculados a partir do histórico de viagens e abastecimentos.
    /// Usado para detecção de outliers e normalização de dados.
    /// </summary>
    [Table("VeiculoPadraoViagem")]
    public class VeiculoPadraoViagem
    {
        [Key]
        [Display(Name = "Veículo")]
        public Guid VeiculoId { get; set; }

        [StringLength(50)]
        [Display(Name = "Tipo de Uso")]
        public string? TipoUso { get; set; }

        [Display(Name = "Total de Viagens")]
        public int TotalViagens { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Média de Duração (Minutos)")]
        public decimal? MediaDuracaoMinutos { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Média de KM por Viagem")]
        public decimal? MediaKmPorViagem { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Média de KM por Dia")]
        public decimal? MediaKmPorDia { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Média KM entre Abastecimentos")]
        public decimal? MediaKmEntreAbastecimentos { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Média Dias entre Abastecimentos")]
        public decimal? MediaDiasEntreAbastecimentos { get; set; }

        [Display(Name = "Total Abastecimentos Analisados")]
        public int? TotalAbastecimentosAnalisados { get; set; }

        [Display(Name = "Data de Atualização")]
        public DateTime? DataAtualizacao { get; set; }

        // Navegação
        [ForeignKey("VeiculoId")]
        public virtual Veiculo? Veiculo { get; set; }
    }
}
