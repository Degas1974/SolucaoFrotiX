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
    public class ViewViagensAgendaTodosMesesRepository : Repository<ViewViagensAgendaTodosMeses>, IViewViagensAgendaTodosMesesRepository
        {
        private readonly FrotiXDbContext _db;

        public ViewViagensAgendaTodosMesesRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

        public IEnumerable<SelectListItem> GetViewViagensAgendaTodosMesesListForDropDown()
            {
            return _db.ViewViagensAgendaTodosMeses
            .OrderBy(o => o.DataInicial)
            .Select(i => new SelectListItem()
                {
                Text = i.DataInicial.ToString(),
                Value = i.ViagemId.ToString()
                });
            }

        public void Update(ViewViagensAgendaTodosMeses viewViagensAgendaTodosMeses)
            {
            var objFromDb = _db.ViewViagensAgendaTodosMeses.FirstOrDefault(s => s.ViagemId == viewViagensAgendaTodosMeses.ViagemId);

            _db.Update(viewViagensAgendaTodosMeses);
            _db.SaveChanges();

            }


        }
    }


