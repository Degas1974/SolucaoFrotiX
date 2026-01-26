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
    public class CorridasTaxiLegRepository : Repository<CorridasTaxiLeg>, ICorridasTaxiLegRepository
        {
        private readonly FrotiXDbContext _db;

        public CorridasTaxiLegRepository(FrotiXDbContext db)
            : base(db)
            {
            _db = db;
            }

        public IEnumerable<SelectListItem> GetCorridasTaxiLegListForDropDown()
            {
            return _db.CorridasTaxiLeg.Select(i => new SelectListItem()
                {
                Text = i.DescUnidade,
                Value = i.CorridaId.ToString(),
                });
            }

        public void Update(CorridasTaxiLeg corridasTaxiLeg)
            {
            var objFromDb = _db.CorridasTaxiLeg.FirstOrDefault(s =>
                s.CorridaId == corridasTaxiLeg.CorridaId
            );

            _db.Update(corridasTaxiLeg);
            _db.SaveChanges();
            }

        public bool ExisteCorridaNoMesAno(int ano, int mes)
            {
            return _db.CorridasTaxiLeg.Any(x =>
                x.DataAgenda.HasValue
                && x.DataAgenda.Value.Year == ano
                && x.DataAgenda.Value.Month == mes
            );
            }
        }
    }


