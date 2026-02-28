using KASHOP11.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP11.DAL.Data
{
    public class ApplicationDbContext : DbContext
    { public DbSet <Category> categories { set; get; }
        public DbSet<CategoryTranslation> categoryTranslations { set; get; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}
