// ╔═══════════════════════════════════════════════════════════════════════════════════════════════════╗
// ║   📋 LavagemRepository.cs | Repository/ | 2026-01-19 | Gerencia LAVAGENS de veículos             ║
// ║   ⚠️ PROBLEMA: Dropdown mostra apenas Data (deveria incluir Veículo/Placa)                       ║
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
    public class LavagemRepository : Repository<Lavagem>, ILavagemRepository
    {
        private new readonly FrotiXDbContext _db;

        public LavagemRepository(FrotiXDbContext db) : base(db)
        {
            _db = db;
        }

        // [ETAPA] ⚠️ PROBLEMA: Text mostra apenas Data (pouco descritivo)
        // TODO: JOIN com Veiculo para exibir "Placa - Data" ou "Modelo - Data"
        public IEnumerable<SelectListItem> GetLavagemListForDropDown()
        {
            return _db.Lavagem
            .OrderBy(o => o.Data)
            .Select(i => new SelectListItem()
            {
                Text = i.Data.ToString(),
                Value = i.LavagemId.ToString()
            }); ; ;
        }

        // [ETAPA] Update - ⚠️ Código morto + quebra Unit of Work
        public new void Update(Lavagem lavagem)
        {
            var objFromDb = _db.Lavagem.AsTracking().FirstOrDefault(s => s.LavagemId == lavagem.LavagemId);

            _db.Update(lavagem);
            _db.SaveChanges();

        }


    }
}


