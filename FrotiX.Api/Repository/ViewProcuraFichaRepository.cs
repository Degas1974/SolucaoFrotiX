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
    public class ViewProcuraFichaRepository : Repository<ViewProcuraFicha>, IViewProcuraFichaRepository
        {
        private readonly FrotiXDbContext _db;

        public ViewProcuraFichaRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

        public IEnumerable<SelectListItem> GetViewProcuraFichaListForDropDown()
            {
            return _db.ViewProcuraFicha
            .OrderBy(o => o.DataInicial)
            .Select(i => new SelectListItem()
                {
                Text = i.DataInicial.ToString(),
                Value = i.VeiculoId.ToString()
                }); ; ;
            }

        public void Update(ViewProcuraFicha viewProcuraFicha)
            {
            var objFromDb = _db.ViewProcuraFicha.FirstOrDefault(s => s.VeiculoId == viewProcuraFicha.VeiculoId);

            _db.Update(viewProcuraFicha);
            _db.SaveChanges();

            }


        }
    }


