using FrotiX.Mobile.Shared.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FrotiX.Mobile.Shared.Data.Repository.IRepository
{
    public interface IViagensEconomildoRepository : IRepository<ViagensEconomildo>
    {
        Task<List<ViagensEconomildo>> GetViagensByDataAsync(DateTime data);
        Task<List<ViagensEconomildo>> GetViagensByVeiculoAsync(Guid veiculoId);
        Task<List<ViagensEconomildo>> GetViagensByMotoristaAsync(Guid motoristaId);
        Task<List<ViagensEconomildo>> GetViagensNaoTransmitidasAsync();
        Task<ViagensEconomildo?> GetLastViagemByVeiculoAsync(Guid veiculoId);
    }
}
