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
    public class OperadorContratoRepository : Repository<OperadorContrato>, IOperadorContratoRepository
        {
        private readonly FrotiXDbContext _db;

        public OperadorContratoRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

        public IEnumerable<SelectListItem> GetOperadorContratoListForDropDown()
            {
            return _db.OperadorContrato.Select(i => new SelectListItem()
                {
                //Text = i.Placa,
                //Value = i.VeiculoId.ToString()
                }); ;
            }

        public void Update(OperadorContrato operadorContrato)
            {
            var objFromDb = _db.OperadorContrato.FirstOrDefault(s => (s.OperadorId == operadorContrato.OperadorId) && (s.ContratoId == operadorContrato.ContratoId));

            _db.Update(operadorContrato);
            _db.SaveChanges();

            }


        }
    }


