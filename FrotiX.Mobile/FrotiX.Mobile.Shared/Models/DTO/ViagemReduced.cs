using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrotiX.Mobile.Shared.Models.DTO
{
    public class ViagemReduced
    {
        public Guid ViagemId { get; set; }
        public DateTime? DataInicial { get; set; } // ← corrigido
        public TimeSpan? HoraInicio { get; set; } // ← corrigido
        public string? PlacaVeiculo { get; set; }
        public string? NomeMotorista { get; set; }
        public DateTime? DataFinal { get; set; }
        public string? Origem { get; set; }
        public string? Destino { get; set; }
        public string? RamalRequisitante { get; set; }
        public string? Rubrica { get; set; }
    }
}
