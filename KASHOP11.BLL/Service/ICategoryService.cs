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
   public interface ICategoryService
    {
        Task<List<CategoryResponse>> GetAllCategories(string lang = "en");
       Task <CategoryResponse> CreateCategory(CategoryRequest request);
       Task<CategoryResponse?> GetCategory(Expression<Func<Category, bool>> filter);
        Task<bool> DeleteCategory(int id);
        //Task<CategoryResponse> UpdateCategory(int id , CategoryRequest req);
    }
}
