using Logistics.Application.Requests.Auth.Roles;
using Logistics.Domain.Entities.Auth.RolesClaim;
using Logistics.Domain.Entities.Auth.RolesEntity;
using Logistics.Domain.Interfaces.Auth.RoleInterface;
using Logistics.Infrastructure.Persistance.ApplicationDbContext;
using Microsoft.EntityFrameworkCore;
using System.Numerics;

namespace Logistics.Infrastructure.Repositories.Auth.RolesRepository
{
    public class RoleRepository : IRoleRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public RoleRepository(ApplicationDbContext dbContext) { 
            _dbContext = dbContext;
        }
        public async Task<bool> ExistsRoleByName(string roleName) {
            return await _dbContext.Roles.AsNoTracking().AnyAsync(r => r.Name!.ToLower() == roleName.Trim().ToLower());
        }
        public async Task CreateRoleAsync (Role role)
        {
            await _dbContext.Roles.AddAsync(role);
        }
        public async Task AddPermissionToRoleAsync (RoleClaim role)
        {
            await _dbContext.RoleClaims.AddAsync(role);
        }
        public async Task<bool> ExistsRoleById (int RoleId)
        {
           return await _dbContext.Roles.AsNoTracking().AnyAsync (r => r.Id == RoleId);
        }
        public async Task<string?> GetRoleNameById (int RoleId)
        {
            return await _dbContext.Roles.AsNoTracking().Where(r => r.Id == RoleId).Select(r => r.Name).FirstOrDefaultAsync();
        }
        public async Task<Role?> GetRoleByIdForUpdate (int RoleId)
        {
            return await _dbContext.Roles.Include(r => r.RoleClaims).FirstOrDefaultAsync(r => r.Id == RoleId);
        }
        public async Task<GetRoleByIdResponse?> GetRoleByIdReadOnly(int RoleId)
        {
            return await _dbContext.Roles.AsNoTracking()
                .Where(r => r.Id == RoleId && r.Name != "Super Admin") 
                .Select(r => new GetRoleByIdResponse
                {
                    Id = r.Id,
                    RoleName = r.Name ?? "",
                    IsActive = r.IsActive,
                    CreatedAt = r.CreatedAt,
                    UpdatedAt = r.UpdatedAt,
                    Permissions = r.RoleClaims.Select(rc => rc.ClaimValue ?? "").ToList()
                })
                .FirstOrDefaultAsync();
        }

        public async Task<List<AllRolesResponse>> GetAllRolesAsync(int RoleId, int pageSize)
        {
            if (pageSize < 1) pageSize = 10;

            return await _dbContext.Roles.AsNoTracking()
                .Where(r => r.Id > RoleId && r.Name != "Super Admin")
                .OrderBy(r => r.Id)
                .Take(pageSize) 
                .Select(r => new AllRolesResponse
                {
                    Id = r.Id,
                    RoleName = r.Name ?? "",
                    IsActive = r.IsActive,
                    CreatedAt = r.CreatedAt,
                    UpdatedAt = r.UpdatedAt,
                    Permissions = r.RoleClaims.Select(rc => rc.ClaimValue ?? "").ToList()
                })
                .ToListAsync();
        }

    }
}
