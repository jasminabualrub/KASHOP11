using Azure.Core;
using KASHOP11.DAL.Data;
using KASHOP11.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP11.DAL.Repository
{
public class CategoryRepository :ICategoryRepository
    {
        private readonly ApplicationDbContext _context;
        public CategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task < Category> CreateAsync (Category category)
        {
           
             await _context.categories.AddAsync(category);
            _context.SaveChanges();
            return category;
        }

        public async Task<List<Category>> GetAllAsync()
        {
            return await _context.categories.Include(c => c.Translations).ToListAsync(); ;
        }

      
    }
}
