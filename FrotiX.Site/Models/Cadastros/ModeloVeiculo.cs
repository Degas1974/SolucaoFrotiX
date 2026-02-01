/* ****************************************************************************************
 * ⚡ ARQUIVO: ModeloVeiculo.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Manter modelos de veículos e sua relação com marcas.
 *
 * 📥 ENTRADAS     : Descrição do modelo, status e marca associada.
 *
 * 📤 SAÍDAS       : Entidade persistida e ViewModel para UI.
 *
 * 🔗 CHAMADA POR  : Cadastros de veículos e filtros de modelo.
 *
 * 🔄 CHAMA        : ValidaLista, ForeignKey, SelectListItem.
 *
 * 📦 DEPENDÊNCIAS : FrotiX.Validations, Microsoft.AspNetCore.Mvc.Rendering.
 **************************************************************************************** */

using FrotiX.Validations;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FrotiX.Models
{
    /****************************************************************************************
     * ⚡ VIEWMODEL: ModeloVeiculoViewModel
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Disponibilizar modelo e lista de marcas para seleção em tela.
     *
     * 📥 ENTRADAS     : ModeloVeiculo e lista de marcas.
     *
     * 📤 SAÍDAS       : ViewModel para telas de cadastro/edição.
     *
     * 🔗 CHAMADA POR  : Controllers/Views de modelos.
     *
     * 🔄 CHAMA        : SelectListItem.
     ****************************************************************************************/
    public class ModeloVeiculoViewModel
    {
        // Identificador do modelo.
        public Guid ModeloId
        {
            get; set;
        }

        // Entidade carregada/alterada no formulário.
        public ModeloVeiculo? ModeloVeiculo
        {
            get; set;
        }

        // Lista de marcas para seleção.
        public IEnumerable<SelectListItem>? MarcaList
        {
            get; set;
        }
    }

    /****************************************************************************************
     * ⚡ MODEL: ModeloVeiculo
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Representar um modelo de veículo vinculado a uma marca.
     *
     * 📥 ENTRADAS     : Descrição, status e marca vinculada.
     *
     * 📤 SAÍDAS       : Registro persistido de modelo.
     *
     * 🔗 CHAMADA POR  : Repositórios e controllers.
     *
     * 🔄 CHAMA        : ForeignKey, ValidaLista.
     ****************************************************************************************/
    public class ModeloVeiculo
    {
        // Identificador único do modelo.
        [Key]
        public Guid ModeloId
        {
            get; set;
        }

        // Descrição do modelo.
        [StringLength(50 , ErrorMessage = "A descrição não pode exceder 50 caracteres")]
        [Required(ErrorMessage = "(A descrição do modelo é obrigatória)")]
        [Display(Name = "Modelo do Veículo")]
        public string? DescricaoModelo
        {
            get; set;
        }

        // Flag de status ativo/inativo.
        [Display(Name = "Ativo/Inativo")]
        public bool Status
        {
            get; set;
        }

        // Marca associada ao modelo.
        [ValidaLista(ErrorMessage = "(A Marca é obrigatória)")]
        [Display(Name = "Marca do Veículo")]
        public Guid MarcaId
        {
            get; set;
        }

        // Navegação para marca.
        [ForeignKey("MarcaId")]
        public virtual MarcaVeiculo? MarcaVeiculo
        {
            get; set;
        }
    }
}
