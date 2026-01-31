/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 📌 ARQUIVO: NavigationItemDTO.cs                                                                    ║
   ║ 📂 CAMINHO: /Models                                                                                 ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🧭 OBJETIVO: DTOs para transferência de itens de navegação entre API e frontend.                   ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🗂️  CONTÉM: NavigationItemDTO, NavigationTreeItem, Requests                                         ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🔗 DEPENDÊNCIAS: System.Collections.Generic                                                         ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝ */

using System.Collections.Generic;

namespace FrotiX.Models
{
    // ==================================================================================================
    // DTO
    // ==================================================================================================
    // Item de navegação para comunicação entre API e frontend.
    // ==================================================================================================
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

    // ==================================================================================================
    // TREEVIEW
    // ==================================================================================================
    // Estrutura para TreeView Syncfusion EJ2.
    // ==================================================================================================
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

    // Request para salvar a árvore de navegação completa.
    public class SaveNavigationRequest
    {
        // Itens da árvore.
        public List<NavigationTreeItem> Items { get; set; }
    }

    // Request para deletar um item.
    public class DeleteNavigationItemRequest
    {
        // Nome do menu a remover.
        public string NomeMenu { get; set; }
    }

    // Request para deletar um recurso por ID.
    public class DeleteRecursoRequest
    {
        // Identificador do recurso.
        public string RecursoId { get; set; }
    }

    // Request para atualizar acesso de usuário a um recurso.
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
