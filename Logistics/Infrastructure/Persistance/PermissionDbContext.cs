    using Logistics.Domain.Entities.PermissionEntities;
using Microsoft.EntityFrameworkCore;

namespace Logistics.Infrastructure.Persistance
{
    public class PermissionDbContext : DbContext
    {
        public PermissionDbContext(DbContextOptions<PermissionDbContext> options) : base(options)
        {

        }
        public DbSet<PermissionEntity> Permissions => Set<PermissionEntity>();
    }
}
    