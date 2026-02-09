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
    public class LavadoresLavagemRepository : Repository<LavadoresLavagem>, ILavadoresLavagemRepository
        {
        private readonly FrotiXDbContext _db;

        public LavadoresLavagemRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

        public IEnumerable<SelectListItem> GetLavadoresLavagemListForDropDown()
            {
            return _db.LavadoresLavagem
            .OrderBy(o => o.LavagemId)
            .Select(i => new SelectListItem()
                {
                Text = i.LavadorId.ToString(),
                Value = i.LavagemId.ToString()
                }); ; ;
            }

        public void Update(LavadoresLavagem lavadoresLavagem)
            {
            var objFromDb = _db.LavadoresLavagem.FirstOrDefault(s => s.LavagemId == lavadoresLavagem.LavagemId);

            _db.Update(lavadoresLavagem);
            _db.SaveChanges();

            }


        }
    }


