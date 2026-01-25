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
    public class FornecedorRepository : Repository<Fornecedor>, IFornecedorRepository
        {
        private readonly FrotiXDbContext _db;

        public FornecedorRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

        public IEnumerable<SelectListItem> GetFornecedorListForDropDown()
            {

            return _db.Fornecedor
            .OrderBy(o => o.DescricaoFornecedor)
            .Select(i => new SelectListItem()
                {
                Text = i.DescricaoFornecedor,
                Value = i.FornecedorId.ToString()
                }); ;
            }

        public void Update(Fornecedor fornecedor)
            {
            var objFromDb = _db.Fornecedor.FirstOrDefault(s => s.FornecedorId == fornecedor.FornecedorId);

            _db.Update(fornecedor);
            _db.SaveChanges();

            }


        }
    }


