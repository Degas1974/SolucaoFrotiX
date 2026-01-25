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
    public class NotaFiscalRepository : Repository<NotaFiscal>, INotaFiscalRepository
        {
        private readonly FrotiXDbContext _db;

        public NotaFiscalRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

        public IEnumerable<SelectListItem> GetNotaFiscalListForDropDown()
            {
            return _db.NotaFiscal
            .OrderBy(o => o.NumeroNF)
            .Select(i => new SelectListItem()
                {
                Text = i.NumeroNF.ToString(),
                Value = i.NotaFiscalId.ToString()
                }); ;
            }

        public void Update(NotaFiscal notaFiscal)
            {
            var objFromDb = _db.NotaFiscal.FirstOrDefault(s => s.NotaFiscalId == notaFiscal.NotaFiscalId);

            _db.Update(notaFiscal);
            _db.SaveChanges();

            }
        }
    }


