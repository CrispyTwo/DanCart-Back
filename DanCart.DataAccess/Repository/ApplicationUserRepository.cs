using DanCart.DataAccess.Data;
using DanCart.DataAccess.Repository.IRepository;
using DanCart.Models;

namespace DanCart.DataAccess.Repository;

public class ApplicationUserRepository(ApplicationDbContext db) : Repository<ApplicationUser>(db), IApplicationUserRepository
{
    public readonly ApplicationDbContext _db = db;
}
