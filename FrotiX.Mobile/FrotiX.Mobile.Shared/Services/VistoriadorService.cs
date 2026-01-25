using FrotiX.Mobile.Shared.Models;
using FrotiX.Mobile.Shared.Services.IServices;

namespace FrotiX.Mobile.Shared.Services
{
    public class VistoriadorService : IVistoriadorService
    {
        private readonly RelayApiService _api;
        private readonly ILogService _logger;

        public VistoriadorService(RelayApiService api, ILogService logger)
        {
            _api = api;
            _logger = logger;
        }

        public async Task<List<VistoriadorViewModel>> GetVistoriadoresAsync()
        {
            try
            {
                var vistoriadores = await _api.GetAsync<List<VistoriadorViewModel>>("/api/Vistoriadores");
                return vistoriadores ?? new List<VistoriadorViewModel>();
            }
            catch (Exception ex)
            {
                _logger.Error("Erro ao obter vistoriadores da API", ex);
                return new List<VistoriadorViewModel>();
            }
        }
    }
}
