using Logistics.Application.DTOS.RoleDTO;
using Logistics.Domain.Entities.RolePermissionEntities;
using Logistics.Domain.Interfaces.PermissionsInterface;
using Logistics.Domain.Interfaces.RoleInterfaces;
using Logistics.Domain.Interfaces.RolePermissionsInterface;

namespace Logistics.Application.Services.RolePermissionService
{
    public class RolePermissionService 
    {
        private readonly IRolePermissionsRepository _rolePermissions;
        private readonly IPermissionRepository _permissionRepository;
        public RolePermissionService (IRolePermissionsRepository rolePermissions,IPermissionRepository permissionRepository)
        {
            _rolePermissions = rolePermissions;
            _permissionRepository = permissionRepository;
        }
        // create role permissions with validation
        public async Task AddRolePermissionsAsync (List<int> PermissionIds, int roleId)
        {
            if (PermissionIds == null || !PermissionIds.Any())
            {
                throw new Exception("Please assign at least one permission.");
            }
            var uniqueIds = PermissionIds.Distinct().ToList();
            var existingPermissions = await _permissionRepository.GetCountByIdAsync (uniqueIds);
            if (existingPermissions != uniqueIds.Count)
            {
                throw new Exception("One or more provided permissions do not exists.");
            }
            var alreadyAssignedIds = await _rolePermissions.GetAlreadyAssignedPermissionIdsAsync(roleId, uniqueIds);
            if (alreadyAssignedIds.Any())
            {
                throw new Exception($"Permissions are already assigned to this role.");
            }
            foreach (var permissionId in uniqueIds)
            {
                var rolePermission = new RolePermissionEntity(roleId,permissionId);
                await _rolePermissions.AddRolePermissionsAsync (rolePermission);
            }
        }
    }
}
