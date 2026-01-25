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
    public class ItensManutencaoRepository : Repository<ItensManutencao>, IItensManutencaoRepository
        {
        private readonly FrotiXDbContext _db;

        public ItensManutencaoRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

        public IEnumerable<SelectListItem> GetItensManutencaoListForDropDown()
            {
            return _db.ItensManutencao
                .OrderBy(o => o.DataItem)
                .Select(i => new SelectListItem()
                    {
                    Text = i.Resumo,
                    Value = i.ItemManutencaoId.ToString()
                    });
            }

        public void Update(ItensManutencao itensManutencao)
            {
            var objFromDb = _db.ItensManutencao.FirstOrDefault(s => s.ItemManutencaoId == itensManutencao.ItemManutencaoId);

            _db.Update(itensManutencao);
            _db.SaveChanges();

            }


        }
    }


