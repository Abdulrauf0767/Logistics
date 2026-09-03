using Microsoft.AspNetCore.Identity;

namespace Logistics.Domain.Entities.Auth.UsersToken
{
    public class UserToken : IdentityUserToken<int>
    {
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
