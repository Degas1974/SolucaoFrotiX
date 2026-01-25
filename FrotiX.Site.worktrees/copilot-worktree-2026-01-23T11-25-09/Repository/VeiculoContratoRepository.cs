// ╔═══════════════════════════════════════════════════════════════════════════════════════════════════╗
// ║   🚗 VeiculoContratoRepository.cs | Repository/ | 2026-01-20                                      ║
// ║   N:N Veiculo ↔ Contrato. ❌ BUG CRÍTICO: GetListForDropDown retorna VAZIO (código comentado!)   ║
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
    public class VeiculoContratoRepository : Repository<VeiculoContrato>, IVeiculoContratoRepository
    {
        private new readonly FrotiXDbContext _db;

        public VeiculoContratoRepository(FrotiXDbContext db) : base(db)
        {
            _db = db;
        }

        // [ETAPA] ❌ BUG CRÍTICO: SelectListItem vazio - Text/Value comentados!
        // TODO: JOIN com Veiculo para exibir Placa
        public IEnumerable<SelectListItem> GetVeiculoContratoListForDropDown()
        {
            return _db.VeiculoContrato.Select(i => new SelectListItem()
            {
                //Text = i.Placa,
                //Value = i.VeiculoId.ToString()
            }); ;
        }

        // [ETAPA] Update - ⚠️ Chave composta: VeiculoId + ContratoId
        // ⚠️ Código morto + quebra Unit of Work
        public new void Update(VeiculoContrato veiculoContrato)
        {
            var objFromDb = _db.VeiculoContrato.AsTracking().FirstOrDefault(s => (s.VeiculoId == veiculoContrato.VeiculoId) && (s.ContratoId == veiculoContrato.ContratoId));

            _db.Update(veiculoContrato);
            _db.SaveChanges();

        }


    }
}


