using FrotiX.Mobile.Shared.Models;

namespace FrotiX.Mobile.Shared.Services.IServices
{
    public interface IVeiculoService
    {
        Task<List<VeiculoViewModel>> ObterVeiculoCompletoDropdownAsync();
        Task<VeiculoViewModel?> GetVeiculoByIdAsync(Guid id);
    }
}
