// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ ARQUIVO: ViagemCalendarDTO.cs                                               ║
// ║ PROJETO: FrotiX - Sistema de Gestão de Frotas                               ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO:                                                                   ║
// ║ DTO para exibição de viagens em calendário (FullCalendar JS).               ║
// ║ Propriedades em lowercase para compatibilidade com biblioteca JS.           ║
// ║                                                                              ║
// ║ PROPRIEDADES FULLCALENDAR:                                                   ║
// ║ - id: Identificador único (Guid)                                            ║
// ║ - title: Título do evento                                                   ║
// ║ - start, end: Datas de início/fim (formato ISO)                             ║
// ║ - backgroundColor, textColor: Cores do evento                               ║
// ║                                                                              ║
// ║ PROPRIEDADES ADICIONAIS:                                                     ║
// ║ - dataInicial, horaInicio, dataFinal, horaFim: Dados originais              ║
// ║ - descricao: Descrição detalhada                                            ║
// ║                                                                              ║
// ║ DOCUMENTADO EM: 2026-01-28 | LOTE: 16                                       ║
// ╚══════════════════════════════════════════════════════════════════════════════╝

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


