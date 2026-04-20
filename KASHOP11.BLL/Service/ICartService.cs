using KASHOP11.DAL.DTO.Request;
using KASHOP11.DAL.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP11.BLL.Service
{
    public interface ICartService
    {
        Task <bool>AddToCart(AddToCartRequest request,string UsetId);
        Task<List<CartResponse>> GetCart(string UserId);
        Task<bool> UpdateQuaintity(int productId,int count,string userId);
        Task<bool> RemoveItem(int productId,string userId);
        Task<bool> ClearCart(string userId);

    }
}
