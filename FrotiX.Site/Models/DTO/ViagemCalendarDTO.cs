/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 📌 ARQUIVO: ViagemCalendarDTO.cs                                                                    ║
   ║ 📂 CAMINHO: /Models/DTO                                                                             ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🧭 OBJETIVO: DTO para exibição de viagens em calendário/agenda (FullCalendar).                     ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🗂️  CONTÉM: ViagemCalendarDTO                                                                       ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🔗 DEPENDÊNCIAS: System                                                                             ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝ */

using System;

namespace FrotiX.Models.DTO
    {
    // DTO para FullCalendar.
    public class ViagemCalendarDTO
        {
        // Identificador do evento.
        public Guid id { get; set; }
        // Título exibido.
        public string title { get; set; }
        // Início no calendário.
        public DateTime? start { get; set; }        // se trouxe assim
        // Fim no calendário.
        public DateTime? end { get; set; }          // idem
        // Data inicial da viagem.
        public DateTime? dataInicial { get; set; }  // <-- adicione este!
        // Hora de início.
        public DateTime? horaInicio { get; set; }   // <-- adicione este!
        // Data final da viagem.
        public DateTime? dataFinal { get; set; }    // <-- adicione este!
        // Hora de fim.
        public DateTime? horaFim { get; set; }      // <-- adicione este!
        // Cor de fundo.
        public string backgroundColor { get; set; }
        // Cor do texto.
        public string textColor { get; set; }
        // Descrição.
        public string descricao { get; set; }
        }
    }

