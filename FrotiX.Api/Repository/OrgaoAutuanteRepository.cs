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
    public class OrgaoAutuanteRepository : Repository<OrgaoAutuante>, IOrgaoAutuanteRepository
        {
        private readonly FrotiXDbContext _db;

        public OrgaoAutuanteRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

        public IEnumerable<SelectListItem> GetOrgaoAutuanteListForDropDown()
            {
            return _db.OrgaoAutuante
                .OrderBy(o => o.Nome)
                .Select(i => new SelectListItem()
                    {
                    Text = i.Nome + " (" + i.Sigla + ")",
                    Value = i.OrgaoAutuanteId.ToString()
                    });
            }

        public void Update(OrgaoAutuante orgaoautuante)
            {
            var objFromDb = _db.OrgaoAutuante.FirstOrDefault(s => s.OrgaoAutuanteId == orgaoautuante.OrgaoAutuanteId);

            _db.Update(orgaoautuante);
            _db.SaveChanges();

            }


        }
    }


