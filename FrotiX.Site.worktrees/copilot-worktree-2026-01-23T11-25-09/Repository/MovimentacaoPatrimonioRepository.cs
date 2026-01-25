// ╔═══════════════════════════════════════════════════════════════════════════════════════════════════╗
// ║   📦 MovimentacaoPatrimonioRepository.cs | Repository/ | 2026-01-20                               ║
// ║   Movimentações de patrimônio. ⚠️ OrderBy PatrimonioId (não DataMovimentacao!)                   ║
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
    public class MovimentacaoPatrimonioRepository : Repository<MovimentacaoPatrimonio>, IMovimentacaoPatrimonioRepository
    {
        private new readonly FrotiXDbContext _db;

        public MovimentacaoPatrimonioRepository(FrotiXDbContext db) : base(db)
        {
            _db = db;
        }

        // [ETAPA] Dropdown - ⚠️ OrderBy PatrimonioId (deveria ser DataMovimentacao?)
        // Text: DataMovimentacao (sem formatação, pode exibir datetime completo)
        public IEnumerable<SelectListItem> GetMovimentacaoPatrimonioListForDropDown()
        {
            return _db.MovimentacaoPatrimonio
            .OrderBy(o => o.PatrimonioId)
            .Select(i => new SelectListItem()
            {
                Text = i.DataMovimentacao.ToString(),
                Value = i.MovimentacaoPatrimonioId.ToString()
            }); ;
        }

        // [ETAPA] Update - ⚠️ Código morto + quebra Unit of Work
        public new void Update(MovimentacaoPatrimonio movimentacaoPatrimonio)
        {
            var objFromDb = _db.MovimentacaoPatrimonio.AsTracking().FirstOrDefault(s => s.MovimentacaoPatrimonioId == movimentacaoPatrimonio.MovimentacaoPatrimonioId);

            _db.Update(movimentacaoPatrimonio);
            _db.SaveChanges();

        }


    }
}


