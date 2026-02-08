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
    
    // ╭───────────────────────────────────────────────────────────────────────────────────────────────╮
    // │ 🎯 CLASSE: Repository                                                                      │
    // │ 📦 HERDA DE: IRepository                                                                   │
    // │ 🔌 IMPLEMENTA: IRepository                                                                 │
    // ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    
    // Implementação genérica de repositório para EF Core.
    // Fornece operações de consulta e persistência sem lógica específica de domínio.
    
    public class Repository<T> :IRepository<T>
        where T : class
        {
        protected readonly DbContext _db;
        protected readonly DbSet<T> dbSet;

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: Repository                                                                    │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Repositórios derivados, UnitOfWork                                    │
        // │    ➡️ CHAMA       : DbContext.Set()                                                    │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Inicializar o repositório genérico com o contexto do banco de dados.
        
        
        
        // 📥 PARÂMETROS:
        // db - Contexto do banco de dados do EF Core.
        
        
        // Param db: Instância de <see cref="DbContext"/>.
        public Repository(DbContext db)
            {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            dbSet = _db.Set<T>();
            }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: PrepareQuery                                                                  │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : GetFirstOrDefault, GetAll, GetAllAsync, GetAllReduced                 │
        // │    ➡️ CHAMA       : AsNoTracking, AsTracking, Where, Include                              │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Montar a query base aplicando filtro, includes (CSV) e modo de tracking.
        
        
        
        // 📥 PARÂMETROS:
        // filter - Filtro opcional a ser aplicado na consulta
        // includeProperties - Propriedades de navegação (CSV) para Include
        // asNoTracking - Define se a consulta será sem tracking
        
        
        
        // 📤 RETORNO:
        // IQueryable&lt;T&gt; - Consulta base montada com os critérios informados.
        
        
        // Param filter: Filtro opcional para a consulta.
        // Param includeProperties: Lista CSV de propriedades de navegação.
        // Param asNoTracking: Define se a consulta será sem tracking.
        // Returns: Consulta base pronta para composição.
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

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: Get                                                                           │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Repositórios derivados, Services, Controllers                         │
        // │    ➡️ CHAMA       : DbSet.Find                                                           │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Buscar uma entidade pela chave primária usando Find().
        
        
        
        // 📥 PARÂMETROS:
        // id - Chave primária da entidade.
        
        
        
        // 📤 RETORNO:
        // T - Entidade encontrada ou null quando inexistente.
        
        
        // Param id: Chave primária.
        // Returns: Entidade encontrada ou null.
        public T Get(object id)
            {
            if (id == null)
                return null;
            return dbSet.Find(id);
            }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: GetFirstOrDefault                                                             │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Repositórios derivados, Services, Controllers                         │
        // │    ➡️ CHAMA       : PrepareQuery, FirstOrDefault                                         │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Retornar o primeiro registro que atende ao filtro informado.
        // Executa consulta em modo NoTracking por padrão.
        
        
        
        // 📥 PARÂMETROS:
        // filter - Filtro opcional
        // includeProperties - Propriedades de navegação (CSV)
        
        
        
        // 📤 RETORNO:
        // T - Primeiro registro encontrado ou null.
        
        
        // Param filter: Filtro opcional.
        // Param includeProperties: Includes em CSV.
        // Returns: Registro encontrado ou null.
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

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: GetFirstOrDefaultAsync                                                        │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Repositórios derivados, Services, Controllers                         │
        // │    ➡️ CHAMA       : PrepareQuery, FirstOrDefaultAsync                                    │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Retornar o primeiro registro que atende ao filtro informado (assíncrono).
        // Executa consulta em modo NoTracking por padrão.
        
        
        
        // 📥 PARÂMETROS:
        // filter - Filtro opcional
        // includeProperties - Propriedades de navegação (CSV)
        
        
        
        // 📤 RETORNO:
        // Task&lt;T&gt; - Primeiro registro encontrado ou null.
        
        
        // Param filter: Filtro opcional.
        // Param includeProperties: Includes em CSV.
        // Returns: Registro encontrado ou null.
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

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: GetAll                                                                        │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Repositórios derivados, Services, Controllers                         │
        // │    ➡️ CHAMA       : PrepareQuery, OrderBy, ToList                                        │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Retornar lista de entidades com filtro, ordenação e includes opcionais.
        
        
        
        // 📥 PARÂMETROS:
        // filter - Filtro opcional
        // orderBy - Função de ordenação opcional
        // includeProperties - Propriedades de navegação (CSV)
        // asNoTracking - Define se a consulta será sem tracking
        
        
        
        // 📤 RETORNO:
        // IEnumerable&lt;T&gt; - Lista materializada.
        
        
        // Param filter: Filtro opcional.
        // Param orderBy: Ordenação opcional.
        // Param includeProperties: Includes em CSV.
        // Param asNoTracking: Define se a consulta será sem tracking.
        // Returns: Lista de entidades.
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

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: GetAllAsync                                                                   │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Repositórios derivados, Services, Controllers                         │
        // │    ➡️ CHAMA       : PrepareQuery, OrderBy, Take, ToListAsync                             │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Retornar lista de entidades de forma assíncrona.
        // Suporta filtro, ordenação, includes e limite de itens.
        
        
        
        // 📥 PARÂMETROS:
        // filter - Filtro opcional
        // orderBy - Função de ordenação opcional
        // includeProperties - Propriedades de navegação (CSV)
        // asNoTracking - Define se a consulta será sem tracking
        // take - Limite opcional de registros
        
        
        
        // 📤 RETORNO:
        // Task&lt;IEnumerable&lt;T&gt;&gt; - Lista materializada.
        
        
        // Param filter: Filtro opcional.
        // Param orderBy: Ordenação opcional.
        // Param includeProperties: Includes em CSV.
        // Param asNoTracking: Define se a consulta será sem tracking.
        // Param take: Limite opcional de registros.
        // Returns: Lista de entidades.
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

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: GetAllReduced                                                                 │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Repositórios derivados, Services, Controllers                         │
        // │    ➡️ CHAMA       : GetAllReducedIQueryable, ToList                                      │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Retornar lista projetada e materializada (compat com páginas antigas).
        
        
        
        // 📥 PARÂMETROS:
        // selector - Expressão de projeção (DTO)
        // filter - Filtro opcional
        // orderBy - Função de ordenação opcional
        // includeProperties - Propriedades de navegação (CSV)
        // asNoTracking - Define se a consulta será sem tracking
        
        
        
        // 📤 RETORNO:
        // IEnumerable&lt;TResult&gt; - Lista projetada e materializada.
        
        
        // Param selector: Expressão de projeção.
        // Param filter: Filtro opcional.
        // Param orderBy: Ordenação opcional.
        // Param includeProperties: Includes em CSV.
        // Param asNoTracking: Define se a consulta será sem tracking.
        // Returns: Lista projetada e materializada.
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

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: GetAllReducedIQueryable                                                       │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : GetAllReduced, Repositórios derivados, Services                       │
        // │    ➡️ CHAMA       : PrepareQuery, OrderBy, Select                                        │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Retornar consulta projetada como IQueryable (lazy).
        
        
        
        // 📥 PARÂMETROS:
        // selector - Expressão de projeção (DTO)
        // filter - Filtro opcional
        // orderBy - Função de ordenação opcional
        // includeProperties - Propriedades de navegação (CSV)
        // asNoTracking - Define se a consulta será sem tracking
        
        
        
        // 📤 RETORNO:
        // IQueryable&lt;TResult&gt; - Consulta projetada.
        
        
        // Param selector: Expressão de projeção.
        // Param filter: Filtro opcional.
        // Param orderBy: Ordenação opcional.
        // Param includeProperties: Includes em CSV.
        // Param asNoTracking: Define se a consulta será sem tracking.
        // Returns: Consulta projetada.
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

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: Add                                                                           │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Repositórios derivados, Services, Controllers                         │
        // │    ➡️ CHAMA       : DbSet.Add                                                            │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Adicionar uma entidade ao contexto de forma síncrona.
        
        
        
        // 📥 PARÂMETROS:
        // entity - Entidade a ser adicionada.
        
        
        // Param entity: Entidade a adicionar.
        public void Add(T entity)
            {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            dbSet.Add(entity);
            }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: AddAsync                                                                      │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Repositórios derivados, Services, Controllers                         │
        // │    ➡️ CHAMA       : DbSet.AddAsync                                                       │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Adicionar uma entidade ao contexto de forma assíncrona.
        
        
        
        // 📥 PARÂMETROS:
        // entity - Entidade a ser adicionada.
        
        
        // Param entity: Entidade a adicionar.
        // Returns: Task representando a operação assíncrona.
        public async Task AddAsync(T entity)
            {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            await dbSet.AddAsync(entity);
            }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: Update                                                                        │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Repositórios derivados, Services, Controllers                         │
        // │    ➡️ CHAMA       : DbSet.Update                                                         │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Atualizar uma entidade no contexto.
        
        
        
        // 📥 PARÂMETROS:
        // entity - Entidade a ser atualizada.
        
        
        // Param entity: Entidade a atualizar.
        public new void Update(T entity)
            {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            dbSet.Update(entity);
            }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: Remove                                                                        │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Repositórios derivados, Services, Controllers                         │
        // │    ➡️ CHAMA       : DbSet.Find, DbSet.Remove                                              │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Remover uma entidade a partir da chave primária.
        
        
        
        // 📥 PARÂMETROS:
        // id - Chave primária da entidade.
        
        
        // Param id: Chave primária da entidade.
        public void Remove(object id)
            {
            if (id == null)
                return;
            var entity = dbSet.Find(id);
            if (entity != null)
                dbSet.Remove(entity);
            }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: Remove                                                                        │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Repositórios derivados, Services, Controllers                         │
        // │    ➡️ CHAMA       : DbSet.Remove                                                         │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Remover uma entidade diretamente pelo objeto.
        
        
        
        // 📥 PARÂMETROS:
        // entity - Entidade a ser removida.
        
        
        // Param entity: Entidade a remover.
        public void Remove(T entity)
            {
            if (entity == null)
                return;
            dbSet.Remove(entity);
            }
        }
    }
