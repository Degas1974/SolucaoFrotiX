// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ ARQUIVO: SetorSolicitante.cs                                                ║
// ║ PROJETO: FrotiX - Sistema de Gestão de Frotas                               ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO:                                                                   ║
// ║ Entidade para cadastro de setores solicitantes de viagens.                  ║
// ║ Departamentos/áreas que podem requisitar veículos.                          ║
// ║                                                                              ║
// ║ CLASSES:                                                                      ║
// ║ • SetorSolicitanteViewModel - ViewModel simples                             ║
// ║ • SetorSolicitante - Entidade principal                                     ║
// ║                                                                              ║
// ║ PROPRIEDADES:                                                                ║
// ║ • SetorSolicitanteId [Key] - Identificador único                            ║
// ║ • Nome - Nome do setor (max 200 chars)                                      ║
// ║ • Sigla - Sigla do setor (max 50 chars)                                     ║
// ║ • SetorPaiId - ID do setor pai (hierarquia)                                 ║
// ║ • Ramal - Ramal telefônico                                                  ║
// ║ • Status - Ativo/Inativo                                                    ║
// ║ • DataAlteracao, UsuarioIdAlteracao - Auditoria                             ║
// ║                                                                              ║
// ║ USO:                                                                          ║
// ║ • Vinculação em Requisitante e Viagem                                       ║
// ║ • Relatórios por setor                                                      ║
// ║                                                                              ║
// ║ DOCUMENTADO EM: 2026-01-28 | LOTE: 18                                       ║
// ╚══════════════════════════════════════════════════════════════════════════════╝
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
