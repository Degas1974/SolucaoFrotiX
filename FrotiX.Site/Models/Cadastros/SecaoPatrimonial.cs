// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ ARQUIVO: SecaoPatrimonial.cs                                                ║
// ║ PROJETO: FrotiX - Sistema de Gestão de Frotas                               ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO:                                                                   ║
// ║ Entidade para cadastro de seções patrimoniais (subdivisões de setor).       ║
// ║ Permite localização mais granular de patrimônios dentro de setores.         ║
// ║                                                                              ║
// ║ CLASSES:                                                                      ║
// ║ • SecaoPatrimonial - Entidade única (sem ViewModel separada)                ║
// ║                                                                              ║
// ║ PROPRIEDADES:                                                                ║
// ║ • SecaoId [Key] - Identificador único                                       ║
// ║ • NomeSecao - Nome da seção (max 50 chars)                                  ║
// ║ • SetorId → SetorPatrimonial - Setor pai                                    ║
// ║ • Status - Ativo/Inativo                                                    ║
// ║                                                                              ║
// ║ USO:                                                                          ║
// ║ • Hierarquia: SetorPatrimonial > SecaoPatrimonial > Patrimonio              ║
// ║ • Ex: Setor "TI" → Seção "Sala de Servidores"                               ║
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
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FrotiX.Models.Cadastros
{
    public class SecaoPatrimonial
    {
        [Key]
        public Guid SecaoId { get; set; }

        [StringLength(50, ErrorMessage = "O NomeSecao não pode exceder 50 caracteres")]
        [Required(ErrorMessage = "(Obrigatória)")]
        [Display(Name = "NomeSecao")]
        public string? NomeSecao { get; set; }

        public Guid SetorId { get; set; }

        [ForeignKey("SetorId")]
        public virtual SetorPatrimonial? SetorPatrimonial { get; set; }

        public bool Status { get; set; }
    }
}
