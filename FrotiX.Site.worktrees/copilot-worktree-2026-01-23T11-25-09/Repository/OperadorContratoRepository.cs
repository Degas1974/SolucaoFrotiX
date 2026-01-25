// ╔═══════════════════════════════════════════════════════════════════════════════════════════════════╗
// ║   🔗 OperadorContratoRepository.cs | Repository/ | 2026-01-20                                     ║
// ║   N:N Operador ↔ Contrato. ❌ BUG CRÍTICO: GetListForDropDown retorna VAZIO (código comentado!)  ║
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
    public class OperadorContratoRepository : Repository<OperadorContrato>, IOperadorContratoRepository
    {
        private new readonly FrotiXDbContext _db;

        public OperadorContratoRepository(FrotiXDbContext db) : base(db)
        {
            _db = db;
        }

        // [ETAPA] ❌ BUG CRÍTICO: SelectListItem vazio - Text/Value comentados!
        // TODO: Implementar Text (ex: JOIN com Operador/Contrato para exibir nomes)
        public IEnumerable<SelectListItem> GetOperadorContratoListForDropDown()
        {
            return _db.OperadorContrato.Select(i => new SelectListItem()
            {
                //Text = i.Placa,
                //Value = i.VeiculoId.ToString()
            }); ;
        }

        // [ETAPA] Update - ⚠️ Chave composta: OperadorId + ContratoId
        // ⚠️ Código morto + quebra Unit of Work
        public new void Update(OperadorContrato operadorContrato)
        {
            var objFromDb = _db.OperadorContrato.AsTracking().FirstOrDefault(s => (s.OperadorId == operadorContrato.OperadorId) && (s.ContratoId == operadorContrato.ContratoId));

            _db.Update(operadorContrato);
            _db.SaveChanges();

        }


    }
}


