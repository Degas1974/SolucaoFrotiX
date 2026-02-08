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
    
    // ╭───────────────────────────────────────────────────────────────────────────────────────────────╮
    // │ 🎯 CLASSE: ViewViagensRepository                                                              │
    // │ 📦 HERDA DE: Repository                                                          │
    // │ 🔌 IMPLEMENTA: IViewViagensRepository                                                         │
    // ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    
    // Repositório responsável pela view de viagens.
    // Fornece paginação genérica e listagens para UI.
    
    public class ViewViagensRepository : Repository<ViewViagens>, IViewViagensRepository
    {
        private new readonly FrotiXDbContext _db;

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: ViewViagensRepository                                                        │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : UnitOfWork, Services, Controllers                                     │
        // │    ➡️ CHAMA       : base(db)                                                             │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Inicializar o repositório com o contexto do banco de dados.
        
        
        
        // 📥 PARÂMETROS:
        // db - Contexto do banco de dados da aplicação.
        
        
        // Param db: Instância de <see cref="FrotiXDbContext"/>.
        public ViewViagensRepository(FrotiXDbContext db)
            : base(db)
        {
            _db = db;
        }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: GetPaginatedAsync                                                           │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Services, Controllers                                                 │
        // │    ➡️ CHAMA       : DbContext.ViewViagens, AsNoTracking, Where, CountAsync, Skip, Take   │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Obter dados paginados da view de viagens com projeção genérica.
        
        
        
        // 📥 PARÂMETROS:
        // selector - Expressão de seleção/projeção
        // filter - Filtro aplicado à consulta
        // page - Página atual (1-based)
        // pageSize - Quantidade de registros por página
        
        
        
        // 📤 RETORNO:
        // Task&lt;(List&lt;T&gt; Items, int TotalCount)&gt; - Itens paginados e total de registros.
        
        
        // Param selector: Expressão de projeção.
        // Param filter: Filtro aplicado à consulta.
        // Param page: Página atual (1-based).
        // Param pageSize: Tamanho da página.
        // Returns: Itens paginados e total de registros.
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

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: GetViewViagensListForDropDown                                                │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Controllers, Services, UI (DropDowns)                                │
        // │    ➡️ CHAMA       : DbContext.ViewViagens, OrderBy, Select                               │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Obter lista da view de viagens para dropdowns.
        // Ordena pela data inicial.
        
        
        
        // 📤 RETORNO:
        // IEnumerable&lt;SelectListItem&gt; - Itens prontos para seleção em UI.
        
        
        // Returns: Lista de itens de seleção para viagens.
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

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: Update                                                                        │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Controllers, Services                                                 │
        // │    ➡️ CHAMA       : DbContext.ViewViagens.FirstOrDefault, _db.Update, _db.SaveChanges     │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Manter compatibilidade com o padrão de repositórios.
        // Views são somente leitura; operação não é recomendada.
        
        
        
        // 📥 PARÂMETROS:
        // viewViagens - Entidade com dados da view.
        
        
        // Param viewViagens: Entidade <see cref="ViewViagens"/>.
        public new void Update(ViewViagens viewViagens)
        {
            var objFromDb = _db.ViewViagens.FirstOrDefault(s => s.ViagemId == viewViagens.ViagemId);

            _db.Update(viewViagens);
            _db.SaveChanges();
        }
    }
}
