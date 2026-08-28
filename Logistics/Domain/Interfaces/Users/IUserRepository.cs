using Logistics.Domain.Entities.UserEntities;

namespace Logistics.Domain.Interfaces.Users
{
    public interface IUserRepository
    {
        Task CreateUserAsync(UserEntity user);
        Task <bool> IsPhoneExists(string Phone);
        Task<bool> IsUserExistsByRoleId(int roleId);
        Task<UserEntity?> GetUserByPhoneAsync(string phone);

    }
}
