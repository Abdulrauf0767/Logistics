using Logistics.Application.Requests.Auth.Roles;
using MediatR;

namespace Logistics.Application.Features.Auth.Roles.Queries.GetRoleByIdQueries
{
    public record GetRoleByIdQuery(int RoleId) : IRequest<GetRoleByIdResponse?>;
}
