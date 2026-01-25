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
    public class ViewEmpenhosRepository : Repository<ViewEmpenhos>, IViewEmpenhosRepository
        {
        private readonly FrotiXDbContext _db;

        public ViewEmpenhosRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

        public IEnumerable<SelectListItem> GetViewEmpenhosListForDropDown()
            {
            return _db.ViewEmpenhos
            .OrderBy(o => o.NotaEmpenho)
            .Select(i => new SelectListItem()
                {
                Text = i.NotaEmpenho,
                Value = i.EmpenhoId.ToString()
                });
            }

        public void Update(ViewEmpenhos viewEmpenhos)
            {
            var objFromDb = _db.ViewEmpenhos.FirstOrDefault(s => s.EmpenhoId == viewEmpenhos.EmpenhoId);

            _db.Update(viewEmpenhos);
            _db.SaveChanges();

            }
        }
    }


