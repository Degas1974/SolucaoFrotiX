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
    public class LavadorRepository : Repository<Lavador>, ILavadorRepository
        {
        private readonly FrotiXDbContext _db;

        public LavadorRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

        public IEnumerable<SelectListItem> GetLavadorListForDropDown()
            {
            return _db.Lavador
            .OrderBy(o => o.Nome)
            .Select(i => new SelectListItem()
                {
                Text = i.Nome,
                Value = i.LavadorId.ToString()
                }); ;
            }

        public void Update(Lavador lavador)
            {
            var objFromDb = _db.Lavador.FirstOrDefault(s => s.LavadorId == lavador.LavadorId);

            _db.Update(lavador);
            _db.SaveChanges();

            }


        }
    }


