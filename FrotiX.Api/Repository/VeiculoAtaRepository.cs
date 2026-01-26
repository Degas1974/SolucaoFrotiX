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
    public class VeiculoAtaRepository : Repository<VeiculoAta>, IVeiculoAtaRepository
        {
        private readonly FrotiXDbContext _db;

        public VeiculoAtaRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

        public IEnumerable<SelectListItem> GetVeiculoAtaListForDropDown()
            {
            return _db.VeiculoAta.Select(i => new SelectListItem()
                {
                //Text = i.Placa,
                //Value = i.VeiculoId.ToString()
                }); ;
            }

        public void Update(VeiculoAta veiculoAta)
            {
            var objFromDb = _db.VeiculoAta.FirstOrDefault(s => (s.VeiculoId == veiculoAta.VeiculoId) && (s.AtaId == veiculoAta.AtaId));

            _db.Update(veiculoAta);
            _db.SaveChanges();

            }


        }
    }


