using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FrotiXApi.Models;

namespace FrotiXApi.Repository.IRepository
{
    public interface IViewOcorrenciasViagemRepository
    {
        IEnumerable<ViewOcorrenciasViagem> GetAll(Expression<Func<ViewOcorrenciasViagem, bool>>? filter = null, string? includeProperties = null);
        ViewOcorrenciasViagem? GetFirstOrDefault(Expression<Func<ViewOcorrenciasViagem, bool>> filter, string? includeProperties = null);
    }
}
