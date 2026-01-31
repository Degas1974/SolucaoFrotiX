/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: Agenda.cs                                                                             ║
   ║ 📂 CAMINHO: Models/Cadastros/                                                                     ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
   ║    Modelos para gerenciamento de agendamentos de viagens (agenda).                                ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 CLASSES DISPONÍVEIS:                                                                           ║
   ║    • AgendaViewModel                                                                              ║
   ║    • Agenda                                                                                       ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🔗 DEPENDÊNCIAS: System.ComponentModel.DataAnnotations, SelectListItem                              ║
   ║ 📅 ATUALIZAÇÃO: 31/01/2026 | 👤 AUTOR: FrotiX Team | 📝 VERSÃO: 2.0                                 ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝ */

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FrotiX.Models
    {

    // ╭───────────────────────────────────────────────────────────────────────────────────────────────╮
    // │ 🎯 CLASSE: AgendaViewModel                                                                   │
    // ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    //
    // 🎯 OBJETIVO:
    // Representar filtros/atributos auxiliares para views de agenda.
    //
    // 🔗 RASTREABILIDADE:
    // ⬅️ CHAMADO POR : Views e Controllers de agenda
    // ➡️ CHAMA       : (sem chamadas internas)
    //
    public class AgendaViewModel
        {
        public string Status { get; set; }
        }


    // ╭───────────────────────────────────────────────────────────────────────────────────────────────╮
    // │ 🎯 CLASSE: Agenda                                                                            │
    // ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    //
    // 🎯 OBJETIVO:
    // Representar um agendamento de viagem com horários, status e metadados.
    //
    // 🔗 RASTREABILIDADE:
    // ⬅️ CHAMADO POR : Calendário/Agenda de viagens
    // ➡️ CHAMA       : (sem chamadas internas)
    //
    public class Agenda
        {

        public Guid ViagemId { get; set; }

        public DateTime HoraInicial { get; set; }

        public DateTime HoraFinal { get; set; }

        public DateTime DataInicial { get; set; }

        public string Descricao { get; set; }

        public string Titulo { get; set; }

        public string Status { get; set; }

        public bool DiaTodo { get; set; }

        public string CorEvento { get; set; }

        public string CorTexto { get; set; }

        public string Finalidade { get; set; }
        }
    }

