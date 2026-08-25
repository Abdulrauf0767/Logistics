using Logistics.Application.Requests.Roles.GetAllRoles;
using MediatR;

namespace Logistics.Application.Features.Roles.Queries.GetRoleById
{
    public class GetRoleByIdCommand
    {
        public record GetRoleByIdQuery(int RoleId) : IRequest<GetAllRolesResponse>;
    }
}
