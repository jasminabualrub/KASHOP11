using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP11.DAL.DTO.Request
{
    public class AddReviewRequest
    {
        public int ProductId { set; get; }
        public string Comment { set; get; }
        [Range(1,5)]
        public int Rate { set; get; }

    }
}
