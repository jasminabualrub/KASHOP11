using KASHOP11.BLL.Service;
using KASHOP11.DAL.Data;
using KASHOP11.DAL.DTO.Request;
using KASHOP11.DAL.DTO.Response;
using KASHOP11.DAL.Models;
using KASHOP11.DAL.Repository;
using KASHOP11.PL.Resources;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Threading.Tasks;

namespace KASHOP11.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryservice;
        private readonly IStringLocalizer<SharedResources> _localizer;
        public CategoriesController(ICategoryService categoryservice, IStringLocalizer<SharedResources> localizer)
        {
            _categoryservice = categoryservice;
            _localizer = localizer;

        }
        [HttpGet("")]
        //[Authorize]
        public async Task<IActionResult> Index()
        {
            var lang = Request.Headers["Accept-lang"].ToString();
            var categories = await _categoryservice.GetAllCategories(lang);

            var response = categories.Adapt<List<CategoryResponse>>();

            _categoryservice.GetAllCategories();

            return Ok(new { data = response, _localizer["Success"].Value });
        }
        [HttpPost("")]
       // [Authorize]
        public async Task<IActionResult> create(CategoryRequest request)
        {
            var category = request.Adapt<Category>();
           /* var user = User.FindFirstValue(ClaimTypes.NameIdentifier);*/
            var response = await _categoryservice.CreateCategory(request);


            return Ok(new { message = _localizer["Success"].Value, response });

        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await _categoryservice.GetCategory(c => c.Id == id));



        }
        [HttpDelete("{id}")]
      //  [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _categoryservice.DeleteCategory(id);
            if (!deleted)
            {
                return NotFound(new { message = _localizer["notfound"].Value });
            }
            return Ok(new { message = _localizer["success"].Value });
        }
        //[HttpPatch("id")]
        ////    //public Task<CategoryResponse> UpdateCategory(int id, CategoryRequest req)
        ////    //{  var 

        //    //}

        //
    } }