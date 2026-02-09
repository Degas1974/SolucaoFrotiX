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
    public class MovimentacaoEmpenhoMultaRepository : Repository<MovimentacaoEmpenhoMulta>, IMovimentacaoEmpenhoMultaRepository
        {
        private readonly FrotiXDbContext _db;

        public MovimentacaoEmpenhoMultaRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

        public IEnumerable<SelectListItem> GetMovimentacaoEmpenhoMultaListForDropDown()
            {
            return _db.MovimentacaoEmpenhoMulta
            .OrderBy(o => o.DataMovimentacao)
            .Select(i => new SelectListItem()
                {
                Text = i.DataMovimentacao + "(" + i.Valor + ")",
                Value = i.MovimentacaoId.ToString()
                });
            }

        public void Update(MovimentacaoEmpenhoMulta movimentacaoempenhomulta)
            {
            var objFromDb = _db.MovimentacaoEmpenhoMulta.FirstOrDefault(s => s.MovimentacaoId == movimentacaoempenhomulta.MovimentacaoId);

            _db.Update(movimentacaoempenhomulta);
            _db.SaveChanges();

            }
        }
    }


