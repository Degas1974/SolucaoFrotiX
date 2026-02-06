/* ****************************************************************************************
 * ⚡ ARQUIVO: NotaFiscal.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Registrar notas fiscais vinculadas a contratos, empenhos e veículos.
 *
 * 📥 ENTRADAS     : Dados da nota, vínculos e referências de período.
 *
 * 📤 SAÍDAS       : Entidade persistida e ViewModel para UI.
 *
 * 🔗 CHAMADA POR  : Módulos financeiros e relatórios.
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
     * ⚡ VIEWMODEL: NotaFiscalViewModel
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Transportar a nota fiscal e listas de seleção (empenho/contrato/ata).
     *
     * 📥 ENTRADAS     : NotaFiscal e listas auxiliares.
     *
     * 📤 SAÍDAS       : ViewModel para telas financeiras.
     *
     * 🔗 CHAMADA POR  : Controllers/Views de notas fiscais.
     *
     * 🔄 CHAMA        : SelectListItem.
     ****************************************************************************************/
    public class NotaFiscalViewModel
    {
        // Identificador da nota fiscal.
        public Guid NotaFiscalId { get; set; }

        // Empenho selecionado no formulário.
        public Guid EmpenhoId { get; set; }

        // Entidade principal do formulário.
        public NotaFiscal? NotaFiscal { get; set; }

        // Lista de empenhos para seleção.
        public IEnumerable<SelectListItem>? EmpenhoList { get; set; }

        // Lista de contratos para seleção.
        public IEnumerable<SelectListItem>? ContratoList { get; set; }

        // Lista de atas para seleção.
        public IEnumerable<SelectListItem>? AtaList { get; set; }
    }

    /****************************************************************************************
     * ⚡ MODEL: NotaFiscal
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Representar uma nota fiscal associada a contratos/empenhos.
     *
     * 📥 ENTRADAS     : Número, datas, valores e vínculos.
     *
     * 📤 SAÍDAS       : Registro persistido para controle financeiro.
     *
     * 🔗 CHAMADA POR  : Repositórios e controllers.
     *
     * 🔄 CHAMA        : ForeignKey, ValidaZero, NotMapped.
     *
     * ⚠️ ATENÇÃO      : Há campos NotMapped para cálculo/exibição de custos.
     ****************************************************************************************/
    public class NotaFiscal
    {
        // Identificador único da nota fiscal.
        [Key]
        public Guid NotaFiscalId { get; set; }

        // Número da nota fiscal.
        [Required(ErrorMessage = "(O número da Nota Fiscal é obrigatóri0)")]
        [Display(Name = "Número da Nota Fiscal")]
        public int? NumeroNF { get; set; }

        // Data de emissão.
        [DataType(DataType.DateTime)]
        [Required(ErrorMessage = "(A data de emissão é obrigatória)")]
        [Display(Name = "Data de Emissão")]
        public DateTime? DataEmissao { get; set; }

        // Valor total da nota fiscal.
        [ValidaZero(ErrorMessage = "(O valor da Nota Fiscal é obrigatório)")]
        [Required(ErrorMessage = "(O valor da Nota Fiscal é obrigatório)")]
        [DataType(DataType.Currency)]
        [Display(Name = "Valor (R$)")]
        public double? ValorNF { get; set; }

        // Tipo de nota fiscal.
        [Display(Name = "Tipo de Nota Fiscal")]
        [Required(ErrorMessage = "(O Tipo da Nota Fiscal é obrigatório)")]
        public string? TipoNF { get; set; }

        // Objeto/descrição da nota.
        [Display(Name = "Objeto da Nota Fiscal")]
        public string? Objeto { get; set; }

        // Valor de glosa (quando aplicável).
        [DataType(DataType.Currency)]
        [Display(Name = "Valor de Glosa (R$)")]
        public double? ValorGlosa { get; set; }

        // Motivo da glosa.
        [Display(Name = "Motivo da Glosa")]
        [MaxLength(150)]
        public string? MotivoGlosa { get; set; }

        // Ano de referência.
        [Display(Name = "Ano/Ref.")]
        [Required(ErrorMessage = "(O ano de referência é obrigatório)")]
        public int? AnoReferencia { get; set; }

        // Mês de referência.
        [Display(Name = "Mês/Ref.")]
        [Required(ErrorMessage = "(O mês de referência é obrigatório)")]
        public int? MesReferencia { get; set; }

        // Contrato associado.
        [Display(Name = "Contrato")]
        public Guid? ContratoId { get; set; }

        // Navegação para contrato.
        [ForeignKey("ContratoId")]
        public virtual Contrato? Contrato { get; set; }

        // Ata de registro de preços associada.
        [Display(Name = "Ata de Registro de Preços")]
        public Guid? AtaId { get; set; }

        // Navegação para ata (obs.: FK usa ContratoId no código atual).
        [ForeignKey("ContratoId")]
        public virtual AtaRegistroPrecos? AtaRegistroPrecos { get; set; }

        // Empenho associado.
        [ValidaLista(ErrorMessage = "(O Empenho é obrigatório)")]
        [Display(Name = "Empenho")]
        public Guid? EmpenhoId { get; set; }

        // Navegação para empenho.
        [ForeignKey("EmpenhoId")]
        public virtual Empenho? Empenho { get; set; }

        // Veículo associado.
        public Guid? VeiculoId { get; set; }

        // Navegação para veículo.
        [ForeignKey("VeiculoId")]
        public virtual Veiculo? Veiculo { get; set; }

        // Média mensal de gasolina (uso exibicional).
        [NotMapped]
        [Display(Name = "(R$) Mensal Gasolina")]
        public double? MediaGasolina { get; set; }

        // Média mensal de diesel (uso exibicional).
        [NotMapped]
        [Display(Name = "(R$) Mensal Diesel")]
        public double? MediaDiesel { get; set; }

        // Custo mensal de operador (uso exibicional).
        [NotMapped]
        [Display(Name = "(R$) Operador")]
        public double? CustoMensalOperador { get; set; }

        // Custo mensal de motorista (uso exibicional).
        [NotMapped]
        [Display(Name = "(R$) Motorista")]
        public double? CustoMensalMotorista { get; set; }

        // Custo mensal de lavador (uso exibicional).
        [NotMapped]
        [Display(Name = "(R$) Lavador")]
        public double? CustoMensalLavador { get; set; }
    }
}
