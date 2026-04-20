using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP11.DAL.DTO.Response
{
    public class CheckoutResponse
    {
        public int OrderId { set; get; }
        public string ?StripeUrl { set; get; }
        public bool? Success { set; get; }
        public string? Error { set; get; }

    }
}
