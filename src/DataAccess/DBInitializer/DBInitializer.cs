using DanCart.DataAccess.Data;
using DanCart.Models;
using DanCart.Models.Auth;
using DanCart.Models.Products;
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
            if (_db.Database.GetPendingMigrations().Any())
            {
                _db.Database.Migrate();
            }
        }
        catch { }
        
        if (!_roleManager.RoleExistsAsync(UserRole.Customer).GetAwaiter().GetResult())
        {
            _roleManager.CreateAsync(new IdentityRole(UserRole.Customer)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(UserRole.Admin)).GetAwaiter().GetResult();

            _userManager.CreateAsync(new ApplicationUser
            {
                UserName = email,
                Email = email,
                FirstName = "Artem",
                LastName = "Huzhvii",
                PhoneNumber = "1112223333",
                Street = "test 123 Ave",
                Region = "IL",
                Country = "USA",
                City = "Chicago",
                HouseNumber = "123",
                EmailConfirmed = true
            }, password).GetAwaiter().GetResult();
            ApplicationUser admin = _db.Users.FirstOrDefault(x => x.Email == email)!;
            _userManager.AddToRoleAsync(admin, UserRole.Admin).GetAwaiter().GetResult();

            _userManager.CreateAsync(new ApplicationUser
            {
                UserName = "dmytrop@gmail.com",
                Email = "dmytrop@gmail.com",
                FirstName = "Dmytro",
                LastName = "Pochapskiy",
                PhoneNumber = "5112273333",
                Street = "Street Avenue",
                Region = "California",
                Country = "USA",
                City = "Los Angeles",
                HouseNumber = "234",
                EmailConfirmed = true
            }, password).GetAwaiter().GetResult();
            ApplicationUser user = _db.Users.FirstOrDefault(x => x.Email == email)!;
            _userManager.AddToRoleAsync(user, UserRole.Customer).GetAwaiter().GetResult();

            IEnumerable<Product> products =
            [
                new Product { Name = "White sugar", Description = "White Sugar", WeightUnit = UnitOfMeasure.Kg, Stock = 10, Price = 12.50m, LowStockThreshold = 10 },
                new Product { Name = "Brown sugar", Description = "Brown Sugar", WeightUnit = UnitOfMeasure.G,  Stock = 20, Price = 0.0125m },
                new Product { Name = "Flour",       Description = "Flour",       WeightUnit = UnitOfMeasure.Kg, Stock = 0,  Price = 4.20m }
            ];

            _db.Products.AddRange(products);
            _db.SaveChanges();
        }
        return;
    }
}
