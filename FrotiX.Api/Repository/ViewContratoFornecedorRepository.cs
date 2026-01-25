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
    public class ViewContratoFornecedorRepository : Repository<ViewContratoFornecedor>, IViewContratoFornecedorRepository
        {
        private readonly FrotiXDbContext _db;

        public ViewContratoFornecedorRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

        public IEnumerable<SelectListItem> GetViewContratoFornecedorListForDropDown()
            {
            return _db.ViewContratoFornecedor
            .OrderBy(o => o.Descricao)
            .Select(i => new SelectListItem()
                {
                Text = i.Descricao.ToString(),
                Value = i.ContratoId.ToString()
                }); ; ;
            }

        public void Update(ViewContratoFornecedor viewContratoFornecedor)
            {
            var objFromDb = _db.ViewContratoFornecedor.FirstOrDefault(s => s.ContratoId == viewContratoFornecedor.ContratoId);

            _db.Update(viewContratoFornecedor);
            _db.SaveChanges();

            }


        }
    }


