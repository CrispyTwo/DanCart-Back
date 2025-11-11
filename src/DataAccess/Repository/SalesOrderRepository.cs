using DanCart.DataAccess.Data;
using DanCart.DataAccess.Repository.IRepository;
using DanCart.Models.SalesOrders;

namespace DanCart.DataAccess.Repository;

public class SalesOrderRepository(ApplicationDbContext db) : Repository<SalesOrder>(db), ISalesOrderRepository
{
    public readonly ApplicationDbContext _db = db;
}
