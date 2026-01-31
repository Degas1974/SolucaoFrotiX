/* ****************************************************************************************
 * ⚡ ARQUIVO: Contrato.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Modelar contratos, repactuações e itens vinculados.
 *
 * 📥 ENTRADAS     : Dados de contratos, vigência, valores e repactuações.
 *
 * 📤 SAÍDAS       : Entidades persistidas e ViewModel para UI.
 *
 * 🔗 CHAMADA POR  : Módulos de contratos, repositórios e relatórios.
 *
 * 🔄 CHAMA        : DataAnnotations, ValidaZero, ForeignKey, SelectListItem.
 *
 * 📦 DEPENDÊNCIAS : FrotiX.Validations, Microsoft.AspNetCore.Mvc.Rendering, NPOI.
 *
 * ⚠️ ATENÇÃO      : Existem chaves compostas (ver CustoMensalItensContrato).
 **************************************************************************************** */

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using FrotiX.Validations;
using Microsoft.AspNetCore.Mvc.Rendering;
using NPOI.SS.Formula.Functions;

namespace FrotiX.Models
    {
    /****************************************************************************************
     * ⚡ VIEWMODEL: ContratoViewModel
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Agrupar contrato e lista de fornecedores para uso em views.
     *
     * 📥 ENTRADAS     : Entidade Contrato e lista de fornecedores.
     *
     * 📤 SAÍDAS       : ViewModel para telas de cadastro/edição.
     *
     * 🔗 CHAMADA POR  : Controllers/Views de contratos.
     *
     * 🔄 CHAMA        : SelectListItem.
     ****************************************************************************************/
    public class ContratoViewModel
        {
        public Guid ContratoId { get; set; }
        public Contrato Contrato { get; set; }
        public IEnumerable<SelectListItem> FornecedorList { get; set; }
        }

    /****************************************************************************************
     * ⚡ MODEL: Contrato
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Representar contrato de fornecedor com vigência, valores e vínculos.
     *
     * 📥 ENTRADAS     : Número, ano, vigência, valores e fornecedor.
     *
     * 📤 SAÍDAS       : Registro persistido e navegável.
     *
     * 🔗 CHAMADA POR  : Repositórios e controllers.
     *
     * 🔄 CHAMA        : DataAnnotations, ForeignKey, ValidaZero.
     ****************************************************************************************/
    public class Contrato
        {

        [Key]
        public Guid ContratoId { get; set; }

        [ValidaZero(ErrorMessage = "(O número do contrato é obrigatório)")]
        [Required(ErrorMessage = "(O número do contrato é obrigatório)")]
        [Display(Name = "Número")]
        public string? NumeroContrato { get; set; }

        [ValidaZero(ErrorMessage = "(O ano é obrigatório)")]
        [Required(ErrorMessage = "(O ano é obrigatório)")]
        [Display(Name = "Ano Contrato")]
        public string? AnoContrato { get; set; }

        [Required(ErrorMessage = "(A vigência é obrigatória)")]
        [Display(Name = "Vigência")]
        public int? Vigencia { get; set; }

        [Display(Name = "Prorrogação")]
        public int? Prorrogacao { get; set; }

        [ValidaZero(ErrorMessage = "(O ano é obrigatório)")]
        [Required(ErrorMessage = "(O ano é obrigatório)")]
        [Display(Name = "Ano Processo")]
        public int? AnoProcesso { get; set; }

        [Required(ErrorMessage = "(O processo é obrigatório)")]
        [Display(Name = "Número Processo")]
        public string? NumeroProcesso { get; set; }

        [Required(ErrorMessage = "(O objeto é obrigatório)")]
        [Display(Name = "Objeto do Contrato")]
        public string? Objeto { get; set; }

        [Required(ErrorMessage = "(O tipo é obrigatório)")]
        [Display(Name = "Tipo do Contrato")]
        public string? TipoContrato { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Data da Última Repactuação")]
        public DateTime? DataRepactuacao { get; set; }

        [DataType(DataType.DateTime)]
        //[DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "(O início é obrigatório)")]
        [Display(Name = "Início do Contrato")]
        public DateTime? DataInicio { get; set; }

        [DataType(DataType.DateTime)]
        //[DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "(O final é obrigatório)")]
        [Display(Name = "Final do Contrato")]
        public DateTime? DataFim { get; set; }

        [ValidaZero(ErrorMessage = "(O valor é obrigatório)")]
        [Required(ErrorMessage = "(O valor é obrigatório)")]
        //[DisplayFormat(DataFormatString = "{0:C}")]
        [DataType(DataType.Currency)]
        [Display(Name = "Valor (R$)")]
        public double? Valor { get; set; }

        public bool ContratoEncarregados { get; set; }

        public bool ContratoOperadores { get; set; }

        public bool ContratoMotoristas { get; set; }

        public bool ContratoLavadores { get; set; }

        [DataType(DataType.Currency)]
        [Display(Name = "Operador (R$)")]
        public double? CustoMensalEncarregado { get; set; }

        [DataType(DataType.Currency)]
        [Display(Name = "Operador (R$)")]
        public double? CustoMensalOperador { get; set; }

        [DataType(DataType.Currency)]
        [Display(Name = "Motorista (R$)")]
        public double? CustoMensalMotorista { get; set; }

        [DataType(DataType.Currency)]
        [Display(Name = "Lavador (R$)")]
        public double? CustoMensalLavador { get; set; }

        public int? QuantidadeEncarregado { get; set; }

        public int? QuantidadeMotorista { get; set; }

        public int? QuantidadeOperador { get; set; }

        public int? QuantidadeLavador { get; set; }

        [Display(Name = "Ativo/Inativo")]
        public bool Status { get; set; }

        [Display(Name = "Fornecedor")]
        public Guid FornecedorId { get; set; }

        [ForeignKey("FornecedorId")]
        public virtual Fornecedor Fornecedor { get; set; }


        }

    /****************************************************************************************
     * ⚡ MODEL: CustoMensalItensContrato
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Registrar custo mensal por nota fiscal e período para itens de contrato.
     *
     * 📥 ENTRADAS     : NotaFiscalId, ano e mês.
     *
     * 📤 SAÍDAS       : Registro de custo mensal para análises.
     *
     * 🔗 CHAMADA POR  : Relatórios/estudos de custos.
     *
     * 🔄 CHAMA        : DataAnnotations, Column(Order).
     *
     * ⚠️ ATENÇÃO      : Chave composta: NotaFiscalId + Ano + Mes.
     ****************************************************************************************/
    public class CustoMensalItensContrato
        {
        [Key, Column(Order = 0)]
        public Guid NotaFiscalId { get; set; }

        [Key, Column(Order = 1)]
        public int Ano { get; set; }

        [Key, Column(Order = 2)]
        public int Mes { get; set; }

        public double? CustoMensalOperador { get; set; }

        public double? CustoMensalMotorista { get; set; }

        public double? CustoMensalLavador { get; set; }

        }


    /****************************************************************************************
     * ⚡ MODEL: RepactuacaoContrato
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Representar repactuações associadas a um contrato.
     *
     * 📥 ENTRADAS     : Datas, valores e percentuais.
     *
     * 📤 SAÍDAS       : Registro de repactuação do contrato.
     *
     * 🔗 CHAMADA POR  : Fluxos de repactuação.
     *
     * 🔄 CHAMA        : ForeignKey, NotMapped.
     ****************************************************************************************/
    public class RepactuacaoContrato
    {
        [Key]
        public Guid RepactuacaoContratoId { get; set; }

        public DateTime? DataRepactuacao { get; set; }

        public string? Descricao { get; set; }

        public double? Valor { get; set; }

        [Display(Name = "Percentual (%)")]
        public double? Percentual { get; set; }

        [Display(Name = "Contrato")]
        public Guid ContratoId { get; set; }

        [ForeignKey("ContratoId")]
        public virtual Contrato Contrato { get; set; }

        public int? Vigencia { get; set; }

        public int? Prorrogacao { get; set; }

        [NotMapped]
        public bool AtualizaContrato { get; set; }
    }

    /****************************************************************************************
     * ⚡ MODEL: ItemVeiculoContrato
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Representar itens de veículos vinculados a uma repactuação de contrato.
     *
     * 📥 ENTRADAS     : Descrição, quantidade e valor unitário.
     *
     * 📤 SAÍDAS       : Registro de item associado à repactuação.
     *
     * 🔗 CHAMADA POR  : Fluxos de repactuação/itens.
     *
     * 🔄 CHAMA        : ForeignKey.
     ****************************************************************************************/
    public class ItemVeiculoContrato
        {
        [Key]
        public Guid ItemVeiculoId { get; set; }

        public int? NumItem { get; set; }

        public string? Descricao { get; set; }

        public int? Quantidade { get; set; }

        public double? ValorUnitario { get; set; }

        [Display(Name = "RepactuacaoContrato")]
        public Guid RepactuacaoContratoId { get; set; }

        [ForeignKey("RepactuacaoContratoId")]
        public virtual RepactuacaoContrato RepactuacaoContrato { get; set; }
        }


    /****************************************************************************************
     * ⚡ MODEL: RepactuacaoTerceirizacao
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Registrar repactuação de serviços terceirizados.
     *
     * 📥 ENTRADAS     : Valores e quantidades por função.
     *
     * 📤 SAÍDAS       : Registro de repactuação de terceirização.
     *
     * 🔗 CHAMADA POR  : Fluxos de repactuação.
     *
     * 🔄 CHAMA        : ForeignKey.
     ****************************************************************************************/
    public class RepactuacaoTerceirizacao
        {
        [Key]
        public Guid RepactuacaoTerceirizacaoId { get; set; }

        public DateTime? DataRepactuacao { get; set; }

        public double? ValorEncarregado { get; set; }

        public double? ValorOperador { get; set; }

        public double? ValorMotorista { get; set; }

        public double? ValorLavador { get; set; }

        public int? QtdEncarregados { get; set; }

        public int? QtdOperadores { get; set; }

        public int? QtdMotoristas { get; set; }

        public int? QtdLavadores { get; set; }

        public Guid RepactuacaoContratoId { get; set; }

        [ForeignKey("RepactuacaoContratoId")]
        public virtual RepactuacaoContrato RepactuacaoContrato { get; set; }

        }


    /****************************************************************************************
     * ⚡ MODEL: RepactuacaoServicos
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Registrar repactuação de serviços gerais do contrato.
     *
     * 📥 ENTRADAS     : Datas e valores de repactuação.
     *
     * 📤 SAÍDAS       : Registro associado ao contrato.
     *
     * 🔗 CHAMADA POR  : Fluxos de repactuação.
     *
     * 🔄 CHAMA        : ForeignKey.
     ****************************************************************************************/
    public class RepactuacaoServicos
        {
        [Key]
        public Guid RepactuacaoServicoId { get; set; }

        public DateTime? DataRepactuacao { get; set; }

        public double? Valor { get; set; }

        public Guid RepactuacaoContratoId { get; set; }

        [ForeignKey("RepactuacaoContratoId")]
        public virtual RepactuacaoContrato RepactuacaoContrato { get; set; }

        }



    }
