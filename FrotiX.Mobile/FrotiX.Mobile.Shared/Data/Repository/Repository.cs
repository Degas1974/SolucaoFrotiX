using FrotiX.Mobile.Shared.Data.Repository.IRepository;
using FrotiX.Mobile.Shared.Services.IServices;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FrotiX.Mobile.Shared.Data
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly FrotiXDbContext _context;
        protected readonly IAlertaService _alerta;
        internal DbSet<T> dbSet;

        public Repository(FrotiXDbContext context, IAlertaService alerta)
        {
            _context = context;
            _alerta = alerta;
            dbSet = _context.Set<T>();
        }

        #region Métodos Assíncronos

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            try
            {
                return await dbSet.ToListAsync();
            }
            catch (Exception ex)
            {
                _ = _alerta.Erro("⚠️ Erro inesperado", "Erro ao buscar dados: " + ex.Message);
                return Enumerable.Empty<T>();
            }
        }

        public async Task<IEnumerable<T>> GetAllAsync(
            Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            string? includeProperties = null)
        {
            try
            {
                IQueryable<T> query = dbSet;

                if (filter != null)
                    query = query.Where(filter);

                if (!string.IsNullOrWhiteSpace(includeProperties))
                {
                    foreach (var includeProp in includeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries))
                    {
                        query = query.Include(includeProp.Trim());
                    }
                }

                return orderBy != null
                    ? await orderBy(query).ToListAsync()
                    : await query.ToListAsync();
            }
            catch (Exception ex)
            {
                _ = _alerta.Erro("⚠️ Erro inesperado", "Erro ao buscar lista filtrada: " + ex.Message);
                return Enumerable.Empty<T>();
            }
        }

        public async Task<List<TResult>> GetReducedAsync<TResult>(
            Expression<Func<T, TResult>> selector,
            Expression<Func<T, bool>>? filter = null,
            string? includeProperties = null)
        {
            try
            {
                IQueryable<T> query = dbSet;

                if (filter != null)
                    query = query.Where(filter);

                if (!string.IsNullOrWhiteSpace(includeProperties))
                {
                    foreach (var includeProp in includeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries))
                    {
                        query = query.Include(includeProp.Trim());
                    }
                }

                return await query.Select(selector).ToListAsync();
            }
            catch (Exception ex)
            {
                _ = _alerta.Erro("⚠️ Erro inesperado", "Erro ao buscar dados reduzidos: " + ex.Message);
                return new List<TResult>();
            }
        }

        public async Task<T?> GetFirstOrDefaultAsync(Expression<Func<T, bool>> filter)
        {
            try
            {
                return await dbSet.FirstOrDefaultAsync(filter);
            }
            catch (Exception ex)
            {
                _ = _alerta.Erro("⚠️ Erro inesperado", "Erro ao buscar item: " + ex.Message);
                return null;
            }
        }

        public async Task AddAsync(T entity)
        {
            try
            {
                await dbSet.AddAsync(entity);
            }
            catch (Exception ex)
            {
                _ = _alerta.Erro("⚠️ Erro inesperado", "Erro ao adicionar item assíncrono: " + ex.Message);
            }
        }

        #endregion

        #region Métodos Síncronos

        public IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, string? includeProperties = null)
        {
            try
            {
                IQueryable<T> query = dbSet;

                if (filter != null)
                    query = query.Where(filter);

                if (!string.IsNullOrWhiteSpace(includeProperties))
                {
                    foreach (var includeProp in includeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries))
                    {
                        query = query.Include(includeProp.Trim());
                    }
                }

                return query.ToList();
            }
            catch (Exception ex)
            {
                _ = _alerta.Erro("⚠️ Erro inesperado", "Erro ao buscar dados: " + ex.Message);
                return Enumerable.Empty<T>();
            }
        }

        public T? GetFirstOrDefault(Expression<Func<T, bool>> filter, string? includeProperties = null)
        {
            try
            {
                IQueryable<T> query = dbSet;

                if (!string.IsNullOrWhiteSpace(includeProperties))
                {
                    foreach (var includeProp in includeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries))
                    {
                        query = query.Include(includeProp.Trim());
                    }
                }

                return query.FirstOrDefault(filter);
            }
            catch (Exception ex)
            {
                _ = _alerta.Erro("⚠️ Erro inesperado", "Erro ao buscar item: " + ex.Message);
                return null;
            }
        }

        public void Add(T entity)
        {
            try
            {
                dbSet.Add(entity);
            }
            catch (Exception ex)
            {
                _ = _alerta.Erro("⚠️ Erro inesperado", "Erro ao adicionar item: " + ex.Message);
            }
        }

        public void Update(T entity)
        {
            try
            {
                dbSet.Update(entity);
            }
            catch (Exception ex)
            {
                _ = _alerta.Erro("⚠️ Erro inesperado", "Erro ao atualizar item: " + ex.Message);
            }
        }

        public void Remove(T entity)
        {
            try
            {
                dbSet.Remove(entity);
            }
            catch (Exception ex)
            {
                _ = _alerta.Erro("⚠️ Erro inesperado", "Erro ao remover item: " + ex.Message);
            }
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            try
            {
                dbSet.RemoveRange(entities);
            }
            catch (Exception ex)
            {
                _ = _alerta.Erro("⚠️ Erro inesperado", "Erro ao remover vários itens: " + ex.Message);
            }
        }

        #endregion
    }
}
