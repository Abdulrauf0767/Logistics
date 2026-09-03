using Logistics.Application.Requests.Auth.Roles;
using Logistics.Domain.Interfaces.Auth.RoleInterface;
using MediatR;

namespace Logistics.Application.Features.Auth.Roles.Queries.GetRoleByIdQueries
{
    public class GetRoleByIdQueryHandler : IRequestHandler<GetRoleByIdQuery,AllRolesResponse?>
    {
        private readonly IRoleRepository _roleRepository;
        public GetRoleByIdQueryHandler(IRoleRepository roleRepository) { 
            _roleRepository = roleRepository;
        }
        public async Task<AllRolesResponse?> Handle(GetRoleByIdQuery request,CancellationToken cancellation) {
            var existsRole = await _roleRepository.ExistsRoleById(request.RoleId);
            if (!existsRole)
            {
                throw new BadHttpRequestException("Invalid role or not found!");
            }
            return await _roleRepository.GetRoleByIdReadOnly(request.RoleId);
        }
    }
}
