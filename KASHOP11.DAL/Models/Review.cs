using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP11.DAL.Models
{
    public  class Review
    {
      public  int Id { set; get; }
      public string UserId { set; get; }
      public ApplicationUser User { get; set; }
      public int ProductId { get; set; }
      public Product Product { get; set; }
      public string Comment { get; set; }
        public int Rate { set; get; }
        public DateTime CreatedAt { set; get; } = DateTime.UtcNow;



    }
}
