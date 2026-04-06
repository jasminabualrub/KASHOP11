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
        [HttpGet("{id}")]
        //[Authorize]
        public async Task<IActionResult> Index(int id)
        {


            var product = await _productservice.GetProduct(p => p.Id == id);
            if (product == null) return NotFound();
            return Ok(new { data = product, _localizer["Success"].Value });
        }
        [HttpPost("")]
        [Authorize]
        public async Task<IActionResult> create([FromForm] ProductRequest request)
        {
            await _productservice.CreateProduct(request);
            return Ok();

        }
        [HttpPatch("{id}")]
        [Authorize]
        public async Task<IActionResult>Update(int id,[FromForm] ProductUpdateRequest req)
        {
            var updated = await _productservice.UpdateProduct(id, req);
            if (!updated) return BadRequest();
            return Ok();

        }
        [HttpPatch("{id}/status")]
        [Authorize]
        public async Task<IActionResult> ChangeStatus(int id)
        {
            var updated = await _productservice.ToggleStatus(id);
            if (!updated) return BadRequest();
            return Ok();

        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _productservice.DeleteProduct(id);
            if (!deleted) return BadRequest();
            return Ok();
        }

    }
}
