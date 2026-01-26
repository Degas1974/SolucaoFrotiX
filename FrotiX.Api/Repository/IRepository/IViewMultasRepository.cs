using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FrotiXApi.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FrotiXApi.Repository.IRepository
    {
    public interface IviewMultasRepository : IRepository<ViewMultas>
        {

        IEnumerable<SelectListItem> GetviewMultasListForDropDown();

        void Update(ViewMultas viewMultas);

        }
    }


