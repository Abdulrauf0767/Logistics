using Logistics.Domain.Entities.PermissionEntities;
using Logistics.Domain.Interfaces.PermissionsInterface;
using Logistics.Infrastructure.Persistance.ApplicationDbContext;
using Microsoft.EntityFrameworkCore;

namespace Logistics.Infrastructure.Repositories.PermissionRepository
{
    public class PermissionRepository : IPermissionRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public PermissionRepository(ApplicationDbContext dbContext) { 
            _dbContext = dbContext;
        }
        // create permission
        public async Task AddPermissionsAsync(PermissionEntity permission)
        {
            await _dbContext.Permissions.AddAsync(permission);
        }
        // get all permissions 
        public async Task<IEnumerable<PermissionEntity>> GetPermissionsAsync()
        {
            return await _dbContext.Permissions.ToListAsync();
        }
        // check existing names
        public async Task<List<string>> GetExistingNamesAsync()
        {
            return await _dbContext.Permissions
                .Select(p => p.Name)
                .ToListAsync();
        }
        // get single permission
        public async Task<PermissionEntity?> GetPermissionByIdAsync(int id)
        {
            return await _dbContext.Permissions.FindAsync(id);
        }
        // bulk validation to check new permissions while creating
        public async Task<int> GetCountByIdAsync(List<int> ids)
        {
            return await _dbContext.Permissions
                .Where(p => ids.Contains(p.Id))
                .CountAsync();
        }
        // save changes in real db
        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
