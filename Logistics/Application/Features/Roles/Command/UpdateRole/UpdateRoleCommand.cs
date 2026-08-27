using MediatR;

namespace Logistics.Application.Features.Roles.Command.UpdateRole
{
    public record UpdateRoleCommand
    (
        int Id,
        string RoleName,
        string? Description,
        List<int> PermissionIds,
        bool IsActive
        ) : IRequest<int>;
    
}
