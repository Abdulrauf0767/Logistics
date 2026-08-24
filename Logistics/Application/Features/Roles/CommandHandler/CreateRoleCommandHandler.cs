using Logistics.Application.Features.Roles.Command;
using Logistics.Domain.Entities;
using Logistics.Domain.Entities.RolePermissionsEntity;
using Logistics.Domain.Interfaces.PermissionsInterface;
using Logistics.Domain.Interfaces.RoleInterface;
using Logistics.Domain.Interfaces.RolePermissionInterface;
using Logistics.Domain.Interfaces.UnitOfWorkInterface;
using Logistics.Infrastructure.Repositories.RolePermissionRepository;
using MediatR;
using Microsoft.AspNetCore.Http; 
using HttpBadHttpRequestException = Microsoft.AspNetCore.Http.BadHttpRequestException;

namespace Logistics.Application.Features.Roles.CommandHandler
{
    public class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand,int>
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IRolePermissionRepository _rolePermissionRepository;
        private readonly IPermissionRepository _permissionRepository;
        private readonly IUnitOfWorkRepository _unitOfWorkRepository;
        public CreateRoleCommandHandler(IRolePermissionRepository rolePermission,IRoleRepository roleRepository,IUnitOfWorkRepository unitOfWorkRepository,IPermissionRepository permissionRepository)
        {
            _permissionRepository = permissionRepository;
            _rolePermissionRepository = rolePermission;
            _roleRepository = roleRepository;
            _unitOfWorkRepository = unitOfWorkRepository;
        }
        public async Task<int> Handle(CreateRoleCommand request , CancellationToken cancellationToken)
        {
            var roleExists = await _roleRepository.ExistsByName(request.RoleName);
            if (roleExists)
                throw new BadHttpRequestException($"Role '{request.RoleName}' already exists.");

            var uniquePermissionIds = request.PermissionIds.Distinct().ToList();
            var validCount = await _permissionRepository.GetCountByIdAsync(uniquePermissionIds);

            if (validCount != uniquePermissionIds.Count)
            {
                throw new BadHttpRequestException("One or more provided Permission IDs do not exist in the database.");
            }

            var newRole = new RoleEntity(request.RoleName, request.Description ?? "");
            await _roleRepository.AddRoleAsync(newRole);
            await _unitOfWorkRepository.SaveChangesAsync();

            foreach (var permissionId in uniquePermissionIds)
            {
                var junction = new RolePermissionEntity(newRole.Id, permissionId);
                await _rolePermissionRepository.AddRolePermissionsAsync(junction);
            }
            await _unitOfWorkRepository.SaveChangesAsync();

            return newRole.Id;
        }
    }
}
