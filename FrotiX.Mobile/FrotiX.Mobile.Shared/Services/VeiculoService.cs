using FrotiX.Mobile.Shared.Models;
using FrotiX.Mobile.Shared.Services.IServices;

namespace FrotiX.Mobile.Shared.Services
{
    public class VeiculoService : IVeiculoService
    {
        private readonly RelayApiService _api;
        private readonly ILogService _logger;

        public VeiculoService(RelayApiService api, ILogService logger)
        {
            _api = api;
            _logger = logger;
        }

        public async Task<List<VeiculoViewModel>> ObterVeiculoCompletoDropdownAsync()
        {
            try
            {
                var veiculos = await _api.GetAsync<List<VeiculoViewModel>>("/api/veiculos/economildo");
                return veiculos ?? new List<VeiculoViewModel>();
            }
            catch (Exception ex)
            {
                _logger.Error("Erro ao obter veículos da API", ex);
                return new List<VeiculoViewModel>();
            }
        }

        public async Task<VeiculoViewModel?> GetVeiculoByIdAsync(Guid id)
        {
            try
            {
                return await _api.GetAsync<VeiculoViewModel>($"/api/veiculos/{id}");
            }
            catch (Exception ex)
            {
                _logger.Error($"Erro ao obter veículo {id} da API", ex);
                return null;
            }
        }
    }
}
