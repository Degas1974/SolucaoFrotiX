/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: CorridasTaxiLegCanceladas.cs                                                          ║
   ║ 📂 CAMINHO: Models/Cadastros/                                                                     ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
   ║    Registro de corridas de táxi canceladas (histórico de cancelamentos).                          ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 CLASSES DISPONÍVEIS:                                                                           ║
   ║    • CorridasCanceladasTaxiLeg                                                                    ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🔗 DEPENDÊNCIAS: FrotiX.Services, FrotiX.Validations                                                ║
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
    // │ 🎯 CLASSE: CorridasCanceladasTaxiLeg                                                         │
    // ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    //
    // 🎯 OBJETIVO:
    // Representar corrida cancelada e seus dados de contexto.
    //
    // 🔗 RASTREABILIDADE:
    // ⬅️ CHAMADO POR : Integração TaxiLeg / Relatórios
    // ➡️ CHAMA       : DataAnnotations
    //
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

