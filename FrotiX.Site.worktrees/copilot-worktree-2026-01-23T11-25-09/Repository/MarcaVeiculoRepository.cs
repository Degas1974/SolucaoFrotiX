// ╔═══════════════════════════════════════════════════════════════════════════════════════════════════╗
// ║   🚗 MarcaVeiculoRepository.cs | Repository/ | 2026-01-20                                         ║
// ║   Marcas de veículo. ✅ Filtro Status. ⚠️ Código morto + quebra Unit of Work                     ║
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
    public class MarcaVeiculoRepository : Repository<MarcaVeiculo>, IMarcaVeiculoRepository
    {
        private new readonly FrotiXDbContext _db;

        public MarcaVeiculoRepository(FrotiXDbContext db)
            : base(db)
        {
            _db = db;
        }

        // [ETAPA] Dropdown - ✅ Filtro Status = true, OrderBy: DescricaoMarca
        public IEnumerable<SelectListItem> GetMarcaVeiculoListForDropDown()
        {
            return _db
                .MarcaVeiculo.Where(e => e.Status == true)
                .OrderBy(o => o.DescricaoMarca)
                .Select(i => new SelectListItem()
                {
                    Text = i.DescricaoMarca,
                    Value = i.MarcaId.ToString(),
                });
        }

        // [ETAPA] Update - ⚠️ Código morto + quebra Unit of Work
        public new void Update(MarcaVeiculo marcaVeiculo)
        {
            var objFromDb = _db.MarcaVeiculo.AsTracking().FirstOrDefault(s => s.MarcaId == marcaVeiculo.MarcaId);

            _db.Update(marcaVeiculo);
            _db.SaveChanges();
        }
    }
}
