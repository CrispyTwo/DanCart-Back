using DanCart.Models.SalesOrders;

namespace DanCart.DataAccess.Repository.IRepository;

public interface ISalesOrderRepository : IRepository<SalesOrder>
{
    void Update(SalesOrder obj);
}
