// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ ARQUIVO: FiltroEscala.cs                                                    ║
// ║ PROJETO: FrotiX - Sistema de Gestão de Frotas                               ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO:                                                                   ║
// ║ ViewModel para filtros de pesquisa nas escalas de motoristas.               ║
// ║ Permite buscas por diversos critérios.                                      ║
// ║                                                                              ║
// ║ CLASSES:                                                                      ║
// ║ • FiltroEscalaViewModel - ViewModel para formulário de filtro               ║
// ║                                                                              ║
// ║ PROPRIEDADES:                                                                ║
// ║ Critérios de Filtro:                                                         ║
// ║ • DataFiltro - Data da escala                                               ║
// ║ • TipoServicoId - Tipo de serviço                                           ║
// ║ • Lotacao - Local de lotação                                                ║
// ║ • VeiculoId - Veículo específico                                            ║
// ║ • MotoristaId - Motorista específico                                        ║
// ║ • StatusMotorista - Status do motorista                                     ║
// ║ • TurnoId - Turno de trabalho                                               ║
// ║ • TextoPesquisa - Texto livre para busca                                    ║
// ║                                                                              ║
// ║ Dropdowns (SelectListItem):                                                  ║
// ║ • TipoServicoList, LotacaoList, VeiculoList                                 ║
// ║ • MotoristaList, StatusList, TurnoList                                      ║
// ║                                                                              ║
// ║ DOCUMENTADO EM: 2026-01-28 | LOTE: 18                                       ║
// ╚══════════════════════════════════════════════════════════════════════════════╝
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
