using DanCart.Models.Auth;
using DanCart.Models.Products;
using DanCart.Models.SalesOrders;

namespace DanCart.DataAccess.Repository.IRepository;

public interface IRefreshTokenRepository : IRepository<RefreshToken>;
public interface IApplicationUserRepository : IUpdatableRepository<ApplicationUser>;
public interface IProductRepository : IUpdatableRepository<Product>;
public interface IInventoryItemRepository : IUpdatableRepository<InventoryItem>;
public interface IShoppingCartRepository : IUpdatableRepository<ShoppingCart>;
public interface ISalesOrderRepository : IUpdatableRepository<SalesOrder>;
public interface ISalesLineRepository : IUpdatableRepository<SalesLine>;