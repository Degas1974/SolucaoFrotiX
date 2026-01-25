namespace FrotiX.Mobile.Shared.Models
{
    /// <summary>
    /// ViewModel para exibição de viagens abertas (aguardando vistoria final)
    /// </summary>
    public class ViagemAbertaViewModel
    {
        public int ViagemId { get; set; }
        public string DescricaoViagem { get; set; } = "";
        public string VeiculoDescricao { get; set; } = "";
        public string MotoristaNome { get; set; } = "";
        public DateTime DataHoraSaida { get; set; }
        public int KmSaida { get; set; }
        public int NivelCombustivelSaida { get; set; }
        public string? ObservacoesSaida { get; set; }
        public string? VistoriadorInicialNome { get; set; }
    }
}
