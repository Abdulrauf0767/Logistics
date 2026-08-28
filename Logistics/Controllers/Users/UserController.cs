using Logistics.Application.Features.Users.Command.CreateUserCommand;
using Logistics.Application.Requests.Users.CreateUserRequest;
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
    }
}
