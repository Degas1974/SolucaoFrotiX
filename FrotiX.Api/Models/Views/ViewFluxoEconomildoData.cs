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
    public class ViewFluxoEconomildoData
    {

        public Guid? VeiculoId { get; set; }

        public Guid? ViagemEconomildoId { get; set; }

        public Guid? MotoristaId { get; set; }

        public string? TipoCondutor { get; set; }

        public DateTime? Data { get; set; }

        public string? MOB { get; set; }

        public string? HoraInicio { get; set; }

        public string? HoraFim { get; set; }

        public int? QtdPassageiros { get; set; }

        public string? NomeMotorista { get; set; }

        public string? DescricaoVeiculo { get; set; }


    }
}
