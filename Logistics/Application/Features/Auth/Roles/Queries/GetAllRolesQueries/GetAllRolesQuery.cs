using Logistics.Application.Requests.Auth.Roles;
using Logistics.Domain.Entities.Auth.RolesEntity;
using MediatR;

namespace Logistics.Application.Features.Auth.Roles.Queries.GetAllRolesQueries
{
    public record GetAllRolesQuery
    () : IRequest<List<AllRolesResponse>>;
}
