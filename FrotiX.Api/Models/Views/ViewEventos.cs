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
    public class ViewEventos
    {

        public Guid? EventoId { get; set; }

        public string? Nome { get; set; }

        public string? Descricao { get; set; }

        public int? QtdParticipantes { get; set; }

        public string? DataInicial { get; set; }

        public string? DataFinal{ get; set; }

        public string? NomeRequisitante { get; set; }

        public string? NomeSetor { get; set; }

        public double? CustoViagem { get; set; }

        public string? Status { get; set; }


    }
}
