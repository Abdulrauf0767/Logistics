using Logistics.Infrastructure.Persistance.ApplicationDbContext;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static Logistics.Application.Features.Roles.Queries.GetAllRoles.GetAllRolesCommand;

namespace Logistics.Application.Requests.Roles.GetAllRoles
{
    public class GetAllRolesQueryHandler : IRequestHandler<GetAllRolesQuery, List<GetAllRolesResponse>>
    {
        private readonly ApplicationDbContext _context; 

        // Constructor Injection
        public GetAllRolesQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<GetAllRolesResponse>> Handle(
                                GetAllRolesQuery request,
                                CancellationToken cancellationToken)
        {
            var roles = await _context.Roles
                                .AsNoTracking()
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
                                        from rp in _context.RolePermissions
                                        join permission in _context.Permissions
                                            on rp.PermissionId equals permission.Id
                                        where rp.RoleId == role.Id
                                        select new MapperPermissionDto(
                                            permission.Name,
                                            permission.Id
                                        )
                                    ).ToList()
                                })
                                .ToListAsync(cancellationToken);

                                        return roles;
        }
    }
}
