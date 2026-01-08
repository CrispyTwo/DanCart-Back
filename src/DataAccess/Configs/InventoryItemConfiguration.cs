using DanCart.Models.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DanCart.DataAccess.Configs;

internal class InventoryItemConfiguration : IEntityTypeConfiguration<InventoryItem>
{
    public void Configure(EntityTypeBuilder<InventoryItem> builder)
    {
        builder.HasKey(it => new { it.Color, it.Size, it.ProductId });
        builder.HasIndex(it => it.ProductId);
    }
}