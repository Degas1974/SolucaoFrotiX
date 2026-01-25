// ╔═══════════════════════════════════════════════════════════════════════════════════════════════════╗
// ║   ⛽ MediaCombustivelRepository.cs | Repository/ | 2026-01-20                                     ║
// ║   Média combustível (Ano/Mês). ⚠️ Chave composta (4 campos). Código morto + Unit of Work         ║
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
    public class MediaCombustivelRepository : Repository<MediaCombustivel>, IMediaCombustivelRepository
    {
        private new readonly FrotiXDbContext _db;

        public MediaCombustivelRepository(FrotiXDbContext db) : base(db)
        {
            _db = db;
        }

        // [ETAPA] Dropdown - OrderBy: Ano, Text: Ano
        // ⚠️ Value usa CombustivelId (pode não ser único - chave é composta!)
        public IEnumerable<SelectListItem> GetMediaCombustivelListForDropDown()
        {
            return _db.MediaCombustivel
                .OrderBy(o => o.Ano)
                .Select(i => new SelectListItem()
                {
                    Text = i.Ano.ToString(),
                    Value = i.CombustivelId.ToString()
                });
        }

        // [ETAPA] Update - ⚠️ Chave composta: CombustivelId + NotaFiscalId + Ano + Mes
        // ⚠️ Código morto + quebra Unit of Work
        public new void Update(MediaCombustivel mediacombustivel)
        {
            var objFromDb = _db.MediaCombustivel.AsTracking().FirstOrDefault(s => (s.CombustivelId == mediacombustivel.CombustivelId) && (s.NotaFiscalId == mediacombustivel.NotaFiscalId) && (s.Ano == mediacombustivel.Ano) && (s.Mes == mediacombustivel.Mes));

            _db.Update(mediacombustivel);
            _db.SaveChanges();

        }


    }
}


