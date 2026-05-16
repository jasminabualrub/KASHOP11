using KASHOP11.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace KASHOP11.DAL.DTO.Request
{
    public class ChangeOrderStatusRequest
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public OrderStatusEnum status { set; get; }
    }
}
