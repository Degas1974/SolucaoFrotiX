// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ ARQUIVO: RecorrenciaToggleSettings.cs                                        ║
// ║ PROJETO: FrotiX - Sistema de Gestão de Frotas                                ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO:                                                                   ║
// ║ Configurações de feature toggles para funcionalidade de recorrência.         ║
// ║ Permite habilitar/desabilitar modos de entrada de recorrência em dev/prod.   ║
// ║                                                                              ║
// ║ PROPRIEDADES:                                                                ║
// ║ - ForcarTextoRecorrencia: Força entrada de recorrência como texto livre      ║
// ║ - ForcarDatePickerRecorrencia: Força uso de DatePicker para recorrência      ║
// ║ - MostrarToggleDev: Exibe toggle de desenvolvimento na interface             ║
// ║                                                                              ║
// ║ USO:                                                                         ║
// ║ - Testes A/B de interface                                                    ║
// ║ - Desenvolvimento incremental de features                                    ║
// ║ - Configuração diferenciada por ambiente                                     ║
// ║                                                                              ║
// ║ DOCUMENTADO EM: 2026-01-28 | LOTE: 13                                        ║
// ╚══════════════════════════════════════════════════════════════════════════════╝

namespace FrotiX.Settings
{
    /// <summary>
    /// Feature toggles para funcionalidade de recorrência.
    /// </summary>
    public class RecorrenciaToggleSettings
    {
        public bool ForcarTextoRecorrencia { get; set; }
        public bool ForcarDatePickerRecorrencia { get; set; }
        public bool MostrarToggleDev { get; set; }
    }
}
