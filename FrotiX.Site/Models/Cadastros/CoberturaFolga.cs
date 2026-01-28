// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ ARQUIVO: CoberturaFolga.cs                                                  ║
// ║ PROJETO: FrotiX - Sistema de Gestão de Frotas                               ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO:                                                                   ║
// ║ ViewModel para gestão de coberturas de folga entre motoristas.              ║
// ║ Permite escalar motorista substituto durante ausências.                     ║
// ║                                                                              ║
// ║ CLASSES:                                                                      ║
// ║ • CoberturaFolgaViewModel - ViewModel para o formulário                     ║
// ║ • (Entidade CoberturaFolga está em Escalas.cs)                              ║
// ║                                                                              ║
// ║ PROPRIEDADES:                                                                ║
// ║ • CoberturaId - ID da cobertura                                             ║
// ║ • MotoristaFolgaId - Motorista que está em folga                            ║
// ║ • MotoristaCoberturaId - Motorista que vai cobrir                           ║
// ║ • DataInicio, DataFim - Período da cobertura                                ║
// ║ • Motivo - Motivo da ausência (Folga, Férias, Atestado)                     ║
// ║ • StatusOriginal - Status original do motorista em folga                    ║
// ║ • Observacoes - Observações adicionais                                      ║
// ║ • MotoristaList - Dropdown de motoristas                                    ║
// ║ • NomeMotoristaFolga, NomeMotoristaCobertor - Nomes para exibição           ║
// ║                                                                              ║
// ║ NOTA: Inicializa DataInicio/DataFim com DateTime.Today                      ║
// ║                                                                              ║
// ║ DOCUMENTADO EM: 2026-01-28 | LOTE: 18                                       ║
// ╚══════════════════════════════════════════════════════════════════════════════╝
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace FrotiX.Models
{
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

        public CoberturaFolgaViewModel()
        {
            DataInicio = DateTime.Today;
            DataFim = DateTime.Today;
        }
    }
}
