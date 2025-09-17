using DanCart.Models;

namespace DanCart.DataAccess.Repository.IRepository;
public interface IApplicationUserRepository : IRepository<ApplicationUser>
{
    public void Update(ApplicationUser applicationUser);
}