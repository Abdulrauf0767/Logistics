using Logistics.Application.DTOS.RoleDTO;
using Logistics.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Logistics.Controllers.RoleController
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoleController : ControllerBase
    {
        private readonly RoleService _roleService;
        public RoleController (RoleService roleService)
        {
            _roleService = roleService;
        }
        // create role method
        [HttpPost]
        public async Task<IActionResult> CreateRole([FromBody] CreateRoleDTO dto)
        {
            try
            {
                await _roleService.CreateRolesAsync (dto);
                return StatusCode(201, new { message = "Role successfully created!" });
            }
            catch (Exception ex) {
                return StatusCode(500, new { message = "Server error", error = ex.Message });
            }
        }
    }
}
