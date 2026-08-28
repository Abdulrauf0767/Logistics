using Logistics.Domain.Entities.PermissionEntities;

namespace Logistics.Domain.Entities.RolePermissionsEntity
{
    public class RolePermissionEntity
    {
        public int RoleId { get; private set; }
        public int PermissionId { get; private set; }
        // Navigation Property
        public PermissionEntity Permission { get; private set; } = null!;
        private RolePermissionEntity() { }

        public RolePermissionEntity(int roleId, int permissionId)
        {
            RoleId = roleId;
            PermissionId = permissionId;
        }
    }
}
