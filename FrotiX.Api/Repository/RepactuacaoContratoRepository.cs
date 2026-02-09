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
    public class RepactuacaoContratoRepository : Repository<RepactuacaoContrato>, IRepactuacaoContratoRepository
        {
        private readonly FrotiXDbContext _db;

        public RepactuacaoContratoRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

        public IEnumerable<SelectListItem> GetRepactuacaoContratoListForDropDown()
            {
            return _db.RepactuacaoContrato
                .OrderBy(o => o.Descricao)
                .Select(i => new SelectListItem()
                    {
                    Text = i.Descricao,
                    Value = i.RepactuacaoContratoId.ToString()
                    });
            }

        public void Update(RepactuacaoContrato RepactuacaoContrato)
            {
            var objFromDb = _db.RepactuacaoContrato.FirstOrDefault(s => s.RepactuacaoContratoId == RepactuacaoContrato.RepactuacaoContratoId);

            _db.Update(RepactuacaoContrato);
            _db.SaveChanges();

            }


        }
    }


