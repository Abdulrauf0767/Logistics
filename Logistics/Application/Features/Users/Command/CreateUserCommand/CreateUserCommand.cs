using MediatR;

namespace Logistics.Application.Features.Users.Command.CreateUserCommand
{
    public record CreateUserCommand
    (
       string PhoneNumber,
       string Password,
       int RoleId
     ) : IRequest<int>;
}
