using FrotiXApi.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FrotiXApi.Repository.IRepository
{
    public interface IViagemRepository : IRepository<Viagem>
    {
        IEnumerable<SelectListItem> GetViagemListForDropDown();

        void Update(Viagem viagem);

        Task<List<string>> GetDistinctOrigensAsync();
        Task<List<string>> GetDistinctDestinosAsync();
        Task CorrigirOrigemAsync(List<string> origensAntigas , string novaOrigem);
        Task CorrigirDestinoAsync(List<string> destinosAntigos , string novoDestino);

        /// <summary>
        /// Busca viagens de recorrência - detecta automaticamente se é primeiro registro ou subsequente
        /// </summary>
        Task<List<Viagem>> BuscarViagensRecorrenciaAsync(Guid id);

        // ✅ CORREÇÃO: Usar Viagem em vez de T genérico
        IQueryable<Viagem> GetQuery(Expression<Func<Viagem , bool>> filter = null);
    }
}
