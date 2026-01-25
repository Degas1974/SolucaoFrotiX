// ╔═══════════════════════════════════════════════════════════════════════════════════════════════════╗
// ║   🚖 ViagensEconomildoRepository.cs | Repository/ | 2026-01-20                                    ║
// ║   Viagens terceirizadas (Economildo). ⚠️ Text=Data (sem formato). Código morto + Unit of Work    ║
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
    public class ViagensEconomildoRepository : Repository<ViagensEconomildo>, IViagensEconomildoRepository
        {
        private new readonly FrotiXDbContext _db;

        public ViagensEconomildoRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

        // [ETAPA] Dropdown - Sem OrderBy! Text: Data (DateTime sem formato)
        public IEnumerable<SelectListItem> GetViagensEconomildoListForDropDown()
            {
            return _db.ViagensEconomildo
            .Select(i => new SelectListItem()
                {
                Text = i.Data.ToString(),
                Value = i.ViagemEconomildoId.ToString()
                }); ;
            }

        // [ETAPA] Update - ⚠️ Código morto + quebra Unit of Work
        public new void Update(ViagensEconomildo viagensEconomildo)
            {
            var objFromDb = _db.ViagensEconomildo.AsTracking().FirstOrDefault(s => s.ViagemEconomildoId == viagensEconomildo.ViagemEconomildoId);

            _db.Update(viagensEconomildo);
            _db.SaveChanges();

            }


        }
    }


