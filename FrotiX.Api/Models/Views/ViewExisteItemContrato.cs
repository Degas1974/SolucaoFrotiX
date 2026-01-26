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
    public class ViewExisteItemContrato
    {

        public Guid? ItemVeiculoId { get; set; }

        public Guid? ExisteVeiculo { get; set; }

        public Guid? RepactuacaoContratoId { get; set; }

        public int? NumItem { get; set; }

        public string? Descricao { get; set; }

        public int? Quantidade { get; set; }

        public double? ValUnitario { get; set; }

    }
}
