/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 📌 ARQUIVO: SetorPatrimonial.cs                                                                     ║
   ║ 📂 CAMINHO: /Models/Cadastros                                                                       ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🧭 OBJETIVO: Definir setores patrimoniais responsáveis pelos bens do órgão.                         ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🗂️  CONTÉM: SetorPatrimonial                                                                         ║
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
    // Representa um setor patrimonial do órgão.
    // ==================================================================================================
    public class SetorPatrimonial
    {
        // Identificador único do setor.
        [Key]
        public Guid SetorId { get; set; }

        // Nome do setor patrimonial.
        [StringLength(50, ErrorMessage = "O Nome do Setor não pode exceder 50 caracteres")]
        [Required(ErrorMessage = "(Obrigatória)")]
        [Display(Name = "Nome do Setor")]
        public string? NomeSetor { get; set; }

        // Identificador do detentor responsável.
        public string? DetentorId { get; set; }

        // Status ativo/inativo.
        public bool Status { get; set; }

        // Indica se o setor realiza baixa patrimonial.
        public bool SetorBaixa { get; set; }
    }
}
