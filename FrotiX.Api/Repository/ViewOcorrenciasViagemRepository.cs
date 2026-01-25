using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using FrotiXApi.Data;
using FrotiXApi.Models;
using FrotiXApi.Repository.IRepository;

namespace FrotiXApi.Repository
{
    public class ViewOcorrenciasViagemRepository : IViewOcorrenciasViagemRepository
    {
        private readonly FrotiXDbContext _db;

        public ViewOcorrenciasViagemRepository(FrotiXDbContext db)
        {
            _db = db;
        }

        public IEnumerable<ViewOcorrenciasViagem> GetAll(Expression<Func<ViewOcorrenciasViagem , bool>>? filter = null , string? includeProperties = null)
        {
            IQueryable<ViewOcorrenciasViagem> query = _db.ViewOcorrenciasViagem;

            if (filter != null)
                query = query.Where(filter);

            return query.ToList();
        }

        public ViewOcorrenciasViagem? GetFirstOrDefault(Expression<Func<ViewOcorrenciasViagem , bool>> filter , string? includeProperties = null)
        {
            return _db.ViewOcorrenciasViagem.Where(filter).FirstOrDefault();
        }
    }
}
