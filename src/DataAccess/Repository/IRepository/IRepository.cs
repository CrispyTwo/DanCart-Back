using DanCart.DataAccess.Models.Utility;
using System.Linq.Expressions;

namespace DanCart.DataAccess.Repository.IRepository;

public interface IRepository<T> where T : class
{
    Task<T?> GetAsync(Expression<Func<T, bool>> filter, string? includeProperties = null, bool tracked = false);
    Task<long> GetTotalAsync(Expression<Func<T, bool>>? filter = null, string? includeProperties = null);
    Task AddAsync(T entity);
    void Remove(T entity);
    void RemoveRange(IEnumerable<T> entities);
    IQueryable<T> GetQuery(bool tracking = false);
}

public interface IUpdatableRepository<T> : IRepository<T> where T : class
{
    void Update(T entity);
}