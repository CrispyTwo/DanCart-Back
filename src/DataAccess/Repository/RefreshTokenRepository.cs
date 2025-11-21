using DanCart.DataAccess.Data;
using DanCart.DataAccess.Repository.IRepository;
using DanCart.Models.Auth;

namespace DanCart.DataAccess.Repository;

public class RefreshTokenRepository(ApplicationDbContext db) : Repository<RefreshToken>(db),  IRefreshTokenRepository
{
}
