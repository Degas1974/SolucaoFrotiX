/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 📌 ARQUIVO: ObservacoesEscala.cs                                                                    ║
   ║ 📂 CAMINHO: /Models/Cadastros                                                                       ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🧭 OBJETIVO: Gerenciar observações de escala com período de exibição e prioridade.                 ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🗂️  CONTÉM: ObservacoesEscalaViewModel                                                              ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🔗 DEPENDÊNCIAS: DataAnnotations, SelectListItem                                                    ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝ */

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;



namespace FrotiX.Models
{

    // ==================================================================================================
    // VIEW MODEL
    // ==================================================================================================
    // Finalidade: registrar observações e controlar período/prioridade de exibição.
    // ==================================================================================================
    public class ObservacoesEscalaViewModel
    {
        // Identificador da observação.
        public Guid ObservacaoId { get; set; }

        // Data da escala.
        [Required(ErrorMessage = "A data da escala Ã© obrigatÃ³ria")]
        [Display(Name = "Data da Escala")]
        [DataType(DataType.Date)]
        public DateTime DataEscala { get; set; }

        // Título da observação.
        [StringLength(200)]
        [Display(Name = "TÃ­tulo")]
        public string? Titulo { get; set; }

        // Descrição detalhada.
        [Required(ErrorMessage = "A descriÃ§Ã£o Ã© obrigatÃ³ria")]
        [Display(Name = "DescriÃ§Ã£o")]
        [DataType(DataType.MultilineText)]
        public string Descricao { get; set; }

        // Prioridade de exibição.
        [Display(Name = "Prioridade")]
        public string Prioridade { get; set; } = "Normal";

        // Data de início de exibição.
        [Required(ErrorMessage = "A data de inÃ­cio de exibiÃ§Ã£o Ã© obrigatÃ³ria")]
        [Display(Name = "Exibir De")]
        [DataType(DataType.Date)]
        public DateTime ExibirDe { get; set; }

        // Data de fim de exibição.
        [Required(ErrorMessage = "A data de fim de exibiÃ§Ã£o Ã© obrigatÃ³ria")]
        [Display(Name = "Exibir AtÃ©")]
        [DataType(DataType.Date)]
        public DateTime ExibirAte { get; set; }

        // Lista para prioridade
        public IEnumerable<SelectListItem>? PrioridadeList { get; set; }

        // Define valores padrão para datas e prioridade.
        public ObservacoesEscalaViewModel()
        {
            DataEscala = DateTime.Today;
            ExibirDe = DateTime.Today;
            ExibirAte = DateTime.Today.AddDays(7);
            Prioridade = "Normal";
        }
    }
}
