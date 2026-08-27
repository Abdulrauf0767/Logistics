using Logistics.Application.Features.Roles.Command;
using Logistics.Domain.Entities;
using Logistics.Domain.Entities.RolePermissionsEntity;
using Logistics.Domain.Interfaces.Roles.PermissionsInterface;
using Logistics.Domain.Interfaces.Roles.RoleInterface;
using Logistics.Domain.Interfaces.Roles.RolePermissionInterface;
using Logistics.Domain.Interfaces.UnitOfWorkInterface;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Logistics.Application.Features.Roles.CommandHandler
{
    public class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, int>
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IRolePermissionRepository _rolePermissionRepository;
        private readonly IPermissionRepository _permissionRepository;
        private readonly IUnitOfWorkRepository _unitOfWorkRepository;

        public CreateRoleCommandHandler(
            IRolePermissionRepository rolePermission,
            IRoleRepository roleRepository,
            IUnitOfWorkRepository unitOfWorkRepository,
            IPermissionRepository permissionRepository)
        {
            _permissionRepository = permissionRepository;
            _rolePermissionRepository = rolePermission;
            _roleRepository = roleRepository;
            _unitOfWorkRepository = unitOfWorkRepository;
        }

        public async Task<int> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
        {
            var roleExists = await _roleRepository.ExistsByName(request.RoleName);
            if (roleExists)
                throw new BadHttpRequestException($"Role '{request.RoleName}' already exists.");

            var uniquePermissionIds = request.PermissionIds.Distinct().ToList();
            var validCount = await _permissionRepository.GetCountByIdAsync(uniquePermissionIds);

            if (validCount != uniquePermissionIds.Count)
                throw new BadHttpRequestException("One or more provided Permission IDs do not exist.");
            await _unitOfWorkRepository.BeginTransactionAsync(cancellationToken);

            try
            {
                var newRole = new RoleEntity(request.RoleName, request.Description ?? "");
                await _roleRepository.AddRoleAsync(newRole);
                await _unitOfWorkRepository.SaveChangesAsync(cancellationToken);
                foreach (var permissionId in uniquePermissionIds)
                {
                    var junction = new RolePermissionEntity(newRole.Id, permissionId);
                    await _rolePermissionRepository.AddRolePermissionsAsync(junction);
                }
                await _unitOfWorkRepository.CommitTransactionAsync(cancellationToken);

                return newRole.Id;
            }
            catch
            {
                await _unitOfWorkRepository.RollbackTransactionAsync(cancellationToken);
                throw;
            }
        }
    }
}
