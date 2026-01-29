// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ 📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                        ║
// ║ ARQUIVO    : ViewViagensRepository.cs                                        ║
// ║ LOCALIZAÇÃO: Repository/                                                     ║
// ║ LOTE       : 24 — Repository                                                 ║
// ║ DATA       : 29/01/2026                                                      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ FINALIDADE                                                                   ║
// ║ Repositório especializado para SQL View ViewViagens. Fornece visão          ║
// ║ consolidada de viagens com dados de motorista, veículo, requisitante, etc.    ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ PRINCIPAIS MÉTODOS                                                           ║
// ║ • GetPaginatedAsync() → Query paginada com selector genérico                 ║
// ║ • GetViewViagensListForDropDown() → SelectList ordenada por data             ║
// ╚══════════════════════════════════════════════════════════════════════════════╝
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using FrotiX.Data;
using FrotiX.Models;
using FrotiX.Repository.IRepository;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FrotiX.Repository
{
    public class ViewViagensRepository : Repository<ViewViagens>, IViewViagensRepository
    {
        private new readonly FrotiXDbContext _db;

        public ViewViagensRepository(FrotiXDbContext db)
            : base(db)
        {
            _db = db;
        }

        public async Task<(List<T> Items, int TotalCount)> GetPaginatedAsync<T>(
            Expression<Func<ViewViagens, T>> selector,
            Expression<Func<ViewViagens, bool>> filter,
            int page,
            int pageSize
        )
        {
            var query = _db.ViewViagens.AsNoTracking();

            if (filter != null)
                query = query.Where(filter);

            var totalCount = await query.CountAsync();

            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(selector)
                .ToListAsync();

            return (items, totalCount);
        }

        public IEnumerable<SelectListItem> GetViewViagensListForDropDown()
        {
            return _db
                .ViewViagens.OrderBy(o => o.DataInicial)
                .Select(i => new SelectListItem()
                {
                    Text = i.DataInicial.ToString(),
                    Value = i.ViagemId.ToString(),
                });
            ;
            ;
        }

        public new void Update(ViewViagens viewViagens)
        {
            var objFromDb = _db.ViewViagens.FirstOrDefault(s => s.ViagemId == viewViagens.ViagemId);

            _db.Update(viewViagens);
            _db.SaveChanges();
        }
    }
}
