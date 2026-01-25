// ╔═══════════════════════════════════════════════════════════════════════════════════════════════════╗
// ║   🚗 RepactuacaoVeiculoRepository.cs | Repository/ | 2026-01-20                                   ║
// ║   Repactuação Veículo. ✅ Update CORRETO (atualiza campos individuais)! ⚠️ Quebra Unit of Work   ║
// ╚═══════════════════════════════════════════════════════════════════════════════════════════════════╝

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using FrotiX.Data;
using FrotiX.Models;
using FrotiX.Repository.IRepository;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FrotiX.Repository
{
    public class RepactuacaoVeiculoRepository : Repository<RepactuacaoVeiculo>, IRepactuacaoVeiculoRepository
    {
        private new readonly FrotiXDbContext _db;

        public RepactuacaoVeiculoRepository(FrotiXDbContext db) : base(db)
        {
            _db = db;
        }

        // [ETAPA] Dropdown - OrderBy: RepactuacaoVeiculoId
        // ⚠️ Text: Valor (decimal sem formato)
        public IEnumerable<SelectListItem> GetRepactuacaoVeiculoListForDropDown()
        {
            return _db.RepactuacaoVeiculo
                .OrderBy(o => o.RepactuacaoVeiculoId)
                .Select(i => new SelectListItem()
                {
                    Text = i.Valor.ToString(),
                    Value = i.RepactuacaoVeiculoId.ToString()
                });
        }

        // [ETAPA] Update - ✅ CORRETO: Atualiza campos individuais de objFromDb!
        // ⚠️ Ainda quebra Unit of Work (SaveChanges direto)
        public new void Update(RepactuacaoVeiculo repactuacaoVeiculo)
        {
            var objFromDb = _db.RepactuacaoVeiculo.AsTracking().FirstOrDefault(s =>
                s.RepactuacaoVeiculoId == repactuacaoVeiculo.RepactuacaoVeiculoId
            );

            if (objFromDb != null)
            {
                objFromDb.Valor = repactuacaoVeiculo.Valor;
                objFromDb.Observacao = repactuacaoVeiculo.Observacao;
                objFromDb.VeiculoId = repactuacaoVeiculo.VeiculoId;
                objFromDb.RepactuacaoContratoId = repactuacaoVeiculo.RepactuacaoContratoId;
            }

            _db.SaveChanges();
        }
    }
}
