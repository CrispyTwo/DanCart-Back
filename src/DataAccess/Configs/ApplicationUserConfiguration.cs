using DanCart.Models.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DanCart.DataAccess.Configs;

internal class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.HasGeneratedTsVectorColumn(
            p => p.SearchVector, "english",
            p => new { p.Email, p.FirstName, p.LastName })
            .HasIndex(p => p.SearchVector)
            .HasMethod("GIN");
    }
}
