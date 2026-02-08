/* ****************************************************************************************
 * ⚡ ARQUIVO: PlacaBronze.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Cadastrar placas bronze (modelo antigo de placas de veículos).
 *
 * 📥 ENTRADAS     : Dados da placa e vínculo com veículo (UI).
 *
 * 📤 SAÍDAS       : Entidade persistida e ViewModel para UI.
 *
 * 🔗 CHAMADA POR  : Cadastros de veículos e histórico de placas.
 *
 * 🔄 CHAMA        : DataAnnotations, ValidateNever, NotMapped.
 *
 * 📦 DEPENDÊNCIAS : Microsoft.AspNetCore.Mvc.ModelBinding.Validation.
 **************************************************************************************** */

using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FrotiX.Models
{
    /****************************************************************************************
     * ⚡ VIEWMODEL: PlacaBronzeViewModel
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Transportar placa bronze e veículo associado na UI.
     *
     * 📥 ENTRADAS     : PlacaBronze e VeiculoId (não mapeado).
     *
     * 📤 SAÍDAS       : ViewModel para telas de associação.
     *
     * 🔗 CHAMADA POR  : Controllers/Views de placas.
     ****************************************************************************************/
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

    /****************************************************************************************
     * ⚡ MODEL: PlacaBronze
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Representar a placa bronze (modelo antigo).
     *
     * 📥 ENTRADAS     : Descrição e status.
     *
     * 📤 SAÍDAS       : Registro persistido para controle histórico.
     *
     * 🔗 CHAMADA POR  : Repositórios e controllers.
     ****************************************************************************************/
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
