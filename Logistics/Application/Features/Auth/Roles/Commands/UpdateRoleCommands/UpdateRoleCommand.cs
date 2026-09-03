using MediatR;

namespace Logistics.Application.Features.Auth.Roles.Commands.UpdateRoleCommands
{
    public record UpdateRoleCommand
    (
        int RoleId,
        string RoleName,
        bool IsActive,
        List<string> Permissions
        ) : IRequest<bool>;
}
