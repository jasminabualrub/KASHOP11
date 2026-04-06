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
    public interface IBrandService
    {
       public Task<List<BrandResponse>> GetAllBrandsAsync();
       public Task<BrandResponse?> GetBrand(Expression<Func<Brand, bool>> filter);
        public Task CreateBrand(BrandRequest request);
        public Task<bool> DeleteBrand(int id);
    }
}
