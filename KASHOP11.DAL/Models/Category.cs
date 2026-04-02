using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP11.DAL.Models
{
    public class Category:AuditableEntity
    {
        public int Id { get; set; }
       // public string createdBy { get; set; }
        public List<CategoryTranslation> Translations { get; set; }
        public List<Product> products { get; set; }

    }
}
