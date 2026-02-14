/* ****************************************************************************************
 * ⚡ ARQUIVO: CorridasTaxiLeg.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Modelar corridas TaxiLeg e ViewModel de apoio.
 *
 * 📥 ENTRADAS     : Dados de corrida, horários, origem/destino e avaliações.
 *
 * 📤 SAÍDAS       : Entidade persistida e ViewModel para UI.
 *
 * 🔗 CHAMADA POR  : Integração TaxiLeg, relatórios e telas de consulta.
 *
 * 🔄 CHAMA        : DataAnnotations e utilitários de UI.
 *
 * 📦 DEPENDÊNCIAS : FrotiX.Services, FrotiX.Validations, Microsoft.AspNetCore.Mvc.Rendering.
 **************************************************************************************** */

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using FrotiX.Services;
using FrotiX.Validations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FrotiX.Models
    {
    /****************************************************************************************
     * ⚡ VIEWMODEL: CorridasTaxiLegViewModel
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Agrupar filtros/dados auxiliares para consultas de corridas TaxiLeg.
     *
     * 📥 ENTRADAS     : Identificadores de filtros.
     *
     * 📤 SAÍDAS       : ViewModel para telas e consultas.
     *
     * 🔗 CHAMADA POR  : Controllers/Views de corridas.
     ****************************************************************************************/
    public class CorridasTaxiLegViewModel
        {
        public Guid AbastecimentoId { get; set; }
        public Guid VeiculoId { get; set; }
        public Guid MotoristaId { get; set; }
        public Guid CombustivelId { get; set; }
        }

    /****************************************************************************************
     * ⚡ MODEL: CorridasTaxiLeg
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Representar uma corrida de táxi legítima integrada ao sistema.
     *
     * 📥 ENTRADAS     : Origem/destino, horários, avaliação e custos.
     *
     * 📤 SAÍDAS       : Registro persistido da corrida.
     *
     * 🔗 CHAMADA POR  : Integração TaxiLeg e relatórios.
     *
     * 🔄 CHAMA        : DataAnnotations.
     ****************************************************************************************/
    public class CorridasTaxiLeg
        {
        [Key]
        public Guid CorridaId { get; set; }

        public int? QRU { get; set; }

        public string? Origem { get; set; }

        public string? Setor { get; set; }

        public string? DescSetor { get; set; }

        public string? Unidade { get; set; }

        public string? DescUnidade { get; set; }

        public int? QtdPassageiros { get; set; }

        public string? MotivoUso { get; set; }

        public DateTime? DataAgenda { get; set; }

        public DateTime? DataFinal { get; set; }

        public string? HoraAgenda { get; set; }

        public string? HoraAceite { get; set; }

        public string? HoraLocal { get; set; }

        public string? HoraInicio { get; set; }

        public string? HoraFinal { get; set; }

        public double? KmReal { get; set; }

        public int? QtdEstrelas { get; set; }

        public string? Avaliacao { get; set; }

        public int? Duracao { get; set; }

        public int? Espera { get; set; }

        public string? OrigemCorrida { get; set; }

        public string? DestinoCorrida { get; set; }

        public bool Glosa { get; set; }

        public double? ValorGlosa { get; set; }

        public double? Valor { get; set; }
        }
    }
