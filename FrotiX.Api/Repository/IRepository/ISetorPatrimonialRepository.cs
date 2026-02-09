using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FrotiXApi.Data;
using FrotiXApi.Models;
using FrotiXApi.Models.Cadastros;
using FrotiXApi.Repository.IRepository;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FrotiXApi.Repository.IRepository
    {
    public interface ISetorPatrimonialRepository : IRepository<SetorPatrimonial>
        {

        IEnumerable<SelectListItem> GetSetorListForDropDown();

        void Update(SetorPatrimonial setor);

        }
    }


