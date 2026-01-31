/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: EmpenhoMulta.cs                                                                       ║
   ║ 📂 CAMINHO: Models/Cadastros/                                                                     ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
   ║    Modelos para empenhos de multas de trânsito.                                                   ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 CLASSES DISPONÍVEIS:                                                                           ║
   ║    • EmpenhoMultaViewModel                                                                        ║
   ║    • EmpenhoMulta                                                                                ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🔗 DEPENDÊNCIAS: FrotiX.Validations, SelectListItem                                                ║
   ║ 📅 ATUALIZAÇÃO: 31/01/2026 | 👤 AUTOR: FrotiX Team | 📝 VERSÃO: 2.0                                 ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝ */

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
    // ╭───────────────────────────────────────────────────────────────────────────────────────────────╮
    // │ 🎯 CLASSE: EmpenhoMultaViewModel                                                             │
    // ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    //
    // 🎯 OBJETIVO:
    // Agrupar empenho de multa e lista de órgãos autuantes para uso em views.
    //
    // 🔗 RASTREABILIDADE:
    // ⬅️ CHAMADO POR : Controllers/Views de multas
    // ➡️ CHAMA       : SelectListItem
    //
    public class EmpenhoMultaViewModel
        {
        public Guid EmpenhoMultaId { get; set; }
        public EmpenhoMulta EmpenhoMulta { get; set; }
        public IEnumerable<SelectListItem> OrgaoList { get; set; }
        }

    // ╭───────────────────────────────────────────────────────────────────────────────────────────────╮
    // │ 🎯 CLASSE: EmpenhoMulta                                                                      │
    // ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    //
    // 🎯 OBJETIVO:
    // Representar empenho destinado a multas de trânsito.
    //
    // 🔗 RASTREABILIDADE:
    // ⬅️ CHAMADO POR : Repositórios/Controllers
    // ➡️ CHAMA       : DataAnnotations, ForeignKey
    //
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

