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
    public class CorridasCanceladasTaxiLegRepository : Repository<CorridasCanceladasTaxiLeg>, ICorridasCanceladasTaxiLegRepository
        {
        private readonly FrotiXDbContext _db;

        public CorridasCanceladasTaxiLegRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

        public IEnumerable<SelectListItem> GetCorridasCanceladasTaxiLegListForDropDown()
            {
            return _db.CorridasCanceladasTaxiLeg
            .Select(i => new SelectListItem()
                {
                Text = i.MotivoCancelamento,
                Value = i.CorridaCanceladaId.ToString()
                });
            }

        public void Update(CorridasCanceladasTaxiLeg corridasCanceladasTaxiLeg)
            {
            var objFromDb = _db.CorridasCanceladasTaxiLeg.FirstOrDefault(s => s.CorridaCanceladaId == corridasCanceladasTaxiLeg.CorridaCanceladaId);

            _db.Update(corridasCanceladasTaxiLeg);
            _db.SaveChanges();

            }


        }
    }


