using DanCart.Models.SalesOrders;

namespace DanCart.DataAccess.Repository.IRepository;
public interface ISalesLineRepository : IRepository<SalesLine>
{
    void Update(SalesLine obj);
}
