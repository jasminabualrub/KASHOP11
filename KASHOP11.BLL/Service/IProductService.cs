using KASHOP11.DAL.DTO.Request;
using KASHOP11.DAL.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP11.BLL.Service
{
    public interface IProductService
    {
      public  Task CreateProduct(ProductRequest request);
       public Task<List<ProductResponse>> GetAllProductsAsync();
    }
}
