using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP11.DAL.Models
{
    public class ProductImage
    {
        public int Id {get;set;}
        public string ImagePath { get; set; }
        public int ProductId { set; get; }
        public Product Product { get; set; }

        
    }
}
