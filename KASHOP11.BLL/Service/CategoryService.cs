using KASHOP11.DAL.DTO.Request;
using KASHOP11.DAL.DTO.Response;
using KASHOP11.DAL.Models;
using KASHOP11.DAL.Repository;
using Mapster;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP11.BLL.Service
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CategoryService(ICategoryRepository categoryRepository, IHttpContextAccessor httpContextAccessor)
        {
            _categoryRepository = categoryRepository;
            _httpContextAccessor = httpContextAccessor;

        }

        //public async Task<List<CategoryResponse>> GetAllCategories()
        //{
        //    // جلب كل الـ categories مع الترجمات
        //    var categories = await _categoryRepository.GetAllAsync(new string[]
        //    {
        //nameof(Category.Translations)
        //    });

        //    // تحويل كل Category إلى CategoryResponse باستخدام Mapster
        //    var response = categories.Adapt<List<CategoryResponse>>();

        //    return response;
        //}
        public async Task<List<CategoryResponse>> GetAllCategories()
        {
            var categories = await _categoryRepository.GetAllAsync(
                 p => p.status == EntityStatus.Active,
                new string[] {
                nameof(Category.Translations),
                nameof(Category.createdBy) });
            foreach (var c in categories)
            {
                Console.WriteLine(c.createdBy == null ? "NULL" : "NOT NULL");
            }
            return categories.Select(c => new CategoryResponse
            {
                categoryId = c.Id,
                User = "Unknown",
                Translations = c.Translations?.Select(t => new CategoryTranslationResponse
                {
                    Name = t.Name,
                    Language = t.Language
                }).ToList() ?? new List<CategoryTranslationResponse>()
            }).ToList();


        }
        public async Task<CategoryResponse> CreateCategory(CategoryRequest req)
        {
            var userId = _httpContextAccessor.HttpContext?.User?
    .FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var userName = _httpContextAccessor.HttpContext?.User?.Identity?.Name;

            var category = req.Adapt<Category>();

            category.createdById = userId;
            category.createdOn = DateTime.UtcNow;

            await _categoryRepository.CreateAsync(category);

            return new CategoryResponse
            {
                categoryId = category.Id,
                User = userName ?? "Unknown",
                Translations = category.Translations.Select(t => new CategoryTranslationResponse
                {
                    Name = t.Name,
                    Language = t.Language
                }).ToList()
            };
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
