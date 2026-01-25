using System.Collections.Generic;
using System.Threading.Tasks;
using FrotiX.Mobile.Shared.Models;

namespace FrotiX.Mobile.Shared.Services.IServices
{
    /// <summary>
    /// Interface para o serviço de Vistoriadores
    /// </summary>
    public interface IVistoriadorService
    {
        Task<List<VistoriadorViewModel>> GetVistoriadoresAsync();
    }
}
