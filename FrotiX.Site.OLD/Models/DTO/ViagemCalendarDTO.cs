/* ****************************************************************************************
 * ⚡ ARQUIVO: ViagemCalendarDTO.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Expor dados de viagem para exibição em calendário/agenda (FullCalendar).
 *
 * 📥 ENTRADAS     : Datas, horários, título e cores do evento.
 *
 * 📤 SAÍDAS       : DTO serializado para consumo no calendário.
 *
 * 🔗 CHAMADA POR  : Endpoints e páginas de agenda/planejamento.
 *
 * 🔄 CHAMA        : Não se aplica.
 *
 * 📦 DEPENDÊNCIAS : System.
 **************************************************************************************** */

using System;

namespace FrotiX.Models.DTO
    {
    /****************************************************************************************
     * ⚡ DTO: ViagemCalendarDTO
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Representar evento de viagem no calendário.
     *
     * 📥 ENTRADAS     : Identificador, título, datas e cores de exibição.
     *
     * 📤 SAÍDAS       : Evento compatível com FullCalendar.
     *
     * 🔗 CHAMADA POR  : Serviços e páginas de agenda.
     *
     * 🔄 CHAMA        : Não se aplica.
     *
     * ⚠️ ATENÇÃO      : Propriedades usam camelCase para compatibilidade com JSON/FullCalendar.
     ****************************************************************************************/
    public class ViagemCalendarDTO
        {
        // Identificador do evento.
        public Guid id { get; set; }
        // Título exibido.
        public string title { get; set; }
        // Início no calendário.
        public DateTime? start { get; set; }
        // Fim no calendário.
        public DateTime? end { get; set; }
        // Data inicial da viagem.
        public DateTime? dataInicial { get; set; }
        // Hora de início.
        public DateTime? horaInicio { get; set; }
        // Data final da viagem.
        public DateTime? dataFinal { get; set; }
        // Hora de fim.
        public DateTime? horaFim { get; set; }
        // Cor de fundo.
        public string backgroundColor { get; set; }
        // Cor do texto.
        public string textColor { get; set; }
        // Descrição.
        public string descricao { get; set; }
        }
    }
