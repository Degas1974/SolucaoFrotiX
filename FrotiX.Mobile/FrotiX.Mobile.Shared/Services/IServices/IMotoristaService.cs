using FrotiX.Mobile.Shared.Models;

namespace FrotiX.Mobile.Shared.Services.IServices
{
    public interface IMotoristaService
    {
        Task<List<MotoristaViewModel>> ObterMotoristasAsync();
    }
}
