using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using FrotiXApi.Services;
using FrotiXApi.Validations;
using Microsoft.AspNetCore.Http;

namespace FrotiXApi.Models
{
    public class ViewProcuraFicha
    {

        public Guid? MotoristaId { get; set; }

        public Guid? VeiculoId { get; set; }

        public DateTime? DataInicial { get; set; }

        public DateTime? DataFinal { get; set; }

        public string? HoraInicio { get; set; }

        public string? HoraFim { get; set; }

        public int? NoFichaVistoria { get; set; }

    }
}
