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
    public class ViewItensManutencaoRepository : Repository<ViewItensManutencao>, IViewItensManutencaoRepository
        {
        private readonly FrotiXDbContext _db;

        public ViewItensManutencaoRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

        public IEnumerable<SelectListItem> GetViewItensManutencaoListForDropDown()
            {
            return _db.ViewItensManutencao
            .OrderBy(o => o.DataItem)
            .Select(i => new SelectListItem()
                {
                Text = i.DataItem.ToString(),
                Value = i.ItemManutencaoId.ToString()
                }); ; ;
            }

        public void Update(ViewItensManutencao viewItensManutencao)
            {
            var objFromDb = _db.ViewItensManutencao.FirstOrDefault(s => s.ItemManutencaoId == viewItensManutencao.ItemManutencaoId);

            _db.Update(viewItensManutencao);
            _db.SaveChanges();

            }


        }
    }


