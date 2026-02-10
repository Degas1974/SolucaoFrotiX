/* ****************************************************************************************
 * ⚡ ARQUIVO: MarcaVeiculo.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Manter o cadastro de marcas de veículos (ex.: Ford, Fiat).
 *
 * 📥 ENTRADAS     : Dados da marca e status.
 *
 * 📤 SAÍDAS       : Entidade persistida e ViewModel simples.
 *
 * 🔗 CHAMADA POR  : Cadastros de veículos e filtros de modelo.
 *
 * 🔄 CHAMA        : DataAnnotations.
 *
 * 📦 DEPENDÊNCIAS : System.ComponentModel.DataAnnotations.
 **************************************************************************************** */

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FrotiX.Models
{
    /****************************************************************************************
     * ⚡ VIEWMODEL: MarcaVeiculoViewModel
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Transportar a chave da marca em operações simples de tela.
     *
     * 📥 ENTRADAS     : MarcaId.
     *
     * 📤 SAÍDAS       : ViewModel simplificado para UI.
     *
     * 🔗 CHAMADA POR  : Controllers/Views de marcas.
     ****************************************************************************************/
    public class MarcaVeiculoViewModel
    {
        // Identificador da marca.
        public Guid MarcaId { get; set; }
    }

    /****************************************************************************************
     * ⚡ MODEL: MarcaVeiculo
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Representar uma marca de veículo.
     *
     * 📥 ENTRADAS     : Descrição e status.
     *
     * 📤 SAÍDAS       : Registro persistido para consulta/edição.
     *
     * 🔗 CHAMADA POR  : Repositórios e controllers.
     ****************************************************************************************/
    public class MarcaVeiculo
    {
        // Identificador único da marca.
        [Key]
        public Guid MarcaId { get; set; }

        // Descrição/nome da marca.
        [StringLength(50, ErrorMessage = "A descrição não pode exceder 50 caracteres")]
        [Required(ErrorMessage = "(A descrição da marca é obrigatória)")]
        [Display(Name = "Marca do Veículo")]
        public string? DescricaoMarca { get; set; }

        // Flag de status ativo/inativo.
        [Display(Name = "Ativo/Inativo")]
        public bool Status { get; set; }
    }
}
