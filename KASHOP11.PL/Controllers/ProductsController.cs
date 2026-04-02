using KASHOP11.BLL.Service;
using KASHOP11.DAL.DTO.Request;
using KASHOP11.DAL.Models;
using KASHOP11.PL.Resources;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace KASHOP11.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productservice;
        private readonly IStringLocalizer<SharedResources> _localizer;
        public ProductsController(IProductService productservice, IStringLocalizer<SharedResources> localizer)
        {
            _productservice = productservice;
            _localizer = localizer;

        }
        [HttpGet("")]
        [Authorize]
        public async Task<IActionResult> Index()
        {
            // var lang = Request.Headers["Accept-lang"].ToString();

            var product = await _productservice.GetAllProductsAsync();

            return Ok(new { data = product, _localizer["Success"].Value });
        }
        [HttpPost("")]
        [Authorize]
        public async Task<IActionResult> create([FromForm]ProductRequest request)
        {
            await _productservice.CreateProduct(request);
            return Ok();

        }
        //[HttpDelete("{id}")]
        //[Authorize]
        //public async Task<IActionResult>Delete(int id)
        //{
        //    var deleted = await _productservice.DeleteProduct(id);
        //    if (!deleted) return BadRequest();
        //    return Ok();
        //}
    
    }
}
