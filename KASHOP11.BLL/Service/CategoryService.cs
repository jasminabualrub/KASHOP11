using KASHOP11.DAL.DTO.Request;
using KASHOP11.DAL.DTO.Response;
using KASHOP11.DAL.Models;
using KASHOP11.DAL.Repository;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
      

        public async Task<List<CategoryResponse>> GetAllCategories(string lang="en")
        {
            var categories = await _categoryRepository.GetAllAsync(new string[] { nameof(Category.Translations), nameof(Category.createdBy) });
            foreach (var c in categories)
            {
                Console.WriteLine(c.createdBy == null ? "NULL" : "NOT NULL");
            }
            return categories.BuildAdapter().AddParameters("lang",lang).AdaptToType<List<CategoryResponse>>();

           

        }
        public async Task <CategoryResponse> CreateCategory(CategoryRequest req)
        {
            var category = req.Adapt<Category>();
            
             await _categoryRepository.CreateAsync(category);
          
            return category.Adapt<CategoryResponse>();
        }

        public async Task <CategoryResponse?> GetCategory (Expression<Func<Category, bool>> filter)
        {
            var category = await _categoryRepository.GetOne(filter, new string[] { nameof(Category.Translations) });
            return category?.Adapt<CategoryResponse>();
        }

        public async Task<bool> DeleteCategory(int id)
        {
            var category = await _categoryRepository.GetOne(c => c.Id == id);
            if (category == null) return false;
            return await _categoryRepository.DeleteAsync(category);
        }

    //    public Task<CategoryResponse> UpdateCategory(int id, CategoryRequest req)
    //    {
    //        var category = await _categoryRepository.GetOne(c => c.Id == id);
    //        if (category == null) return null;
    //        foreach(var item in category.Translations)
    //        {
    //         var updated = req.Translations
    //        .FirstOrDefault(t => t.Language == translation.Language);

    //    if (updated != null)
    //    {
    //        translation.Name = updated.Name;
    //    }
    //}
    //    }
    }
}
