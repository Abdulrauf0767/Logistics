using System.Text.Json.Serialization;

namespace Logistics.Application.Requests.Auth.Roles
{
    public class AllRolesResponse
    {
        [JsonIgnore]
        public int RoleId { get; set; }
        [JsonIgnore]
        public int PageSize { get; set; }
        public int Id { get; set; }
        public string RoleName { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public List<string> Permissions { get; set; } = new();

    }
}
