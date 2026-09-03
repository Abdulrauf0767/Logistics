using Logistics.Application.Features.Auth.Roles.Commands.CreateRoleCommands;
using Logistics.Application.Features.Auth.Roles.Commands.UpdateRoleCommands;
using Logistics.Application.Features.Auth.Roles.Queries.GetAllRolesQueries;
using Logistics.Application.Features.Auth.Roles.Queries.GetRoleByIdQueries;
using Logistics.Application.Requests.Auth.Roles;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Logistics.Controllers.Auth.RoleControllers
{
    [ApiController]
    [Route("api/role")]
    public class RoleController : ControllerBase
    {
        private readonly IMediator _mediator;
        public RoleController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost]
        public async Task<IActionResult> CreateRoleAsync([FromBody] CreateRoleRequest request)
        {
            var command = new CreateRoleCommand(request.RoleName,request.PermissionsList);
            await _mediator.Send(command);
            return Ok(new {success = true , message = "Role Created Successfully."});
        }
        [HttpPut]
        public async Task<IActionResult> UpdateRoleAsync([FromBody] UpdateRoleRequest request) {
            var command = new UpdateRoleCommand(request.RoleId,request.RoleName, request.IsActive, request.PermissionsList);
            await _mediator.Send(command);
            return Ok(new { success = true, message = "Role updated successfully!"});
        }
        [HttpGet]
        public async Task<IActionResult> GetAllRolesAsync()
        {
            var result =await _mediator.Send(new GetAllRolesQuery());
            return Ok(new {sucess = true, roles = result});
        }
        [HttpGet("{id:int}")]
        public async Task <IActionResult> GetRoleById([FromRoute] int id)
        {
            var result = await _mediator.Send(new GetRoleByIdQuery(id));
            return Ok(new {success = true, role = result});
        }
    }
}
