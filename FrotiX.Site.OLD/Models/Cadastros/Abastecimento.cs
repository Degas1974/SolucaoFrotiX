/* ****************************************************************************************
 * ⚡ ARQUIVO: Abastecimento.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Modelar abastecimentos de veículos e o ViewModel de apoio às telas.
 *
 * 📥 ENTRADAS     : Dados de abastecimento, listas de seleção e vínculos a entidades.
 *
 * 📤 SAÍDAS       : Entidade persistida e ViewModel para UI.
 *
 * 🔗 CHAMADA POR  : Controllers/Views de abastecimento, repositórios e serviços.
 *
 * 🔄 CHAMA        : DataAnnotations, ForeignKey, SelectListItem.
 *
 * 📦 DEPENDÊNCIAS : FrotiX.Services, FrotiX.Validations, Microsoft.AspNetCore.Mvc.Rendering.
 *
 * 📝 OBSERVAÇÕES  : A entidade referencia Veiculo, Combustivel e Motorista.
 **************************************************************************************** */

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
    
    /****************************************************************************************
     * ⚡ VIEWMODEL: AbastecimentoViewModel
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Agregar entidade Abastecimento e listas de seleção para uso em views.
     *
     * 📥 ENTRADAS     : Entidade Abastecimento e listas de veículos/motoristas/combustíveis.
     *
     * 📤 SAÍDAS       : Payload completo para telas de cadastro/edição.
     *
     * 🔗 CHAMADA POR  : Controllers e Views de abastecimento.
     *
     * 🔄 CHAMA        : SelectListItem.
     ****************************************************************************************/
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

    
    /****************************************************************************************
     * ⚡ MODEL: Abastecimento
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Representar o registro de abastecimento de veículo.
     *
     * 📥 ENTRADAS     : Litros, valor unitário, data/hora, hodômetro e vínculos.
     *
     * 📤 SAÍDAS       : Registro persistido e navegável via EF Core.
     *
     * 🔗 CHAMADA POR  : Repositórios, controllers e serviços de cadastro.
     *
     * 🔄 CHAMA        : DataAnnotations, ForeignKey.
     ****************************************************************************************/
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

        // =====================================================================
        // CAMPOS DE NORMALIZAÇÃO - Sistema de detecção de outliers
        // =====================================================================

        // Quilometragem normalizada (após remoção de outliers).
        public int? KmRodadoNormalizado { get; set; }

        // Litros normalizado (após remoção de outliers).
        public double? LitrosNormalizado { get; set; }

        // Consumo calculado (Km/Litros).
        public decimal? ConsumoCalculado { get; set; }

        // Consumo normalizado (após remoção de outliers).
        public decimal? ConsumoNormalizado { get; set; }

        // Indica se o abastecimento foi identificado como outlier.
        public bool? EhOutlier { get; set; }

        }
    }
