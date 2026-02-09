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
    public class ViewFluxoEconomildoRepository : Repository<ViewFluxoEconomildo>, IViewFluxoEconomildoRepository
        {
        private readonly FrotiXDbContext _db;

        public ViewFluxoEconomildoRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

        public IEnumerable<SelectListItem> GetViewFluxoEconomildoListForDropDown()
            {
            return _db.ViewFluxoEconomildo
            .OrderBy(o => o.Data)
            .Select(i => new SelectListItem()
                {
                Text = i.Data.ToString(),
                Value = i.ViagemEconomildoId.ToString()
                }); ; ;
            }

        public void Update(ViewFluxoEconomildo viewFluxoEconomildo)
            {
            var objFromDb = _db.ViewFluxoEconomildo.FirstOrDefault(s => s.ViagemEconomildoId == viewFluxoEconomildo.ViagemEconomildoId);

            _db.Update(viewFluxoEconomildo);
            _db.SaveChanges();

            }


        }
    }


