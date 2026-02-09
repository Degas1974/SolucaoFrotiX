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
    public class ViewExisteItemContratoRepository : Repository<ViewExisteItemContrato>, IViewExisteItemContratoRepository
        {
        private readonly FrotiXDbContext _db;

        public ViewExisteItemContratoRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

        public IEnumerable<SelectListItem> GetViewExisteItemContratoListForDropDown()
            {
            return _db.ViewExisteItemContrato
            .OrderBy(o => o.NumItem)
            .Select(i => new SelectListItem()
                {
                Text = i.Descricao.ToString(),
                Value = i.NumItem.ToString()
                }); ; ;
            }

        public void Update(ViewExisteItemContrato viewExisteItemContrato)
            {
            var objFromDb = _db.ViewExisteItemContrato.FirstOrDefault(v => v.RepactuacaoContratoId == viewExisteItemContrato.RepactuacaoContratoId);

            _db.Update(viewExisteItemContrato);
            _db.SaveChanges();

            }


        }
    }


