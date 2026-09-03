using Logistics.Domain.Entities.Auth.RolesClaim;
using Logistics.Domain.Entities.Auth.RolesEntity;
using Logistics.Domain.Entities.Auth.UsersClaimEntity;
using Logistics.Domain.Entities.Auth.UsersEntity;
using Logistics.Domain.Entities.Auth.UsersLogin;
using Logistics.Domain.Entities.Auth.UsersRole;
using Logistics.Domain.Entities.Auth.UsersToken;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Logistics.Infrastructure.Persistance.ApplicationDbContext
{
    public class ApplicationDbContext
        : IdentityDbContext<User, Role, int, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<Role>().ToTable("Roles");
            modelBuilder.Entity<UserRole>().ToTable("UserRoles");
            modelBuilder.Entity<UserClaim>().ToTable("UserClaims");
            modelBuilder.Entity<UserLogin>().ToTable("UserLogins");
            modelBuilder.Entity<RoleClaim>().ToTable("RoleClaims");
            modelBuilder.Entity<UserToken>().ToTable("UserTokens");
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }

        public override int SaveChanges()
        {
            ApplyAuditLogics();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ApplyAuditLogics();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void ApplyAuditLogics()
        {
            var entries = ChangeTracker.Entries().Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);
            var currentTime = DateTime.UtcNow;
            foreach (var entry in entries)
            {
                if (entry.Metadata.FindProperty("CreatedAt") != null)
                {
                    if (entry.State == EntityState.Added)
                    {
                        entry.Property("CreatedAt").CurrentValue = currentTime;
                    }
                    entry.Property("UpdatedAt").CurrentValue = currentTime;
                }
            }
        }
    }
}