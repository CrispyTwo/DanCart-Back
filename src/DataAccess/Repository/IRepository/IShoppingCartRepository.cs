using DanCart.Models.SalesOrders;

namespace DanCart.DataAccess.Repository.IRepository;

public interface IShoppingCartRepository : IRepository<ShoppingCart>
{
    void Update(ShoppingCart obj);
}