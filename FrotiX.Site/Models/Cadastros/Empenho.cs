/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: Empenho.cs                                                                            ║
   ║ 📂 CAMINHO: Models/Cadastros/                                                                     ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
   ║    Modelos para empenhos orçamentários e listas de apoio.                                         ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 CLASSES DISPONÍVEIS:                                                                           ║
   ║    • EmpenhoViewModel                                                                             ║
   ║    • Empenho                                                                                      ║
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
    // │ 🎯 CLASSE: EmpenhoViewModel                                                                  │
    // ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    //
    // 🎯 OBJETIVO:
    // Agrupar empenho e listas de contratos/atas para uso em views.
    //
    // 🔗 RASTREABILIDADE:
    // ⬅️ CHAMADO POR : Controllers/Views de empenhos
    // ➡️ CHAMA       : SelectListItem
    //
    public class EmpenhoViewModel
    {
        public Guid EmpenhoId { get; set; }
        public Empenho Empenho { get; set; }
        public IEnumerable<SelectListItem> ContratoList { get; set; }
        public IEnumerable<SelectListItem> AtaList { get; set; }
    }

    // ╭───────────────────────────────────────────────────────────────────────────────────────────────╮
    // │ 🎯 CLASSE: Empenho                                                                           │
    // ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    //
    // 🎯 OBJETIVO:
    // Representar empenho orçamentário vinculado a contrato ou ata.
    //
    // 🔗 RASTREABILIDADE:
    // ⬅️ CHAMADO POR : Repositórios/Controllers
    // ➡️ CHAMA       : DataAnnotations, ForeignKey
    //
    public class Empenho
    {
        [Key]
        public Guid EmpenhoId { get; set; }

        [Required(ErrorMessage = "(A nota de Empenho é obrigatória)")]
        [MinLength(12), MaxLength(12)]
        [Display(Name = "Nota de Empenho")]
        public string? NotaEmpenho { get; set; }

        [DataType(DataType.DateTime)]
        [Required(ErrorMessage = "(A data de emissão é obrigatória)")]
        [Display(Name = "Data de Emissão")]
        public DateTime? DataEmissao { get; set; }

        [DataType(DataType.DateTime)]
        [Required(ErrorMessage = "(A data de vigência inicial é obrigatória)")]
        [Display(Name = "Vigência Inicial")]
        public DateTime? VigenciaInicial { get; set; }

        [DataType(DataType.DateTime)]
        [Required(ErrorMessage = "(A data de vigência final é obrigatória)")]
        [Display(Name = "Vigência Final")]
        public DateTime? VigenciaFinal { get; set; }

        [ValidaZero(ErrorMessage = "(O ano de vigência é obrigatório)")]
        [Required(ErrorMessage = "(O ano de vigência é obrigatório)")]
        [Display(Name = "Ano de Vigência")]
        public int? AnoVigencia { get; set; }

        [ValidaZero(ErrorMessage = "(O saldo inicial é obrigatório)")]
        [Required(ErrorMessage = "(O saldo inicial é obrigatório)")]
        [DataType(DataType.Currency)]
        [Display(Name = "Saldo Inicial (R$)")]
        public double? SaldoInicial { get; set; }

        [DataType(DataType.Currency)]
        [Display(Name = "Saldo Final (R$)")]
        public double? SaldoFinal { get; set; }

        [Display(Name = "Contrato")]
        public Guid? ContratoId { get; set; }

        [ForeignKey("ContratoId")]
        public virtual Contrato Contrato { get; set; }

        [Display(Name = "Ata de Registro de Preços")]
        public Guid? AtaId { get; set; }

        [ForeignKey("AtaId")]
        public virtual AtaRegistroPrecos AtaRegistroPrecos { get; set; }
    }
}
