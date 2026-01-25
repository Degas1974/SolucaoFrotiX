using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using FrotiXApi.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FrotiXApi.Repository.IRepository
{
    public interface IViewViagensRepository : IRepository<ViewViagens>
    {
        IEnumerable<SelectListItem> GetViewViagensListForDropDown();

        void Update(ViewViagens viewViagens);

        Task<(List<T> Items, int TotalCount)> GetPaginatedAsync<T>(
            Expression<Func<ViewViagens, T>> selector,
            Expression<Func<ViewViagens, bool>> filter,
            int page,
            int pageSize
        );
    }
}
