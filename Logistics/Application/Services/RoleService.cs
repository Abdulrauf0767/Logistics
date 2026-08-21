using Logistics.Application.DTOS.RoleDTO;
using Logistics.Domain.Entities.RoleEntities;
using Logistics.Domain.Interfaces.RoleInterfaces;

namespace Logistics.Application.Services
{
    public class RoleService
    {
        private readonly IRoleRepository _roleRepository;
        public RoleService(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }
        // create role with business logic
        public async Task CreateRolesAsync (CreateRoleDTO dto)
        {
            var exists = await _roleRepository.ExistsByName(dto.RoleName);
            if (exists)
            {
                throw new InvalidOperationException($"Role {dto.RoleName} already exists");  
            }                                                            
            var newRole = new RoleEntity(dto.RoleName, dto.RoleDescription ?? "");
            await _roleRepository.AddRoleAsync(newRole);
            await _roleRepository.SaveChangesAsync();
        }
    }
}
