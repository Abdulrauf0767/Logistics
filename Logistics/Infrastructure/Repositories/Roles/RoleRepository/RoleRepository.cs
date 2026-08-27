using Logistics.Domain.Entities;
using Logistics.Domain.Interfaces.Roles.RoleInterface;
using Logistics.Infrastructure.Persistance.ApplicationDbContext;
using Microsoft.EntityFrameworkCore;
namespace Logistics.Infrastructure.Repositories.Roles.RoleRepository
{
    public class RoleRepository : IRoleRepository
    {
        private readonly ApplicationDbContext _roleDb;
        public RoleRepository(ApplicationDbContext roleDb)
        {
            _roleDb = roleDb;
        }
        
        // create role 
        public async Task AddRoleAsync(RoleEntity role)
        {
            await _roleDb.Roles.AddAsync(role);
        }
        // update role
        public void UpdateRole(RoleEntity role)
        {
            _roleDb.Roles.Update(role);
        }
        // delete role
        public void DeleteRole(RoleEntity role)
        {
            _roleDb.Roles.Remove(role);
        }
        // check exists name 
        public async Task<bool> ExistsByName(string name)
        {
            return await _roleDb.Roles.AsNoTracking().AnyAsync(r => r.Name.ToLower() == name.ToLower());
        }
        public async Task<RoleEntity?> GetByIdAsync(int id)
        {
            return await _roleDb.Roles.FirstOrDefaultAsync(r => r.Id == id);
        }
    }
}
