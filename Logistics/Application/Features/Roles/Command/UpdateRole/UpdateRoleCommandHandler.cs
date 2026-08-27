using Logistics.Domain.Interfaces.Roles.PermissionsInterface;
using Logistics.Domain.Interfaces.Roles.RoleInterface;
using Logistics.Domain.Interfaces.Roles.RolePermissionInterface;
using Logistics.Domain.Interfaces.UnitOfWorkInterface;
using Logistics.Application.Features.Roles.Command.UpdateRole;
using MediatR;
using Logistics.Domain.Entities.RolePermissionsEntity;
namespace Logistics.Application.Features.Roles.Command.UpdateRole
{
    public class UpdateRoleCommandHandler : IRequestHandler<UpdateRoleCommand,int>
    {
        private readonly IRolePermissionRepository _rolePermissionRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IUnitOfWorkRepository _unitOfWorkRepository;
        private readonly IPermissionRepository _permissionRepository;
        public UpdateRoleCommandHandler(IRolePermissionRepository rolePermissionRepository,IUnitOfWorkRepository unitOfWorkRepository,IPermissionRepository permissionRepository,IRoleRepository roleRepository)
        {
            _rolePermissionRepository = rolePermissionRepository;
            _roleRepository = roleRepository;
            _unitOfWorkRepository = unitOfWorkRepository;
            _permissionRepository = permissionRepository;
        }
        public async Task<int> Handle (UpdateRoleCommand request ,CancellationToken cancellationToken)
        {
            
            if (request.PermissionIds == null || !request.PermissionIds.Any())
            {
                throw new BadHttpRequestException("Atleast one permission is required");
            }
            var existingRole = await _roleRepository.GetByIdAsync(request.Id);
            if (existingRole == null)
            {
                throw new BadHttpRequestException("invalid role or role not found");
            }
            if (existingRole.Name.Equals("Super Admin" , StringComparison.OrdinalIgnoreCase))
            {
                throw new BadHttpRequestException("The 'Super Admin' role is system-protected and cannot be modified.");
            }
            if (existingRole.Name != request.RoleName)
            {
                var existingName = await _roleRepository.ExistsByName(request.RoleName);
                if (existingName)
                {
                    throw new BadHttpRequestException($"another role with {request.RoleName} already exists.");
                }
            }
            var uniqueIncomingIds = request.PermissionIds.Distinct().ToList();
            var validCount = await _permissionRepository.GetCountByIdAsync(uniqueIncomingIds);
            if (validCount != uniqueIncomingIds.Count)
            {
                throw new BadHttpRequestException("One or more provided Permission IDs do not exist in the database.");
            }
            existingRole.Update(request.RoleName, request.Description ?? "",request.IsActive);
            _roleRepository.UpdateRole(existingRole);
            var currentJunctionEntries = await _rolePermissionRepository.GetByRoleIdAsync(existingRole.Id);
            var entriesToRemove = currentJunctionEntries.Where(cj => !uniqueIncomingIds.Contains(cj.PermissionId)).ToList();
            foreach (var entry in entriesToRemove) {
                _rolePermissionRepository.DeleteRolePermissions(entry);
            }
            var currentPermissionIds = currentJunctionEntries.Select(cj => cj.PermissionId).ToList();
            var idsToAdd = uniqueIncomingIds.Except(currentPermissionIds).ToList();
            foreach (var permissionId in idsToAdd) { 
                var junction = new RolePermissionEntity(existingRole.Id, permissionId);
                await _rolePermissionRepository.AddRolePermissionsAsync(junction);
            }
            await _unitOfWorkRepository.SaveChangesAsync();
            return existingRole.Id;
        }
    }
}
