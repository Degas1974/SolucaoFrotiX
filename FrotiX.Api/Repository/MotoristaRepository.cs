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
    public class MotoristaRepository : Repository<Motorista>, IMotoristaRepository
        {
        private readonly FrotiXDbContext _db;

        public MotoristaRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

        public IEnumerable<SelectListItem> GetMotoristaListForDropDown()
            {
            return _db.Motorista
            .OrderBy(o => o.Nome)
            .Select(i => new SelectListItem()
                {
                Text = i.Nome,
                Value = i.MotoristaId.ToString()
                }); ;
            }

        public void Update(Motorista motorista)
            {
            var objFromDb = _db.Motorista.FirstOrDefault(s => s.MotoristaId == motorista.MotoristaId);

            _db.Update(motorista);
            _db.SaveChanges();

            }


        }
    }


