/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 📌 ARQUIVO: Requisitante.cs                                                                         ║
   ║ 📂 CAMINHO: /Models/Cadastros                                                                       ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🧭 OBJETIVO: Cadastrar requisitantes de viagens (pessoas autorizadas a solicitar).                 ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🗂️  CONTÉM: RequisitanteViewModel, Requisitante                                                     ║
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
    // Finalidade: transportar requisitante e lista de setores solicitantes.
    // ==================================================================================================
    public class RequisitanteViewModel
    {
        // Identificador do requisitante.
        public Guid RequisitanteId
        {
            get; set;
        }

        // Entidade principal do formulário.
        public Requisitante? Requisitante
        {
            get; set;
        }

        // Lista de setores solicitantes para seleção.
        public IEnumerable<SelectListItem>? SetorSolicitanteList
        {
            get; set;
        }
    }

    // ==================================================================================================
    // ENTIDADE
    // ==================================================================================================
    // Representa um requisitante de viagens.
    // ==================================================================================================
    public class Requisitante
    {
        // Identificador único do requisitante.
        [Key]
        public Guid RequisitanteId
        {
            get; set;
        }

        // Nome do requisitante.
        [Required(ErrorMessage = "(O nome do requisitante é obrigatório)")]
        [Display(Name = "Requisitante")]
        public string? Nome
        {
            get; set;
        }

        // Ponto/matrícula.
        [Required(ErrorMessage = "(O ponto é obrigatório)")]
        [Display(Name = "Ponto")]
        public string? Ponto
        {
            get; set;
        }

        // Ramal de contato.
        [ValidaZero(ErrorMessage = "(O ramal é obrigatório)")]
        [Required(ErrorMessage = "(O ramal é obrigatório)")]
        [Display(Name = "Ramal")]
        public int? Ramal
        {
            get; set;
        }

        // Email de contato.
        [Display(Name = "Email")]
        public string? Email
        {
            get; set;
        }

        // Status ativo/inativo.
        [Display(Name = "Ativo/Inativo")]
        public bool Status
        {
            get; set;
        }

        // Data da última alteração.
        public DateTime? DataAlteracao
        {
            get; set;
        }

        // Usuário responsável pela alteração.
        public string? UsuarioIdAlteracao
        {
            get; set;
        }

        // Setor solicitante vinculado.
        [Display(Name = "Setor Solicitante")]
        public Guid SetorSolicitanteId
        {
            get; set;
        }

        // Navegação para setor solicitante.
        [ForeignKey("SetorSolicitanteId")]
        public virtual SetorSolicitante? SetorSolicitante
        {
            get; set;
        }
    }
}
