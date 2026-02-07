/* ****************************************************************************************
 * ⚡ ARQUIVO: Agenda.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Modelar agendamentos de viagens e ViewModel de filtros para agenda.
 *
 * 📥 ENTRADAS     : Dados de viagem, horários, status e filtros de tela.
 *
 * 📤 SAÍDAS       : Objetos utilizados na renderização do calendário.
 *
 * 🔗 CHAMADA POR  : Controllers e Views de agenda/viagens.
 *
 * 🔄 CHAMA        : DataAnnotations e utilitários de UI.
 *
 * 📦 DEPENDÊNCIAS : System.ComponentModel.DataAnnotations, Microsoft.AspNetCore.Mvc.Rendering.
 **************************************************************************************** */

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

    /****************************************************************************************
     * ⚡ VIEWMODEL: AgendaViewModel
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Representar filtros/atributos auxiliares para views de agenda.
     *
     * 📥 ENTRADAS     : Status e parâmetros usados na filtragem.
     *
     * 📤 SAÍDAS       : ViewModel para a interface de agenda.
     *
     * 🔗 CHAMADA POR  : Views e controllers de agenda.
     ****************************************************************************************/
    public class AgendaViewModel
        {
        public string Status { get; set; }
        }


    /****************************************************************************************
     * ⚡ MODEL: Agenda
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Representar um agendamento de viagem com horários e metadados.
     *
     * 📥 ENTRADAS     : Datas/horas, título, cores e status.
     *
     * 📤 SAÍDAS       : Evento de calendário exibido na UI.
     *
     * 🔗 CHAMADA POR  : Calendário/agenda de viagens.
     ****************************************************************************************/
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


