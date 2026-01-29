/*
 * ╔══════════════════════════════════════════════════════════════════════════╗
 * ║  📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                   ║
 * ║  Arquivo: Models/Cadastros/FiltroEscala.cs                               ║
 * ║  Descrição: ViewModel para filtros de pesquisa em escalas               ║
 * ║  Data: 29/01/2026 | LOTE: 22                                             ║
 * ╚══════════════════════════════════════════════════════════════════════════╝
 */

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FrotiX.Models
{

    public class FiltroEscalaViewModel
    {
        [Display(Name = "Data")]
        [DataType(DataType.Date)]
        public DateTime? DataFiltro { get; set; }

        [Display(Name = "Tipo de Serviço")]
        public Guid? TipoServicoId { get; set; }

        [Display(Name = "Lotação")]
        public string? Lotacao { get; set; }

        [Display(Name = "Veículo")]
        public Guid? VeiculoId { get; set; }

        [Display(Name = "Motorista")]
        public Guid? MotoristaId { get; set; }

        [Display(Name = "Status")]
        public string? StatusMotorista { get; set; }

        [Display(Name = "Turno")]
        public Guid? TurnoId { get; set; }

        [Display(Name = "Pesquisar")]
        public string? TextoPesquisa { get; set; }

        // Listas para dropdowns
        public IEnumerable<SelectListItem>? TipoServicoList { get; set; }
        public IEnumerable<SelectListItem>? LotacaoList { get; set; }
        public IEnumerable<SelectListItem>? VeiculoList { get; set; }
        public IEnumerable<SelectListItem>? MotoristaList { get; set; }
        public IEnumerable<SelectListItem>? StatusList { get; set; }
        public IEnumerable<SelectListItem>? TurnoList { get; set; }
    }

}
