using MediatR;

namespace Logistics.Application.Features.Auth.Roles.Commands.CreateRoleCommands
{
    public record CreateRoleCommand
    (
        string RoleName,
        List<string> Permissions
        ) : IRequest<bool>;
}
