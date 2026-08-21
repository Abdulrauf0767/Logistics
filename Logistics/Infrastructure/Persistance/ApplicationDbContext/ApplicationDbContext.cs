using Logistics.Domain.Entities.PermissionEntities;
using Logistics.Domain.Entities.RoleEntities;
using Logistics.Domain.Entities.RolePermissionEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;

namespace Logistics.Infrastructure.Persistance.ApplicationDbContext
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<PermissionEntity> Permissions =>  Set <PermissionEntity > ();
        public DbSet<RoleEntity> Roles => Set<RoleEntity>();
        public DbSet<RolePermissionEntity> RolePermissions => Set<RolePermissionEntity>();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }
    }
}
