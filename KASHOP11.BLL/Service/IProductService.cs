using KASHOP11.DAL.DTO.Request;
using KASHOP11.DAL.DTO.Response;
using KASHOP11.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP11.BLL.Service
{
    public interface IProductService
    {
      public  Task CreateProduct(ProductRequest request);
       public Task<List<ProductResponse>> GetAllProductsAsync();
        public Task<ProductResponse?> GetProduct(Expression<Func<Product, bool>> filter);
        public Task<bool> DeleteProduct(int id);
        public Task<bool> UpdateProduct(int id, ProductUpdateRequest req);
    }
}
