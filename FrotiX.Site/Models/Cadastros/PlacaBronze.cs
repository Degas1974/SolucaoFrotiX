/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 📌 ARQUIVO: PlacaBronze.cs                                                                          ║
   ║ 📂 CAMINHO: /Models/Cadastros                                                                       ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🧭 OBJETIVO: Cadastrar placas bronze (modelo antigo de placas de veículos).                         ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🗂️  CONTÉM: PlacaBronzeViewModel, PlacaBronze                                                       ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🔗 DEPENDÊNCIAS: DataAnnotations, ValidateNever                                                     ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝ */

using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FrotiX.Models
{
    // ==================================================================================================
    // VIEW MODEL
    // ==================================================================================================
    // Finalidade: transportar placa bronze e o veículo associado na UI.
    // ==================================================================================================
    public class PlacaBronzeViewModel
    {
        // Identificador da placa bronze.
        public Guid PlacaBronzeId
        {
            get; set;
        }

        // Entidade principal do formulário.
        public PlacaBronze? PlacaBronze
        {
            get; set;
        }

        // Veículo associado (não mapeado).
        [NotMapped]
        [ValidateNever]
        [Display(Name = "Veículo Associado")]
        public Guid VeiculoId
        {
            get; set;
        }
    }

    // ==================================================================================================
    // ENTIDADE
    // ==================================================================================================
    // Representa a placa bronze (modelo antigo).
    // ==================================================================================================
    public class PlacaBronze
    {
        // Identificador único da placa.
        [Key]
        public Guid PlacaBronzeId
        {
            get; set;
        }

        // Descrição da placa.
        [StringLength(100 , ErrorMessage = "A descrição não pode exceder 100 caracteres")]
        [Required(ErrorMessage = "(A descrição da placa é obrigatória)")]
        [Display(Name = "Placa de Bronze")]
        public string? DescricaoPlaca
        {
            get; set;
        }

        // Status ativo/inativo.
        [Display(Name = "Ativo/Inativo")]
        public bool Status
        {
            get; set;
        }
    }
}
