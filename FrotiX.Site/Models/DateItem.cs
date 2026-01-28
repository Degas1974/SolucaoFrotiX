// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ ARQUIVO: DateItem.cs                                                        ║
// ║ PROJETO: FrotiX - Sistema de Gestão de Frotas                               ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO:                                                                   ║
// ║ Modelo simples para dropdown de seleção de datas.                           ║
// ║ Usado em filtros de mês/ano nos dashboards e relatórios.                    ║
// ║                                                                              ║
// ║ PROPRIEDADES:                                                                ║
// ║ - Text: Texto exibido (ex: "Janeiro/2026")                                  ║
// ║ - Value: Valor do item (ex: "2026-01")                                      ║
// ║                                                                              ║
// ║ DOCUMENTADO EM: 2026-01-28 | LOTE: 16                                       ║
// ╚══════════════════════════════════════════════════════════════════════════════╝

namespace FrotiX.Models
    {
    public class DateItem
        {
        public string Text { get; set; }
        public string Value { get; set; }
        }
    }


