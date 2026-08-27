using Logistics.Application.Features.Roles.Command;
using Logistics.Application.Features.Roles.Command.DeleteRole;
using Logistics.Application.Features.Roles.Command.UpdateRole;
using Logistics.Application.Requests.Roles.CreateRoleRequest;
using Logistics.Application.Requests.Roles.UpdateRoleRequest;
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
                var command = new CreateRoleCommand(
                    request.RoleName,
                    request.Description,
                    request.PermissionIds
                );

                var roleId = await _mediator.Send(command);
                return Ok(new { Success = true, RoleId = roleId });
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
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateRole([FromRoute] int id, [FromBody] UpdateRoleRequest request)
        {
           
                var command = new UpdateRoleCommand(
                    id,
                    request.RoleName,
                    request.Description,
                    request.PermissionIds,
                    request.IsActive
                );

                var updatedRoleId = await _mediator.Send(command);
                return Ok(new { Success = true, RoleId = updatedRoleId, Message = "Role updated successfully." });
        }
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteRole([FromRoute] int id)
        {
                var command = new DeleteRoleCommand(id);
                var result = await _mediator.Send(command);

                return Ok(new { Success = result, Message = "Role deleted successfully." });
            }

    }
}
