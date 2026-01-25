// ╔═══════════════════════════════════════════════════════════════════════════════════════════════════╗
// ║   🏛️ SetorPatrimonialRepository.cs | Repository/ | 2026-01-20                                     ║
// ║   Setor Patrimonial. ⚠️ Código morto + quebra Unit of Work                                       ║
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
    public class SetorPatrimonialRepository : Repository<SetorPatrimonial>, ISetorPatrimonialRepository
    {
        private new readonly FrotiXDbContext _db;

        public SetorPatrimonialRepository(FrotiXDbContext db) : base(db)
        {
            _db = db;
        }

        // [ETAPA] Dropdown - OrderBy: NomeSetor
        public IEnumerable<SelectListItem> GetSetorListForDropDown()
        {
            return _db.SetorPatrimonial
            .OrderBy(o => o.NomeSetor)
            .Select(i => new SelectListItem()
            {
                Text = i.NomeSetor,
                Value = i.SetorId.ToString()
            }); ;
        }

        // [ETAPA] Update - ⚠️ Código morto + quebra Unit of Work
        public new void Update(SetorPatrimonial setor)
        {
            var objFromDb = _db.SetorPatrimonial.AsTracking().FirstOrDefault(s => s.SetorId == setor.SetorId);

            _db.Update(setor);
            _db.SaveChanges();

        }


    }
}


