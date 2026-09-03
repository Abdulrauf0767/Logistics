using Logistics.Domain.Entities.Auth.RolesClaim;
using Logistics.Domain.Entities.Auth.RolesEntity;
using Logistics.Domain.Entities.PermissionsEntities;
using Logistics.Domain.Interfaces.Auth.RoleClaimInterface;
using Logistics.Domain.Interfaces.Auth.RoleInterface;
using Logistics.Domain.Interfaces.UnitOfWorkInterface;
using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Logistics.Application.Features.Auth.Roles.Commands.UpdateRoleCommands
{
    public class UpdateRoleCommandHandler : IRequestHandler<UpdateRoleCommand,bool>
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IRoleClaimRepository _claimRepository;
        private readonly IUnitOfWorkRepository _unitOfWorkRepository;
        public UpdateRoleCommandHandler(IRoleClaimRepository roleClaimRepository , IRoleRepository roleRepository , IUnitOfWorkRepository unitOfWorkRepository)
        {
            _unitOfWorkRepository = unitOfWorkRepository;
            _roleRepository = roleRepository;
            _claimRepository = roleClaimRepository;
        }
        public async Task<bool> Handle (UpdateRoleCommand request,CancellationToken cancellationToken)
        {
            var existedRole = await _roleRepository.ExistsRoleById(request.RoleId);
            if (!existedRole)
            {
                throw new BadHttpRequestException("Invalid role or role not found!");
            }
           var realRoleName = await _roleRepository.GetRoleNameById(request.RoleId);
            if (realRoleName == null)
            {
                throw new BadHttpRequestException("Role not found");
            }
            if (realRoleName.Equals("Super Admin", StringComparison.OrdinalIgnoreCase) ||
                realRoleName.Equals("Admin", StringComparison.OrdinalIgnoreCase))
            {
                throw new BadHttpRequestException("This system-defined role cannot be modified or updated!");
            }
            var existingPermissions = Permission.GetAllPermissions();
            var incomingPermissions = request.Permissions;
            var invalidPermissions = incomingPermissions.Except(existingPermissions).ToList();
            if (invalidPermissions.Any())
            {
                throw new BadHttpRequestException("One or more permissions are invalid");
            }
            await _unitOfWorkRepository.BeginTransactionAsync(cancellationToken);
            try
            {
                var roleEntity = await _roleRepository.GetRoleByIdForUpdate(request.RoleId);
                roleEntity?.Name = request.RoleName.Trim();
                roleEntity?.NormalizedName = request.RoleName.Trim().ToUpper();
                roleEntity?.IsActive = request.IsActive;
                var currentDbClaims = await _claimRepository.GetClaimsByRoleIdAsync(request.RoleId);
                var currentDbValues = currentDbClaims.Select(c => c.ClaimValue).ToList();
                var claimsToDelete = currentDbClaims.Where(dbClaim => !incomingPermissions.Contains(dbClaim.ClaimValue ?? "")).ToList();
                if (claimsToDelete.Any())
                {
                   _claimRepository.RemoveClaims(claimsToDelete);
                }
                var valuesToAdd = incomingPermissions.Except(currentDbValues).ToList();
                var claimsToAdd = valuesToAdd.Select(permissionStr => new RoleClaim
                {
                    RoleId = request.RoleId,
                    ClaimType = "Permission",
                    ClaimValue = permissionStr
                }).ToList();
                if (claimsToAdd.Any())
                {
                    await _claimRepository.AddClaimsAsync(claimsToAdd);
                }
                await _unitOfWorkRepository.SaveChangesAsync(cancellationToken);
                await _unitOfWorkRepository.CommitTransactionAsync(cancellationToken);
                return true;
            }
            catch(DbUpdateException ex)
            {
                await _unitOfWorkRepository.RollbackTransactionAsync(cancellationToken);

                if (ex.InnerException is Microsoft.Data.SqlClient.SqlException sqlEx)
                {
                    if (sqlEx.Number == 2627 || sqlEx.Number == 2601)
                    {
                        throw new BadHttpRequestException("This name or record already exists. Duplication is not allowed!");
                    }
                }

                throw;
            }
        }
    }
}
