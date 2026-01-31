/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 📌 ARQUIVO: RecursoTreeDTO.cs                                                                       ║
   ║ 📂 CAMINHO: /Models                                                                                 ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🧭 OBJETIVO: DTO para representação de recursos em TreeView Syncfusion (ejs-treeview).             ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🗂️  CONTÉM: RecursoTreeDTO                                                                          ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🔗 DEPENDÊNCIAS: System.Collections.Generic                                                         ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝ */

using System;
using System.Collections.Generic;

namespace FrotiX.Models
{
    // ==================================================================================================
    // DTO
    // ==================================================================================================
    // Representa um recurso no TreeView Syncfusion.
    // ==================================================================================================
    public class RecursoTreeDTO
    {
        // ID único do recurso (string para compatibilidade com TreeView).
        public string? Id { get; set; }

        // Texto exibido no menu.
        public string? Text { get; set; }

        // Identificador único do recurso (para vínculo com ControleAcesso).
        public string? NomeMenu { get; set; }

        // Classe FontAwesome do ícone (ex: "fa-duotone fa-car").
        public string? Icon { get; set; }

        // CSS do ícone para TreeView Syncfusion.
        public string? IconCss { get; set; }

        // URL da página (ex: "veiculo_index.html").
        public string? Href { get; set; }

        // ID do recurso pai (null = raiz).
        public string? ParentId { get; set; }

        // Indica se tem filhos (usado pelo TreeView).
        public bool HasChild { get; set; }

        // Indica se o nó está expandido.
        public bool Expanded { get; set; } = true;

        // Ordem de exibição.
        public double Ordem { get; set; }

        // Nível na hierarquia (0=raiz, 1=filho, 2=neto).
        public int Nivel { get; set; }

        // Descrição do recurso.
        public string? Descricao { get; set; }

        // Indica se o recurso está ativo no menu.
        public bool Ativo { get; set; } = true;

        // Lista de filhos (subitens do menu).
        public List<RecursoTreeDTO>? Items { get; set; } = new List<RecursoTreeDTO>();

        // Converte um Recurso do banco para DTO.
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

        // Converte DTO para Recurso do banco.
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
