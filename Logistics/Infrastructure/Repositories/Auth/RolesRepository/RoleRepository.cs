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
        public async Task<AllRolesResponse?> GetRoleByIdReadOnly(int RoleId)
        {
            return await _dbContext.Roles.AsNoTracking().Select(r => new AllRolesResponse
            {
                Id = r.Id,
                RoleName = r.Name ?? "",
                IsActive = r.IsActive,
                CreatedAt = r.CreatedAt,
                UpdatedAt = r.UpdatedAt,
                Permissions = r.RoleClaims.Select(rc => rc.ClaimValue ?? "").ToList()
            }).FirstOrDefaultAsync(r => r.Id == RoleId); ;
        }
        public async Task <List<AllRolesResponse>> GetAllRolesAsync ()
        {
            return await _dbContext.Roles
        .AsNoTracking()
        .Select(r => new AllRolesResponse
        {
            Id = r.Id,
            RoleName = r.Name ?? "",
            IsActive = r.IsActive,
            CreatedAt = r.CreatedAt,
            UpdatedAt = r.UpdatedAt,
            Permissions = r.RoleClaims.Select(rc => rc.ClaimValue ?? "").ToList()
        })
        .OrderByDescending(r => r.CreatedAt)
        .ToListAsync();
        }
    }
}
