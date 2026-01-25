using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FrotiXApi.Data;
using FrotiXApi.Models;
using FrotiXApi.Repository.IRepository;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FrotiXApi.Repository
    {
    public class LavagemRepository : Repository<Lavagem>, ILavagemRepository
        {
        private readonly FrotiXDbContext _db;

        public LavagemRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

        public IEnumerable<SelectListItem> GetLavagemListForDropDown()
            {
            return _db.Lavagem
            .OrderBy(o => o.Data)
            .Select(i => new SelectListItem()
                {
                Text = i.Data.ToString(),
                Value = i.LavagemId.ToString()
                }); ; ;
            }

        public void Update(Lavagem lavagem)
            {
            var objFromDb = _db.Lavagem.FirstOrDefault(s => s.LavagemId == lavagem.LavagemId);

            _db.Update(lavagem);
            _db.SaveChanges();

            }


        }
    }


