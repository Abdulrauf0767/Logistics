using Logistics.Domain.Entities.Auth.RolesClaim;

namespace Logistics.Domain.Interfaces.Auth.RoleClaimInterface
{
    public interface IRoleClaimRepository
    {
        Task<List<RoleClaim>> GetClaimsByRoleIdAsync(int roleId);
        Task AddClaimsAsync(IEnumerable<RoleClaim> roleClaims);
        void RemoveClaims(IEnumerable<RoleClaim> roleClaims);
    }
}
