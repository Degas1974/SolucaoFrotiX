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
    public class EmpenhoRepository : Repository<Empenho>, IEmpenhoRepository
        {
        private readonly FrotiXDbContext _db;

        public EmpenhoRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

        public IEnumerable<SelectListItem> GetEmpenhoListForDropDown()
            {
            return _db.Empenho
            .Join(_db.Contrato, empenho => empenho.ContratoId, contrato => contrato.ContratoId, (empenho, contrato) => new { empenho, contrato })
            .OrderBy(o => o.empenho.NotaEmpenho)
            .Select(i => new SelectListItem()
                {
                Text = i.empenho.NotaEmpenho + "(" + i.contrato.AnoContrato + "/" + i.contrato.NumeroContrato + ")",
                Value = i.contrato.ContratoId.ToString()
                });
            }

        public void Update(Empenho empenho)
            {
            var objFromDb = _db.Empenho.FirstOrDefault(s => s.EmpenhoId == empenho.EmpenhoId);

            _db.Update(empenho);
            _db.SaveChanges();

            }
        }
    }


