using DanCart.Models.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DanCart.DataAccess.Configs;

internal class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.HasKey(rt => rt.Id);
        builder.Property(rt => rt.Token).HasMaxLength(256);
        builder.HasIndex(rt => rt.Token).IsUnique();
        builder.HasOne(rt => rt.User).WithMany().HasForeignKey(r => r.UserId);
    }
}
