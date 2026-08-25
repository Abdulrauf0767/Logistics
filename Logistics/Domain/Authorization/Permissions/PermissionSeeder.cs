using Logistics.Domain.Entities.PermissionEntities;
using Logistics.Domain.Interfaces.Roles.PermissionsInterface;

namespace Logistics.Domain.Authorization.Permissions
{
    public static class PermissionSeeder
    {
        public static async Task SeedAsync(IPermissionRepository permissionRepository)
        {
            var codePermissions = new List<PermissionEntity>
            {
                new PermissionEntity(Permissions.Roles.create, "Create a role"),
                new PermissionEntity(Permissions.Roles.update, "Update a role"),
                new PermissionEntity(Permissions.Roles.view, "View roles"),
                new PermissionEntity(Permissions.Roles.delete, "Delete a role")
            }; 
            var dbPermissionNames = await permissionRepository.GetExistingNamesAsync();
            var missingPermissions = codePermissions
                .Where(cp => !dbPermissionNames.Contains(cp.Name))
                .ToList();
            if (missingPermissions.Any())
            {
                foreach (var permission in missingPermissions)
                {
                    await permissionRepository.AddPermissionsAsync(permission);
                }

                await permissionRepository.SaveChangesAsync();
            }
        }
    }
}
