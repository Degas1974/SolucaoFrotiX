// ╔═══════════════════════════════════════════════════════════════════════════════════════════════════╗
// ║   📜 TipoMultaRepository.cs | Repository/ | 2026-01-20                                            ║
// ║   Tipo de Multa. ✅ Text: Artigo + Descricao. ⚠️ Código morto + Unit of Work                     ║
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
    public class TipoMultaRepository : Repository<TipoMulta>, ITipoMultaRepository
    {
        private new readonly FrotiXDbContext _db;

        public TipoMultaRepository(FrotiXDbContext db) : base(db)
        {
            _db = db;
        }

        // [ETAPA] Dropdown - OrderBy: Artigo, ✅ Text: Artigo + Descricao (completo)
        public IEnumerable<SelectListItem> GetTipoMultaListForDropDown()
        {
            return _db.TipoMulta
                .OrderBy(o => o.Artigo)
                .Select(i => new SelectListItem()
                {
                    Text = i.Artigo + " - " + i.Descricao,
                    Value = i.TipoMultaId.ToString()
                });
        }

        // [ETAPA] Update - ⚠️ Código morto + quebra Unit of Work
        public new void Update(TipoMulta tipomulta)
        {
            var objFromDb = _db.TipoMulta.AsTracking().FirstOrDefault(s => s.TipoMultaId == tipomulta.TipoMultaId);

            _db.Update(tipomulta);
            _db.SaveChanges();

        }


    }
}


