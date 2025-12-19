using DanCart.DataAccess.Models.Utility;
using System.Linq.Expressions;

namespace DanCart.DataAccess.Repository.IRepository;

public interface IRepository<T> where T : class
{
    Task<IEnumerable<T>> GetRangeAsync(Page page, GetRangeOptions<T>? options = null);
    Task<T?> GetAsync(Expression<Func<T, bool>> filter, string? includeProperties = null, bool tracked = false);
    Task<long> GetTotalAsync(Expression<Func<T, bool>>? filter = null);
    Task AddAsync(T entity);
    void Remove(T entity);
    void RemoveRange(IEnumerable<T> entities);
    IQueryable<T> GetQuery(bool tracking = false);
}