using Logistics.Domain.Entities.PermissionEntities;
using Logistics.Domain.Interfaces.Roles.PermissionsInterface;
using Logistics.Infrastructure.Persistance.ApplicationDbContext;
using Microsoft.EntityFrameworkCore;

namespace Logistics.Infrastructure.Repositories.Roles.PermissionRepository
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
        // check existing names
        public async Task<List<string>> GetExistingNamesAsync()
        {
            return await _dbContext.Permissions
                .Select(p => p.Name)
                .ToListAsync();
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
