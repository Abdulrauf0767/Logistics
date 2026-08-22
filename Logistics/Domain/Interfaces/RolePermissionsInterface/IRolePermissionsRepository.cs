using Logistics.Domain.Entities.RolePermissionEntities;

namespace Logistics.Domain.Interfaces.RolePermissionsInterface
{
    public interface IRolePermissionsRepository
    {
        // create role permissions
        Task AddRolePermissionsAsync(RolePermissionEntity rp);
        // delete role permissions
        public void DeleteRolePermissions(RolePermissionEntity rp);
        // update role permissions
        public void UpdateRolePermissions (RolePermissionEntity rp);
        // check already assigned permissions
        Task<List<int>> GetAlreadyAssignedPermissionIdsAsync(int roleId, List<int> permissionIds);
        // save changes
        Task SaveChangesAsync();

    }
}
