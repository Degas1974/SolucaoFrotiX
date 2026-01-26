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
    public class ViewControleAcessoRepository : Repository<ViewControleAcesso>, IViewControleAcessoRepository
        {
        private readonly FrotiXDbContext _db;

        public ViewControleAcessoRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

        public IEnumerable<SelectListItem> GetViewControleAcessoListForDropDown()
            {
            return _db.ViewControleAcesso
            .OrderBy(o => o.Ordem)
            .Select(i => new SelectListItem()
                {
                Text = i.Nome,
                Value = i.UsuarioId.ToString()
                }); ;
            }

        public void Update(ViewControleAcesso viewControleAcesso)
            {
            var objFromDb = _db.ViewControleAcesso.FirstOrDefault(s => s.RecursoId == viewControleAcesso.RecursoId);

            _db.Update(viewControleAcesso);
            _db.SaveChanges();

            }


        }
    }


