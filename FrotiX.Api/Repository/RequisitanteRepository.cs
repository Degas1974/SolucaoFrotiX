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
    public class RequisitanteRepository : Repository<Requisitante>, IRequisitanteRepository
    {
        private readonly FrotiXDbContext _db;

        public RequisitanteRepository(FrotiXDbContext db)
            : base(db)
        {
            _db = db;
        }

        public IEnumerable<SelectListItem> GetRequisitanteListForDropDown()
        {
            return _db
                .Requisitante.Where(s => s.Status == true)
                .OrderBy(o => o.Nome)
                .Select(i => new SelectListItem()
                {
                    Text = i.Nome + "(" + i.Ponto + ")",
                    Value = i.RequisitanteId.ToString(),
                });
        }

        public void Update(Requisitante requisitante)
        {
            var objFromDb = _db.Requisitante.FirstOrDefault(s =>
                s.RequisitanteId == requisitante.RequisitanteId
            );

            _db.Update(requisitante);
            _db.SaveChanges();
        }
    }
}
