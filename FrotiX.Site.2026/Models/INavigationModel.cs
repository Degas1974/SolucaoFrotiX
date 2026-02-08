/* ****************************************************************************************
 * ⚡ ARQUIVO: INavigationModel.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Definir contrato para navegação do sistema (menu lateral).
 *
 * 📥 ENTRADAS     : Implementações que montam a navegação.
 *
 * 📤 SAÍDAS       : Propriedades de navegação Seed e Full.
 *
 * 🔗 CHAMADA POR  : Layouts e componentes de menu.
 *
 * 🔄 CHAMA        : SmartNavigation.
 *
 * 📦 DEPENDÊNCIAS : SmartNavigation.
 **************************************************************************************** */

namespace FrotiX.Models
    {
    /****************************************************************************************
     * ⚡ INTERFACE: INavigationModel
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Padronizar acesso às navegações Seed e Full.
     *
     * 📥 ENTRADAS     : Implementações de montagem de menu.
     *
     * 📤 SAÍDAS       : SmartNavigation para UI.
     *
     * 🔗 CHAMADA POR  : Layouts/Views.
     *
     * 🔄 CHAMA        : SmartNavigation.
     ****************************************************************************************/
    public interface INavigationModel
        {
        // Navegação inicial (mínima).
        SmartNavigation Seed { get; }

        // Navegação completa.
        SmartNavigation Full { get; }
        }
    }
