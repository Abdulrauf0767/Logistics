using Logistics.Domain.Entities;
using Logistics.Domain.Entities.PermissionEntities;
using Logistics.Domain.Entities.RolePermissionsEntity;
using Logistics.Domain.Entities.UserEntity;
using Microsoft.EntityFrameworkCore;

namespace Logistics.Infrastructure.Persistance.ApplicationDbContext
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<PermissionEntity> Permissions =>  Set <PermissionEntity > ();
        public DbSet<RoleEntity> Roles => Set<RoleEntity>();
        public DbSet<RolePermissionEntity> RolePermissions => Set<RolePermissionEntity>();
        public DbSet<UserEntity> Users => Set<UserEntity>();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (entityType != null)
                {
                    modelBuilder.Entity(entityType.ClrType).Property<DateTime>("CreatedAt");
                    modelBuilder.Entity(entityType.ClrType).Property<DateTime>("UpdatedAt");
                }
            }
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
                if (entry.Metadata.FindProperty("CreatedAt") != null )
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
