// ╔═══════════════════════════════════════════════════════════════════════════════════════════════════╗
// ║   🚙 ModeloVeiculoRepository.cs | Repository/ | 2026-01-20                                        ║
// ║   Modelos de veículo. ⚠️ Código morto + quebra Unit of Work                                      ║
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
    public class ModeloVeiculoRepository : Repository<ModeloVeiculo>, IModeloVeiculoRepository
    {
        private new readonly FrotiXDbContext _db;

        public ModeloVeiculoRepository(FrotiXDbContext db) : base(db)
        {
            _db = db;
        }

        // [ETAPA] Dropdown - OrderBy: DescricaoModelo
        public IEnumerable<SelectListItem> GetModeloVeiculoListForDropDown()
        {
            return _db.ModeloVeiculo
            .OrderBy(o => o.DescricaoModelo)
            .Select(i => new SelectListItem()
            {
                Text = i.DescricaoModelo,
                Value = i.ModeloId.ToString()
            }); ;
        }

        // [ETAPA] Update - ⚠️ Código morto + quebra Unit of Work
        public new void Update(ModeloVeiculo modeloVeiculo)
        {
            var objFromDb = _db.ModeloVeiculo.AsTracking().FirstOrDefault(s => s.ModeloId == modeloVeiculo.ModeloId);

            _db.Update(modeloVeiculo);
            _db.SaveChanges();

        }


    }
}


