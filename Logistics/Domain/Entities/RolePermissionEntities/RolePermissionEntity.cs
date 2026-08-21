namespace Logistics.Domain.Entities.RolePermissionEntities
{
    public class RolePermissionEntity
    {
        public int RoleId { get; private set; }
        public int PermissionId { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; internal set; }
        private RolePermissionEntity() { }
        public RolePermissionEntity(int roleId, int permissionId)
        {
            RoleId  = roleId;
            PermissionId = permissionId;
            CreatedAt = DateTime.UtcNow;
        }
    }
}
