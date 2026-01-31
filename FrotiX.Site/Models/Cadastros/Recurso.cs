/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 📌 ARQUIVO: Recurso.cs                                                                              ║
   ║ 📂 CAMINHO: /Models/Cadastros                                                                       ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🧭 OBJETIVO: Definir recursos/menus do sistema e sua hierarquia para controle de acesso.           ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🗂️  CONTÉM: RecursoViewModel, Recurso                                                               ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🔗 DEPENDÊNCIAS: DataAnnotations, SelectListItem, Identity                                          ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝ */

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FrotiX.Models
{
    // ==================================================================================================
    // VIEW MODEL
    // ==================================================================================================
    // Finalidade: transportar dados do recurso em operações de tela.
    // ==================================================================================================
    public class RecursoViewModel
    {
        // Identificador do recurso.
        public Guid RecursoId { get; set; }

        // Nome interno do recurso.
        public string? Nome { get; set; }

        // Nome exibido no menu.
        public string? NomeMenu { get; set; }

        // Descrição do recurso.
        public string? Descricao { get; set; }

        // Ordem de exibição.
        public double? Ordem { get; set; }

        // Entidade principal do formulário.
        public Recurso? Recurso { get; set; }
    }

    // ==================================================================================================
    // ENTIDADE
    // ==================================================================================================
    // Representa um recurso de menu com hierarquia (pai/filhos).
    // ==================================================================================================
    public class Recurso
    {
        // Identificador único do recurso.
        [Key]
        public Guid RecursoId { get; set; }

        // Nome do recurso.
        [Required(ErrorMessage = "O nome do Recurso é obrigatório")]
        [Display(Name = "Nome do Recurso")]
        public string Nome { get; set; } = string.Empty;

        // Nome do menu.
        [Required(ErrorMessage = "O nome de Menu do Recurso é obrigatório")]
        [Display(Name = "Nome de Menu do Recurso")]
        public string NomeMenu { get; set; } = string.Empty;

        // Descrição do recurso.
        [Display(Name = "Descrição do Recurso")]
        public string? Descricao { get; set; }

        // Ordem de exibição.
        [Required(ErrorMessage = "A Ordem é obrigatória")]
        [Display(Name = "Ordem do Recurso")]
        public double Ordem { get; set; }

        // ====== CAMPOS PARA NAVEGAÇÃO HIERÁRQUICA ======

        // Recurso pai (nível superior).
        [Display(Name = "Recurso Pai")]
        public Guid? ParentId { get; set; }

        // Ícone do menu (FontAwesome).
        [Required(ErrorMessage = "O ícone é obrigatório")]
        [Display(Name = "Ícone FontAwesome")]
        public string Icon { get; set; } = "fa-duotone fa-folder";

        // URL da página.
        [Required(ErrorMessage = "A URL é obrigatória")]
        [Display(Name = "URL da Página")]
        public string Href { get; set; } = "javascript:void(0);";

        // Indica se o recurso aparece no menu.
        [Display(Name = "Ativo no Menu")]
        public bool Ativo { get; set; } = true;

        // Nível na hierarquia.
        [Display(Name = "Nível na Hierarquia")]
        public int Nivel { get; set; } = 0;

        // Indica se possui filhos.
        [Display(Name = "Tem Filhos")]
        public bool HasChild { get; set; } = false;

        // Navegação EF Core
        // Recurso pai.
        public virtual Recurso? Parent { get; set; }

        // Lista de filhos.
        public virtual ICollection<Recurso> Children { get; set; } = new List<Recurso>();
    }
}
