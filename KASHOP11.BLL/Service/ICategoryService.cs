using KASHOP11.DAL.DTO.Request;
using KASHOP11.DAL.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP11.BLL.Service
{
   public interface ICategoryService
    {
       Task< List<CategoryResponse> >GetAllCategories();
      Task <CategoryResponse> CreateCategory(CategoryRequest request);
    }
}
