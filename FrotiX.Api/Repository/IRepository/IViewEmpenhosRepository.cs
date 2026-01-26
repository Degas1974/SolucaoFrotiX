using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FrotiXApi.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FrotiXApi.Repository.IRepository
    {
    public interface IViewEmpenhosRepository : IRepository<ViewEmpenhos>
        {

        IEnumerable<SelectListItem> GetViewEmpenhosListForDropDown();

        void Update(ViewEmpenhos viewEmpenhos);

        }
    }


