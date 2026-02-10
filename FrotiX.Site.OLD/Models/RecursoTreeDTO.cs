/* ****************************************************************************************
 * ⚡ ARQUIVO: RecursoTreeDTO.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Representar recursos em TreeView Syncfusion (ejs-treeview).
 *
 * 📥 ENTRADAS     : Dados de recurso e hierarquia.
 *
 * 📤 SAÍDAS       : DTO para montagem de árvore de navegação.
 *
 * 🔗 CHAMADA POR  : Telas de administração de menu.
 *
 * 🔄 CHAMA        : System.Collections.Generic.
 *
 * 📦 DEPENDÊNCIAS : System.Collections.Generic.
 **************************************************************************************** */

using System;
using System.Collections.Generic;

namespace FrotiX.Models
{
    /****************************************************************************************
     * ⚡ DTO: RecursoTreeDTO
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Representar recurso no TreeView Syncfusion.
     *
     * 📥 ENTRADAS     : Identificadores, texto, URL e hierarquia.
     *
     * 📤 SAÍDAS       : Estrutura pronta para árvore de navegação.
     *
     * 🔗 CHAMADA POR  : APIs e telas de administração.
     *
     * 🔄 CHAMA        : Recurso.
     ****************************************************************************************/
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

        /****************************************************************************************
         * ⚡ MÉTODO: FromRecurso
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Converter entidade Recurso em DTO.
         *
         * 📥 ENTRADAS     : recurso.
         *
         * 📤 SAÍDAS       : RecursoTreeDTO.
         *
         * 🔗 CHAMADA POR  : Serviços de administração.
         *
         * 🔄 CHAMA        : Guid.ToString.
         ****************************************************************************************/
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

        /****************************************************************************************
         * ⚡ MÉTODO: ToRecurso
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Converter DTO em entidade Recurso.
         *
         * 📥 ENTRADAS     : Nenhuma (dados do DTO).
         *
         * 📤 SAÍDAS       : Recurso.
         *
         * 🔗 CHAMADA POR  : Serviços de persistência.
         *
         * 🔄 CHAMA        : Guid.TryParse, Guid.NewGuid.
         ****************************************************************************************/
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
