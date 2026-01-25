// ╔═══════════════════════════════════════════════════════════════════════════════════════════════════╗
// ║   🏛️ RecursoRepository.cs | Repository/ | 2026-01-20                                              ║
// ║   Recursos (multas). ⚠️ Código morto + quebra Unit of Work                                       ║
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
    public class RecursoRepository : Repository<Recurso>, IRecursoRepository
    {
        private new readonly FrotiXDbContext _db;

        public RecursoRepository(FrotiXDbContext db) : base(db)
        {
            _db = db;
        }

        // [ETAPA] Dropdown - OrderBy: Nome
        public IEnumerable<SelectListItem> GetRecursoListForDropDown()
        {
            return _db.Recurso
            .OrderBy(o => o.Nome)
            .Select(i => new SelectListItem()
            {
                Text = i.Nome,
                Value = i.RecursoId.ToString()
            }); ;
        }

        // [ETAPA] Update - ⚠️ Código morto + quebra Unit of Work
        public new void Update(Recurso recurso)
        {
            var objFromDb = _db.Recurso.AsTracking().FirstOrDefault(s => s.RecursoId == recurso.RecursoId);

            _db.Update(recurso);
            _db.SaveChanges();

        }


    }
}


