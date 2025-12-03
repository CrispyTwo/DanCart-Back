using DanCart.DataAccess.Data;
using DanCart.DataAccess.Repository.IRepository;
using DanCart.Models.SalesOrders;

namespace DanCart.DataAccess.Repository;

public class ShoppingCartRepository(ApplicationDbContext db) : Repository<ShoppingCart>(db), IShoppingCartRepository
{
    public readonly ApplicationDbContext _db = db;
}
