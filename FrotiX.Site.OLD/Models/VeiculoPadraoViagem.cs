/* ****************************************************************************************
 * ⚡ ARQUIVO: VeiculoPadraoViagem.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Registrar padrões de comportamento de veículos para validação de dados.
 *
 * 📥 ENTRADAS     : Métricas médias e totais por veículo.
 *
 * 📤 SAÍDAS       : Registro persistido de padrões.
 *
 * 🔗 CHAMADA POR  : Validações e análises de viagens.
 *
 * 🔄 CHAMA        : DataAnnotations, EF Core (Table).
 *
 * 📦 DEPENDÊNCIAS : System.ComponentModel.DataAnnotations,
 *                   System.ComponentModel.DataAnnotations.Schema.
 **************************************************************************************** */

#nullable enable
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FrotiX.Models
{
    /****************************************************************************************
     * ⚡ MODEL: VeiculoPadraoViagem
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Armazenar padrões de comportamento por veículo.
     *
     * 📥 ENTRADAS     : Métricas médias e totais de viagens/abastecimentos.
     *
     * 📤 SAÍDAS       : Entidade consultável para validações.
     *
     * 🔗 CHAMADA POR  : Serviços de estatística e validação.
     *
     * 🔄 CHAMA        : Key, Display, Column, ForeignKey.
     ****************************************************************************************/
    [Table("VeiculoPadraoViagem")]
    public class VeiculoPadraoViagem
    {
        // Veículo associado.
        [Key]
        [Display(Name = "Veículo")]
        public Guid VeiculoId { get; set; }

        // Tipo de uso do veículo.
        [StringLength(50)]
        [Display(Name = "Tipo de Uso")]
        public string? TipoUso { get; set; }

        // Total de viagens analisadas.
        [Display(Name = "Total de Viagens")]
        public int TotalViagens { get; set; }

        // Média de duração em minutos.
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Média de Duração (Minutos)")]
        public decimal? MediaDuracaoMinutos { get; set; }

        // Média de km por viagem.
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Média de KM por Viagem")]
        public decimal? MediaKmPorViagem { get; set; }

        // Média de km por dia.
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Média de KM por Dia")]
        public decimal? MediaKmPorDia { get; set; }

        // Média de km entre abastecimentos.
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Média KM entre Abastecimentos")]
        public decimal? MediaKmEntreAbastecimentos { get; set; }

        // Média de dias entre abastecimentos.
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Média Dias entre Abastecimentos")]
        public decimal? MediaDiasEntreAbastecimentos { get; set; }

        // Total de abastecimentos analisados.
        [Display(Name = "Total Abastecimentos Analisados")]
        public int? TotalAbastecimentosAnalisados { get; set; }

        // Data da última atualização.
        [Display(Name = "Data de Atualização")]
        public DateTime? DataAtualizacao { get; set; }

        // Navegação
        // Navegação para veículo.
        [ForeignKey("VeiculoId")]
        public virtual Veiculo? Veiculo { get; set; }
    }
}
