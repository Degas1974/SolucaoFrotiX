using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FrotiXApi.Data;
using FrotiXApi.Models;
using FrotiXApi.Models.Cadastros;
using FrotiXApi.Repository.IRepository;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FrotiXApi.Repository
    {
    public class SecaoPatrimonialRepository : Repository<SecaoPatrimonial>, ISecaoPatrimonialRepository
        {
        private readonly FrotiXDbContext _db;

        public SecaoPatrimonialRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

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

        public void Update(SecaoPatrimonial secao)
            {
            var objFromDb = _db.SecaoPatrimonial.FirstOrDefault(s => s.SecaoId == secao.SecaoId);

            _db.Update(secao);
            _db.SaveChanges();

            }


        }
    }


