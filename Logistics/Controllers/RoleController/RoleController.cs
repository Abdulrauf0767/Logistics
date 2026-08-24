using Logistics.Application.Features.Roles.Command;
using Logistics.Application.Requests.CreateRoleRequest;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Logistics.Controllers.RoleController
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoleController : ControllerBase
    {
        private readonly IMediator _mediator;
        public RoleController (IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost]
        public async Task<IActionResult> CreateRole([FromBody] CreateRoleRequest request)
        {
            try
            {
                var command = new CreateRoleCommand(
                    request.RoleName,
                    request.Description,
                    request.PermissionIds
                );

                var roleId = await _mediator.Send(command);
                return Ok(new { Success = true, RoleId = roleId });
            }
            catch (BadHttpRequestException ex)
            {
                return BadRequest(new
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Validation Error",
                    Detail = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Title = "Internal Server Error",
                    Detail = ex.Message
                });
            }
        }
    }
}
