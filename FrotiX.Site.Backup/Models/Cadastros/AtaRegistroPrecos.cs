/* ****************************************************************************************
 * ⚡ ARQUIVO: AtaRegistroPrecos.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Modelar atas de registro de preços e seus itens/repactuações.
 *
 * 📥 ENTRADAS     : Dados de atas, fornecedores e itens vinculados.
 *
 * 📤 SAÍDAS       : Entidades persistidas e ViewModel para UI.
 *
 * 🔗 CHAMADA POR  : Módulos de contratos/atas, controllers e repositórios.
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
     * ⚡ VIEWMODEL: AtaRegistroPrecosViewModel
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Agrupar ata e lista de fornecedores para uso em views.
     *
     * 📥 ENTRADAS     : AtaRegistroPrecos e lista de fornecedores.
     *
     * 📤 SAÍDAS       : ViewModel para telas de cadastro/edição.
     *
     * 🔗 CHAMADA POR  : Controllers/Views de atas.
     *
     * 🔄 CHAMA        : SelectListItem.
     ****************************************************************************************/
    public class AtaRegistroPrecosViewModel
        {
        public Guid AtaId { get; set; }
        public AtaRegistroPrecos AtaRegistroPrecos { get; set; }
        public IEnumerable<SelectListItem> FornecedorList { get; set; }
        }

    /****************************************************************************************
     * ⚡ MODEL: AtaRegistroPrecos
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Representar uma ata de registro de preços com vínculo a fornecedor.
     *
     * 📥 ENTRADAS     : Dados de processo, vigência e valor.
     *
     * 📤 SAÍDAS       : Registro persistido e navegável.
     *
     * 🔗 CHAMADA POR  : Repositórios e controllers de atas.
     *
     * 🔄 CHAMA        : DataAnnotations, ForeignKey, ValidaZero.
     ****************************************************************************************/
    public class AtaRegistroPrecos
        {

        [Key]
        public Guid AtaId { get; set; }

        [ValidaZero(ErrorMessage = "(O número da Ata é obrigatório)")]
        [Required(ErrorMessage = "(O número do Ata é obrigatório)")]
        [Display(Name = "Número Ata")]
        public string? NumeroAta { get; set; }

        [ValidaZero(ErrorMessage = "(O ano é obrigatório)")]
        [Required(ErrorMessage = "(O ano é obrigatório)")]
        [Display(Name = "Ano Ata")]
        public string? AnoAta { get; set; }

        [ValidaZero(ErrorMessage = "(O ano é obrigatório)")]
        [Required(ErrorMessage = "(O ano é obrigatório)")]
        [Display(Name = "Ano Processo")]
        public int? AnoProcesso { get; set; }

        [Required(ErrorMessage = "(O processo é obrigatório)")]
        [Display(Name = "Número Processo")]
        public string NumeroProcesso { get; set; }

        [Required(ErrorMessage = "(O objeto é obrigatório)")]
        [Display(Name = "Objeto da Ata")]
        public string Objeto { get; set; }

        [DataType(DataType.DateTime)]
        [Required(ErrorMessage = "(O início é obrigatório)")]
        [Display(Name = "Início da Ata")]
        public DateTime? DataInicio { get; set; }

        [DataType(DataType.DateTime)]
        [Required(ErrorMessage = "(O final é obrigatório)")]
        [Display(Name = "Final da Ata")]
        public DateTime? DataFim { get; set; }

        [ValidaZero(ErrorMessage = "(O valor é obrigatório)")]
        [Required(ErrorMessage = "(O valor é obrigatório)")]
        [DataType(DataType.Currency)]
        [Display(Name = "Valor (R$)")]
        public double? Valor { get; set; }

        [Display(Name = "Ativo/Inativo")]
        public bool Status { get; set; }

        [Display(Name = "Fornecedor")]
        public Guid FornecedorId { get; set; }

        [ForeignKey("FornecedorId")]
        public virtual Fornecedor Fornecedor { get; set; }

        }

    /****************************************************************************************
     * ⚡ MODEL: RepactuacaoAta
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Representar repactuações vinculadas a uma ata.
     *
     * 📥 ENTRADAS     : Datas e descrição de repactuação.
     *
     * 📤 SAÍDAS       : Registro de histórico de repactuação.
     *
     * 🔗 CHAMADA POR  : Processos de repactuação.
     *
     * 🔄 CHAMA        : ForeignKey.
     ****************************************************************************************/
    public class RepactuacaoAta
        {
        [Key]
        public Guid RepactuacaoAtaId { get; set; }

        public DateTime? DataRepactuacao { get; set; }

        public string? Descricao { get; set; }

        [Display(Name = "Ata")]
        public Guid AtaId { get; set; }

        [ForeignKey("AtaId")]
        public virtual AtaRegistroPrecos AtaRegistroPrecos { get; set; }

        }


    /****************************************************************************************
     * ⚡ MODEL: ItemVeiculoAta
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Representar itens de veículos vinculados a uma repactuação de ata.
     *
     * 📥 ENTRADAS     : Descrição, quantidade, valor e vínculo à repactuação.
     *
     * 📤 SAÍDAS       : Registro de item de ata para consultas/relatórios.
     *
     * 🔗 CHAMADA POR  : Processos de repactuação e itens.
     *
     * 🔄 CHAMA        : ForeignKey.
     ****************************************************************************************/
    public class ItemVeiculoAta
        {
        [Key]
        public Guid ItemVeiculoAtaId { get; set; }

        public int? NumItem { get; set; }

        public string? Descricao { get; set; }

        public int? Quantidade { get; set; }

        public double? ValorUnitario { get; set; }

        [Display(Name = "RepactuacaoAta")]
        public Guid RepactuacaoAtaId { get; set; }

        [ForeignKey("RepactuacaoAtaId")]
        public virtual RepactuacaoAta RepactuacaoAta { get; set; }
        }



    }


