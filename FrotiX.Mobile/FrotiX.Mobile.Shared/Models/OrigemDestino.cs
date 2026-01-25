namespace FrotiX.Mobile.Shared.Models
{
    /// <summary>
    /// Classe para deserializar origens retornadas pela API
    /// (SELECT DISTINCT Origem FROM Viagem)
    /// </summary>
    public class Origem
    {
        public string Origens { get; set; } = string.Empty;
    }

    /// <summary>
    /// Classe para deserializar destinos retornados pela API
    /// (SELECT DISTINCT Destino FROM Viagem)
    /// </summary>
    public class Destino
    {
        public string Destinos { get; set; } = string.Empty;
    }
}
