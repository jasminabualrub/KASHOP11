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
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {

        private readonly ApplicationDbContext _context;
        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<T> CreateAsync(T entity)
        {

            await _context.AddAsync(entity);
            _context.SaveChanges();
            return entity;
        }

        public async Task<List<T>> GetAllAsync()
        {
            //.Include(c => c.Translations).
            return await _context.Set<T>().ToListAsync(); ;
        }




    }

}