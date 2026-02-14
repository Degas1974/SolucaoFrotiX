/* ****************************************************************************************
 * ⚡ ARQUIVO: DateItem.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Representar item simples de data em dropdowns/seletores.
 *
 * 📥 ENTRADAS     : Text (exibição) e Value (valor associado).
 *
 * 📤 SAÍDAS       : Item usado em listas de seleção de data.
 *
 * 🔗 CHAMADA POR  : Views e componentes de filtro.
 *
 * 🔄 CHAMA        : Não se aplica.
 *
 * 📦 DEPENDÊNCIAS : Nenhuma.
 **************************************************************************************** */

namespace FrotiX.Models
    {
    /****************************************************************************************
     * ⚡ DTO: DateItem
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Transportar texto e valor de uma opção de data.
     *
     * 📥 ENTRADAS     : Text e Value.
     *
     * 📤 SAÍDAS       : Item para dropdowns.
     *
     * 🔗 CHAMADA POR  : Interfaces de filtro e seleção.
     *
     * 🔄 CHAMA        : Não se aplica.
     ****************************************************************************************/
    public class DateItem
        {
        // Texto exibido.
        public string Text { get; set; }

        // Valor associado.
        public string Value { get; set; }
        }
    }
