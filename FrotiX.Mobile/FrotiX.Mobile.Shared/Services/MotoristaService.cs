using FrotiX.Mobile.Shared.Models;
using FrotiX.Mobile.Shared.Services.IServices;

namespace FrotiX.Mobile.Shared.Services
{
    public class MotoristaService : IMotoristaService
    {
        private readonly RelayApiService _api;
        private readonly ILogService _logger;

        public MotoristaService(RelayApiService api, ILogService logger)
        {
            _api = api;
            _logger = logger;
        }

        public async Task<List<MotoristaViewModel>> ObterMotoristasAsync()
        {
            try
            {
                var motoristas = await _api.GetAsync<List<MotoristaViewModel>>("/api/motoristas/dropdown");
                return motoristas ?? new List<MotoristaViewModel>();
            }
            catch (Exception ex)
            {
                _logger.Error("Erro ao obter motoristas da API", ex);
                return new List<MotoristaViewModel>();
            }
        }
    }
}
