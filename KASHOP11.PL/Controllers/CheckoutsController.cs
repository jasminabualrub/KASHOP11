using KASHOP11.BLL.Service;
using KASHOP11.DAL.DTO.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace KASHOP11.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CheckoutsController : ControllerBase
    {
        private readonly ICheckoutService _checkoutService;

        public CheckoutsController(ICheckoutService checkoutService)
        {
            _checkoutService = checkoutService;
        }

        [HttpPost("")]
        public async Task<IActionResult> Payment([FromBody] CheckoutRequest req)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new
                {
                    Success = false,
                    Error = "User not authenticated"
                });
            }

            var response = await _checkoutService.ProcessCheckout(userId, req);

            if ((bool)!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpGet("success")]
        [AllowAnonymous]
        public async Task< IActionResult > Success([FromQuery] string sessionId)
        {
            var result = await _checkoutService.HandleSuccess(sessionId);
            return Ok(new { message = "success",sessionId });
        }



    }
}