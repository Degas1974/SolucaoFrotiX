/* ****************************************************************************************
 * ⚡ ARQUIVO: TipoMulta.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Cadastrar tipos de multa com artigos, descrição e códigos Denatran.
 *
 * 📥 ENTRADAS     : Artigo, descrição e códigos da infração.
 *
 * 📤 SAÍDAS       : Entidade persistida para cadastro de multas.
 *
 * 🔗 CHAMADA POR  : Cadastros de multas e relatórios.
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
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FrotiX.Models
{
    /****************************************************************************************
     * ⚡ MODEL: TipoMulta
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Representar um tipo de multa de trânsito.
     *
     * 📥 ENTRADAS     : Artigo, descrição, infração e códigos.
     *
     * 📤 SAÍDAS       : Registro persistido para uso em autuações.
     *
     * 🔗 CHAMADA POR  : Processos de multas.
     ****************************************************************************************/
    public class TipoMulta
    {
        // Identificador único do tipo de multa.
        [Key]
        public Guid TipoMultaId { get; set; }

        // Artigo/parágrafo/inciso.
        [StringLength(100, ErrorMessage = "O artigo não pode exceder 100 caracteres")]
        [Required(ErrorMessage = "(O artigo/parágrafo/inciso da multa é obrigatório)")]
        [Display(Name = "Artigo/Parágrafo/Inciso")]
        public string? Artigo { get; set; }

        // Descrição da multa.
        [Required(ErrorMessage = "(A descrição da multa é obrigatório)")]
        [Display(Name = "Descrição")]
        public string? Descricao { get; set; }

        // Infração associada.
        [Required(ErrorMessage = "(A infração da multa é obrigatória)")]
        [Display(Name = "Infração")]
        public string? Infracao { get; set; }

        // Código Denatran.
        [Display(Name = "Código Denatran")]
        public string? CodigoDenatran { get; set; }

        // Desdobramento Denatran.
        [Display(Name = "Desdobramento Denatran")]
        public string? Desdobramento { get; set; }
    }
}
