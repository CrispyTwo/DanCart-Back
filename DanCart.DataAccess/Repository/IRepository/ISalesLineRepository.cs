using DanCart.Models;

namespace DanCart.DataAccess.Repository.IRepository;
public interface ISalesLineRepository : IRepository<SalesLine>
{
    void Update(SalesLine obj);
}
