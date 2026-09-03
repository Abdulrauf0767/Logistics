using Microsoft.AspNetCore.Identity;

namespace Logistics.Domain.Entities.Auth.UsersEntity
{
    public class User : IdentityUser<int> 
    {
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
