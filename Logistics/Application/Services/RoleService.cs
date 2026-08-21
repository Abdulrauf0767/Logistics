using Logistics.Application.DTOS.RoleDTO;
using Logistics.Domain.Entities.RoleEntities;
using Logistics.Domain.Interfaces.RoleInterfaces;
using Logistics.Infrastructure.Persistance.RoleDbContext;

namespace Logistics.Application.Services
{
    public class RoleService
    {
        private readonly IRoleRepository _roleRepository;
        private readonly RoleDbContext _dbcontext;
        public RoleService(IRoleRepository roleRepository , RoleDbContext dbContext)
        {
            _roleRepository = roleRepository;
            _dbcontext = dbContext;
        }
        // create role with business logic
        public async Task CreateRolesAsync (CreateRoleDTO dto)
        {
            var allRoles = await _roleRepository.GetRolesAsync();
            var exists = allRoles.Any(r => r.RoleName.Equals(dto.RoleName,StringComparison.OrdinalIgnoreCase));
            if (exists)
            {
                throw new InvalidOperationException($"Role {dto.RoleName} already exists");  
            }
            var newRole = new RoleEntity(dto.RoleName, dto.RoleDescription);
            await _roleRepository.AddRoleAsync(newRole);
            await _dbcontext.SaveChangesAsync();
        }
    }
}
