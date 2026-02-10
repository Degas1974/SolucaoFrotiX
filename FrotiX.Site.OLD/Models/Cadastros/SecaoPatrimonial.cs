/* ****************************************************************************************
 * ⚡ ARQUIVO: SecaoPatrimonial.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Definir seções patrimoniais vinculadas a setores patrimoniais.
 *
 * 📥 ENTRADAS     : Nome da seção e vínculo com setor patrimonial.
 *
 * 📤 SAÍDAS       : Entidade persistida para organização patrimonial.
 *
 * 🔗 CHAMADA POR  : Gestão patrimonial e inventário.
 *
 * 🔄 CHAMA        : DataAnnotations, ForeignKey.
 *
 * 📦 DEPENDÊNCIAS : System.ComponentModel.DataAnnotations.
 **************************************************************************************** */

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
    /****************************************************************************************
     * ⚡ MODEL: SecaoPatrimonial
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Representar seção patrimonial vinculada a um setor.
     *
     * 📥 ENTRADAS     : Nome e setor associado.
     *
     * 📤 SAÍDAS       : Registro persistido da seção.
     *
     * 🔗 CHAMADA POR  : Fluxos patrimoniais.
     ****************************************************************************************/
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
