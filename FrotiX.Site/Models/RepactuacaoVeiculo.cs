/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: RepactuacaoVeiculo.cs                                                                   ║
   ║ 📂 CAMINHO: /Models                                                                                 ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO: Entidade para registro de repactuações de veículos/contratos. Grava valores           ║
   ║    individuais de cada veículo quando há repactuação de contrato de locação.                       ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 PROPS: RepactuacaoVeiculoId, RepactuacaoContratoId (FK), VeiculoId (FK), Valor, Observacao      ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🔗 DEPS: RepactuacaoContrato, Veiculo                                                               ║
   ║ 📅 Atualizado: 2026 | 👤 FrotiX Team | 📝 Versão: 2.0                                              ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝ */

#nullable enable
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FrotiX.Models
{
    /// <summary>
    /// Repactuação de Veículos - Grava os valores individuais de cada veículo
    /// quando há repactuação de contrato de locação
    /// </summary>
    public class RepactuacaoVeiculo
    {
        [Key]
        public Guid RepactuacaoVeiculoId { get; set; }

        [Display(Name = "Repactuação")]
        public Guid RepactuacaoContratoId { get; set; }

        [ForeignKey("RepactuacaoContratoId")]
        public virtual RepactuacaoContrato RepactuacaoContrato { get; set; } = null!;

        [Display(Name = "Veículo")]
        public Guid VeiculoId { get; set; }

        [ForeignKey("VeiculoId")]
        public virtual Veiculo Veiculo { get; set; } = null!;

        [DataType(DataType.Currency)]
        [Display(Name = "Valor (R$)")]
        public double? Valor { get; set; }

        [Display(Name = "Observação")]
        public string? Observacao { get; set; }
    }
}
