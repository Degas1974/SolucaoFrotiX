/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: ViagemCalendarDTO.cs                                                                    ║
   ║ 📂 CAMINHO: /Models/DTO                                                                             ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO: DTO para exibição de viagens em calendário/agenda (FullCalendar JS).                  ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 PROPS: id, title, start, end, backgroundColor, textColor, descricao (formato FullCalendar)       ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🔗 DEPS: System                                                                                     ║
   ║ 📅 Atualizado: 2026 | 👤 FrotiX Team | 📝 Versão: 2.0                                              ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝ */

using System;

namespace FrotiX.Models.DTO
    {
    public class ViagemCalendarDTO
        {
        public Guid id { get; set; }
        public string title { get; set; }
        public DateTime? start { get; set; }        // se trouxe assim
        public DateTime? end { get; set; }          // idem
        public DateTime? dataInicial { get; set; }  // <-- adicione este!
        public DateTime? horaInicio { get; set; }   // <-- adicione este!
        public DateTime? dataFinal { get; set; }    // <-- adicione este!
        public DateTime? horaFim { get; set; }      // <-- adicione este!
        public string backgroundColor { get; set; }
        public string textColor { get; set; }
        public string descricao { get; set; }
        }
    }


