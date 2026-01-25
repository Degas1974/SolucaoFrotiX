using FrotiX.Mobile.Shared.Models;
using FrotiX.Mobile.Shared.Shared;

namespace FrotiX.Mobile.Shared.Services.IServices
{
    /// <summary>
    /// Interface para o serviço de Ocorrências de Viagem
    /// </summary>
    public interface IOcorrenciaService
    {
        /// <summary>
        /// Lista ocorrências em aberto de um veículo específico
        /// </summary>
        Task<OperationResult<List<OcorrenciaViagem>>> GetOcorrenciasAbertasByVeiculoAsync(Guid veiculoId);

        /// <summary>
        /// Conta quantas ocorrências em aberto um veículo possui
        /// </summary>
        Task<OperationResult<int>> ContarOcorrenciasAbertasVeiculoAsync(Guid veiculoId);

        /// <summary>
        /// Cria uma nova ocorrência
        /// </summary>
        Task<OperationResult<bool>> CriarOcorrenciaAsync(OcorrenciaViagem ocorrencia);

        /// <summary>
        /// Cria múltiplas ocorrências de uma vez (finalização de viagem)
        /// </summary>
        Task<OperationResult<int>> CriarOcorrenciasMultiplasAsync(CriarOcorrenciasDTO dto);

        /// <summary>
        /// Dá baixa em uma ocorrência (marca como resolvida)
        /// </summary>
        Task<OperationResult<bool>> BaixarOcorrenciaAsync(Guid ocorrenciaId, string? solucao = null);

        /// <summary>
        /// Exclui uma ocorrência
        /// </summary>
        Task<OperationResult<bool>> ExcluirOcorrenciaAsync(Guid ocorrenciaId);
    }
}
