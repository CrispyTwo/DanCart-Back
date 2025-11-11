using DanCart.DataAccess.Models;
using DanCart.Models;
using DanCart.Models.Auth;
using DanCart.Models.Products;
using DanCart.Models.SalesOrders;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace DanCart.DataAccess.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<Store> Stores { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<SalesOrder> SalesOrders { get; set; }
    public DbSet<SalesLine> SalesLines { get; set; }
    public new DbSet<ApplicationUser> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Product>()
            .Property(x => x.WeightUnit)
            .HasConversion<string>()
            .HasMaxLength(5);

        builder.Entity<SalesOrder>()
            .HasMany(o => o.SalesLines)
            .WithOne(l => l.SalesOrder)
            .HasForeignKey(l => l.SalesOrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<SalesLine>()
            .HasIndex(x => new { x.SalesOrderId, x.ProductId })
            .IsUnique();

        builder.Entity<ApplicationUser>()
            .HasIndex(x => x.Email)
            .IsUnique();
    }
}
