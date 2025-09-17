using System.Linq.Expressions;

namespace DanCart.DataAccess.Repository.IRepository;

public interface IRepository<T> where T : class
{
    Task<IEnumerable<T>> GetRangeAsync(int page, int pageSize, Expression<Func<T, bool>>? filter = null, string? includeProperties = null);
    Task<T> GetAsync(Expression<Func<T, bool>> filter, string? includeProperties = null, bool tracked = false);
    Task AddAsync(T entity);
    void Remove(T entity);
    void RemoveRange(IEnumerable<T> entities);
}