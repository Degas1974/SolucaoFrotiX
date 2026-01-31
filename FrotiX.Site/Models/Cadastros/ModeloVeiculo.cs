/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 📌 ARQUIVO: ModeloVeiculo.cs                                                                        ║
   ║ 📂 CAMINHO: /Models/Cadastros                                                                       ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🧭 OBJETIVO: Manter modelos de veículos e sua relação com marcas.                                   ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🗂️  CONTÉM: ModeloVeiculoViewModel, ModeloVeiculo                                                   ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🔗 DEPENDÊNCIAS: DataAnnotations, EF Core, SelectListItem, Validations                              ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝ */

using FrotiX.Validations;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FrotiX.Models
{
    // ==================================================================================================
    // VIEW MODEL
    // ==================================================================================================
    // Finalidade: disponibilizar o modelo e a lista de marcas para seleção em tela.
    // ==================================================================================================
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

    // ==================================================================================================
    // ENTIDADE
    // ==================================================================================================
    // Representa um modelo de veículo vinculado a uma marca.
    // ==================================================================================================
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
