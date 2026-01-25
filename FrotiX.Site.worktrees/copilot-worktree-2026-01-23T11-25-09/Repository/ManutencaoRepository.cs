// ╔═══════════════════════════════════════════════════════════════════════════════════════════════════╗
// ║   🔧 ManutencaoRepository.cs | Repository/ | 2026-01-20                                           ║
// ║   Manutenções (OS). Dropdown: ResumoOS. ⚠️ Código morto + quebra Unit of Work                    ║
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
    public class ManutencaoRepository : Repository<Manutencao>, IManutencaoRepository
    {
        private new readonly FrotiXDbContext _db;

        public ManutencaoRepository(FrotiXDbContext db) : base(db)
        {
            _db = db;
        }

        // [ETAPA] Dropdown para selects - OrderBy: ResumoOS, Text: ResumoOS
        public IEnumerable<SelectListItem> GetManutencaoListForDropDown()
        {
            return _db.Manutencao
            .OrderBy(o => o.ResumoOS)
            .Select(i => new SelectListItem()
            {
                Text = i.ResumoOS,
                Value = i.ManutencaoId.ToString()
            }); ;
        }

        // [ETAPA] Update - ⚠️ Código morto (objFromDb) + quebra Unit of Work (SaveChanges)
        public new void Update(Manutencao manutencao)
        {
            var objFromDb = _db.Manutencao.AsTracking().FirstOrDefault(s => s.ManutencaoId == manutencao.ManutencaoId);

            _db.Update(manutencao);
            _db.SaveChanges();

        }


    }
}


