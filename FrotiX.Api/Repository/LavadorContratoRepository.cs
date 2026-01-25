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
    public class LavadorContratoRepository : Repository<LavadorContrato>, ILavadorContratoRepository
        {
        private readonly FrotiXDbContext _db;

        public LavadorContratoRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

        public IEnumerable<SelectListItem> GetLavadorContratoListForDropDown()
            {
            return _db.LavadorContrato.Select(i => new SelectListItem()
                {
                //Text = i.Placa,
                //Value = i.VeiculoId.ToString()
                }); ;
            }

        public void Update(LavadorContrato lavadorContrato)
            {
            var objFromDb = _db.LavadorContrato.FirstOrDefault(s => (s.LavadorId == lavadorContrato.LavadorId) && (s.ContratoId == lavadorContrato.ContratoId));

            _db.Update(lavadorContrato);
            _db.SaveChanges();

            }


        }
    }


