using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FrotiXApi.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FrotiXApi.Repository.IRepository
    {
    public interface IViewPendenciasManutencaoRepository : IRepository<ViewPendenciasManutencao>
        {

        IEnumerable<SelectListItem> GetViewPendenciasManutencaoListForDropDown();

        void Update(ViewPendenciasManutencao viewPendenciasManutencao);

        }
    }


