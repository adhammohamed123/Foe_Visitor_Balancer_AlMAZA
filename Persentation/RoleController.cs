using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;


namespace Presentaion
{
    [ApiController]
    [Route("api/Roles")]
    [Authorize]
    public class RoleController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> roleManager;

        public RoleController(RoleManager<IdentityRole> roleManager)
        {
            this.roleManager = roleManager;
        }
        [HttpGet]
        public IActionResult GetAllRoles()
        {
            var roles = roleManager.Roles.ToList();
            if (roles == null || !roles.Any())
            {
                return NotFound(new { message = "No roles found" });
            }
            return Ok(roles.Select(r => new { r.Id, r.Name }));
        }
    }
}
