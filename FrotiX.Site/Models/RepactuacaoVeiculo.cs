/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 📌 ARQUIVO: RepactuacaoVeiculo.cs                                                                   ║
   ║ 📂 CAMINHO: /Models                                                                                 ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🧭 OBJETIVO: Registrar repactuações de veículos vinculadas a contratos de locação.                 ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🗂️  CONTÉM: RepactuacaoVeiculo                                                                      ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🔗 DEPENDÊNCIAS: RepactuacaoContrato, Veiculo                                                       ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝ */

#nullable enable
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FrotiX.Models
{
    // ==================================================================================================
    // ENTIDADE
    // ==================================================================================================
    // Repactuação de veículos: grava valores individuais por contrato.
    // ==================================================================================================
    public class RepactuacaoVeiculo
    {
        // Identificador da repactuação.
        [Key]
        public Guid RepactuacaoVeiculoId { get; set; }

        // Contrato de repactuação associado.
        [Display(Name = "Repactuação")]
        public Guid RepactuacaoContratoId { get; set; }

        // Navegação para repactuação de contrato.
        [ForeignKey("RepactuacaoContratoId")]
        public virtual RepactuacaoContrato RepactuacaoContrato { get; set; } = null!;

        // Veículo associado.
        [Display(Name = "Veículo")]
        public Guid VeiculoId { get; set; }

        // Navegação para veículo.
        [ForeignKey("VeiculoId")]
        public virtual Veiculo Veiculo { get; set; } = null!;

        // Valor de repactuação.
        [DataType(DataType.Currency)]
        [Display(Name = "Valor (R$)")]
        public double? Valor { get; set; }

        // Observações da repactuação.
        [Display(Name = "Observação")]
        public string? Observacao { get; set; }
    }
}
