/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: Repository.cs                                                                         ║
   ║ 📂 CAMINHO: Repository/                                                                            ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
   ║    Implementação genérica do padrão Repository para EF Core.                                       ║
   ║    Centraliza consultas, projeções e operações CRUD reutilizáveis.                                 ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 MÉTODOS DISPONÍVEIS:                                                                            ║
   ║    • Repository(DbContext db)                                                                      ║
   ║    • Get(object id)                                                                                ║
   ║    • GetFirstOrDefault(...) / GetFirstOrDefaultAsync(...)                                           ║
   ║    • GetAll(...) / GetAllAsync(...)                                                                 ║
   ║    • GetAllReduced<TResult>(...) / GetAllReducedIQueryable<TResult>(...)                           ║
   ║    • Add(T entity) / AddAsync(T entity)                                                             ║
   ║    • Update(T entity)                                                                              ║
   ║    • Remove(object id) / Remove(T entity)                                                           ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ ⚠️ OBSERVAÇÕES:                                                                                     ║
   ║    PrepareQuery controla AsTracking/AsNoTracking e includes via CSV.                               ║
   ║    O DbContext é NoTracking global; AsTracking é forçado quando necessário.                        ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using FrotiX.Repository.IRepository;

using Microsoft.EntityFrameworkCore;

namespace FrotiX.Repository
    {
    /// <summary>
    /// ╭───────────────────────────────────────────────────────────────────────────────────────────────╮
    /// │ 🎯 CLASSE: Repository<T>                                                                      │
    /// │ 📦 HERDA DE: IRepository<T>                                                                   │
    /// │ 🔌 IMPLEMENTA: IRepository<T>                                                                 │
    /// ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    ///
    /// Implementação genérica de repositório para EF Core.
    /// Fornece operações de consulta e persistência sem lógica específica de domínio.
    /// </summary>
    public class Repository<T> :IRepository<T>
        where T : class
        {
        protected readonly DbContext _db;
        protected readonly DbSet<T> dbSet;

        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ MÉTODO: Repository                                                                    │
        /// │ 🔗 RASTREABILIDADE:                                                                      │
        /// │    ⬅️ CHAMADO POR : Repositórios derivados, UnitOfWork                                    │
        /// │    ➡️ CHAMA       : DbContext.Set<T>()                                                    │
        /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
        ///
        /// <para>
        /// 🎯 <b>OBJETIVO:</b><br/>
        ///    Inicializar o repositório genérico com o contexto do banco de dados.
        /// </para>
        ///
        /// <para>
        /// 📥 <b>PARÂMETROS:</b><br/>
        ///    db - Contexto do banco de dados do EF Core.
        /// </para>
        /// </summary>
        /// <param name="db">Instância de <see cref="DbContext"/>.</param>
        public Repository(DbContext db)
            {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            dbSet = _db.Set<T>();
            }

        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ MÉTODO: PrepareQuery                                                                  │
        /// │ 🔗 RASTREABILIDADE:                                                                      │
        /// │    ⬅️ CHAMADO POR : GetFirstOrDefault, GetAll, GetAllAsync, GetAllReduced                 │
        /// │    ➡️ CHAMA       : AsNoTracking, AsTracking, Where, Include                              │
        /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
        ///
        /// <para>
        /// 🎯 <b>OBJETIVO:</b><br/>
        ///    Montar a query base aplicando filtro, includes (CSV) e modo de tracking.
        /// </para>
        ///
        /// <para>
        /// 📥 <b>PARÂMETROS:</b><br/>
        ///    filter - Filtro opcional a ser aplicado na consulta<br/>
        ///    includeProperties - Propriedades de navegação (CSV) para Include<br/>
        ///    asNoTracking - Define se a consulta será sem tracking
        /// </para>
        ///
        /// <para>
        /// 📤 <b>RETORNO:</b><br/>
        ///    IQueryable&lt;T&gt; - Consulta base montada com os critérios informados.
        /// </para>
        /// </summary>
        /// <param name="filter">Filtro opcional para a consulta.</param>
        /// <param name="includeProperties">Lista CSV de propriedades de navegação.</param>
        /// <param name="asNoTracking">Define se a consulta será sem tracking.</param>
        /// <returns>Consulta base pronta para composição.</returns>
        protected IQueryable<T> PrepareQuery(
            Expression<Func<T , bool>> filter = null ,
            string includeProperties = null ,
            bool asNoTracking = false
        )
            {
            IQueryable<T> query = dbSet;

            // Observação: o DbContext está configurado globalmente como NoTracking.
            // Precisamos forçar AsTracking quando asNoTracking == false para permitir persistência.
            if (asNoTracking)
                query = query.AsNoTracking();
            else
                query = query.AsTracking();

            if (filter != null)
                query = query.Where(filter);

            if (!string.IsNullOrWhiteSpace(includeProperties))
                {
                foreach (
                    var inc in includeProperties.Split(
                        new[] { ',' } ,
                        StringSplitOptions.RemoveEmptyEntries
                    )
                )
                    {
                    var path = inc.Trim();
                    if (!string.IsNullOrEmpty(path))
                        query = query.Include(path);
                    }
                }

            return query;
            }

        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ MÉTODO: Get                                                                           │
        /// │ 🔗 RASTREABILIDADE:                                                                      │
        /// │    ⬅️ CHAMADO POR : Repositórios derivados, Services, Controllers                         │
        /// │    ➡️ CHAMA       : DbSet.Find                                                           │
        /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
        ///
        /// <para>
        /// 🎯 <b>OBJETIVO:</b><br/>
        ///    Buscar uma entidade pela chave primária usando Find().
        /// </para>
        ///
        /// <para>
        /// 📥 <b>PARÂMETROS:</b><br/>
        ///    id - Chave primária da entidade.
        /// </para>
        ///
        /// <para>
        /// 📤 <b>RETORNO:</b><br/>
        ///    T - Entidade encontrada ou null quando inexistente.
        /// </para>
        /// </summary>
        /// <param name="id">Chave primária.</param>
        /// <returns>Entidade encontrada ou null.</returns>
        public T Get(object id)
            {
            if (id == null)
                return null;
            return dbSet.Find(id);
            }

        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ MÉTODO: GetFirstOrDefault                                                             │
        /// │ 🔗 RASTREABILIDADE:                                                                      │
        /// │    ⬅️ CHAMADO POR : Repositórios derivados, Services, Controllers                         │
        /// │    ➡️ CHAMA       : PrepareQuery, FirstOrDefault                                         │
        /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
        ///
        /// <para>
        /// 🎯 <b>OBJETIVO:</b><br/>
        ///    Retornar o primeiro registro que atende ao filtro informado.
        ///    Executa consulta em modo NoTracking por padrão.
        /// </para>
        ///
        /// <para>
        /// 📥 <b>PARÂMETROS:</b><br/>
        ///    filter - Filtro opcional<br/>
        ///    includeProperties - Propriedades de navegação (CSV)
        /// </para>
        ///
        /// <para>
        /// 📤 <b>RETORNO:</b><br/>
        ///    T - Primeiro registro encontrado ou null.
        /// </para>
        /// </summary>
        /// <param name="filter">Filtro opcional.</param>
        /// <param name="includeProperties">Includes em CSV.</param>
        /// <returns>Registro encontrado ou null.</returns>
        public T GetFirstOrDefault(
            Expression<Func<T , bool>> filter = null ,
            string includeProperties = null
        )
            {
            try
                {
                return PrepareQuery(filter , includeProperties , asNoTracking: true).FirstOrDefault();
                }
            catch (InvalidOperationException ex) when (ex.Message.Contains("second operation"))
                {
                // Erro de concorrência - tentar novamente com um novo contexto
                // ou simplesmente retornar null e deixar o chamador lidar
                return null;
                }
            catch (Exception)
                {
                // Logar o erro para debug
                throw; // Re-lançar o erro para não esconder problemas
                }
            }

        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ MÉTODO: GetFirstOrDefaultAsync                                                        │
        /// │ 🔗 RASTREABILIDADE:                                                                      │
        /// │    ⬅️ CHAMADO POR : Repositórios derivados, Services, Controllers                         │
        /// │    ➡️ CHAMA       : PrepareQuery, FirstOrDefaultAsync                                    │
        /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
        ///
        /// <para>
        /// 🎯 <b>OBJETIVO:</b><br/>
        ///    Retornar o primeiro registro que atende ao filtro informado (assíncrono).
        ///    Executa consulta em modo NoTracking por padrão.
        /// </para>
        ///
        /// <para>
        /// 📥 <b>PARÂMETROS:</b><br/>
        ///    filter - Filtro opcional<br/>
        ///    includeProperties - Propriedades de navegação (CSV)
        /// </para>
        ///
        /// <para>
        /// 📤 <b>RETORNO:</b><br/>
        ///    Task&lt;T&gt; - Primeiro registro encontrado ou null.
        /// </para>
        /// </summary>
        /// <param name="filter">Filtro opcional.</param>
        /// <param name="includeProperties">Includes em CSV.</param>
        /// <returns>Registro encontrado ou null.</returns>
        public async Task<T> GetFirstOrDefaultAsync(
            Expression<Func<T , bool>> filter = null ,
            string includeProperties = null
        )
            {
            try
                {
                return await PrepareQuery(filter , includeProperties , asNoTracking: true)
                    .FirstOrDefaultAsync();
                }
            catch (InvalidOperationException ex) when (ex.Message.Contains("second operation"))
                {
                // Erro de concorrência - tentar novamente com um novo contexto
                // ou simplesmente retornar null e deixar o chamador lidar
                return null;
                }
            catch (Exception)
                {
                // Logar o erro para debug
                throw; // Re-lançar o erro para não esconder problemas
                }
            }

        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ MÉTODO: GetAll                                                                        │
        /// │ 🔗 RASTREABILIDADE:                                                                      │
        /// │    ⬅️ CHAMADO POR : Repositórios derivados, Services, Controllers                         │
        /// │    ➡️ CHAMA       : PrepareQuery, OrderBy, ToList                                        │
        /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
        ///
        /// <para>
        /// 🎯 <b>OBJETIVO:</b><br/>
        ///    Retornar lista de entidades com filtro, ordenação e includes opcionais.
        /// </para>
        ///
        /// <para>
        /// 📥 <b>PARÂMETROS:</b><br/>
        ///    filter - Filtro opcional<br/>
        ///    orderBy - Função de ordenação opcional<br/>
        ///    includeProperties - Propriedades de navegação (CSV)<br/>
        ///    asNoTracking - Define se a consulta será sem tracking
        /// </para>
        ///
        /// <para>
        /// 📤 <b>RETORNO:</b><br/>
        ///    IEnumerable&lt;T&gt; - Lista materializada.
        /// </para>
        /// </summary>
        /// <param name="filter">Filtro opcional.</param>
        /// <param name="orderBy">Ordenação opcional.</param>
        /// <param name="includeProperties">Includes em CSV.</param>
        /// <param name="asNoTracking">Define se a consulta será sem tracking.</param>
        /// <returns>Lista de entidades.</returns>
        public IEnumerable<T> GetAll(
            Expression<Func<T , bool>> filter = null ,
            Func<IQueryable<T> , IOrderedQueryable<T>> orderBy = null ,
            string includeProperties = null ,
            bool asNoTracking = true
        )
            {
            var q = PrepareQuery(filter , includeProperties , asNoTracking);
            if (orderBy != null)
                q = orderBy(q);
            return q.ToList();
            }

        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ MÉTODO: GetAllAsync                                                                   │
        /// │ 🔗 RASTREABILIDADE:                                                                      │
        /// │    ⬅️ CHAMADO POR : Repositórios derivados, Services, Controllers                         │
        /// │    ➡️ CHAMA       : PrepareQuery, OrderBy, Take, ToListAsync                             │
        /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
        ///
        /// <para>
        /// 🎯 <b>OBJETIVO:</b><br/>
        ///    Retornar lista de entidades de forma assíncrona.
        ///    Suporta filtro, ordenação, includes e limite de itens.
        /// </para>
        ///
        /// <para>
        /// 📥 <b>PARÂMETROS:</b><br/>
        ///    filter - Filtro opcional<br/>
        ///    orderBy - Função de ordenação opcional<br/>
        ///    includeProperties - Propriedades de navegação (CSV)<br/>
        ///    asNoTracking - Define se a consulta será sem tracking<br/>
        ///    take - Limite opcional de registros
        /// </para>
        ///
        /// <para>
        /// 📤 <b>RETORNO:</b><br/>
        ///    Task&lt;IEnumerable&lt;T&gt;&gt; - Lista materializada.
        /// </para>
        /// </summary>
        /// <param name="filter">Filtro opcional.</param>
        /// <param name="orderBy">Ordenação opcional.</param>
        /// <param name="includeProperties">Includes em CSV.</param>
        /// <param name="asNoTracking">Define se a consulta será sem tracking.</param>
        /// <param name="take">Limite opcional de registros.</param>
        /// <returns>Lista de entidades.</returns>
        public async Task<IEnumerable<T>> GetAllAsync(
            Expression<Func<T , bool>> filter = null ,
            Func<IQueryable<T> , IOrderedQueryable<T>> orderBy = null ,
            string includeProperties = null ,
            bool asNoTracking = true ,
            int? take = null
        )
            {
            var q = PrepareQuery(filter , includeProperties , asNoTracking);
            if (orderBy != null)
                q = orderBy(q);
            if (take.HasValue)
                q = q.Take(take.Value);
            return await q.ToListAsync();
            }

        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ MÉTODO: GetAllReduced                                                                 │
        /// │ 🔗 RASTREABILIDADE:                                                                      │
        /// │    ⬅️ CHAMADO POR : Repositórios derivados, Services, Controllers                         │
        /// │    ➡️ CHAMA       : GetAllReducedIQueryable, ToList                                      │
        /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
        ///
        /// <para>
        /// 🎯 <b>OBJETIVO:</b><br/>
        ///    Retornar lista projetada e materializada (compat com páginas antigas).
        /// </para>
        ///
        /// <para>
        /// 📥 <b>PARÂMETROS:</b><br/>
        ///    selector - Expressão de projeção (DTO)<br/>
        ///    filter - Filtro opcional<br/>
        ///    orderBy - Função de ordenação opcional<br/>
        ///    includeProperties - Propriedades de navegação (CSV)<br/>
        ///    asNoTracking - Define se a consulta será sem tracking
        /// </para>
        ///
        /// <para>
        /// 📤 <b>RETORNO:</b><br/>
        ///    IEnumerable&lt;TResult&gt; - Lista projetada e materializada.
        /// </para>
        /// </summary>
        /// <param name="selector">Expressão de projeção.</param>
        /// <param name="filter">Filtro opcional.</param>
        /// <param name="orderBy">Ordenação opcional.</param>
        /// <param name="includeProperties">Includes em CSV.</param>
        /// <param name="asNoTracking">Define se a consulta será sem tracking.</param>
        /// <returns>Lista projetada e materializada.</returns>
        public IEnumerable<TResult> GetAllReduced<TResult>(
            Expression<Func<T , TResult>> selector ,
            Expression<Func<T , bool>> filter = null ,
            Func<IQueryable<T> , IOrderedQueryable<T>> orderBy = null ,
            string includeProperties = null ,
            bool asNoTracking = true
        )
            {
            return GetAllReducedIQueryable(
                    selector ,
                    filter ,
                    orderBy ,
                    includeProperties ,
                    asNoTracking
                )
                .ToList();
            }

        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ MÉTODO: GetAllReducedIQueryable                                                       │
        /// │ 🔗 RASTREABILIDADE:                                                                      │
        /// │    ⬅️ CHAMADO POR : GetAllReduced, Repositórios derivados, Services                       │
        /// │    ➡️ CHAMA       : PrepareQuery, OrderBy, Select                                        │
        /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
        ///
        /// <para>
        /// 🎯 <b>OBJETIVO:</b><br/>
        ///    Retornar consulta projetada como IQueryable (lazy).
        /// </para>
        ///
        /// <para>
        /// 📥 <b>PARÂMETROS:</b><br/>
        ///    selector - Expressão de projeção (DTO)<br/>
        ///    filter - Filtro opcional<br/>
        ///    orderBy - Função de ordenação opcional<br/>
        ///    includeProperties - Propriedades de navegação (CSV)<br/>
        ///    asNoTracking - Define se a consulta será sem tracking
        /// </para>
        ///
        /// <para>
        /// 📤 <b>RETORNO:</b><br/>
        ///    IQueryable&lt;TResult&gt; - Consulta projetada.
        /// </para>
        /// </summary>
        /// <param name="selector">Expressão de projeção.</param>
        /// <param name="filter">Filtro opcional.</param>
        /// <param name="orderBy">Ordenação opcional.</param>
        /// <param name="includeProperties">Includes em CSV.</param>
        /// <param name="asNoTracking">Define se a consulta será sem tracking.</param>
        /// <returns>Consulta projetada.</returns>
        public IQueryable<TResult> GetAllReducedIQueryable<TResult>(
            Expression<Func<T , TResult>> selector ,
            Expression<Func<T , bool>> filter = null ,
            Func<IQueryable<T> , IOrderedQueryable<T>> orderBy = null ,
            string includeProperties = null ,
            bool asNoTracking = true
        )
            {
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            var q = PrepareQuery(filter , includeProperties , asNoTracking);
            if (orderBy != null)
                q = orderBy(q);

            return q.Select(selector);
            }

        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ MÉTODO: Add                                                                           │
        /// │ 🔗 RASTREABILIDADE:                                                                      │
        /// │    ⬅️ CHAMADO POR : Repositórios derivados, Services, Controllers                         │
        /// │    ➡️ CHAMA       : DbSet.Add                                                            │
        /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
        ///
        /// <para>
        /// 🎯 <b>OBJETIVO:</b><br/>
        ///    Adicionar uma entidade ao contexto de forma síncrona.
        /// </para>
        ///
        /// <para>
        /// 📥 <b>PARÂMETROS:</b><br/>
        ///    entity - Entidade a ser adicionada.
        /// </para>
        /// </summary>
        /// <param name="entity">Entidade a adicionar.</param>
        public void Add(T entity)
            {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            dbSet.Add(entity);
            }

        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ MÉTODO: AddAsync                                                                      │
        /// │ 🔗 RASTREABILIDADE:                                                                      │
        /// │    ⬅️ CHAMADO POR : Repositórios derivados, Services, Controllers                         │
        /// │    ➡️ CHAMA       : DbSet.AddAsync                                                       │
        /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
        ///
        /// <para>
        /// 🎯 <b>OBJETIVO:</b><br/>
        ///    Adicionar uma entidade ao contexto de forma assíncrona.
        /// </para>
        ///
        /// <para>
        /// 📥 <b>PARÂMETROS:</b><br/>
        ///    entity - Entidade a ser adicionada.
        /// </para>
        /// </summary>
        /// <param name="entity">Entidade a adicionar.</param>
        /// <returns>Task representando a operação assíncrona.</returns>
        public async Task AddAsync(T entity)
            {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            await dbSet.AddAsync(entity);
            }

        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ MÉTODO: Update                                                                        │
        /// │ 🔗 RASTREABILIDADE:                                                                      │
        /// │    ⬅️ CHAMADO POR : Repositórios derivados, Services, Controllers                         │
        /// │    ➡️ CHAMA       : DbSet.Update                                                         │
        /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
        ///
        /// <para>
        /// 🎯 <b>OBJETIVO:</b><br/>
        ///    Atualizar uma entidade no contexto.
        /// </para>
        ///
        /// <para>
        /// 📥 <b>PARÂMETROS:</b><br/>
        ///    entity - Entidade a ser atualizada.
        /// </para>
        /// </summary>
        /// <param name="entity">Entidade a atualizar.</param>
        public new void Update(T entity)
            {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            dbSet.Update(entity);
            }

        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ MÉTODO: Remove                                                                        │
        /// │ 🔗 RASTREABILIDADE:                                                                      │
        /// │    ⬅️ CHAMADO POR : Repositórios derivados, Services, Controllers                         │
        /// │    ➡️ CHAMA       : DbSet.Find, DbSet.Remove                                              │
        /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
        ///
        /// <para>
        /// 🎯 <b>OBJETIVO:</b><br/>
        ///    Remover uma entidade a partir da chave primária.
        /// </para>
        ///
        /// <para>
        /// 📥 <b>PARÂMETROS:</b><br/>
        ///    id - Chave primária da entidade.
        /// </para>
        /// </summary>
        /// <param name="id">Chave primária da entidade.</param>
        public void Remove(object id)
            {
            if (id == null)
                return;
            var entity = dbSet.Find(id);
            if (entity != null)
                dbSet.Remove(entity);
            }

        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ MÉTODO: Remove                                                                        │
        /// │ 🔗 RASTREABILIDADE:                                                                      │
        /// │    ⬅️ CHAMADO POR : Repositórios derivados, Services, Controllers                         │
        /// │    ➡️ CHAMA       : DbSet.Remove                                                         │
        /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
        ///
        /// <para>
        /// 🎯 <b>OBJETIVO:</b><br/>
        ///    Remover uma entidade diretamente pelo objeto.
        /// </para>
        ///
        /// <para>
        /// 📥 <b>PARÂMETROS:</b><br/>
        ///    entity - Entidade a ser removida.
        /// </para>
        /// </summary>
        /// <param name="entity">Entidade a remover.</param>
        public void Remove(T entity)
            {
            if (entity == null)
                return;
            dbSet.Remove(entity);
            }
        }
    }
