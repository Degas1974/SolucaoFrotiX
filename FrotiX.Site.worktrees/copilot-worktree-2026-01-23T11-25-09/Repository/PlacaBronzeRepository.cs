// ╔═══════════════════════════════════════════════════════════════════════════════════════════════════╗
// ║   🪙 PlacaBronzeRepository.cs | Repository/ | 2026-01-20                                          ║
// ║   Placas Bronze. ✅ Filtro Status. ⚠️ Código morto + quebra Unit of Work                         ║
// ╚═══════════════════════════════════════════════════════════════════════════════════════════════════╝

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FrotiX.Data;
using FrotiX.Models;
using FrotiX.Repository.IRepository;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FrotiX.Repository
{
    public class PlacaBronzeRepository : Repository<PlacaBronze>, IPlacaBronzeRepository
    {
        private new readonly FrotiXDbContext _db;

        public PlacaBronzeRepository(FrotiXDbContext db)
            : base(db)
        {
            _db = db;
        }

        // [ETAPA] Dropdown - ✅ Filtro Status = true, OrderBy: DescricaoPlaca
        public IEnumerable<SelectListItem> GetPlacaBronzeListForDropDown()
        {
            return _db
                .PlacaBronze.Where(e => e.Status == true)
                .OrderBy(o => o.DescricaoPlaca)
                .Select(i => new SelectListItem()
                {
                    Text = i.DescricaoPlaca,
                    Value = i.PlacaBronzeId.ToString(),
                });
        }

        // [ETAPA] Update - ⚠️ Código morto + quebra Unit of Work
        public new void Update(PlacaBronze placaBronze)
        {
            var objFromDb = _db.PlacaBronze.AsTracking().FirstOrDefault(s =>
                s.PlacaBronzeId == placaBronze.PlacaBronzeId
            );

            _db.Update(placaBronze);
            _db.SaveChanges();
        }
    }
}
