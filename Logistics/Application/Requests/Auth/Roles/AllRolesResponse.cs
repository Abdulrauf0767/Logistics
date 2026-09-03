namespace Logistics.Application.Requests.Auth.Roles
{
    public class AllRolesResponse
    {
        public int Id { get; set; }
        public string RoleName { get; set; } = null!;
        public bool IsActive { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public List<string> Permissions { get; set; } = new();

    }
}
