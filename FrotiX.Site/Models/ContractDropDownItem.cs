// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ ARQUIVO: ContractDropDownItem.cs                                            ║
// ║ PROJETO: FrotiX - Sistema de Gestão de Frotas                               ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO:                                                                   ║
// ║ Modelo simples para dropdown de seleção de contratos.                       ║
// ║ Usado em componentes Syncfusion DropDownList e ComboBox.                    ║
// ║                                                                              ║
// ║ PROPRIEDADES:                                                                ║
// ║ - Value: Valor do item (geralmente Guid como string)                        ║
// ║ - Text: Texto exibido no dropdown                                           ║
// ║                                                                              ║
// ║ DOCUMENTADO EM: 2026-01-28 | LOTE: 16                                       ║
// ╚══════════════════════════════════════════════════════════════════════════════╝

namespace FrotiX.Models
    {
    public sealed class ContractDropDownItem
        {
        public string Value { get; set; }
        public string Text { get; set; }
        }
    }


