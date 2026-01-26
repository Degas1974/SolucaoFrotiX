using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FrotiXApi.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FrotiXApi.Repository.IRepository
    {
    public interface ILavagemRepository : IRepository<Lavagem>
        {

        IEnumerable<SelectListItem> GetLavagemListForDropDown();

        void Update(Lavagem lavagem);

        }
    }


