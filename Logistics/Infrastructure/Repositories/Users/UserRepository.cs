using Logistics.Domain.Entities.UserEntities;
using Logistics.Domain.Interfaces.Users;
using Logistics.Infrastructure.Persistance.ApplicationDbContext;
using Microsoft.EntityFrameworkCore;

namespace Logistics.Infrastructure.Repositories.Users
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        public UserRepository(ApplicationDbContext context) { 
            _context = context;
        }
        public async Task CreateUserAsync (UserEntity user)
        {
            await _context.Users.AddAsync(user);
        }
        public async Task<bool> IsPhoneExists(string phone) { 
            return await _context.Users.AsNoTracking().AnyAsync(u => u.PhoneNumber.Trim() == phone.Trim());
        }
        public async Task<bool> IsUserExistsByRoleId(int roleId)
        {
            return await _context.Users.AsNoTracking().AnyAsync(u => u.RoleId == roleId);
        }

    }
}
