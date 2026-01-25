// IContratoRepository.cs
using System.Linq;
using FrotiXApi.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FrotiXApi.Repository.IRepository
    {
    public interface IContratoRepository : IRepository<Contrato>
        {
        // Status é sempre TRUE, sem parâmetro "status"
        IQueryable<SelectListItem> GetDropDown(string? tipoContrato = null);
        }
    }


