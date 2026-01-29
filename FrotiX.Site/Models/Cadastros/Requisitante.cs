/*
 * ╔══════════════════════════════════════════════════════════════════════════╗
 * ║  📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                   ║
 * ║  Arquivo: Models/Cadastros/Requisitante.cs                               ║
 * ║  Descrição: Entidade e ViewModels para cadastro de requisitantes         ║
 * ║  Data: 29/01/2026 | LOTE: 22                                             ║
 * ╚══════════════════════════════════════════════════════════════════════════╝
 */

using FrotiX.Validations;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FrotiX.Models
{
    public class RequisitanteViewModel
    {
        public Guid RequisitanteId
        {
            get; set;
        }
        public Requisitante? Requisitante
        {
            get; set;
        }
        public IEnumerable<SelectListItem>? SetorSolicitanteList
        {
            get; set;
        }
    }

    public class Requisitante
    {
        [Key]
        public Guid RequisitanteId
        {
            get; set;
        }

        [Required(ErrorMessage = "(O nome do requisitante é obrigatório)")]
        [Display(Name = "Requisitante")]
        public string? Nome
        {
            get; set;
        }

        [Required(ErrorMessage = "(O ponto é obrigatório)")]
        [Display(Name = "Ponto")]
        public string? Ponto
        {
            get; set;
        }

        [ValidaZero(ErrorMessage = "(O ramal é obrigatório)")]
        [Required(ErrorMessage = "(O ramal é obrigatório)")]
        [Display(Name = "Ramal")]
        public int? Ramal
        {
            get; set;
        }

        [Display(Name = "Email")]
        public string? Email
        {
            get; set;
        }

        [Display(Name = "Ativo/Inativo")]
        public bool Status
        {
            get; set;
        }

        public DateTime? DataAlteracao
        {
            get; set;
        }

        public string? UsuarioIdAlteracao
        {
            get; set;
        }

        [Display(Name = "Setor Solicitante")]
        public Guid SetorSolicitanteId
        {
            get; set;
        }

        [ForeignKey("SetorSolicitanteId")]
        public virtual SetorSolicitante? SetorSolicitante
        {
            get; set;
        }
    }
}
