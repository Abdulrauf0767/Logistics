using Microsoft.AspNetCore.Identity;

namespace Logistics.Domain.Entities.Auth.RolesClaim
{
    public class RoleClaim : IdentityRoleClaim<int>
    {
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
