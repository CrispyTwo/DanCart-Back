using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using DanCart.Models;

namespace DanCart.DataAccess.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<Store> Stores { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<SalesOrder> SalesOrders { get; set; }
    public DbSet<SalesLine> SalesLines { get; set; }
    public DbSet<ApplicationUser> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Product>()
            .HasIndex(p => p.Id)
            .IsUnique();

        builder.Entity<SalesOrder>()
            .HasIndex(o => o.Id)
            .IsUnique();

        builder.Entity<ApplicationUser>()
            .HasIndex(c => c.Email)
            .IsUnique();
    }
}
