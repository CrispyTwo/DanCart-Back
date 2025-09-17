using DanCart.DataAccess.Data;
using DanCart.DataAccess.Repository.IRepository;
using DanCart.Models;

namespace DanCart.DataAccess.Repository;

public class StoreRepository(ApplicationDbContext db) : Repository<Store>(db), IStoreRepository
{
    public readonly ApplicationDbContext _db = db;
}
