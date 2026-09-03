using Microsoft.AspNetCore.Identity;

namespace Logistics.Domain.Entities.Auth.UsersRole
{
    public class UserRole :IdentityUserRole<int>
    {
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
