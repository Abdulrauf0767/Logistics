namespace Logistics.Application.Requests.Roles.GetAllRoles
{
    public class MapperPermissionDto { 
        public int PermissionId { get; set; }
        public string PermissionName { get; set; } = string.Empty;
        public MapperPermissionDto (string permissionName , int id)
        {
            PermissionId = id;
            PermissionName = permissionName;
        }
    }
    public class GetAllRolesResponse
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; } = string.Empty;
        public string RoleDescription { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<MapperPermissionDto> MappedPermission { get; set; } = new List<MapperPermissionDto>();
    }
}
