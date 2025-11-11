using DanCart.DataAccess.Data;
using DanCart.DataAccess.Repository.IRepository;
using DanCart.Models.SalesOrders;

namespace DanCart.DataAccess.Repository;

public class SalesLineRepository(ApplicationDbContext db) : Repository<SalesLine>(db), ISalesLineRepository
{
    public readonly ApplicationDbContext _db = db;
}