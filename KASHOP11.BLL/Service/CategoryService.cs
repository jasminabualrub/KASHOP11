using KASHOP11.DAL.DTO.Request;
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
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
      

        public async Task<List<CategoryResponse>> GetAllCategories()
        {
            var categories = await _categoryRepository.GetAllAsync();
            return categories.Adapt<List<CategoryResponse>>();
        }
        public async Task <CategoryResponse> CreateCategory(CategoryRequest req)
        {
            var category = req.Adapt<Category>();
             await _categoryRepository.CreateAsync(category);
            return category.Adapt<CategoryResponse>();
        }
    }
}
