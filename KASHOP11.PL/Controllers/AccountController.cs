using KASHOP11.BLL.Service;
using KASHOP11.DAL.DTO.Request;
using KASHOP11.DAL.DTO.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

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
        public async Task<IActionResult> Register(RegisterRequest req)
        {
            var result = await _authenticationservice.RegisterAsync(req);
            return Ok(result);
        }
        [HttpPost("login")]
        public async Task <IActionResult>Login(LoginRequest req)
        {
            var result = await _authenticationservice.LoginAsync(req);
            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }
        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string token, string userId)
        {
            var isconfirmed = await _authenticationservice.ConfirmedEmailAsync(token, userId);
            if (isconfirmed) return Ok(new { message = "ok" });

            return BadRequest();
        }

        [HttpPost("SendCode")]
        public async Task <IActionResult>RequestPasswordReset(ForgotPasswordRequest req)
        {
            var result = await _authenticationservice.RequestPasswordReset(req);
            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }
        [HttpPost("ResetPassword")]
        public async Task<IActionResult> PasswordAsync(ResetPasswordRequest req)
        {
            var result = await _authenticationservice.ResetPasswordAsync(req);
            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }

    }
}
