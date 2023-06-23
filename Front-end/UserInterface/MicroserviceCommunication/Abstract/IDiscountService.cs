using UserInterface.Models.Discount;

namespace UserInterface.MicroserviceCommunication.Abstract
{
    public interface IDiscountService
    {
        Task<DiscountViewModel> GetDiscount(string discountCode);
    }
}
