namespace FrotiX.Mobile.Shared.Models
{
    /// <summary>
    /// DTO para criar múltiplas ocorrências de uma vez durante a finalização
    /// </summary>
    public class CriarOcorrenciasDTO
    {
        /// <summary>
        /// ID da viagem relacionada
        /// </summary>
        public Guid ViagemId { get; set; }

        /// <summary>
        /// ID do veículo relacionado
        /// </summary>
        public Guid VeiculoId { get; set; }

        /// <summary>
        /// ID do motorista (opcional)
        /// </summary>
        public Guid? MotoristaId { get; set; }

        /// <summary>
        /// Lista de ocorrências a serem criadas
        /// </summary>
        public List<OcorrenciaItemDTO> Ocorrencias { get; set; } = new();
    }
}
