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
    public class PatrimonioRepository : Repository<Patrimonio>, IPatrimonioRepository
        {
        private readonly FrotiXDbContext _db;

        public PatrimonioRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

        public IEnumerable<SelectListItem> GetPatrimonioListForDropDown()
            {
            return _db.Patrimonio
            .OrderBy(o => o.NumeroSerie)
            .Select(i => new SelectListItem()
                {
                Text = i.NPR,
                Value = i.PatrimonioId.ToString()
                });
            }

        public void Update(Patrimonio patrimonio)
            {
            var objFromDb = _db.Patrimonio.FirstOrDefault(s => s.PatrimonioId == patrimonio.PatrimonioId);

            _db.Update(patrimonio);
            _db.SaveChanges();

            }


        }
    }


