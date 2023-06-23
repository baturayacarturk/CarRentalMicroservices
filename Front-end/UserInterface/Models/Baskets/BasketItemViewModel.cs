using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace UserInterface.Models.Baskets
{
    public class BasketItemViewModel
    {
        //public int Quantity { get; set; } = 1;
        public string? UserId { get; set; }
        public string CarId { get; set; }
        public string CarName { get; set; }
        public decimal Price { get; set; }
        public DateTime RentDate { get; set; }
        public DateTime LeaveDate { get; set; }
        public decimal? TotalPricePaid { get; set; }
        private decimal? DiscountPrice { get; set; }

        public decimal CurrentPrice
        {
            get => DiscountPrice != null ? DiscountPrice.Value : Price;
        }
        public void ApplyDiscount(decimal discountPrice)
        {
            DiscountPrice = discountPrice;
        }
    }
}
