using Logistics.Domain.Entities.RolePermissionsEntity;

namespace Logistics.Domain.Interfaces.RolePermissionInterface
{
    public interface IRolePermissionRepository
    {
        // create role permissions
        Task AddRolePermissionsAsync(RolePermissionEntity rp);
        // delete role permissions
        public void DeleteRolePermissions(RolePermissionEntity rp);
        // update role permissions
        public void UpdateRolePermissions(RolePermissionEntity rp);
        // check already assigned permissions
        Task<List<int>> GetAlreadyAssignedPermissionIdsAsync(int roleId, List<int> permissionIds);
        // save changes
        Task SaveChangesAsync();
    }
}
