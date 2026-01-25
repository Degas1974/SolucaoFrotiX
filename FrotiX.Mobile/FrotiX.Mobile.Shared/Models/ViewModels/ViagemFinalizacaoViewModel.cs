using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrotiX.Mobile.Shared.Models.ViewModels
{
    public class ViagemFinalizacaoViewModel
    {
        public Guid ViagemId { get; set; }

        public DateTime? DataInicial { get; set; }

        public DateTime? HoraInicio { get; set; }

        [Required(ErrorMessage = "Informe a data final.")]
        public DateTime? DataFinal { get; set; }

        [Required(ErrorMessage = "Informe a hora final.")]
        public DateTime? HoraFim { get; set; }

        public string? NivelCombustivelFinal { get; set; }

        public int? KmInicial { get; set; }

        [Required(ErrorMessage = "Informe o km final.")]
        public int? KmFinal { get; set; }

        public string? DanoAvariaFinal { get; set; }

        [Required(ErrorMessage = "A assinatura é obrigatória.")]
        public string? RubricaFinal { get; set; }

        public string? FotosFinaisBase64 { get; set; }
        public string? VideosFinaisBase64 { get; set; }
    }
}
