using System;

namespace FrotiX.Mobile.Shared.Models
{
    /// <summary>
    /// ViewModel para exibição de vistorias na lista
    /// </summary>
    public class ViewViagemVistoria
    {
        public Guid ViagemId { get; set; }
        
        /// <summary>
        /// Nº Ficha Vistoria - Campo obrigatório para exibição na lista
        /// </summary>
        public int? NoFichaVistoria { get; set; }
        
        public Guid? VeiculoId { get; set; }
        public string? VeiculoCompleto { get; set; }
        
        public Guid? MotoristaId { get; set; }
        public string? MotoristaCondutor { get; set; }
        
        public DateTime? DataInicial { get; set; }
        public DateTime? HoraInicio { get; set; }
        public DateTime? DataFinal { get; set; }
        public DateTime? HoraFim { get; set; }
        
        public string? Origem { get; set; }
        public string? Destino { get; set; }
        public string? Finalidade { get; set; }
        
        public string? Status { get; set; }
        public bool? StatusAgendamento { get; set; }
        
        public int? KmInicial { get; set; }
        public int? KmFinal { get; set; }
        
        public string? NivelCombustivelInicial { get; set; }
        public string? NivelCombustivelFinal { get; set; }
    }
}
