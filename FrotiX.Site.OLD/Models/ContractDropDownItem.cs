/* ****************************************************************************************
 * ⚡ ARQUIVO: ContractDropDownItem.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Representar item simples de dropdown para contratos (Value/Text).
 *
 * 📥 ENTRADAS     : Value (identificador/valor) e Text (descrição exibida).
 *
 * 📤 SAÍDAS       : Par Value/Text usado na UI para seleção de contratos.
 *
 * 🔗 CHAMADA POR  : Controllers/Views que montam combos de contratos.
 *
 * 🔄 CHAMA        : Não se aplica.
 *
 * 📦 DEPENDÊNCIAS : Nenhuma.
 **************************************************************************************** */

namespace FrotiX.Models
    {
    /****************************************************************************************
     * ⚡ DTO: ContractDropDownItem
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Transportar valor e texto de contratos para listas de seleção.
     *
     * 📥 ENTRADAS     : Value (id/valor) e Text (descrição).
     *
     * 📤 SAÍDAS       : Item pronto para dropdowns.
     *
     * 🔗 CHAMADA POR  : Serviços, controllers e views de contratos.
     *
     * 🔄 CHAMA        : Não se aplica.
     ****************************************************************************************/
    public sealed class ContractDropDownItem
        {
        // Valor associado ao item.
        public string Value { get; set; }

        // Texto exibido no dropdown.
        public string Text { get; set; }
        }
    }
