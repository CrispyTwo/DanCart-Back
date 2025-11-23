using DanCart.Models.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DanCart.DataAccess.Configs;

internal class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(p => p.Id);

        builder.HasGeneratedTsVectorColumn(
            p => p.SearchVector, "english", 
            p => new { p.Name, p.Description })
            .HasIndex(p => p.SearchVector)
            .HasMethod("GIN");

        builder.Property(x => x.WeightUnit)
            .HasConversion<string>()
            .HasMaxLength(5);
    }
}
