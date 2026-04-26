using KASHOP11.DAL.DTO.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP11.DAL.Models
{  public enum OrderStatusEnum { 
    Pending=1,
    Approved=2,
    Shipped=3,
    Delivered=4,
    Cancelled=5,
    Paid=6,

    
    
    }
    public class Order
    {
        public int Id { get; set; }
        public PaymentMethodEnum PaymentMethod { get; set; }
        public DateTime OrderDate { set; get; } = DateTime.UtcNow;
        public DateTime? ShippedDate { set; get; }
        public OrderStatusEnum OrderStatus { set; get; } = OrderStatusEnum.Pending;
        public string? StripeSession { get; set; }
        public decimal ? AmountPaid { set; get; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string PhoneNumber { get; set; }
        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public string? StripeSessionId { get; set; }
    }
}
