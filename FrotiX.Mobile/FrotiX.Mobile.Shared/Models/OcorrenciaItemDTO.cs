namespace FrotiX.Mobile.Shared.Models
{
    /// <summary>
    /// DTO para representar um item de ocorrência na finalização de vistoria
    /// </summary>
    public class OcorrenciaItemDTO
    {
        /// <summary>
        /// Resumo breve da ocorrência (obrigatório)
        /// </summary>
        public string Resumo { get; set; } = string.Empty;

        /// <summary>
        /// Descrição detalhada da ocorrência (opcional)
        /// </summary>
        public string? Descricao { get; set; }

        /// <summary>
        /// Imagem em base64 da ocorrência (opcional)
        /// </summary>
        public string? ImagemOcorrencia { get; set; }
    }
}
