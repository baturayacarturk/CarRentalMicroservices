using CarRental.Services.Basket.DTOs;

namespace CarRental.Services.Basket.Dtos
{
    public class BasketDto
    {

        public string UserId { get; set; }

        public string? DiscountCode { get; set; }

        public int? DiscountRate { get; set; }
        public DateTime RentDate { get; set; }
        public DateTime LeaveDate { get; set; }
        public List<BasketItemDto>? BasketItesm { get; set; }

        public decimal TotalPrice
        {
            get => BasketItesm.Sum(x=>x.Price);
        }
    }
}