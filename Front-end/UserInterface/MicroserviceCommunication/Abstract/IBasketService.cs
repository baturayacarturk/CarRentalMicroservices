using UserInterface.Models.Baskets;

namespace UserInterface.MicroserviceCommunication.Abstract
{
    public interface IBasketService
    {

        public Task<BasketViewModel> GetBasket();

        public Task<bool> DeleteBasket(BasketViewModel basketViewModel);

        public Task AddBasketItem(BasketItemViewModel basketItemViewModel);

        public Task<bool> RemoveBasketItem(string carId);

        public Task<bool> ApplyDiscount(string discountCode);

        public Task<bool> CancelTheDiscount();
    }
}
