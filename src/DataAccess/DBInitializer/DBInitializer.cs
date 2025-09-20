using DanCart.DataAccess.Data;
using DanCart.Models;
using DanCart.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DanCart.DataAccess.DBInitializer;

public class DBInitializer(ApplicationDbContext db, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager) : IDBInitializer
{
    readonly ApplicationDbContext _db = db;
    readonly UserManager<ApplicationUser> _userManager = userManager;
    readonly RoleManager<IdentityRole> _roleManager = roleManager;

    public void Initialize()
    {
        const string email = "Admin@gmail.com", password = "Admin123*";

        try
        {
            if(_db.Database.GetPendingMigrations().Any())
            {
                _db.Database.Migrate();
            }
        }
        catch (Exception ex) { }
        
        if (!_roleManager.RoleExistsAsync(UserRole.Customer).GetAwaiter().GetResult())
        {
            _roleManager.CreateAsync(new IdentityRole(UserRole.Customer)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(UserRole.Admin)).GetAwaiter().GetResult();

            _userManager.CreateAsync(new ApplicationUser
            {
                UserName = email,
                Email = email,
                Name = "Artem",
                LastName = "Huzhvii",
                PhoneNumber = "1112223333",
                Street = "test 123 Ave",
                Region = "IL",
                Country = "USA",
                City = "Chicago",
                HouseNumber = "123",
                EmailConfirmed = true
            }, password).GetAwaiter().GetResult();
            ApplicationUser user = _db.Users.FirstOrDefault(x => x.Email == email);
            _userManager.AddToRoleAsync(user, UserRole.Admin).GetAwaiter().GetResult();
        }
        return;
    }
}
