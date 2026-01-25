// ╔═══════════════════════════════════════════════════════════════════════════════════════════════════╗
// ║   🚗 VeiculoRepository.cs | Repository/ | 2026-01-20                                              ║
// ║   Veículos. ✅ 2 Dropdowns (simples + completo com View). ✅ Update SEM código morto!            ║
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
    public class VeiculoRepository : Repository<Veiculo>, IVeiculoRepository
    {
        private new readonly FrotiXDbContext _db;

        public VeiculoRepository(FrotiXDbContext db) : base(db)
        {
            _db = db;
        }

        // [ETAPA] Dropdown simples - OrderBy: Placa, Text: Placa
        public IEnumerable<SelectListItem> GetVeiculoListForDropDown()
        {
            return _db.Veiculo
            .OrderBy(o => o.Placa)
            .Select(i => new SelectListItem()
            {
                Text = i.Placa,
                Value = i.VeiculoId.ToString()
            });
        }

        // [ETAPA] Dropdown completo - ✅ Usa ViewVeiculos (com JOIN de dados relacionados)
        // ✅ Filtro Status = true, Text: VeiculoCompleto (formatado na View)
        public IEnumerable<SelectListItem> GetVeiculoCompletoListForDropDown()
        {
            return _db.ViewVeiculos
                .Where(v => v.Status == true)
                .OrderBy(v => v.Placa)
                .Select(v => new SelectListItem()
                {
                    Text = v.VeiculoCompleto,
                    Value = v.VeiculoId.ToString()
                });
        }


        // [ETAPA] Update - ✅ SEM código morto objFromDb (único repository assim!)
        // ⚠️ Ainda quebra Unit of Work (SaveChanges direto)
        public new void Update(Veiculo veiculo)
        {
            // Atualiza diretamente sem buscar novamente do banco
            // (evita conflito de tracking no EF Core)
            _db.Update(veiculo);
            _db.SaveChanges();
        }


    }
}
