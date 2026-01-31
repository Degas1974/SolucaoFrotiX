/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: AtaRegistroPrecos.cs                                                                  ║
   ║ 📂 CAMINHO: Models/Cadastros/                                                                     ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
   ║    Modelos para Atas de Registro de Preços e seus itens/repactuações.                              ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 CLASSES DISPONÍVEIS:                                                                           ║
   ║    • AtaRegistroPrecosViewModel                                                                   ║
   ║    • AtaRegistroPrecos                                                                            ║
   ║    • RepactuacaoAta                                                                               ║
   ║    • ItemVeiculoAta                                                                               ║
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
    // │ 🎯 CLASSE: AtaRegistroPrecosViewModel                                                        │
    // ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    //
    // 🎯 OBJETIVO:
    // Agrupar AtaRegistroPrecos com lista de fornecedores para uso em views.
    //
    // 🔗 RASTREABILIDADE:
    // ⬅️ CHAMADO POR : Controllers/Views de atas
    // ➡️ CHAMA       : SelectListItem
    //
    public class AtaRegistroPrecosViewModel
        {
        public Guid AtaId { get; set; }
        public AtaRegistroPrecos AtaRegistroPrecos { get; set; }
        public IEnumerable<SelectListItem> FornecedorList { get; set; }
        }

    // ╭───────────────────────────────────────────────────────────────────────────────────────────────╮
    // │ 🎯 CLASSE: AtaRegistroPrecos                                                                 │
    // ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    //
    // 🎯 OBJETIVO:
    // Representar uma Ata de Registro de Preços com vínculo a fornecedor.
    //
    // 🔗 RASTREABILIDADE:
    // ⬅️ CHAMADO POR : Repositórios, Controllers
    // ➡️ CHAMA       : DataAnnotations, ForeignKey
    //
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

    // ╭───────────────────────────────────────────────────────────────────────────────────────────────╮
    // │ 🎯 CLASSE: RepactuacaoAta                                                                     │
    // ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    //
    // 🎯 OBJETIVO:
    // Representar repactuações de uma ata de registro de preços.
    //
    // 🔗 RASTREABILIDADE:
    // ⬅️ CHAMADO POR : Processos de repactuação
    // ➡️ CHAMA       : ForeignKey
    //
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


    // ╭───────────────────────────────────────────────────────────────────────────────────────────────╮
    // │ 🎯 CLASSE: ItemVeiculoAta                                                                     │
    // ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    //
    // 🎯 OBJETIVO:
    // Representar itens de veículos vinculados a uma repactuação de ata.
    //
    // 🔗 RASTREABILIDADE:
    // ⬅️ CHAMADO POR : Processos de repactuação/itens
    // ➡️ CHAMA       : ForeignKey
    //
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

