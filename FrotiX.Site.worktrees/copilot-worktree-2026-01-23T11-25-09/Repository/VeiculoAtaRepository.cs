// ╔═══════════════════════════════════════════════════════════════════════════════════════════════════╗
// ║   🚙 VeiculoAtaRepository.cs | Repository/ | 2026-01-20                                           ║
// ║   N:N Veiculo ↔ Ata. ❌ BUG CRÍTICO: GetListForDropDown retorna VAZIO (código comentado!)        ║
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
    public class VeiculoAtaRepository : Repository<VeiculoAta>, IVeiculoAtaRepository
    {
        private new readonly FrotiXDbContext _db;

        public VeiculoAtaRepository(FrotiXDbContext db) : base(db)
        {
            _db = db;
        }

        // [ETAPA] ❌ BUG CRÍTICO: SelectListItem vazio - Text/Value comentados!
        // TODO: JOIN com Veiculo para exibir Placa
        public IEnumerable<SelectListItem> GetVeiculoAtaListForDropDown()
        {
            return _db.VeiculoAta.Select(i => new SelectListItem()
            {
                //Text = i.Placa,
                //Value = i.VeiculoId.ToString()
            }); ;
        }

        // [ETAPA] Update - ⚠️ Chave composta: VeiculoId + AtaId
        // ⚠️ Código morto + quebra Unit of Work
        public new void Update(VeiculoAta veiculoAta)
        {
            var objFromDb = _db.VeiculoAta.AsTracking().FirstOrDefault(s => (s.VeiculoId == veiculoAta.VeiculoId) && (s.AtaId == veiculoAta.AtaId));

            _db.Update(veiculoAta);
            _db.SaveChanges();

        }


    }
}


