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
    public class SetorPatrimonialRepository : Repository<SetorPatrimonial>, ISetorPatrimonialRepository
        {
        private readonly FrotiXDbContext _db;

        public SetorPatrimonialRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

        public IEnumerable<SelectListItem> GetSetorListForDropDown()
            {
            return _db.SetorPatrimonial
            .OrderBy(o => o.NomeSetor)
            .Select(i => new SelectListItem()
                {
                Text = i.NomeSetor,
                Value = i.SetorId.ToString()
                }); ;
            }

        public void Update(SetorPatrimonial setor)
            {
            var objFromDb = _db.SetorPatrimonial.FirstOrDefault(s => s.SetorId == setor.SetorId);

            _db.Update(setor);
            _db.SaveChanges();

            }


        }
    }


