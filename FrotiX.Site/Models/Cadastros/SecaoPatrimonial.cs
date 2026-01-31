/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 📌 ARQUIVO: SecaoPatrimonial.cs                                                                     ║
   ║ 📂 CAMINHO: /Models/Cadastros                                                                       ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🧭 OBJETIVO: Definir seções patrimoniais vinculadas a setores patrimoniais.                         ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🗂️  CONTÉM: SecaoPatrimonial                                                                         ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🔗 DEPENDÊNCIAS: DataAnnotations, EF Core                                                           ║
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
    // ==================================================================================================
    // ENTIDADE
    // ==================================================================================================
    // Representa uma seção patrimonial vinculada a um setor.
    // ==================================================================================================
    public class SecaoPatrimonial
    {
        // Identificador único da seção.
        [Key]
        public Guid SecaoId { get; set; }

        // Nome da seção.
        [StringLength(50, ErrorMessage = "O NomeSecao não pode exceder 50 caracteres")]
        [Required(ErrorMessage = "(Obrigatória)")]
        [Display(Name = "NomeSecao")]
        public string? NomeSecao { get; set; }

        // Setor patrimonial vinculado.
        public Guid SetorId { get; set; }

        // Navegação para setor patrimonial.
        [ForeignKey("SetorId")]
        public virtual SetorPatrimonial? SetorPatrimonial { get; set; }

        // Status ativo/inativo.
        public bool Status { get; set; }
    }
}
