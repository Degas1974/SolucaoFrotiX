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
    public class ViewCustosViagemRepository : Repository<ViewCustosViagem>, IViewCustosViagemRepository
        {
        private readonly FrotiXDbContext _db;

        public ViewCustosViagemRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

        public IEnumerable<SelectListItem> GetViewCustosViagemListForDropDown()
            {
            return _db.ViewViagens
            .OrderBy(o => o.DataInicial)
            .Select(i => new SelectListItem()
                {
                Text = i.DataInicial.ToString(),
                Value = i.ViagemId.ToString()
                }); ; ;
            }

        public void Update(ViewCustosViagem viewCustosViagem)
            {
            var objFromDb = _db.ViewViagens.FirstOrDefault(s => s.ViagemId == viewCustosViagem.ViagemId);

            _db.Update(viewCustosViagem);
            _db.SaveChanges();

            }


        }
    }


