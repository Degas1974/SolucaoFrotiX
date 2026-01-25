using System;

namespace FrotiX.Mobile.Shared.Models
{
    /// <summary>
    /// DTO para armazenamento local de veículos
    /// Contém os campos necessários da ViewVeiculoCompleto
    /// </summary>
    public class VeiculoLocalDto
    {
        public Guid VeiculoId { get; set; }
        public string Placa { get; set; } = string.Empty;
        public string VeiculoCompleto { get; set; } = string.Empty;
        public bool Economildo { get; set; }
        public bool Status { get; set; }
    }
}
