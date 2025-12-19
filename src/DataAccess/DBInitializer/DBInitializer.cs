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
                new Product { Name = "Classic White Tee", Description = "100% cotton crew neck t-shirt", Colors = "White,Black,Navy", Category = "T-Shirt", WeightUnit = UnitOfMeasure.Kg, Stock = 45, Price = 19.99m, LowStockThreshold = 5 },
                new Product { Name = "Oxford Button-Down Shirt", Description = "Long-sleeve oxford shirt, tailored fit", Colors = "Blue,White,Pink", Category = "T-Shirt", WeightUnit = UnitOfMeasure.Kg, Stock = 30, Price = 49.50m, LowStockThreshold = 6 },
                new Product { Name = "Slim Fit Denim Jeans", Description = "Stretch denim, slim leg", Colors = "Indigo,Black", Category = "Pants", WeightUnit = UnitOfMeasure.Kg, Stock = 25, Price = 69.00m, LowStockThreshold = 4 },
                new Product { Name = "Chino Pants", Description = "Smart-casual chinos with tapered leg", Colors = "Khaki,Navy,Olive", Category = "Pants", WeightUnit = UnitOfMeasure.Kg, Stock = 40, Price = 54.99m, LowStockThreshold = 8 },
                new Product { Name = "Pullover Hoodie", Description = "Fleece-lined pullover hoodie with kangaroo pocket", Colors = "Grey,Black,ForestGreen", Category = "Hoodie", WeightUnit = UnitOfMeasure.Kg, Stock = 22, Price = 39.99m, LowStockThreshold = 5 },
                new Product { Name = "Bomber Jacket", Description = "Lightweight bomber jacket with ribbed cuffs", Colors = "Black,Navy,Maroon", Category = "Jacket", WeightUnit = UnitOfMeasure.Kg, Stock = 12, Price = 89.99m, LowStockThreshold = 3 },
                new Product { Name = "Wool Overcoat", Description = "Single-breasted wool overcoat, knee length", Colors = "Charcoal,Navy", Category = "Jacket", WeightUnit = UnitOfMeasure.Kg, Stock = 8, Price = 199.00m, LowStockThreshold = 2 },
                new Product { Name = "Leather Belt", Description = "Full-grain leather belt with metal buckle", Colors = "Brown,Black", Category = "Accessories", WeightUnit = UnitOfMeasure.Kg, Stock = 60, Price = 29.95m, LowStockThreshold = 10 },
                new Product { Name = "Casual Sneakers", Description = "Low-top canvas sneakers with rubber sole", Colors = "White,Black,Navy", Category = "Boots", WeightUnit = UnitOfMeasure.Kg, Stock = 35, Price = 74.50m, LowStockThreshold = 7 },
                new Product { Name = "Swim Shorts", Description = "Quick-dry swim shorts with mesh lining", Colors = "Blue,Red,Black", Category = "Shorts", WeightUnit = UnitOfMeasure.Kg, Stock = 50, Price = 24.00m, LowStockThreshold = 10 },
            ];


            _db.Products.AddRange(products);
            _db.SaveChanges();
        }
        return;
    }
}
