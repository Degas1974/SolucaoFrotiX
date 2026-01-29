/*
 * ╔══════════════════════════════════════════════════════════════════════════╗
 * ║  📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                   ║
 * ║  Arquivo: Models/Cadastros/ObservacoesEscala.cs                          ║
 * ║  Descrição: Entidade e ViewModels para observações em escalas           ║
 * ║  Data: 29/01/2026 | LOTE: 22                                             ║
 * ╚══════════════════════════════════════════════════════════════════════════╝
 */

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;



namespace FrotiX.Models
{

    public class ObservacoesEscalaViewModel
    {
        public Guid ObservacaoId { get; set; }

        [Required(ErrorMessage = "A data da escala Ã© obrigatÃ³ria")]
        [Display(Name = "Data da Escala")]
        [DataType(DataType.Date)]
        public DateTime DataEscala { get; set; }

        [StringLength(200)]
        [Display(Name = "TÃ­tulo")]
        public string? Titulo { get; set; }

        [Required(ErrorMessage = "A descriÃ§Ã£o Ã© obrigatÃ³ria")]
        [Display(Name = "DescriÃ§Ã£o")]
        [DataType(DataType.MultilineText)]
        public string Descricao { get; set; }

        [Display(Name = "Prioridade")]
        public string Prioridade { get; set; } = "Normal";

        [Required(ErrorMessage = "A data de inÃ­cio de exibiÃ§Ã£o Ã© obrigatÃ³ria")]
        [Display(Name = "Exibir De")]
        [DataType(DataType.Date)]
        public DateTime ExibirDe { get; set; }

        [Required(ErrorMessage = "A data de fim de exibiÃ§Ã£o Ã© obrigatÃ³ria")]
        [Display(Name = "Exibir AtÃ©")]
        [DataType(DataType.Date)]
        public DateTime ExibirAte { get; set; }

        // Lista para prioridade
        public IEnumerable<SelectListItem>? PrioridadeList { get; set; }

        public ObservacoesEscalaViewModel()
        {
            DataEscala = DateTime.Today;
            ExibirDe = DateTime.Today;
            ExibirAte = DateTime.Today.AddDays(7);
            Prioridade = "Normal";
        }
    }
}
