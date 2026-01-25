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
    public class ViewPendenciasManutencaoRepository : Repository<ViewPendenciasManutencao>, IViewPendenciasManutencaoRepository
        {
        private readonly FrotiXDbContext _db;

        public ViewPendenciasManutencaoRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

        public IEnumerable<SelectListItem> GetViewPendenciasManutencaoListForDropDown()
            {
            return _db.ViewPendenciasManutencao
            .OrderBy(o => o.DataItem)
            .Select(i => new SelectListItem()
                {
                Text = i.DataItem.ToString(),
                Value = i.Nome.ToString()
                }); ; ;
            }

        public void Update(ViewPendenciasManutencao viewPendenciasManutencao)
            {
            var objFromDb = _db.ViewPendenciasManutencao.FirstOrDefault(s => s.ItemManutencaoId == viewPendenciasManutencao.ItemManutencaoId);

            _db.Update(viewPendenciasManutencao);
            _db.SaveChanges();

            }


        }
    }


