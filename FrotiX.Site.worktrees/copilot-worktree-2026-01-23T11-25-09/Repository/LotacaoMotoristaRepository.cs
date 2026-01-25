// ╔═══════════════════════════════════════════════════════════════════════════════════════════════════╗
// ║   📋 LotacaoMotoristaRepository.cs | Repository/ | 2026-01-20                                     ║
// ║   ❌ BUG CRÍTICO: GetListForDropDown retorna SelectListItem VAZIO (sem Text/Value)!               ║
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
    public class LotacaoMotoristaRepository : Repository<LotacaoMotorista>, ILotacaoMotoristaRepository
    {
        private new readonly FrotiXDbContext _db;

        public LotacaoMotoristaRepository(FrotiXDbContext db) : base(db)
        {
            _db = db;
        }

        // [ETAPA] ❌ BUG CRÍTICO: SelectListItem vazio - método não funciona!
        // TODO: Implementar Text/Value (ex: JOIN com Motorista/Lotacao para exibir nomes)
        public IEnumerable<SelectListItem> GetLotacaoMotoristaListForDropDown()
        {
            return _db.LotacaoMotorista
                .Select(i => new SelectListItem()
                {
                });
        }

        // [ETAPA] Update - ⚠️ Código morto + quebra Unit of Work
        public new void Update(LotacaoMotorista lotacaoMotorista)
        {
            var objFromDb = _db.LotacaoMotorista.AsTracking().FirstOrDefault(lm => lm.LotacaoMotoristaId == lotacaoMotorista.LotacaoMotoristaId);

            _db.Update(lotacaoMotorista);
            _db.SaveChanges();

        }


    }
}


