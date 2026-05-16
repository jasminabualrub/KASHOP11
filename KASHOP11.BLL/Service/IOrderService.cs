using KASHOP11.DAL.DTO.Request;
using KASHOP11.DAL.DTO.Response;
using KASHOP11.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP11.BLL.Service
{
    public interface IOrderService
    {
        Task<List<OrderResponse>>GetUserOrders(string userId);
        Task<OrderDetailsResponse> GetUserOrder(string userId, int OrderId);
        Task<bool> CancelOrder(string userId, int OrderId);

        //from admin
        Task<List<OrderResponse>> GetAllOrders(OrderStatusEnum status);
        Task<bool> ChangeOrderStatus(int orderId,ChangeOrderStatusRequest request);
        
    }
}
