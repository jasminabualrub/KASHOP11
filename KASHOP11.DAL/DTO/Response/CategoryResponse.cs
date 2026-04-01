using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP11.DAL.DTO.Response
{
  public class CategoryResponse
    {
        public int categoryId { get; set; }
        public string User { get; set; }
        public List <CategoryTranslationResponse> Translations { get; set; }
        //public string Name { get; set; }
    }
}
