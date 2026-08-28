using Logistics.Domain.Entities.RefreshTokenEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Logistics.Infrastructure.Persistance.Configurations.RefreshTokenConfiguration
{
    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshTokenEntity>
    {
        public void Configure (EntityTypeBuilder<RefreshTokenEntity> builder)
        {
            builder.ToTable("RefreshTokens");
            builder.HasKey(rf => rf.Id);
            builder.HasIndex(rf => rf.TokenHash).IsUnique();
            builder.HasOne(rt => rt.User).WithMany().HasForeignKey(rf => rf.UserId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
