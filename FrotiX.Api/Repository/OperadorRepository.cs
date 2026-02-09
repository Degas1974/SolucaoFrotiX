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
    public class OperadorRepository : Repository<Operador>, IOperadorRepository
        {
        private readonly FrotiXDbContext _db;

        public OperadorRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

        public IEnumerable<SelectListItem> GetOperadorListForDropDown()
            {
            return _db.Operador
            .OrderBy(o => o.Nome)
            .Select(i => new SelectListItem()
                {
                Text = i.Nome,
                Value = i.OperadorId.ToString()
                }); ;
            }

        public void Update(Operador operador)
            {
            var objFromDb = _db.Operador.FirstOrDefault(s => s.OperadorId == operador.OperadorId);

            _db.Update(operador);
            _db.SaveChanges();

            }


        }
    }


