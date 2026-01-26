using FrotiXApi.Data;
using FrotiXApi.Models;
using FrotiXApi.Models.Views;
using FrotiXApi.Repository.IRepository;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrotiXApi.Repository
    {
    public class ViewMotoristasViagemRepository : Repository<ViewMotoristasViagem>, IViewMotoristasViagemRepository
        {
        private readonly FrotiXDbContext _db;

        public ViewMotoristasViagemRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

        public IEnumerable<SelectListItem> GetViewMotoristasViagemListForDropDown()
            {
            return _db.ViewMotoristasViagem
            .OrderBy(o => o.Nome)
            .Select(i => new SelectListItem()
                {
                Text = i.Nome,
                Value = i.MotoristaId.ToString()
                }); ;
            }

        public void Update(ViewMotoristasViagem viewMotoristasviagem)
            {
            var objFromDb = _db.ViewMotoristasViagem.FirstOrDefault(s => s.MotoristaId == viewMotoristasviagem.MotoristaId);

            _db.Update(viewMotoristasviagem);
            _db.SaveChanges();

            }


        }
    }


