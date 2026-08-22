namespace Logistics.Domain.Entities.RolePermissionEntities
{
    public class RolePermissionEntity
    {
        public int RoleId { get; private set; }
        public int PermissionId { get; private set; }
        private RolePermissionEntity() { }
        public RolePermissionEntity(int roleId, int permissionId)
        {
            RoleId  = roleId;
            PermissionId = permissionId;
        }
    }
}
