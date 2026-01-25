using FrotiX.Mobile.Shared.Models;

namespace FrotiX.Mobile.Shared.Data.Repository.IRepository
{
    public interface IUnitOfWork : IDisposable
    {
        IViagensEconomildoRepository ViagensEconomildo { get; }
        IRepository<OcorrenciaViagem> OcorrenciasViagem { get; }

        void Save();
        Task SaveAsync();
    }
}
