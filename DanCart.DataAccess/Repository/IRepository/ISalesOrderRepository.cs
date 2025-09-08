using DanCart.Models;

namespace DanCart.DataAccess.Repository.IRepository;

public interface ISalesOrderRepository : IRepository<SalesOrder>
{
    void Update(SalesOrder obj);
}
