/*
 * ╔══════════════════════════════════════════════════════════════════════════╗
 * ║  📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                   ║
 * ║  Arquivo: Models/RecursoTreeDTO.cs                                       ║
 * ║  Descrição: DTO para representação de recursos em TreeView Syncfusion    ║
 * ║  Data: 29/01/2026 | LOTE: 22                                             ║
 * ╚══════════════════════════════════════════════════════════════════════════╝
 */

using System;
using System.Collections.Generic;

namespace FrotiX.Models
{
    /// <summary>
    /// DTO para representar um recurso na TreeView Syncfusion.
    /// Estrutura compatível com o componente ejs-treeview.
    /// </summary>
    public class RecursoTreeDTO
    {
        /// <summary>ID único do recurso (string para compatibilidade com TreeView)</summary>
        public string? Id { get; set; }

        /// <summary>Texto exibido no menu</summary>
        public string? Text { get; set; }

        /// <summary>Identificador único do recurso (para vinculação com ControleAcesso)</summary>
        public string? NomeMenu { get; set; }

        /// <summary>Classe FontAwesome do ícone (ex: "fa-duotone fa-car")</summary>
        public string? Icon { get; set; }

        /// <summary>CSS do ícone para TreeView Syncfusion</summary>
        public string? IconCss { get; set; }

        /// <summary>URL da página (ex: "veiculo_index.html")</summary>
        public string? Href { get; set; }

        /// <summary>ID do recurso pai (null = raiz)</summary>
        public string? ParentId { get; set; }

        /// <summary>Indica se tem filhos (usado pelo TreeView)</summary>
        public bool HasChild { get; set; }

        /// <summary>Se o nó está expandido</summary>
        public bool Expanded { get; set; } = true;

        /// <summary>Ordem de exibição</summary>
        public double Ordem { get; set; }

        /// <summary>Nível na hierarquia (0=raiz, 1=filho, 2=neto)</summary>
        public int Nivel { get; set; }

        /// <summary>Descrição do recurso</summary>
        public string? Descricao { get; set; }

        /// <summary>Se o recurso está ativo no menu</summary>
        public bool Ativo { get; set; } = true;

        /// <summary>Lista de filhos (subitens do menu)</summary>
        public List<RecursoTreeDTO>? Items { get; set; } = new List<RecursoTreeDTO>();

        /// <summary>
        /// Converte um Recurso do banco para DTO
        /// </summary>
        public static RecursoTreeDTO FromRecurso(Recurso recurso)
        {
            return new RecursoTreeDTO
            {
                Id = recurso.RecursoId.ToString(),
                Text = recurso.Nome,
                NomeMenu = recurso.NomeMenu,
                Icon = recurso.Icon,
                IconCss = recurso.Icon,
                Href = recurso.Href,
                ParentId = recurso.ParentId?.ToString(),
                Ordem = recurso.Ordem,
                Nivel = recurso.Nivel,
                Descricao = recurso.Descricao,
                Ativo = recurso.Ativo,
                HasChild = recurso.HasChild,
                Expanded = true
            };
        }

        /// <summary>
        /// Converte DTO para Recurso do banco
        /// </summary>
        public Recurso ToRecurso()
        {
            return new Recurso
            {
                RecursoId = Guid.TryParse(Id, out var id) ? id : Guid.NewGuid(),
                Nome = Text,
                NomeMenu = NomeMenu,
                Icon = Icon,
                Href = Href,
                ParentId = Guid.TryParse(ParentId, out var parentId) ? parentId : null,
                Ordem = Ordem,
                Nivel = Nivel,
                Descricao = Descricao,
                Ativo = Ativo,
                HasChild = HasChild
            };
        }
    }
}
