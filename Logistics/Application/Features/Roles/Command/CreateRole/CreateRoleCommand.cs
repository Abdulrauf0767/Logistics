using MediatR;
namespace Logistics.Application.Features.Roles.Command
{
    public record CreateRoleCommand
    (
       string RoleName,
       string? Description ,
       List<int> PermissionIds
        ) : IRequest<int>; 
    
}
