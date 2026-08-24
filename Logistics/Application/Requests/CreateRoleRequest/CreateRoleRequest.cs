using System.ComponentModel.DataAnnotations;

namespace Logistics.Application.Requests.CreateRoleRequest
{
    public class CreateRoleRequest
    {
        [Required(ErrorMessage = "Role Name is required")]
        [StringLength(100, MinimumLength = 4, ErrorMessage = "Role contains atleast four characters")]
        public string RoleName { get; set; } = null!;
        [StringLength(500,ErrorMessage ="Description cannot exceed than 500 characters")]
        public string Description { get; set; } = string.Empty;
        [MinLength(1, ErrorMessage = "At least one PermissionId is required")]
        public List<int> PermissionIds { get; set; } = new();
    }
}
