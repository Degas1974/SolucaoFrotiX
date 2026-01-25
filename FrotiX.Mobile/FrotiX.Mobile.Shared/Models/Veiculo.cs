using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FrotiX.Economildo.Validations;

namespace FrotiX.Mobile.Shared.Models
{
    public class Veiculo
    {
        [Key]
        public Guid VeiculoId { get; set; }

        [StringLength(10 , ErrorMessage = "A placa não pode exceder 10 caracteres")]
        [Required(ErrorMessage = "(Obrigatória)")]
        [Display(Name = "Placa")]
        public string? Placa { get; set; }

        [StringLength(20 , ErrorMessage = "O Renavam não pode exceder 20 caracteres")]
        [Display(Name = "Renavam")]
        public string? Renavam { get; set; }

        [StringLength(20 , ErrorMessage = "A Placa Vinculada não pode exceder 20 caracteres")]
        [Display(Name = "Placa Vinculada")]
        public string? PlacaVinculada { get; set; }

        [Display(Name = "Ano de Fabricação")]
        public int? AnoFabricacao { get; set; }

        [Display(Name = "Ano do Modelo")]
        public int? AnoModelo { get; set; }

        [Display(Name = "Carro Reserva")]
        public bool Reserva { get; set; }

        [Display(Name = "Ativo/Inativo")]
        public bool Status { get; set; }

        [Display(Name = "Veículo Próprio")]
        public bool VeiculoProprio { get; set; }

        public DateTime DataAlteracao { get; set; }

        [Display(Name = "Placa de Bronze")]
        public Guid? PlacaBronzeId { get; set; }

        [Display(Name = "Data de Ingresso na Frota")]
        public DateTime? DataIngresso { get; set; }

        [Display(Name = "Faz parte da Frota do Economildo?")]
        public bool Economildo { get; set; }

        public double? ValorMensal { get; set; }

        // ===== Relações =====
    }
}