using KASHOP11.BLL.Service;
using KASHOP11.DAL.DTO.Request;
using KASHOP11.PL.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Stripe;
using System.Security.Claims;

namespace KASHOP11.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewService _reviewservice;
        private readonly IStringLocalizer<SharedResources> _localizer;
        public ReviewsController(IReviewService reviewService, IStringLocalizer<SharedResources> localizer)
        {
            _reviewservice = reviewService;
            _localizer = localizer;

        }

        [HttpPost]
        public async Task<IActionResult> AddReview([FromBody] AddReviewRequest request)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
                return Unauthorized();

            var result = await _reviewservice.AddReview(userId, request);

            if (!result)
            {
                return BadRequest();

            }

            return Ok(
                new
                {
                    Message = _localizer["Review added successfully"]
                });
        }
    }
}
