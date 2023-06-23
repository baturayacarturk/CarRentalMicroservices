using CarRental.Services.Order.Domain.Core;

namespace CarRental.Services.Order.Domain.OrderAggregate
{
    public class OrderItem:Entity
    {
        public string ProductId { get;  set; }
        public string ProductName { get;  set; }
        public string PictureUrl { get;  set; }
        public Decimal Price { get;  set; }
        public DateTime LeaveDate { get; set; }

        public OrderItem(string productId, string productName, string pictureUrl, decimal price, DateTime leaveDate)
        {
            ProductId = productId;
            ProductName = productName;
            PictureUrl = pictureUrl;
            Price = price;
            LeaveDate = leaveDate;
        }
        public void UpdateOrderItem(string productName, string pictureUrl, decimal price)
        {
            ProductName=productName;
            PictureUrl=pictureUrl;
            Price = price;
        }
    }
}
