// ╔═══════════════════════════════════════════════════════════════════════════════════════════════════╗
// ║   📋 LavadorRepository.cs | Repository/ | 2026-01-19 | Gerencia cadastro de LAVADORES            ║
// ║   ✅ GetListForDropDown correto (mostra Nome) | ⚠️ Update quebra Unit of Work                    ║
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
    public class LavadorRepository : Repository<Lavador>, ILavadorRepository
    {
        private new readonly FrotiXDbContext _db;

        public LavadorRepository(FrotiXDbContext db) : base(db)
        {
            _db = db;
        }

        // [ETAPA] Busca lavadores para dropdown - ✅ Correto (Text = Nome)
        public IEnumerable<SelectListItem> GetLavadorListForDropDown()
        {
            return _db.Lavador
            .OrderBy(o => o.Nome)
            .Select(i => new SelectListItem()
            {
                Text = i.Nome,
                Value = i.LavadorId.ToString()
            }); ;
        }

        // [ETAPA] Update - ⚠️ Código morto + quebra Unit of Work
        public new void Update(Lavador lavador)
        {
            var objFromDb = _db.Lavador.AsTracking().FirstOrDefault(s => s.LavadorId == lavador.LavadorId);

            _db.Update(lavador);
            _db.SaveChanges();

        }


    }
}


