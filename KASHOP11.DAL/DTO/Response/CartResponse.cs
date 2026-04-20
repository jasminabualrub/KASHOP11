using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP11.DAL.DTO.Response
{
    public class CartResponse
    {   public int ProductId { set; get; }
        public string ProductName { set; get; }
        public string ProductImage { set; get; }
        public decimal Price { set; get; }
        public decimal Discount { set; get; }
        public int Count { set; get; }
        public decimal Subtotal => Count * (Price -(Price * Discount)/100);
    }
}
