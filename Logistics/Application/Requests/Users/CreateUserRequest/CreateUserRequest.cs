using System.ComponentModel.DataAnnotations;

namespace Logistics.Application.Requests.Users.CreateUserRequest
{
    public class CreateUserRequest
    {
        [Required(ErrorMessage = "Phone number is required")]
        [StringLength(20, MinimumLength =11,ErrorMessage = "Phone Number must be atleast 11 digits and cannot exceed than 20 digits")]
        public string PhoneNumber { get; set; } = null!;
        [Required(ErrorMessage ="Password is required")]
        [StringLength(30,MinimumLength =6,ErrorMessage ="Password must be between 6 to 30 characters")]
        public string Password { get; set; } = null!;
        [Required(ErrorMessage = "Role ID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid Role ID")]
        public int RoleId { get; set; }

    }
}
