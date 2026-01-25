// ╔═══════════════════════════════════════════════════════════════════════════════════════════════════╗
// ║   📄 RepactuacaoAtaRepository.cs | Repository/ | 2026-01-20                                       ║
// ║   Repactuação Ata. ⚠️ Código morto + quebra Unit of Work                                         ║
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
    public class RepactuacaoAtaRepository : Repository<RepactuacaoAta>, IRepactuacaoAtaRepository
    {
        private new readonly FrotiXDbContext _db;

        public RepactuacaoAtaRepository(FrotiXDbContext db) : base(db)
        {
            _db = db;
        }

        // [ETAPA] Dropdown - OrderBy: Descricao
        public IEnumerable<SelectListItem> GetRepactuacaoAtaListForDropDown()
        {
            return _db.RepactuacaoAta
                .OrderBy(o => o.Descricao)
                .Select(i => new SelectListItem()
                {
                    Text = i.Descricao,
                    Value = i.RepactuacaoAtaId.ToString()
                });
        }

        // [ETAPA] Update - ⚠️ Código morto + quebra Unit of Work
        public new void Update(RepactuacaoAta repactuacaoitemveiculoata)
        {
            var objFromDb = _db.RepactuacaoAta.AsTracking().FirstOrDefault(s => s.RepactuacaoAtaId == repactuacaoitemveiculoata.RepactuacaoAtaId);

            _db.Update(repactuacaoitemveiculoata);
            _db.SaveChanges();

        }


    }
}


