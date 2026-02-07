/* ****************************************************************************************
 * ⚡ ARQUIVO: CorridasTaxiLegCanceladas.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Registrar corridas TaxiLeg canceladas (histórico de cancelamentos).
 *
 * 📥 ENTRADAS     : Dados de origem/destino, horários e motivo do cancelamento.
 *
 * 📤 SAÍDAS       : Entidade persistida para consultas e relatórios.
 *
 * 🔗 CHAMADA POR  : Integração TaxiLeg e auditoria de cancelamentos.
 *
 * 🔄 CHAMA        : DataAnnotations.
 *
 * 📦 DEPENDÊNCIAS : System.ComponentModel.DataAnnotations.
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
     * ⚡ MODEL: CorridasCanceladasTaxiLeg
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Representar corrida cancelada e seus dados de contexto.
     *
     * 📥 ENTRADAS     : Motivo, horários e metadados do cancelamento.
     *
     * 📤 SAÍDAS       : Registro persistido da corrida cancelada.
     *
     * 🔗 CHAMADA POR  : Integração TaxiLeg e relatórios.
     *
     * 🔄 CHAMA        : DataAnnotations.
     ****************************************************************************************/
    public class CorridasCanceladasTaxiLeg
        {

        [Key]
        public Guid CorridaCanceladaId { get; set; }

        public string? Origem { get; set; }

        public string? Setor { get; set; }

        public string? SetorExtra { get; set; }

        public string? Unidade { get; set; }

        public string? UnidadeExtra { get; set; }

        public int? QtdPassageiros { get; set; }

        public string? MotivoUso { get; set; }

        public DateTime? DataAgenda { get; set; }

        public string? HoraAgenda { get; set; }

        public DateTime? DataHoraCancelamento { get; set; }

        public string? HoraCancelamento { get; set; }

        public string? TipoCancelamento { get; set; }

        public string? MotivoCancelamento { get; set; }

        public int? TempoEspera { get; set; }

        }
    }
