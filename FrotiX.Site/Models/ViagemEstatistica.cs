/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 📌 ARQUIVO: ViagemEstatistica.cs                                                                    ║
   ║ 📂 CAMINHO: /Models                                                                                 ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🧭 OBJETIVO: Consolidar estatísticas de viagens (custos, totais e médias).                          ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🗂️  CONTÉM: ViagemEstatistica                                                                       ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🔗 DEPENDÊNCIAS: DataAnnotations, EF Core                                                           ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝ */

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FrotiX.Models
{
    [Table("ViagemEstatistica")]
    public class ViagemEstatistica
    {
        // Identificador do registro.
        [Key]
        public int Id { get; set; }

        // Data de referência das estatísticas.
        [Required]
        public DateTime DataReferencia { get; set; }

        // ========================================
        // ESTATÍSTICAS GERAIS DE VIAGENS
        // ========================================
        // Total de viagens.
        public int TotalViagens { get; set; }

        // Totais por status.
        public int ViagensFinalizadas { get; set; }
        public int ViagensEmAndamento { get; set; }
        public int ViagensAgendadas { get; set; }
        public int ViagensCanceladas { get; set; }

        // ========================================
        // CUSTOS GERAIS
        // ========================================
        // Custo total.
        public decimal CustoTotal { get; set; }

        // Custo médio por viagem.
        public decimal CustoMedioPorViagem { get; set; }

        // Custos por Tipo
        // Custo de veículo.
        public decimal CustoVeiculo { get; set; }

        // Custo de motorista.
        public decimal CustoMotorista { get; set; }
        // Custo de operador.
        public decimal CustoOperador { get; set; }
        // Custo de lavador.
        public decimal CustoLavador { get; set; }
        // Custo de combustível.
        public decimal CustoCombustivel { get; set; }

        // ========================================
        // QUILOMETRAGEM
        // ========================================
        // Quilometragem total.
        public decimal QuilometragemTotal { get; set; }

        // Quilometragem média.
        public decimal QuilometragemMedia { get; set; }

        // ========================================
        // DADOS AGREGADOS (JSON)
        // ========================================

        [Column(TypeName = "nvarchar(max)")]
        // JSON agregado: viagens por status.
        public string ViagensPorStatusJson { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        // JSON agregado: viagens por motorista.
        public string ViagensPorMotoristaJson { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        // JSON agregado: viagens por veículo.
        public string ViagensPorVeiculoJson { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        // JSON agregado: viagens por finalidade.
        public string ViagensPorFinalidadeJson { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        // JSON agregado: viagens por requisitante.
        public string ViagensPorRequisitanteJson { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        // JSON agregado: viagens por setor.
        public string ViagensPorSetorJson { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        // JSON agregado: custos por motorista.
        public string CustosPorMotoristaJson { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        // JSON agregado: custos por veículo.
        public string CustosPorVeiculoJson { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        // JSON agregado: km por veículo.
        public string KmPorVeiculoJson { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        // JSON agregado: custos por tipo.
        public string CustosPorTipoJson { get; set; }

        // ========================================
        // TIMESTAMPS
        // ========================================
        // Data de criação.
        public DateTime DataCriacao { get; set; }

        // Data de atualização.
        public DateTime? DataAtualizacao { get; set; }
    }
}
