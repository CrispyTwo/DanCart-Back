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

            Product[] products =
            [
                new Product { Id = Guid.Parse("a36ecfe2-86a3-42b3-80aa-0c46c9efcd8e"), Name = "Classic White Tee", Description = "100% cotton crew neck t-shirt", Category = "T-Shirt", WeightUnit = UnitOfMeasure.Kg, Price = 19.99m, LowStockThreshold = 5 },
                new Product { Id = Guid.Parse("2f968c26-fb4d-4c12-b79d-b0a9ec221c87"), Name = "Oxford Button-Down Shirt", Description = "Long-sleeve oxford shirt, tailored fit", Category = "T-Shirt", WeightUnit = UnitOfMeasure.Kg, Price = 49.50m, LowStockThreshold = 6 },
                new Product { Id = Guid.Parse("ed4af53e-7298-4e5b-b366-44d2d9fa55ef"), Name = "Slim Fit Denim Jeans", Description = "Stretch denim, slim leg", Category = "Pants", WeightUnit = UnitOfMeasure.Kg, Price = 69.00m, LowStockThreshold = 4 },
                new Product { Id = Guid.Parse("ec37d3f1-3b15-4e8f-b9c0-0a2a1af935ee"), Name = "Chino Pants", Description = "Smart-casual chinos with tapered leg", Category = "Pants", WeightUnit = UnitOfMeasure.Kg, Price = 54.99m, LowStockThreshold = 8 },
                new Product { Id = Guid.Parse("2e0819dd-84eb-44fb-a821-f4bd7787c164"), Name = "Pullover Hoodie", Description = "Fleece-lined pullover hoodie with kangaroo pocket", Category = "Hoodie", WeightUnit = UnitOfMeasure.Kg, Price = 39.99m, LowStockThreshold = 5 },
                new Product { Id = Guid.Parse("6f8fd97c-d7aa-48d2-a6a3-4c21961eaf7d"), Name = "Bomber Jacket", Description = "Lightweight bomber jacket with ribbed cuffs", Category = "Jacket", WeightUnit = UnitOfMeasure.Kg, Price = 89.99m, LowStockThreshold = 3 },
                new Product { Id = Guid.Parse("61a3778d-bde7-4a89-9e96-6bad5353eb58"), Name = "Wool Overcoat", Description = "Single-breasted wool overcoat, knee length", Category = "Jacket", WeightUnit = UnitOfMeasure.Kg, Price = 199.00m, LowStockThreshold = 2 },
                new Product { Id = Guid.Parse("2b8396c4-9182-43f7-b170-0be762245de0"), Name = "Leather Belt", Description = "Full-grain leather belt with metal buckle", Category = "Accessories", WeightUnit = UnitOfMeasure.Kg, Price = 29.95m, LowStockThreshold = 10 },
                new Product { Id = Guid.Parse("4278bf7c-0a88-4b15-ba84-ee1beed0b9c5"), Name = "Casual Sneakers", Description = "Low-top canvas sneakers with rubber sole", Category = "Boots", WeightUnit = UnitOfMeasure.Kg, Price = 74.50m, LowStockThreshold = 7 },
                new Product { Id = Guid.Parse("c664101e-fb80-4fd1-8ee0-09b64f44d6c2"), Name = "Swim Shorts", Description = "Quick-dry swim shorts with mesh lining", Category = "Shorts", WeightUnit = UnitOfMeasure.Kg, Price = 24.00m, LowStockThreshold = 10 },
            ];


            _db.Products.AddRange(products);

            IEnumerable<InventoryItem> inventory = new List<InventoryItem>
            {
                 new InventoryItem { ProductId = products[0].Id, Color = "White", Size = ProductSize.XS, Quantity = 50 },
                 new InventoryItem { ProductId = products[0].Id, Color = "White", Size = ProductSize.S, Quantity = 100 },
                 new InventoryItem { ProductId = products[0].Id, Color = "Blue", Size = ProductSize.M, Quantity = 200 },
                 new InventoryItem { ProductId = products[0].Id, Color = "White", Size = ProductSize.L, Quantity = 150 },

                 new InventoryItem { ProductId = products[1].Id, Color = "Yellow", Size = ProductSize.S, Quantity = 100 },
                 new InventoryItem { ProductId = products[1].Id, Color = "White", Size = ProductSize.M, Quantity = 200 },
                 new InventoryItem { ProductId = products[1].Id, Color = "Yellow", Size = ProductSize.L, Quantity = 150 },
                 new InventoryItem { ProductId = products[1].Id, Color = "White", Size = ProductSize.XL, Quantity = 50 },
                 new InventoryItem { ProductId = products[1].Id, Color = "White", Size = ProductSize.XXL, Quantity = 50 },

                 new InventoryItem { ProductId = products[2].Id, Color = "Yellow", Size = ProductSize.S, Quantity = 100 },
                 new InventoryItem { ProductId = products[2].Id, Color = "White", Size = ProductSize.M, Quantity = 200 },
                 new InventoryItem { ProductId = products[2].Id, Color = "Yellow", Size = ProductSize.L, Quantity = 150 },
                 new InventoryItem { ProductId = products[2].Id, Color = "White", Size = ProductSize.XL, Quantity = 50 },
                 new InventoryItem { ProductId = products[2].Id, Color = "White", Size = ProductSize.XXL, Quantity = 50 },

                 new InventoryItem { ProductId = products[3].Id, Color = "Yellow", Size = ProductSize.S, Quantity = 100 },
                 new InventoryItem { ProductId = products[3].Id, Color = "White", Size = ProductSize.M, Quantity = 200 },
                 new InventoryItem { ProductId = products[3].Id, Color = "Yellow", Size = ProductSize.L, Quantity = 150 },
                 new InventoryItem { ProductId = products[3].Id, Color = "White", Size = ProductSize.XL, Quantity = 50 },
                 new InventoryItem { ProductId = products[3].Id, Color = "White", Size = ProductSize.XXL, Quantity = 50 },

                 new InventoryItem { ProductId = products[4].Id, Color = "Yellow", Size = ProductSize.S, Quantity = 100 },
                 new InventoryItem { ProductId = products[4].Id, Color = "White", Size = ProductSize.M, Quantity = 200 },
                 new InventoryItem { ProductId = products[4].Id, Color = "Yellow", Size = ProductSize.L, Quantity = 150 },
                 new InventoryItem { ProductId = products[4].Id, Color = "White", Size = ProductSize.XL, Quantity = 50 },
                 new InventoryItem { ProductId = products[4].Id, Color = "White", Size = ProductSize.XXL, Quantity = 50 },

                 new InventoryItem { ProductId = products[5].Id, Color = "White", Size = ProductSize.XS, Quantity = 50 },
                 new InventoryItem { ProductId = products[5].Id, Color = "White", Size = ProductSize.S, Quantity = 100 },
                 new InventoryItem { ProductId = products[5].Id, Color = "Blue", Size = ProductSize.M, Quantity = 200 },
                 new InventoryItem { ProductId = products[5].Id, Color = "White", Size = ProductSize.L, Quantity = 150 },

                 new InventoryItem { ProductId = products[6].Id, Color = "White", Size = ProductSize.XS, Quantity = 50 },
                 new InventoryItem { ProductId = products[6].Id, Color = "Red", Size = ProductSize.S, Quantity = 100 },
                 new InventoryItem { ProductId = products[6].Id, Color = "Blue", Size = ProductSize.M, Quantity = 200 },
                 new InventoryItem { ProductId = products[6].Id, Color = "White", Size = ProductSize.L, Quantity = 150 },

                 new InventoryItem { ProductId = products[7].Id, Color = "White", Size = ProductSize.XS, Quantity = 50 },
                 new InventoryItem { ProductId = products[7].Id, Color = "Red", Size = ProductSize.S, Quantity = 100 },
                 new InventoryItem { ProductId = products[7].Id, Color = "Blue", Size = ProductSize.M, Quantity = 200 },
                 new InventoryItem { ProductId = products[7].Id, Color = "White", Size = ProductSize.L, Quantity = 150 },

                 new InventoryItem { ProductId = products[8].Id, Color = "White", Size = ProductSize.XS, Quantity = 50 },
                 new InventoryItem { ProductId = products[8].Id, Color = "Red", Size = ProductSize.S, Quantity = 100 },
                 new InventoryItem { ProductId = products[8].Id, Color = "Blue", Size = ProductSize.M, Quantity = 200 },
                 new InventoryItem { ProductId = products[8].Id, Color = "White", Size = ProductSize.L, Quantity = 150 },

                 new InventoryItem { ProductId = products[9].Id, Color = "White", Size = ProductSize.XS, Quantity = 50 },
                 new InventoryItem { ProductId = products[9].Id, Color = "Red", Size = ProductSize.S, Quantity = 100 },
                 new InventoryItem { ProductId = products[9].Id, Color = "Blue", Size = ProductSize.M, Quantity = 200 },
                 new InventoryItem { ProductId = products[9].Id, Color = "White", Size = ProductSize.L, Quantity = 150 },
            };

            _db.InventoryItems.AddRange(inventory);
            _db.SaveChanges();
        }
        return;
    }
}
