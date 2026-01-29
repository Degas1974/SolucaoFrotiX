/*
 * ╔══════════════════════════════════════════════════════════════════════════╗
 * ║  📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                   ║
 * ║  Arquivo: Settings/RecorrenciaToggleSettings.cs                          ║
 * ║  Descrição: Feature toggles para funcionalidades de recorrência.         ║
 * ║             ForcarTextoRecorrencia, ForcarDatePickerRecorrencia,         ║
 * ║             MostrarToggleDev (controles de dev). Vinculado a appsettings.║
 * ║  Data: 29/01/2026 | LOTE: 22                                             ║
 * ╚══════════════════════════════════════════════════════════════════════════╝
 */

namespace FrotiX.Settings
{
    public class RecorrenciaToggleSettings
    {
        public bool ForcarTextoRecorrencia { get; set; }
        public bool ForcarDatePickerRecorrencia { get; set; }
        public bool MostrarToggleDev { get; set; }
    }
}
