using Logistics.Domain.Entities.RoleEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Logistics.Infrastructure.Persistance.Configurations.RoleConfiguration
{
    public class RoleConfiguration : IEntityTypeConfiguration<RoleEntity>
    {
        public void Configure (EntityTypeBuilder<RoleEntity> builder)
        {
            builder.ToTable("Roles");
            builder.HasKey(r => r.Id);
            builder.Property(r => r.RoleName).IsRequired().HasMaxLength(100);
            builder.HasIndex(r => r.RoleName).IsUnique();
        }
    }
}
