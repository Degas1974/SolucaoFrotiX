using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FrotiXApi.Models;
using FrotiXApi.Models.Views;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FrotiXApi.Repository.IRepository
    {
    public interface IViewMotoristasViagemRepository : IRepository<ViewMotoristasViagem>
        {

        IEnumerable<SelectListItem> GetViewMotoristasViagemListForDropDown();

        void Update(ViewMotoristasViagem viewMotoristasViagem);

        }
    }


