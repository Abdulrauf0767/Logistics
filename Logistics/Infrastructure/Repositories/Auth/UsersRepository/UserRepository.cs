using Logistics.Domain.Entities.Auth.UsersEntity;
using Logistics.Domain.Entities.Auth.UsersRole;
using Logistics.Domain.Interfaces.Auth.UserInterface;
using Logistics.Infrastructure.Persistance.ApplicationDbContext;
using Microsoft.EntityFrameworkCore;

namespace Logistics.Infrastructure.Repositories.Auth.UsersRepository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public UserRepository (ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<bool> ExistUserByPhone(string phone)
        {
            return await _dbContext.Users.AsNoTracking().AnyAsync(u => u.PhoneNumber.ToLower() == phone.ToLower());
        }
        public async Task CreateUserAsync (User user)
        {
            await _dbContext.Users.AddAsync(user);
        }
        public async Task AssignRoleToUserAsync (UserRole role)
        {
            await  _dbContext.UserRoles.AddAsync(role);
        }
    }
}
