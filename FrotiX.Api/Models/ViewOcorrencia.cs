using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FrotiXApi.Data;
using FrotiXApi.Repository;
using FrotiXApi.Repository.IRepository;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FrotiXApi.Models
    {
    public class ViewOcorrenciaRepository : Repository<ViewOcorrencia>, IViewOcorrenciaRepository
        {
        private readonly FrotiXDbContext _db;

        public ViewOcorrenciaRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

        public IEnumerable<SelectListItem> GetViewOcorrenciaListForDropDown()
            {
            return _db.ViewOcorrencia
            .OrderBy(o => o.DataInicial)
            .Select(i => new SelectListItem()
                {
                Text = i.DataInicial.ToString(),
                Value = i.ViagemId.ToString()
                }); ; ;
            }

        public void Update(ViewOcorrencia viewOcorrencia)
            {
            var objFromDb = _db.ViewOcorrencia.FirstOrDefault(s => s.ViagemId == viewOcorrencia.ViagemId);

            _db.Update(viewOcorrencia);
            _db.SaveChanges();

            }


        }
    }


