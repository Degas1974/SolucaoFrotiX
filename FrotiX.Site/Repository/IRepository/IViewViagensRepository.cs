// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ 📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                        ║
// ║ ARQUIVO    : IViewViagensRepository.cs                                       ║
// ║ LOCALIZAÇÃO: Repository/IRepository/                                         ║
// ║ LOTE       : 24 — Repository/IRepository                                     ║
// ║ DATA       : 29/01/2026                                                      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ FINALIDADE                                                                   ║
// ║ Interface do repositório de ViewViagens, consultando SQL View consolidada de ║
// ║ viagens com dados de motorista, veículo, requisitante e custos.              ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ MÉTODOS ADICIONAIS                                                           ║
// ║ • GetViewViagensListForDropDown() → DropDown de viagens                      ║
// ║ • GetPaginatedAsync<T>() → Paginação com projeção genérica                   ║
// ╚══════════════════════════════════════════════════════════════════════════════╝
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using FrotiX.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FrotiX.Repository.IRepository
{
    /// <summary>
    /// Interface do repositório de ViewViagens. Estende IRepository&lt;ViewViagens&gt;.
    /// </summary>
    public interface IViewViagensRepository : IRepository<ViewViagens>
    {
        IEnumerable<SelectListItem> GetViewViagensListForDropDown();

        void Update(ViewViagens viewViagens);

        Task<(List<T> Items, int TotalCount)> GetPaginatedAsync<T>(
            Expression<Func<ViewViagens, T>> selector,
            Expression<Func<ViewViagens, bool>> filter,
            int page,
            int pageSize
        );
    }
}
