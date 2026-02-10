/* ****************************************************************************************
 * ⚡ ARQUIVO: NavigationItemDTO.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Definir DTOs de navegação para comunicação entre API e frontend.
 *
 * 📥 ENTRADAS     : Itens de menu, árvore e requests de manutenção.
 *
 * 📤 SAÍDAS       : Estruturas para sincronização de navegação.
 *
 * 🔗 CHAMADA POR  : APIs de navegação e administração.
 *
 * 🔄 CHAMA        : System.Collections.Generic.
 *
 * 📦 DEPENDÊNCIAS : System.Collections.Generic.
 **************************************************************************************** */

using System.Collections.Generic;

namespace FrotiX.Models
{
    /****************************************************************************************
     * ⚡ DTO: NavigationItemDTO
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Representar item de navegação para comunicação API/frontend.
     *
     * 📥 ENTRADAS     : Identificadores, texto, URL e ícone.
     *
     * 📤 SAÍDAS       : Item serializável para UI.
     *
     * 🔗 CHAMADA POR  : APIs de navegação.
     *
     * 🔄 CHAMA        : Não se aplica.
     ****************************************************************************************/
    public class NavigationItemDTO
    {
        // Identificador do item.
        public string Id { get; set; }
        // Título exibido.
        public string Title { get; set; }
        // Nome do menu.
        public string NomeMenu { get; set; }
        // Nome anterior do menu.
        public string OldNomeMenu { get; set; }
        // URL do item.
        public string Href { get; set; }
        // Ícone do item.
        public string Icon { get; set; }
        // Identificador do item pai.
        public string ParentId { get; set; }
    }

    /****************************************************************************************
     * ⚡ DTO: NavigationTreeItem
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Estrutura de TreeView Syncfusion EJ2.
     *
     * 📥 ENTRADAS     : Identificadores, texto, URL e filhos.
     *
     * 📤 SAÍDAS       : Árvore de navegação para UI.
     *
     * 🔗 CHAMADA POR  : Interfaces de administração do menu.
     *
     * 🔄 CHAMA        : List<NavigationTreeItem>.
     ****************************************************************************************/
    public class NavigationTreeItem
    {
        // Identificador do item.
        public string Id { get; set; }
        // Texto principal.
        public string Text { get; set; }
        // Título exibido.
        public string Title { get; set; }
        // Nome do menu.
        public string NomeMenu { get; set; }
        // URL do item.
        public string Href { get; set; }
        // Ícone principal.
        public string Icon { get; set; }
        // Classe CSS do ícone.
        public string IconCss { get; set; }
        // Identificador do item pai.
        public string ParentId { get; set; }
        // Indica se possui filhos.
        public bool HasChild { get; set; }
        // Indica se está expandido.
        public bool Expanded { get; set; }
        // Lista de filhos.
        public List<NavigationTreeItem> Items { get; set; } = new List<NavigationTreeItem>();
    }

    /****************************************************************************************
     * ⚡ REQUEST: SaveNavigationRequest
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Representar requisição de salvamento da árvore completa.
     *
     * 📥 ENTRADAS     : Lista de itens da árvore.
     *
     * 📤 SAÍDAS       : Payload para API de navegação.
     *
     * 🔗 CHAMADA POR  : Endpoints de navegação.
     *
     * 🔄 CHAMA        : NavigationTreeItem.
     ****************************************************************************************/
    public class SaveNavigationRequest
    {
        // Itens da árvore.
        public List<NavigationTreeItem> Items { get; set; }
    }

    /****************************************************************************************
     * ⚡ REQUEST: DeleteNavigationItemRequest
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Representar requisição de remoção de item de menu.
     *
     * 📥 ENTRADAS     : NomeMenu.
     *
     * 📤 SAÍDAS       : Payload para API de navegação.
     *
     * 🔗 CHAMADA POR  : Endpoints de navegação.
     *
     * 🔄 CHAMA        : Não se aplica.
     ****************************************************************************************/
    public class DeleteNavigationItemRequest
    {
        // Nome do menu a remover.
        public string NomeMenu { get; set; }
    }

    /****************************************************************************************
     * ⚡ REQUEST: DeleteRecursoRequest
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Representar requisição de remoção de recurso por ID.
     *
     * 📥 ENTRADAS     : RecursoId.
     *
     * 📤 SAÍDAS       : Payload para API de recursos.
     *
     * 🔗 CHAMADA POR  : Endpoints de recursos.
     *
     * 🔄 CHAMA        : Não se aplica.
     ****************************************************************************************/
    public class DeleteRecursoRequest
    {
        // Identificador do recurso.
        public string RecursoId { get; set; }
    }

    /****************************************************************************************
     * ⚡ REQUEST: UpdateAcessoRequest
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Representar atualização de acesso de usuário a um recurso.
     *
     * 📥 ENTRADAS     : UsuarioId, RecursoId e Acesso.
     *
     * 📤 SAÍDAS       : Payload para API de controle de acesso.
     *
     * 🔗 CHAMADA POR  : Endpoints de controle de acesso.
     *
     * 🔄 CHAMA        : Não se aplica.
     ****************************************************************************************/
    public class UpdateAcessoRequest
    {
        // Identificador do usuário.
        public string UsuarioId { get; set; }
        // Identificador do recurso.
        public string RecursoId { get; set; }
        // Flag de acesso.
        public bool Acesso { get; set; }
    }
}
