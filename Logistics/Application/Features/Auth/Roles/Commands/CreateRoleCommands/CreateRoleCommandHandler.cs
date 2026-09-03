using Logistics.Domain.Entities.Auth.RolesClaim;
using Logistics.Domain.Entities.Auth.RolesEntity;
using Logistics.Domain.Entities.PermissionsEntities;
using Logistics.Domain.Interfaces.Auth.RoleClaimInterface;
using Logistics.Domain.Interfaces.Auth.RoleInterface;
using Logistics.Domain.Interfaces.UnitOfWorkInterface;
using MediatR;

namespace Logistics.Application.Features.Auth.Roles.Commands.CreateRoleCommands
{
    public class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand ,bool>
    {
        private readonly IRoleRepository _roleRepository ;
        private readonly IUnitOfWorkRepository _unitOfWorkRepository ;
        private readonly IRoleClaimRepository _roleClaimRepository ;
        public CreateRoleCommandHandler (IRoleRepository roleRepository,IUnitOfWorkRepository unitOfWorkRepository,IRoleClaimRepository roleClaimRepository)
        {
            _roleRepository = roleRepository;
            _unitOfWorkRepository = unitOfWorkRepository;
            _roleClaimRepository = roleClaimRepository;
        }

        public async Task<bool> Handle (CreateRoleCommand request,CancellationToken cancellationToken)
        {
            var roleExists = await _roleRepository.ExistsRoleByName(request.RoleName);
            if (roleExists)
            {
                throw new BadHttpRequestException("This role already exists");
            }
            var existingPermissions = Permission.GetAllPermissions();
            var incomingPermissions = request.Permissions;
            var invalidPermissions = incomingPermissions.Except(existingPermissions).ToList();
            if (invalidPermissions.Any())
            {
                throw new BadHttpRequestException("One or more permissions not exists or invalid!");
            }
            await _unitOfWorkRepository.BeginTransactionAsync(cancellationToken);
            try
            {
                var newRole = new Role
                {
                    Name = request.RoleName.Trim(),
                    NormalizedName = request.RoleName.Trim().ToUpper(),
                    ConcurrencyStamp = Guid.NewGuid().ToString(),
                    IsActive = true,
                };
                await _roleRepository.CreateRoleAsync(newRole);
                await _unitOfWorkRepository.SaveChangesAsync();
                var roleClaimsToAdd = incomingPermissions.Select(permissionStr => new RoleClaim
                {
                    RoleId = newRole.Id,
                    ClaimType = "Permission",
                    ClaimValue = permissionStr
                }).ToList();
                await _roleClaimRepository.AddClaimsAsync(roleClaimsToAdd);
                await _unitOfWorkRepository.CommitTransactionAsync(cancellationToken);
                return true;
            }
            catch { 
                await _unitOfWorkRepository.RollbackTransactionAsync(cancellationToken);
                throw;
            }
        }
    }
}
