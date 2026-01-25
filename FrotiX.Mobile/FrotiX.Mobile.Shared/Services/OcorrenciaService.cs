using FrotiX.Mobile.Shared.Models;
using FrotiX.Mobile.Shared.Services.IServices;
using FrotiX.Mobile.Shared.Shared;

namespace FrotiX.Mobile.Shared.Services
{
    /// <summary>
    /// Serviço de Ocorrências de Viagem - comunica com a API via Azure Relay
    /// </summary>
    public class OcorrenciaService : IOcorrenciaService
    {
        private readonly RelayApiService _api;
        private readonly ILogService _logger;

        public OcorrenciaService(RelayApiService api, ILogService logger)
        {
            _api = api;
            _logger = logger;
        }

        /// <inheritdoc />
        public async Task<OperationResult<List<OcorrenciaViagem>>> GetOcorrenciasAbertasByVeiculoAsync(Guid veiculoId)
        {
            try
            {
                if (veiculoId == Guid.Empty)
                    return OperationResult<List<OcorrenciaViagem>>.Fail("ID do veículo não informado");

                var result = await _api.GetAsync<List<OcorrenciaViagem>>($"/api/ocorrencia/veiculo/{veiculoId}/abertas");
                return OperationResult<List<OcorrenciaViagem>>.Ok(result ?? new List<OcorrenciaViagem>());
            }
            catch (Exception ex)
            {
                _logger.Error($"Erro ao obter ocorrências do veículo {veiculoId}", ex);
                return OperationResult<List<OcorrenciaViagem>>.Fail($"Erro: {ex.Message}");
            }
        }

        /// <inheritdoc />
        public async Task<OperationResult<int>> ContarOcorrenciasAbertasVeiculoAsync(Guid veiculoId)
        {
            try
            {
                if (veiculoId == Guid.Empty)
                    return OperationResult<int>.Fail("ID do veículo não informado");

                var result = await _api.GetAsync<int>($"/api/ocorrencia/veiculo/{veiculoId}/contar");
                return OperationResult<int>.Ok(result);
            }
            catch (Exception ex)
            {
                _logger.Error($"Erro ao contar ocorrências do veículo {veiculoId}", ex);
                return OperationResult<int>.Fail($"Erro: {ex.Message}");
            }
        }

        /// <inheritdoc />
        public async Task<OperationResult<bool>> CriarOcorrenciaAsync(OcorrenciaViagem ocorrencia)
        {
            try
            {
                if (ocorrencia.VeiculoId == Guid.Empty)
                    return OperationResult<bool>.Fail("ID do veículo não informado");

                if (ocorrencia.OcorrenciaViagemId == Guid.Empty)
                    ocorrencia.OcorrenciaViagemId = Guid.NewGuid();

                if (ocorrencia.DataCriacao == default)
                    ocorrencia.DataCriacao = DateTime.Now;

                ocorrencia.StatusOcorrencia = true;
                ocorrencia.Status = "Aberta";

                var success = await _api.PostAsync("/api/ocorrencia/criar", ocorrencia);
                
                if (success)
                    return OperationResult<bool>.Ok(true);

                return OperationResult<bool>.Fail("Erro ao criar ocorrência");
            }
            catch (Exception ex)
            {
                _logger.Error("Erro ao criar ocorrência", ex);
                return OperationResult<bool>.Fail($"Erro: {ex.Message}");
            }
        }

        /// <inheritdoc />
        public async Task<OperationResult<int>> CriarOcorrenciasMultiplasAsync(CriarOcorrenciasDTO dto)
        {
            try
            {
                if (dto.VeiculoId == Guid.Empty)
                    return OperationResult<int>.Fail("ID do veículo não informado");

                if (dto.Ocorrencias == null || dto.Ocorrencias.Count == 0)
                    return OperationResult<int>.Ok(0);

                var count = await _api.PostAsync<int>("/api/ocorrencia/criar-multiplas", dto);
                return OperationResult<int>.Ok(count);
            }
            catch (Exception ex)
            {
                _logger.Error("Erro ao criar múltiplas ocorrências", ex);
                return OperationResult<int>.Fail($"Erro: {ex.Message}");
            }
        }

        /// <inheritdoc />
        public async Task<OperationResult<bool>> BaixarOcorrenciaAsync(Guid ocorrenciaId, string? solucao = null)
        {
            try
            {
                if (ocorrenciaId == Guid.Empty)
                    return OperationResult<bool>.Fail("ID da ocorrência não informado");

                var payload = new
                {
                    OcorrenciaId = ocorrenciaId,
                    DescricaoBaixa = solucao ?? "Baixa via Vistoria Mobile"
                };

                var success = await _api.PostAsync("/api/ocorrencia/baixar-mobile", payload);
                
                if (success)
                    return OperationResult<bool>.Ok(true);

                return OperationResult<bool>.Fail("Erro ao baixar ocorrência");
            }
            catch (Exception ex)
            {
                _logger.Error($"Erro ao baixar ocorrência {ocorrenciaId}", ex);
                return OperationResult<bool>.Fail($"Erro: {ex.Message}");
            }
        }

        /// <inheritdoc />
        public async Task<OperationResult<bool>> ExcluirOcorrenciaAsync(Guid ocorrenciaId)
        {
            try
            {
                if (ocorrenciaId == Guid.Empty)
                    return OperationResult<bool>.Fail("ID da ocorrência não informado");

                var success = await _api.DeleteAsync($"/api/ocorrencia/{ocorrenciaId}");
                
                if (success)
                    return OperationResult<bool>.Ok(true);

                return OperationResult<bool>.Fail("Erro ao excluir ocorrência");
            }
            catch (Exception ex)
            {
                _logger.Error($"Erro ao excluir ocorrência {ocorrenciaId}", ex);
                return OperationResult<bool>.Fail($"Erro: {ex.Message}");
            }
        }
    }
}
