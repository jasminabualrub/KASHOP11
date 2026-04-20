using KASHOP11.BLL.Service;
using KASHOP11.DAL.DTO.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace KASHOP11.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CheckoutsController : ControllerBase
    {
        private readonly ICheckoutService _CheckoutService;
        public CheckoutsController(ICheckoutService checkoutService)
        {
            _CheckoutService = checkoutService;
        }
        [HttpPost("")]
        public async Task<IActionResult> Payment([FromBody]CheckoutRequest req)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var response = await _CheckoutService.ProcessCheckout(userId,req);
            if ((bool)!response.Success) return BadRequest();
            return Ok(response);
        }
    }
}
