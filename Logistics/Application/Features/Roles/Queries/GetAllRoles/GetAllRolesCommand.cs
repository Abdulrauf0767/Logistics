using Logistics.Application.Requests.Roles.GetAllRoles;
using MediatR;

namespace Logistics.Application.Features.Roles.Queries.GetAllRoles
{
    public class GetAllRolesCommand
    {
        public record GetAllRolesQuery() : IRequest<List<GetAllRolesResponse>>;
    }
}
