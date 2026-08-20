using Logistics.Domain.Entities.PermissionEntities;
using Logistics.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace Logistics.Domain.Authorization.Permissions
{
    public static class PermissionSeeder
    {
        public static async Task SeedAsync(PermissionDbContext dbContext)
        {
            if (await dbContext.Permissions.AnyAsync())
                return;

            var permissions = new List<PermissionEntity>
        {
            new PermissionEntity(
                Permissions.Roles.create,
                "Create a role"
            ),

            new PermissionEntity(
                Permissions.Roles.update,
                "Update a role"
            ),

            new PermissionEntity(
                Permissions.Roles.view,
                "View roles"
            ),

            new PermissionEntity(
                Permissions.Roles.delete,
                "Delete a role"
            )
        };

            await dbContext.Permissions.AddRangeAsync(permissions);
            await dbContext.SaveChangesAsync();
        }
    }
}
