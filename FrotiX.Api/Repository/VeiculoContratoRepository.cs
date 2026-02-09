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
    public class VeiculoContratoRepository : Repository<VeiculoContrato>, IVeiculoContratoRepository
        {
        private readonly FrotiXDbContext _db;

        public VeiculoContratoRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

        public IEnumerable<SelectListItem> GetVeiculoContratoListForDropDown()
            {
            return _db.VeiculoContrato.Select(i => new SelectListItem()
                {
                //Text = i.Placa,
                //Value = i.VeiculoId.ToString()
                }); ;
            }

        public void Update(VeiculoContrato veiculoContrato)
            {
            var objFromDb = _db.VeiculoContrato.FirstOrDefault(s => (s.VeiculoId == veiculoContrato.VeiculoId) && (s.ContratoId == veiculoContrato.ContratoId));

            _db.Update(veiculoContrato);
            _db.SaveChanges();

            }


        }
    }


