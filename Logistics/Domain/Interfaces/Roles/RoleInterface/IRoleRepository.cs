using Logistics.Domain.Entities;

namespace Logistics.Domain.Interfaces.Roles.RoleInterface
{
    public interface IRoleRepository
    {
        Task AddRoleAsync(RoleEntity role);
        // interface to update role
        void UpdateRole(RoleEntity role);
        // interface to delete role
        void DeleteRole(RoleEntity role);
        // check exists name
        Task<bool> ExistsByName(string name);
    }
}
