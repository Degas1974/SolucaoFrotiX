// ╔═══════════════════════════════════════════════════════════════════════════════════════════════════╗
// ║   🏢 SecaoPatrimonialRepository.cs | Repository/ | 2026-01-20                                     ║
// ║   Seção Patrimonial. ⚠️ Text: NomeSecao + SetorId (GUID, deveria ser nome do setor!)             ║
// ╚═══════════════════════════════════════════════════════════════════════════════════════════════════╝

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FrotiX.Data;
using FrotiX.Models;
using FrotiX.Models.Cadastros;
using FrotiX.Repository.IRepository;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FrotiX.Repository
{
    public class SecaoPatrimonialRepository : Repository<SecaoPatrimonial>, ISecaoPatrimonialRepository
    {
        private new readonly FrotiXDbContext _db;

        public SecaoPatrimonialRepository(FrotiXDbContext db) : base(db)
        {
            _db = db;
        }

        // [ETAPA] Dropdown - OrderBy: NomeSecao
        // ⚠️ Text: NomeSecao + SetorId (mostra GUID em vez do nome do setor!)
        // TODO: JOIN com SetorPatrimonial para exibir NomeSetor
        public IEnumerable<SelectListItem> GetSecaoListForDropDown()
        {
            return _db.SecaoPatrimonial
            .OrderBy(o => o.NomeSecao)
            .Select(i => new SelectListItem()
            {
                Text = i.NomeSecao + "/" + i.SetorId.ToString(),
                Value = i.SecaoId.ToString()
            }); ;
        }

        // [ETAPA] Update - ⚠️ Código morto + quebra Unit of Work
        public new void Update(SecaoPatrimonial secao)
        {
            var objFromDb = _db.SecaoPatrimonial.AsTracking().FirstOrDefault(s => s.SecaoId == secao.SecaoId);

            _db.Update(secao);
            _db.SaveChanges();

        }


    }
}


