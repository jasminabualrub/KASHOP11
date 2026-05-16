using KASHOP11.BLL.Service;
using KASHOP11.DAL.DTO.Request;
using KASHOP11.DAL.Models;
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
           
            return Ok(new {data = orders});
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserOrder(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var order = await _orderservice.GetUserOrder(userId,id);
            
            return Ok(new { data = order });
        }

        [HttpGet("admin")]
       // [Authorize(Roles ="Admin")]
        public async Task<IActionResult> GetAllOrders([FromQuery]OrderStatusEnum status=OrderStatusEnum.Pending){

            var orders = await _orderservice.GetAllOrders(status);
            return Ok(new {data=orders });

        }
        [HttpPatch("admin/{id}/status")]
public async Task<IActionResult>ChangeStatus(int id, [FromBody]ChangeOrderStatusRequest status)
        {
            var result = await _orderservice.ChangeOrderStatus(id, status);
            if (!result) return BadRequest();
            return Ok();
        }
    }
}
