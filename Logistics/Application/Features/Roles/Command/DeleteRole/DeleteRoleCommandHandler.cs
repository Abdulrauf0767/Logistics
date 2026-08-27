using Logistics.Application.Features.Roles.Command.DeleteRole;
using Logistics.Domain.Interfaces.Roles.RoleInterface;
using Logistics.Domain.Interfaces.Roles.RolePermissionInterface;
using Logistics.Domain.Interfaces.UnitOfWorkInterface;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Logistics.Application.Features.Roles.Command.DeleteRole
{
    public class DeleteRoleCommandHandler : IRequestHandler<DeleteRoleCommand, bool>
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IRolePermissionRepository _rolePermissionRepository;
        private readonly IUnitOfWorkRepository _unitOfWorkRepository;

        public DeleteRoleCommandHandler(
            IRoleRepository roleRepository,
            IRolePermissionRepository rolePermissionRepository,
            IUnitOfWorkRepository unitOfWorkRepository)
        {
            _roleRepository = roleRepository;
            _rolePermissionRepository = rolePermissionRepository;
            _unitOfWorkRepository = unitOfWorkRepository;
        }

        public async Task<bool> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
        {
            var existingRole = await _roleRepository.GetByIdAsync(request.id);
            if (existingRole == null)
            {
                throw new BadHttpRequestException("Role not found or invalid ID.");
            }

            if (existingRole.Name.Equals("Super Admin", StringComparison.OrdinalIgnoreCase))
            {
                throw new BadHttpRequestException("The 'SuperAdmin' role is system-protected and cannot be deleted.");
            }

            var currentJunctionEntries = await _rolePermissionRepository.GetByRoleIdAsync(existingRole.Id);
            foreach (var entry in currentJunctionEntries)
            {
                _rolePermissionRepository.DeleteRolePermissions(entry);
            }

            _roleRepository.DeleteRole(existingRole);

            await _unitOfWorkRepository.SaveChangesAsync();

            return true;
        }
    }
}
