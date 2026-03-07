using KASHOP11.BLL.Service;
using KASHOP11.DAL.DTO.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KASHOP11.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationservice;
        public AccountController(IAuthenticationService authenticationservice)
        {
            _authenticationservice = authenticationservice;
        }
        [HttpPost("register")]
        public async Task <IActionResult> Register (RegisterRequest req)
        {
            var result = await _authenticationservice.RegisterAsync(req);
            return Ok(result);
        }
    }
}
