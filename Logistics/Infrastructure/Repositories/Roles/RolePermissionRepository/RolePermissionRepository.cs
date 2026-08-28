using Logistics.Domain.Entities.RolePermissionsEntity;
using Logistics.Domain.Interfaces.Roles.RolePermissionInterface;
using Logistics.Infrastructure.Persistance.ApplicationDbContext;
using Microsoft.EntityFrameworkCore;

namespace Logistics.Infrastructure.Repositories.Roles.RolePermissionRepository
{
    public class RolePermissionRepository : IRolePermissionRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public RolePermissionRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // 1. CHECK ALREADY EXISTS ROLES AND PERMISSIONS
        public async Task<List<int>> GetAlreadyAssignedPermissionIdsAsync(int roleId, List<int> permissionIds)
        {
            return await _dbContext.Set<RolePermissionEntity>()
                .Where(rp => rp.RoleId == roleId && permissionIds.Contains(rp.PermissionId))
                .Select(rp => rp.PermissionId)
                .ToListAsync();
        }

        // 2. CREATE ROLE PERMISSIONS (Abhi iski zaroorat hai tumhein)
        public async Task AddRolePermissionsAsync(RolePermissionEntity rp)
        {
            await _dbContext.Set<RolePermissionEntity>().AddAsync(rp);
        }

        // 3. DELETE ROLE PERMISSIONS (Future mein kaam aayega jab permissions remove karoge)
        public void DeleteRolePermissions(RolePermissionEntity rp)
        {
            _dbContext.Set<RolePermissionEntity>().Remove(rp);
        }

        // 4. UPDATE ROLE PERMISSIONS (Sync logic ke liye)
        public void UpdateRolePermissions(RolePermissionEntity rp)
        {
            _dbContext.Set<RolePermissionEntity>().Update(rp);
        }
        public async Task<List<RolePermissionEntity>> GetByRoleIdAsync(int roleId)
        {
            return await _dbContext.Set<RolePermissionEntity>()
                        .Include(rp => rp.Permission)
                        .Where(rp => rp.RoleId == roleId)
                        .ToListAsync();
        }
    }
}
