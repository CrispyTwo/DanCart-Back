using DanCart.Models;

namespace DanCart.DataAccess.Repository.IRepository;

public interface IStoreRepository : IRepository<Store>
{
    void Update(Store obj);
}
