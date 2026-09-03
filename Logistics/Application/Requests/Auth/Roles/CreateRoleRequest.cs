using System.ComponentModel.DataAnnotations;

namespace Logistics.Application.Requests.Auth.Roles
{
    public class CreateRoleRequest
    {
        [Required(ErrorMessage = "Role Name is required!")]
        [StringLength(50, MinimumLength = 4, ErrorMessage = "Atleast role name consists of four characters.")]
        public string RoleName { get; set; } = null!;
        [MinLength(1,ErrorMessage ="Atleast one permission is required!")]
        public List<string> PermissionsList { get; set; } = new ();

    }
}
