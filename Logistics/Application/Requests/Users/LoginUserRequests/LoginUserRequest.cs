using System.ComponentModel.DataAnnotations;

namespace Logistics.Application.Requests.Users.LoginUserRequests
{
    public class LoginUserRequest
    {
        [Required(ErrorMessage ="Phone Number is required")]
        [StringLength(20,MinimumLength =11, ErrorMessage ="Phone number must between 11 and 20 digits")]
        public string? PhoneNumber { get; set; }
        [Required(ErrorMessage ="Password is required")]
        public string? Password { get; set; }
    }
}
