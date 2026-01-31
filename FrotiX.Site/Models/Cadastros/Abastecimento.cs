/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: Abastecimento.cs                                                                      ║
   ║ 📂 CAMINHO: Models/Cadastros/                                                                     ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
   ║    Modelos para cadastro de abastecimentos de veículos (entidade e ViewModel).                    ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 CLASSES DISPONÍVEIS:                                                                           ║
   ║    • AbastecimentoViewModel                                                                       ║
   ║    • Abastecimento                                                                               ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🔗 DEPENDÊNCIAS: FrotiX.Services, FrotiX.Validations, SelectListItem                                ║
   ║ 📅 ATUALIZAÇÃO: 31/01/2026 | 👤 AUTOR: FrotiX Team | 📝 VERSÃO: 2.0                                 ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝ */

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using FrotiX.Services;
using FrotiX.Validations;
using Microsoft.AspNetCore.Http;

namespace FrotiX.Models
    {
    
    // ╭───────────────────────────────────────────────────────────────────────────────────────────────╮
    // │ 🎯 CLASSE: AbastecimentoViewModel                                                            │
    // ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    //
    // 🎯 OBJETIVO:
    // Agregar entidade Abastecimento e listas de seleção para uso em views.
    //
    // 🔗 RASTREABILIDADE:
    // ⬅️ CHAMADO POR : Controllers e Views de cadastro
    // ➡️ CHAMA       : SelectListItem (listas de apoio)
    //
    public class AbastecimentoViewModel
        {
        public Guid AbastecimentoId { get; set; }
        public Guid VeiculoId { get; set; }
        public Guid MotoristaId { get; set; }
        public Guid CombustivelId { get; set; }
        public Abastecimento Abastecimento { get; set; }
        public IEnumerable<SelectListItem> VeiculoList { get; set; }
        public IEnumerable<SelectListItem> MotoristaList { get; set; }
        public IEnumerable<SelectListItem> CombustivelList { get; set; }

        }

    
    // ╭───────────────────────────────────────────────────────────────────────────────────────────────╮
    // │ 🎯 CLASSE: Abastecimento                                                                     │
    // ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    //
    // 🎯 OBJETIVO:
    // Representar o registro de abastecimento de veículo com vínculos a Veiculo, Combustivel e Motorista.
    //
    // 🔗 RASTREABILIDADE:
    // ⬅️ CHAMADO POR : Repositórios, Controllers, serviços de cadastro
    // ➡️ CHAMA       : DataAnnotations, ForeignKey
    //
    public class Abastecimento
        {

        //public double litros;
        //public double valorUnitario;
        //public DateTime data;
        //public DateTime hora;
        //public int kmRodado;
        //public int hodometro;
        //public Guid veiculoId;
        //public Guid combustivelId;
        //public Guid motoristaId;

        //public Abastecimento(double? litros, double? valorUnitario, DateTime? data, DateTime? hora, int? kmRodado, int? hodometro, Guid veiculoId, Guid combustivelId, Guid motoristaId)
        //{
        //    this.litros = (double)litros;
        //    this.valorUnitario = (double)valorUnitario;
        //    this.data = (DateTime)data;
        //    this.hora = (DateTime)hora;
        //    this.kmRodado = (int)kmRodado;
        //    this.hodometro = (int)hodometro;
        //    this.veiculoId = (Guid)veiculoId;
        //    this.combustivelId = (Guid)combustivelId;
        //    this.motoristaId = (Guid)motoristaId;
        //}

        [Key]
        public Guid AbastecimentoId { get; set; }

        [Required(ErrorMessage = "A quantidade de litros é obrigatória")]
        public double? Litros { get; set; }

        [Required(ErrorMessage = "O valor unitário é obrigatório")]
        public double? ValorUnitario { get; set; }

        [Required(ErrorMessage = "A data/hora é obrigatória")]
        public DateTime? DataHora { get; set; }

        public int? KmRodado { get; set; }

        [Required(ErrorMessage = "O hodômetro é obrigatório")]
        public int? Hodometro { get; set; }

        [Required(ErrorMessage = "A autorização QCard é obrigatória")]
        public int? AutorizacaoQCard { get; set; }

        [Required(ErrorMessage = "O veículo é obrigatório")]
        public Guid VeiculoId { get; set; }

        [ForeignKey("VeiculoId")]
        public virtual Veiculo Veiculo { get; set; }

        [Required(ErrorMessage = "O tipo de combustível é obrigatório")]
        public Guid CombustivelId { get; set; }

        [ForeignKey("CombustivelId")]
        public virtual Combustivel Combustivel { get; set; }

        [Required(ErrorMessage = "O motorista é obrigatório")]
        public Guid MotoristaId { get; set; }

        [ForeignKey("MotoristaId")]
        public virtual Motorista Motorista { get; set; }

        }
    }

