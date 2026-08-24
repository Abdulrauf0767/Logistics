using Logistics.Domain.Entities;

namespace Logistics.Domain.Interfaces.RoleInterface
{
    public interface IRoleRepository
    {
        // interface to get all roles
        Task<IEnumerable<RoleEntity>> GetRolesAsync();
        // get single role by id interface
        Task<RoleEntity?> GetRoleById(int id);
        // interface to create role 
        Task AddRoleAsync(RoleEntity role);
        // interface to update role
        void UpdateRole(RoleEntity role);
        // interface to delete role
        void DeleteRole(RoleEntity role);
        // check exists name
        Task<bool> ExistsByName(string name);
        // save changes 
        Task SaveChangesAsync();
    }
}
