/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: FiltroEscala.cs                                                                         ║
   ║ 📂 CAMINHO: /Models/Cadastros                                                                       ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO: ViewModel para filtros de pesquisa em escalas (por data, tipo, turno, motorista).     ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 PROPS: DataFiltro, TipoServicoId, Lotacao, VeiculoId, MotoristaId, StatusMotorista, TurnoId     ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🔗 DEPS: System.ComponentModel.DataAnnotations, SelectListItem                                      ║
   ║ 📅 Atualizado: 2026 | 👤 FrotiX Team | 📝 Versão: 2.0                                              ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝ */

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
