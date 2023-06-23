using CarRental.Common.Messages;
using System.Net;

namespace UserInterface.Models.Orders
{
    public class OrderModel
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        
        public string RenterId { get; set; }
        
        public List<OrderItemCreateInput> OrderItems { get; set; }
    }
}
