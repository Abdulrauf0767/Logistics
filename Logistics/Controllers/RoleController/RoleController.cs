using Logistics.Application.Features.Roles.Command;
using Logistics.Application.Requests.Roles.CreateRoleRequest;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static Logistics.Application.Features.Roles.Queries.GetAllRoles.GetAllRolesCommand;
using static Logistics.Application.Features.Roles.Queries.GetRoleById.GetRoleByIdCommand;

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
        [HttpGet]
        public async Task<IActionResult> GetAllRoles(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(
                new GetAllRolesQuery(),
                cancellationToken);

            return Ok(result);
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetRoleById( int id,CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(
                new GetRoleByIdQuery(id),
                cancellationToken);

            return Ok(result);
        }
    }
}
