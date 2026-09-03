using Logistics.Domain.Entities.Auth.RolesEntity;
using Logistics.Domain.Entities.Auth.UsersEntity;
using Logistics.Domain.Entities.Auth.UsersRole;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Logistics.Infrastructure.Persistance.Configurations.UsersRoleConfiguration
{
    public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
    {
        public void Configure (EntityTypeBuilder<UserRole> builder)
        {
            builder.ToTable("UsersRole");
            builder.HasKey(ur => ur.UserId);
            builder.HasOne<Role>()
                .WithMany()
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne<User>()
                .WithOne()
                .HasForeignKey<UserRole>(ur => ur.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }

    }
}
