// ╔═══════════════════════════════════════════════════════════════════════════════════════════════════╗
// ║   👨‍💼 OperadorRepository.cs | Repository/ | 2026-01-20                                             ║
// ║   Operadores. ⚠️ Código morto + quebra Unit of Work                                              ║
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
    public class OperadorRepository : Repository<Operador>, IOperadorRepository
    {
        private new readonly FrotiXDbContext _db;

        public OperadorRepository(FrotiXDbContext db) : base(db)
        {
            _db = db;
        }

        // [ETAPA] Dropdown - OrderBy: Nome
        public IEnumerable<SelectListItem> GetOperadorListForDropDown()
        {
            return _db.Operador
            .OrderBy(o => o.Nome)
            .Select(i => new SelectListItem()
            {
                Text = i.Nome,
                Value = i.OperadorId.ToString()
            }); ;
        }

        // [ETAPA] Update - ⚠️ Código morto + quebra Unit of Work
        public new void Update(Operador operador)
        {
            var objFromDb = _db.Operador.AsTracking().FirstOrDefault(s => s.OperadorId == operador.OperadorId);

            _db.Update(operador);
            _db.SaveChanges();

        }


    }
}


