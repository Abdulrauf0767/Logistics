using System.ComponentModel.DataAnnotations;

namespace Logistics.Application.Requests.Auth.Roles
{
    public class UpdateRoleRequest
    {
        [Required(ErrorMessage ="Role Id is required")]
        [Range(1,int.MaxValue,ErrorMessage ="Id must be positive")]
        public int RoleId { get; set; }
        [Required(ErrorMessage = "Role Name is required!")]
        [StringLength(50, MinimumLength = 4, ErrorMessage = "Atleast role name consists of four characters.")]
        public string RoleName { get; set; } = null!;
        [MinLength(1, ErrorMessage = "Atleast one permission is required!")]
        public List<string> PermissionsList { get; set; } = new();
        public bool IsActive { get; set; }
    }
}
