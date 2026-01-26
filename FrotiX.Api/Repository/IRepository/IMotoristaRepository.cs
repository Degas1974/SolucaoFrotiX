using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FrotiXApi.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FrotiXApi.Repository.IRepository
    {
    public interface IMotoristaRepository : IRepository<Motorista>
        {

        IEnumerable<SelectListItem> GetMotoristaListForDropDown();

        void Update(Motorista motorista);

        }
    }


