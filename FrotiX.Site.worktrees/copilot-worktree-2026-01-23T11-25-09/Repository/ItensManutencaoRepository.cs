// ╔═══════════════════════════════════════════════════════════════════════════════════════════════════╗
// ║   📋 ItensManutencaoRepository.cs | Repository/ | 2026-01-19                                     ║
// ║   Gerencia ITENS das Manutenções (serviços realizados, peças trocadas, etc.)                     ║
// ║   Problemas: Update() quebra Unit of Work + código morto objFromDb                                ║
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
    public class ItensManutencaoRepository : Repository<ItensManutencao>, IItensManutencaoRepository
    {
        private new readonly FrotiXDbContext _db;

        public ItensManutencaoRepository(FrotiXDbContext db) : base(db)
        {
            _db = db;
        }

        // [ETAPA] Busca itens de manutenção para dropdown - Ordenação por DataItem, Texto = Resumo
        public IEnumerable<SelectListItem> GetItensManutencaoListForDropDown()
        {
            return _db.ItensManutencao
                .OrderBy(o => o.DataItem)
                .Select(i => new SelectListItem()
                {
                    Text = i.Resumo,
                    Value = i.ItemManutencaoId.ToString()
                });
        }

        // [ETAPA] Update - ⚠️ Código morto + quebra Unit of Work
        public new void Update(ItensManutencao itensManutencao)
        {
            var objFromDb = _db.ItensManutencao.AsTracking().FirstOrDefault(s => s.ItemManutencaoId == itensManutencao.ItemManutencaoId);

            _db.Update(itensManutencao);
            _db.SaveChanges();

        }


    }
}


