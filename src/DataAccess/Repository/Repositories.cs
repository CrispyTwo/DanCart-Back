using DanCart.DataAccess.Data;
using DanCart.DataAccess.Repository.IRepository;
using DanCart.Models.Auth;
using DanCart.Models.Products;
using DanCart.Models.SalesOrders;

namespace DanCart.DataAccess.Repository;

public class RefreshTokenRepository(ApplicationDbContext db) : Repository<RefreshToken>(db), IRefreshTokenRepository;
public class ApplicationUserRepository(ApplicationDbContext db) : Repository<ApplicationUser>(db), IApplicationUserRepository;
public class ProductRepository(ApplicationDbContext db) : Repository<Product>(db), IProductRepository;
public class InventoryItemRepository(ApplicationDbContext db) : Repository<InventoryItem>(db), IInventoryItemRepository;
public class ShoppingCartRepository(ApplicationDbContext db) : Repository<ShoppingCart>(db), IShoppingCartRepository;
public class SalesOrderRepository(ApplicationDbContext db) : Repository<SalesOrder>(db), ISalesOrderRepository;
public class SalesLineRepository(ApplicationDbContext db) : Repository<SalesLine>(db), ISalesLineRepository;