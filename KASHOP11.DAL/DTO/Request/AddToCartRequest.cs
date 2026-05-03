using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP11.DAL.DTO.Request
{
    public class AddToCartRequest
    {
        public int ProductId { set; get; }
        
        public int count { set; get; } = 1;
    }
}
