/* ****************************************************************************************
 * ⚡ ARQUIVO: VeiculoPadraoViagem.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Armazenar padrões estatísticos de comportamento de veículos.
 *
 * 📥 ENTRADAS     : Métricas de duração, Km, outliers e agregações estatísticas.
 *
 * 📤 SAÍDAS       : Registro persistido para validação de viagens anômalas.
 *
 * 🔗 CHAMADA POR  : Sistema de detecção de outliers e validações de viagens.
 *
 * 🔄 CHAMA        : DataAnnotations, EF Core (Table).
 *
 * 📦 DEPENDÊNCIAS : System.ComponentModel.DataAnnotations,
 *                   System.ComponentModel.DataAnnotations.Schema.
 *
 * ⚠️ ATENÇÃO      : MODELO COMPLETAMENTE REESCRITO (13/02/2026) para refletir banco real.
 *                   - PK corrigida: VeiculoPadraoViagemId (int) ao invés de VeiculoId (Guid)
 *                   - Adicionadas 22 colunas estatísticas faltantes
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
     * 🎯 OBJETIVO     : Armazenar padrões estatísticos por veículo (média, desvio, quartis).
     *
     * 📥 ENTRADAS     : Análise de viagens históricas por veículo.
     *
     * 📤 SAÍDAS       : Baseline para detecção de viagens anômalas/fraudulentas.
     *
     * 🔗 CHAMADA POR  : Sistema de validação de viagens e dashboards.
     *
     * 🔄 CHAMA        : Veiculo (navegação).
     ****************************************************************************************/
    [Table("VeiculoPadraoViagem")]
    public class VeiculoPadraoViagem
    {
        // =====================================================================
        // IDENTIFICAÇÃO
        // =====================================================================

        /// <summary>
        /// Chave primária (identity int).
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int VeiculoPadraoViagemId { get; set; }

        /// <summary>
        /// Veículo associado a este padrão.
        /// </summary>
        [Required]
        public Guid VeiculoId { get; set; }

        /// <summary>
        /// Navegação para o veículo.
        /// </summary>
        [ForeignKey("VeiculoId")]
        public virtual Veiculo? Veiculo { get; set; }

        /// <summary>
        /// Tipo de uso do veículo (ex: "Administrativo", "Transporte").
        /// </summary>
        [StringLength(50)]
        public string? TipoUso { get; set; }

        // =====================================================================
        // ESTATÍSTICAS DE DURAÇÃO (MINUTOS)
        // =====================================================================

        /// <summary>
        /// Média de duração em minutos.
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal? AvgDuracaoMinutos { get; set; }

        /// <summary>
        /// Desvio padrão da duração em minutos.
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal? DesvioPadraoDuracaoMinutos { get; set; }

        /// <summary>
        /// Duração mínima em minutos.
        /// </summary>
        public int? MinDuracaoMinutos { get; set; }

        /// <summary>
        /// Duração máxima normal (sem outliers) em minutos.
        /// </summary>
        public int? MaxDuracaoNormalMinutos { get; set; }

        /// <summary>
        /// Mediana da duração em minutos (int).
        /// </summary>
        public int? MedianaDuracaoMinutos { get; set; }

        /// <summary>
        /// Mediana da duração em minutos (decimal - versão precisa).
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal? MedianaMinutos { get; set; }

        /// <summary>
        /// Percentil 95 da duração (95% das viagens têm duração menor).
        /// </summary>
        public int? Percentil95Duracao { get; set; }

        /// <summary>
        /// Percentil 99 da duração (99% das viagens têm duração menor).
        /// </summary>
        public int? Percentil99Duracao { get; set; }

        // =====================================================================
        // ESTATÍSTICAS DE QUILOMETRAGEM
        // =====================================================================

        /// <summary>
        /// Média de Km por viagem.
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal? AvgKmPorViagem { get; set; }

        /// <summary>
        /// Desvio padrão de Km por viagem.
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal? DesvioPadraoKm { get; set; }

        /// <summary>
        /// Km máximo normal (sem outliers) por viagem.
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal? MaxKmNormalPorViagem { get; set; }

        /// <summary>
        /// Mediana de Km por viagem.
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal? MedianaKm { get; set; }

        /// <summary>
        /// Primeiro quartil (Q1) de Km - 25% das viagens têm Km menor.
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal? Q1Km { get; set; }

        /// <summary>
        /// Terceiro quartil (Q3) de Km - 75% das viagens têm Km menor.
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal? Q3Km { get; set; }

        /// <summary>
        /// Intervalo interquartil (IQR = Q3 - Q1) de Km.
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal? IQRKm { get; set; }

        /// <summary>
        /// Limite inferior para outliers (Q1 - 1.5*IQR).
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal? LimiteInferiorKm { get; set; }

        /// <summary>
        /// Limite superior para outliers (Q3 + 1.5*IQR).
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal? LimiteSuperiorKm { get; set; }

        /// <summary>
        /// Média de Km por dia.
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal? AvgKmPorDia { get; set; }

        // =====================================================================
        // ESTATÍSTICAS DE ABASTECIMENTO
        // =====================================================================

        /// <summary>
        /// Média de Km entre abastecimentos.
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal? MediaKmEntreAbastecimentos { get; set; }

        /// <summary>
        /// Média de dias entre abastecimentos.
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal? MediaDiasEntreAbastecimentos { get; set; }

        /// <summary>
        /// Total de abastecimentos analisados.
        /// </summary>
        public int? TotalAbastecimentosAnalisados { get; set; }

        // =====================================================================
        // TOTALIZADORES
        // =====================================================================

        /// <summary>
        /// Total de viagens analisadas (usadas para calcular estatísticas).
        /// </summary>
        public int? TotalViagensAnalisadas { get; set; }

        /// <summary>
        /// Total de viagens realizadas (inclui todas, mesmo outliers).
        /// </summary>
        public int? TotalViagensRealizadas { get; set; }

        // =====================================================================
        // TIMESTAMPS
        // =====================================================================

        /// <summary>
        /// Data de criação do registro.
        /// </summary>
        public DateTime? DataCriacao { get; set; }

        /// <summary>
        /// Data da última atualização do padrão.
        /// </summary>
        public DateTime? DataAtualizacao { get; set; }
    }
}
