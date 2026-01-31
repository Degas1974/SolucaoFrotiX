/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 📌 ARQUIVO: MarcaVeiculo.cs                                                                         ║
   ║ 📂 CAMINHO: /Models/Cadastros                                                                       ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🧭 OBJETIVO: Manter o cadastro de marcas de veículos (ex.: Ford, Fiat).                             ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🗂️  CONTÉM: MarcaVeiculoViewModel, MarcaVeiculo                                                     ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🔗 DEPENDÊNCIAS: DataAnnotations                                                                     ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝ */

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FrotiX.Models
{
    // ==================================================================================================
    // VIEW MODEL
    // ==================================================================================================
    // Finalidade: transportar a chave da marca em operações simples de tela.
    // ==================================================================================================
    public class MarcaVeiculoViewModel
    {
        // Identificador da marca.
        public Guid MarcaId { get; set; }
    }

    // ==================================================================================================
    // ENTIDADE
    // ==================================================================================================
    // Representa uma marca de veículo.
    // ==================================================================================================
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
