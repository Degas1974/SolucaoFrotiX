/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: ViewViagensRepository.cs                                                               ║
   ║ 📂 CAMINHO: Repository/                                                                            ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
   ║    Repositório para a SQL View ViewViagens.                                                        ║
   ║    Fornece paginação genérica e listagens para UI com dados consolidados de viagens.               ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 MÉTODOS DISPONÍVEIS:                                                                            ║
   ║    • ViewViagensRepository(FrotiXDbContext db)                                                     ║
   ║    • GetPaginatedAsync<T>(Expression<Func<ViewViagens, T>> selector, ...)                          ║
   ║    • GetViewViagensListForDropDown()                                                              ║
   ║    • Update(ViewViagens viewViagens)                                                              ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ ⚠️ OBSERVAÇÕES:                                                                                     ║
   ║    Views são somente leitura; Update é mantido por compatibilidade.                                ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using FrotiX.Data;
using FrotiX.Models;
using FrotiX.Repository.IRepository;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FrotiX.Repository
{
    /// <summary>
    /// ╭───────────────────────────────────────────────────────────────────────────────────────────────╮
    /// │ 🎯 CLASSE: ViewViagensRepository                                                              │
    /// │ 📦 HERDA DE: Repository<ViewViagens>                                                          │
    /// │ 🔌 IMPLEMENTA: IViewViagensRepository                                                         │
    /// ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    ///
    /// Repositório responsável pela view de viagens.
    /// Fornece paginação genérica e listagens para UI.
    /// </summary>
    public class ViewViagensRepository : Repository<ViewViagens>, IViewViagensRepository
    {
        private new readonly FrotiXDbContext _db;

        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ MÉTODO: ViewViagensRepository                                                        │
        /// │ 🔗 RASTREABILIDADE:                                                                      │
        /// │    ⬅️ CHAMADO POR : UnitOfWork, Services, Controllers                                     │
        /// │    ➡️ CHAMA       : base(db)                                                             │
        /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
        ///
        /// <para>
        /// 🎯 <b>OBJETIVO:</b><br/>
        ///    Inicializar o repositório com o contexto do banco de dados.
        /// </para>
        ///
        /// <para>
        /// 📥 <b>PARÂMETROS:</b><br/>
        ///    db - Contexto do banco de dados da aplicação.
        /// </para>
        /// </summary>
        /// <param name="db">Instância de <see cref="FrotiXDbContext"/>.</param>
        public ViewViagensRepository(FrotiXDbContext db)
            : base(db)
        {
            _db = db;
        }

        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ MÉTODO: GetPaginatedAsync                                                           │
        /// │ 🔗 RASTREABILIDADE:                                                                      │
        /// │    ⬅️ CHAMADO POR : Services, Controllers                                                 │
        /// │    ➡️ CHAMA       : DbContext.ViewViagens, AsNoTracking, Where, CountAsync, Skip, Take   │
        /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
        ///
        /// <para>
        /// 🎯 <b>OBJETIVO:</b><br/>
        ///    Obter dados paginados da view de viagens com projeção genérica.
        /// </para>
        ///
        /// <para>
        /// 📥 <b>PARÂMETROS:</b><br/>
        ///    selector - Expressão de seleção/projeção<br/>
        ///    filter - Filtro aplicado à consulta<br/>
        ///    page - Página atual (1-based)<br/>
        ///    pageSize - Quantidade de registros por página
        /// </para>
        ///
        /// <para>
        /// 📤 <b>RETORNO:</b><br/>
        ///    Task&lt;(List&lt;T&gt; Items, int TotalCount)&gt; - Itens paginados e total de registros.
        /// </para>
        /// </summary>
        /// <param name="selector">Expressão de projeção.</param>
        /// <param name="filter">Filtro aplicado à consulta.</param>
        /// <param name="page">Página atual (1-based).</param>
        /// <param name="pageSize">Tamanho da página.</param>
        /// <returns>Itens paginados e total de registros.</returns>
        public async Task<(List<T> Items, int TotalCount)> GetPaginatedAsync<T>(
            Expression<Func<ViewViagens, T>> selector,
            Expression<Func<ViewViagens, bool>> filter,
            int page,
            int pageSize
        )
        {
            var query = _db.ViewViagens.AsNoTracking();

            if (filter != null)
                query = query.Where(filter);

            var totalCount = await query.CountAsync();

            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(selector)
                .ToListAsync();

            return (items, totalCount);
        }

        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ MÉTODO: GetViewViagensListForDropDown                                                │
        /// │ 🔗 RASTREABILIDADE:                                                                      │
        /// │    ⬅️ CHAMADO POR : Controllers, Services, UI (DropDowns)                                │
        /// │    ➡️ CHAMA       : DbContext.ViewViagens, OrderBy, Select                               │
        /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
        ///
        /// <para>
        /// 🎯 <b>OBJETIVO:</b><br/>
        ///    Obter lista da view de viagens para dropdowns.
        ///    Ordena pela data inicial.
        /// </para>
        ///
        /// <para>
        /// 📤 <b>RETORNO:</b><br/>
        ///    IEnumerable&lt;SelectListItem&gt; - Itens prontos para seleção em UI.
        /// </para>
        /// </summary>
        /// <returns>Lista de itens de seleção para viagens.</returns>
        public IEnumerable<SelectListItem> GetViewViagensListForDropDown()
        {
            return _db
                .ViewViagens.OrderBy(o => o.DataInicial)
                .Select(i => new SelectListItem()
                {
                    Text = i.DataInicial.ToString(),
                    Value = i.ViagemId.ToString(),
                });
            ;
            ;
        }

        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ MÉTODO: Update                                                                        │
        /// │ 🔗 RASTREABILIDADE:                                                                      │
        /// │    ⬅️ CHAMADO POR : Controllers, Services                                                 │
        /// │    ➡️ CHAMA       : DbContext.ViewViagens.FirstOrDefault, _db.Update, _db.SaveChanges     │
        /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
        ///
        /// <para>
        /// 🎯 <b>OBJETIVO:</b><br/>
        ///    Manter compatibilidade com o padrão de repositórios.
        ///    Views são somente leitura; operação não é recomendada.
        /// </para>
        ///
        /// <para>
        /// 📥 <b>PARÂMETROS:</b><br/>
        ///    viewViagens - Entidade com dados da view.
        /// </para>
        /// </summary>
        /// <param name="viewViagens">Entidade <see cref="ViewViagens"/>.</param>
        public new void Update(ViewViagens viewViagens)
        {
            var objFromDb = _db.ViewViagens.FirstOrDefault(s => s.ViagemId == viewViagens.ViagemId);

            _db.Update(viewViagens);
            _db.SaveChanges();
        }
    }
}
