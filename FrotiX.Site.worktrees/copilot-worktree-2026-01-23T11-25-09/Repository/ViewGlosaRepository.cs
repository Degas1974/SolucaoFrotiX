// ═══════════════════════════════════════════════════════════════════════════════════════════════════╗
// ║   📊 ViewGlosaRepository.cs | VIEW (Read-Only) | 2026-01-20                                     ║
// ╚═══════════════════════════════════════════════════════════════════════════════════════════════════╝

using Microsoft.EntityFrameworkCore;
using FrotiX.Data;
using FrotiX.Models; // ViewGlosaModel
using FrotiX.Repository.IRepository;

namespace FrotiX.Repository
    {
    /// <summary>
    /// Implementação do repositório para ViewGlosaModel usando EF Core.
    /// </summary>
    public class ViewGlosaRepository : Repository<ViewGlosa>, IViewGlosaRepository
        {
        private new readonly FrotiXDbContext _db;

        public ViewGlosaRepository(FrotiXDbContext db)
            : base(db)
            {
            _db = db;
            }
        }
    }


