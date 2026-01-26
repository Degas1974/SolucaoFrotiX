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
    public class ViewSetores
    {

        public Guid? SetorSolicitanteId { get; set; }

        public string? Nome { get; set; }

        public Guid? SetorPaiId { get; set; }

    }
}
