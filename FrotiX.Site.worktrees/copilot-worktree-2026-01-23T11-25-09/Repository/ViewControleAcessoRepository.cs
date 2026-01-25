// ═══════════════════════════════════════════════════════════════════════════════════════════════════╗
// ║   📊 ViewControleAcessoRepository.cs | VIEW (Read-Only) | 2026-01-20                            ║
// ╚═══════════════════════════════════════════════════════════════════════════════════════════════════╝

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FrotiX.Data;
using FrotiX.Models;
using FrotiX.Repository.IRepository;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FrotiX.Repository
    {
    public class ViewControleAcessoRepository : Repository<ViewControleAcesso>, IViewControleAcessoRepository
        {
        private new readonly FrotiXDbContext _db;

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

        public new void Update(ViewControleAcesso viewControleAcesso)
            {
            var objFromDb = _db.ViewControleAcesso.AsTracking().FirstOrDefault(s => s.RecursoId == viewControleAcesso.RecursoId);

            _db.Update(viewControleAcesso);
            _db.SaveChanges();

            }


        }
    }


