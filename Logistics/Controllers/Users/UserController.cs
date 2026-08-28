using Logistics.Application.Features.Users.Command.CreateUserCommand;
using Logistics.Application.Features.Users.Command.LoginUserCommands;
using Logistics.Application.Requests.Users.CreateUserRequest;
using Logistics.Application.Requests.Users.LoginUserRequests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Logistics.Controllers.Users
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;
        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost]
        public async Task<IActionResult> CreateUserAsync([FromBody] CreateUserRequest request)
        {
            var command = new CreateUserCommand(
                            request.PhoneNumber,
                            request.Password,
                            request.RoleId
                );
            var userId = await _mediator.Send( command );
            return Ok(new { success = true, UserId = userId });
        }
        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginUserRequest request)
        {
            var command = new LoginUserCommand(request.PhoneNumber!, request.Password!);
            var result = await _mediator.Send(command);

            Response.Cookies.Append("access_token", result.AccessToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = result.AccessTokenExpiresAt
            });

            return Ok(new { success = true, message = "Login successful" });
        }
    }
}
