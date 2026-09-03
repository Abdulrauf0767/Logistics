using Logistics.Domain.Entities.Auth.RolesClaim;
using Microsoft.AspNetCore.Identity;

namespace Logistics.Domain.Entities.Auth.RolesEntity
{
    public class Role : IdentityRole<int>
    {
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public bool IsActive { get; set; } = true;
        public virtual ICollection<RoleClaim> RoleClaims { get; set; } = new List<RoleClaim>();
    }
}
