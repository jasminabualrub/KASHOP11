using KASHOP11.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP11.DAL.Repository
{
  public interface ICategoryRepository
    {
     Task  < List<Category>> GetAllAsync();

 Task <Category> CreateAsync(Category category);
    }
}
