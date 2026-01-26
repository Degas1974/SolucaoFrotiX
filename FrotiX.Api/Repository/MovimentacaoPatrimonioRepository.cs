using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FrotiXApi.Data;
using FrotiXApi.Models;
using FrotiXApi.Models.Cadastros;
using FrotiXApi.Repository.IRepository;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FrotiXApi.Repository
    {
    public class MovimentacaoPatrimonioRepository : Repository<MovimentacaoPatrimonio>, IMovimentacaoPatrimonioRepository
        {
        private readonly FrotiXDbContext _db;

        public MovimentacaoPatrimonioRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

        public IEnumerable<SelectListItem> GetMovimentacaoPatrimonioListForDropDown()
            {
            return _db.MovimentacaoPatrimonio
            .OrderBy(o => o.PatrimonioId)
            .Select(i => new SelectListItem()
                {
                Text = i.DataMovimentacao.ToString(),
                Value = i.MovimentacaoPatrimonioId.ToString()
                }); ;
            }

        public void Update(MovimentacaoPatrimonio movimentacaoPatrimonio)
            {
            var objFromDb = _db.MovimentacaoPatrimonio.FirstOrDefault(s => s.MovimentacaoPatrimonioId == movimentacaoPatrimonio.MovimentacaoPatrimonioId);

            _db.Update(movimentacaoPatrimonio);
            _db.SaveChanges();

            }


        }
    }


