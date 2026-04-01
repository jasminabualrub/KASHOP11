using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP11.DAL.DTO.Response
{
public class LoginResponse
    {
        public string Message { get; set; }
        public bool Success { get; set; }
        public string ? AcessToken { get; set; }


    }
}
