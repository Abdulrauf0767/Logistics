namespace Logistics.Domain.Interfaces.JwtProvider
{
    public class MappingPermissions
    {
        public int PermissionId { get; set; }
        public string? PermissionName { get; set; }
    }
    public interface IJwtProvider
    {
        string GenerateToken(int userId, string roleName, List<MappingPermissions> mappingPermissions);
    }
}
