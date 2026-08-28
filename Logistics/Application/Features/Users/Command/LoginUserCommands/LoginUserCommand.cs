using MediatR;

namespace Logistics.Application.Features.Users.Command.LoginUserCommands
{
    public record LoginUserCommand
    (
        string PhoneNumber,
        string Password
    ) : IRequest<LoginUserResponse>;

    public record LoginUserResponse
    (
        string AccessToken,
        DateTime AccessTokenExpiresAt
    );
}
