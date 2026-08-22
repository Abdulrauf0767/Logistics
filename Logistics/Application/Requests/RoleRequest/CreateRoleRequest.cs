using System.ComponentModel.DataAnnotations;

namespace Logistics.Application.DTOS.RoleDTO
{
    public class CreateRoleRequest
    {
        [Required(ErrorMessage = "Role Name is required")]
        [StringLength(100,MinimumLength =4, ErrorMessage="string length contains atleast 4 characters")]
        public string RoleName { get; set; } = null!;
        [StringLength(500 , ErrorMessage = "Description cannot exceed 500 characters")]
        public string? RoleDescription { get; set; }
        [MinLength(1, ErrorMessage = "At least one PermissionId is required")]
        public List<int> PermissionIds { get; set; } = new ();
        
    }
}
