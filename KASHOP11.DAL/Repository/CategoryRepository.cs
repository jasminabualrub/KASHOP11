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
public class CategoryRepository :GenericRepository<Category>, ICategoryRepository
    {

        public CategoryRepository(ApplicationDbContext context) : base(context) { }

      
    }
}
