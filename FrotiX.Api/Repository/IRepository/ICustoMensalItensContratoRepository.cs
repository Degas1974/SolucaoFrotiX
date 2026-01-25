using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FrotiXApi.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FrotiXApi.Repository.IRepository
    {
    public interface ICustoMensalItensContratoRepository : IRepository<CustoMensalItensContrato>
        {

        IEnumerable<SelectListItem> GetCustoMensalItensContratoListForDropDown();

        void Update(CustoMensalItensContrato customensalitenscontrato);

        }
    }


