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
    public class ViewLavagemRepository : Repository<ViewLavagem>, IViewLavagemRepository
        {
        private readonly FrotiXDbContext _db;

        public ViewLavagemRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

        public IEnumerable<SelectListItem> GetViewLavagemListForDropDown()
            {
            return _db.ViewLavagem
            .OrderBy(o => o.Data)
            .Select(i => new SelectListItem()
                {
                Text = i.Data.ToString(),
                Value = i.Lavadores.ToString()
                }); ; ;
            }

        public void Update(ViewLavagem viewLavagem)
            {
            var objFromDb = _db.ViewLavagem.FirstOrDefault(s => s.LavagemId == viewLavagem.LavagemId);

            _db.Update(viewLavagem);
            _db.SaveChanges();

            }


        }
    }


