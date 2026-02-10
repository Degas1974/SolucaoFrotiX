/* ****************************************************************************************
 * ⚡ ARQUIVO: EmpenhoMulta.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Modelar empenhos de multas de trânsito.
 *
 * 📥 ENTRADAS     : Dados de empenho, órgão autuante e saldos.
 *
 * 📤 SAÍDAS       : Entidade persistida e ViewModel para UI.
 *
 * 🔗 CHAMADA POR  : Cadastros de multas, repositórios e controllers.
 *
 * 🔄 CHAMA        : DataAnnotations, ValidaZero, ForeignKey, SelectListItem.
 *
 * 📦 DEPENDÊNCIAS : FrotiX.Validations, Microsoft.AspNetCore.Mvc.Rendering.
 **************************************************************************************** */

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using FrotiX.Validations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FrotiX.Models
    {
    /****************************************************************************************
     * ⚡ VIEWMODEL: EmpenhoMultaViewModel
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Agrupar empenho de multa e lista de órgãos autuantes.
     *
     * 📥 ENTRADAS     : Entidade EmpenhoMulta e lista de órgãos.
     *
     * 📤 SAÍDAS       : ViewModel para telas de cadastro/edição.
     *
     * 🔗 CHAMADA POR  : Controllers/Views de multas.
     *
     * 🔄 CHAMA        : SelectListItem.
     ****************************************************************************************/
    public class EmpenhoMultaViewModel
        {
        public Guid EmpenhoMultaId { get; set; }
        public EmpenhoMulta EmpenhoMulta { get; set; }
        public IEnumerable<SelectListItem> OrgaoList { get; set; }
        }

    /****************************************************************************************
     * ⚡ MODEL: EmpenhoMulta
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Representar empenho destinado a multas de trânsito.
     *
     * 📥 ENTRADAS     : Nota de empenho, saldos e órgão autuante.
     *
     * 📤 SAÍDAS       : Registro persistido e navegável.
     *
     * 🔗 CHAMADA POR  : Repositórios e controllers.
     *
     * 🔄 CHAMA        : DataAnnotations, ForeignKey, ValidaZero.
     ****************************************************************************************/
    public class EmpenhoMulta
        {

        [Key]
        public Guid EmpenhoMultaId { get; set; }

        [Required(ErrorMessage = "(A nota de Empenho é obrigatória)")]
        [MinLength(12), MaxLength(12)]
        [Display(Name = "Nota de Empenho")]
        public string? NotaEmpenho { get; set; }

        [Required(ErrorMessage = "(O ano de vigência é obrigatório)")]
        [Display(Name = "Ano de Vigência")]
        public int? AnoVigencia { get; set; }

        [ValidaZero(ErrorMessage = "(O saldo inicial é obrigatório)")]
        [Required(ErrorMessage = "(O saldo inicial é obrigatório)")]
        [DataType(DataType.Currency)]
        [Display(Name = "Saldo Inicial (R$)")]
        public double? SaldoInicial { get; set; }

        [DataType(DataType.Currency)]
        [Display(Name = "Saldo Atual (R$)")]
        public double? SaldoAtual { get; set; }

        public bool Status { get; set; }

        [Display(Name = "Órgão Autuante")]
        public Guid OrgaoAutuanteId { get; set; }

        [ForeignKey("OrgaoAutuanteId")]
        public virtual OrgaoAutuante OrgaoAutuante { get; set; }

        }
    }
