// ╔═══════════════════════════════════════════════════════════════════════════════════════════════════╗
// ║   🧾 NotaFiscalRepository.cs | Repository/ | 2026-01-20                                           ║
// ║   Notas fiscais. ⚠️ Código morto + quebra Unit of Work                                           ║
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
    public class NotaFiscalRepository : Repository<NotaFiscal>, INotaFiscalRepository
    {
        private new readonly FrotiXDbContext _db;

        public NotaFiscalRepository(FrotiXDbContext db) : base(db)
        {
            _db = db;
        }

        // [ETAPA] Dropdown - OrderBy: NumeroNF
        public IEnumerable<SelectListItem> GetNotaFiscalListForDropDown()
        {
            return _db.NotaFiscal
            .OrderBy(o => o.NumeroNF)
            .Select(i => new SelectListItem()
            {
                Text = i.NumeroNF.ToString(),
                Value = i.NotaFiscalId.ToString()
            }); ;
        }

        // [ETAPA] Update - ⚠️ Código morto + quebra Unit of Work
        public new void Update(NotaFiscal notaFiscal)
        {
            var objFromDb = _db.NotaFiscal.AsTracking().FirstOrDefault(s => s.NotaFiscalId == notaFiscal.NotaFiscalId);

            _db.Update(notaFiscal);
            _db.SaveChanges();

        }
    }
}


