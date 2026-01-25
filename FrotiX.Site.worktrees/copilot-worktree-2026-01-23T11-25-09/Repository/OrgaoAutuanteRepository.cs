// ╔═══════════════════════════════════════════════════════════════════════════════════════════════════╗
// ║   🏛️ OrgaoAutuanteRepository.cs | Repository/ | 2026-01-20                                        ║
// ║   Órgãos autuantes (multas). ✅ Text: Nome + Sigla. ⚠️ Código morto + Unit of Work               ║
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
    public class OrgaoAutuanteRepository : Repository<OrgaoAutuante>, IOrgaoAutuanteRepository
    {
        private new readonly FrotiXDbContext _db;

        public OrgaoAutuanteRepository(FrotiXDbContext db) : base(db)
        {
            _db = db;
        }

        // [ETAPA] Dropdown - OrderBy: Nome, ✅ Text: Nome + Sigla
        public IEnumerable<SelectListItem> GetOrgaoAutuanteListForDropDown()
        {
            return _db.OrgaoAutuante
                .OrderBy(o => o.Nome)
                .Select(i => new SelectListItem()
                {
                    Text = i.Nome + " (" + i.Sigla + ")",
                    Value = i.OrgaoAutuanteId.ToString()
                });
        }

        // [ETAPA] Update - ⚠️ Código morto + quebra Unit of Work
        public new void Update(OrgaoAutuante orgaoautuante)
        {
            var objFromDb = _db.OrgaoAutuante.AsTracking().FirstOrDefault(s => s.OrgaoAutuanteId == orgaoautuante.OrgaoAutuanteId);

            _db.Update(orgaoautuante);
            _db.SaveChanges();

        }


    }
}


