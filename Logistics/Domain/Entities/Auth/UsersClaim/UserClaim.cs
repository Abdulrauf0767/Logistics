using Microsoft.AspNetCore.Identity;

namespace Logistics.Domain.Entities.Auth.UsersClaimEntity
{
    public class UserClaim : IdentityUserClaim<int>
    {
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
