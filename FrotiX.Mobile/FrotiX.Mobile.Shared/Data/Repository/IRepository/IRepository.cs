using System.Linq.Expressions;

namespace FrotiX.Mobile.Shared.Data.Repository.IRepository
{
    public interface IRepository<T>
        where T : class
    {
        // Métodos Assíncronos
        Task<IEnumerable<T>> GetAllAsync();

        Task<IEnumerable<T>> GetAllAsync(
            Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            string? includeProperties = null
        );

        Task<List<TResult>> GetReducedAsync<TResult>(
            Expression<Func<T, TResult>> selector,
            Expression<Func<T, bool>>? filter = null,
            string? includeProperties = null
        );

        Task<T?> GetFirstOrDefaultAsync(Expression<Func<T, bool>> filter);

        Task AddAsync(T entity);

        // Métodos Síncronos
        IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, string? includeProperties = null);

        T? GetFirstOrDefault(Expression<Func<T, bool>> filter, string? includeProperties = null);

        void Add(T entity);
        void Update(T entity);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
    }
}
