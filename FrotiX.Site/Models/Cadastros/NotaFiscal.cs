/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: NotaFiscal.cs                                                                           ║
   ║ 📂 CAMINHO: /Models/Cadastros                                                                       ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO: Entidade e ViewModels para notas fiscais de contratos (número, data, empenho).        ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 CLASSES: NotaFiscal (NotaFiscalId, NumeroNF, EmpenhoId), NotaFiscalViewModel (EmpenhoList)       ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🔗 DEPS: FrotiX.Validations, SelectListItem                                                         ║
   ║ 📅 Atualizado: 2026 | 👤 FrotiX Team | 📝 Versão: 2.0                                              ║
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
    public class NotaFiscalViewModel
    {
        public Guid NotaFiscalId { get; set; }
        public Guid EmpenhoId { get; set; }

        public NotaFiscal? NotaFiscal { get; set; }
        public IEnumerable<SelectListItem>? EmpenhoList { get; set; }
        public IEnumerable<SelectListItem>? ContratoList { get; set; }
        public IEnumerable<SelectListItem>? AtaList { get; set; }
    }

    public class NotaFiscal
    {
        [Key]
        public Guid NotaFiscalId { get; set; }

        [Required(ErrorMessage = "(O número da Nota Fiscal é obrigatóri0)")]
        [Display(Name = "Número da Nota Fiscal")]
        public int? NumeroNF { get; set; }

        [DataType(DataType.DateTime)]
        [Required(ErrorMessage = "(A data de emissão é obrigatória)")]
        [Display(Name = "Data de Emissão")]
        public DateTime? DataEmissao { get; set; }

        [ValidaZero(ErrorMessage = "(O valor da Nota Fiscal é obrigatório)")]
        [Required(ErrorMessage = "(O valor da Nota Fiscal é obrigatório)")]
        [DataType(DataType.Currency)]
        [Display(Name = "Valor (R$)")]
        public double? ValorNF { get; set; }

        [Display(Name = "Tipo de Nota Fiscal")]
        [Required(ErrorMessage = "(O Tipo da Nota Fiscal é obrigatório)")]
        public string? TipoNF { get; set; }

        [Display(Name = "Objeto da Nota Fiscal")]
        public string? Objeto { get; set; }

        [DataType(DataType.Currency)]
        [Display(Name = "Valor de Glosa (R$)")]
        public double? ValorGlosa { get; set; }

        [Display(Name = "Motivo da Glosa")]
        [MaxLength(150)]
        public string? MotivoGlosa { get; set; }

        [Display(Name = "Ano/Ref.")]
        [Required(ErrorMessage = "(O ano de referência é obrigatório)")]
        public int? AnoReferencia { get; set; }

        [Display(Name = "Mês/Ref.")]
        [Required(ErrorMessage = "(O mês de referência é obrigatório)")]
        public int? MesReferencia { get; set; }

        [Display(Name = "Contrato")]
        public Guid? ContratoId { get; set; }

        [ForeignKey("ContratoId")]
        public virtual Contrato? Contrato { get; set; }

        [Display(Name = "Ata de Registro de Preços")]
        public Guid? AtaId { get; set; }

        [ForeignKey("ContratoId")]
        public virtual AtaRegistroPrecos? AtaRegistroPrecos { get; set; }

        [ValidaLista(ErrorMessage = "(O Empenho é obrigatório)")]
        [Display(Name = "Empenho")]
        public Guid? EmpenhoId { get; set; }

        [ForeignKey("EmpenhoId")]
        public virtual Empenho? Empenho { get; set; }

        public Guid? VeiculoId { get; set; }

        [ForeignKey("VeiculoId")]
        public virtual Veiculo? Veiculo { get; set; }

        [NotMapped]
        [Display(Name = "(R$) Mensal Gasolina")]
        public double? MediaGasolina { get; set; }

        [NotMapped]
        [Display(Name = "(R$) Mensal Diesel")]
        public double? MediaDiesel { get; set; }

        [NotMapped]
        [Display(Name = "(R$) Operador")]
        public double? CustoMensalOperador { get; set; }

        [NotMapped]
        [Display(Name = "(R$) Motorista")]
        public double? CustoMensalMotorista { get; set; }

        [NotMapped]
        [Display(Name = "(R$) Lavador")]
        public double? CustoMensalLavador { get; set; }
    }
}
