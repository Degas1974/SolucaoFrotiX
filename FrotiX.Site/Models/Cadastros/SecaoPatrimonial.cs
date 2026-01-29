/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: SecaoPatrimonial.cs                                                                     ║
   ║ 📂 CAMINHO: /Models/Cadastros                                                                       ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO: Entidade e ViewModels para seções patrimoniais (subdivisão de setores patrimoniais).  ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 ENTIDADE: SecaoPatrimonial (SecaoId, NomeSecao, SetorId) + FK para SetorPatrimonial              ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🔗 DEPS: FrotiX.Services, FrotiX.Validations, SetorPatrimonial                                      ║
   ║ 📅 Atualizado: 2026 | 👤 FrotiX Team | 📝 Versão: 2.0                                              ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝ */

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
