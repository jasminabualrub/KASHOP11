using KASHOP11.DAL.DTO.Response;
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
        
    }
}
