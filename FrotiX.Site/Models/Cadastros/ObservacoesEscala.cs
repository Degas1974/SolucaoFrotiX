// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ ARQUIVO: ObservacoesEscala.cs                                               ║
// ║ PROJETO: FrotiX - Sistema de Gestão de Frotas                               ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO:                                                                   ║
// ║ ViewModel para gestão de observações/avisos nas escalas.                    ║
// ║ Permite adicionar notas com período de exibição e prioridade.               ║
// ║                                                                              ║
// ║ CLASSES:                                                                      ║
// ║ • ObservacoesEscalaViewModel - ViewModel para o formulário                  ║
// ║ • (Entidade ObservacoesEscala está em Escalas.cs)                           ║
// ║                                                                              ║
// ║ PROPRIEDADES:                                                                ║
// ║ • ObservacaoId - ID da observação                                           ║
// ║ • DataEscala - Data da escala relacionada                                   ║
// ║ • Titulo - Título da observação (max 200 chars)                             ║
// ║ • Descricao - Descrição detalhada (MultilineText)                           ║
// ║ • Prioridade - Baixa, Normal, Alta (default: Normal)                        ║
// ║ • ExibirDe, ExibirAte - Período de exibição                                 ║
// ║ • PrioridadeList - Dropdown de prioridades                                  ║
// ║                                                                              ║
// ║ CONSTRUTOR:                                                                   ║
// ║ • Inicializa DataEscala e ExibirDe com DateTime.Today                       ║
// ║ • Inicializa ExibirAte com DateTime.Today.AddDays(7)                        ║
// ║ • Inicializa Prioridade com "Normal"                                        ║
// ║                                                                              ║
// ║ DOCUMENTADO EM: 2026-01-28 | LOTE: 18                                       ║
// ╚══════════════════════════════════════════════════════════════════════════════╝
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
