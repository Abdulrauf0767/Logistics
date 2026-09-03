using Logistics.Application.Requests.Auth.Roles;
using Logistics.Domain.Entities.Auth.RolesEntity;
using Logistics.Domain.Interfaces.Auth.RoleInterface;
using MediatR;

namespace Logistics.Application.Features.Auth.Roles.Queries.GetAllRolesQueries
{
    public class GetAllRolesQueryHandler : IRequestHandler<GetAllRolesQuery ,List<AllRolesResponse>>
    {
        private readonly IRoleRepository _roleRepository;
        public GetAllRolesQueryHandler(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }
        public async Task<List<AllRolesResponse>> Handle (GetAllRolesQuery request,CancellationToken cancellationToken)
        {
            return await _roleRepository.GetAllRolesAsync();
        }
    }
}
