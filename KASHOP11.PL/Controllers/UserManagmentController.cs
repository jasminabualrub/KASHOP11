using KASHOP11.BLL.Service;
using KASHOP11.DAL.DTO.Request;
using KASHOP11.PL.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace KASHOP11.PL.Controllers
{
    [Route("api/admin")]
    [ApiController]
    [Authorize]
    public class UserManagmentController : ControllerBase
    {
        private readonly IUserManagerService _usermanagerservice;
        private readonly IStringLocalizer<SharedResources> _localizer;
        public UserManagmentController(IUserManagerService usermanagerservice, 
            IStringLocalizer<SharedResources> localizer)
        {
            _usermanagerservice=usermanagerservice;
            _localizer = localizer;

        }
        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _usermanagerservice.GetAllUsers();
            return Ok(new {users});
        }
        [HttpGet("users/{userId}")]
        public async Task<IActionResult> GetUser([FromRoute]string userId)
        {
            var users = await _usermanagerservice.GetUser(userId);
           
            return Ok(new { users });
        }
        [HttpPatch("{userId}/role")]
        public async Task<IActionResult> ChangeRole(string userId, [FromBody] ChangeRoleRequest request)
        {
            var result = await _usermanagerservice.ChangeRole(userId,request.newRole);
            if (!result) return BadRequest();
            return Ok();
        }
        [HttpPatch("{userId}/toggle-block")]
        public async Task<IActionResult> ToggleBlock(string userId)
        {
            var result = await _usermanagerservice.ToggleBlockUser(userId);
            if (!result) return BadRequest();
            return Ok();
        }
    }
}
