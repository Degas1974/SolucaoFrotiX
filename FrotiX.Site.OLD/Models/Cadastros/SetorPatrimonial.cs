/* ****************************************************************************************
 * ⚡ ARQUIVO: SetorPatrimonial.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Definir setores patrimoniais responsáveis pelos bens do órgão.
 *
 * 📥 ENTRADAS     : Nome do setor, detentor responsável e flags de baixa.
 *
 * 📤 SAÍDAS       : Entidade persistida para gestão patrimonial.
 *
 * 🔗 CHAMADA POR  : Inventário e fluxos patrimoniais.
 *
 * 🔄 CHAMA        : DataAnnotations.
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
     * ⚡ MODEL: SetorPatrimonial
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Representar setor patrimonial do órgão.
     *
     * 📥 ENTRADAS     : Nome, detentor e status.
     *
     * 📤 SAÍDAS       : Registro persistido do setor.
     *
     * 🔗 CHAMADA POR  : Gestão patrimonial.
     ****************************************************************************************/
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
