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
    public class LavadorContratoViewModel
    {
        public Guid LavadorId { get; set; }
        public Guid ContratoId { get; set; }
        public LavadorContrato LavadorContrato { get; set; }
    }

    public class LavadorContrato
    {
        //2 Foreign Keys as Primary Key
        //=============================
        [Key, Column(Order = 0)]
        public Guid LavadorId { get; set; }

        [Key, Column(Order = 1)]
        public Guid ContratoId { get; set; }

    }
}
