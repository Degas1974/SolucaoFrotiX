// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ ARQUIVO: RepactuacaoVeiculo.cs                                              ║
// ║ PROJETO: FrotiX - Sistema de Gestão de Frotas                               ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO:                                                                   ║
// ║ Entidade para armazenar valores individuais de veículos em repactuações.    ║
// ║ Cada veículo pode ter valor diferente dentro de uma mesma repactuação.      ║
// ║                                                                              ║
// ║ CAMPOS:                                                                      ║
// ║ - RepactuacaoVeiculoId: Chave primária                                      ║
// ║ - RepactuacaoContratoId: FK para RepactuacaoContrato                        ║
// ║ - VeiculoId: FK para Veiculo                                                ║
// ║ - Valor: Valor mensal do veículo nesta repactuação (R$)                     ║
// ║ - Observacao: Observações específicas do veículo                            ║
// ║                                                                              ║
// ║ RELACIONAMENTOS:                                                             ║
// ║ - RepactuacaoContrato (N:1) - Repactuação pai                               ║
// ║ - Veiculo (N:1) - Veículo referenciado                                      ║
// ║                                                                              ║
// ║ DOCUMENTADO EM: 2026-01-28 | LOTE: 16                                       ║
// ╚══════════════════════════════════════════════════════════════════════════════╝

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
