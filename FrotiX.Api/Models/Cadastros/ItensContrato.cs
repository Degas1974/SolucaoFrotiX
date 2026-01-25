using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using FrotiXApi.Services;
using FrotiXApi.Validations;

namespace FrotiXApi.Models
{
    public class ItensContratoViewModel
    {
        public Guid ContratoId { get; set; }
        public ItensContrato ItensContrato { get; set; }

        public IEnumerable<SelectListItem> ContratoList { get; set; }


    }

    public class ItensContrato
    {

        [NotMapped]
        public Guid ContratoId { get; set; }

    }
}
