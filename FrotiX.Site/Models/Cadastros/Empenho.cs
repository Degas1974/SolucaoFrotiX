// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ ARQUIVO: Empenho.cs                                                         ║
// ║ PROJETO: FrotiX - Sistema de Gestão de Frotas                               ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO:                                                                   ║
// ║ Entidade para gestão de empenhos orçamentários vinculados a contratos/atas. ║
// ║ Controla saldos disponíveis para pagamento de notas fiscais.                ║
// ║                                                                              ║
// ║ CLASSES:                                                                      ║
// ║ • EmpenhoViewModel - ViewModel com dropdowns de contrato e ata              ║
// ║ • Empenho - Entidade principal de empenho                                   ║
// ║                                                                              ║
// ║ PROPRIEDADES:                                                                ║
// ║ • EmpenhoId [Key] - Identificador único                                     ║
// ║ • NotaEmpenho - Número da nota de empenho (12 caracteres)                   ║
// ║ • DataEmissao - Data de emissão do empenho                                  ║
// ║ • VigenciaInicial/Final - Período de vigência                               ║
// ║ • AnoVigencia - Ano de exercício orçamentário                               ║
// ║ • SaldoInicial, SaldoFinal - Valores em R$                                  ║
// ║ • ContratoId → Contrato (FK opcional)                                       ║
// ║ • AtaId → AtaRegistroPrecos (FK opcional)                                   ║
// ║                                                                              ║
// ║ REGRAS DE NEGÓCIO:                                                           ║
// ║ • Empenho pode estar vinculado a Contrato OU Ata (não ambos)                ║
// ║ • SaldoFinal é atualizado conforme movimentações                            ║
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
    public class EmpenhoViewModel
    {
        public Guid EmpenhoId { get; set; }
        public Empenho Empenho { get; set; }
        public IEnumerable<SelectListItem> ContratoList { get; set; }
        public IEnumerable<SelectListItem> AtaList { get; set; }
    }

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
