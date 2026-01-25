using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FrotiXApi.Models;

namespace FrotiXApi.Repository.IRepository
{
    public interface IOcorrenciaViagemRepository
    {
        IEnumerable<OcorrenciaViagem> GetAll(Expression<Func<OcorrenciaViagem, bool>>? filter = null, string? includeProperties = null);
        OcorrenciaViagem? GetFirstOrDefault(Expression<Func<OcorrenciaViagem, bool>> filter, string? includeProperties = null);
        void Add(OcorrenciaViagem entity);
        void Remove(OcorrenciaViagem entity);
        void Update(OcorrenciaViagem entity);
    }
}
