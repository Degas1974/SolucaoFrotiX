// ╔═══════════════════════════════════════════════════════════════════════════════════════════════════╗
// ║   💼 RepactuacaoServicosRepository.cs | Repository/ | 2026-01-20                                  ║
// ║   Repactuação Serviços. ⚠️ Text=Valor (decimal sem formato). Value=Wrong ID!                     ║
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
    public class RepactuacaoServicosRepository : Repository<RepactuacaoServicos>, IRepactuacaoServicosRepository
    {
        private new readonly FrotiXDbContext _db;

        public RepactuacaoServicosRepository(FrotiXDbContext db) : base(db)
        {
            _db = db;
        }

        // [ETAPA] Dropdown - ⚠️ Sem OrderBy! Text: Valor (decimal)
        // ❌ BUG: Value usa RepactuacaoContratoId (deveria ser RepactuacaoServicoId!)
        public IEnumerable<SelectListItem> GetRepactuacaoServicosListForDropDown()
        {
            return _db.RepactuacaoServicos
                .Select(i => new SelectListItem()
                {
                    Text = i.Valor.ToString(),
                    Value = i.RepactuacaoContratoId.ToString()
                });
        }

        // [ETAPA] Update - ⚠️ Código morto + quebra Unit of Work
        public new void Update(RepactuacaoServicos RepactuacaoServicos)
        {
            var objFromDb = _db.RepactuacaoServicos.AsTracking().FirstOrDefault(s => s.RepactuacaoServicoId == RepactuacaoServicos.RepactuacaoServicoId);

            _db.Update(RepactuacaoServicos);
            _db.SaveChanges();

        }


    }
}


