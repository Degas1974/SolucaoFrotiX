// ╔═══════════════════════════════════════════════════════════════════════════════════════════════════╗
// ║   🚨 MultaRepository.cs | Repository/ | 2026-01-20                                                ║
// ║   Multas de trânsito. ⚠️ Código morto + quebra Unit of Work                                      ║
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
    public class MultaRepository : Repository<Multa>, IMultaRepository
    {
        private new readonly FrotiXDbContext _db;

        public MultaRepository(FrotiXDbContext db) : base(db)
        {
            _db = db;
        }

        // [ETAPA] Dropdown - OrderBy: NumInfracao, Text: NumInfracao
        public IEnumerable<SelectListItem> GetMultaListForDropDown()
        {
            return _db.Multa
                .OrderBy(o => o.NumInfracao)
                .Select(i => new SelectListItem()
                {
                    Text = i.NumInfracao,
                    Value = i.MultaId.ToString()
                });
        }

        // [ETAPA] Update - ⚠️ Código morto + quebra Unit of Work
        public new void Update(Multa multa)
        {
            var objFromDb = _db.Multa.AsTracking().FirstOrDefault(s => s.MultaId == multa.MultaId);

            _db.Update(multa);
            _db.SaveChanges();

        }


    }
}


