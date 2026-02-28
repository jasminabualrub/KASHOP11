using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP11.DAL.Models
{
   public class CategoryTranslation
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Language { get; set; }
        public Category Category { get; set; }
        public int CategoryId { get; set; }


    }
}
