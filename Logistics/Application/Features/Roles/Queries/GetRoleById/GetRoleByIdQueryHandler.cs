using Logistics.Application.Requests.Roles.GetAllRoles;
using Logistics.Infrastructure.Persistance.ApplicationDbContext;
using MediatR;
using Microsoft.EntityFrameworkCore;
using static Logistics.Application.Features.Roles.Queries.GetRoleById.GetRoleByIdCommand;

namespace Logistics.Application.Features.Roles.Queries.GetRoleById
{
    public class GetRoleByIdQueryHandler : IRequestHandler<GetRoleByIdQuery ,GetAllRolesResponse>
    {
        private readonly ApplicationDbContext _dbContext;
        public GetRoleByIdQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<GetAllRolesResponse> Handle(
        GetRoleByIdQuery request,
        CancellationToken cancellationToken)
        {
            var role = await _dbContext.Roles
                .AsNoTracking()
                .Where(role => role.Id == request.RoleId)
                .Select(role => new GetAllRolesResponse
                {
                    RoleId = role.Id,
                    RoleName = role.Name,
                    RoleDescription = role.Description,
                    IsActive = role.IsActive,

                    CreatedAt = EF.Property<DateTime>(
                        role,
                        "CreatedAt"),

                    UpdatedAt = EF.Property<DateTime>(
                        role,
                        "UpdatedAt"),

                    MappedPermission = (
                        from rp in _dbContext.RolePermissions
                        join permission in _dbContext.Permissions
                            on rp.PermissionId equals permission.Id
                        where rp.RoleId == role.Id
                        select new MapperPermissionDto(
                            permission.Name,
                            permission.Id
                        )
                    ).ToList()
                })
                .FirstOrDefaultAsync(cancellationToken);

                if (role == null)
                {
                    throw new KeyNotFoundException(
                        $"Role with ID {request.RoleId} was not found.");
                }

                return role;
            }
    }
}
