namespace CarRental.Services.Basket.DTOs
{
    public class BasketItemDto
    {
        //public int Quantity { get; set; }
        public string UserId { get; set; }
        public string CarId { get; set; }
        public string CarName{ get; set; }
        public decimal Price { get; set; }
        public DateTime RentDate { get; set; }
        public DateTime LeaveDate { get; set; }
        public decimal? TotalPricePaid { get; set; }

    }
}
