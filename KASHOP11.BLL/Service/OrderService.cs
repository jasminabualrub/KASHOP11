using KASHOP11.DAL.DTO.Response;
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
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
           _orderRepository = orderRepository;
        }
        public async Task<List<OrderResponse>> GetUserOrders(string userId)
        {
            var orders = await _orderRepository.GetAllAsync(
                filter:o=>o.UserId==userId,
                includes: new[]
                {
                    nameof(Order.OrderItems),
                   $"{nameof(Order.OrderItems)}.{nameof(OrderItem.Product)}" ,
                    $"{nameof(Order.OrderItems)}.{nameof(OrderItem.Product)}"+
                    $".{nameof(Product.Translations)}"

                }
                );
            return orders.Adapt<List<OrderResponse>>();
        }
       
    }
}
