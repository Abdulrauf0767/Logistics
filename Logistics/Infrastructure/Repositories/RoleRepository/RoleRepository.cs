using Logistics.Domain.Entities.RoleEntities;
using Logistics.Domain.Interfaces.RoleInterfaces;
using Logistics.Infrastructure.Persistance.RoleDbContext;
using Microsoft.EntityFrameworkCore;
namespace Logistics.Infrastructure.Repositories.RoleRepository
{
    public class RoleRepository : IRoleRepository
    {
        private readonly RoleDbContext _roleDb;
        public RoleRepository(RoleDbContext roleDb) { 
            _roleDb = roleDb;
        }
        // get all roles from db
        public async Task<IEnumerable<RoleEntity>> GetRolesAsync ()
        {
            return await _roleDb.Roles.AsNoTracking().ToListAsync();
        }
        // get role by id
        public async Task<RoleEntity?> GetRoleById (int id)
        {
           return await _roleDb.Roles.FindAsync (id); 
        }
        // create role 
        public async Task AddRoleAsync (RoleEntity role)
        {
            await _roleDb.Roles.AddAsync(role);  
        }
        // update role
        public void  UpdateRole (RoleEntity role)
        {
          _roleDb.Roles.Update(role);  
        }
        // delete role
        public void DeleteRole(RoleEntity role) {
        _roleDb.Roles.Remove(role);
        }
    }
}
