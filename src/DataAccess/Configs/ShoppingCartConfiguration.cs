using DanCart.Models.Products;
using DanCart.Models.SalesOrders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DanCart.DataAccess.Configs;

internal class ShoppingCartConfiguration : IEntityTypeConfiguration<ShoppingCart>
{
    public void Configure(EntityTypeBuilder<ShoppingCart> builder)
    {
        builder.HasKey(sc => new
        {
            sc.UserId,
            sc.ProductId,
            sc.Color,
            sc.Size
        });

        builder.Property(sc => sc.Color)
               .IsRequired()
               .HasMaxLength(50);

        builder.Property(sc => sc.Size)
               .IsRequired();

        builder.HasOne(sc => sc.User)
               .WithMany()
               .HasForeignKey(sc => sc.UserId);

        builder.HasOne(sc => sc.Product)
               .WithMany()
               .HasForeignKey(sc => sc.ProductId);
    }
}
