using Logistics.Domain.Entities.Auth.RolesClaim;
using Logistics.Domain.Interfaces.Auth.RoleClaimInterface;
using Logistics.Infrastructure.Persistance.ApplicationDbContext;
using Microsoft.EntityFrameworkCore;

namespace Logistics.Infrastructure.Repositories.Auth.RoleClaimsRepository
{
    public class RoleClaimRepository : IRoleClaimRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public RoleClaimRepository(ApplicationDbContext dbContext) { 
            _dbContext = dbContext;
        }
        public async Task<List<RoleClaim>> GetClaimsByRoleIdAsync (int RoleId)
        {
            return await _dbContext.RoleClaims.AsNoTracking().Where(c => c.RoleId == RoleId).ToListAsync();
        }
        public async Task AddClaimsAsync (IEnumerable<RoleClaim> roleClaim)
        {
            await _dbContext.RoleClaims.AddRangeAsync(roleClaim);
        }
        public void RemoveClaims (IEnumerable<RoleClaim> roleClaims)
        {
             _dbContext.RoleClaims.RemoveRange(roleClaims);
        }
    }
}
