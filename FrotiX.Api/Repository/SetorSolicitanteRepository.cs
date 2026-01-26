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
    public class SetorSolicitanteRepository : Repository<SetorSolicitante>, ISetorSolicitanteRepository
        {
        private readonly FrotiXDbContext _db;

        public SetorSolicitanteRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

        public IEnumerable<SelectListItem> GetSetorSolicitanteListForDropDown()
            {
            return _db.SetorSolicitante
            .OrderBy(o => o.Nome)
            .Select(i => new SelectListItem()
                {
                Text = i.Nome,
                Value = i.SetorSolicitanteId.ToString()
                }); ;
            }

        public void Update(SetorSolicitante setorSolicitante)
            {
            var objFromDb = _db.SetorSolicitante.FirstOrDefault(s => s.SetorSolicitanteId == setorSolicitante.SetorSolicitanteId);

            _db.Update(setorSolicitante);
            _db.SaveChanges();

            }


        }
    }


