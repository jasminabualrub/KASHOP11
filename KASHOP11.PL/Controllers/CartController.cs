using KASHOP11.BLL.Service;
using KASHOP11.DAL;
using KASHOP11.DAL.DTO.Request;
using KASHOP11.DAL.Models;
using KASHOP11.DAL.Request;
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
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartservice;
        private readonly IStringLocalizer<SharedResources> _localizer;
        public CartController(ICartService cartservice, IStringLocalizer<SharedResources> localizer)
        {
            _cartservice = cartservice;
            _localizer = localizer;

        }
        [HttpPost("")]
       
        public async Task<IActionResult> AddToCart(AddToCartRequest request)
        {

            var UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
           var result=  await _cartservice.AddToCart(request,UserId);
            if (!result) return BadRequest();

            return Ok(new { message = _localizer["Success"].Value });

        }
       [ HttpGet("")]
        public async Task<IActionResult> GetCart()
        {
            var UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var items = await _cartservice.GetCart(UserId);
            return Ok(new { data = items });
        }
        [HttpPatch("{productId}")]
        public async Task<IActionResult> UpdateQuaintity([FromRoute] int productId, [FromBody] UpdateCartRequest requset)
        {
            var UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var updated = await _cartservice.UpdateQuaintity(productId, requset.Count, UserId);
            if (!updated) return BadRequest();
            return Ok();
        }

        [HttpDelete("{productId}")]
        public async Task<IActionResult> Removeitem([FromRoute]int productId)
        {
            var UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var removed = await _cartservice.RemoveItem(productId,UserId);
            if (!removed) return BadRequest();
            return Ok();
        }



    }
}
