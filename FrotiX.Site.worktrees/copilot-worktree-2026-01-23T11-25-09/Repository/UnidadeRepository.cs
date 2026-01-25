// ╔═══════════════════════════════════════════════════════════════════════════════════════════════════╗
// ║   🏢 UnidadeRepository.cs | Repository/ | 2026-01-20                                              ║
// ║   Unidades. ✅ Filtro Status. ✅ Text: Sigla + Descricao. ⚠️ Código morto + Unit of Work         ║
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
    public class UnidadeRepository : Repository<Unidade>, IUnidadeRepository
    {
        private new readonly FrotiXDbContext _db;

        public UnidadeRepository(FrotiXDbContext db)
            : base(db)
        {
            _db = db;
        }

        // [ETAPA] Dropdown - ✅ Filtro Status = true
        // ✅ OrderBy: Sigla + Descricao, Text: Sigla + Descricao (completo)
        public IEnumerable<SelectListItem> GetUnidadeListForDropDown()
        {
            return _db
                .Unidade.Where(e => e.Status == true)
                .OrderBy(o => o.Sigla + " - " + o.Descricao)
                .Select(i => new SelectListItem()
                {
                    Text = i.Sigla + " - " + i.Descricao,
                    Value = i.UnidadeId.ToString(),
                });
        }

        // [ETAPA] Update - ⚠️ Código morto + quebra Unit of Work
        public new void Update(Unidade unidade)
        {
            var objFromDb = _db.Unidade.AsTracking().FirstOrDefault(s => s.UnidadeId == unidade.UnidadeId);

            _db.Update(unidade);
            _db.SaveChanges();
        }
    }
}
