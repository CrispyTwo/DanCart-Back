namespace DanCart.DataAccess.Repository.IRepository;
public interface IUnitOfWork
{
    IApplicationUserRepository ApplicationUser { get; }
    IRefreshTokenRepository RefreshToken { get; }
    ISalesOrderRepository SalesOrder { get; }
    ISalesLineRepository SalesLine { get; }
    IProductRepository Product { get; }
    IInventoryItemRepository Inventory { get; }
    IShoppingCartRepository ShoppingCart { get; }
    Task SaveAsync();
    Task<bool> TrySaveAsync();
}
