/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: PlacaBronze.cs                                                                          ║
   ║ 📂 CAMINHO: /Models/Cadastros                                                                       ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO: Entidade e ViewModels para placas bronze (modelo antigo de placas de veículos).       ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 CLASSES: PlacaBronze (PlacaBronzeId), PlacaBronzeViewModel (VeiculoId)                           ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🔗 DEPS: System.ComponentModel.DataAnnotations, ValidateNever                                       ║
   ║ 📅 Atualizado: 2026 | 👤 FrotiX Team | 📝 Versão: 2.0                                              ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝ */

using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FrotiX.Models
{
    public class PlacaBronzeViewModel
    {
        public Guid PlacaBronzeId
        {
            get; set;
        }
        public PlacaBronze? PlacaBronze
        {
            get; set;
        }

        [NotMapped]
        [ValidateNever]
        [Display(Name = "Veículo Associado")]
        public Guid VeiculoId
        {
            get; set;
        }
    }

    public class PlacaBronze
    {
        [Key]
        public Guid PlacaBronzeId
        {
            get; set;
        }

        [StringLength(100 , ErrorMessage = "A descrição não pode exceder 100 caracteres")]
        [Required(ErrorMessage = "(A descrição da placa é obrigatória)")]
        [Display(Name = "Placa de Bronze")]
        public string? DescricaoPlaca
        {
            get; set;
        }

        [Display(Name = "Ativo/Inativo")]
        public bool Status
        {
            get; set;
        }
    }
}
