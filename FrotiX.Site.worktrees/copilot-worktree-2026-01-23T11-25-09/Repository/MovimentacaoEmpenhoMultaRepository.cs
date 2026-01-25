// ╔═══════════════════════════════════════════════════════════════════════════════════════════════════╗
// ║   💸 MovimentacaoEmpenhoMultaRepository.cs | Repository/ | 2026-01-20                             ║
// ║   Movimentações de empenho (Multas). ⚠️ Text concatena DateTime+Decimal                          ║
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
    public class MovimentacaoEmpenhoMultaRepository : Repository<MovimentacaoEmpenhoMulta>, IMovimentacaoEmpenhoMultaRepository
    {
        private new readonly FrotiXDbContext _db;

        public MovimentacaoEmpenhoMultaRepository(FrotiXDbContext db) : base(db)
        {
            _db = db;
        }

        // [ETAPA] Dropdown - OrderBy: DataMovimentacao
        // ⚠️ Text concatena DateTime + Decimal (pode falhar formatação)
        public IEnumerable<SelectListItem> GetMovimentacaoEmpenhoMultaListForDropDown()
        {
            return _db.MovimentacaoEmpenhoMulta
            .OrderBy(o => o.DataMovimentacao)
            .Select(i => new SelectListItem()
            {
                Text = i.DataMovimentacao + "(" + i.Valor + ")",
                Value = i.MovimentacaoId.ToString()
            });
        }

        // [ETAPA] Update - ⚠️ Código morto + quebra Unit of Work
        public new void Update(MovimentacaoEmpenhoMulta movimentacaoempenhomulta)
        {
            var objFromDb = _db.MovimentacaoEmpenhoMulta.AsTracking().FirstOrDefault(s => s.MovimentacaoId == movimentacaoempenhomulta.MovimentacaoId);

            _db.Update(movimentacaoempenhomulta);
            _db.SaveChanges();

        }
    }
}


