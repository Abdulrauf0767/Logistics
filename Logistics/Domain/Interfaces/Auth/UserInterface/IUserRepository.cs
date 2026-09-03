using Logistics.Domain.Entities.Auth.UsersEntity;
using Logistics.Domain.Entities.Auth.UsersRole;

namespace Logistics.Domain.Interfaces.Auth.UserInterface
{
    public interface IUserRepository
    {
        Task <bool> ExistUserByPhone (string phone);
        Task CreateUserAsync(User user);
        Task AssignRoleToUserAsync(UserRole userRole);
    }
}
