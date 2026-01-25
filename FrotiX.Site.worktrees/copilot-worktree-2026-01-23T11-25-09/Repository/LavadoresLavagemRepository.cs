// ╔═══════════════════════════════════════════════════════════════════════════════════════════════════╗
// ║   📋 LavadoresLavagemRepository.cs | Repository/ | 2026-01-19                                     ║
// ║   Tabela N:N → Lavador ↔ Lavagem (múltiplos lavadores por lavagem)                               ║
// ║   ⚠️ PROBLEMA: GetListForDropDown mostra GUID de Lavador ao invés de Nome                        ║
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
    public class LavadoresLavagemRepository : Repository<LavadoresLavagem>, ILavadoresLavagemRepository
    {
        private new readonly FrotiXDbContext _db;

        public LavadoresLavagemRepository(FrotiXDbContext db) : base(db)
        {
            _db = db;
        }

        // [ETAPA] ⚠️ PROBLEMA DE USABILIDADE: Text mostra LavadorId (GUID) ao invés de Nome do lavador
        // TODO: JOIN com Lavador para exibir ".Text = i.Lavador.Nome"
        public IEnumerable<SelectListItem> GetLavadoresLavagemListForDropDown()
        {
            return _db.LavadoresLavagem
            .OrderBy(o => o.LavagemId)
            .Select(i => new SelectListItem()
            {
                Text = i.LavadorId.ToString(),
                Value = i.LavagemId.ToString()
            }); ; ;
        }

        // [ETAPA] Update - ⚠️ Código morto + quebra Unit of Work
        public new void Update(LavadoresLavagem lavadoresLavagem)
        {
            var objFromDb = _db.LavadoresLavagem.AsTracking().FirstOrDefault(s => s.LavagemId == lavadoresLavagem.LavagemId);

            _db.Update(lavadoresLavagem);
            _db.SaveChanges();

        }


    }
}


