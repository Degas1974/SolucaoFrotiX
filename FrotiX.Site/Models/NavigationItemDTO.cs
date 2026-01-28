// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ ARQUIVO: NavigationItemDTO.cs                                               ║
// ║ PROJETO: FrotiX - Sistema de Gestão de Frotas                               ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO:                                                                   ║
// ║ DTOs para transferência de dados de navegação entre API e frontend.         ║
// ║ Usado pelo NavigationController para gerenciar menu dinâmico.               ║
// ║                                                                              ║
// ║ CLASSES:                                                                     ║
// ║ - NavigationItemDTO: Item de menu básico                                    ║
// ║ - NavigationTreeItem: Estrutura para TreeView Syncfusion EJ2                ║
// ║ - SaveNavigationRequest: Request para salvar árvore completa                ║
// ║ - DeleteNavigationItemRequest: Request para deletar item por NomeMenu       ║
// ║ - DeleteRecursoRequest: Request para deletar recurso por ID                 ║
// ║ - UpdateAcessoRequest: Request para atualizar acesso usuário/recurso        ║
// ║                                                                              ║
// ║ USO: Integração com Syncfusion TreeView para gestão de menus                ║
// ║                                                                              ║
// ║ DOCUMENTADO EM: 2026-01-28 | LOTE: 16                                       ║
// ╚══════════════════════════════════════════════════════════════════════════════╝

using System.Collections.Generic;

namespace FrotiX.Models
{
    /// <summary>
    /// DTO para transferência de dados de item de navegação entre API e frontend
    /// </summary>
    public class NavigationItemDTO
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string NomeMenu { get; set; }
        public string OldNomeMenu { get; set; }
        public string Href { get; set; }
        public string Icon { get; set; }
        public string ParentId { get; set; }
    }

    /// <summary>
    /// Estrutura para TreeView Syncfusion EJ2
    /// </summary>
    public class NavigationTreeItem
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public string Title { get; set; }
        public string NomeMenu { get; set; }
        public string Href { get; set; }
        public string Icon { get; set; }
        public string IconCss { get; set; }
        public string ParentId { get; set; }
        public bool HasChild { get; set; }
        public bool Expanded { get; set; }
        public List<NavigationTreeItem> Items { get; set; } = new List<NavigationTreeItem>();
    }

    /// <summary>
    /// Request para salvar a árvore de navegação completa
    /// </summary>
    public class SaveNavigationRequest
    {
        public List<NavigationTreeItem> Items { get; set; }
    }

    /// <summary>
    /// Request para deletar um item
    /// </summary>
    public class DeleteNavigationItemRequest
    {
        public string NomeMenu { get; set; }
    }

    /// <summary>
    /// Request para deletar um recurso por ID
    /// </summary>
    public class DeleteRecursoRequest
    {
        public string RecursoId { get; set; }
    }

    /// <summary>
    /// Request para atualizar acesso de usuário a um recurso
    /// </summary>
    public class UpdateAcessoRequest
    {
        public string UsuarioId { get; set; }
        public string RecursoId { get; set; }
        public bool Acesso { get; set; }
    }
}
