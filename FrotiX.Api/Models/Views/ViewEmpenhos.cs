using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using FrotiXApi.Validations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FrotiXApi.Models
{
    public class ViewEmpenhos
    {

        public Guid EmpenhoId { get; set; }

        public string? NotaEmpenho { get; set; }

        public DateTime? DataEmissao { get; set; }

        public int? AnoVigencia { get; set; }

        public DateTime? VigenciaInicial { get; set; }

        public DateTime? VigenciaFinal { get; set; }

        public double? SaldoInicial { get; set; }

        public double? SaldoFinal { get; set; }

        public double? SaldoMovimentacao { get; set; }

        public double? SaldoNotas { get; set; }

        public int? Movimentacoes { get; set; }

        public Guid? ContratoId { get; set; }

        public Guid? AtaId { get; set; }


    }
}
