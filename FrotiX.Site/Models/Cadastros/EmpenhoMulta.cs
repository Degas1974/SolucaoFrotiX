// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ ARQUIVO: EmpenhoMulta.cs                                                    ║
// ║ PROJETO: FrotiX - Sistema de Gestão de Frotas                               ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO:                                                                   ║
// ║ Entidade para gestão de empenhos específicos para pagamento de multas.      ║
// ║ Vinculado a órgão autuante para controle de saldos por DETRAN/PRF/etc.      ║
// ║                                                                              ║
// ║ CLASSES:                                                                      ║
// ║ • EmpenhoMultaViewModel - ViewModel com dropdown de órgãos autuantes        ║
// ║ • EmpenhoMulta - Entidade de empenho de multas                              ║
// ║                                                                              ║
// ║ PROPRIEDADES:                                                                ║
// ║ • EmpenhoMultaId [Key] - Identificador único                                ║
// ║ • NotaEmpenho - Número da nota de empenho (12 caracteres)                   ║
// ║ • AnoVigencia - Ano de exercício orçamentário                               ║
// ║ • SaldoInicial - Valor empenhado em R$                                      ║
// ║ • SaldoAtual - Saldo disponível após movimentações                          ║
// ║ • Status - Ativo/Inativo                                                    ║
// ║ • OrgaoAutuanteId → OrgaoAutuante (FK)                                      ║
// ║                                                                              ║
// ║ USO:                                                                          ║
// ║ • Controle de verba para pagamento de multas                                ║
// ║ • Vinculação de pagamentos de multas ao empenho correto                     ║
// ║                                                                              ║
// ║ DOCUMENTADO EM: 2026-01-28 | LOTE: 18                                       ║
// ╚══════════════════════════════════════════════════════════════════════════════╝
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
    public class EmpenhoMultaViewModel
        {
        public Guid EmpenhoMultaId { get; set; }
        public EmpenhoMulta EmpenhoMulta { get; set; }
        public IEnumerable<SelectListItem> OrgaoList { get; set; }
        }

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


