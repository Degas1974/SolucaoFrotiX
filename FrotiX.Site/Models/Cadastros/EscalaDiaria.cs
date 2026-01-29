/*
 * ╔══════════════════════════════════════════════════════════════════════════╗
 * ║  📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                   ║
 * ║  Arquivo: Models/Cadastros/EscalaDiaria.cs                               ║
 * ║  Descrição: Entidade e ViewModels para escalas diárias de motoristas     ║
 * ║  Data: 29/01/2026 | LOTE: 22                                             ║
 * ╚══════════════════════════════════════════════════════════════════════════╝
 */

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace FrotiX.Models
{

    public class EscalaDiariaViewModel
    {
        public Guid EscalaDiaId { get; set; }

        // Dados do Motorista
        [Required(ErrorMessage = "O motorista Ã© obrigatÃ³rio")]
        [Display(Name = "Motorista")]
        public Guid? MotoristaId { get; set; }

        // Dados do VeÃ­culo
        [Display(Name = "VeÃ­culo")]
        public Guid? VeiculoId { get; set; }

        // Dados da Escala
        [Required(ErrorMessage = "O tipo de serviÃ§o Ã© obrigatÃ³rio")]
        [Display(Name = "Tipo de ServiÃ§o")]
        public Guid TipoServicoId { get; set; }

        [Required(ErrorMessage = "O turno Ã© obrigatÃ³rio")]
        [Display(Name = "Turno")]
        public Guid TurnoId { get; set; }

        [Required(ErrorMessage = "A data da escala Ã© obrigatÃ³ria")]
        [Display(Name = "Data da Escala")]
        [DataType(DataType.Date)]
        public DateTime DataEscala { get; set; }

        [Required(ErrorMessage = "A hora de inÃ­cio Ã© obrigatÃ³ria")]
        [Display(Name = "Hora InÃ­cio")]
        public string HoraInicio { get; set; }

        [Required(ErrorMessage = "A hora de fim Ã© obrigatÃ³ria")]
        [Display(Name = "Hora Fim")]
        public string HoraFim { get; set; }

        [Display(Name = "InÃ­cio do Intervalo")]
        public string? HoraIntervaloInicio { get; set; }

        [Display(Name = "Fim do Intervalo")]
        public string? HoraIntervaloFim { get; set; }

        [Display(Name = "LotaÃ§Ã£o")]
        public string? Lotacao { get; set; }

        [Display(Name = "NÃºmero de SaÃ­das")]
        public int NumeroSaidas { get; set; } = 0;

        [Required(ErrorMessage = "O status do motorista Ã© obrigatÃ³rio")]
        [Display(Name = "Status do Motorista")]
        public string StatusMotorista { get; set; } = "DisponÃ­vel";

        [Display(Name = "Requisitante (ServiÃ§o Fixo)")]
        public Guid? RequisitanteId { get; set; }

        [Display(Name = "ObservaÃ§Ãµes")]
        [DataType(DataType.MultilineText)]
        public string? Observacoes { get; set; }

        // Checkboxes para status especiais
        public bool MotoristaIndisponivel { get; set; }
        public bool MotoristaEconomildo { get; set; }
        public bool MotoristaEmServico { get; set; }
        public bool MotoristaReservado { get; set; }

        // Campos para Indisponibilidade
        [Display(Name = "Categoria")]
        public string? CategoriaIndisponibilidade { get; set; } // Folga, FÃ©rias, Recesso

        [Display(Name = "Data InÃ­cio Indisponibilidade")]
        [DataType(DataType.Date)]
        public DateTime? DataInicioIndisponibilidade { get; set; }

        [Display(Name = "Data Fim Indisponibilidade")]
        [DataType(DataType.Date)]
        public DateTime? DataFimIndisponibilidade { get; set; }

        [Display(Name = "Motorista Cobertor")]
        public Guid? MotoristaCobertorId { get; set; }

        // Listas para dropdowns
        public IEnumerable<SelectListItem>? MotoristaList { get; set; }
        public IEnumerable<SelectListItem>? VeiculoList { get; set; }
        public IEnumerable<SelectListItem>? TipoServicoList { get; set; }
        public IEnumerable<SelectListItem>? TurnoList { get; set; }
        public IEnumerable<SelectListItem>? RequisitanteList { get; set; }
        public IEnumerable<SelectListItem>? LotacaoList { get; set; }
        public IEnumerable<SelectListItem>? StatusList { get; set; }

        // Dados para exibiÃ§Ã£o
        public string? NomeMotorista { get; set; }
        public string? DescricaoVeiculo { get; set; }
        public string? NomeTurno { get; set; }
        public string? NomeServico { get; set; }
        public string? NomeRequisitante { get; set; }
        public string? NomeUsuarioAlteracao { get; set; }

        public EscalaDiariaViewModel()
        {
            DataEscala = DateTime.Today;
            StatusMotorista = "DisponÃ­vel";
        }
    }
}
