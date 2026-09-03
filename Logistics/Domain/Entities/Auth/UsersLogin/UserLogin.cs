using Microsoft.AspNetCore.Identity;

namespace Logistics.Domain.Entities.Auth.UsersLogin
{
    public class UserLogin : IdentityUserLogin<int>
    {
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
