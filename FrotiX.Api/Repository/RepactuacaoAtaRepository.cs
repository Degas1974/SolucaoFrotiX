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
    public class RepactuacaoAtaRepository : Repository<RepactuacaoAta>, IRepactuacaoAtaRepository
        {
        private readonly FrotiXDbContext _db;

        public RepactuacaoAtaRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

        public IEnumerable<SelectListItem> GetRepactuacaoAtaListForDropDown()
            {
            return _db.RepactuacaoAta
                .OrderBy(o => o.Descricao)
                .Select(i => new SelectListItem()
                    {
                    Text = i.Descricao,
                    Value = i.RepactuacaoAtaId.ToString()
                    });
            }

        public void Update(RepactuacaoAta repactuacaoitemveiculoata)
            {
            var objFromDb = _db.RepactuacaoAta.FirstOrDefault(s => s.RepactuacaoAtaId == repactuacaoitemveiculoata.RepactuacaoAtaId);

            _db.Update(repactuacaoitemveiculoata);
            _db.SaveChanges();

            }


        }
    }


