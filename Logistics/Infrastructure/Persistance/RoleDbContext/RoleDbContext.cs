using Logistics.Domain.Entities.RoleEntities;
using Microsoft.EntityFrameworkCore;

namespace Logistics.Infrastructure.Persistance.RoleDbContext
{
    public class RoleDbContext : DbContext
    {
        public RoleDbContext(DbContextOptions<RoleDbContext> options) : base (options) { }
        public DbSet<RoleEntity> Roles => Set<RoleEntity>();
    }
}
