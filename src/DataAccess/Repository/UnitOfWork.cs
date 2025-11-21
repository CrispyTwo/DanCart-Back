using DanCart.DataAccess.Data;
using DanCart.DataAccess.Repository.IRepository;

namespace DanCart.DataAccess.Repository;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _db;

    public IApplicationUserRepository ApplicationUser { get; private set; }
    public IRefreshTokenRepository RefreshToken { get; private set; }
    public ISalesOrderRepository SalesOrder { get; private set; }
    public ISalesLineRepository SalesLine { get; private set; }
    public IStoreRepository Store { get; private set; }
    public IProductRepository Product { get; private set; }

    public UnitOfWork(ApplicationDbContext db)
    {
        _db = db;
        Product = new ProductRepository(_db);
        ApplicationUser = new ApplicationUserRepository(_db);
        RefreshToken = new RefreshTokenRepository(_db);
        Store = new StoreRepository(_db);
        SalesOrder = new SalesOrderRepository(_db);
        SalesLine = new SalesLineRepository(_db);
    }
    public async Task SaveAsync()
    {
        await _db.SaveChangesAsync();
    }
    public async Task<bool> TrySaveAsync()
    {
        try
        {
            await _db.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }
}
