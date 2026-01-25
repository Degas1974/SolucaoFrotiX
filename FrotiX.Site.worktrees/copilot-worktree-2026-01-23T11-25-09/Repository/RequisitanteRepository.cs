// ╔═══════════════════════════════════════════════════════════════════════════════════════════════════╗
// ║   📋 RequisitanteRepository.cs | Repository/ | 2026-01-20                                         ║
// ║   Requisitantes. ✅ Filtro Status. ✅ Text: Nome + Ponto. ⚠️ Código morto + Unit of Work         ║
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
    public class RequisitanteRepository : Repository<Requisitante>, IRequisitanteRepository
    {
        private new readonly FrotiXDbContext _db;

        public RequisitanteRepository(FrotiXDbContext db)
            : base(db)
        {
            _db = db;
        }

        // [ETAPA] Dropdown - ✅ Filtro Status = true, OrderBy: Nome
        // ✅ Text: Nome + Ponto (informativo)
        public IEnumerable<SelectListItem> GetRequisitanteListForDropDown()
        {
            return _db
                .Requisitante.Where(s => s.Status == true)
                .OrderBy(o => o.Nome)
                .Select(i => new SelectListItem()
                {
                    Text = i.Nome + "(" + i.Ponto + ")",
                    Value = i.RequisitanteId.ToString(),
                });
        }

        // [ETAPA] Update - ⚠️ Código morto + quebra Unit of Work
        public new void Update(Requisitante requisitante)
        {
            var objFromDb = _db.Requisitante.AsTracking().FirstOrDefault(s =>
                s.RequisitanteId == requisitante.RequisitanteId
            );

            _db.Update(requisitante);
            _db.SaveChanges();
        }
    }
}
