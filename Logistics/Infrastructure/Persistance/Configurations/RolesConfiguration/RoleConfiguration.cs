using Logistics.Domain.Entities.Auth.RolesEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Logistics.Infrastructure.Persistance.Configurations.RolesConfiguration
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure (EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("Roles");
            builder.HasIndex(r => r.Name).IsUnique();
            builder.HasMany(r => r.RoleClaims).WithOne().HasForeignKey(r => r.RoleId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        }
    }
}
