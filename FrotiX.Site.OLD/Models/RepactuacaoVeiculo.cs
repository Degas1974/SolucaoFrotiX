/* ****************************************************************************************
 * ⚡ ARQUIVO: RepactuacaoVeiculo.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Registrar repactuações de veículos vinculadas a contratos de locação.
 *
 * 📥 ENTRADAS     : Contrato, veículo, valor e observações.
 *
 * 📤 SAÍDAS       : Registro persistido de repactuação por veículo.
 *
 * 🔗 CHAMADA POR  : Fluxos de repactuação e contratos.
 *
 * 🔄 CHAMA        : DataAnnotations, ForeignKey.
 *
 * 📦 DEPENDÊNCIAS : RepactuacaoContrato, Veiculo.
 **************************************************************************************** */

#nullable enable
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FrotiX.Models
{
    /****************************************************************************************
     * ⚡ MODEL: RepactuacaoVeiculo
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Armazenar valores de repactuação por veículo.
     *
     * 📥 ENTRADAS     : Identificadores de contrato/veículo e valor.
     *
     * 📤 SAÍDAS       : Entidade persistida para gestão de repactuação.
     *
     * 🔗 CHAMADA POR  : Repositórios e controllers.
     *
     * 🔄 CHAMA        : Key, ForeignKey, Display, DataType.
     ****************************************************************************************/
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
