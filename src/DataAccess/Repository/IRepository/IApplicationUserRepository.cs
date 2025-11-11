using DanCart.DataAccess.Models;
using DanCart.Models.Auth;

namespace DanCart.DataAccess.Repository.IRepository;
public interface IApplicationUserRepository : IRepository<ApplicationUser>
{
    public void Update(ApplicationUser applicationUser);
}