using KASHOP11.BLL.Service;
using KASHOP11.DAL.DTO.Request;
using KASHOP11.PL.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace KASHOP11.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandsController : ControllerBase
    {
        private readonly IBrandService _brandService;
        private readonly IStringLocalizer<SharedResources> _localizer;

        public BrandsController(IBrandService brandService, IStringLocalizer<SharedResources> localizer)
        {
            _brandService = brandService;
            _localizer = localizer;
        }

       
        [HttpGet("")]
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var brands = await _brandService.GetAllBrandsAsync();
            return Ok(new { data = brands, message = _localizer["Success"].Value });
        }

        
        [HttpGet("{id}")]
      
        public async Task<IActionResult> Index(int id)
        {
            var brand = await _brandService.GetBrand(b => b.Id == id);
            if (brand == null) return NotFound(new { message = _localizer["NotFound"].Value });

            return Ok(new { data = brand, message = _localizer["Success"].Value });
        }

       
        [HttpPost("")]
        [Authorize]
        public async Task<IActionResult> Create([FromForm] BrandRequest request)
        {
            if (request == null) return BadRequest(new { message = _localizer["InvalidRequest"].Value });

            await _brandService.CreateBrand(request);
            return Ok(new { message = _localizer["Success"].Value });
        }

       
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _brandService.DeleteBrand(id);
            if (!deleted) return NotFound(new { message = _localizer["NotFound"].Value });

            return Ok(new { message = _localizer["Success"].Value });
        }
    }
}