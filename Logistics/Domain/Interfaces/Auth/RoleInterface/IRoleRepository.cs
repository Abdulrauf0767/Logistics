using Logistics.Application.Requests.Auth.Roles;
using Logistics.Domain.Entities.Auth.RolesClaim;
using Logistics.Domain.Entities.Auth.RolesEntity;

namespace Logistics.Domain.Interfaces.Auth.RoleInterface
{
    public interface IRoleRepository
    {
        Task<bool> ExistsRoleByName (string roleName);
        Task<bool> ExistsRoleById (int roleId);
        Task CreateRoleAsync (Role role);
        Task AddPermissionToRoleAsync(RoleClaim roleClaim);
        Task<string?> GetRoleNameById (int roleId);
        Task<Role?> GetRoleByIdForUpdate (int roleId);
        Task<GetRoleByIdResponse?> GetRoleByIdReadOnly(int roleId);
        Task<List<AllRolesResponse>> GetAllRolesAsync(int pageSize,int RoleId);
    }
}
