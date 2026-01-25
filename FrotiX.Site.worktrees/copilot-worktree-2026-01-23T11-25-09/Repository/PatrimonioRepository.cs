// ╔═══════════════════════════════════════════════════════════════════════════════════════════════════╗
// ║   📋 PatrimonioRepository.cs | Repository/ | 2026-01-20                                           ║
// ║   Patrimônio (bens). ⚠️ OrderBy NumeroSerie mas Text=NPR. Código morto + Unit of Work            ║
// ╚═══════════════════════════════════════════════════════════════════════════════════════════════════╝

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FrotiX.Data;
using FrotiX.Models;
using FrotiX.Models.Cadastros;
using FrotiX.Repository.IRepository;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FrotiX.Repository
{
    public class PatrimonioRepository : Repository<Patrimonio>, IPatrimonioRepository
    {
        private new readonly FrotiXDbContext _db;

        public PatrimonioRepository(FrotiXDbContext db) : base(db)
        {
            _db = db;
        }

        // [ETAPA] Dropdown - ⚠️ OrderBy: NumeroSerie, mas Text: NPR (inconsistente)
        public IEnumerable<SelectListItem> GetPatrimonioListForDropDown()
        {
            return _db.Patrimonio
            .OrderBy(o => o.NumeroSerie)
            .Select(i => new SelectListItem()
            {
                Text = i.NPR,
                Value = i.PatrimonioId.ToString()
            });
        }

        // [ETAPA] Update - ⚠️ Código morto + quebra Unit of Work
        public new void Update(Patrimonio patrimonio)
        {
            var objFromDb = _db.Patrimonio.AsTracking().FirstOrDefault(s => s.PatrimonioId == patrimonio.PatrimonioId);

            _db.Update(patrimonio);
            _db.SaveChanges();

        }


    }
}


