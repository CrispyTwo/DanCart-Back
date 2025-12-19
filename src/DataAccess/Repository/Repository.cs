using DanCart.DataAccess.Data;
using DanCart.DataAccess.Extensions;
using DanCart.DataAccess.Models.Utility;
using DanCart.DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace DanCart.DataAccess.Repository;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly ApplicationDbContext _db;
    internal DbSet<T> _dbSet;
    public Repository(ApplicationDbContext db)
    {
        _db = db;
        _dbSet = _db.Set<T>();
    }
    public async Task AddAsync(T entity) => await _dbSet.AddAsync(entity);
    public async Task<long> GetTotalAsync(Expression<Func<T, bool>>? filter = null)
    {
        IQueryable<T> query = _dbSet.AsNoTracking();
        if (filter != null) query = query.Where(filter);
        return await query.LongCountAsync();
    }
    public async Task<T?> GetAsync(Expression<Func<T, bool>> filter, string? includeProperties = null, bool tracked = false)
    {
        IQueryable<T> query = tracked ? _dbSet : _dbSet.AsNoTracking();

        query = query.Where(filter);
        if (!string.IsNullOrEmpty(includeProperties))
        {
            foreach (var includeProp in includeProperties.Split([','], StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProp);
            }
        }

        return await query.FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<T>> GetRangeAsync(Page page, GetRangeOptions<T>? options = null)
    {
        IQueryable<T> query = _dbSet;
        if (options != null)
        {
            if (options.Filter != null) query = query.Where(options.Filter);

            query = query.ApplySorting(options.Sortings);
            foreach (var property in options.IncludeProperties)
                query = query.Include(property.Trim());
        }

        return await query.Paginate(page).ToListAsync();
    }

    public void Update(T entity) => _dbSet.Update(entity);
    public void Remove(T entity) => _dbSet.Remove(entity);
    public void RemoveRange(IEnumerable<T> entities) => _dbSet.RemoveRange(entities);
    public IQueryable<T> GetQuery(bool tracking) => tracking ? _dbSet.AsTracking() : _dbSet.AsNoTracking();
}