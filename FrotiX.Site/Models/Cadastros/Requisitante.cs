// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ ARQUIVO: Requisitante.cs                                                    ║
// ║ PROJETO: FrotiX - Sistema de Gestão de Frotas                               ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO:                                                                   ║
// ║ Entidade para cadastro de requisitantes de viagens.                         ║
// ║ Pessoas autorizadas a solicitar veículos para viagens.                      ║
// ║                                                                              ║
// ║ CLASSES:                                                                      ║
// ║ • RequisitanteViewModel - ViewModel com dropdown de SetorSolicitante        ║
// ║ • Requisitante - Entidade principal                                         ║
// ║                                                                              ║
// ║ PROPRIEDADES:                                                                ║
// ║ Identificação:                                                               ║
// ║ • RequisitanteId [Key] - Identificador único                                ║
// ║ • Nome - Nome do requisitante                                               ║
// ║ • Ponto - Ponto funcional                                                   ║
// ║ • Ramal - Ramal telefônico [ValidaZero]                                     ║
// ║ • Email - Email para contato                                                ║
// ║                                                                              ║
// ║ Relacionamentos:                                                              ║
// ║ • SetorSolicitanteId → SetorSolicitante - Setor do requisitante             ║
// ║                                                                              ║
// ║ Metadados:                                                                    ║
// ║ • Status - Ativo/Inativo                                                    ║
// ║ • DataAlteracao, UsuarioIdAlteracao - Auditoria                             ║
// ║                                                                              ║
// ║ DOCUMENTADO EM: 2026-01-28 | LOTE: 18                                       ║
// ╚══════════════════════════════════════════════════════════════════════════════╝
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
