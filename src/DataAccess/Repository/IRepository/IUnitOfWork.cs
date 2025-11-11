namespace DanCart.DataAccess.Repository.IRepository;
public interface IUnitOfWork
{
    IApplicationUserRepository ApplicationUser { get; }
    ISalesOrderRepository SalesOrder { get; }
    ISalesLineRepository SalesLine { get; }
    IProductRepository Product { get; }
    IStoreRepository Store { get; }
    Task SaveAsync();
    Task<bool> TrySaveAsync();
}
