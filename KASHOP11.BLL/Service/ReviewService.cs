using KASHOP11.DAL.DTO.Request;
using KASHOP11.DAL.Models;
using KASHOP11.DAL.Repository;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP11.BLL.Service
{
    public class ReviewService : IReviewService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IReviewRepository _reviewRepository;

        public ReviewService(IOrderRepository orderRepository,IReviewRepository reviewRepository)
        {
            _orderRepository = orderRepository;
          _reviewRepository = reviewRepository;
        }
        public async Task<bool> AddReview(string userId, AddReviewRequest request)
        {
            var purchased = await _orderRepository.GetOne(
                filter: o => o.UserId == userId
            && o.OrderStatus == DAL.Models.OrderStatusEnum.Delivered &&
            o.OrderItems.Any(oi => oi.ProductId == request.ProductId),
                includes: new[]
                { nameof(Order.OrderItems)

                });
            if (purchased == null) return false;
            var alreadyReview = await _reviewRepository.GetOne(
                r => r.UserId == userId && r.ProductId == request.ProductId

                );
            if (alreadyReview != null) return false;
            var review = request.Adapt<Review>();
            review.UserId = userId;
            await _reviewRepository.CreateAsync(review);
            return true;
        }
    }
}
