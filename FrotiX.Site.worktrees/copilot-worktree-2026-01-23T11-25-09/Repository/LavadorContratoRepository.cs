// ╔═══════════════════════════════════════════════════════════════════════════════════════════════════╗
// ║   📋 LavadorContratoRepository.cs | Repository/ | 2026-01-19                                      ║
// ║   Tabela N:N → Lavador ↔ Contrato (associação de lavadores a contratos)                          ║
// ║   ❌ BUG CRÍTICO: GetListForDropDown() tem código comentado (não funciona!)                       ║
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
    public class LavadorContratoRepository : Repository<LavadorContrato>, ILavadorContratoRepository
    {
        private new readonly FrotiXDbContext _db;

        public LavadorContratoRepository(FrotiXDbContext db) : base(db)
        {
            _db = db;
        }

        // [ETAPA] ❌ BUG: Método não funciona - código comentado retorna SelectListItem vazio!
        // TODO: Implementar corretamente (ex: JOIN com Lavador para mostrar Nome)
        public IEnumerable<SelectListItem> GetLavadorContratoListForDropDown()
        {
            return _db.LavadorContrato.Select(i => new SelectListItem()
            {
                //Text = i.Placa,
                //Value = i.VeiculoId.ToString()
            }); ;
        }

        // [ETAPA] Update com chave composta (LavadorId + ContratoId)
        // ⚠️ Código morto + quebra Unit of Work
        public new void Update(LavadorContrato lavadorContrato)
        {
            var objFromDb = _db.LavadorContrato.AsTracking().FirstOrDefault(s => (s.LavadorId == lavadorContrato.LavadorId) && (s.ContratoId == lavadorContrato.ContratoId));

            _db.Update(lavadorContrato);
            _db.SaveChanges();

        }


    }
}


