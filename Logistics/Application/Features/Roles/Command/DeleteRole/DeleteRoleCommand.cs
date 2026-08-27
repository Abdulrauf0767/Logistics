using MediatR;

namespace Logistics.Application.Features.Roles.Command.DeleteRole
{
    public record DeleteRoleCommand
    (int id) : IRequest<bool>;
    
}
