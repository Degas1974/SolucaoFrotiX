/*
 * ╔══════════════════════════════════════════════════════════════════════════╗
 * ║  📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                   ║
 * ║  Arquivo: Models/Cadastros/SetorSolicitante.cs                           ║
 * ║  Descrição: Entidade e ViewModels para setores solicitantes de viagens   ║
 * ║  Data: 29/01/2026 | LOTE: 22                                             ║
 * ╚══════════════════════════════════════════════════════════════════════════╝
 */

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using FrotiX.Services;
using FrotiX.Validations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FrotiX.Models
{
    public class SetorSolicitanteViewModel
    {
        public Guid SetorSolicitanteId { get; set; }
        public SetorSolicitante? SetorSolicitante { get; set; }
        public string? NomeUsuarioAlteracao { get; set; }
    }

    public class SetorSolicitante
    {
        [Key]
        public Guid SetorSolicitanteId { get; set; }

        [StringLength(200, ErrorMessage = "o Nome não pode exceder 200 caracteres")]
        [Required(ErrorMessage = "(O Nome é obrigatório)")]
        [Display(Name = "Nome do Setor")]
        public string? Nome { get; set; }

        [StringLength(50, ErrorMessage = "A Sigla não pode exceder 50 caracteres")]
        [Display(Name = "Sigla")]
        public string? Sigla { get; set; }

        [Display(Name = "CNH")]
        public Guid? SetorPaiId { get; set; }

        [Required(ErrorMessage = "(O ramal é obrigatório)")]
        [Display(Name = "Ramal")]
        public int? Ramal { get; set; }

        [Display(Name = "Ativo/Inativo")]
        public bool Status { get; set; }

        public DateTime? DataAlteracao { get; set; }

        public string? UsuarioIdAlteracao { get; set; }
    }
}
