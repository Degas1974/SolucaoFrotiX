using FrotiX.Mobile.Shared.Data.Repository.IRepository;
using FrotiX.Mobile.Shared.Models;
using FrotiX.Mobile.Shared.Services.IServices;

namespace FrotiX.Mobile.Shared.Data.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly FrotiXDbContext _db;
        private readonly IAlertaService _alerta;

        public IViagensEconomildoRepository ViagensEconomildo { get; private set; }
        public IRepository<OcorrenciaViagem> OcorrenciasViagem { get; private set; }

        public UnitOfWork(
            FrotiXDbContext db,
            IAlertaService alerta,
            IViagensEconomildoRepository viagensEconomildo
        )
        {
            _db = db;
            _alerta = alerta;
            ViagensEconomildo = viagensEconomildo;
            OcorrenciasViagem = new Repository<OcorrenciaViagem>(db, alerta);
        }

        public void Save() => _db.SaveChanges();

        public Task SaveAsync() => _db.SaveChangesAsync();

        public void Dispose() => _db.Dispose();
    }
}
