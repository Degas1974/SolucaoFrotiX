/* ****************************************************************************************
 * ⚡ ARQUIVO: CoberturaFolga.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : ViewModel para cobertura de folgas de motoristas (substituições).
 *
 * 📥 ENTRADAS     : Dados de período, motoristas envolvidos e observações.
 *
 * 📤 SAÍDAS       : ViewModel para telas de escala/agenda.
 *
 * 🔗 CHAMADA POR  : Controllers/Views de escala.
 *
 * 🔄 CHAMA        : DataAnnotations e SelectListItem.
 *
 * 📦 DEPENDÊNCIAS : System.ComponentModel.DataAnnotations, Microsoft.AspNetCore.Mvc.Rendering.
 **************************************************************************************** */

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace FrotiX.Models
{
    /****************************************************************************************
     * ⚡ VIEWMODEL: CoberturaFolgaViewModel
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Representar cobertura de folga e listas de apoio para a UI.
     *
     * 📥 ENTRADAS     : Motoristas, período e motivo/observações.
     *
     * 📤 SAÍDAS       : Dados prontos para formulários e listagens.
     *
     * 🔗 CHAMADA POR  : Controllers/Views de escala.
     *
     * 🔄 CHAMA        : SelectListItem.
     ****************************************************************************************/
    public class CoberturaFolgaViewModel
    {
        public Guid CoberturaId { get; set; }

        [Required(ErrorMessage = "O motorista em folga Ã© obrigatÃ³rio")]
        [Display(Name = "Motorista em Folga")]
        public Guid MotoristaFolgaId { get; set; }

        [Required(ErrorMessage = "O motorista cobertor Ã© obrigatÃ³rio")]
        [Display(Name = "Motorista Cobertor")]
        public Guid MotoristaCoberturaId { get; set; }

        [Required(ErrorMessage = "A data de inÃ­cio Ã© obrigatÃ³ria")]
        [Display(Name = "De")]
        [DataType(DataType.Date)]
        public DateTime DataInicio { get; set; }

        [Required(ErrorMessage = "A data de fim Ã© obrigatÃ³ria")]
        [Display(Name = "AtÃ©")]
        [DataType(DataType.Date)]
        public DateTime DataFim { get; set; }

        [Display(Name = "Motivo")]
        public string? Motivo { get; set; }

        [Display(Name = "ObservaÃ§Ãµes")]
        [DataType(DataType.MultilineText)]
        public string? StatusOriginal { get; set; }
        public string? Observacoes { get; set; }

        // Listas para dropdowns
        public IEnumerable<SelectListItem>? MotoristaList { get; set; }

        // Dados para exibiÃ§Ã£o
        public string? NomeMotoristaFolga { get; set; }
        public string? NomeMotoristaCobertor { get; set; }

        /****************************************************************************************
         * ⚡ FUNÇÃO: CoberturaFolgaViewModel (Construtor)
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Inicializar datas padrão com o dia atual.
         *
         * 📥 ENTRADAS     : Nenhuma.
         *
         * 📤 SAÍDAS       : Datas preenchidas com DateTime.Today.
         *
         * 🔗 CHAMADA POR  : DI e instanciação manual.
         *
         * 🔄 CHAMA        : DateTime.Today.
         ****************************************************************************************/
        public CoberturaFolgaViewModel()
        {
            DataInicio = DateTime.Today;
            DataFim = DateTime.Today;
        }
    }
}
