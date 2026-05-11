using KASHOP11.BLL.Service;
using KASHOP11.PL.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Security.Claims;

namespace KASHOP11.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderservice;
        private readonly IStringLocalizer<SharedResources> _localizer;
        public OrdersController(IOrderService orderservice, IStringLocalizer<SharedResources> localizer)
        {
            _orderservice = orderservice;
            _localizer = localizer;

        }
        [HttpGet("")]
        public async Task <IActionResult> GetMyOrders()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var orders = await _orderservice.GetUserOrders(userId);
            Console.WriteLine(userId);
            return Ok(new {data = orders});
        }
    }
}
